/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PDF annotation class
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
	/// Appearance type code
	/// </summary>
	public enum AppearanceType
		{
		/// <summary>
		/// Normal appearance
		/// </summary>
		Normal,

		/// <summary>
		/// Pointing device rollover appearance
		/// </summary>
		Rollover,

		/// <summary>
		/// Pointing device down appearance
		/// </summary>
		Down,
		}

	/// <summary>
	/// Border style
	/// </summary>
	public enum BorderStyle
		{
		/// <summary>
		/// Border style solid line
		/// </summary>
		Solid,

		/// <summary>
		/// Border style dashed line
		/// </summary>
		Dashed,

		/// <summary>
		/// Border style beveled line
		/// </summary>
		Beveled,

		/// <summary>
		/// Border style inset line
		/// </summary>
		Inset,

		/// <summary>
		/// Border style underline
		/// </summary>
		Underline,
		}

	/// <summary>
	/// PDF Annotation class
	/// </summary>
	public class PdfAnnotation : PdfObject
		{
		/// <summary>
		/// Annotation page (/P)
		/// </summary>
		public PdfPage AnnotPage;

		/// <summary>
		/// Annotation rectangle (/Rect)
		/// </summary>
		public PdfRectangle AnnotRect;

		/// <summary>
		/// Annotation text or description (/Contents)
		/// </summary>
		public string AnnotText;

		/// <summary>
		/// Annotation flags (/F)
		/// This program sets Bit 3 (value=4) Allow print
		/// </summary>
		public int AnnotFlags;

		/// <summary>
		/// Appearance state (/AS)
		/// selects the applicable appearance stream from an appearance subdictionary
		/// </summary>
		public string AppearanceState;

		/// <summary>
		/// Appearance dictionary (/AP)
		/// </summary>
		public PdfDictionary AppearanceDictionary;

		/// <summary>
		/// Border width (/BS&lt;&lt;/W x&gt;&gt;)
		/// </summary>
		public double BorderWidth;

		/// <summary>
		/// Border style (/BS&lt;&lt;/W x/S/code&gt;&gt;)
		/// </summary>
		public BorderStyle BorderStyle;

		// border style code to letter
		internal static string[] CodeLetter = {"S", "D", "B", "I", "U"};

		/// <summary>
		/// Border dash array (/BS7ltl&lt;/summary>W x/S/D/D[x x x]&gt;&gt;)
		/// </summary>
		public int[] BorderDashArray;

		/// <summary>
		/// Color value for some specifc cases (/C)
		/// Color of: The background of the annotation’s icon when closed
		///	The title bar of the annotation’s pop-up window
		///	The border of a link annotation
		/// </summary>
		public Color ColorSpecific;

		/// <summary>
		/// Optional content (Layer control) (/OC)
		/// </summary>
		public PdfObject OptionalContent;

		// appearance type code
		internal static string[] AppTypeCode = {"/N" , "/R", "/D"};

		/// <summary>
		/// PDF annotation constructor
		/// </summary>
		/// <param name="Document">PDF document</param>
		/// <param name="Subtype">Annotation subtype</param>
		internal PdfAnnotation
				(
				PdfDocument Document,
				string Subtype
				) : base(Document, ObjectType.Dictionary, "/Annot")
			{
			// annotation subtype
			Dictionary.Add("/Subtype", Subtype);

			// annotation flags. value of 4 = Bit position 3 print
			AnnotFlags = 4;

			// default border style is solid
			BorderStyle = BorderStyle.Solid;

			// assume that annotation page is the current page
			AnnotPage = Document.CurrentPage;
			return;
			}

		// copy base elements
		internal void CreateCopy
				(
				PdfAnnotation Other
				)
			{
			AnnotRect = Other.AnnotRect;
			AnnotText = Other.AnnotText;
			AnnotFlags = Other.AnnotFlags;
			AppearanceState = Other.AppearanceState;
			AppearanceDictionary = Other.AppearanceDictionary;
			BorderWidth = Other.BorderWidth;
			BorderStyle = Other.BorderStyle;
			BorderDashArray = Other.BorderDashArray;
			ColorSpecific = Other.ColorSpecific;
			OptionalContent = Other.OptionalContent;
			return;
			}

		/// <summary>
		/// Add appearance XObject to annotation
		/// </summary>
		/// <param name="AppearanceObject">Appearance XObject</param>
		/// <param name="AppearanceType">Appearance type</param>
		public void AddAppearance
				(
				PdfXObject AppearanceObject,
				AppearanceType AppearanceType
				)
			{ 
			// create appearance dictionary
			if(AppearanceDictionary == null) AppearanceDictionary = new PdfDictionary(this);

			// add PdfXobject to appearance dictionary
			AppearanceDictionary.AddIndirectReference(AppTypeCode[(int) AppearanceType], AppearanceObject);
			return;
			}

		/// <summary>
		/// Add appearance XObject to annotation
		/// </summary>
		/// <param name="AppearanceObject">Appearance XObject</param>
		/// <param name="AppearanceType">Appearance type</param>
		/// <param name="AppearanceSubtype">Appearance subtype</param>
		public void AddAppearance
				(
				PdfXObject AppearanceObject,
				AppearanceType AppearanceType,
				string AppearanceSubtype
				)
			{ 
			// create appearance dictionary
			if(AppearanceDictionary == null) AppearanceDictionary = new PdfDictionary(this);

			// get or add appearance sub-dictionary
			PdfDictionary AppSubDict = AppearanceDictionary.GetOrAddDictionary(AppTypeCode[(int) AppearanceType]);

			// add PdfXobject to appearance dictionary
			AppSubDict.AddIndirectReference("/" + AppearanceSubtype, AppearanceObject);
			return;
			}

		/// <summary>
		/// close object before writing to PDF file
		/// </summary>
		internal override void CloseObject()
			{
			// test program error
			if(AnnotPage == null) throw new ApplicationException("Annotation page is undefined");

			// test program error
			if(AnnotRect == null) throw new ApplicationException("Annotation rectangle is undefined");

			// add indirect reference to page object (it is optional)
			Dictionary.AddRefNo("/P", AnnotPage.ObjectNumber);

			// add annotation object to page /Annots array
			PdfKeyValue KeyValue = AnnotPage.Dictionary.GetKeyValue("/Annots");
			if(KeyValue == null)
				{
				// start a new array
				AnnotPage.Dictionary.AddFormat("/Annots", "[{0} 0 R]", ObjectNumber);
				}
			else
				{
				// append to existing aray
				AnnotPage.Dictionary.Add("/Annots", ((string) KeyValue.Value).Replace("]", string.Format(" {0} 0 R]", ObjectNumber)));
				}

			// area rectangle on the page
			Dictionary.AddRectangle("/Rect", AnnotRect);

			// annotation flags.
			Dictionary.Add("/F", AnnotFlags.ToString());

			// appearance state
			if(AppearanceState != null) Dictionary.Add("/AS", AppearanceState);

			// appearance dictionary
			if(AppearanceDictionary != null) Dictionary.AddDictionary("/AP", AppearanceDictionary);

			// border style dictionary
			StringBuilder BSDict = new StringBuilder();
				
			// format border width and style
			BSDict.AppendFormat("<</W {0}/S/{1}", ToPt(BorderWidth), CodeLetter[(int) BorderStyle]);

			// style is dash
			if(BorderStyle == BorderStyle.Dashed)
				{ 
				BSDict.Append("/D");
				if(BorderDashArray != null)
					{ 
					BSDict.Append('[');
					foreach(double Value in BorderDashArray) ObjectValueFormat("{0} ", ToPt(Value));
					BSDict.Append(']');
					}
				}

			// terminate
			BSDict.Append(">>");

			// add to dictionary
			Dictionary.Add("/BS", BSDict.ToString());

			// color specific for icons
			if(ColorSpecific != Color.Empty) Dictionary.Add("/C", PdfContents.ColorToString(ColorSpecific, ColorToStr.Array));

			// Annotation text or description
			if(AnnotText != null) Dictionary.AddPdfString("/Contents", AnnotText);

			// optional content (layer control)
			if(OptionalContent != null) Dictionary.AddIndirectReference("/OC", OptionalContent);

			// exit
			return;
			}
		}
	}
