/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Acro form
//
//	Author: Uzi Granot
//	Original Version: 1.0
//	Date: April 1, 2013
//	Major rewrite Version: 2.0
//	Date: February 1, 2022
//	Copyright (C) 2013-2022 Uzi Granot. All Rights Reserved
//
//	PdfFileWriter C# class library and TestPdfFileWriter test/demo
//  application are free software. They are distributed under the
//  Code Project Open License (CPOL-1.02).
//
//	The main points of CPOL-1.02 subject to the terms of the License are:
//
//	Source Code and Executable Files can be used in commercial applications;
//	Source Code and Executable Files can be redistributed; and
//	Source Code can be modified to create derivative works.
//	No claim of suitability, guarantee, or any warranty whatsoever is
//	provided. The software is provided "as-is".
//	The Article accompanying the Work may not be distributed or republished
//	without the Author's consent
//
//	The document PdfFileWriterLicense.pdf contained within
//	the distribution specify the license agreement and other
//	conditions and notes. You must read this document and agree
//	with the conditions specified in order to use this software.
//
//	For version history please refer to PdfDocument.cs
//
/////////////////////////////////////////////////////////////////////

using System.Text;

namespace PdfFileWriter
	{
	/// <summary>
	/// Signature flags
	/// Table 8.68 page 674 and 695
	/// Note: flags can be combined with OR oerator
	/// </summary>
	public enum SigFlag
		{
		/// <summary>
		/// At least one signatre field exists
		/// </summary>
		SignaturesExist = 1,

		/// <summary>
		/// Append only
		/// </summary>
		AppendOnly = 2,
		}

	/// <summary>
	/// PDF AcroForm class (controls the /AcroForm dictionary)
	/// </summary>
	public class PdfAcroForm : PdfObject
		{
		internal List<int> PageObjectNo;

		internal List<PdfAcroPageNode> PageNode;

		// default font
		internal PdfDrawTextCtrl DefaultTextCtrl;

		/// <summary>
		/// AcroForm constructor
		/// </summary>
		/// <param name="Document">PDF document</param>
		public PdfAcroForm
				(
				PdfDocument Document
				) : base(Document)
			{
			// create empty list of pages containing fields
			PageObjectNo = new List<int>();
			PageNode = new List<PdfAcroPageNode>();

			// set need appearances to false
			NeedAppearances(false);

			// set signature flags to append only
			SignatureFlags(SigFlag.AppendOnly);
			return;
			}

		/// <summary>
		/// Create or return existing PdfAcroForm
		/// </summary>
		/// <param name="Document">PDF document</param>
		/// <returns>PdfScroForm</returns>
		public static PdfAcroForm CreateAcroForm
				(
				PdfDocument Document
				)
			{
			// create PdfAcroForm
			if(Document.AcroForm == null) Document.AcroForm = new PdfAcroForm(Document);

			// return acroform object
			return Document.AcroForm;
			}

		/// <summary>
		/// Add new field to the list
		/// </summary>
		/// <param name="Field">PDF AcroField</param>
		public void AddField
				(
				PdfAcroWidgetField Field
				)
			{
			// search the list of pages
			int Index = PageObjectNo.BinarySearch(Field.AnnotPage.ObjectNumber);

			// not found it is the first field for this page
			if(Index < 0)
				{
				// add a new page node
				PdfAcroPageNode AcroPageNode = new PdfAcroPageNode(Document);

				// each page node has array of kids one for each field on this page
				Index = ~Index;
				PageObjectNo.Insert(Index, Field.AnnotPage.ObjectNumber);
				PageNode.Insert(Index, AcroPageNode);

				// update page numbers
				for(int Index1 = Index; Index1 < PageNode.Count; Index1++) PageNode[Index1].PageNo = Index1 + 1;
				}

			// all fields but radio button
			if(Field.GetType() != typeof(PdfAcroRadioButton))
				{ 
				// add field to page node
				PageNode[Index].AddField(Field);
				}

			// radio button field
			else
				{
				// add field to page node
				PageNode[Index].AddRadioButtonField((PdfAcroRadioButton) Field);
				}
			return;
			}

		/// <summary>
		/// Default text fields appearance
		/// </summary>
		/// <param name="DefaultTextCtrl">PDF Font</param>
		public void DefaultTextAppearance
				(
				PdfDrawTextCtrl DefaultTextCtrl
				)
			{
			// save text ctrl
			this.DefaultTextCtrl = DefaultTextCtrl;

			// default appearance string
			Color TextColor = DefaultTextCtrl.TextColor == Color.Empty ? Color.Black : DefaultTextCtrl.TextColor;
			string DAString = string.Format("{0} {1} Tf {2}", DefaultTextCtrl.Font.ResourceCode,
				DefaultTextCtrl.FontSizeStr, PdfContents.ColorToString(TextColor, ColorToStr.NonStroking));

			// add default appearance to acro form dictionary
			Dictionary.AddPdfString("/DA", DAString);

			// default text justify
			if(DefaultTextCtrl.Justify == TextJustify.Center || DefaultTextCtrl.Justify == TextJustify.Right)
				{ 
				Dictionary.AddInteger("/Q", (int) DefaultTextCtrl.Justify);
				}
			else
				{
				Dictionary.Remove("/Q");
				}
			return;
			}

		/// <summary>
		/// Signature flags
		/// </summary>
		/// <param name="SigFlags">SigFlags enumeration</param>
		public void SignatureFlags
				(
				SigFlag SigFlags
				)
			{
			Dictionary.AddInteger("/SigFlags", (int) SigFlags);
			return;
			}

		/// <summary>
		/// Need appearances dictionary
		/// </summary>
		/// <param name="NeedApp">Need appearances (true or false)</param>
		public void NeedAppearances
				(
				bool NeedApp
				)
			{
			Dictionary.AddBoolean("/NeedAppearances", NeedApp);
			return;
			}

		/// <summary>
		/// Fields calculation order (option)
		/// </summary>
		/// <param name="AcroFields">Acro fields</param>
		public void CalculationOrder
				(
				PdfAnnotation[] AcroFields
				)
			{
			StringBuilder Str = new StringBuilder("[");
			foreach(PdfAnnotation Obj in AcroFields)
				{
				if(AcroFields.GetType() != typeof(PdfAcroWidgetField)) throw new ApplicationException("Acro field calculation order");
				Str.AppendFormat("{0} 0 R ", Obj.ObjectNumber);
				}
			Str[^1] = ']';
			Dictionary.Add("/CO", Str.ToString());
			return;
			}

		/// <summary>
		/// close object before writing to PDF file
		/// </summary>
		internal override void CloseObject()
			{
			// we have no fields
			if(PageNode.Count == 0) return;

			// default resource dictionary
			if(DefaultTextCtrl != null &&
				(DefaultTextCtrl.Font.FontResCodeUsed || DefaultTextCtrl.Font.FontResGlyphUsed))
				{ 
				// build default resource dictionary
				StringBuilder Resources = new StringBuilder("<</Font<<");
				if(DefaultTextCtrl.Font.FontResCodeUsed)
					Resources.Append(string.Format("{0} {1} 0 R", DefaultTextCtrl.Font.ResourceCode, DefaultTextCtrl.Font.ObjectNumber));
				if(DefaultTextCtrl.Font.FontResGlyphUsed)
					Resources.Append(string.Format("{0} {1} 0 R", DefaultTextCtrl.Font.ResourceCodeGlyph, DefaultTextCtrl.Font.GlyphIndexFont.ObjectNumber));
				Resources.Append(">>/ProcSet[/PDF/Text/ImageB/ImageC/ImageI]>>");
				Dictionary.Add("/DR", Resources.ToString());
				}

			// acro form fields array string
			StringBuilder KidsStr = new StringBuilder("[");

			// loop for all page nodes
			for(int Index = 0; Index < PageNode.Count; Index++)
				{ 
				// add first page fields object to acro form fields array
				KidsStr.AppendFormat("{0} 0 R ", PageNode[Index].ObjectNumber);
				}
			
			// add array of page nodes to acro form
			KidsStr[^1] = ']';
			Dictionary.Add("/Fields", KidsStr.ToString());

			// add acro form object to Catalog object
			Document.CatalogObject.Dictionary.AddIndirectReference("/AcroForm", this);

			// exit
			return;
			}
		}
	}
