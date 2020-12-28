/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	ArticleExample
//	Produce PDF file when the Artice Example is clicked.
//
//	Uzi Granot
//	Version: 1.0
//	Date: April 1, 2013
//	Copyright (C) 2013-2019 Uzi Granot. All Rights Reserved
//
//	PdfFileWriter C# class library and TestPdfFileWriter test/demo
//  application are free software.
//	They is distributed under the Code Project Open License (CPOL).
//	The document PdfFileWriterReadmeAndLicense.pdf contained within
//	the distribution specify the license agreement and other
//	conditions and notes. You must read this document and agree
//	with the conditions specified in order to use this software.
//
//	For version history please refer to PdfDocument.cs
//
/////////////////////////////////////////////////////////////////////

using PdfFileWriter;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace TestPdfFileWriter
{
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

	private static string ArticleLink = "http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version";

	////////////////////////////////////////////////////////////////////
	// Create article's example test PDF document
	////////////////////////////////////////////////////////////////////
	
	public void Test
			(
			bool Debug,
			string	FileName
			)
		{
		// Step 1: Create empty document
		// Arguments: page width: 8.5”, page height: 11”, Unit of measure: inches
		// Return value: PdfDocument main class
		Document = new PdfDocument(PaperType.Letter, false, UnitOfMeasure.Inch, FileName);

		// for encryption test
//		Document.SetEncryption(null, null, Permission.All & ~Permission.Print, EncryptionType.Aes128);

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
		Info.Subject("PDF File Writer C# Class Library (Version 1.27.0)");

		// add metadata
		new PdfMetadata(Document, "Metadata.xmp");

		// Step 2: create resources
		// define font resources
		DefineFontResources();

		// define tiling pattern resources
		DefineTilingPatternResource();

		// Step 3: Add new page
		Page = new PdfPage(Document);

		// Step 4:Add contents to page
		Contents = new PdfContents(Page);

		// Step 5: add graphices and text contents to the contents object
		DrawFrameAndBackgroundWaterMark();
		DrawTwoLinesOfHeading();
		DrawHappyFace();
		DrawBarcode();
		DrawPdf417Barcode();
		DrawImage();
		DrawChart();
		DrawTextBox();
		DrawBookOrderForm();

		// Step 6: create pdf file
		Document.CreateFile();

		// start default PDF reader and display the file
		Process Proc = new Process();
	    Proc.StartInfo = new ProcessStartInfo(FileName);
	    Proc.Start();

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Define Font Resources
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
//		Comic = PdfFont.CreatePdfFont(Document, "ＭＳ ゴシック", /* "MS UI Gothic",*/ FontStyle.Bold, true); // embeded fails
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Define Tiling Pattern Resource
	////////////////////////////////////////////////////////////////////

	private void DefineTilingPatternResource()
		{
		// create empty tiling pattern
		WaterMark = new PdfTilingPattern(Document);

		// the pattern will be PdfFileWriter laied out in brick pattern
		string Mark = "PdfFileWriter";

		// text width and height for Arial bold size 18 points
		double FontSize = 18.0;
		double TextWidth = ArialBold.TextWidth(FontSize, Mark);
		double TextHeight = ArialBold.LineSpacing(FontSize);

		// text base line
		double BaseLine = ArialBold.DescentPlusLeading(FontSize);

		// the overall pattern box (we add text height value as left and right text margin)
		double BoxWidth = TextWidth + 2 * TextHeight;
		double BoxHeight = 4 * TextHeight;
		WaterMark.SetTileBox(BoxWidth, BoxHeight);

		// save graphics state
		WaterMark.SaveGraphicsState();

		// fill the pattern box with background light blue color
		WaterMark.SetColorNonStroking(Color.FromArgb(230, 244, 255));
		WaterMark.DrawRectangle(0, 0, BoxWidth, BoxHeight, PaintOp.Fill);

		// set fill color for water mark text to white
		WaterMark.SetColorNonStroking(Color.White);

		// draw PdfFileWriter at the bottom center of the box
		WaterMark.DrawText(ArialBold, FontSize, BoxWidth / 2, BaseLine, TextJustify.Center, Mark);

		// adjust base line upward by half height
		BaseLine += BoxHeight / 2;

		// draw the right half of PdfFileWriter shifted left by half width
		WaterMark.DrawText(ArialBold, FontSize, 0.0, BaseLine, TextJustify.Center, Mark);

		// draw the left half of PdfFileWriter shifted right by half width
		WaterMark.DrawText(ArialBold, FontSize, BoxWidth, BaseLine, TextJustify.Center, Mark);

		// restore graphics state
		WaterMark.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw frame around example area
	////////////////////////////////////////////////////////////////////

	private void DrawFrameAndBackgroundWaterMark()
		{
		// save graphics state
		Contents.SaveGraphicsState();

		// Draw frame around the page
		// Set line width to 0.02"
		Contents.SetLineWidth(0.02);

		// set frame color dark blue
		Contents.SetColorStroking(Color.DarkBlue);

		// use water mark tiling pattern to fill the frame
		Contents.SetPatternNonStroking(WaterMark);

		// rectangle position: x=1.0", y=1.0", width=6.5", height=9.0"
		Contents.DrawRectangle(1.0, 1.0, 6.5, 9.0, PaintOp.CloseFillStroke);

		// restore graphics sate
		Contents.RestoreGraphicsState();

		// draw article name under the frame
		// Note: the \u00a4 is character 164 that was substituted during Font resource definition
		// this character is a solid circle it is normally Unicode 9679 or \u25cf in the Arial family
		Contents.DrawText(ArialNormal, 9.0, 1.1, 0.85, "PdfFileWriter \u25cf PDF File Writer C# Class Library \u25cf Author: Uzi Granot");

		// draw web link to the article
		Contents.DrawWebLink(Page, ArialNormal, 9.0, 7.4, 0.85, TextJustify.Right,
			DrawStyle.Underline, Color.Blue, "Click to view article", ArticleLink);

		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw heading
	////////////////////////////////////////////////////////////////////

	private void DrawTwoLinesOfHeading()
		{
		// page heading
		// Arguments: Font: ArialBold, size: 36 points, Position: X = 4.25", Y = 9.5"
		// Text Justify: Center (text center will be at X position)
		// Stoking color: R=128, G=0, B=255 (text outline)
		// Nonstroking color: R=255, G=0, B=128 (text body)
		Contents.DrawText(Comic, 40.0, 4.25, 9.25, TextJustify.Center, 0.02, Color.FromArgb(128, 0, 255), Color.FromArgb(255, 0, 128), "PDF FILE WRITER");

		// save graphics state
		Contents.SaveGraphicsState();

		// change nonstroking (fill) color to purple
		Contents.SetColorNonStroking(Color.Purple);

		// Draw second line of heading text
		// arguments: Handwriting font, Font size 30 point, Position X=4.25", Y=9.0"
		// Text Justify: Center (text center will be at X position)
		Contents.DrawText(Comic, 30.0, 4.25, 8.75, TextJustify.Center, "Example");
//		Contents.DrawText(Comic, 30.0, 4.25, 8.75, TextJustify.Center, "コレハ日本語の見本です。"); // "Example");

		// restore graphics sate (non stroking color will be restored to default)
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Happy Face
	////////////////////////////////////////////////////////////////////

	private void DrawHappyFace()
		{
		// save graphics state
		Contents.SaveGraphicsState();

		// translate coordinate origin to the center of the happy face
		Contents.Translate(4.25, 7.5);

		// change nonstroking (fill) color to yellow
		Contents.SetColorNonStroking(Color.Yellow);

		// draw happy face yellow oval
		Contents.DrawOval(-1.5, -1.0, 3.0, 2.0, PaintOp.Fill);

		// set line width to 0.2" this is the black circle around the eye
		Contents.SetLineWidth(0.2);

		// eye color is white with black outline circle
		Contents.SetColorNonStroking(Color.White);
		Contents.SetColorStroking(Color.Black);

		// draw eyes
		Contents.DrawOval(-0.75, 0.0, 0.5, 0.5, PaintOp.CloseFillStroke);
		Contents.DrawOval(0.25, 0.0, 0.5, 0.5, PaintOp.CloseFillStroke);

		// mouth color is black
		Contents.SetColorNonStroking(Color.Black);

		// draw mouth by creating a path made of one line and one Bezier curve 
		Contents.MoveTo(-0.6, -0.4);
		Contents.LineTo(0.6, -0.4);
		Contents.DrawBezier(0.0, -0.8, 0, -0.8, -0.6, -0.4);

		// fill the path with black color
		Contents.SetPaintOp(PaintOp.Fill);

		// restore graphics sate
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Barcode
	////////////////////////////////////////////////////////////////////

	private void DrawBarcode()
		{
		// save graphics state
		Contents.SaveGraphicsState();

		// draw EAN13 barcode
		BarcodeEAN13 Barcode1 = new BarcodeEAN13("1234567890128");
		Contents.DrawBarcode(1.3, 7.05, 0.012, 0.75, Barcode1, ArialNormal, 8.0);

		// create QRCode barcode
		QREncoder QREncoder = new QREncoder();

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

		// define a web link area coinsiding with the qr code
		Page.AddWebLink(6.0, 6.8, 7.2, 8.0, ArticleLink);

		// restore graphics sate
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Barcode
	////////////////////////////////////////////////////////////////////

	private void DrawPdf417Barcode()
		{
		// save graphics state
		Contents.SaveGraphicsState();

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

		// define a web link area coinsiding with the qr code
		double Height = 2.5 * Pdf417.ImageHeight / Pdf417.ImageWidth;
		Page.AddWebLink(1.1, 5.2, 1.1 + 2.5, 5.2 + Height, ArticleLink);

		// restore graphics sate
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw image and clip it
	////////////////////////////////////////////////////////////////////

	private void DrawImage()
		{
		// define local image resources
		// resolution 96 pixels per inch, image quality 50%
		PdfImage Image1 = new PdfImage(Document);
		Image1.Resolution = 96.0;
		Image1.ImageQuality = 50;
		Image1.LoadImage("TestImage.jpg");

		// save graphics state
		Contents.SaveGraphicsState();

		// translate coordinate origin to the center of the picture
		Contents.Translate(3.75, 5.0);

		// adjust image size and preserve aspect ratio
		PdfRectangle NewSize = Image1.ImageSizePosition(1.75, 1.5, ContentAlignment.MiddleCenter);

		// clipping path
		Contents.DrawOval(NewSize.Left, NewSize.Bottom, NewSize.Width, NewSize.Height, PaintOp.ClipPathEor);

		// draw image
		Contents.DrawImage(Image1, NewSize.Left, NewSize.Bottom, NewSize.Width, NewSize.Height);

		// restore graphics state
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw chart
	////////////////////////////////////////////////////////////////////

	private void DrawChart()
		{
		// save graphics state
		Contents.SaveGraphicsState();

		// create chart
		Chart PieChart = PdfChart.CreateChart(Document, 1.8, 1.5, 300.0);

		// create PdfChart object with Chart object
		PdfChart PiePdfChart = new PdfChart(Document, PieChart);
		PiePdfChart.SaveAs = SaveImageAs.IndexedImage;

		// make sure we have good quality image
		PieChart.AntiAliasing = AntiAliasingStyles.None; //.All;

		// set colors
		PieChart.BackColor = Color.FromArgb(220, 220, 255);
		PieChart.Palette = ChartColorPalette.BrightPastel;

		// default font
		Font DefaultFont = PiePdfChart.CreateFont("Verdana", FontStyle.Regular, 0.05, FontSizeUnit.UserUnit);
		Font TitleFont = PiePdfChart.CreateFont("Verdana", FontStyle.Bold, 0.07, FontSizeUnit.UserUnit);

		// title (font size is 0.25 inches)
		Title Title1 = new Title("Pie Chart Example", Docking.Top, TitleFont, Color.Purple);
		PieChart.Titles.Add(Title1);

		// legend
		Legend Legend1 = new Legend();
		PieChart.Legends.Add(Legend1);
		Legend1.BackColor = Color.FromArgb(230, 230, 255);
		Legend1.Docking = Docking.Bottom;
		Legend1.Font = DefaultFont;

		// chart area
		ChartArea ChartArea1 = new ChartArea();
		PieChart.ChartAreas.Add(ChartArea1);

		// chart area background color
		ChartArea1.BackColor = Color.FromArgb(255, 200, 255);

		// series 1
		Series Series1 = new Series();
		PieChart.Series.Add(Series1);
		Series1.ChartType = SeriesChartType.Pie;
		Series1.Font = DefaultFont;
		Series1.IsValueShownAsLabel = true;
		Series1.LabelFormat = "{0} %";

		// series values
		Series1.Points.Add(22.0);
		Series1.Points[0].LegendText = "Apple";
		Series1.Points.Add(27.0);
		Series1.Points[1].LegendText = "Banana";
		Series1.Points.Add(33.0);
		Series1.Points[2].LegendText = "Orange";
		Series1.Points.Add(18.0);
		Series1.Points[3].LegendText = "Grape";

		Contents.DrawChart(PiePdfChart, 5.6, 5.0);

		// restore graphics state
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw example of a text box
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
		TextBox Box = new TextBox(Width, 0.25);

		// add text to the text box
		Box.AddText(ArialNormal, FontSize,
			"This area is an example of displaying text that is too long to fit within a fixed width " +
			"area. The text is displayed justified to right edge. You define a text box with the required " +
			"width and first line indent. You add text to this box. The box will divide the text into " + 
			"lines. Each line is made of segments of text. For each segment, you define font, font " +
			"size, drawing style and color. After loading all the text, the program will draw the formatted text.\n");
		Box.AddText(TimesNormal, FontSize + 1.0, "Example of multiple fonts: Times New Roman, ");
		Box.AddText(Comic, FontSize, "Comic Sans MS, ");
		Box.AddText(ArialNormal, FontSize, "Example of regular, ");
		Box.AddText(ArialBold, FontSize, "bold, ");
		Box.AddText(ArialItalic, FontSize, "italic, ");
		Box.AddText(ArialBoldItalic, FontSize, "bold plus italic. ");
		Box.AddText(ArialNormal, FontSize - 2.0, "Arial size 7, ");
		Box.AddText(ArialNormal, FontSize - 1.0, "size 8, ");
		Box.AddText(ArialNormal, FontSize, "size 9, ");
		Box.AddText(ArialNormal, FontSize + 1.0, "size 10. ");
		Box.AddText(ArialNormal, FontSize, DrawStyle.Underline, "Underline, ");
		Box.AddText(ArialNormal, FontSize, DrawStyle.Strikeout, "Strikeout. ");
		Box.AddText(ArialNormal, FontSize, "Subscript H");
		Box.AddText(ArialNormal, FontSize, DrawStyle.Subscript, "2");
		Box.AddText(ArialNormal, FontSize, "O. Superscript A");
		Box.AddText(ArialNormal, FontSize, DrawStyle.Superscript, "2");
		Box.AddText(ArialNormal, FontSize, "+B");
		Box.AddText(ArialNormal, FontSize, DrawStyle.Superscript, "2");
		Box.AddText(ArialNormal, FontSize, "=C");
		Box.AddText(ArialNormal, FontSize, DrawStyle.Superscript, "2");
		Box.AddText(ArialNormal, FontSize, "\n");
		Box.AddText(Comic, FontSize, Color.Red, "Some color, ");
		Box.AddText(Comic, FontSize, Color.Green, "green, ");
		Box.AddText(Comic, FontSize, Color.Blue, "blue, ");
		Box.AddText(Comic, FontSize, Color.Orange, "orange, ");
		Box.AddText(Comic, FontSize, DrawStyle.Underline, Color.Purple, "and purple.\n");
		Box.AddText(ArialNormal, FontSize, "Support for non-Latin letters: ");
		Box.AddText(ArialNormal, FontSize, Contents.ReverseString( "עברית"));
		Box.AddText(ArialNormal, FontSize, "АБВГДЕ");
		Box.AddText(ArialNormal, FontSize, "αβγδεζ");

		Box.AddText(ArialNormal, FontSize, "\n");

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
		Box = new TextBox(Width);

		// Add text as before.
		// No extra line spacing.
		// No right edge adjustment
		Box.AddText(ArialNormal, FontSize,
			"In the examples above this area the text box was set for first line indent of " +
			"0.25 inches. This paragraph has zero first line indent and no right justify.");
		Contents.DrawText(0.0, ref PosY, 0.0, 0, 0.01, 0.05, TextBoxJustify.Left, Box);

		// Create text box object width 2.75
		// First line hanging indent of 0.5"
		Box = new TextBox(Width - 0.5, -0.5);

		// Add text
		Box.AddText(ArialNormal, FontSize,
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
	////////////////////////////////////////////////////////////////////

	private void DrawBookOrderForm()
		{
		// Define constants to make the code readable
		const double Left = 4.35;
		const double Top = 4.65;
		const double Bottom = 1.1;
		const double Right = 7.4;
		const double FontSize = 9.0;
		const double MarginHor = 0.04;
		const double MarginVer = 0.04;
		const double FrameWidth = 0.015;
		const double GridWidth = 0.01;

		// column widths
		double ColWidthPrice = ArialNormal.TextWidth(FontSize, "9999.99") + 2.0 * MarginHor;
		double ColWidthQty = ArialNormal.TextWidth(FontSize, "Qty") + 2.0 * MarginHor;
		double ColWidthDesc = Right - Left - FrameWidth - 3 * GridWidth - 2 * ColWidthPrice - ColWidthQty;

		// define table
		PdfTable Table = new PdfTable(Page, Contents, ArialNormal, FontSize);
		Table.TableArea = new PdfRectangle(Left, Bottom, Right, Top);
		Table.SetColumnWidth(new double[] {ColWidthDesc, ColWidthPrice, ColWidthQty, ColWidthPrice});

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

		Contents.DrawText(ArialBold, FontSize, 0.5 * (Left + Right), Top + MarginVer + Table.DefaultCellStyle.FontDescent,
			TextJustify.Center, DrawStyle.Normal, Color.Purple, "Example of PdfTable support");

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
		double PosY = Table.RowTopPosition - 2.0 * MarginVer - Table.DefaultCellStyle.FontAscent;
		Contents.DrawText(ArialNormal, FontSize, TotalDesc, PosY, TextJustify.Right, "Total before tax");
		Contents.DrawText(ArialNormal, FontSize, TotalValue, PosY, TextJustify.Right, Total.ToString("#.00"));

		// draw tax (Ontario Canada HST)
		PosY -= Table.DefaultCellStyle.FontLineSpacing;
		Contents.DrawText(ArialNormal, FontSize, TotalDesc, PosY, TextJustify.Right, "Tax (13%)");
		double Tax = Math.Round(0.13 * Total, 2, MidpointRounding.AwayFromZero);
		Contents.DrawText(ArialNormal, FontSize, TotalValue, PosY, TextJustify.Right, Tax.ToString("#.00"));

		// draw total line
		PosY -= Table.DefaultCellStyle.FontDescent + 0.5 * MarginVer;
		Contents.DrawLine(ColumnPosition[3], PosY, ColumnPosition[4], PosY);

		// draw final total
		PosY -= Table.DefaultCellStyle.FontAscent + 0.5 * MarginVer;
		Contents.DrawText(ArialNormal, FontSize, TotalDesc, PosY, TextJustify.Right, "Total payable");
		Total += Tax;
		Contents.DrawText(ArialNormal, FontSize, TotalValue, PosY, TextJustify.Right, Total.ToString("#.00"));

		PosY -= Table.DefaultCellStyle.FontDescent + MarginVer;
		Contents.DrawLine(ColumnPosition[0], Table.RowTopPosition, ColumnPosition[0], PosY);
		Contents.DrawLine(ColumnPosition[0], PosY, ColumnPosition[4], PosY);
		Contents.DrawLine(ColumnPosition[4], Table.RowTopPosition, ColumnPosition[4], PosY);

		// restore graphics state
		Contents.RestoreGraphicsState();
		return;
		}
	}

// Simulation of book order database
public class Order
	{
	public string Title;
	public string Authors;
	public double Price;
	public int Qty;

	public double Total
		{
		get
			{
			return Price * Qty;
			}
		}

	public Order
			(
			string	Title,
			string	Authors,
			double	Price,
			int	Qty
			)
		{
		this.Title = Title;
		this.Authors = Authors;
		this.Price = Price;
		this.Qty = Qty;
		return;
		}

	public static Order[] OrderList = new Order[]
		{
		new Order("Current Trends in Theoritical Computer Science: Algorithms and Complexity", "Paun", 123.5, 2),
		new Order("Theoretical Computer Science: Introduction to Automata, Computability, Complexity, Algorithmics, Randomization", "Juraj Hromkovic", 76.81, 1),
		new Order("Discrete Mathematics for Computer Science (with Student Solutions Manual CD-ROM)", "Gary Haggard, John Schlipf and Sue Whitesides", 229.88, 1),
		};
	}
}
