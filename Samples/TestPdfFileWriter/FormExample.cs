/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter II
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	PDF Form example
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

using PdfFileWriter;
using System.Diagnostics;

namespace TestPdfFileWriter
	{
	/// <summary>
	/// PDF Form example
	/// </summary>
	public class FormExample
		{
		private PdfDocument Document;
		private PdfPage Page;
		private PdfContents Contents;
		private PdfAcroForm AcroForm;

		private PdfFont FixedTextFont;
		private PdfDrawTextCtrl FixedTextCtrl;
		private PdfFont FieldTextFont;
		private PdfDrawTextCtrl FieldTextCtrl;
		private PdfDrawTextCtrl ButtonTextCtrl;

		private PdfDrawCtrl DrawFrame;

		private const double FrameMargin = 0.04;
		private const double FieldMargin = 0.04;
		private const double TitleTopMargin = 0.1;
		private const double TitleBottomMargin = 0.04;

		/// <summary>
		/// Create fillable form PDF document
		/// </summary>
		/// <param name="FileName">PDF file name</param>
		/// <param name="Debug">Debug flag</param>
		public void Test
				(
				string FileName,
				bool Debug
				)
			{
			// create document
			using (Document = new PdfDocument(PaperType.Letter, false, UnitOfMeasure.Inch, FileName))
				{
				// debug flag
				Document.Debug = Debug;

				// fixed text font
				FixedTextFont = PdfFont.CreatePdfFont(Document, "Times New Roman", FontStyle.Regular);
				FixedTextCtrl = new PdfDrawTextCtrl(FixedTextFont, 12);

				// data entry font
				FieldTextFont = PdfFont.CreatePdfFont(Document, "Arial", FontStyle.Regular);
				FieldTextCtrl = new PdfDrawTextCtrl(FieldTextFont, 12);

				// set all ascii range is included
				FieldTextFont.SetCharRangeActive(' ', '~');

				// font for navigation buttons
				ButtonTextCtrl = new PdfDrawTextCtrl(Document, "Arial", FontStyle.Regular, 12);
				ButtonTextCtrl.TextColor = Color.DarkBlue;

				// frame around data entry field
				DrawFrame = new PdfDrawCtrl();
				DrawFrame.Paint = DrawPaint.BorderAndFill;
				DrawFrame.BorderWidth = 0.01;
				DrawFrame.BorderColor = Color.LightGray;
				DrawFrame.BackgroundTexture = Color.FromArgb(255, 255, 210);

				// this document has fillable fields
				// define acro form to control the fields
				AcroForm = PdfAcroForm.CreateAcroForm(Document);

				// draw pages
				DrawPage1();
				DrawPage2();

				// create pdf file
				Document.CreateFile();

				// start default PDF reader and display the file
				Process Proc = new Process();
				Proc.StartInfo = new ProcessStartInfo(FileName) {UseShellExecute = true};
				Proc.Start();
				}
			return;
			}

		// Draw page 1
		private void DrawPage1()
			{
			// add new page
			NewPage(1);

			// page reset button
			double ResetButWidth = ButtonTextCtrl.TextWidth("Reset Page") + 0.8 * ButtonTextCtrl.LineSpacing;
			double ResetButHeight = 1.4 * ButtonTextCtrl.LineSpacing;
			double PosX = 7.4 - ResetButWidth;
			double PosY = 9.9 - ResetButHeight;

			// clear page javascript text string
			string ClearStr =
				"var Result = app.alert('Do you want to reset the current page?', 2, 2, 'Reset Page');" +
				"if(Result == 4) {this.resetForm('Page1');}";

			// reset button
			PdfRectangle ResetRect = new PdfRectangle(PosX, PosY, PosX + ResetButWidth, PosY + ResetButHeight);
			AddButtonField(ResetRect, "ResetButton", "Reset Page", ClearStr);

			// define position of first text field
			PosX = 1.25;
			PosY -= 0.1;

			// name
			AddTextField(PosX, PosY, out double Width, out double Height, "Name (First Middle Last)", "Name", "John Doe", 40);

			// street address 1
			PosY -=  Height + 0.08;
			AddTextField(PosX, PosY, out Width, out Height, "Street address 1", "Address1", null, 40);

			// street address 2
			PosY -= Height + 0.08;
			AddTextField(PosX, PosY, out Width, out Height, "Street address 2", "Address2", null, 40);

			// City
			PosY -= Height + 0.08;
			AddTextField(PosX, PosY, out Width, out Height, "City/Town", "City", null, 20);

			// province state
			AddTextField(PosX + Width + 0.2, PosY, out Width, out Height, "Province/State", "Province", null, 15);

			// country
			PosY -= Height + 0.08;
			AddTextField(PosX, PosY, out Width, out Height, "Country", "Country", null, 15);

			// postal/zip code
			AddTextField(PosX + Width + 0.2, PosY, out Width, out Height, "Postal/Zip code", "PostalCode", null, 12);

			// horizontal line
			PosY -= Height + 0.14;
			LineD HLine = new LineD(PosX, PosY, 8.5 - PosX, PosY);
			Contents.DrawLine(HLine, 0.02, Color.Gray);

			// telephone
			PosY -= 0.06;
			AddTextField(PosX, PosY, out Width, out Height, "Telephone", "Phone", null, 17);

			// extension
			PosX += Width + 0.2;
			AddTextField(PosX, PosY, out Width, out Height, "Ext.", "Extension", null, 4);

			// email
			PosX += Width + 0.2;
			AddTextField(PosX, PosY, out Width, out Height, "EMail", "EMail", null, 30);

			// horizontal line
			PosX = 1.25;
			PosY -= Height + 0.14;
			HLine = new LineD(PosX, PosY, 8.5 - PosX, PosY);
			Contents.DrawLine(HLine, 0.02, Color.Gray);

			// gender radio buttons
			PosY -= 0.06;
			AddRadioButtonsGroup(PosX, PosY, out Width, out Height, "Gender", "Gender", new string[] {"Male", "Female", "Other" });

			// save y position
			//double SaveY = PosY - Height + 0.06;

			// date of birth
			PosX = 3;
			PosY -= FieldMargin;
			PdfAcroTextField DobField = AddTextField(PosX, PosY, out Width, out Height, "Date of birth (yyyy/mm/dd)", "DOB", null, 10);
			DobField.FieldFormatEvent = "AFDate_FormatEx(\"yyyy/mm/dd\");";
			DobField.FieldKeystrokeEvent = "AFDate_KeystrokeEx(\"yyyy/mm/dd\");";

			// send form button
			double SendButWidth = ButtonTextCtrl.TextWidth("Email Form") + 0.8 * ButtonTextCtrl.LineSpacing;
			double SendButHeight = 1.4 * ButtonTextCtrl.LineSpacing;
			double SendButPosX = 7.4 - ResetButWidth;
			double SendButPosY = 1.1;

			// send form button
			PdfRectangle SendRect = new PdfRectangle(SendButPosX, SendButPosY, SendButPosX + SendButWidth, SendButPosY + SendButHeight);
			AddButtonField(SendRect, "SendButton", "Email Form", "this.submitForm('mailto:?subject=Form%20Info&body=Here%20is%20my%20form%20data.', true)");
			return;
			}

		// Draw page 2
		private void DrawPage2()
			{
			// add new page
			NewPage(2);

			// define position of first text field
			double PosX = 1.25;
			double PosY = 9.85;

			// combo box select color title
			PosY -= FrameMargin + TitleTopMargin + FixedTextCtrl.TextAscent;
			Contents.DrawText(FixedTextCtrl, PosX, PosY, "Combo box Select color");
	
			string[] ColorChoices = {"Red", "Orange", "Yellow", "Green", "Blue", "Indigo", "Violet"};

			// combo box select color data entry
			PosY -= FixedTextCtrl.TextDescent + TitleBottomMargin + FrameMargin + FieldTextCtrl.LineSpacing;
			PdfRectangle ComboColorRect = new PdfRectangle(PosX, PosY, PosX + 2, PosY + FieldTextCtrl.LineSpacing);
			Contents.DrawGraphics(DrawFrame, ComboColorRect.AddMargin(FrameMargin));
			AddComboBoxField(ComboColorRect, "ComboSelectColor", ColorChoices[3], ColorChoices);

			// List box select color title
			PosY -= FrameMargin + TitleTopMargin + FixedTextCtrl.TextAscent;
			Contents.DrawText(FixedTextCtrl, PosX, PosY, "List box Select color");
			PosY -= FixedTextCtrl.TextDescent + TitleBottomMargin + FrameMargin;

			// List box select color rectangle
			double FontSize = 12.0;
			double LineSpacing = 1.2 * FontSize / Document.ScaleFactor;
			double ListBoxHeight = 4 * LineSpacing;
			PosY -= ListBoxHeight;
			PdfRectangle ListColorRect = new PdfRectangle(PosX, PosY, PosX + 2, PosY + ListBoxHeight);

			// draw frame
			Contents.DrawGraphics(DrawFrame, ListColorRect.AddMargin(FrameMargin));

			// font
			PdfFontTypeOne Helvetica = PdfFontTypeOne.CreateFontTypeOne(Document, TypeOneFontCode.Helvetica);

			// field
			PdfAcroListBoxField Field = new PdfAcroListBoxField(AcroForm, "ListSelectColor", Helvetica, FontSize);

			Field.AnnotRect = ListColorRect;
			Field.BackgroundColor = Color.FromArgb(240, 240, 255);
			Field.FieldValue = ColorChoices[0];
			Field.Items = ColorChoices;
			Field.TopIndex = 0;

			// list box field appearance
			Field.DrawListBox();

			// radio buttons selections
			PosY -= FrameMargin + TitleTopMargin + FixedTextCtrl.TextAscent;
			Contents.DrawText(FixedTextCtrl, PosX, PosY, "Check boxes selections");

			char[] ZaDiChar =
				{
				PdfFontTypeOne.ZaDiStylizedV,
				PdfFontTypeOne.ZaDiStrightV,
				PdfFontTypeOne.ZaDiThinX,
				PdfFontTypeOne.ZaDiThickX,
				PdfFontTypeOne.ZaDiStylizedThinX,
				PdfFontTypeOne.ZaDiStylizedThickX,
				PdfFontTypeOne.ZaDiFiveSidesStar,
				PdfFontTypeOne.ZaDiSixSidesStar,
				PdfFontTypeOne.ZaDiCircle,
				PdfFontTypeOne.ZaDiSquare,
				};

			string[] CheckMarkText =
				{
				"StylizedV",
				"StrightV",
				"ThinX",
				"ThickX",
				"StylizedThinX",
				"StylizedThickX",
				"FiveSidesStar",
				"SixSidesStar",
				"Circle",
				"Square",
				};

			// display all available checkbox symbols
			double CheckBoxSide = FixedTextCtrl.LineSpacing;
			PosY -= FixedTextCtrl.TextDescent + 2 * TitleBottomMargin + CheckBoxSide;
			double SavePosY = PosY;
			for(int Index = 0; Index < 10; Index++)
				{
				// index number as text
				string IndexStr = (Index + 1).ToString();

				PdfRectangle CheckBoxRect = new PdfRectangle(PosX, PosY, PosX + CheckBoxSide, PosY + CheckBoxSide);
				AddCheckBoxField(CheckBoxRect, "CheckBox" + IndexStr, "Select" + IndexStr, ZaDiChar[Index], (Index & 1) == 0);

				// check box 1 caption
				Contents.DrawText(FixedTextCtrl, PosX + CheckBoxSide + 0.1, PosY + FixedTextCtrl.TextDescent, IndexStr + ". " + CheckMarkText[Index]);

				// next line
				if(Index != 4)
					{ 
					PosY -= CheckBoxSide + 0.1;
					}
				// new column
				else
					{
					PosY = SavePosY;
					PosX = 4.25;
					}
				}

			// set position
			PosX = 1.25;
			PosY -= 0.5;

			// radio buttons selections
			Contents.DrawText(FixedTextCtrl, PosX, PosY, "Radio buttons group A");

			// radio button group A button 1
			double RadioButtonSide = FixedTextCtrl.LineSpacing;
			PosY -= FixedTextCtrl.TextDescent + TitleBottomMargin + RadioButtonSide;
			PdfRectangle RadioButtonA1Rect = new PdfRectangle(PosX, PosY, PosX + RadioButtonSide, PosY + RadioButtonSide);
			AddRadioButton(RadioButtonA1Rect, "RadioButtonGroupA", "SelectA1", false);

			// check box A1 caption
			Contents.DrawText(FixedTextCtrl, PosX + RadioButtonSide + 0.1, PosY + FixedTextCtrl.TextDescent, "Radio button A-1");

			// radio button group A button 2
			PosY -= FixedTextCtrl.TextDescent + TitleBottomMargin + RadioButtonSide;
			PdfRectangle RadioButtonA2Rect = new PdfRectangle(PosX, PosY, PosX + RadioButtonSide, PosY + RadioButtonSide);
			AddRadioButton(RadioButtonA2Rect, "RadioButtonGroupA", "SelectA2", true);

			// check box A2 caption
			Contents.DrawText(FixedTextCtrl, PosX + RadioButtonSide + 0.1, PosY + FixedTextCtrl.TextDescent, "Radio button A-2");

			// radio button group A button 3
			PosY -= FixedTextCtrl.TextDescent + TitleBottomMargin + RadioButtonSide;
			PdfRectangle RadioButtonA3Rect = new PdfRectangle(PosX, PosY, PosX + RadioButtonSide, PosY + RadioButtonSide);
			AddRadioButton(RadioButtonA3Rect, "RadioButtonGroupA", "SelectA3", false);

			// check box A3 caption
			Contents.DrawText(FixedTextCtrl, PosX + RadioButtonSide + 0.1, PosY + FixedTextCtrl.TextDescent, "Radio button A-3");

			// radio buttons selections
			PosY -= FixedTextCtrl.TextDescent + 2 * TitleBottomMargin + RadioButtonSide;
			Contents.DrawText(FixedTextCtrl, PosX, PosY, "Radio buttons group B");

			// radio button group B button 1
			PosY -= FixedTextCtrl.TextDescent + TitleBottomMargin + RadioButtonSide;
			PdfRectangle RadioButtonB1Rect = new PdfRectangle(PosX, PosY, PosX + RadioButtonSide, PosY + RadioButtonSide);
			AddRadioButton(RadioButtonB1Rect, "RadioButtonGroupB", "SelectB1", false);

			// check box B1 caption
			Contents.DrawText(FixedTextCtrl, PosX + RadioButtonSide + 0.1, PosY + FixedTextCtrl.TextDescent, "Radio button B-1");

			// radio button group B button 2
			PosY -= FixedTextCtrl.TextDescent + TitleBottomMargin + RadioButtonSide;
			PdfRectangle RadioButtonB2Rect = new PdfRectangle(PosX, PosY, PosX + RadioButtonSide, PosY + RadioButtonSide);
			AddRadioButton(RadioButtonB2Rect, "RadioButtonGroupB", "SelectB2", false);

			// check box B2 caption
			Contents.DrawText(FixedTextCtrl, PosX + RadioButtonSide + 0.1, PosY + FixedTextCtrl.TextDescent, "Radio button B-2");

			// check boxs selections
			PosY -= FixedTextCtrl.TextDescent + 2 * TitleBottomMargin + RadioButtonSide;
			Contents.DrawText(FixedTextCtrl, PosX, PosY, "Radio button group C");

			// radio button group C button 1
			PosY -= FixedTextCtrl.TextDescent + TitleBottomMargin + RadioButtonSide;
			PdfRectangle RadioButtonC1Rect = new PdfRectangle(PosX, PosY, PosX + RadioButtonSide, PosY + RadioButtonSide);
			AddRadioButton(RadioButtonC1Rect, "RadioButtonGroupC", "SelectC1", false);

			// check box C1 caption
			Contents.DrawText(FixedTextCtrl, PosX + RadioButtonSide + 0.1, PosY + FixedTextCtrl.TextDescent, "Radio button C-1");
			return;
			}

		// new page common code
		private void NewPage
				(
				int PageNo
				)
			{ 
			// Add new page
			Page = new PdfPage(Document);

			// Add contents to page
			Contents = new PdfContents(Page);

			// draw frame with 1" margin
			PdfRectangle Rect = new PdfRectangle(1.0, 1.0, 7.5, 10.0);
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.BorderWidth = 0.02;
			DrawCtrl.BorderColor = Color.Gray;
			Contents.DrawGraphics(DrawCtrl, Rect);

			// page number
			FixedTextCtrl.Justify = TextJustify.Right;
			Contents.DrawText(FixedTextCtrl, 7.5, 10.06 + FixedTextCtrl.TextDescent, string.Format("Page {0}", PageNo));
			FixedTextCtrl.Justify = TextJustify.Left;

			// navigation buttons position
			double NavButWidth = 2 * ButtonTextCtrl.LineSpacing;
			double NavButHeight = 1.2 * ButtonTextCtrl.LineSpacing;
			double NavButMargin = 0.1;
			double PosX = 7.5 - 4 * NavButWidth - 3 * NavButMargin;
			double PosY = 1 - NavButHeight - NavButMargin;

			// navigation buttons
			PdfRectangle FirstPageRect = new PdfRectangle(PosX, PosY, PosX + NavButWidth, PosY + NavButHeight);
			AddButtonField(FirstPageRect, "FirstPage", "<<", NamedActionCode.FirstPage);

			PdfRectangle PrevPageRect = FirstPageRect.Move(NavButWidth + NavButMargin, 0);
			AddButtonField(PrevPageRect, "PrevPage", "<", NamedActionCode.PrevPage);

			PdfRectangle NextPageRect = PrevPageRect.Move(NavButWidth + NavButMargin, 0);
			AddButtonField(NextPageRect, "NextPage", ">", NamedActionCode.NextPage);

			PdfRectangle LastPageRect = NextPageRect.Move(NavButWidth + NavButMargin, 0);
			AddButtonField(LastPageRect, "LastPage", ">>", NamedActionCode.LastPage);

			// exit with page object
			return;
			}

		/// <summary>
		/// Add text field
		/// </summary>
		/// <param name="OrigX">Field's left origin</param>
		/// <param name="OrigY">Field's top origin</param>
		/// <param name="Width">Returned overall width</param>
		/// <param name="Height">Returned overall height</param>
		/// <param name="FieldDescription">Field description</param>
		/// <param name="FieldName">Field name</param>
		/// <param name="FieldValue">Field value</param>
		/// <param name="TextMaxLength">Field maximum length</param>
		/// <returns>PDF acro text field object</returns>
		private PdfAcroTextField AddTextField
				(
				double OrigX,
				double OrigY,
				out double Width,
				out double Height,
				string FieldDescription,
				string FieldName,
				string FieldValue,
				int TextMaxLength
				)
			{
			const double VertGap = 0.12;
			const double FrameToField = 0.04;

			// adjust PosY to the bottom of field description
			double PosY = OrigY;
			PosY -= FixedTextCtrl.LineSpacing;

			// field description
			double DescriptionWidth = Contents.DrawText(FixedTextCtrl, OrigX + FieldMargin, PosY + FixedTextCtrl.TextDescent, FieldDescription);

			// adjust top to the bottom of field frame
			PosY -= FieldTextCtrl.LineSpacing + VertGap;

			// field width
			double FieldWidth = TextMaxLength * FieldTextCtrl.CharWidth('0');
			double FieldHeight = FieldTextCtrl.LineSpacing;

			// frame margin
			double FrameMargin = DrawFrame.BorderWidth + FrameToField;

			// frame rectangle
			PdfRectangle FrameRect = new PdfRectangle(OrigX, PosY, OrigX + FieldWidth + 2 * FrameMargin, PosY + FieldHeight + 2 * FrameMargin);

			// draw frame
			Contents.DrawGraphics(DrawFrame, FrameRect);

			// data entry field rectangle
			PdfRectangle FieldRect = FrameRect.AddMargin(-FrameMargin);

			// text field
			PdfAcroTextField TextField = new PdfAcroTextField(AcroForm, FieldName);
			TextField.AnnotRect = FieldRect;
			TextField.TextCtrl = FieldTextCtrl;
			TextField.TextMaxLength = TextMaxLength;
			TextField.BackgroundColor = Color.FromArgb(240, 240, 255);
			if(!string.IsNullOrWhiteSpace(FieldValue)) TextField.FieldValue = FieldValue;

			// text field appearance
			TextField.DrawTextField();

			Width = Math.Max(FrameRect.Width, DescriptionWidth);
			Height = OrigY - FrameRect.Bottom;

			// return
			return TextField;
			}

		/// <summary>
		/// Add button field
		/// </summary>
		/// <param name="Rect">Button's rectangle</param>
		/// <param name="FieldName">Field name</param>
		/// <param name="Caption">Caption</param>
		/// <param name="ActionCode">Action code</param>
		/// <exception cref="ApplicationException">Action must be either named action or java script</exception>
		private void AddButtonField
				(
				PdfRectangle Rect,
				string FieldName,
				string Caption,
				Object ActionCode
				)
			{
			// action must be either named action or java script
			if(ActionCode.GetType() != typeof(NamedActionCode) && ActionCode.GetType() != typeof(string))
				throw new ApplicationException("Button field action code must be NamedActionCode or string");

			// create button field
			PdfAcroButtonField Button = new PdfAcroButtonField(AcroForm, FieldName);
			Button.AnnotRect = Rect;
			Button.TextCtrl = ButtonTextCtrl;
			Button.Caption = Caption;
			Button.BackgroundColor = Color.FromArgb(220, 220, 255);
			Button.BorderColor = Color.FromArgb(128, 128, 200);

			if(ActionCode.GetType() == typeof(NamedActionCode))
				Button.NamedAction = (NamedActionCode) ActionCode;
			else
				Button.JavaScriptAction = (string) ActionCode;

			// button field appearance
			Button.DrawButtonField(AppearanceType.Normal);
			return;
			}

		/// <summary>
		/// Add radio buttons group
		/// </summary>
		/// <param name="PosX">Field's left origin</param>
		/// <param name="PosY">Field's top origin</param>
		/// <param name="Width">Returned overall width</param>
		/// <param name="Height">Returned overall height</param>
		/// <param name="GroupDescription">Group description</param>
		/// <param name="GroupName">Group name</param>
		/// <param name="ButtonDescription">Buttons description array</param>
		private void AddRadioButtonsGroup
				(
				double PosX,
				double PosY,
				out double Width,
				out double Height,
				string GroupDescription,
				string GroupName,
				string[] ButtonDescription
				)
			{
			// save top
			double Top = PosY;

			// radio buttons group description
			PosY -= FixedTextCtrl.LineSpacing + FieldMargin;
			Contents.DrawText(FixedTextCtrl, PosX + FieldMargin, PosY + FixedTextCtrl.TextDescent, GroupDescription);

			// number of buttons
			int Buttons = ButtonDescription.Length;

			// radio button size
			double RadioButtonSide = FixedTextCtrl.LineSpacing;

			// frame top position
			PosY -= FieldMargin;

			// frame width
			double FrameWidth = 1.2;

			// frame height
			double FrameHeight = Buttons * RadioButtonSide + (Buttons + 3) * FieldMargin + 2 * DrawFrame.BorderWidth;

			// frame rectangle
			PdfRectangle FrameRect = new PdfRectangle(PosX, PosY - FrameHeight, PosX + FrameWidth, PosY);

			// draw frame
			Contents.DrawGraphics(DrawFrame, FrameRect);

			// draw gray area within the frame
			PdfRectangle FrameActiveRect = FrameRect.AddMargin(-FieldMargin);
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Paint = DrawPaint.Fill;
			DrawCtrl.BackgroundTexture = Color.FromArgb(240, 240, 255);
			Contents.DrawGraphics(DrawCtrl, FrameActiveRect);

			// position of first button
			PosX = FrameActiveRect.Left + FieldMargin;
			PosY = FrameActiveRect.Top - FieldMargin - RadioButtonSide;

			// draw buttons
			for(int Index = 0; Index < Buttons; Index++)
				{
				PdfRectangle RadioButtonRect = new PdfRectangle(PosX, PosY, PosX + RadioButtonSide, PosY + RadioButtonSide);
				AddRadioButton(RadioButtonRect, GroupName, ButtonDescription[Index], Index == 0);

				// check box A1 caption
				Contents.DrawText(FixedTextCtrl, PosX + RadioButtonSide + 3 * FieldMargin, PosY + FixedTextCtrl.TextDescent, ButtonDescription[Index]);

				// adjust y position
				PosY -= RadioButtonSide + FieldMargin;
				}

			Width = FrameWidth;
			Height = Top - PosY - FieldMargin - DrawFrame.BorderWidth;
			return;
			}

		/// <summary>
		/// Add radio button
		/// </summary>
		/// <param name="Rect">Button's rectangle</param>
		/// <param name="GroupName">Group button belongs to</param>
		/// <param name="OnStateName">On-State name</param>
		/// <param name="InitialCheck">Initial check mark</param>
		private void AddRadioButton
				(
				PdfRectangle Rect,
				string GroupName,
				string OnStateName,
				bool InitialCheck
				)
			{
			// checkbox field
			PdfAcroRadioButton RadioButton = new PdfAcroRadioButton(AcroForm, GroupName, OnStateName);
			RadioButton.AnnotRect = Rect;
			RadioButton.Check = InitialCheck;
			RadioButton.BackgroundColor = Color.FromArgb(240, 240, 255);
			RadioButton.BorderColor = Color.FromArgb(0, 0, 128);
			RadioButton.RadioButtonColor = Color.FromArgb(0, 0, 128);

			// call auto appearance method and save appearance stream
			RadioButton.DrawRadioButton(AppearanceType.Normal, false);
			RadioButton.DrawRadioButton(AppearanceType.Normal, true);
			return;
			}

		/// <summary>
		/// Add combo box
		/// </summary>
		/// <param name="BoxRect">Box rectangle</param>
		/// <param name="FieldName">Field name</param>
		/// <param name="FieldValue">Field value</param>
		/// <param name="Items">Items array</param>
		private void AddComboBoxField
				(
				PdfRectangle BoxRect,
				string FieldName,
				string FieldValue,
				string[] Items
				)
			{
			// combo box field
			PdfAcroComboBoxField Field = new PdfAcroComboBoxField(AcroForm, FieldName);
			Field.AnnotRect = BoxRect;
			Field.TextCtrl = FieldTextCtrl;
			Field.BackgroundColor = Color.FromArgb(240, 240, 255);
			Field.Edit = true;
			Field.Sort = true;
			Field.FieldValue = FieldValue;
			Field.Items = Items;

			// combo box field appearance
			Field.DrawComboBox();

			// return
			return;
			}

		/// <summary>
		/// Add check box field
		/// </summary>
		/// <param name="Rect">Check field rectangle</param>
		/// <param name="FieldName">Field name</param>
		/// <param name="OnStateName">On-State name</param>
		/// <param name="ZaDiChar">Display character</param>
		/// <param name="InitialCheck">Field initially checked</param>
		private void AddCheckBoxField
				(
				PdfRectangle Rect,
				string FieldName,
				string OnStateName,
				char ZaDiChar,
				bool InitialCheck
				)
			{
			// checkbox field
			PdfAcroCheckBoxField CheckBox = new PdfAcroCheckBoxField(AcroForm, FieldName, OnStateName);
			CheckBox.AnnotRect = Rect;
			CheckBox.Check = InitialCheck;
			CheckBox.CheckMarkChar = ZaDiChar;
			CheckBox.CheckMarkColor = Color.FromArgb(0, 0, 128);
			CheckBox.BackgroundColor = Color.FromArgb(240, 240, 255);
			CheckBox.BorderColor = Color.FromArgb(0, 0, 128);
			CheckBox.BorderWidth = 1 / Document.ScaleFactor;

			// call auto appearance method and save appearance stream
			CheckBox.DrawCheckBox(AppearanceType.Normal, false);
			CheckBox.DrawCheckBox(AppearanceType.Normal, true);
			return;
			}
		}
	}
