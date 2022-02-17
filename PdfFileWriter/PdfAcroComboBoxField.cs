/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Acro checkbox field
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
	/// Acro field combo box
	/// </summary>
	public class PdfAcroComboBoxField : PdfAcroWidgetField
		{
		/// <summary>
		/// Enable edit
		/// </summary>
		public bool Edit
			{
			get
				{
				return (FieldFlags & FieldFlag.Edit) != 0;
				}
			set
				{
				if(value)
					{
					FieldFlags |= FieldFlag.Edit;
					}
				else
					{
					FieldFlags &= ~FieldFlag.Edit;
					}
				}
			}

		/// <summary>
		/// Sort indicator
		/// NOTE: PDF Readers ignore this flag
		/// The reader will not sort the combobox items
		/// </summary>
		public bool Sort
			{
			get
				{
				return (FieldFlags & FieldFlag.Sort) != 0;
				}
			set
				{
				if(value)
					{
					FieldFlags |= FieldFlag.Sort;
					}
				else
					{
					FieldFlags &= ~FieldFlag.Sort;
					}
				}
			}

		/// <summary>
		/// Enable multi-select
		/// </summary>
		public bool MultiSelect
			{
			get
				{
				return (FieldFlags & FieldFlag.MultiSelect) != 0;
				}
			set
				{
				if(value)
					{
					FieldFlags |= FieldFlag.MultiSelect;
					}
				else
					{
					FieldFlags &= ~FieldFlag.MultiSelect;
					}
				}
			}

		/// <summary>
		/// Commit on select change
		/// </summary>
		public bool CommitOnSelChange
			{
			get
				{
				return (FieldFlags & FieldFlag.CommitOnSelChange) != 0;
				}
			set
				{
				if(value)
					{
					FieldFlags |= FieldFlag.CommitOnSelChange;
					}
				else
					{
					FieldFlags &= ~FieldFlag.CommitOnSelChange;
					}
				}
			}

		/// <summary>
		/// Array of items
		/// Items will be displayed in array's order
		/// </summary>
		public string[] Items { get; set; }

		/// <summary>
		/// Field value (/V) (selected item)
		/// </summary>
		public string FieldValue { get; set; }

		/// <summary>
		/// Acro field choice constructor
		/// </summary>
		/// <param name="AcroForm">Acro form parent</param>
		/// <param name="FieldName">Combobox field name</param>
		public PdfAcroComboBoxField
				(
				PdfAcroForm AcroForm,
				string FieldName
				) : base(AcroForm.Document, FieldName)
			{
			// field type is choice
			Dictionary.AddName("/FT", "Ch");

			// subtype is combo box
			FieldFlags |= FieldFlag.ComboBox;

			// add the field to acro fields structure
			AcroForm.AddField(this);
			return;
			}

		/// <summary>
		/// Draw combobox (appearance XObject)
		/// </summary>
		public void DrawComboBox()
			{
			// Create xobject
			PdfXObject XObject = new PdfXObject(Document, AnnotRect.Width, AnnotRect.Height);

			// clear field area
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Paint = DrawPaint.Fill;
			DrawCtrl.BackgroundTexture = BackgroundColor;
			XObject.DrawGraphics(DrawCtrl, XObject.BBox);

			// start of marked content
			XObject.BeginMarkedContent("/Tx");

			// calculate position
			double XPos = 0;
			if(TextCtrl.Justify == TextJustify.Center) XPos = 0.5 * XObject.BBox.Right;
			else if(TextCtrl.Justify == TextJustify.Right) XPos = XObject.BBox.Right;

			// draw the text
			XObject.DrawText(TextCtrl, XPos, TextCtrl.TextDescent, FieldValue);

			// end of marked content
			XObject.EndMarkedContent();

			// add to annotation appearance dictionary
			AddAppearance(XObject, AppearanceType.Normal);
			return;
			}

		/// <summary>
		/// close object before writing to PDF file
		/// </summary>
		internal override void CloseObject()
			{
			// build PDF options (items) array
			StringBuilder OptStr = new StringBuilder("[");
			foreach(string Op in Items)
				{
				OptStr.Append(TextToPdfString(Op, this));
				}
			OptStr.Append(']');
			Dictionary.Add("/Opt", OptStr.ToString());

			// text justify
			if(TextCtrl != null && (TextCtrl.Justify == TextJustify.Center || TextCtrl.Justify == TextJustify.Right))
				Dictionary.AddInteger("/Q", (int) TextCtrl.Justify);

			// Text field value (/V)
			if(FieldValue != null) Dictionary.AddPdfString("/V", FieldValue);

			// close PdfAnnotation object
			base.CloseObject();
			return;
			}
		}
	}
