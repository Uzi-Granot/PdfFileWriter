/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Acro radio button widget
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
	/// Radio button widget
	/// A group of radio buttons will make one field
	/// </summary>
	public class PdfAcroRadioButton : PdfAcroWidgetField
		{
		/// <summary>
		/// This radio button widget is on
		/// </summary>
		public bool Check { get; set; }

		/// <summary>
		/// On-state name (the off-state is always "Off") 
		/// </summary>
		internal string OnStateName;

		/// <summary>
		/// ZapfDingbats font color
		/// </summary>
		public Color RadioButtonColor;

		/// <summary>
		/// Acro field radio button constructor
		/// </summary>
		/// <param name="AcroForm">Acro form object</param>
		/// <param name="GroupName">Group of radio buttons name</param>
		/// <param name="OnStateName">Radio button on value</param>
		public PdfAcroRadioButton
				(
				PdfAcroForm AcroForm,
				string GroupName,
				string OnStateName
				) : base(AcroForm.Document, GroupName)
			{
			// test argument
			if(string.IsNullOrWhiteSpace(OnStateName)) throw new ApplicationException("Radio button On-state must be defined");

			// save on-state name
			this.OnStateName = OnStateName;

			// radio button color
			BackgroundColor = Color.White;
			BorderColor = Color.Black;
			RadioButtonColor = Color.Black;

			// add the field to acro fields structure
			AcroForm.AddField(this);
			return;
			}

		/// <summary>
		/// Draw radio button
		/// </summary>
		/// <param name="AppType">Appearance type (Normal, Down, Rollover)</param>
		/// <param name="Selected">Selected</param>
		public void DrawRadioButton
				(
				AppearanceType AppType,
				bool Selected
				)
			{
			// Normal off appearance stream
			PdfXObject XObject = new PdfXObject(Document, AnnotRect.Width, AnnotRect.Height);

			// clear field area and draw border within field area
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Shape = DrawShape.Oval;
			DrawCtrl.Paint = DrawPaint.BorderAndFill;
			DrawCtrl.BackgroundTexture = BackgroundColor;
			DrawCtrl.BorderColor = BorderColor;
			DrawCtrl.BorderWidth = 1 / Document.ScaleFactor;
			XObject.DrawGraphics(DrawCtrl, XObject.BBox);

			if(Selected)
				{ 
				// reduce box size by two point (border one point and clear space one point)
				PdfRectangle SymbolRect = XObject.BBox.AddMargin(-2 / Document.ScaleFactor);

				DrawCtrl = new PdfDrawCtrl();
				DrawCtrl.Shape = DrawShape.Oval;
				DrawCtrl.Paint = DrawPaint.Fill;
				DrawCtrl.BackgroundTexture = BorderColor;
				XObject.DrawGraphics(DrawCtrl, SymbolRect);
				}

			// add to appearance dictionary
			AddAppearance(XObject, AppType, Selected ? OnStateName : "Off");
			return;
			}

		/// <summary>
		/// close object before writing to PDF file
		/// </summary>
		internal override void CloseObject()
			{
			// selected value
			Dictionary.AddName("/AS", Check ? OnStateName : "Off");

			// close PdfAnnotation object
			base.CloseObject();
			return;
			}
		}
	}
