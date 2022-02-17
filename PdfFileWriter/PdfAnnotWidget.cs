/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Annotation widget
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

namespace PdfFileWriter
	{
	/// <summary>
	/// Annotation widget for user interactive fields
	/// </summary>
	public class PdfAnnotWidget : PdfAnnotation
		{
		/// <summary>
		/// Border color (/BC)
		/// </summary>
		public Color BorderColor { get; set; }

		/// <summary>
		/// Background color (/BG)
		/// </summary>
		public Color BackgroundColor { get; set; }

		/// <summary>
		/// Caption (/CA)
		/// </summary>
		public string Caption { get; set; }

		/// <summary>
		/// Widget annotation (base for fields)
		/// </summary>
		/// <param name="Document">PDF document</param>
		internal PdfAnnotWidget
				(
				PdfDocument Document
				) : base(Document, "/Widget")
			{
			BorderColor = Color.Empty;
			BackgroundColor = Color.Empty;
			return;
			}

		/// <summary>
		/// close object before writing to PDF file
		/// </summary>
		internal override void CloseObject()
			{
			// all but radio button
			if(this.GetType() != typeof(PdfAcroRadioButton))
				{ 
				// test for at least one color is defined
				if(BorderColor != Color.Empty || BackgroundColor != Color.Empty || Caption != null) // || CaptionPosition != CaptionPosStyle.NoIcon)
					{ 
					// add appearance characteristics dictionary
					PdfDictionary AppCharDict = new PdfDictionary(this);
					Dictionary.AddDictionary("/MK", AppCharDict);

					// border color
					// add color array appearance characteristics dictionary
					if(BorderColor != Color.Empty) AppCharDict.Add("/BC", PdfContents.ColorToString(BorderColor, ColorToStr.Array));

					// background color
					// add color array appearance characteristics dictionary
					if(BackgroundColor != Color.Empty) AppCharDict.Add("/BG", PdfContents.ColorToString(BackgroundColor, ColorToStr.Array));

					// caption
					if(Caption != null) AppCharDict.AddPdfString("/CA", Caption);

					// caption position style
					// if(CaptionPosition != CaptionPosStyle.NoIcon) AppCharDict.AddInteger("/TP", (int) CaptionPosition);
					}
				}

			// close PdfAnnotation object
			base.CloseObject();
			return;
			}
		}
	}
