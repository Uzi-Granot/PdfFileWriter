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

namespace PdfFileWriter
	{
	/// <summary>
	/// Acro check box field
	/// </summary>
	public class PdfAcroCheckBoxField : PdfAcroWidgetField
		{
		/// <summary>
		/// field value (/V)
		/// if field value is false, it is set to /Off
		/// if field value is true, it is set to the on-state name
		/// </summary>
		public bool Check { get; set; }

		/// <summary>
		/// On-state name (the off-state is always "Off") 
		/// On-state is defined by the constructor
		/// </summary>
		internal string OnStateName;

		/// <summary>
		/// ZapfDingbats font for checkbox
		/// It is acrobat's build-in type1 font
		/// </summary>
		internal PdfFontTypeOne Font;

		/// <summary>
		/// Check-mark (ZapfDignbats) symbol code
		/// </summary>
		public char CheckMarkChar;

		/// <summary>
		/// Checkmark font color
		/// </summary>
		public Color CheckMarkColor;

		/// <summary>
		/// Acro field button constructor
		/// </summary>
		/// <param name="AcroForm">Acro form (parent)</param>
		/// <param name="FieldName">Field name</param>
		/// <param name="OnStateName">Check box on value</param>
		public PdfAcroCheckBoxField
				(
				PdfAcroForm AcroForm,
				string FieldName,
				string OnStateName
				) : base(AcroForm.Document, FieldName)
			{
			// if on-state is missing, set it to /Yes
			if(string.IsNullOrWhiteSpace(OnStateName)) OnStateName = "Yes";

			// save on-state name
			this.OnStateName = OnStateName;

			// create ZapfDingbats font
			Font = PdfFontTypeOne.CreateFontTypeOne(Document, TypeOneFontCode.ZapfDingbats);

			// default checkmark
			CheckMarkChar = PdfFontTypeOne.ZaDiThinX;
			CheckMarkColor = Color.Black;

			// field type is button
			Dictionary.AddName("/FT", "Btn");

			// add the field to acro fields structure
			AcroForm.AddField(this);
			return;
			}

		/// <summary>
		/// Draw checkbox (Appearance dictionary XObject)
		/// </summary>
		/// <param name="AppType">Normal (/N), Roll over (/R), Down (/D)</param>
		/// <param name="Selected">Off (false) or selected (true)</param>
		public void DrawCheckBox
				(
				AppearanceType AppType,
				bool Selected
				)
			{
			// Normal off appearance stream
			PdfXObject XObject = new PdfXObject(Document, AnnotRect.Width, AnnotRect.Height);

			// set caption to check mark character
			Caption = CheckMarkChar.ToString();

			// clear field area and draw border within field area
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Paint = DrawPaint.BorderAndFill;
			DrawCtrl.BackgroundTexture = BackgroundColor;
			DrawCtrl.BorderColor = BorderColor;
			DrawCtrl.BorderWidth = 1 / Document.ScaleFactor;
			XObject.DrawGraphics(DrawCtrl, XObject.BBox);

			// display checkbox selected
			if (Selected)
				{
				// reduce box size by two point (border one point and clear space one point)
				PdfRectangle SymbolRect = XObject.BBox.AddMargin(-2 / Document.ScaleFactor);

				// font info class
				PdfTypeOneFontInfo FontInfo = Font.FontInfo;

				// symbol bounding box
				double SymLeft = 0.001 * FontInfo.CharBBoxLeft(CheckMarkChar);
				double SymBottom = 0.001 * FontInfo.CharBBoxBottom(CheckMarkChar);
				double SymRight = 0.001 * FontInfo.CharBBoxRight(CheckMarkChar);
				double SymTop = 0.001 * FontInfo.CharBBoxTop(CheckMarkChar);

				// symbol design width and height
				double DesignWidth = SymRight - SymLeft;
				double DesignHeight = SymTop - SymBottom;
				if (DesignWidth > 0 && DesignHeight > 0)
					{
					// font size in user units to fill the drawing rectangle
					double FontSize = SymbolRect.Width / DesignWidth;
					double FontSizeH = SymbolRect.Height / DesignHeight;

					// adjustment to center the symbol
					double DeltaX = 0;
					double DeltaY = 0;
					if (FontSize <= FontSizeH)
						{
						DeltaY = 0.5 * (SymbolRect.Height - FontSize * DesignHeight);
						}
					else
						{
						FontSize = FontSizeH;
						DeltaX = 0.5 * (SymbolRect.Width - FontSize * DesignWidth);
						}

					// symbol position
					double PosX = SymbolRect.Left - FontSize * SymLeft + DeltaX;
					double PosY = SymbolRect.Bottom - FontSize * SymBottom + DeltaY;

					// add font to used resources
					XObject.AddToUsedResources(Font);

					// begin marked content and save grapics state
					XObject.BeginMarkedContent("/Tx");
					XObject.SaveGraphicsState();
					XObject.BeginTextMode();

					// text mode
					XObject.SetColorNonStroking(CheckMarkColor);
					XObject.SetFontAndSize(Font, FontSize * ScaleFactor);
					XObject.SetTextPosition(PosX, PosY);

					// output text
					XObject.ObjectValueList.Add((byte)'(');
					XObject.OutputOneByte(CheckMarkChar);
					XObject.ObjectValueList.Add((byte)')');
					XObject.ObjectValueList.Add((byte)'T');
					XObject.ObjectValueList.Add((byte)'j');
					XObject.ObjectValueList.Add((byte)'\n');

					// restore grapics state and end marked content
					XObject.EndTextMode();
					XObject.RestoreGraphicsState();
					XObject.EndMarkedContent();
					}
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
			// initial value (either Off or on-state name)
			string InitialValue = Check ? OnStateName : "Off";
			Dictionary.AddName("/AS", InitialValue);

			// field value same as above
			Dictionary.AddName("/V", InitialValue);

			// default field value
			Dictionary.AddName("/DV", "Off");

			// create default appearance string
			string DefAppStr = string.Format("{0} 0 Tf {1}", Font.ResourceCode, 
				PdfContents.ColorToString(CheckMarkColor, ColorToStr.NonStroking));
			Dictionary.AddPdfString("/DA", DefAppStr);

			// create resource dictionary
			string ResDictStr = string.Format("<</Font<<{0} {1} 0 R>>>>", Font.ResourceCode, Font.ObjectNumber);
			Dictionary.Add("/DR", ResDictStr);

			// close PdfAnnotation object
			base.CloseObject();
			return;
			}
		}
	}
