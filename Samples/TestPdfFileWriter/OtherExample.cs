/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter II
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	OtherExample
//	Produce PDF document when Other Example button is clicked.
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
	/// Other example
	/// </summary>
	public class OtherExample
		{
		private const double DegToRad = Math.PI / 180.0;
		private const double PageWidth = 8.5;
		private const double PageHeight = 11.0;
		private const double Margin = 0.75;
		private const double HeadingHeight = 0.5;
		private const double DispWidth = PageWidth - 2 * Margin;
		private const double DispHeight = PageHeight - 2 * Margin - HeadingHeight;
		private const double AreaWidth = DispWidth / 2;
		private const double AreaHeight = DispHeight / 3;
		private const double AreaX1 = Margin;
		private const double AreaX2 = Margin + AreaWidth;
		private const double AreaX3 = Margin + 2 * AreaWidth;
		private const double AreaY1 = Margin;
		private const double AreaY2 = Margin + AreaHeight;
		private const double AreaY3 = Margin + 2 * AreaHeight;
		private const double AreaY4 = Margin + 3 * AreaHeight;
		private const double NoteX = 0.1;
		private const double NoteY0 = 0.1;
		private double NoteY1;
		private double NoteY2;
		private const double NoteSize = 10;

		private PdfDocument Document;
		private PdfPage Page;
		private PdfContents BaseContents;
		private PdfContents Contents;
		private PdfFont ArialNormal;
		private PdfFont ArialBold;
		private PdfFont ArialItalic;
		private PdfFont ArialBoldItalic;
		private PdfFont TimesNormal;
		private PdfFont TimesBold;
		private PdfFont TimesItalic;
		private PdfFont TimesBoldItalic;
		private PdfFont LucidaNormal;
		private PdfFont Comic;
		private PdfFont Symbol;

		private PdfDrawTextCtrl NoteTextCtrl;

		private PdfBookmark BookmarkRoot;
		private PdfBookmark FirstLevelBookmark;
		private double FirstLevelYPos;

		private PdfLayer ImageLayer;
		private PdfLayer BarcodeLayer;
		private PdfLayer AnnotationLayer;

		private PdfAnnotDisplayMedia DisplayMedia;

		private static readonly string ArticleLink =
			"http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version";

		/// <summary>
		/// Create test PDF document
		/// </summary>
		/// <param name="FileName">PDF file name</param>
		/// <param name="Debug">Debug flag</param>
		public void Test
				(
				string FileName,
				bool Debug
				)
			{
			// create document (letter size, portrait, inches)
			using (Document = new PdfDocument(PageWidth, PageHeight, UnitOfMeasure.Inch, FileName))
				{
				// set or clear debug flag
				Document.Debug = Debug;

				// set encryption
				// Document.SetEncryption("password", Permission.All & ~Permission.Print);

				// set the program to display bookmarks
				Document.InitialDocDisplay = InitialDocDisplay.UseBookmarks;

				// and get bookmark root object
				BookmarkRoot = Document.GetBookmarksRoot();

				// open layer control object (in PDF terms optional content object)
				PdfLayers Layers = new PdfLayers(Document, "PDF layers group");

				// define layers
				ImageLayer = new PdfLayer(Layers, "Image Layer Control");
				BarcodeLayer = new PdfLayer(Layers, "Barcode Layer Control");
				AnnotationLayer = new PdfLayer(Layers, "Annotation Layer Control");

				// define font resource
				ArialNormal = PdfFont.CreatePdfFont(Document, "Arial", FontStyle.Regular);
				ArialBold = PdfFont.CreatePdfFont(Document, "Arial", FontStyle.Bold);
				ArialItalic = PdfFont.CreatePdfFont(Document, "Arial", FontStyle.Italic);
				ArialBoldItalic = PdfFont.CreatePdfFont(Document, "Arial", FontStyle.Bold | FontStyle.Italic);
				TimesNormal = PdfFont.CreatePdfFont(Document, "Times New Roman", FontStyle.Regular);
				TimesBold = PdfFont.CreatePdfFont(Document, "Times New Roman", FontStyle.Bold);
				TimesItalic = PdfFont.CreatePdfFont(Document, "Times New Roman", FontStyle.Italic);
				TimesBoldItalic = PdfFont.CreatePdfFont(Document, "Times New Roman", FontStyle.Bold | FontStyle.Italic);
				LucidaNormal = PdfFont.CreatePdfFont(Document, "Lucida Console", FontStyle.Regular);
				Comic = PdfFont.CreatePdfFont(Document, "Comic Sans MS", FontStyle.Regular);
				Symbol = PdfFont.CreatePdfFont(Document, "Wingdings", FontStyle.Regular);

				NoteTextCtrl = new PdfDrawTextCtrl(ArialNormal, NoteSize);
				NoteY1 = NoteY0 + NoteTextCtrl.LineSpacing;
				NoteY2 = NoteY1 + NoteTextCtrl.LineSpacing;

				// table of contents
				CreateTableOfContents();

				// create page base contents
				CreateBaseContents();

				// pages
				CreatePage1Contents();
				CreatePage2Contents();
				CreatePage3Contents();
				CreatePage4Contents();
				CreatePage5Contents();
				CreatePage6Contents();
				CreatePage7Contents();
				CreatePage8Contents();
				CreatePage9Contents();

				// create pdf file
				Document.CreateFile();

				// start default PDF reader and display the file
				Process Proc = new Process();
				Proc.StartInfo = new ProcessStartInfo(FileName) {UseShellExecute = true};
				Proc.Start();
				}

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// create table of contents
		////////////////////////////////////////////////////////////////////
		private void CreateTableOfContents()
			{
			// define page and contents
			Page = new PdfPage(Document);
			Contents = new PdfContents(Page);
			Document.AddLocationMarker("TableOfContents", Page, LocMarkerScope.LocalDest, DestFit.FitH, 10.5);

			// display rectangle
			PdfRectangle Rect = new PdfRectangle(AreaX1, AreaY1, AreaX1 + DispWidth, AreaY1 + DispHeight);
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.BorderWidth = 0.02;
			DrawCtrl.BorderColor = Color.Gray;
			Contents.DrawGraphics(DrawCtrl, Rect);

			// heading
			PdfDrawTextCtrl HeadingCtrl = new PdfDrawTextCtrl(ArialBold, 24);
			HeadingCtrl.Justify = TextJustify.Center;
			double PosX = 0.5 * PageWidth;
			double PosY = PageHeight - Margin - 0.5 * (HeadingHeight - HeadingCtrl.CapHeight);
			Contents.DrawText(HeadingCtrl, PosX, PosY, "PDF File Writer II Example");

			// index
			PosX = AreaX1 + 0.5;
			PosY = AreaY4 - 2.0;

			// table of contents heading
			HeadingCtrl = new PdfDrawTextCtrl(TimesBold, 20);
			HeadingCtrl.Justify = TextJustify.Center;
			Contents.DrawText(HeadingCtrl, 0.5 * PageWidth, PosY, "Table of Contents");

			// table of contents
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialItalic, 14);

			double LineHeight = TextCtrl.LineSpacing + 0.1;
			PosY -= LineHeight + 0.2;
			TextCtrl.Annotation = new PdfAnnotLinkAction(Document, "Page1");
			Contents.DrawText(TextCtrl, PosX, PosY, "Page 1 Lines Rectangles Bezier");

			PosY -= LineHeight;
			TextCtrl.Annotation = new PdfAnnotLinkAction(Document, "Page2");
			Contents.DrawText(TextCtrl, PosX, PosY, "Page 2 Bezier and Image");

			PosY -= LineHeight;
			TextCtrl.Annotation = new PdfAnnotLinkAction(Document, "Page3");
			Contents.DrawText(TextCtrl, PosX, PosY, "Page 3 Bezier and Line Caps");

			PosY -= LineHeight;
			TextCtrl.Annotation = new PdfAnnotLinkAction(Document, "Page4");
			Contents.DrawText(TextCtrl, PosX, PosY, "Page 4 Shading and Patterns");

			PosY -= LineHeight;
			TextCtrl.Annotation = new PdfAnnotLinkAction(Document, "Page5");
			Contents.DrawText(TextCtrl, PosX, PosY, "Page 5 Text and TextBox");

			PosY -= LineHeight;
			TextCtrl.Annotation = new PdfAnnotLinkAction(Document, "Page6");
			Contents.DrawText(TextCtrl, PosX, PosY, "Page 6 Barcode QR Code and Web Link");

			PosY -= LineHeight;
			TextCtrl.Annotation = new PdfAnnotLinkAction(Document, "Page7");
			Contents.DrawText(TextCtrl, PosX, PosY, "Page 7 Media, Sound and attached files");

			PosY -= LineHeight;
			TextCtrl.Annotation = new PdfAnnotLinkAction(Document, "Page8");
			Contents.DrawText(TextCtrl, PosX, PosY, "Page 8 Transparency, Blend and Draw Symbol");

			PosY -= LineHeight;
			TextCtrl.Annotation = new PdfAnnotLinkAction(Document, "Page9");
			Contents.DrawText(TextCtrl, PosX, PosY, "Page 9 Fillable form");

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// create base contents for all pages
		////////////////////////////////////////////////////////////////////
		private void CreateBaseContents()
			{
			// create unattached contents
			BaseContents = new PdfContents(Document);

			// draw frame
			PdfRectangle Frame = new PdfRectangle(AreaX1, AreaY1, AreaX1 + DispWidth, AreaY1 + DispHeight);
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.BorderWidth = 0.02;
			DrawCtrl.BorderColor = Color.Gray;
			BaseContents.DrawGraphics(DrawCtrl, Frame);

			// draw dividing lines
			BaseContents.SaveGraphicsState();
			BaseContents.SetLineWidth(0.02);
			BaseContents.SetColorStroking(Color.Gray);
			LineD CenterLine = new LineD(AreaX2, AreaY1, AreaX2, AreaY4);
			BaseContents.DrawLine(CenterLine);
			LineD HorLine1 = new LineD(AreaX1, AreaY2, AreaX3, AreaY2);
			BaseContents.DrawLine(HorLine1);
			LineD HorLine2 = new LineD(AreaX1, AreaY3, AreaX3, AreaY3);
			BaseContents.DrawLine(HorLine2);
			BaseContents.RestoreGraphicsState();

			// heading
			PdfDrawTextCtrl HeadingCtrl = new PdfDrawTextCtrl(ArialBold, 24);
			HeadingCtrl.Justify = TextJustify.Center;
			BaseContents.DrawText(HeadingCtrl, 0.5 * PageWidth,
				PageHeight - Margin - 0.5 * (HeadingHeight - ArialBold.CapHeight(24)), "PDF File Writer Example");

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// create page1 contents
		////////////////////////////////////////////////////////////////////
		private void CreatePage1Contents()
			{
			// add contents to page
			Contents = AddPageToDocument(1);

			// bookmark
			PdfBookmarkCtrl BookmarkCtrl = new PdfBookmarkCtrl();
			BookmarkCtrl.Color = Color.Red;
			BookmarkCtrl.TextStyle = BookmarkTextStyle.Bold;
			FirstLevelYPos = PageHeight - 0.75 + ArialNormal.Ascent(10);
			FirstLevelBookmark = BookmarkRoot.AddBookmark(BookmarkCtrl, "Page 1 Lines Rectangles Bezier", Page, 0.0, FirstLevelYPos);

			// location markers
			Document.AddLocationMarker("Page1", Page, LocMarkerScope.LocalDest, DestFit.FitH, 10.5);
			Document.AddLocationMarker("MidPage1", Page, LocMarkerScope.LocalDest, DestFit.FitH, 5.5);

			// draw examples
			Example1a(AreaX1, AreaY3);
			Example1b(AreaX2, AreaY3);
			Example1c(AreaX1, AreaY2);
			Example1d(AreaX2, AreaY2);
			Example1e(AreaX1, AreaY1);
			Example1f(AreaX2, AreaY1);

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// create page2 contents
		////////////////////////////////////////////////////////////////////
		private void CreatePage2Contents()
			{
			// add contents to page
			Contents = AddPageToDocument(2);

			// location markers
			Document.AddLocationMarker("Page2", Page, LocMarkerScope.LocalDest, DestFit.FitH, 10.5);

			// bookmark
			PdfBookmarkCtrl BookmarkCtrl = new PdfBookmarkCtrl();
			BookmarkCtrl.Color = Color.Red;
			BookmarkCtrl.TextStyle = BookmarkTextStyle.Bold;
			FirstLevelBookmark = BookmarkRoot.AddBookmark(BookmarkCtrl, "Page 2 Bezier and Image", Page, 0.0, FirstLevelYPos);

			// draw examples
			Example2a(AreaX1, AreaY3);
			Example2b(AreaX2, AreaY3);
			Example2c(AreaX1, AreaY2);
			Example2d(AreaX2, AreaY2);
			Example2e(AreaX1, AreaY1);
			Example2f(AreaX2, AreaY1);

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// create page3 contents
		////////////////////////////////////////////////////////////////////
		private void CreatePage3Contents()
			{
			// add contents to page
			Contents = AddPageToDocument(3);

			// location markers
			Document.AddLocationMarker("Page3", Page, LocMarkerScope.NamedDest, DestFit.FitH, 10.5);

			// bookmark
			PdfBookmarkCtrl BookmarkCtrl = new PdfBookmarkCtrl();
			BookmarkCtrl.Color = Color.Red;
			BookmarkCtrl.TextStyle = BookmarkTextStyle.Bold;
			FirstLevelBookmark = BookmarkRoot.AddBookmark(BookmarkCtrl, "Page 3 Bezier and Line Caps", Page, 0.0, FirstLevelYPos);

			// draw examples
			PointD P0 = new PointD(0.25, 0.5);
			PointD P1 = new PointD(0.75, 2.2);
			PointD P2a = new PointD(2.75, 1.9);
			PointD P3a = new PointD(3.25, 0.9);
			Example3a(AreaX1, AreaY3, P0, P1, P2a, P3a, "Example 3a: Cubic Bezier with both control points", "on the same side of the curve");

			PointD P2b = new PointD(2.75, .75);
			PointD P3b = new PointD(3.25, 2.75);
			Example3a(AreaX2, AreaY3, P0, P1, P2b, P3b, "Example 3b: Cubic Bezier with control points", "on the two sides of the curve");

			PointD P2c = new PointD(3.25, 2.9);
			PointD P3c = new PointD(2.75, 0.9);
			Example3a(AreaX1, AreaY2, P0, null, P2c, P3c, "Example 3c: Cubic Bezier with end point P0", "equals control point P1");

			PointD P3d = new PointD(3.25, 2.75);
			Example3a(AreaX2, AreaY2, P0, P1, null, P3d, "Example 3d: Cubic Bezier with control point P2", "equals end point P3");

			Example3e(AreaX1, AreaY1);
			Example3f(AreaX2, AreaY1);

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// create page4 contents
		////////////////////////////////////////////////////////////////////
		private void CreatePage4Contents()
			{
			// add contents to page
			Contents = AddPageToDocument(4);

			// location markers
			Document.AddLocationMarker("Page4", Page, LocMarkerScope.NamedDest, DestFit.FitH, 10.5);

			// bookmark
			PdfBookmarkCtrl BookmarkCtrl = new PdfBookmarkCtrl();
			BookmarkCtrl.Color = Color.Red;
			BookmarkCtrl.TextStyle = BookmarkTextStyle.Bold;
			FirstLevelBookmark = BookmarkRoot.AddBookmark(BookmarkCtrl, "Page 4 Shading and Patterns", Page, 0.0, FirstLevelYPos);

			// draw examples
			Example4a(AreaX1, AreaY3);
			Example4b(AreaX2, AreaY3);
			Example4c(AreaX1, AreaY2);
			Example4d(AreaX2, AreaY2);
			Example4e(AreaX1, AreaY1);
			Example4f(AreaX2, AreaY1);

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// create page5 contents
		////////////////////////////////////////////////////////////////////
		private void CreatePage5Contents()
			{
			// add contents to page
			Contents = AddPageToDocument(5);

			// location markers
			Document.AddLocationMarker("Page5", Page, LocMarkerScope.NamedDest, DestFit.FitH, 10.5);

			// bookmark
			PdfBookmarkCtrl BookmarkCtrl = new PdfBookmarkCtrl();
			BookmarkCtrl.Color = Color.Red;
			BookmarkCtrl.TextStyle = BookmarkTextStyle.Bold;
			BookmarkCtrl.OpenEntries = true;
			FirstLevelBookmark = BookmarkRoot.AddBookmark(BookmarkCtrl, "Page 5 Text and TextBox", Page, 0.0, FirstLevelYPos);

			// draw examples
			Example5a(AreaX1, AreaY3);
			Example5b(AreaX2, AreaY3);
			Example5c(AreaX1, AreaY2);
			Example5d(AreaX2, AreaY2);
			Example5e(AreaX1, AreaY1);
			Example5f(AreaX2, AreaY1);

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// create page6 contents
		////////////////////////////////////////////////////////////////////
		private void CreatePage6Contents()
			{
			// add contents to page
			Contents = AddPageToDocument(6);

			// location markers
			Document.AddLocationMarker("Page6", Page, LocMarkerScope.NamedDest, DestFit.FitH, 10.5);

			// bookmark
			PdfBookmarkCtrl BookmarkCtrl = new PdfBookmarkCtrl();
			BookmarkCtrl.Color = Color.Red;
			BookmarkCtrl.TextStyle = BookmarkTextStyle.Bold;
			FirstLevelBookmark = BookmarkRoot.AddBookmark(BookmarkCtrl, "Page 6 Barcode QR Code and Web Link", Page, 0.0, FirstLevelYPos);

			// draw examples
			Example6a(AreaX1, AreaY3);
			Example6b(AreaX2, AreaY3);
			Example6c(AreaX1, AreaY2);
			Example6d(AreaX2, AreaY2);
			Example6e(AreaX1, AreaY1);
			Example6f(AreaX2, AreaY1);

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// create page7 contents
		////////////////////////////////////////////////////////////////////
		private void CreatePage7Contents()
			{
			// add contents to page
			Contents = AddPageToDocument(7);

			// location markers
			Document.AddLocationMarker("Page7", Page, LocMarkerScope.NamedDest, DestFit.FitH, 10.5);

			// bookmark
			PdfBookmarkCtrl BookmarkCtrl = new PdfBookmarkCtrl();
			BookmarkCtrl.Color = Color.Red;
			BookmarkCtrl.TextStyle = BookmarkTextStyle.Bold;
			FirstLevelBookmark = BookmarkRoot.AddBookmark(BookmarkCtrl, "Page 7 Media, Sound and attached files", Page, 0.0, FirstLevelYPos);

			// draw examples
			Example7a(AreaX1, AreaY3);
			Example7b(AreaX2, AreaY3);
			Example7c(AreaX1, AreaY2);
			Example7d(AreaX2, AreaY2);
			Example7e(AreaX1, AreaY1);
			Example7f(AreaX2, AreaY1);

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// create page8 contents
		////////////////////////////////////////////////////////////////////
		private void CreatePage8Contents()
			{
			// add contents to page
			Contents = AddPageToDocument(8);

			// location markers
			Document.AddLocationMarker("Page8", Page, LocMarkerScope.NamedDest, DestFit.FitH, 10.5);

			// bookmark
			PdfBookmarkCtrl BookmarkCtrl = new PdfBookmarkCtrl();
			BookmarkCtrl.Color = Color.Red;
			BookmarkCtrl.TextStyle = BookmarkTextStyle.Bold;
			FirstLevelBookmark = BookmarkRoot.AddBookmark(BookmarkCtrl, "Page 8 Transparency, Blend and Draw Symbol", Page, 0.0, FirstLevelYPos);

			// draw examples
			Example8a(AreaX1, AreaY3);
			Example8b(AreaX2, AreaY3);
			Example8c(AreaX1, AreaY2);
			Example8d(AreaX2, AreaY2);
			Example8e(AreaX1, AreaY1);
			Example8f(AreaX2, AreaY1);

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// create page9 contents
		////////////////////////////////////////////////////////////////////
		private void CreatePage9Contents()
			{
			// add contents to page
			Contents = AddPageToDocument(9);

			// location markers
			Document.AddLocationMarker("Page9", Page, LocMarkerScope.NamedDest, DestFit.FitH, 10.5);

			// bookmark
			PdfBookmarkCtrl BookmarkCtrl = new PdfBookmarkCtrl();
			BookmarkCtrl.Color = Color.Red;
			BookmarkCtrl.TextStyle = BookmarkTextStyle.Bold;
			FirstLevelBookmark = BookmarkRoot.AddBookmark(BookmarkCtrl, "Page 9 Fillable form", Page, 0.0, FirstLevelYPos);

			// draw examples
			Example9a(AreaX1, AreaY3);
			Example9b(AreaX2, AreaY3);
			Example9c(AreaX1, AreaY2);

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Add page to document and draw page number
		////////////////////////////////////////////////////////////////////
		private PdfContents AddPageToDocument
				(
				int PageNo
				)
			{
			// add new page with two contents objects
			Page = new PdfPage(Document);
			Page.AddContents(BaseContents);
			PdfContents Contents = new PdfContents(Page);

			// draw page number right justified
			PdfDrawTextCtrl PageNoTextCtrl = new PdfDrawTextCtrl(ArialNormal, 10);
			PageNoTextCtrl.Justify = TextJustify.Right;
			double PosY = PageHeight - 0.75 + PageNoTextCtrl.LineSpacing - PageNoTextCtrl.TextAscent;
			Contents.DrawText(PageNoTextCtrl, PageWidth - Margin, PosY, string.Format("Page {0}", PageNo));

			// link to table of contents
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialItalic, 12);
			TextCtrl.Justify = TextJustify.Right;
			TextCtrl.Annotation = new PdfAnnotLinkAction(Document, "TableOfContents");
			TextCtrl.TextColor = Color.DarkRed;
			Contents.DrawText(TextCtrl, AreaX3, AreaY1 - 0.18, "Table of contents");
			return Contents;
			}

		////////////////////////////////////////////////////////////////////
		// Example 1a Draw three solid lines
		////////////////////////////////////////////////////////////////////
		private void Example1a
				(
				double PosX,
				double PosY
				)
			{
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			// draw solid line 1
			Contents.SetLineWidth(0.05);
			Contents.SetColorStroking(Color.Red);
			LineD Line1 = new LineD(0.25, 1.0, AreaWidth - 0.25, 1.0);
			Contents.DrawLine(Line1);

			// draw solid line 2
			Contents.SetLineWidth(0.1);
			Contents.SetColorStroking(Color.Green);
			LineD Line2 = new LineD(1.5, 0.5, 1.5, AreaHeight - 0.25);
			Contents.DrawLine(Line2);

			// draw solid line 3
			Contents.SetLineWidth(0.15);
			Contents.SetColorStroking(Color.DarkBlue);
			LineD Line3 = new LineD(0.5, AreaHeight - 0.5, AreaWidth - 0.25, 0.25);
			Contents.DrawLine(Line3);

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 1a: Draw solid lines");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 1b Three dashed lines
		////////////////////////////////////////////////////////////////////
		private void Example1b
				(
				double PosX,
				double PosY
				)
			{
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			// draw three lines
			Contents.SetLineWidth(0.05);
			Contents.SetDashLine(new double[] { 0.05, 0.05, 0.1, 0.05 }, 0);
			Contents.SetColorStroking(Color.Red);
			LineD RedLine = new LineD(0.25, 1.0, AreaWidth - 0.25, 1.0);
			Contents.DrawLine(RedLine);

			Contents.SetLineWidth(0.1);
			Contents.SetDashLine(new double[] { 0.05, 0.05 }, 0);
			Contents.SetColorStroking(Color.Green);
			LineD GreenLine = new LineD(1.5, 0.5, 1.5, AreaHeight - 0.25);
			Contents.DrawLine(GreenLine);

			Contents.SetLineWidth(0.15);
			Contents.SetDashLine(new double[] { 0.15, 0.02, 0.3, 0.02 }, 0);
			Contents.SetColorStroking(Color.DarkBlue);
			LineD BlueLine = new LineD(0.5, AreaHeight - 0.5, AreaWidth - 0.25, 0.25);
			Contents.DrawLine(BlueLine);

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 1b: Draw dashed lines");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 1c Three rectangles: stroke, fill, fill and stroke
		////////////////////////////////////////////////////////////////////
		private void Example1c
				(
				double PosX,
				double PosY
				)
			{
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);
		
			// draw three rectangles
			double Left = 0.2;
			double Bottom = 2.1;
			double Width = 1.6;
			double Height = 0.7;

			// rectangle 1
			PdfRectangle Rect1 = new PdfRectangle(Left, Bottom, Left + Width, Bottom + Height);
			PdfDrawCtrl RectCtrl1 = new PdfDrawCtrl();
			RectCtrl1.BorderWidth = 0.1;
			RectCtrl1.BorderColor = Color.DarkBlue;
			Contents.DrawGraphics(RectCtrl1, Rect1);

			// rectangle 2
			Left += 0.6;
			Bottom -= 0.85;
			PdfRectangle Rect2 = new PdfRectangle(Left, Bottom, Left + Width, Bottom + Height);
			PdfDrawCtrl RectCtrl2 = new PdfDrawCtrl();
			RectCtrl2.Paint = DrawPaint.Fill;
			RectCtrl2.BackgroundTexture = Color.Turquoise;
			Contents.DrawGraphics(RectCtrl2, Rect2);

			// rectangle 3
			Left += 0.6;
			Bottom -= 0.85;
			PdfRectangle Rect3 = new PdfRectangle(Left, Bottom, Left + Width, Bottom + Height);
			PdfDrawCtrl RectCtrl3 = new PdfDrawCtrl();
			RectCtrl3.Paint = DrawPaint.BorderAndFill;
			RectCtrl3.BorderWidth = 0.1;
			RectCtrl3.BorderColor = Color.DarkBlue;
			RectCtrl3.BackgroundTexture = Color.Turquoise;
			Contents.DrawGraphics(RectCtrl3, Rect3);

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 1c: Rectangles: stroke, fill, fill+stroke");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 1d Rounded rectangle
		////////////////////////////////////////////////////////////////////
		private void Example1d
				(
				double PosX,
				double PosY
				)
			{
			// save graphics state and move origin
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			// draw rounded rectangle
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Shape = DrawShape.RoundedRect;
			DrawCtrl.Paint = DrawPaint.BorderAndFill;
			DrawCtrl.BorderWidth = 0.06;
			DrawCtrl.BorderColor = Color.Purple;
			DrawCtrl.BackgroundTexture = Color.Cyan;
			PdfRectangle Rect = new PdfRectangle(0.2, 1.7, 2.7, 2.7);
			Contents.DrawGraphics(DrawCtrl, Rect);

			DrawCtrl.Shape = DrawShape.InvRoundedRect;
			Rect = new PdfRectangle(0.8, 0.4, 3.2, 1.4);
			Contents.DrawGraphics(DrawCtrl, Rect);

			// display note
			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 1d: Draw Rounded Rectangle (fill+stroke)");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 1e oval and circle
		////////////////////////////////////////////////////////////////////
		private void Example1e
				(
				double PosX,
				double PosY
				)
			{
			// draw oval
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			// center position
			double X = 0.5 * AreaWidth;
			double Y = 0.5 * AreaHeight;

			// draw rectangle control
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Shape = DrawShape.Oval;
			DrawCtrl.Paint = DrawPaint.Fill;
			DrawCtrl.BackgroundTexture = Color.Yellow;

			// draw yellow oval
			PdfRectangle Rect = new PdfRectangle(X - 1.5, Y - 1.0, X + 1.5, Y + 1.0);
			Contents.DrawGraphics(DrawCtrl, Rect);

			// draw two eyes
			DrawCtrl.Paint = DrawPaint.BorderAndFill;
			DrawCtrl.BorderWidth = 0.2;
			DrawCtrl.BackgroundTexture = Color.White;
			Rect = new PdfRectangle(X - 0.85, Y - 0.1, X - 0.15, Y + 0.6);
			Contents.DrawGraphics(DrawCtrl, Rect);
			Rect = new PdfRectangle(X + 0.15, Y - 0.1, X + 0.85, Y + 0.6);
			Contents.DrawGraphics(DrawCtrl, Rect);

			// draw mouth
			Contents.SetColorNonStroking(Color.Black);
			Contents.MoveTo(new PointD(X - 0.6, Y - 0.4));
			Contents.LineTo(new PointD(X + 0.6, Y - 0.4));
			Contents.DrawBezier(new PointD(X, Y - 0.8), new PointD(X, Y - 0.8), new PointD(X - 0.6, Y - 0.4));
			Contents.SetPaintOp(PaintOp.Fill);

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 1e: Oval, two circles, line and Bezier");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 1f polygon
		////////////////////////////////////////////////////////////////////
		private void Example1f
				(
				double PosX,
				double PosY
				)
			{
			PointF[] Polygon = new PointF[]
				{
				new PointF(0.4F, 1.5F),
				new PointF(1.5F, 2.5F),
				new PointF(2.8F, 2.1F),
				new PointF(3.2F, 0.8F),
				new PointF(1.8F, 1.7F),
				new PointF(1.3F, 0.7F),
				};

			// draw polygon
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);
			Contents.SetLineWidth(0.06);
			Contents.SetColorNonStroking(Color.HotPink);
			Contents.SetColorStroking(Color.MidnightBlue);
			Contents.DrawPolygon(Polygon, PaintOp.CloseFillStroke);

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 1f: polygon");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 2a polygon
		////////////////////////////////////////////////////////////////////
		private void Example2a
				(
				double PosX,
				double PosY
				)
			{
			// add first level bookmark
			PdfBookmarkCtrl BookmarkCtrl = new PdfBookmarkCtrl();
			BookmarkCtrl.Zoom = 2;
			FirstLevelBookmark.AddBookmark(BookmarkCtrl, "Example 2A Polygons", Page, PosX, PosY + AreaHeight);

			// translate origin
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			double CenterX = 0.5 * AreaWidth;
			double CenterY = NoteY2 + 0.5 * (AreaHeight - NoteY2);

			Color Fill = Color.LightCyan;
			for(int Index = 0; Index < 4; Index++)
				{
				Contents.SetColorNonStroking(Fill);
				Contents.DrawRegularPolygon(CenterX, CenterY, (0.4 - 0.1 * Index) * AreaHeight, 90.0 * DegToRad, 8 - Index, PaintOp.CloseFillStroke);
				Fill = Color.FromArgb((Fill.R * 8) / 10, Fill.G, Fill.B);
				}

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY1, "Example 2a: Regular polygons.");
			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "From 8 sides to 5 sides");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 2b polygon
		////////////////////////////////////////////////////////////////////
		private void Example2b
				(
				double PosX,
				double PosY
				)
			{
			// add first level bookmark
			PdfBookmarkCtrl BookmarkCtrl = new PdfBookmarkCtrl();
			BookmarkCtrl.Zoom = 2;
			FirstLevelBookmark.AddBookmark(BookmarkCtrl, "Example 2B Stars", Page, PosX, PosY + AreaHeight);

			// translate origin
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			PointD Center = new PointD(0.5 * AreaWidth, NoteY2 + 0.5 * (AreaHeight - NoteY2));

			Color Fill = Color.HotPink;
			for(int Index = 0; Index < 4; Index++)
				{
				Contents.SetColorNonStroking(Fill);
				Contents.DrawStar(Center, (0.4 - 0.1 * Index) * AreaHeight, 90, 8 - Index, PaintOp.CloseFillStroke);
				Fill = Color.FromArgb((Fill.R * 8) / 10, Fill.G, Fill.B);
				}

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY1, "Example 2b: Star shape polygons.");
			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "From 8 sides to 5 sides");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 2c polygon
		////////////////////////////////////////////////////////////////////
		private void Example2c
				(
				double PosX,
				double PosY
				)
			{
			// translate origin
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			int Sides = 18;
			double Radius1 = 0.45 * (AreaHeight - NoteY2);
			double Radius2 = 0.75 * Radius1;
			double Radius3 = 0.75 * Radius2;
			double Radius4 = 0.85 * Radius3;
			PointD Center = new PointD(0.5 * AreaWidth, NoteY2 + 0.5 * (AreaHeight - NoteY2));
			double Alpha = 0;
			double DeltaAlpha = 2.0 * Math.PI / Sides;

			Contents.SetColorNonStroking(Color.CornflowerBlue);
			for(int Index = 0; Index < Sides; Index++)
				{
				// hart
				LineD HartLine = new LineD(new PointD(Center, Radius2, Alpha), new PointD(Center, Radius1, Alpha));
				Contents.DrawHeart(HartLine, PaintOp.Fill);

				// double bezier
				LineD BezierLine = new LineD(new PointD(Center, Radius3, Alpha), new PointD(Center, Radius2, Alpha));
				Contents.DrawDoubleBezierPath(BezierLine, 0.5, 0.5 * Math.PI, 0.5, 1.5 * Math.PI, PaintOp.Fill);

				// star line
				Contents.DrawStar(new PointD(Center, Radius4, Alpha), Radius3 - Radius4, Alpha, 5, PaintOp.Fill);
				Alpha += DeltaAlpha;
				}

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY1, "Example 2c: 18 spokes with heart");
			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "double Bezier path and a star");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 2d polygon
		////////////////////////////////////////////////////////////////////
		private void Example2d
				(
				double PosX,
				double PosY
				)
			{
			// translate origin
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			Contents.SetColorNonStroking(Color.HotPink);
			LineD CenterLine = new LineD(0.5 * AreaWidth, NoteY2, 0.5 * AreaWidth, AreaHeight - 0.6);
			Contents.DrawHeart(CenterLine, PaintOp.CloseFillStroke);

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY1, "Example 2d: Heart shape made of two");
			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Symmetric Bezier curves");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 2e Image
		////////////////////////////////////////////////////////////////////
		private void Example2e
				(
				double PosX,
				double PosY
				)
			{
			// add first level bookmark
			PdfBookmarkCtrl BookmarkCtrl = new PdfBookmarkCtrl();
			BookmarkCtrl.Zoom = 2;
			FirstLevelBookmark.AddBookmark(BookmarkCtrl, "Example 2E JPEG Image file", Page, PosX, PosY + AreaHeight);

			// translate origin
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			// image resource
			PdfImage Image1 = new PdfImage(Document);
			Image1.Resolution = 300.0;
			Image1.LayerControl = ImageLayer;
			Image1.LoadImage("TestImage.jpg");

			// adjust image size and position
			PdfRectangle ImageArea = new PdfRectangle(0.1, 0.2, AreaWidth - 0.1, AreaHeight - 0.1);
			PdfRectangle NewArea = PdfImageSizePos.ImageArea(Image1, ImageArea, ContentAlignment.MiddleCenter);

			// draw image		
			Contents.DrawImage(Image1, NewArea);

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 2e: JPEG image file");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 2f Image
		////////////////////////////////////////////////////////////////////
		private void Example2f
				(
				double PosX,
				double PosY
				)
			{
			// translate origin
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			// image resource cropped by percent size
			PdfImage Image1 = new PdfImage(Document);
			Image1.Resolution = 300.0;
			Image1.CropPercent = new RectangleF(50.0F, 38.0F, 40.0F, 40.0F);
			Image1.LayerControl = ImageLayer;
			Image1.LoadImage("TestImage.jpg");

			// adjust image size and position
			PdfRectangle ImageArea = new PdfRectangle(0.1, 0.2, AreaWidth - 0.1, AreaHeight - 0.1);
			PdfRectangle NewArea = PdfImageSizePos.ImageArea(Image1, ImageArea, ContentAlignment.MiddleCenter);

			// draw image		
			Contents.DrawImage(Image1, NewArea);

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 2f: The same image but cropped");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 3a to 3d Bezier curves
		////////////////////////////////////////////////////////////////////
		private void Example3a
				(
				double PosX,
				double PosY,
				PointD P0,
				PointD P1,
				PointD P2,
				PointD P3,
				string Notes1,
				string Notes2
				)
			{
			// draw Bezier
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);
			
			// draw crosses for control points
			Contents.SetLineWidth(0.01);
			Contents.SetColorStroking(Color.Red);
			DrawCross(P0);
			if(P1 != null) DrawCross(P1);
			if(P2 != null) DrawCross(P2);
			DrawCross(P3);

			// draw control lines
			Contents.SetDashLine(new double[] { 0.05, 0.05 }, 0);
			Contents.DrawLine(new LineD(P0, P1 != null ? P1 : P2));
			Contents.DrawLine(new LineD(P2 != null ? P2 : P1, P3));
			Contents.SetDashLine(null, 0);

			// draw bezier courve
			Contents.SetLineWidth(0.03);
			Contents.SetColorStroking(Color.SteelBlue);
			Contents.MoveTo(P0);

			// draw P0, P2 and P3
			if(P1 == null) Contents.DrawBezierP2andP3(P2, P3);

			// draw P0, P1 and P3
			else if(P2 == null) Contents.DrawBezierP1andP3(P1, P3);

			// draw P0, P1, P2 and P3
			else Contents.DrawBezier(P1, P2, P3);
			Contents.SetPaintOp(PaintOp.Stroke);

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY1, Notes1);
			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, Notes2);
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Draw small cross
		////////////////////////////////////////////////////////////////////
		private void DrawCross
				(
				PointD Center
				)
			{
			Contents.SaveGraphicsState();
			Contents.Translate(Center);
			LineD HorLine = new LineD(-0.1, 0, 0.1, 0);
			Contents.DrawLine(HorLine);
			LineD VerLine = new LineD(0, -0.1, 0, 0.1);
			Contents.DrawLine(VerLine);
			Contents.RestoreGraphicsState();
			}

		////////////////////////////////////////////////////////////////////
		// Example 3e Tiled colored pattern
		////////////////////////////////////////////////////////////////////
		private void Example3e
				(
				double PosX,
				double PosY
				)
			{
			// draw rounded rectangle
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			double Y = 2.8;
			double Dy = 0.3;

			Contents.SetLineWidth(0.2);
			Contents.SetColorStroking(Color.DarkBlue);
			Contents.SetLineCap(PdfLineCap.Butt);
			Contents.DrawText(NoteTextCtrl, NoteX, Y, "Line cap: butt");
			LineD Line1 = new LineD(1.4, Y, 2.8, Y);
			Contents.DrawLine(Line1);

			Y -= Dy;
			Contents.SetLineCap(PdfLineCap.Square);
			Contents.DrawText(NoteTextCtrl, NoteX, Y, "Line cap: square");
			LineD Line2 = new LineD(1.4, Y, 2.8, Y);
			Contents.DrawLine(Line2);

			Y -= Dy;
			Contents.SetLineCap(PdfLineCap.Round);
			Contents.DrawText(NoteTextCtrl, NoteX, Y, "Line cap: round");
			LineD Line3 = new LineD(1.4, Y, 2.8, Y);
			Contents.DrawLine(Line3);

			Contents.SetLineWidth(0.01);
			Contents.SetColorStroking(Color.Red);
			Contents.SetDashLine(new double[] { 0.05, 0.05 }, 0);
			LineD Line4 = new LineD(1.4, 2.9, 1.4, Y - 0.1);
			Contents.DrawLine(Line4);

			LineD Line5 = new LineD(2.8, 2.9, 2.8, Y - 0.1);
			Contents.DrawLine(Line5);
			Contents.SetDashLine(null, 0);

			Dy = 0.6;
			Y -= Dy;
			Contents.SetLineWidth(0.12);
			Contents.SetColorStroking(Color.DarkBlue);
			Contents.SetLineCap(PdfLineCap.Square);
			Contents.SetLineJoin(PdfLineJoin.Miter);
			Contents.DrawText(NoteTextCtrl, NoteX, Y, "Line join: miter");
			Contents.DrawPolygon(new float[] { 1.4F, (float) Y, 2.8F, (float) Y, 2.1F, (float) Y + 0.35F }, PaintOp.Stroke);

			Y -= Dy;
			Contents.SetLineJoin(PdfLineJoin.Bevel);
			Contents.DrawText(NoteTextCtrl, NoteX, Y, "Line join: bevel");
			Contents.DrawPolygon(new float[] { 1.4F, (float) Y, 2.8F, (float) Y, 2.1F, (float) Y + 0.35F }, PaintOp.Stroke);

			Y -= Dy;
			Contents.SetLineJoin(PdfLineJoin.Round);
			Contents.DrawText(NoteTextCtrl, NoteX, Y, "Line join: round");
			Contents.DrawPolygon(new float[] { 1.4F, (float) Y, 2.8F, (float) Y, 2.1F, (float) Y + 0.35F }, PaintOp.Stroke);

			Contents.SetColorNonStroking(Color.Black);
			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 3e: Line cap and line join");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 3f Line cap line join
		////////////////////////////////////////////////////////////////////
		private void Example3f
				(
				double PosX,
				double PosY
				)
			{
			// draw three lines
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);
			double Y = 2.0;
			double Dy = 0.8;
			Contents.SetLineWidth(0.12);
			Contents.SetColorStroking(Color.DarkBlue);
			Contents.SetLineCap(PdfLineCap.Square);
			Contents.SetLineJoin(PdfLineJoin.Miter);
			Contents.SetMiterLimit(5.8);
			Contents.DrawText(NoteTextCtrl, NoteX, Y, "Miter limit 5.8");
			Contents.DrawPolygon(new float[] { 1.4F, (float) Y, 2.8F, (float) Y, 2.1F, (float) Y + 0.25478F }, PaintOp.Stroke);
			Y -= Dy;
			Contents.SetMiterLimit(5.7);
			Contents.DrawText(NoteTextCtrl, NoteX, Y, "Miter limit 5.7");
			Contents.DrawPolygon(new float[] { 1.4F, (float) Y, 2.8F, (float) Y, 2.1F, (float) Y + 0.25478F }, PaintOp.Stroke);

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY1, "Example 3f: Miter limit for 20\u00b0. If miter limit is");
			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "less than 5.759 it is bevel join, otherwise it is miter join.");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 4a axial shading operator
		////////////////////////////////////////////////////////////////////
		private void Example4a
				(
				double PosX,
				double PosY
				)
			{
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			// define shading function with 5 samples
			Color[] ColorArray = new Color[] { Color.Red, Color.DarkOrange, Color.Green, Color.Turquoise, Color.Blue };
			PdfShadingFunction ShadingFunction = new PdfShadingFunction(Document, ColorArray);

			// draw control
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Paint = DrawPaint.Border;
			DrawCtrl.Shape = DrawShape.RoundedRect;
			DrawCtrl.BorderWidth = 0.1;
			DrawCtrl.BorderColor = Color.Black;

			// define axial shading object with default horizontal shading axis
			DrawCtrl.BackgroundTexture = new PdfAxialShading(Document, ShadingFunction);

			// define painting rectangle
			PdfRectangle Rect = new PdfRectangle(0.2, 0.4, AreaWidth - 0.2, 2.75);

			// draw rectangle
			Contents.DrawGraphics(DrawCtrl, Rect);

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 4a: Horizontal Axial shading");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 4b axial shading operator
		////////////////////////////////////////////////////////////////////
		private void Example4b
				(
				double PosX,
				double PosY
				)
			{
			// define shading function with 2 samples
			Color[] ColorArray = new Color[] { Color.Red, Color.HotPink };
			PdfShadingFunction ShadingFunction = new PdfShadingFunction(Document, ColorArray);

			// create axial shading object
			PdfAxialShading AxialShading = new PdfAxialShading(Document, ShadingFunction);
			AxialShading.BBox = new PdfRectangle(0.25, 0.3, 3.25, 0.3 + 0.9 * AreaHeight);

			// set shading axial direction to vertical
			AxialShading.Direction = new PdfRectangle(0.25, 0.3, 0, 2.45);
			AxialShading.Mapping = MappingMode.Absolute;

			// translate origin
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			// heart shape clip
			Contents.SaveGraphicsState();
			LineD CenterLine = new LineD(0.5 * AreaWidth, 0.2 * AreaHeight, 0.5 * AreaWidth, 0.75 * AreaHeight);
			Contents.DrawHeart(CenterLine, PaintOp.ClipPathEor);

			// draw shading
			Contents.DrawShading(AxialShading);
			Contents.RestoreGraphicsState();

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY1, "Example 4b: Vertical Axial shading clipped by two");
			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "symmetric Bezier curves.");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 4c axial shading operator
		////////////////////////////////////////////////////////////////////
		private void Example4c
				(
				double PosX,
				double PosY
				)
			{
			Color[] ColorArray = new Color[]
				{
				Color.FromArgb(255, 200, 255), Color.FromArgb(200, 255, 255),
				Color.FromArgb(255, 200, 255), Color.FromArgb(200, 255, 255),
				Color.FromArgb(255, 200, 255), Color.FromArgb(200, 255, 255),
				Color.FromArgb(255, 200, 255), Color.FromArgb(200, 255, 255),
				Color.FromArgb(255, 200, 255), Color.FromArgb(200, 255, 255),
				};
			PdfShadingFunction ShadingFunction = new PdfShadingFunction(Document, ColorArray);
			PdfAxialShading AxialShading = new PdfAxialShading(Document, ShadingFunction);
			AxialShading.BBox = new PdfRectangle(0.25, 0.5, 3.25, 2.85);
			AxialShading.Direction = new PdfRectangle(0.25, 2.75, 3, -3 * Math.Tan(30.0 * Math.PI / 180.0));
			AxialShading.Mapping = MappingMode.Absolute;

			// translate origin
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			// clip path with regular polygon
			Contents.SaveGraphicsState();
			Contents.DrawRegularPolygon(0.25 + 3.0 / 2, 0.5 + 2.35 / 2, 1.2, 0, 6, PaintOp.NoOperator);
			Contents.DrawRegularPolygon(0.25 + 3.0 / 2, 0.5 + 2.35 / 2, 0.4, 30.0 * DegToRad, 6, PaintOp.ClipPathEor);

			// draw shading
			Contents.DrawShading(AxialShading);
			Contents.RestoreGraphicsState();

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY1, "Example 4c: Diagonal Axial shading");
			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Shading axis is from top-left to bottom-right");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 4d axial shading operator
		////////////////////////////////////////////////////////////////////
		private void Example4d
				(
				double PosX,
				double PosY
				)
			{
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			Color[] ColorArray = new Color[] { Color.Red, Color.DarkOrange, Color.Green, Color.Turquoise, Color.Blue };
			PdfShadingFunction ShadingFunction = new PdfShadingFunction(Document, ColorArray);

			// draw control
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Paint = DrawPaint.Border;
			DrawCtrl.Shape = DrawShape.Oval;
			DrawCtrl.BorderWidth = 0.1;
			DrawCtrl.BorderColor = Color.Black;

			// define axial shading object with default horizontal shading axis
			DrawCtrl.BackgroundTexture = new PdfRadialShading(Document, ShadingFunction);

			// define painting rectangle
			PdfRectangle Rect = new PdfRectangle(0.1, 0.45, AreaWidth - 0.1, 2.85);

			// draw rectangle
			Contents.DrawGraphics(DrawCtrl, Rect);

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY1, "Example 4d: Radial shading. One large circle and");
			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "a second concentric circle with zero radius.");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 4e axial shading operator
		////////////////////////////////////////////////////////////////////
		private void Example4e
				(
				double PosX,
				double PosY
				)
			{
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			// draw control
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Shape = DrawShape.RoundedRect;
			DrawCtrl.Paint = DrawPaint.BorderAndFill;
			DrawCtrl.BorderWidth = 0.04;
			DrawCtrl.BorderColor = Color.Purple;
			DrawCtrl.Radius = 0.3;

			// define brick pattern
			DrawCtrl.BackgroundTexture = PdfTilingPattern.SetBrickPattern(Document, 0.25, Color.LightYellow, Color.SandyBrown);

			// draw rounded rectangle
			PdfRectangle Rect = new PdfRectangle(0.25, 0.5, 3.25, 2.75);
			Contents.DrawGraphics(DrawCtrl, Rect);

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 4e: Brick pattern");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 4f Tiled colored pattern
		////////////////////////////////////////////////////////////////////
		private void Example4f
				(
				double PosX,
				double PosY
				)
			{
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			// draw control
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Shape = DrawShape.RoundedRect;
			DrawCtrl.Paint = DrawPaint.BorderAndFill;
			DrawCtrl.BorderWidth = 0.04;
			DrawCtrl.BorderColor = Color.Purple;
			DrawCtrl.Radius = 0.3;

			// define brick pattern
			DrawCtrl.BackgroundTexture = PdfTilingPattern.SetWeavePattern(Document, 0.3, Color.Black, Color.Beige, Color.Yellow);

			// draw rounded rectangle
			PdfRectangle Rect = new PdfRectangle(0.25, 0.5, 3.25, 2.75);
			Contents.DrawGraphics(DrawCtrl, Rect);

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 4f: Weave pattern");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 5a Text
		////////////////////////////////////////////////////////////////////
		private void Example5a
				(
				double PosX,
				double PosY
				)
			{
			// add first level bookmark
			PdfBookmarkCtrl BookmarkCtrl = new PdfBookmarkCtrl();
			BookmarkCtrl.Zoom = 2;
			BookmarkCtrl.OpenEntries = true;
			FirstLevelBookmark.AddBookmark(BookmarkCtrl, "Example 5A Draw text", Page, PosX, PosY + AreaHeight);

			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			double FontSize = 14.0;
			PdfDrawTextCtrl TextCtrlArialNormal = new PdfDrawTextCtrl(ArialNormal, FontSize);
			double BaseLine = AreaHeight - 0.4;
			Contents.DrawText(TextCtrlArialNormal, 0.1, BaseLine, "Arial normal ABCD abcd 1234");

			PdfDrawTextCtrl TextCtrlArialBold = new PdfDrawTextCtrl(ArialBold, FontSize);
			BaseLine -= TextCtrlArialBold.LineSpacing;
			Contents.DrawText(TextCtrlArialBold, 0.1, BaseLine, "Arial bold ABCD abcd 1234");

			PdfDrawTextCtrl TextCtrlArialItalic = new PdfDrawTextCtrl(ArialItalic, FontSize);
			BaseLine -= TextCtrlArialItalic.LineSpacing;
			Contents.DrawText(TextCtrlArialItalic, 0.1, BaseLine, "Arial Italic ABCD abcd 1234");

			PdfDrawTextCtrl TextCtrlArialBoldItalic = new PdfDrawTextCtrl(ArialBoldItalic, FontSize);
			BaseLine -= TextCtrlArialBoldItalic.LineSpacing;
			Contents.DrawText(TextCtrlArialBoldItalic, 0.1, BaseLine, "Arial bold-italic ABCD abcd 1234");

			FontSize = 12.0;
			PdfDrawTextCtrl TextCtrlTimesNormal = new PdfDrawTextCtrl(TimesNormal, FontSize);
			BaseLine -= 0.2 + TextCtrlTimesNormal.LineSpacing;
			Contents.DrawText(TextCtrlTimesNormal, 0.1, BaseLine, "Times New Roman normal ABCD abcd 1234");

			PdfDrawTextCtrl TextCtrlTimesBold = new PdfDrawTextCtrl(TimesBold, FontSize);
			BaseLine -= TextCtrlTimesBold.LineSpacing;
			Contents.DrawText(TextCtrlTimesBold, 0.1, BaseLine, "Times New Roman bold ABCD abcd 1234");

			PdfDrawTextCtrl TextCtrlTimesItalic = new PdfDrawTextCtrl(TimesItalic, FontSize);
			BaseLine -= TextCtrlTimesItalic.LineSpacing;
			Contents.DrawText(TextCtrlTimesItalic, 0.1, BaseLine, "Times New Roman Italic ABCD abcd 1234");

			PdfDrawTextCtrl TextCtrlTimesBoldItalic = new PdfDrawTextCtrl(TimesBoldItalic, FontSize);
			BaseLine -= TextCtrlTimesBoldItalic.LineSpacing;
			Contents.DrawText(TextCtrlTimesBoldItalic, 0.1, BaseLine, "Times New Roman bold-italic ABCD abcd 1234");

			FontSize = 11.0;
			PdfDrawTextCtrl TextCtrlLucida = new PdfDrawTextCtrl(LucidaNormal, FontSize);
			BaseLine -= 0.2 + TextCtrlLucida.LineSpacing;
			Contents.DrawText(TextCtrlLucida, 0.1, BaseLine, "Lucida Consol normal ABCD abcd 1234");

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY1, "Example 5a: Draw text using Arial,");
			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Times New Roman and Lucida Consol fonts.");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 5b Text
		////////////////////////////////////////////////////////////////////
		private void Example5b
				(
				double PosX,
				double PosY
				)
			{
			// add second level bookmark
			PdfBookmarkCtrl BookmarkCtrl = new PdfBookmarkCtrl();
			BookmarkCtrl.Zoom = 2;
			PdfBookmark SecondLevel = FirstLevelBookmark.AddBookmark(BookmarkCtrl, "Example 5B Draw text with style", Page, PosX, PosY + AreaHeight);

			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialNormal, 14);
			double LineSpacing = (TextCtrl.LineSpacing * 105) / 100;
			double BaseLine = AreaHeight - 0.4;
			TextCtrl.DrawStyle = DrawStyle.Underline;
			Contents.DrawText(TextCtrl, 0.1, BaseLine, "Underline text example");
			BaseLine -= LineSpacing;

			TextCtrl.DrawStyle = DrawStyle.Strikeout;
			Contents.DrawText(TextCtrl, 0.1, BaseLine, "Strikeout text example");
			BaseLine -= LineSpacing;

			TextCtrl.DrawStyle = DrawStyle.Strikeout | DrawStyle.Underline;
			Contents.DrawText(TextCtrl, 0.1, BaseLine, "Underline and strikeout combination");
			BaseLine -= LineSpacing;

			double TextPos = 0.1;
			TextCtrl.DrawStyle = DrawStyle.Normal;
			TextPos += Contents.DrawText(TextCtrl, TextPos, BaseLine, "Subscript example: H");

			TextCtrl.DrawStyle = DrawStyle.Subscript;
			TextPos += Contents.DrawText(TextCtrl, TextPos, BaseLine, "2");

			TextCtrl.DrawStyle = DrawStyle.Normal;
			Contents.DrawText(TextCtrl, TextPos, BaseLine, "O");
			BaseLine -= LineSpacing;

			TextPos = 0.1;
			TextPos += Contents.DrawText(TextCtrl, TextPos, BaseLine, "Superscript example: A");

			TextCtrl.DrawStyle = DrawStyle.Superscript;
			TextPos += Contents.DrawText(TextCtrl, TextPos, BaseLine, "2");

			TextCtrl.DrawStyle = DrawStyle.Normal;
			TextPos += Contents.DrawText(TextCtrl, TextPos, BaseLine, "+B");

			TextCtrl.DrawStyle = DrawStyle.Superscript;
			TextPos += Contents.DrawText(TextCtrl, TextPos, BaseLine, "2");

			TextCtrl.DrawStyle = DrawStyle.Normal;
			TextPos += Contents.DrawText(TextCtrl, TextPos, BaseLine, "=C");

			TextCtrl.DrawStyle = DrawStyle.Superscript;
			Contents.DrawText(TextCtrl, TextPos, BaseLine, "2");
			BaseLine -= LineSpacing;

			BookmarkCtrl.OpenEntries = true;
			SecondLevel.AddBookmark(BookmarkCtrl, "Unicode characters and symbols", Page, PosX, PosY + BaseLine + TextCtrl.TextAscent);

			TextCtrl.DrawStyle = DrawStyle.Normal;
			Contents.DrawText(TextCtrl, 0.1, BaseLine, "Euro over Cent: €");
			BaseLine -= 1.5 * LineSpacing;

			PdfDrawTextCtrl ComicTextCtrl = new PdfDrawTextCtrl(Comic, 24);
			Contents.DrawText(ComicTextCtrl, 0.1, BaseLine, "Comic Sans MS");
			BaseLine -= 1.5 * LineSpacing;

			SecondLevel.AddBookmark(BookmarkCtrl, "Wingdings symbols", Page, PosX, PosY + BaseLine + Symbol.Ascent(24));
			TextPos = 0.1;
			TextPos += Contents.DrawText(TextCtrl, TextPos, BaseLine, "Wingdings");

			PdfDrawTextCtrl SymbolTextCtrl = new PdfDrawTextCtrl(Symbol, 24);
			Contents.DrawText(SymbolTextCtrl, TextPos, BaseLine, "\u0022\u0024\u002a\u003a");
			BaseLine -= LineSpacing;
			TextPos = 0.1;

			SecondLevel.AddBookmark(BookmarkCtrl, "Non latin АБВГДЕ αβγδεζ", Page, PosX, PosY + BaseLine + TextCtrl.TextAscent);

			TextPos += Contents.DrawText(TextCtrl, TextPos, BaseLine, "Non-Latin: ");
			TextPos += Contents.DrawText(TextCtrl, TextPos, BaseLine, PdfContents.ReverseString("עברית"));
			TextPos += Contents.DrawText(TextCtrl, TextPos, BaseLine, " АБВГДЕ");
			Contents.DrawText(TextCtrl, TextPos, BaseLine, " αβγδεζ");

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 5b: DrawStyle examples");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 5c Text
		////////////////////////////////////////////////////////////////////
		private void Example5c
				(
				double PosX,
				double PosY
				)
			{
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialBold, 36);
			TextCtrl.TextColor = Color.Empty;
			Contents.DrawText(TextCtrl, 0.1, 2.2, 0.02, Color.RoyalBlue, "Stroke Only");

			TextCtrl.TextColor = Color.LightBlue;
			Contents.DrawText(TextCtrl, 0.1, 1.5, 0.02, Color.DarkRed, "Fill & Stroke");

			Contents.SaveGraphicsState();
			TextCtrl.TextColor = Color.Black;
			Contents.ClipText(TextCtrl, 0.1, 0.8, "Clip&Shading");

			Color[] ColorArray = new Color[] { Color.Red, Color.DarkOrange, Color.Green, Color.Turquoise, Color.Blue};
			PdfShadingFunction ShadingFunction = new PdfShadingFunction(Document, ColorArray);
			PdfAxialShading AxialShading = new PdfAxialShading(Document, ShadingFunction);
			AxialShading.BBox = new PdfRectangle(0.09, 0.5, 3.39, 1.5);
			Contents.DrawShading(AxialShading);
			Contents.RestoreGraphicsState();

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 5c: Draw text with special effects");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 5d Text
		////////////////////////////////////////////////////////////////////
		private void Example5d
				(
				double PosX,
				double PosY
				)
			{
			// save grapics state
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			// example of text with kerning potential
			string Text = "ATAWLTPAV";

			// draw text control
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(TimesNormal, 36);

			// text bounding box
			PdfRectangle Box = TextCtrl.TextBoundingBox(Text);

			// move bounding box
			double BaseLine = 0.5 * AreaHeight;
			Box = Box.Move(0.2, BaseLine);

			// draw rectangle control (1 point black border)
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();

			// draw bounding box and text without kerning
			Contents.DrawGraphics(DrawCtrl, Box);
			Contents.DrawText(TextCtrl, 0.2, BaseLine, Text);

			// move box down
			BaseLine -= 0.75;
			Box = Box.Move(0, -0.75);

			Contents.DrawGraphics(DrawCtrl, Box);
			double Width = Contents.DrawTextWithKerning(TextCtrl, 0.2, BaseLine, Text);
			LineD BorderLine = new LineD(0.2 + Width, Box.Bottom, 0.2 + Width, Box.Top); 
			Contents.DrawLine(BorderLine);

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 5d: Draw text with and without kerning");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 5e text box
		////////////////////////////////////////////////////////////////////
		private void Example5e
				(
				double PosX,
				double PosY
				)
			{
			// save graphics state and set origin
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			// draw rectangle
			PdfRectangle Rect = new PdfRectangle(0.25, 0.5, AreaWidth - 0.25, AreaHeight - 0.25);
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.BorderColor = Color.Gray;
			Contents.DrawGraphics(DrawCtrl, Rect);

			// create text box
			PdfTextBox Box = new PdfTextBox(AreaWidth - 0.5);

			// add text to the text box
			Box.AddText(new PdfDrawTextCtrl(ArialNormal, 11),
				"This area is an example of displaying text that is too long to fit within a fixed width " +
				"area. The text is displayed justified to right edge. You define a text box with the required " +
				"width and first line indent. You add text to this box. The box will divide the text into " +
				"lines. Each line is made of segments of text. For each segment, you define font, font " +
				"size, drawing style and color. After loading all the text, the program will draw the formatted text.");

			double PosYText = AreaHeight - 0.25;
			Contents.DrawText(0.25, ref PosYText, 0.5, 0, 0.015, 0.06, TextBoxJustify.FitToWidth, Box);
			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 5e: TextBox class example");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 5f Text
		////////////////////////////////////////////////////////////////////
		private void Example5f
				(
				double PosX,
				double PosY
				)
			{
			// draw three lines
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);
			double BaseLine = AreaHeight - 0.4;
			string Str = "Test word and char spacing";
			double FontSize = 14.0;
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialNormal, FontSize);
			double LineSpacing = TextCtrl.LineSpacing;

			Contents.DrawText(NoteTextCtrl, 0.1, BaseLine, "Draw text normal");
			BaseLine -= LineSpacing;
			Contents.DrawText(TextCtrl, 0.1, BaseLine, Str);
			BaseLine -= LineSpacing + 0.1;

			Contents.DrawText(NoteTextCtrl, 0.1, BaseLine, "Draw text with character spacing 0.02\"");
			BaseLine -= LineSpacing;
			Contents.SaveGraphicsState();
			Contents.SetCharacterSpacing(0.02);
			Contents.DrawText(TextCtrl, 0.1, BaseLine, Str);
			Contents.RestoreGraphicsState();
			BaseLine -= LineSpacing + 0.1;

			Contents.DrawText(NoteTextCtrl, 0.1, BaseLine, "Draw text with word spacing 0.1\"");
			BaseLine -= LineSpacing;
			Contents.SaveGraphicsState();
			Contents.SetWordSpacing(0.1);
			Contents.DrawText(TextCtrl, 0.1, BaseLine, Str);
			Contents.RestoreGraphicsState();
			BaseLine -= LineSpacing + 0.1;

			Contents.DrawText(NoteTextCtrl, 0.1, BaseLine, "Draw text with word and charater spacing");
			BaseLine -= LineSpacing;
			Contents.SaveGraphicsState();
			ArialNormal.TextFitToWidth(FontSize, AreaWidth - 0.2, out double WordSpacing, out double CharSpacing, Str);
			Contents.SetWordSpacing(WordSpacing);
			Contents.SetCharacterSpacing(CharSpacing);
			Contents.DrawText(TextCtrl, 0.1, BaseLine, Str);
			Contents.RestoreGraphicsState();

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 5f: Word and character extra spacing");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 6a Barcode 128
		////////////////////////////////////////////////////////////////////
		private void Example6a
				(
				double PosX,
				double PosY
				)
			{
			// draw three solid lines
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			double BaseLine = AreaHeight - 2.0;

			// create PDF417 barcode
			Pdf417Encoder Pdf417 = new Pdf417Encoder();
			Pdf417.DefaultDataColumns = 3;
			Pdf417.Encode(ArticleLink);
			Pdf417.WidthToHeightRatio(2.5);

			// convert Pdf417 to black and white image
			PdfImage BarcodeImage = new PdfImage(Document);
			BarcodeImage.LayerControl = BarcodeLayer;
			BarcodeImage.LoadImage(Pdf417);

			// draw image
			double Width = 3.0;
			Contents.DrawImage(BarcodeImage, 0.2, BaseLine, Width);

			// create annotation object
			PdfAnnotWebLink WebLink = new PdfAnnotWebLink(Document, ArticleLink);

			// define a web link area coinsiding with the qr code
			double Height = Width * Pdf417.ImageHeight / Pdf417.ImageWidth;
			WebLink.AnnotRect = new PdfRectangle(PosX + 0.2, PosY + BaseLine, PosX + 0.2 + Width, PosY + BaseLine + Height);

			// note
			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 6a: PDF417 Barcode");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 6b Barcode 39
		////////////////////////////////////////////////////////////////////
		private void Example6b
				(
				double PosX,
				double PosY
				)
			{
			// draw three solid lines
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			double BaseLine = AreaHeight - 1.0;

			Contents.LayerStart(BarcodeLayer);

			PdfBarcode128 Barcode128 = new PdfBarcode128("PDF File Writer");
			PdfDrawBarcodeCtrl BarcodeCtrl = new PdfDrawBarcodeCtrl();
			BarcodeCtrl.NarrowBarWidth = 0.012;
			BarcodeCtrl.Height = 0.5;
			BarcodeCtrl.TextCtrl = new PdfDrawTextCtrl(ArialNormal, 8.0);
			Contents.DrawBarcode(BarcodeCtrl, 0.25, BaseLine, Barcode128);

			BaseLine -= 1.0;
			PdfBarcode39 Barcode39 = new PdfBarcode39("123456789012");
			Contents.DrawBarcode(BarcodeCtrl, 0.25, BaseLine, Barcode39);

			Contents.LayerEnd();

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY1, "Example 6b: Barcode128 at the top");
			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "and Barcode 39 at the bottom");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 6c Barcode EAN-13 or UPC-A
		////////////////////////////////////////////////////////////////////
		private void Example6c
				(
				double PosX,
				double PosY
				)
			{
			// draw three solid lines
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			double LineSpacing = ArialNormal.LineSpacing(NoteSize);

			Contents.LayerStart(BarcodeLayer);

			double BaseLine = AreaHeight - 1.0;
			PdfBarcodeEAN13 Barcode = new PdfBarcodeEAN13("9876543210980");
			PdfDrawBarcodeCtrl BarcodeCtrl = new PdfDrawBarcodeCtrl();
			BarcodeCtrl.NarrowBarWidth = 0.014;
			BarcodeCtrl.Height = 0.75;
			BarcodeCtrl.TextCtrl = new PdfDrawTextCtrl(ArialNormal, 8.0);
			Contents.DrawBarcode(BarcodeCtrl, 0.25, BaseLine, Barcode);
			BaseLine -= 1.3 * LineSpacing;
			Contents.DrawText(NoteTextCtrl, 0.25, BaseLine, "EAN-13");

			BaseLine -= 0.9;
			PdfBarcodeEAN13 Barcode2 = new PdfBarcodeEAN13("123456789010");
			Contents.DrawBarcode(BarcodeCtrl, 0.25, BaseLine, Barcode2);
			BaseLine -= 1.3 * LineSpacing;
			Contents.DrawText(NoteTextCtrl, 0.25, BaseLine, "UPC-A");
			Contents.LayerEnd();

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY2, "Example 6c: Barcode EAN-13 and UPC-A");
			Contents.DrawText(NoteTextCtrl, NoteX, NoteY1, "UPC-A is a special case of EAN-13.");
			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "It is 12 digits or 13 digits with leading zero");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 6d Vertical Text
		////////////////////////////////////////////////////////////////////
		private void Example6d
				(
				double PosX,
				double PosY
				)
			{
			// add first level bookmark
			PdfBookmarkCtrl BookmarkCtrl = new PdfBookmarkCtrl();
			BookmarkCtrl.Zoom = 2;
			BookmarkCtrl.OpenEntries = true;
			FirstLevelBookmark.AddBookmark(BookmarkCtrl, "Example 6D QR Code and Web Link", Page, PosX, PosY + AreaHeight);

			Contents.SaveGraphicsState();

			// draw QR Code with web link to this article
			PdfQREncoder QREncoder = new PdfQREncoder();
			QREncoder.ErrorCorrection = ErrorCorrection.H;
			QREncoder.ModuleSize = 1;
			QREncoder.QuietZone = 4;
			QREncoder.Encode(ArticleLink);

			// convert QRCode to black and white image
			PdfImage BarcodeImage = new PdfImage(Document);
			BarcodeImage.LayerControl = BarcodeLayer;
			BarcodeImage.LoadImage(QREncoder);

			// draw image (height is the same as width for QRCode)
			double QRCodeWidth = AreaWidth - 1.5;
			Contents.DrawImage(BarcodeImage, PosX + 0.75, PosY + 0.6, QRCodeWidth);

			// create annotation object
			PdfAnnotWebLink WebLink = new PdfAnnotWebLink(Document, ArticleLink);

			// define a web link area coinsiding with the qr code
			WebLink.AnnotRect = new PdfRectangle(PosX + 0.75, PosY + 0.6, PosX + 0.75 + QRCodeWidth, PosY + 0.6 + QRCodeWidth);

			Contents.Translate(PosX, PosY);
			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 6d: QR Code and Web Link");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 6e text box
		////////////////////////////////////////////////////////////////////
		private void Example6e
				(
				double PosX,
				double PosY
				)
			{
			// add first level bookmark
			PdfBookmarkCtrl BookmarkCtrl = new PdfBookmarkCtrl();
			BookmarkCtrl.Zoom = 2;
			BookmarkCtrl.OpenEntries = true;
			FirstLevelBookmark.AddBookmark(BookmarkCtrl, "Example 6E Text Box and Web Link", Page, PosX, PosY + AreaHeight);

			Contents.SaveGraphicsState();
			PdfTextBox Box = new PdfTextBox(AreaWidth - 0.2);
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialNormal, 11);

			// add text to the text box
			Box.AddText(TextCtrl, "Articles by Uzi Granot\n");
			Box.AddText(TextCtrl, "Section Files and Folders, Subsection File Formats: ");

			PdfDrawTextCtrl AnnotTextCtrl = new PdfDrawTextCtrl(TextCtrl);
			AnnotTextCtrl.Annotation = new PdfAnnotWebLink(Document, ArticleLink);
			Box.AddText(AnnotTextCtrl, "PDF File Writer C# Class Library (Version 1.8)\n");

			Box.AddText(TextCtrl, "Section Files and Folders, Subsection File Formats: ");

			AnnotTextCtrl.Annotation = new PdfAnnotWebLink(Document,
				"http://www.codeproject.com/Articles/450254/PDF-File-Analyzer-With-Csharp-Parsing-Classes-Vers");
			Box.AddText(AnnotTextCtrl, "PDF File Analyzer With C# Parsing Classes (Version 1.2)\n");
	
			Box.AddText(TextCtrl, "Section Files and Folders, Subsection Compression: ");

			AnnotTextCtrl.Annotation = new PdfAnnotWebLink(Document,
				"http://www.codeproject.com/Articles/359758/Processing-Standard-Zip-Files-with-Csharp-compress");
			Box.AddText(AnnotTextCtrl, "Processing Standard Zip Files with C# compression/decompression classes\n");

			double PosYText = PosY + AreaHeight - 0.1;
			Contents.DrawText(PosX + 0.1, ref PosYText, PosY + 0.3, 0, 0.015, 0.06, TextBoxJustify.Left, Box); //, Page);

			Contents.DrawText(NoteTextCtrl, PosX + NoteX, PosY + NoteY0, "Example 6e: TextBox Web Link example");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 6f Vertical Text
		////////////////////////////////////////////////////////////////////
		private void Example6f
				(
				double PosX,
				double PosY
				)
			{
			// draw three solid lines
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialNormal, 14);
			TextCtrl.Justify = TextJustify.Center;
			double LineSpacing = TextCtrl.LineSpacing;
			double BaseLine = AreaHeight - 0.5;
			string Str = "VERTICAL";
			foreach(char C in Str)
				{
				Contents.DrawText(TextCtrl, 1.0, BaseLine, C.ToString());
				BaseLine -= LineSpacing;
				}
			BaseLine = AreaHeight - 0.5 - 2 * LineSpacing;
			Str = "TEXT";
			foreach(char C in Str)
				{
				Contents.DrawText(TextCtrl, 1.5, BaseLine, C.ToString());
				BaseLine -= LineSpacing;
				}

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 6f: Vertical Text");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 7a 
		////////////////////////////////////////////////////////////////////
		private void Example7a
				(
				double PosX,
				double PosY
				)
			{
			// available area for video display in abs coordinates
			PdfRectangle VideoArea = new PdfRectangle(PosX + 0.1, PosY + NoteY0 + NoteTextCtrl.TextAscent + 0.5,
				PosX + AreaWidth - 0.1, PosY + AreaHeight - 0.1);

			// load video file
			PdfEmbeddedFile VideoFile = PdfEmbeddedFile.CreateEmbeddedFile(Document, "LooneyTunes.mp4");

			// create annotation object for video file
			PdfAnnotDisplayMedia Media = new PdfAnnotDisplayMedia(Document, VideoFile);
		
			// define annotation rectangle that has the same aspect ratio as the video
			Media.AnnotRect = PdfImageSizePos.ImageArea(480, 360, VideoArea, ContentAlignment.MiddleCenter);

			// display media layer control
			Media.OptionalContent = AnnotationLayer;

			// create xobject with the correct width and height
			PdfXObject AnnotArea = new PdfXObject(Document, Media.AnnotRect.Width, Media.AnnotRect.Height);

			// create rectangle that is little smaller than the video area
			PdfRectangle Rect = AnnotArea.BBox.AddMargin(-0.05);

			// draw control object
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Paint = DrawPaint.BorderAndFill;
			DrawCtrl.BorderColor = Color.Indigo;
			DrawCtrl.BackgroundTexture = Color.Lavender;
			AnnotArea.DrawGraphics(DrawCtrl, Rect);

			// draw text
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialNormal, 14);
			TextCtrl.Justify = TextJustify.Center;
			TextCtrl.TextColor = Color.Indigo;
			AnnotArea.DrawText(TextCtrl, 0.5 * AnnotArea.BBox.Width, 0.5 * AnnotArea.BBox.Height, "Click here to play the video");

			// add appearance dictionary to annotation <</AP <</N Ref 0 R>>>>
			Media.AddAppearance(AnnotArea, AppearanceType.Normal);

			Media.DisplayControls = true;

			// example note
			Contents.DrawText(NoteTextCtrl, PosX + NoteX, PosY + NoteY0, "Example 7a: Play video within the document");
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 7b
		////////////////////////////////////////////////////////////////////
		private void Example7b
				(
				double PosX,
				double PosY
				)
			{
			// create display media object
			PdfEmbeddedFile Video = PdfEmbeddedFile.CreateEmbeddedFile(Document, "Omega.mp4");
			DisplayMedia = new PdfAnnotDisplayMedia(Document, Video);

			// activate display controls
			DisplayMedia.DisplayControls = true;

			// display in floating window
			DisplayMedia.MediaWindowType = MediaWindow.Floating;

			// title bar text
			DisplayMedia.FloatingWindowTitleText = "Floating Window Example";

			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialNormal, 12);
			TextCtrl.Justify = TextJustify.Center;
			double LineSpacing = TextCtrl.LineSpacing;
			double TextPosX = PosX + 0.5 * AreaWidth;
			double TextPosY = PosY + 0.5 * AreaHeight + LineSpacing;
			double TextWidth = Contents.DrawText(TextCtrl, TextPosX, TextPosY, "Click this text to play the video");
			TextPosY -= LineSpacing;
			Contents.DrawText(TextCtrl, TextPosX, TextPosY, "in a floating window");

			// create annotation rectangle over the two lines of text
			DisplayMedia.AnnotRect = new PdfRectangle(TextPosX - 0.5 * TextWidth, TextPosY - TextCtrl.TextDescent,
				TextPosX + 0.5 * TextWidth, TextPosY + TextCtrl.TextAscent + LineSpacing);

			// layer control
			DisplayMedia.OptionalContent = AnnotationLayer;

			// example note
			Contents.DrawText(NoteTextCtrl, PosX + NoteX, PosY + NoteY0, "Example 7b: Play video in a floating window");
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 7c 
		////////////////////////////////////////////////////////////////////
		private void Example7c
				(
				double PosX,
				double PosY
				)
			{
			// sticky note text
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialNormal, 12);
			TextCtrl.Justify = TextJustify.Left;
			TextCtrl.TextColor = Color.DarkBlue;
			double TextPosX = PosX + 0.1;
			double TextPosY = PosY + AreaHeight - TextCtrl.LineSpacing - 0.1;
			double TextWidth = Contents.DrawText(TextCtrl, TextPosX, TextPosY, "Sticky note");

			string[] IconName = {"Comment", "Key", "Note", "Help", "NewParagraph", "Paragraph", "Insert" };

			TextPosX += TextWidth + 0.4;
			double IconLeft = PosX + AreaWidth - 0.75;
			double IconTop = TextPosY + TextCtrl.TextAscent;
			for(int Icon = 0; Icon < 7; Icon++)
				{
				Contents.DrawText(TextCtrl, TextPosX, TextPosY, IconName[Icon]);

				// sticky note annotation
				PdfAnnotStickyNote StickyNote = new PdfAnnotStickyNote(Document, "My first sticky note", (StickyNoteIcon) Icon);
				StickyNote.AnnotRect = new PdfRectangle(IconLeft, IconTop, IconLeft, IconTop);
				StickyNote.ColorSpecific = Color.DeepPink;
				StickyNote.OptionalContent = AnnotationLayer;
				IconTop -= 0.35;
				TextPosY -= 0.35;
				}

			// example note
			Contents.DrawText(NoteTextCtrl, PosX + NoteX, PosY + NoteY0, "Example 7c: Sticky notes examples");
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 7d 
		////////////////////////////////////////////////////////////////////
		private void Example7d
				(
				double PosX,
				double PosY
				)
			{
			// display text area to activate the file attachment
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialNormal, 12);
			double LineSpacing = TextCtrl.LineSpacing;
			double TextPosX = PosX + 0.1;
			double TextPosY = PosY + AreaHeight - LineSpacing - 0.1;
			Contents.DrawText(TextCtrl, TextPosX, TextPosY, "Right click on one of the icons");
			TextPosY -= LineSpacing;
			Contents.DrawText(TextCtrl, TextPosX, TextPosY, "to open or save the attached file");

			// create embedded media file
			PdfEmbeddedFile EmbeddedFile = PdfEmbeddedFile.CreateEmbeddedFile(Document, "BookList.txt");

			string[] IconName = {"Push Pin", "Graph", "Paper Clip", "Tag"};
			double IconPosX = PosX + AreaWidth - 2;
			double IconPosY = TextPosY - 0.5;
			double IconHeight = 0.35;
			for(int Icon = 0; Icon < 4; Icon++)
				{ 
				Contents.DrawText(TextCtrl, IconPosX - 1.0, IconPosY + 0.5 * IconHeight, IconName[Icon]);

				// File attachment annotation
				double IconWidth = IconHeight * PdfAnnotFileAttachment.IconAspectRatio[Icon]; 
				PdfRectangle AnnotRect = new PdfRectangle(IconPosX, IconPosY, IconPosX + IconWidth, IconPosY + IconHeight);
				PdfAnnotation AnnotFileAttachment = new PdfAnnotFileAttachment(Document, EmbeddedFile, (FileAttachIcon) Icon);
				AnnotFileAttachment.AnnotRect = AnnotRect;
				AnnotFileAttachment.ColorSpecific = Color.DeepPink;
				IconPosY -= 0.45;
				}

			TextCtrl.Annotation = new PdfAnnotFileAttachment(Document, EmbeddedFile);
			TextCtrl.TextColor = Color.Red;
			Contents.DrawText(TextCtrl, IconPosX - 1.0, IconPosY + 0.5 * IconHeight, "Right click. No icon.");

			// example note
			Contents.DrawText(NoteTextCtrl, PosX + NoteX, PosY + NoteY0, "Example 7d: File attachment icons");
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 7e
		////////////////////////////////////////////////////////////////////
		private void Example7e
				(
				double PosX,
				double PosY
				)
			{
			// text control
			PdfDrawTextCtrl RedTextCtrl = new PdfDrawTextCtrl(ArialNormal, 12);
			RedTextCtrl.Justify = TextJustify.Center;
			RedTextCtrl.TextColor = Color.Red;

			// display text area to activate the sound
			double TextPosX = PosX + 0.5 * AreaWidth;
			double TextPosY = PosY + 0.7 * AreaHeight + RedTextCtrl.LineSpacing;
			double TextWidth = Contents.DrawText(RedTextCtrl, TextPosX, TextPosY, "Click this text to play");

			// second line of ringing sound
			TextPosY -= RedTextCtrl.LineSpacing;
			Contents.DrawText(RedTextCtrl, TextPosX, TextPosY, "Ringing sound");

			// rectangle around the two lines of text
			PdfRectangle TextRect = new PdfRectangle(TextPosX - 0.5 * TextWidth, TextPosY - RedTextCtrl.TextDescent,
				TextPosX + 0.5 * TextWidth, TextPosY + RedTextCtrl.LineSpacing + RedTextCtrl.TextAscent);

			// create embedded media file
			PdfEmbeddedFile RingSoundFile = PdfEmbeddedFile.CreateEmbeddedFile(Document, "Ring01.wav");
			PdfAnnotDisplayMedia RingSound = new PdfAnnotDisplayMedia(Document, RingSoundFile);
			RingSound.MediaWindowType = MediaWindow.Hidden;
			RingSound.AnnotRect = TextRect;

			// activate ring sound when page is visible
			//RingSound.ActivateWhenPageIsVisible();

			// embedded file
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialNormal, 12);
			TextCtrl.Justify = TextJustify.Center;
			PdfEmbeddedFile EmbeddedFile = PdfEmbeddedFile.CreateEmbeddedFile(Document, "BookList.txt");
			TextCtrl.Annotation = new PdfAnnotFileAttachment(Document, EmbeddedFile);
			TextCtrl.TextColor = Color.DarkViolet;
			Contents.DrawText(TextCtrl, PosX + 0.5 * AreaWidth, PosY + 0.4 * AreaHeight, "Right click to open or save the attached file,");

			// example note
			Contents.DrawText(NoteTextCtrl, PosX + NoteX, PosY + NoteY1, "Example 7e: Play ringing sound (top)");
			Contents.DrawText(NoteTextCtrl, PosX + NoteX, PosY + NoteY0, "Example 7e: File attachment without icon");
			return;
			}

	////////////////////////////////////////////////////////////////////
	// Example 7F text box
	////////////////////////////////////////////////////////////////////
	private void Example7f
				(
				double PosX,
				double PosY
				)
			{
			// load omega commercial video
			PdfEmbeddedFile Video = PdfEmbeddedFile.CreateEmbeddedFile(Document, "Omega.mp4");

			// create display media object
			DisplayMedia = new PdfAnnotDisplayMedia(Document, Video);

			// activate display controls
			DisplayMedia.DisplayControls = true;

			// display in floating window
			DisplayMedia.MediaWindowType = MediaWindow.Floating;

			// title bar text
			DisplayMedia.FloatingWindowTitleText = "Floating Window Example";

			// create empty text box
			PdfTextBox Box = new PdfTextBox(AreaWidth - 0.2);

			// display floating video text
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialNormal, 11);
			Box.AddText(TextCtrl, "Floating video: ");

			// display text with annotation action
			PdfDrawTextCtrl TextCtrlUnderline = new PdfDrawTextCtrl(TextCtrl);
			TextCtrlUnderline.Annotation = DisplayMedia;
			Box.AddText(TextCtrlUnderline, "Omega commercial\n");

			// create embedded media file
			PdfEmbeddedFile Audio = PdfEmbeddedFile.CreateEmbeddedFile(Document, "Ring01.wav");
			PdfAnnotDisplayMedia RingSound = new PdfAnnotDisplayMedia(Document, Audio);
			RingSound.MediaWindowType = MediaWindow.Hidden;

			// display text
			Box.AddText(TextCtrl, "Sound file: ");

			// display text with annotations
			TextCtrlUnderline.Annotation = RingSound;
			Box.AddText(TextCtrlUnderline, "Ring Tone\n");

			// display text
			Box.AddText(TextCtrl, "Activate web link: ");

			// display text with annotations
			TextCtrlUnderline.Annotation = new PdfAnnotWebLink(Document,
				"http://www.codeproject.com/Articles/359758/Processing-Standard-Zip-Files-with-Csharp-compress");
			Box.AddText(TextCtrlUnderline, "Processing Standard Zip Files with C# compression/decompression classes\n");

			// display text
			Box.AddText(TextCtrl, "Link action: ");

			// display text with annotations
			TextCtrlUnderline.Annotation = new PdfAnnotLinkAction(Document, "Page2");
			Box.AddText(TextCtrlUnderline, "Page 2\n");

			// display text
			Box.AddText(TextCtrl, "Link action: ");

			// display text with annotations
			TextCtrlUnderline.Annotation = new PdfAnnotLinkAction(Document, "Page8");
			Box.AddText(TextCtrlUnderline, "Page 8\n");

			// display text
			Box.AddText(TextCtrl, "View attached file: ");

			// create embedded media file
			PdfEmbeddedFile EmbeddedFile = PdfEmbeddedFile.CreateEmbeddedFile(Document, "BookList.txt");
			TextCtrlUnderline.Annotation = new PdfAnnotFileAttachment(Document, EmbeddedFile);
			Box.AddText(TextCtrlUnderline, "Book List\n");

			double PosYText = PosY + AreaHeight - 0.1;
			Contents.DrawText(PosX + 0.1, ref PosYText, PosY + 0.3, 0, 0.01, 0.10, TextBoxJustify.Left, Box, Page);

			Contents.DrawText(NoteTextCtrl, PosX + NoteX, PosY + NoteY0, "Example 7f: TextBox annotation actions examples");
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 8a Draw three circles fully opaque
		////////////////////////////////////////////////////////////////////
		private void Example8a
				(
				double PosX,
				double PosY
				)
			{
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			// define radius for the three circles
			double Radius = (AreaWidth - 1.0) / 3.0;

			// write the word transparency
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialBold, 36);
			TextCtrl.Justify = TextJustify.Center;
			Contents.DrawText(TextCtrl, 0.5 + 1.5 * Radius, 0.35 + Radius, "Transparency");

			// draw red circle
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Shape = DrawShape.Oval;
			DrawCtrl.Paint = DrawPaint.Fill;
			DrawCtrl.BackgroundTexture = Color.Red;
			PdfRectangle Rect = new PdfRectangle(0.5, 0.35, 0.5 + 2 * Radius, 0.35 + 2 * Radius);
			Contents.DrawGraphics(DrawCtrl, Rect);

			// draw blue circle
			DrawCtrl.BackgroundTexture = Color.Blue;
			Rect = Rect.Move(Radius, 0);
			Contents.DrawGraphics(DrawCtrl, Rect);

			// draw green circle
			DrawCtrl.BackgroundTexture = Color.Green;
			Rect = Rect.Move(- 0.5 * Radius, Radius);
			Contents.DrawGraphics(DrawCtrl, Rect);

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 8a: Draw Alpha 1.0 full opaque");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 8b Draw three circles partially opaque (alpha=0.5)
		////////////////////////////////////////////////////////////////////
		private void Example8b
				(
				double PosX,
				double PosY
				)
			{
			// draw three solid lines
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			// define radius for the three circles
			double Radius = (AreaWidth - 1.0) / 3.0;

			// write the word transparency
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialBold, 36);
			TextCtrl.Justify = TextJustify.Center;
			Contents.DrawText(TextCtrl, 0.5 + 1.5 * Radius, 0.35 + Radius, "Transparency");

			// draw red circle
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Shape = DrawShape.Oval;
			DrawCtrl.Paint = DrawPaint.Fill;
			DrawCtrl.BackgroundTexture = Color.Red;
			DrawCtrl.BackgroundAlpha = 0.5;
			PdfRectangle Rect = new PdfRectangle(0.5, 0.35, 0.5 + 2 * Radius, 0.35 + 2 * Radius);
			Contents.DrawGraphics(DrawCtrl, Rect);

			// draw blue circle
			DrawCtrl.BackgroundTexture = Color.Blue;
			Rect = Rect.Move(Radius, 0);
			Contents.DrawGraphics(DrawCtrl, Rect);

			// draw green circle
			DrawCtrl.BackgroundTexture = Color.Green;
			Rect = Rect.Move(-0.5 * Radius, Radius);
			Contents.DrawGraphics(DrawCtrl, Rect);

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 8b: Draw Alpha 0.5 transparent");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 8c Draw three circles fully opaque
		////////////////////////////////////////////////////////////////////
		private void Example8c
				(
				double PosX,
				double PosY
				)
			{
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			// define radius for the three circles
			double Radius = (AreaWidth - 1.0) / 3.0;

			// write the word transparency
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialBold, 36);
			TextCtrl.Justify = TextJustify.Center;
			Contents.DrawText(TextCtrl, 0.5 + 1.5 * Radius, 0.35 + Radius, "Transparency");

			// draw red circle
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Shape = DrawShape.Oval;
			DrawCtrl.Paint = DrawPaint.Fill;
			DrawCtrl.BackgroundTexture = Color.Red;
			DrawCtrl.BackgroundAlpha = 0.5;
			DrawCtrl.Blend = BlendMode.Difference;
			PdfRectangle Rect = new PdfRectangle(0.5, 0.35, 0.5 + 2 * Radius, 0.35 + 2 * Radius);
			Contents.DrawGraphics(DrawCtrl, Rect);

			// draw blue circle
			DrawCtrl.BackgroundTexture = Color.Blue;
			Rect = Rect.Move(Radius, 0);
			Contents.DrawGraphics(DrawCtrl, Rect);

			// draw green circle
			DrawCtrl.BackgroundTexture = Color.Green;
			Rect = Rect.Move(-0.5 * Radius, Radius);
			Contents.DrawGraphics(DrawCtrl, Rect);

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 8c: Blend mode is Difference Alpha is 0.5");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 8d Draw three circles partial opaque
		////////////////////////////////////////////////////////////////////
		private void Example8d
				(
				double PosX,
				double PosY
				)
			{
			Contents.SaveGraphicsState();
			Contents.Translate(PosX, PosY);

			// define radius for the three circles
			double Radius = (AreaWidth - 1.0) / 3.0;

			// write the word transparency
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialBold, 36);
			TextCtrl.Justify = TextJustify.Center;
			Contents.DrawText(TextCtrl, 0.5 + 1.5 * Radius, 0.35 + Radius, "Transparency");

			// draw red circle
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Shape = DrawShape.Oval;
			DrawCtrl.Paint = DrawPaint.Fill;
			DrawCtrl.BackgroundTexture = Color.Red;
			DrawCtrl.Blend = BlendMode.Screen;
			PdfRectangle Rect = new PdfRectangle(0.5, 0.35, 0.5 + 2 * Radius, 0.35 + 2 * Radius);
			Contents.DrawGraphics(DrawCtrl, Rect);

			// draw blue circle
			DrawCtrl.BackgroundTexture = Color.Blue;
			Rect = Rect.Move(Radius, 0);
			Contents.DrawGraphics(DrawCtrl, Rect);

			// draw green circle
			DrawCtrl.BackgroundTexture = Color.Green;
			Rect = Rect.Move(-0.5 * Radius, Radius);
			Contents.DrawGraphics(DrawCtrl, Rect);

			Contents.DrawText(NoteTextCtrl, NoteX, NoteY0, "Example 8d: Blend mode is screen (Alpha is 1.0)");
			Contents.RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 8e draw character as graphics
		////////////////////////////////////////////////////////////////////
		private void Example8e
				(
				double OrigX, // bottom left
				double OrigY
				)
			{
			PdfSymbol Symbol = new PdfSymbol("Wingdings 2", FontStyle.Regular, 39);
			PdfRectangle SymbolRect = new PdfRectangle(OrigX + 0.1, OrigY + NoteY1 + 0.1, OrigX + AreaWidth - 0.1, OrigY + AreaHeight - 0.1);
			Contents.DrawSymbol(Symbol, SymbolRect, Color.PaleVioletRed);

			// example note
			Contents.DrawText(NoteTextCtrl, OrigX + NoteX, OrigY + NoteY0, "Example 8e: Draw one character as a graphic shape");
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 8e draw character as graphics
		////////////////////////////////////////////////////////////////////
		private void Example8f
				(
				double OrigX, // bottom left
				double OrigY
				)
			{
			PdfSymbol Symbol = new PdfSymbol("Arial", FontStyle.Regular, 'A');
			PdfRectangle SymbolRect = new PdfRectangle(OrigX + 0.1, OrigY + NoteY1 + 0.1, OrigX + AreaWidth - 0.1, OrigY + AreaHeight - 0.1);
			Contents.DrawSymbol(Symbol, SymbolRect, Color.PaleVioletRed);

			// example note
			Contents.DrawText(NoteTextCtrl, OrigX + NoteX, OrigY + NoteY0, "Example 8e: Draw one character as a graphic shape");
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 9a Fillable form - text fields
		////////////////////////////////////////////////////////////////////
		private void Example9a
				(
				double OrigX, // bottom left
				double OrigY
				)
			{
			// constant
			const double AreaMargin = 0.1;

			// define acro form to control the fields
			PdfAcroForm AcroForm = PdfAcroForm.CreateAcroForm(Document);

			// fixed text font
			PdfDrawTextCtrl FixedTextCtrl = new PdfDrawTextCtrl(Document, "Times New Roman", FontStyle.Regular, 12);

			// data entry font
			PdfDrawTextCtrl FieldTextCtrl = new PdfDrawTextCtrl(Document, "Arial", FontStyle.Regular, 12);

			// set all ascii range is included
			FieldTextCtrl.Font.SetCharRangeActive(' ', '~');

			// field description position bottom left
			double PosX = OrigX + AreaMargin;
			double PosY = OrigY + AreaHeight - FixedTextCtrl.LineSpacing - AreaMargin;

			// field description
			Contents.DrawText(FixedTextCtrl, PosX, PosY + FixedTextCtrl.TextDescent, "Text field example");

			// frame around data entry field
			PdfDrawCtrl DrawFrame = new PdfDrawCtrl();
			DrawFrame.Paint = DrawPaint.BorderAndFill;
			double BorderWidth = 1 / Document.ScaleFactor;
			DrawFrame.BorderWidth = BorderWidth;
			DrawFrame.BorderColor = Color.LightGray;
			DrawFrame.BackgroundTexture = Color.FromArgb(255, 255, 210);

			// frame size and position
			double FrameWidth = AreaWidth - 2 * AreaMargin;
			double FrameHeight = FieldTextCtrl.LineSpacing + 4 * BorderWidth;
			PosY -= FrameHeight + 0.5 * FixedTextCtrl.LineSpacing;
			PdfRectangle FrameRect = new PdfRectangle(PosX, PosY, PosX + FrameWidth, PosY + FrameHeight);

			// draw the frame
			Contents.DrawGraphics(DrawFrame, FrameRect);

			// data entry field rectangle
			PdfRectangle FieldRect = FrameRect.AddMargin(- 2 * BorderWidth);

			// text field
			PdfAcroTextField TextField = new PdfAcroTextField(AcroForm, "TextField9a");
			TextField.AnnotRect = FieldRect;
			TextField.TextCtrl = FieldTextCtrl;
			TextField.TextMaxLength = 40;
			TextField.BackgroundColor = Color.FromArgb(240, 240, 255);

			// text field appearance
			TextField.DrawTextField();

			// field description
			PosY -= 2 * FixedTextCtrl.LineSpacing;
			Contents.DrawText(FixedTextCtrl, PosX, PosY + FixedTextCtrl.TextDescent, "Date field example");

			// date frame size and position
			double DateFrameWidth = FieldTextCtrl.TextWidth("0000/00/00 ") + 4 * BorderWidth;
			double DateFrameHeight = FieldTextCtrl.LineSpacing + 4 * BorderWidth;
			PosY -= FrameHeight + 0.5 * FixedTextCtrl.LineSpacing;
			PdfRectangle DateFrameRect = new PdfRectangle(PosX, PosY, PosX + DateFrameWidth, PosY + DateFrameHeight);

			// draw the frame
			Contents.DrawGraphics(DrawFrame, DateFrameRect);

			// data entry field rectangle
			PdfRectangle DateFieldRect = DateFrameRect.AddMargin(- 2 * BorderWidth);

			// text field
			PdfAcroTextField DateField = new PdfAcroTextField(AcroForm, "DateField9a");
			DateField.AnnotRect = DateFieldRect;
			DateField.TextCtrl = FieldTextCtrl;
			DateField.TextMaxLength = 10;
			DateField.BackgroundColor = Color.FromArgb(240, 240, 255);
			DateField.FieldFormatEvent = "AFDate_FormatEx(\"yyyy/mm/dd\");";
			DateField.FieldKeystrokeEvent = "AFDate_KeystrokeEx(\"yyyy/mm/dd\");";

			// text field appearance
			DateField.DrawTextField();

			// field description
			PosY -= 2 * FixedTextCtrl.LineSpacing;
			Contents.DrawText(FixedTextCtrl, PosX, PosY + FixedTextCtrl.TextDescent, "Numeric field example");

			// number frame size and position
			double NumberFrameWidth = FieldTextCtrl.TextWidth("123456789012.00") + 4 * BorderWidth;
			double NumberFrameHeight = FieldTextCtrl.LineSpacing + 4 * BorderWidth;
			PosY -= FrameHeight + 0.5 * FixedTextCtrl.LineSpacing;
			PdfRectangle NumberFrameRect = new PdfRectangle(PosX, PosY, PosX + NumberFrameWidth, PosY + NumberFrameHeight);

			// draw the frame
			Contents.DrawGraphics(DrawFrame, NumberFrameRect);

			// data entry field rectangle
			PdfRectangle NumberFieldRect = NumberFrameRect.AddMargin(- 2 * BorderWidth);

			// text field
			PdfAcroTextField NumberField = new PdfAcroTextField(AcroForm, "NumberField9a");
			NumberField.AnnotRect = NumberFieldRect;
			NumberField.TextCtrl = FieldTextCtrl;
			NumberField.TextMaxLength = 15;
			NumberField.BackgroundColor = Color.FromArgb(240, 240, 255);
			NumberField.FieldFormatEvent = "AFNumber_Format(2, 0, 0, 0, '$', true);";
			NumberField.FieldKeystrokeEvent = "AFNumber_Keystroke(2, 0, 0, 0, '$', true);";

			// text field appearance
			NumberField.DrawTextField();

			// example note
			Contents.DrawText(NoteTextCtrl, OrigX + NoteX, OrigY + NoteY0, "Example 9a: Draw text field");
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Example 9b Fillable form - check boxes
		////////////////////////////////////////////////////////////////////
		private void Example9b
				(
				double OrigX, // bottom left
				double OrigY
				)
			{
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

			// constant
			const double AreaMargin = 0.1;

			// define acro form to control the fields
			PdfAcroForm AcroForm = PdfAcroForm.CreateAcroForm(Document);

			// fixed text font
			PdfDrawTextCtrl FixedTextCtrl = new PdfDrawTextCtrl(Document, "Times New Roman", FontStyle.Regular, 12);

			// field description position bottom left
			double PosX = OrigX + AreaMargin;
			double PosY = OrigY + AreaHeight - FixedTextCtrl.LineSpacing - AreaMargin;

			// field description
			Contents.DrawText(FixedTextCtrl, PosX, PosY + FixedTextCtrl.TextDescent, "Checkboxes examples");

			// y position
			PosY -= 1.4 * FixedTextCtrl.LineSpacing;

			// display all available checkbox symbols
			double CheckBoxSide = FixedTextCtrl.LineSpacing;
			double SavePosY = PosY;
			for(int Index = 0; Index < 10; Index++)
				{
				// index number as text
				string IndexStr = (Index + 1).ToString();

				// check box location and size
				PdfRectangle CheckBoxRect = new PdfRectangle(PosX, PosY, PosX + CheckBoxSide, PosY + CheckBoxSide);

				// checkbox field
				PdfAcroCheckBoxField CheckBox = new PdfAcroCheckBoxField(AcroForm, "CheckBox" + IndexStr, "Select" + IndexStr);
				CheckBox.AnnotRect = CheckBoxRect;
				CheckBox.CheckMarkChar = ZaDiChar[Index];
				CheckBox.CheckMarkColor = Color.FromArgb(0, 0, 128);
				CheckBox.BackgroundColor = Color.FromArgb(240, 240, 255);
				CheckBox.BorderColor = Color.FromArgb(0, 0, 128);

				// call auto appearance method and save appearance stream
				CheckBox.DrawCheckBox(AppearanceType.Normal, false);
				CheckBox.DrawCheckBox(AppearanceType.Normal, true);

				// check box 1 caption
				Contents.DrawText(FixedTextCtrl, PosX + CheckBoxSide + 0.1, PosY + FixedTextCtrl.TextDescent, IndexStr + ". " + CheckMarkText[Index]);

				// next line
				if(Index != 4)
					{ 
					PosY -= CheckBoxSide + 0.05;
					}
				// new column
				else
					{
					PosY = SavePosY;
					PosX = OrigX + 0.5 * AreaWidth;
					}
				}

			// radio buttons example
			string[] ButtonDescription = {"Male", "Female", "Other" };

			// radio buttons group description
			PosX = OrigX + AreaMargin;
			PosY -= 0.05;
			Contents.DrawText(FixedTextCtrl, PosX + 0.04, PosY + FixedTextCtrl.TextDescent, "Radio buttons example");

			// frame around data entry field
			PdfDrawCtrl DrawFrame = new PdfDrawCtrl();
			DrawFrame.Paint = DrawPaint.BorderAndFill;
			double BorderWidth = 1 / Document.ScaleFactor;
			DrawFrame.BorderWidth = BorderWidth;
			DrawFrame.BorderColor = Color.LightGray;
			DrawFrame.BackgroundTexture = Color.FromArgb(255, 255, 210);

			// number of buttons
			int Buttons = ButtonDescription.Length;

			// radio button size
			double RadioButtonSide = FixedTextCtrl.LineSpacing;

			// frame top position
			PosY -= 0.04;

			// frame width
			double FrameWidth = 1.2;

			// frame height
			double FrameHeight = Buttons * RadioButtonSide + (Buttons + 3) * 0.04 + 2 * BorderWidth;

			// frame rectangle
			PdfRectangle FrameRect = new PdfRectangle(PosX, PosY - FrameHeight, PosX + FrameWidth, PosY);

			// draw frame
			Contents.DrawGraphics(DrawFrame, FrameRect);

			// draw gray area within the frame
			PdfRectangle FrameActiveRect = FrameRect.AddMargin(-0.04);
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Paint = DrawPaint.Fill;
			DrawCtrl.BackgroundTexture = Color.FromArgb(240, 240, 255);
			Contents.DrawGraphics(DrawCtrl, FrameActiveRect);

			// position of first button
			PosX = FrameActiveRect.Left + 0.04;
			PosY = FrameActiveRect.Top - 0.04 - RadioButtonSide;

			// draw buttons
			for(int Index = 0; Index < Buttons; Index++)
				{
				PdfRectangle RadioButtonRect = new PdfRectangle(PosX, PosY, PosX + RadioButtonSide, PosY + RadioButtonSide);
				AddRadioButton(AcroForm, RadioButtonRect, "Gender", ButtonDescription[Index], Index == 0);

				// check box A1 caption
				Contents.DrawText(FixedTextCtrl, PosX + RadioButtonSide + 3 * 0.04, PosY + FixedTextCtrl.TextDescent, ButtonDescription[Index]);

				// adjust y position
				PosY -= RadioButtonSide + 0.04;
				}

			// example note
			Contents.DrawText(NoteTextCtrl, OrigX + NoteX, OrigY + NoteY0, "Example 9b: Checkboxes examples");
			return;
			}

		private static void AddRadioButton
				(
				PdfAcroForm AcroForm,
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

		////////////////////////////////////////////////////////////////////
		// Example 9c Fillable form - combobox and list box
		////////////////////////////////////////////////////////////////////
		private void Example9c
				(
				double OrigX, // bottom left
				double OrigY
				)
			{
			// constant
			const double AreaMargin = 0.1;

			// define acro form to control the fields
			PdfAcroForm AcroForm = PdfAcroForm.CreateAcroForm(Document);

			// fixed text font
			PdfDrawTextCtrl FixedTextCtrl = new PdfDrawTextCtrl(Document, "Times New Roman", FontStyle.Regular, 12);

			// data entry font
			PdfDrawTextCtrl FieldTextCtrl = new PdfDrawTextCtrl(Document, "Arial", FontStyle.Regular, 12);

			// set all ascii range is included
			FieldTextCtrl.Font.SetCharRangeActive(' ', '~');

			// field description position bottom left
			double PosX = OrigX + AreaMargin;
			double PosY = OrigY + AreaHeight - FixedTextCtrl.LineSpacing - AreaMargin;

			// field description
			Contents.DrawText(FixedTextCtrl, PosX, PosY + FixedTextCtrl.TextDescent, "Editable combo-box field example");

			// color choices
			string[] ColorChoices = {"Red", "Orange", "Yellow", "Green", "Blue", "Indigo", "Violet"};

			// frame around data entry field
			PdfDrawCtrl DrawFrame = new PdfDrawCtrl();
			DrawFrame.Paint = DrawPaint.BorderAndFill;
			double BorderWidth = 1 / Document.ScaleFactor;
			DrawFrame.BorderWidth = BorderWidth;
			DrawFrame.BorderColor = Color.LightGray;
			DrawFrame.BackgroundTexture = Color.FromArgb(255, 255, 210);

			// frame size and position
			double FrameWidth = 10 * FieldTextCtrl.CharWidth('X') + 4 * BorderWidth;
			double FrameHeight = FieldTextCtrl.LineSpacing + 4 * BorderWidth;
			PosY -= FrameHeight + 0.5 * FixedTextCtrl.LineSpacing;
			PdfRectangle FrameRect = new PdfRectangle(PosX, PosY, PosX + FrameWidth, PosY + FrameHeight);

			// draw the frame
			Contents.DrawGraphics(DrawFrame, FrameRect);

			// data entry field rectangle
			PdfRectangle FieldRect = FrameRect.AddMargin(- 2 * BorderWidth);

			// combo box field
			PdfAcroComboBoxField Field = new PdfAcroComboBoxField(AcroForm, "ComboboxField9c");
			Field.AnnotRect = FieldRect;
			Field.TextCtrl = FieldTextCtrl;
			Field.BackgroundColor = Color.FromArgb(240, 240, 255);
			Field.Edit = true;
			Field.FieldValue = ColorChoices[0];
			Field.Items = ColorChoices;

			// combo box field appearance
			Field.DrawComboBox();

			// field description position bottom left
			PosY -= 3 * FixedTextCtrl.LineSpacing;

			// field description
			Contents.DrawText(FixedTextCtrl, PosX, PosY + FixedTextCtrl.TextDescent, "List-box field example");

			// List box select color rectangle
			double FontSize = 12.0;
			double LineSpacing = 1.2 * FontSize / Document.ScaleFactor;
			double ListBoxHeight = 4 * LineSpacing;
			PosY -= FixedTextCtrl.LineSpacing + ListBoxHeight;
			PdfRectangle ListColorRect = new PdfRectangle(PosX, PosY, PosX + 2, PosY + ListBoxHeight);

			// draw frame
			Contents.DrawGraphics(DrawFrame, ListColorRect.AddMargin(1 / Contents.ScaleFactor));

			// font
			PdfFontTypeOne Helvetica = PdfFontTypeOne.CreateFontTypeOne(Document, TypeOneFontCode.Helvetica);

			// field
			PdfAcroListBoxField ListBoxField = new PdfAcroListBoxField(AcroForm, "ListSelectColor", Helvetica, FontSize);

			ListBoxField.AnnotRect = ListColorRect;
			ListBoxField.BackgroundColor = Color.FromArgb(240, 240, 255);
			ListBoxField.FieldValue = ColorChoices[0];
			ListBoxField.Items = ColorChoices;
			ListBoxField.TopIndex = 0;

			// list box field appearance
			ListBoxField.DrawListBox();

			// example note
			Contents.DrawText(NoteTextCtrl, OrigX + NoteX, OrigY + NoteY0, "Example 9c: Draw combo-box and list-box fields");
			return;
			}
		}
	}
