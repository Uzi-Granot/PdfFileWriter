/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter II
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	ArticleExample
//	Produce PDF file when the Artice Example is clicked.
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
	/// Article example class 
	/// </summary>
	public class ArticleExample
		{
		private PdfFont ArialNormal;
		private PdfFont ArialBold;
		private PdfFont ArialItalic;
		private PdfFont ArialBoldItalic;
		private PdfFont TimesNormal;
		private PdfFont Comic;
		private PdfTilingPattern WaterMark;
		private PdfDocument Document;
		private PdfPage Page;
		private PdfContents Contents;

		private static readonly string
			ArticleLink = "http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version";

		/// <summary>
		/// Create article's example test PDF document
		/// </summary>
		/// <param name="Debug">Debug flag</param>
		/// <param name="FileName">PDF file name</param>
		public void Test
				(
				string FileName,
				bool Debug
				)
			{
			// The Test method below demonstrates the six steps described in the introduction for creating
			// a PDF file.The method will be executed when you press on the “Article Example” button of the
			// demo program.The following subsections describe in detail each step.

			// Step 1: Create empty document
			// Arguments: page width: 8.5”, page height: 11”, Unit of measure: inches
			// Return value: PdfDocument main class
			Document = new PdfDocument(PaperType.Letter, false, UnitOfMeasure.Inch, FileName);

			// for encryption test
			// Document.SetEncryption(null, null, Permission.All & ~Permission.Print, EncryptionType.Aes128);
			// Document.SetEncryption("userpw", "ownerpw", Permission.All & ~Permission.Print, EncryptionType.Aes128);

			// Debug property
			// By default it is set to false. Use it for debugging only.
			// If this flag is set, PDF objects will not be compressed, font and images will be replaced
			// by text place holder. You can view the file with a text editor but you cannot open it with PDF reader.
			Document.Debug = Debug;

			// add pdf info
			PdfInfo Info = PdfInfo.CreatePdfInfo(Document);
			Info.Title("Article Example");
			Info.Author("Uzi Granot");
			Info.Keywords("PDF, .NET, C#, Library, Document Creator");
			Info.Subject("PDF File Writer II C# Class Library (Version 2.0.0)");

			// add metadata
			// the statement below constructs a PDF metadata object and saves it to the file
			#pragma warning disable CA1806
			new PdfMetadata(Document, "Metadata.xmp");
			#pragma warning restore CA1806

			// Step 2: create resources
			// define font resources
			DefineFontResources();

			// define tiling pattern resources
			DefineTilingPatternResource();

			// Step 3: Add new page
			Page = new PdfPage(Document);

			// Step 4: Add contents to page
			Contents = new PdfContents(Page);

			// Step 5: add graphices and text contents to the contents object
			DrawFrameAndBackgroundWaterMark();
			DrawTwoLinesOfHeading();
			DrawHappyFace();
			DrawBarcode();
			DrawPdf417Barcode();
			DrawImage();
			DrawCombobox();
			DrawTextBox();
			DrawBookOrderForm();

			// Step 6: create pdf file
			Document.CreateFile();

			// start default PDF reader and display the file
			Process Proc = new Process();
			Proc.StartInfo = new ProcessStartInfo(FileName) {UseShellExecute = true};
			Proc.Start();

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Define Font Resources
		// The DefineFontResources method creates all the font resources used
		// in this example.To see all the characters available for any font,
		// press the button “Font Families”. Select a family and view the
		// glyphs defined for each character.To view individual glyph press
		// view or double click.
		////////////////////////////////////////////////////////////////////
		private void DefineFontResources()
			{
			// Define font resources
			// Arguments: PdfDocument class, font family name, font style, embed flag
			// Font style (must be: Regular, Bold, Italic or Bold | Italic) All other styles are invalid.
			// Embed font. If true, the font file will be embedded in the PDF file.
			// If false, the font will not be embedded
			string FontName1 = "Arial";
			string FontName2 = "Times New Roman";

			ArialNormal = PdfFont.CreatePdfFont(Document, FontName1, FontStyle.Regular, true);
			ArialBold = PdfFont.CreatePdfFont(Document, FontName1, FontStyle.Bold, true);
			ArialItalic = PdfFont.CreatePdfFont(Document, FontName1, FontStyle.Italic, true);
			ArialBoldItalic = PdfFont.CreatePdfFont(Document, FontName1, FontStyle.Bold | FontStyle.Italic, true);
			TimesNormal = PdfFont.CreatePdfFont(Document, FontName2, FontStyle.Regular, true);
			Comic = PdfFont.CreatePdfFont(Document, "Comic Sans MS", FontStyle.Bold, true);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Define Tiling Pattern Resource
		//
		// ThThe DefineTilingPatternResource method defines background pattern
		// resource for the example area. The pattern is the word “PdfFileWriter”
		// in white over light blue background. The pattern is made of two lines
		// of repeating the key word. The two lines are skewed by half word length.
		//
		// If you want to find interesting patterns, search the internet for
		// catalogs of companies making floor or wall tiles.p>
		////////////////////////////////////////////////////////////////////
		private void DefineTilingPatternResource()
			{
			// create empty tiling pattern
			WaterMark = new PdfTilingPattern(Document);

			// the pattern will be PdfFileWriter laied out in brick pattern
			string Mark = "PdfFileWriter II";

			// text width and height for Arial bold size 18 points
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialBold, 18.0);
			double TextWidth = TextCtrl.TextWidth(Mark);
			double TextHeight = TextCtrl.LineSpacing;

			// text base line
			double BaseLine = TextCtrl.TextDescent;

			// the overall pattern box (we add text height value as left and right text margin)
			double BoxWidth = TextWidth + 2 * TextHeight;
			double BoxHeight = 4 * TextHeight;
			WaterMark.SetTileBox(BoxWidth, BoxHeight);

			// fill the pattern box with background light blue color
			PdfRectangle Rect = new PdfRectangle(0, 0, BoxWidth, BoxHeight);
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Paint = DrawPaint.Fill;
			DrawCtrl.BackgroundTexture = Color.FromArgb(230, 244, 255);
			WaterMark.DrawGraphics(DrawCtrl, Rect);

			// draw PdfFileWriter at the bottom center of the box
			TextCtrl.TextColor = Color.White;
			TextCtrl.Justify = TextJustify.Center;
			WaterMark.DrawText(TextCtrl, BoxWidth / 2, BaseLine, Mark);

			// adjust base line upward by half height
			BaseLine += BoxHeight / 2;

			// draw the right half of PdfFileWriter shifted left by half width
			WaterMark.DrawText(TextCtrl, 0.0, BaseLine, Mark);

			// draw the left half of PdfFileWriter shifted right by half width
			WaterMark.DrawText(TextCtrl, BoxWidth, BaseLine, Mark);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Draw frame around example area
		//
		// The DrawFrameAndBackgroundWaterMark method draws a frame around
		// the example area with background water mark pattern. The pattern
		// resource was define in the previous subsection.
		////////////////////////////////////////////////////////////////////
		private void DrawFrameAndBackgroundWaterMark()
			{
			// rectangle position: x=1.0", y=1.0", width=6.5", height=9.0"
			PdfRectangle Rect = new PdfRectangle(1, 1, 7.5, 10);
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Paint = DrawPaint.BorderAndFill;
			DrawCtrl.BorderWidth = 0.02;
			DrawCtrl.BorderColor = Color.DarkBlue;
			DrawCtrl.BackgroundTexture = WaterMark;
			Contents.DrawGraphics(DrawCtrl, Rect);

			// draw article name under the frame
			// Note: the \u00a4 is character 164 that was substituted during Font resource definition
			// this character is a solid circle it is normally Unicode 9679 or \u25cf in the Arial family
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialNormal, 9);
			Contents.DrawText(TextCtrl, 1.1, 0.85, "PdfFileWriter II \u25cf PDF File Writer C# Class Library \u25cf Author: Uzi Granot");

			// draw web link to the article
			TextCtrl.Justify = TextJustify.Right;
			TextCtrl.Annotation = new PdfAnnotWebLink(Document, ArticleLink);
			Contents.DrawText(TextCtrl, 7.4, 0.85, "Click to view article");
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Draw two lines of heading
		//
		// The DrawTwoLinesOfHeading method draws two heading lines at the
		// center of the page. The first line is drawing text with outline
		// special effect.
		////////////////////////////////////////////////////////////////////
		private void DrawTwoLinesOfHeading()
			{
			// page heading
			// Arguments: Font: ArialBold, size: 36 points, Position: X = 4.25", Y = 9.5"
			// Text Justify: Center (text center will be at X position)
			// Stoking color: R=128, G=0, B=255 (text outline)
			// Nonstroking color: R=255, G=0, B=128 (text body)
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(Comic, 40);
			TextCtrl.Justify = TextJustify.Center;
			TextCtrl.TextColor = Color.FromArgb(255, 0, 128);
			Contents.DrawText(TextCtrl, 4.25, 9.25, 0.02, Color.FromArgb(128, 0, 255), "PDF FILE WRITER II");

			// Draw second line of heading text
			// arguments: Handwriting font, Font size 30 point, Position X=4.25", Y=9.0"
			// Text Justify: Center (text center will be at X position)
			TextCtrl.FontSize = 30;
			TextCtrl.TextColor = Color.Purple;
			Contents.DrawText(TextCtrl, 4.25, 8.75, "Coding Example");
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Draw Happy Face
		//
		// The DrawHappyFace method is an example of drawing oval and
		// constructing path from a line and Bezier curve.
		////////////////////////////////////////////////////////////////////
		private void DrawHappyFace()
			{
			// save graphics state
			Contents.SaveGraphicsState();

			// translate coordinate origin to the center of the happy face
			Contents.Translate(4.25, 7.5);

			// draw oval control
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Paint = DrawPaint.Fill;
			DrawCtrl.Shape = DrawShape.Oval;
			DrawCtrl.BackgroundTexture = Color.Yellow;

			// draw happy face yellow oval
			PdfRectangle Rect1 = new PdfRectangle(-1.5, -1, 1.5, 1);
			Contents.DrawGraphics(DrawCtrl, Rect1);

			// set line width to 0.2" this is the black circle around the eye
			DrawCtrl.Paint = DrawPaint.BorderAndFill;
			DrawCtrl.BorderWidth = 0.2;

			// eye color is white with black outline circle
			DrawCtrl.BackgroundTexture = Color.White;

			// draw left eye
			PdfRectangle Rect2 = new PdfRectangle(-0.85, -0.1, -0.15, 0.6);
			Contents.DrawGraphics(DrawCtrl, Rect2);

			// draw right eye
			PdfRectangle Rect3 = new PdfRectangle(0.15, -0.1, 0.85, 0.6);
			Contents.DrawGraphics(DrawCtrl, Rect3);

			// mouth color is black
			Contents.SetColorNonStroking(Color.Black);

			// draw mouth by creating a path made of one line and one Bezier curve 
			Contents.MoveTo(new PointD(-0.6, -0.4));
			Contents.LineTo(new PointD(0.6, -0.4));
			Contents.DrawBezier(new PointD(0, -0.8), new PointD(0, -0.8), new PointD(-0.6, -0.4));

			// fill the path with black color
			Contents.SetPaintOp(PaintOp.Fill);

			// restore graphics sate
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Draw Barcode
		// The DrawBarcode method is an example of drawing two barcodes
		// EAN-13 and Code-128
		////////////////////////////////////////////////////////////////////
		private void DrawBarcode()
			{
			// draw EAN13 barcode
			PdfBarcodeEAN13 Barcode1 = new PdfBarcodeEAN13("1234567890128");
			PdfDrawBarcodeCtrl BarcodeCtrl = new PdfDrawBarcodeCtrl();
			BarcodeCtrl.NarrowBarWidth = 0.012;
			BarcodeCtrl.Height = 0.75;
			BarcodeCtrl.TextCtrl = new PdfDrawTextCtrl(ArialNormal, 8.0);
			Contents.DrawBarcode(BarcodeCtrl, 1.3, 7.05, Barcode1);

			// create QRCode barcode
			PdfQREncoder QREncoder = new PdfQREncoder();

			// set error correction code
			QREncoder.ErrorCorrection = ErrorCorrection.M;

			// set module size in pixels
			QREncoder.ModuleSize = 1;

			// set quiet zone in pixels
			QREncoder.QuietZone = 4;

			// encode your text or byte array
			QREncoder.Encode(ArticleLink);

			// convert QRCode to black and white image
			PdfImage BarcodeImage = new PdfImage(Document);
			BarcodeImage.LoadImage(QREncoder);

			// draw image (height is the same as width for QRCode)
			Contents.DrawImage(BarcodeImage, 6.0, 6.8, 1.2);

			// create annotation object
			PdfAnnotWebLink WebLink = new PdfAnnotWebLink(Document, ArticleLink);
			WebLink.AnnotRect = new PdfRectangle(6.0, 6.8, 7.2, 8.0);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Draw PDF417 Barcode
		////////////////////////////////////////////////////////////////////
		private void DrawPdf417Barcode()
			{
			// create PDF417 barcode
			Pdf417Encoder Pdf417 = new Pdf417Encoder();
			Pdf417.DefaultDataColumns = 3;
			Pdf417.Encode(ArticleLink);
			Pdf417.WidthToHeightRatio(2.5);

			// convert Pdf417 to black and white image
			PdfImage BarcodeImage = new PdfImage(Document);
			BarcodeImage.LoadImage(Pdf417);

			// draw image
			Contents.DrawImage(BarcodeImage, 1.1, 5.2, 2.5);

			// create annotation object
			PdfAnnotWebLink WebLink = new PdfAnnotWebLink(Document, ArticleLink);

			// define a web link area coinsiding with the qr code
			double Height = 2.5 * Pdf417.ImageHeight / Pdf417.ImageWidth;
			WebLink.AnnotRect = new PdfRectangle(1.1, 5.2, 1.1 + 2.5, 5.2 + Height);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Draw image and clip it
		//
		// The DrawImage method is an example of drawing an image.
		// The PdfFileWriter support drawing images stored in all image files
		// supported by Bitmap class and Metafile class. The ImageFormat class
		// defines all image types. The JPEG image file type is the native
		// image format of the PDF file. If you call the PdfImage constructor
		// with JPEG file, the program copies the file as is into the PDF
		// file. If you call the PdfImage constructor with any other type of
		// image file, the program converts it into JPEG file. In order to keep
		// the PDF file size as small as possible, make sure your image file
		// resolution is not unreasonably high.
		//
		// The PdfImage class loads the image and calculates maximum size that
		// can fit a given image size in user coordinates and preserve the
		// original aspect ratio.Before drawing the image we create an oval
		// clipping path to clip the image.
		////////////////////////////////////////////////////////////////////
		private void DrawImage()
			{
			// define local image resources
			// resolution 96 pixels per inch, image quality 50%
			PdfImage Image1 = new PdfImage(Document);
			Image1.Resolution = 96.0;
			Image1.ImageQuality = 50;
			Image1.LoadImage("TestImage.jpg");

			// adjust image size and preserve aspect ratio
			PdfRectangle ImageArea = new PdfRectangle(3.6, 4.8, 5.55, 6.5);
			PdfRectangle NewArea = PdfImageSizePos.ImageArea(Image1, ImageArea, ContentAlignment.MiddleCenter);

			// clipping path
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Shape = DrawShape.Oval;
			DrawCtrl.Paint = DrawPaint.Border;
			DrawCtrl.BorderWidth = 0.04;
			DrawCtrl.BorderColor = Color.DarkBlue;
			DrawCtrl.BackgroundTexture = Image1;

			// draw image
			Contents.DrawGraphics(DrawCtrl, NewArea);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Draw combo box for color selection
		////////////////////////////////////////////////////////////////////
		private void DrawCombobox()
			{
			// define origin position 
			double PosX = 5.8;
			double PosY = 6.3;

			// fixed text font
			PdfFont FixedTextFont = PdfFont.CreatePdfFont(Document, "Times New Roman", FontStyle.Regular);
			PdfDrawTextCtrl FixedTextCtrl = new PdfDrawTextCtrl(FixedTextFont, 12);

			// data entry font
			PdfFont FieldTextFont = PdfFont.CreatePdfFont(Document, "Arial", FontStyle.Regular);
			PdfDrawTextCtrl FieldTextCtrl = new PdfDrawTextCtrl(FieldTextFont, 12);

			// set all ascii range is included
			FieldTextFont.SetCharRangeActive(' ', '~');

			// combo box select color title
			PosY -= FixedTextCtrl.TextAscent;
			Contents.DrawText(FixedTextCtrl, PosX, PosY, "PDF Form Example");
			PosY -= FixedTextCtrl.LineSpacing;
			Contents.DrawText(FixedTextCtrl, PosX, PosY, "Select color");

			string[] ColorChoices = { "Red", "Orange", "Yellow", "Green", "Blue", "Indigo", "Violet" };

			// frame around data entry field
			PdfDrawCtrl DrawFrame = new PdfDrawCtrl();
			DrawFrame.Paint = DrawPaint.BorderAndFill;
			DrawFrame.BorderWidth = 0.01;
			DrawFrame.BorderColor = Color.LightGray;
			DrawFrame.BackgroundTexture = Color.FromArgb(255, 255, 210);

			// this document has fillable fields
			// define acro form to control the fields
			PdfAcroForm AcroForm = PdfAcroForm.CreateAcroForm(Document);

			// combo box select color data entry
			PosY -= FixedTextCtrl.TextDescent + FieldTextCtrl.LineSpacing + 0.04;
			PdfRectangle ComboColorRect = new PdfRectangle(PosX, PosY, PosX + 1.5, PosY + FieldTextCtrl.LineSpacing);
			Contents.DrawGraphics(DrawFrame, ComboColorRect.AddMargin(0.04));

			// combo box field
			PdfAcroComboBoxField Field = new PdfAcroComboBoxField(AcroForm, "SelectColor");
			Field.AnnotPage = Page;
			Field.AnnotRect = ComboColorRect;
			Field.TextCtrl = FieldTextCtrl;
			Field.BackgroundColor = Color.FromArgb(240, 240, 255);
			Field.Edit = true;
			Field.Sort = true;
			Field.FieldValue = ColorChoices[0];
			Field.Items = ColorChoices;

			// combo box field appearance
			Field.DrawComboBox();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Draw example of a text box
		// The DrawTextBox method is an example of using the TextBox class.
		// The TextBox class formats text to fit within a column. The text
		// can be drawn using a verity of font's styles and sizes.
		////////////////////////////////////////////////////////////////////
		private void DrawTextBox()
			{
			// save graphics state
			Contents.SaveGraphicsState();

			// translate origin to PosX=1.1" and PosY=1.1" this is the bottom left corner of the text box example
			Contents.Translate(1.1, 1.1);

			// Define constants
			// Box width 3.25"
			// Box height is 3.65"
			// Normal font size is 9.0 points.
			const double Width = 3.15;
			const double Height = 3.65;
			const double FontSize = 9.0;

			// Create text box object width 3.25"
			// First line indent of 0.25"
			PdfTextBox Box = new PdfTextBox(Width, 0.25);

			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialNormal, FontSize);

			// add text to the text box
			Box.AddText(TextCtrl,
				"This area is an example of displaying text that is too long to fit within a fixed width " +
				"area. The text is displayed justified to right edge. You define a text box with the required " +
				"width and first line indent. You add text to this box. The box will divide the text into " +
				"lines. Each line is made of segments of text. For each segment, you define font, font " +
				"size, drawing style and color. After loading all the text, the program will draw the formatted text.\n");

			PdfDrawTextCtrl TimesTextCtrl = new PdfDrawTextCtrl(TimesNormal, FontSize + 1);
			Box.AddText(TimesTextCtrl, "Example of multiple fonts: Times New Roman, ");

			PdfDrawTextCtrl ComicTextCtrl = new PdfDrawTextCtrl(Comic, FontSize);
			Box.AddText(ComicTextCtrl, "Comic Sans MS, ");

			Box.AddText(TextCtrl, "Example of regular, ");

			PdfDrawTextCtrl BoldTextCtrl = new PdfDrawTextCtrl(ArialBold, FontSize);
			Box.AddText(BoldTextCtrl, "bold, ");

			PdfDrawTextCtrl ItalicTextCtrl = new PdfDrawTextCtrl(ArialItalic, FontSize);
			Box.AddText(ItalicTextCtrl, "italic, ");

			PdfDrawTextCtrl BoldItalicTextCtrl = new PdfDrawTextCtrl(ArialBoldItalic, FontSize);
			Box.AddText(BoldItalicTextCtrl, "bold plus italic. ");

			TextCtrl.FontSize = FontSize - 2;
			Box.AddText(TextCtrl, "Arial size 7, ");

			TextCtrl.FontSize = FontSize - 1;
			Box.AddText(TextCtrl, "size 8, ");

			TextCtrl.FontSize = FontSize;
			Box.AddText(TextCtrl, "size 9, ");

			TextCtrl.FontSize = FontSize + 1;
			Box.AddText(TextCtrl, "size 10. ");

			TextCtrl.FontSize = FontSize;
			TextCtrl.DrawStyle = DrawStyle.Underline;
			Box.AddText(TextCtrl, "Underline, ");

			TextCtrl.DrawStyle = DrawStyle.Strikeout;
			Box.AddText(TextCtrl, "Strikeout. ");

			TextCtrl.DrawStyle = DrawStyle.Normal;
			Box.AddText(TextCtrl, "Subscript H");

			TextCtrl.DrawStyle = DrawStyle.Subscript;
			Box.AddText(TextCtrl, "2");

			TextCtrl.DrawStyle = DrawStyle.Normal;
			Box.AddText(TextCtrl, "O. Superscript A");

			TextCtrl.DrawStyle = DrawStyle.Superscript;
			Box.AddText(TextCtrl, "2");

			TextCtrl.DrawStyle = DrawStyle.Normal;
			Box.AddText(TextCtrl, "+B");

			TextCtrl.DrawStyle = DrawStyle.Superscript;
			Box.AddText(TextCtrl, "2");

			TextCtrl.DrawStyle = DrawStyle.Normal;
			Box.AddText(TextCtrl, "=C");

			TextCtrl.DrawStyle = DrawStyle.Superscript;
			Box.AddText(TextCtrl, "2");

			TextCtrl.DrawStyle = DrawStyle.Normal;
			Box.AddText(TextCtrl, "\n");

			ComicTextCtrl.TextColor = Color.Red;
			Box.AddText(ComicTextCtrl, "Some color, ");

			ComicTextCtrl.TextColor = Color.Green;
			Box.AddText(ComicTextCtrl, "green, ");

			ComicTextCtrl.TextColor = Color.Blue;
			Box.AddText(ComicTextCtrl, "blue, ");

			ComicTextCtrl.TextColor = Color.Orange;
			Box.AddText(ComicTextCtrl, "orange, ");

			ComicTextCtrl.TextColor = Color.Purple;
			ComicTextCtrl.DrawStyle = DrawStyle.Underline;
			Box.AddText(ComicTextCtrl, "and purple.\n");

			TextCtrl.TextColor = Color.Black;
			Box.AddText(TextCtrl, "Support for non-Latin letters: ");
			Box.AddText(TextCtrl, PdfContents.ReverseString("עברית"));
			Box.AddText(TextCtrl, "АБВГДЕ");
			Box.AddText(TextCtrl, "αβγδεζ");

			Box.AddText(TextCtrl, "\n");

			// Draw the text box
			// Text left edge is at zero (note: origin was translated to 1.1") 
			// The top text base line is at Height less first line ascent.
			// Text drawing is limited to vertical coordinate of zero.
			// First line to be drawn is line zero.
			// After each line add extra 0.015".
			// After each paragraph add extra 0.05"
			// Stretch all lines to make smooth right edge at box width of 3.15"
			// After all lines are drawn, PosY will be set to the next text line after the box's last paragraph
			double PosY = Height;
			Contents.DrawText(0.0, ref PosY, 0.0, 0, 0.015, 0.05, TextBoxJustify.FitToWidth, Box);

			// Create text box object width 3.25"
			// No first line indent
			Box = new PdfTextBox(Width);

			// Add text as before.
			// No extra line spacing.
			// No right edge adjustment
			Box.AddText(TextCtrl,
				"In the examples above this area the text box was set for first line indent of " +
				"0.25 inches. This paragraph has zero first line indent and no right justify.");
			Contents.DrawText(0.0, ref PosY, 0.0, 0, 0.01, 0.05, TextBoxJustify.Left, Box);

			// Create text box object width 2.75
			// First line hanging indent of 0.5"
			Box = new PdfTextBox(Width - 0.5, -0.5);

			// Add text
			Box.AddText(TextCtrl,
				"This paragraph is set to first line hanging indent of 0.5 inches. " +
				"The left margin of this paragraph is 0.5 inches.");

			// Draw the text
			// left edge at 0.5"
			Contents.DrawText(0.5, ref PosY, 0.0, 0, 0.01, 0.05, TextBoxJustify.Left, Box);

			// restore graphics state
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Draw example of order form
		// The DrawBookOrderForm method is an example of an order entry form
		// or an invoice. It is an example for data table support. It
		// demonstrate the use of PdfTable, PdfTableCell and PdfTableStyle
		// classes.
		////////////////////////////////////////////////////////////////////
		private void DrawBookOrderForm()
			{
			// Define constants to make the code readable
			const double Left = 4.35;
			const double Top = 4.65;
			const double Bottom = 1.1;
			const double Right = 7.4;
			const double MarginHor = 0.04;
			const double MarginVer = 0.04;
			const double FrameWidth = 0.015;
			const double GridWidth = 0.01;

			// normal text control
			const double FontSize = 9.0;
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialNormal, FontSize);

			// column widths
			double ColWidthPrice = TextCtrl.TextWidth("9999.99") + 2.0 * MarginHor;
			double ColWidthQty = TextCtrl.TextWidth("Qty") + 2.0 * MarginHor;
			double ColWidthDesc = Right - Left - FrameWidth - 3 * GridWidth - 2 * ColWidthPrice - ColWidthQty;

			// define table
			PdfTable Table = new PdfTable(Page, Contents, TextCtrl);
			Table.TableArea = new PdfRectangle(Left, Bottom, Right, Top);
			Table.SetColumnWidth(new double[] { ColWidthDesc, ColWidthPrice, ColWidthQty, ColWidthPrice });

			// define borders
			Table.Borders.SetAllBorders(FrameWidth, GridWidth);

			// margin
			PdfRectangle Margin = new PdfRectangle(MarginHor, MarginVer);

			// default header style
			Table.DefaultHeaderStyle.Margin = Margin;
			Table.DefaultHeaderStyle.BackgroundColor = Color.FromArgb(255, 196, 255);
			Table.DefaultHeaderStyle.Alignment = ContentAlignment.MiddleRight;

			// private header style for description
			Table.Header[0].Style = Table.HeaderStyle;
			Table.Header[0].Style.Alignment = ContentAlignment.MiddleLeft;

			// table heading
			Table.Header[0].Value = "Description";
			Table.Header[1].Value = "Price";
			Table.Header[2].Value = "Qty";
			Table.Header[3].Value = "Total";

			// default style
			Table.DefaultCellStyle.Margin = Margin;

			// description column style
			Table.Cell[0].Style = Table.CellStyle;
			Table.Cell[0].Style.MultiLineText = true;

			// qty column style
			Table.Cell[2].Style = Table.CellStyle;
			Table.Cell[2].Style.Alignment = ContentAlignment.BottomRight;

			Table.DefaultCellStyle.Format = "#,##0.00";
			Table.DefaultCellStyle.Alignment = ContentAlignment.BottomRight;

			PdfDrawTextCtrl TextCtrlBold = new PdfDrawTextCtrl(ArialBold, FontSize);
			TextCtrlBold.Justify = TextJustify.Center;
			TextCtrlBold.TextColor = Color.Purple;
			Contents.DrawText(TextCtrlBold, 0.5 * (Left + Right), Top + MarginVer + Table.DefaultCellStyle.TextDescent, "Example of PdfTable support");

			// reset order total
			double Total = 0;

			// loop for all items in the order
			// Order class is a atabase simulation for this example
			foreach(Order Book in Order.OrderList)
				{
				Table.Cell[0].Value = Book.Title + ". By: " + Book.Authors;
				Table.Cell[1].Value = Book.Price;
				Table.Cell[2].Value = Book.Qty;
				Table.Cell[3].Value = Book.Total;
				Table.DrawRow();

				// accumulate total
				Total += Book.Total;
				}
			Table.Close();

			// save graphics state
			Contents.SaveGraphicsState();

			// form line width 0.01"
			Contents.SetLineWidth(FrameWidth);
			Contents.SetLineCap(PdfLineCap.Square);

			// draw total before tax
			double[] ColumnPosition = Table.ColumnPosition;
			double TotalDesc = ColumnPosition[3] - MarginHor;
			double TotalValue = ColumnPosition[4] - MarginHor;
			double PosY = Table.RowTopPosition - 2.0 * MarginVer - Table.DefaultCellStyle.TextAscent;

			TextCtrl.Justify = TextJustify.Right;
			Contents.DrawText(TextCtrl, TotalDesc, PosY, "Total before tax");
			Contents.DrawText(TextCtrl, TotalValue, PosY, Total.ToString("#.00"));

			// draw tax (Ontario Canada HST)
			PosY -= Table.DefaultCellStyle.LineSpacing;
			Contents.DrawText(TextCtrl, TotalDesc, PosY, "Tax (13%)");
			double Tax = Math.Round(0.13 * Total, 2, MidpointRounding.AwayFromZero);
			Contents.DrawText(TextCtrl, TotalValue, PosY, Tax.ToString("#.00"));

			// draw total line
			PosY -= Table.DefaultCellStyle.TextDescent + 0.5 * MarginVer;
			LineD TotalLine = new LineD(ColumnPosition[3], PosY, ColumnPosition[4], PosY);
			Contents.DrawLine(TotalLine);

			// draw final total
			PosY -= Table.DefaultCellStyle.TextAscent + 0.5 * MarginVer;
			Contents.DrawText(TextCtrl, TotalDesc, PosY, "Total payable");
			Total += Tax;
			Contents.DrawText(TextCtrl, TotalValue, PosY, Total.ToString("#.00"));

			PosY -= Table.DefaultCellStyle.TextDescent + MarginVer;
			LineD VertLine1 = new LineD(ColumnPosition[0], Table.RowTopPosition, ColumnPosition[0], PosY);
			Contents.DrawLine(VertLine1);
			LineD HorLine = new LineD(ColumnPosition[0], PosY, ColumnPosition[4], PosY);
			Contents.DrawLine(HorLine);
			LineD VertLine2 = new LineD(ColumnPosition[4], Table.RowTopPosition, ColumnPosition[4], PosY);
			Contents.DrawLine(VertLine2);

			// restore graphics state
			Contents.RestoreGraphicsState();
			return;
			}
		}

	// Simulation of book order database
	internal class Order
		{
		internal string Title;
		internal string Authors;
		internal double Price;
		internal int Qty;

		internal double Total
			{
			get
				{
				return Price * Qty;
				}
			}

		internal Order
				(
				string Title,
				string Authors,
				double Price,
				int Qty
				)
			{
			this.Title = Title;
			this.Authors = Authors;
			this.Price = Price;
			this.Qty = Qty;
			return;
			}

		internal static Order[] OrderList = new Order[]
			{
			new Order("Current Trends in Theoritical Computer Science: Algorithms and Complexity", "Paun", 123.5, 2),
			new Order("Theoretical Computer Science: Introduction to Automata, Computability, Complexity, Algorithmics, Randomization", "Juraj Hromkovic", 76.81, 1),
			new Order("Discrete Mathematics for Computer Science (with Student Solutions Manual CD-ROM)", "Gary Haggard, John Schlipf and Sue Whitesides", 229.88, 1),
			};
		}
	}
