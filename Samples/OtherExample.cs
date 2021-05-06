/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	OtherExample
//	Produce PDF document when Other Example button is clicked.
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

using System;
using System.Diagnostics;
using System.Drawing;
using PdfFileWriter;
using SysMedia = System.Windows.Media;
using SysWin = System.Windows;

namespace TestPdfFileWriter
{
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
	private       double NoteY1;
	private       double NoteY2;
	private const double NoteSize = 10;

	private PdfDocument	Document;
	private PdfPage		Page;
	private PdfContents BaseContents;
	private PdfContents Contents;
	private PdfFont		ArialNormal;
	private PdfFont		ArialBold;
	private PdfFont		ArialItalic;
	private PdfFont		ArialBoldItalic;
	private PdfFont		TimesNormal;
	private PdfFont		TimesBold;
	private PdfFont		TimesItalic;
	private PdfFont		TimesBoldItalic;
	private PdfFont		LucidaNormal;
	private PdfFont		Comic;
	private PdfFont		Symbol;

	private PdfBookmark BookmarkRoot;
	private PdfBookmark FirstLevelBookmark;
	private double		FirstLevelYPos;

	private PdfLayer ImageLayer;
	private PdfLayer BarcodeLayer;
	private PdfLayer AnnotationLayer;

	private static string ArticleLink = "http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version";

	////////////////////////////////////////////////////////////////////
	// create test PDF document
	////////////////////////////////////////////////////////////////////
	
	public void Test
			(
			bool Debug,
			string FileName
			)
		{
		// create document (letter size, portrait, inches)
		using(Document = new PdfDocument(PageWidth, PageHeight, UnitOfMeasure.Inch, FileName))
			{
			// set or clear debug flag
			Document.Debug = Debug;

			// set encryption
//			Document.SetEncryption("password", Permission.All & ~Permission.Print);

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
			ArialNormal = PdfFont.CreatePdfFont(Document, "Arial", FontStyle.Regular, true);
			ArialBold = PdfFont.CreatePdfFont(Document, "Arial", FontStyle.Bold, true);
			ArialItalic = PdfFont.CreatePdfFont(Document, "Arial", FontStyle.Italic, true);
			ArialBoldItalic = PdfFont.CreatePdfFont(Document, "Arial", FontStyle.Bold | FontStyle.Italic, true);
			TimesNormal = PdfFont.CreatePdfFont(Document, "Times New Roman", FontStyle.Regular, true);
			TimesBold = PdfFont.CreatePdfFont(Document, "Times New Roman", FontStyle.Bold, true);
			TimesItalic = PdfFont.CreatePdfFont(Document, "Times New Roman", FontStyle.Italic, true);
			TimesBoldItalic = PdfFont.CreatePdfFont(Document, "Times New Roman", FontStyle.Bold | FontStyle.Italic, true);
	//		LucidaNormal = PdfFont.CreatePdfFont(Document, "Courier New", FontStyle.Regular, true);
			LucidaNormal = PdfFont.CreatePdfFont(Document, "Lucida Console", FontStyle.Regular, true);
			Comic = PdfFont.CreatePdfFont(Document, "Comic Sans MS", FontStyle.Regular, true);
			Symbol = PdfFont.CreatePdfFont(Document, "Wingdings", FontStyle.Regular, true);

			NoteY1 = NoteY0 + ArialNormal.LineSpacing(NoteSize);
			NoteY2 = NoteY1 + ArialNormal.LineSpacing(NoteSize);

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

			// create pdf file
			Document.CreateFile();

			// start default PDF reader and display the file
			Process Proc = new Process();
			Proc.StartInfo = new ProcessStartInfo(FileName);
			Proc.Start();
			}

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// create table of contents
	////////////////////////////////////////////////////////////////////

	public void CreateTableOfContents()
		{
		Page = new PdfPage(Document);
		Contents = new PdfContents(Page);
		Page.AddLocationMarker("TableOfContents", LocMarkerScope.LocalDest, DestFit.FitH, 10.5);

		// save graphics state
		Contents.SaveGraphicsState();

		// frame
		Contents.SetLineWidth(0.02);
		Contents.SetColorStroking(Color.Black);
		Contents.DrawRectangle(AreaX1, AreaY1, DispWidth, DispHeight, PaintOp.CloseStroke);

		// heading
		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialBold, 24, 0.5 * PageWidth, PageHeight - Margin - (HeadingHeight - ArialBold.CapHeight(24)) / 2,
			TextJustify.Center, "PDF File Writer Example");

		double PosX = AreaX1 + 0.5;
		double PosY = AreaY4 - 2.0;
		double LineHeight = ArialItalic.LineSpacing(14.0) + 0.1;
		Contents.DrawText(TimesBold, 16, 0.5 * PageWidth, PosY, TextJustify.Center, "Table of Contents");
		PosY -= LineHeight + 0.2;
		Contents.DrawTextWithAnnotation(Page, ArialItalic, 14, PosX, PosY, "Page 1 Lines Rectangles Bezier", new AnnotLinkAction("Page1"));
		PosY -= LineHeight;
		Contents.DrawTextWithAnnotation(Page, ArialItalic, 14, PosX, PosY, "Page 2 Bezier and Image", new AnnotLinkAction("Page2"));
		PosY -= LineHeight;
		Contents.DrawTextWithAnnotation(Page, ArialItalic, 14, PosX, PosY, "Page 3 Bezier and Line Caps", new AnnotLinkAction("Page3"));
		PosY -= LineHeight;
		Contents.DrawTextWithAnnotation(Page, ArialItalic, 14, PosX, PosY, "Page 4 Shading and Patterns", new AnnotLinkAction("Page4"));
		PosY -= LineHeight;
		Contents.DrawTextWithAnnotation(Page, ArialItalic, 14, PosX, PosY, "Page 5 Text and TextBox", new AnnotLinkAction("Page5"));
		PosY -= LineHeight;
		Contents.DrawTextWithAnnotation(Page, ArialItalic, 14, PosX, PosY, "Page 6 Barcode QR Code and Web Link", new AnnotLinkAction("Page6"));
		PosY -= LineHeight;
		Contents.DrawTextWithAnnotation(Page, ArialItalic, 14, PosX, PosY, "Page 7 Media, Sound and attached files", new AnnotLinkAction("Page7"));
		PosY -= LineHeight;
		Contents.DrawTextWithAnnotation(Page, ArialItalic, 14, PosX, PosY, "Page 8 Transparency, Blend and WPF graphics", new AnnotLinkAction("Page8"));

		// restore graphics state
		Contents.RestoreGraphicsState();

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// create base contents for all pages
	////////////////////////////////////////////////////////////////////

	public void CreateBaseContents()
		{
		// create unattached contents
		BaseContents = new PdfContents(Document);

		// save graphics state
		BaseContents.SaveGraphicsState();

		// frame
		BaseContents.SetLineWidth(0.02);
		BaseContents.SetColorStroking(Color.Black);
		BaseContents.DrawRectangle(AreaX1, AreaY1, DispWidth, DispHeight, PaintOp.CloseStroke);
		BaseContents.DrawLine(AreaX2, AreaY1, AreaX2, AreaY4);
		BaseContents.DrawLine(AreaX1, AreaY2, AreaX3, AreaY2);
		BaseContents.DrawLine(AreaX1, AreaY3, AreaX3, AreaY3);

		// heading
		BaseContents.SetColorNonStroking(Color.Black);
		BaseContents.DrawText(ArialBold, 24, 0.5 * PageWidth, PageHeight - Margin - (HeadingHeight - ArialBold.CapHeight(24)) / 2,
			TextJustify.Center, "PDF File Writer Example");

		// restore graphics state
		BaseContents.RestoreGraphicsState();

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// create page1 contents
	////////////////////////////////////////////////////////////////////

	public void CreatePage1Contents()
		{
		// add contents to page
		Contents = AddPageToDocument(1);

		// bookmark
		FirstLevelYPos = PageHeight - 0.75 + ArialNormal.Ascent(10);
		FirstLevelBookmark = BookmarkRoot.AddBookmark("Page 1 Lines Rectangles Bezier", Page, 0.0, FirstLevelYPos, 1.0, Color.Red, PdfBookmark.TextStyle.Bold, false);

		// location markers
		Page.AddLocationMarker("Page1", LocMarkerScope.LocalDest, DestFit.FitH, 10.5);
		Page.AddLocationMarker("MidPage1", LocMarkerScope.LocalDest, DestFit.FitH, 5.5);

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

	public void CreatePage2Contents()
		{
		// add contents to page
		Contents = AddPageToDocument(2);

		// location markers
		Page.AddLocationMarker("Page2", LocMarkerScope.LocalDest, DestFit.FitH, 10.5);

		// bookmark
		FirstLevelBookmark = BookmarkRoot.AddBookmark("Page 2 Bezier and Image", Page, 0.0, FirstLevelYPos, 1.0, Color.Red, PdfBookmark.TextStyle.Bold, false);

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

	public void CreatePage3Contents()
		{
		// add contents to page
		Contents = AddPageToDocument(3);

		// location markers
		Page.AddLocationMarker("Page3", LocMarkerScope.NamedDest, DestFit.FitH, 10.5);

		// bookmark
		FirstLevelBookmark = BookmarkRoot.AddBookmark("Page 3 Bezier and Line Caps", Page, 0.0, FirstLevelYPos, 1.0, Color.Red, PdfBookmark.TextStyle.Bold, false);

		// draw examples
		Example3a(AreaX1, AreaY3, 0.25, 0.5, 0.75, 2.2,  2.75, 1.9, 3.25, 0.9, "Example 3a: Cubic Bezier with both control points", "on the same side of the curve");
		Example3a(AreaX2, AreaY3, 0.25, 0.5, 0.75, 2.2,  2.75, .75, 3.25, 2.75, "Example 3b: Cubic Bezier with control points", "on the two sides of the curve");
		Example3a(AreaX1, AreaY2, 0.25, 0.5, 0, 0,  3.25, 2.9, 2.75, 0.9, "Example 3c: Cubic Bezier with end point P0", "equals control point P1");
		Example3a(AreaX2, AreaY2, 0.25, 0.5, 0.75, 2.2,  0, 0, 3.25, 2.75, "Example 3d: Cubic Bezier with control point P2", "equals end point P3");
		Example3e(AreaX1, AreaY1);
		Example3f(AreaX2, AreaY1);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// create page4 contents
	////////////////////////////////////////////////////////////////////

	public void CreatePage4Contents()
		{
		// add contents to page
		Contents = AddPageToDocument(4);

		// location markers
		Page.AddLocationMarker("Page4", LocMarkerScope.NamedDest, DestFit.FitH, 10.5);

		// bookmark
		FirstLevelBookmark = BookmarkRoot.AddBookmark("Page 4 Shading and Patterns", Page, 0.0, FirstLevelYPos, 1.0, Color.Red, PdfBookmark.TextStyle.Bold, false);

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

	public void CreatePage5Contents()
		{
		// add contents to page
		Contents = AddPageToDocument(5);

		// location markers
		Page.AddLocationMarker("Page5", LocMarkerScope.NamedDest, DestFit.FitH, 10.5);

		// bookmark
		FirstLevelBookmark = BookmarkRoot.AddBookmark("Page 5 Text and TextBox", Page, 0.0, FirstLevelYPos, 1.0, Color.Red, PdfBookmark.TextStyle.Bold, true);

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

	public void CreatePage6Contents()
		{
		// add contents to page
		Contents = AddPageToDocument(6);

		// location markers
		Page.AddLocationMarker("Page6", LocMarkerScope.NamedDest, DestFit.FitH, 10.5);

		// bookmark
		FirstLevelBookmark = BookmarkRoot.AddBookmark("Page 6 Barcode QR Code and Web Link", Page, 0.0, FirstLevelYPos, 1.0, Color.Red, PdfBookmark.TextStyle.Bold, false);

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

	public void CreatePage7Contents()
		{
		// add contents to page
		Contents = AddPageToDocument(7);

		// location markers
		Page.AddLocationMarker("Page7", LocMarkerScope.NamedDest, DestFit.FitH, 10.5);

		// bookmark
		FirstLevelBookmark = BookmarkRoot.AddBookmark("Page 7 Media, Sound and attached files", Page, 0.0, FirstLevelYPos, 1.0, Color.Red, PdfBookmark.TextStyle.Bold, false);

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
	// create page7 contents
	////////////////////////////////////////////////////////////////////

	public void CreatePage8Contents()
		{
		// add contents to page
		Contents = AddPageToDocument(8);

		// location markers
		Page.AddLocationMarker("Page8", LocMarkerScope.NamedDest, DestFit.FitH, 10.5);

		// bookmark
		FirstLevelBookmark = BookmarkRoot.AddBookmark("Page 8 Transparency, Blend and WPF graphics", Page, 0.0, FirstLevelYPos, 1.0, Color.Red, PdfBookmark.TextStyle.Bold, false);

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
	// Add page to document and draw page number
	////////////////////////////////////////////////////////////////////

	public PdfContents AddPageToDocument
			(
			int PageNo
			)
		{
		// add new page with two contents objects
		Page = new PdfPage(Document);
		Page.AddContents(BaseContents);
		PdfContents Contents = new PdfContents(Page);

		// draw page number right justified
		Contents.SaveGraphicsState();
		Contents.DrawText(ArialNormal, 10, PageWidth - Margin, PageHeight - 0.75 - ArialNormal.Descent(10), TextJustify.Right, string.Format("Page {0}", PageNo));
		Contents.RestoreGraphicsState();

		Contents.DrawTextWithAnnotation(Page, ArialItalic, 12, AreaX3, AreaY1 - 0.18, TextJustify.Right, DrawStyle.Underline,
			Color.DarkRed, "Table of contents", new AnnotLinkAction("TableOfContents"));
		return Contents;
		}

	////////////////////////////////////////////////////////////////////
	// Example 1a Draw three solid lines
	////////////////////////////////////////////////////////////////////

	public void Example1a
			(
			double PosX,
			double PosY
			)
		{
		// draw three solid lines
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SetLineWidth(0.05);
		Contents.SetColorStroking(Color.Red);
		Contents.DrawLine(0.25, 1.0, AreaWidth - 0.25, 1.0);
		Contents.SetLineWidth(0.1);
		Contents.SetColorStroking(Color.Green);
		Contents.DrawLine(1.5, 0.5, 1.5, AreaHeight - 0.25);
		Contents.SetLineWidth(0.15);
		Contents.SetColorStroking(Color.DarkBlue);
		Contents.DrawLine(0.5, AreaHeight - 0.5, AreaWidth - 0.25, 0.25);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 1a: Draw solid lines");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 1b Three dashed lines
	////////////////////////////////////////////////////////////////////

	public void Example1b
			(
			double PosX,
			double PosY
			)
		{
		// draw three lines
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SetLineWidth(0.05);
		Contents.SetDashLine(new double[] {0.05, 0.05, 0.1, 0.05}, 0);
		Contents.SetColorStroking(Color.Red);
		Contents.DrawLine(0.25, 1.0, AreaWidth - 0.25, 1.0);
		Contents.SetLineWidth(0.1);
		Contents.SetDashLine(new double[] {0.05, 0.05}, 0);
		Contents.SetColorStroking(Color.Green);
		Contents.DrawLine(1.5, 0.5, 1.5, AreaHeight - 0.25);
		Contents.SetLineWidth(0.15);
		Contents.SetDashLine(new double[] {0.15, 0.02, 0.3, 0.02}, 0);
		Contents.SetColorStroking(Color.DarkBlue);
		Contents.DrawLine(0.5, AreaHeight - 0.5, AreaWidth - 0.25, 0.25);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 1b: Draw dashed lines");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 1c Three rectangles: stroke, fill, fill and stroke
	////////////////////////////////////////////////////////////////////

	public void Example1c
			(
			double PosX,
			double PosY
			)
		{
		// draw three rectangles
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SetLineWidth(0.1);
		Contents.SetColorStroking(Color.DarkBlue);
		Contents.SetColorNonStroking(Color.Turquoise);
		Contents.DrawRectangle(0.2, 2.0, 1.0, 0.7, PaintOp.CloseStroke);
		Contents.DrawRectangle(0.8, 1.2, 1.2, 0.7, PaintOp.Fill);
		Contents.DrawRectangle(1.4, 0.4, 1.4, 0.7, PaintOp.CloseFillStroke);
		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 1c: Rectangles: stroke, fill, fill+stroke");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 1d Rounded rectangle
	////////////////////////////////////////////////////////////////////

	public void Example1d
			(
			double PosX,
			double PosY
			)
		{
		// draw rounded rectangle
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SetLineWidth(0.06);
		Contents.SetColorNonStroking(Color.Cyan);
		Contents.SetColorStroking(Color.Purple);
		Contents.DrawRoundedRectangle(0.2, 1.7, 2.5, 1.0, 0.25, PaintOp.CloseFillStroke);
		Contents.DrawInwardCornerRectangle(0.8, 0.4, 2.5, 1.0, 0.25, PaintOp.CloseFillStroke);
		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 1d: Draw Rounded Rectangle (fill+stroke)");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 1e Ellipse and circle
	////////////////////////////////////////////////////////////////////

	public void Example1e
			(
			double PosX,
			double PosY
			)
		{
		// draw ellipse
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		double X = 0.5 * AreaWidth;
		double Y = 0.5 * AreaHeight;

		Contents.SetColorNonStroking(Color.Yellow);
		Contents.DrawOval(X - 1.5, Y - 1.0, 3.0, 2.0, PaintOp.Fill);

		Contents.SetLineWidth(0.2);
		Contents.SetColorNonStroking(Color.White);
		Contents.SetColorStroking(Color.Black);
		Contents.DrawOval(X - 0.75, Y, 0.5, 0.5, PaintOp.CloseFillStroke);
		Contents.DrawOval(X + 0.25, Y, 0.5, 0.5, PaintOp.CloseFillStroke);

		Contents.SetColorNonStroking(Color.Black);
		Contents.MoveTo(X - 0.6, Y - 0.4);
		Contents.LineTo(X + 0.6, Y - 0.4);
		Contents.DrawBezier(X, Y - 0.8, X, Y - 0.8, X - 0.6, Y - 0.4);
		Contents.SetPaintOp(PaintOp.Fill);

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 1e: Ellipse, two circles, line and Bezier");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 1f polygon
	////////////////////////////////////////////////////////////////////

	public void Example1f
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
		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 1f: polygon");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 2a polygon
	////////////////////////////////////////////////////////////////////

	public void Example2a
			(
			double PosX,
			double PosY
			)
		{
		// add first level bookmark
		FirstLevelBookmark.AddBookmark("Example 2A Polygons", Page, PosX, PosY + AreaHeight, 2.0, false);

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
			Fill = Color.FromArgb((Fill.R * 8)/ 10, Fill.G, Fill.B);
			}

		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 2a: Regular polygons.");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "From 8 sides to 5 sides");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 2b polygon
	////////////////////////////////////////////////////////////////////

	public void Example2b
			(
			double PosX,
			double PosY
			)
		{
		// add first level bookmark
		FirstLevelBookmark.AddBookmark("Example 2B Stars", Page, PosX, PosY + AreaHeight, 2.0, false);

		// translate origin
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);

		PointD Center = new PointD(0.5 * AreaWidth, NoteY2 + 0.5 * (AreaHeight - NoteY2));

		Color Fill = Color.HotPink;
		for(int Index = 0; Index < 4; Index++)
			{
			Contents.SetColorNonStroking(Fill);
			Contents.DrawStar(Center, (0.4 - 0.1 * Index) * AreaHeight, 90, 8 - Index, PaintOp.CloseFillStroke);
			Fill = Color.FromArgb((Fill.R * 8)/ 10, Fill.G, Fill.B);
			}

		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 2b: Star shape polygons.");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "From 8 sides to 5 sides");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 2c polygon
	////////////////////////////////////////////////////////////////////

	public void Example2c
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
			Contents.DrawHeart(new LineD(new PointD(Center, Radius2, Alpha), new PointD(Center, Radius1, Alpha)), PaintOp.Fill);
			Contents.DrawDoubleBezierPath(new LineD(new PointD(Center, Radius3, Alpha), new PointD(Center, Radius2, Alpha)), 0.5, 0.5 * Math.PI, 0.5, 1.5 * Math.PI, PaintOp.Fill);
			Contents.DrawStar(new PointD(Center, Radius4, Alpha), Radius3 - Radius4, Alpha, 5, PaintOp.Fill);
			Alpha += DeltaAlpha;
			}

		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 2c: 18 spokes with heart");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "double Bezier path and a star");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 2d polygon
	////////////////////////////////////////////////////////////////////

	public void Example2d
			(
			double PosX,
			double PosY
			)
		{
		// translate origin
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);

		Contents.SetColorNonStroking(Color.HotPink);
		Contents.DrawHeart(0.5 * AreaWidth, NoteY2, 0.5 * AreaWidth, AreaHeight - 0.6, PaintOp.CloseFillStroke);

		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 2d: Heart shape made of two");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Symmetric Bezier curves");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 2e Image
	////////////////////////////////////////////////////////////////////

	public void Example2e
			(
			double PosX,
			double PosY
			)
		{
		// add first level bookmark
		FirstLevelBookmark.AddBookmark("Example 2E JPEG Image file", Page, PosX, PosY + AreaHeight, 2.0, false);

		// translate origin
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);

		// OLD PdfImage test
		//PdfImageControl ImageControl = new PdfImageControl();
		//ImageControl.Resolution = 300.0;
		//PdfImage Image1 = new PdfImage(Document, "TestImage.jpg", ImageControl);

		// image resource
		PdfImage Image1 = new PdfImage(Document);
		Image1.Resolution = 300.0;
		Image1.LayerControl = ImageLayer;
		Image1.LoadImage("TestImage.jpg");

		// adjust image size and position
		PdfRectangle NewSize = Image1.ImageSizePosition(0.9 * AreaWidth, 0.9 * AreaHeight, ContentAlignment.MiddleCenter);

		// draw image		
		Contents.SaveGraphicsState();
		Contents.DrawImage(Image1, 0.05 * AreaWidth + NewSize.Left, 0.05 * AreaHeight + NewSize.Bottom, NewSize.Width, NewSize.Height);
		Contents.RestoreGraphicsState();

		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 2e: JPEG image file");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 2f Image
	////////////////////////////////////////////////////////////////////

	public void Example2f
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
		PdfRectangle NewSize = Image1.ImageSizePosition(0.9 * AreaWidth, 0.9 * AreaHeight, ContentAlignment.MiddleCenter);

		// draw image		
		Contents.SaveGraphicsState();
		Contents.DrawImage(Image1, 0.05 * AreaWidth + NewSize.Left, 0.05 * AreaHeight + NewSize.Bottom, NewSize.Width, NewSize.Height);
		Contents.RestoreGraphicsState();

		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 2f: The same image but cropped");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 3a to 3d Bezier curves
	////////////////////////////////////////////////////////////////////

	public void Example3a
			(
			double PosX,
			double PosY,
			double X0,
			double Y0,
			double X1,
			double Y1,
			double X2,
			double Y2,
			double X3,
			double Y3,
			string Notes1,
			string Notes2
			)
		{
		// draw Bezier
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SetLineWidth(0.01);
		Contents.SetColorStroking(Color.Red);
		DrawCross(X0, Y0);
		DrawCross(X3, Y3);
		if(X1 != 0) DrawCross(X1, Y1);
		if(X2 != 0) DrawCross(X2, Y2);
		Contents.SetDashLine(new double[] {0.05, 0.05}, 0);
		if(X1 != 0) Contents.DrawLine(X0, Y0, X1, Y1);
		if(X2 != 0) Contents.DrawLine(X2, Y2, X3, Y3);
		Contents.SetDashLine(null, 0);
		Contents.SetLineWidth(0.03);
		Contents.SetColorStroking(Color.SteelBlue);
		Contents.MoveTo(X0, Y0);
		if(X1 == 0) Contents.DrawBezierNoP1(X2, Y2, X3, Y3);
		else if(X2 == 0) Contents.DrawBezierNoP2(X1, Y1, X3, Y3);
		else Contents.DrawBezier(X1, Y1, X2, Y2, X3, Y3);
		Contents.SetPaintOp(PaintOp.Stroke);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, Notes1);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, Notes2);
		Contents.RestoreGraphicsState();
		return;
		}
	
	////////////////////////////////////////////////////////////////////
	// Draw small cross
	////////////////////////////////////////////////////////////////////

	public void DrawCross
			(
			double PosX,
			double PosY
			)
		{
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.DrawLine(-0.1, 0, 0.1, 0);
		Contents.DrawLine(0, -0.1, 0, 0.1);
		Contents.RestoreGraphicsState();
		}

	////////////////////////////////////////////////////////////////////
	// Example 3e Tiled colored pattern
	////////////////////////////////////////////////////////////////////

	public void Example3e
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
		Contents.DrawText(ArialNormal, NoteSize, NoteX, Y, "Line cap: butt");
		Contents.DrawLine(1.4, Y, 2.8, Y);
		Y -= Dy;
		Contents.SetLineCap(PdfLineCap.Square);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, Y, "Line cap: square");
		Contents.DrawLine(1.4, Y, 2.8, Y);
		Y -= Dy;
		Contents.SetLineCap(PdfLineCap.Round);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, Y, "Line cap: round");
		Contents.DrawLine(1.4, Y, 2.8, Y);

		Contents.SetLineWidth(0.01);
		Contents.SetColorStroking(Color.Red);
		Contents.SetDashLine(new double[] {0.05, 0.05}, 0);
		Contents.DrawLine(1.4, 2.9, 1.4, Y - 0.1);
		Contents.DrawLine(2.8, 2.9, 2.8, Y - 0.1);
		Contents.SetDashLine(null, 0);

		Dy = 0.6;
		Y -= Dy;
		Contents.SetLineWidth(0.12);
		Contents.SetColorStroking(Color.DarkBlue);
		Contents.SetLineCap(PdfLineCap.Square);
		Contents.SetLineJoin(PdfLineJoin.Miter);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, Y, "Line join: miter");
		Contents.DrawPolygon(new float[] {1.4F, (float) Y, 2.8F, (float) Y, 2.1F, (float) Y + 0.35F}, PaintOp.Stroke);
		Y -= Dy;
		Contents.SetLineJoin(PdfLineJoin.Bevel);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, Y, "Line join: bevel");
		Contents.DrawPolygon(new float[] {1.4F, (float) Y, 2.8F, (float) Y, 2.1F, (float) Y + 0.35F}, PaintOp.Stroke);
		Y -= Dy;
		Contents.SetLineJoin(PdfLineJoin.Round);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, Y, "Line join: round");
		Contents.DrawPolygon(new float[] {1.4F, (float) Y, 2.8F, (float) Y, 2.1F, (float) Y + 0.35F}, PaintOp.Stroke);
		Y -= Dy;

		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 3e: Line cap and line join");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 3f Line cap line join
	////////////////////////////////////////////////////////////////////

	public void Example3f
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
		Contents.DrawText(ArialNormal, NoteSize, NoteX, Y, "Miter limit 5.8");
		Contents.DrawPolygon(new float[] {1.4F, (float) Y, 2.8F, (float) Y, 2.1F, (float) Y + 0.25478F}, PaintOp.Stroke);
		Y -= Dy;
		Contents.SetMiterLimit(5.7);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, Y, "Miter limit 5.7");
		Contents.DrawPolygon(new float[] {1.4F, (float) Y, 2.8F, (float) Y, 2.1F, (float) Y + 0.25478F}, PaintOp.Stroke);

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 3f: Miter limit for 20\u00b0. If miter limit is");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "less than 5.759 it is bevel join, otherwise it is miter join.");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 4a axial shading operator
	////////////////////////////////////////////////////////////////////

	public void Example4a
			(
			double PosX,
			double PosY
			)
		{
		// define shading function with 5 samples
		Color[] ColorArray = new Color[] {Color.Red, Color.DarkOrange, Color.Green, Color.Turquoise, Color.Blue};
		PdfShadingFunction ShadingFunction = new PdfShadingFunction(Document, ColorArray);

		// define axial shading object with default horizontal shading axis
		PdfAxialShading AxialShading = new PdfAxialShading(Document, 0.25, 0.4, 3, 2.35, ShadingFunction);

		// draw the shading object
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.DrawShading(AxialShading);

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 4a: Horizontal Axial shading");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 4b axial shading operator
	////////////////////////////////////////////////////////////////////

	public void Example4b
			(
			double PosX,
			double PosY
			)
		{
		// define shading function with 2 samples
		Color[] ColorArray = new Color[] {Color.Red, Color.HotPink};
		PdfShadingFunction ShadingFunction = new PdfShadingFunction(Document, ColorArray);

		// create axial shading object
		PdfAxialShading AxialShading = new PdfAxialShading(Document, 0.25, 0.3, 3, 0.9 * AreaHeight, ShadingFunction);

		// set shading axial direction to vertical
		AxialShading.SetAxisDirection(0.25, 0.3, 0, 2.45, MappingMode.Absolute);

		// translate origin
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);

		// heart shape clip
		Contents.SaveGraphicsState();
		Contents.DrawHeart(0.5 * AreaWidth, 0.2 * AreaHeight, 0.5 * AreaWidth, 0.75 * AreaHeight, PaintOp.ClipPathEor);

		// draw shading
		Contents.DrawShading(AxialShading);
		Contents.RestoreGraphicsState();

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 4b: Vertical Axial shading clipped by two");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "symmetric Bezier curves.");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 15 axial shading operator
	////////////////////////////////////////////////////////////////////

	public void Example4c
			(
			double PosX,
			double PosY
			)
		{
		Color[] ColorArray = new Color[]
			{Color.FromArgb(255, 200, 255), Color.FromArgb(200, 255, 255),
			Color.FromArgb(255, 200, 255), Color.FromArgb(200, 255, 255),
			Color.FromArgb(255, 200, 255), Color.FromArgb(200, 255, 255),
			Color.FromArgb(255, 200, 255), Color.FromArgb(200, 255, 255),
			Color.FromArgb(255, 200, 255), Color.FromArgb(200, 255, 255),
			};
		PdfShadingFunction ShadingFunction = new PdfShadingFunction(Document, ColorArray);
		PdfAxialShading AxialShading = new PdfAxialShading(Document, 0.25, 0.5, 3, 2.35, ShadingFunction);
		AxialShading.SetAxisDirection(0.25, 2.75, 3, -3 * Math.Tan(30.0 * Math.PI / 180.0), MappingMode.Absolute);

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

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 4c: Diagonal Axial shading");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Shading axis is from top-left to bottom-right");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 4d axial shading operator
	////////////////////////////////////////////////////////////////////

	public void Example4d
			(
			double PosX,
			double PosY
			)
		{
		Color[] ColorArray = new Color[] {Color.Red, Color.DarkOrange, Color.Green, Color.Turquoise, Color.Blue};
		PdfShadingFunction ShadingFunction = new PdfShadingFunction(Document, ColorArray);
		PdfRadialShading RadialShading = new PdfRadialShading(Document, 0.25, 0.5, 3, 2.35, ShadingFunction);

		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.DrawShading(RadialShading);
		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 4d: Radial shading. One large circle and");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "a second concentric circle with zero radius.");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 4e axial shading operator
	////////////////////////////////////////////////////////////////////

	public void Example4e
			(
			double PosX,
			double PosY
			)
		{
		// define patterns
		PdfTilingPattern BrickPattern = PdfTilingPattern.SetBrickPattern(Document, 0.25, Color.LightYellow, Color.SandyBrown);

		// draw rounded rectangle
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SetPatternNonStroking(BrickPattern);
		Contents.SetLineWidth(0.04);
		Contents.SetColorStroking(Color.Purple);
		Contents.DrawRoundedRectangle(0.25, 0.5, 3.0, 2.25, 0.3, PaintOp.CloseFillStroke);
		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 4e: Brick pattern");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 4f Tiled colored pattern
	////////////////////////////////////////////////////////////////////

	public void Example4f
			(
			double PosX,
			double PosY
			)
		{
		// define patterns
		PdfTilingPattern WeavePattern = PdfTilingPattern.SetWeavePattern(Document, 0.3, Color.Black, Color.Beige, Color.Yellow);

		// draw rounded rectangle
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SetPatternNonStroking(WeavePattern);
		Contents.SetLineWidth(0.04);
		Contents.SetColorStroking(Color.Purple);
		Contents.DrawRoundedRectangle(0.25, 0.5, 3.0, 2.25, 0.3, PaintOp.CloseFillStroke);
		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 4f: Weave pattern");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 5a Text
	////////////////////////////////////////////////////////////////////

	public void Example5a
			(
			double PosX,
			double PosY
			)
		{
		// add first level bookmark
		FirstLevelBookmark.AddBookmark("Example 5A Draw text", Page, PosX, PosY + AreaHeight, 2.0, true);

		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		double FontSize = 14.0;
		double BaseLine = AreaHeight - 0.4;
		Contents.DrawText(ArialNormal, FontSize, 0.1, BaseLine, TextJustify.Left, "Arial normal ABCD abcd 1234");
		BaseLine -= ArialBold.LineSpacing(FontSize);
		Contents.DrawText(ArialBold, FontSize, 0.1, BaseLine, "Arial bold ABCD abcd 1234");
		BaseLine -= ArialItalic.LineSpacing(FontSize);
		Contents.DrawText(ArialItalic, FontSize, 0.1, BaseLine, "Arial Italic ABCD abcd 1234");
		BaseLine -= ArialBoldItalic.LineSpacing(FontSize);
		Contents.DrawText(ArialBoldItalic, FontSize, 0.1, BaseLine, "Arial bold-italic ABCD abcd 1234");

		FontSize = 12.0;
		BaseLine -= 0.2 + TimesNormal.LineSpacing(FontSize);
		Contents.DrawText(TimesNormal, FontSize, 0.1, BaseLine, "Times New Roman normal ABCD abcd 1234");
		BaseLine -= TimesBold.LineSpacing(FontSize);
		Contents.DrawText(TimesBold, FontSize, 0.1, BaseLine, "Times New Roman bold ABCD abcd 1234");
		BaseLine -= TimesItalic.LineSpacing(FontSize);
		Contents.DrawText(TimesItalic, FontSize, 0.1, BaseLine, "Times New Roman Italic ABCD abcd 1234");
		BaseLine -= TimesBoldItalic.LineSpacing(FontSize);
		Contents.DrawText(TimesBoldItalic, FontSize, 0.1, BaseLine, TextJustify.Left, "Times New Roman bold-italic ABCD abcd 1234");

		string Str = "Lucida Consol normal ABCD abcd 1234";
		FontSize = 10.0;
		BaseLine -= 0.2 + LucidaNormal.LineSpacing(FontSize);
		Contents.DrawText(LucidaNormal, FontSize, 0.1, BaseLine, Str);

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 5a: Draw text using Arial,");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Times New Roman and Lucida Consol fonts.");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 5b Text
	////////////////////////////////////////////////////////////////////

	public void Example5b
			(
			double PosX,
			double PosY
			)
		{
		// add first level bookmark
		PdfBookmark SecondLevel = FirstLevelBookmark.AddBookmark("Example 5B Draw text with style", Page, PosX, PosY + AreaHeight, 2.0, false);

		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		double FontSize = 14.0;
		double LineSpacing = ArialNormal.LineSpacing(FontSize);
		double BaseLine = AreaHeight - 0.4;
		Contents.DrawText(ArialNormal, FontSize, 0.1, BaseLine, DrawStyle.Underline, "Underline text example");
		BaseLine -= LineSpacing;

		Contents.DrawText(ArialNormal, FontSize, 0.1, BaseLine, DrawStyle.Strikeout, "Strikeout text example");
		BaseLine -= LineSpacing;

		Contents.DrawText(ArialNormal, FontSize, 0.1, BaseLine, DrawStyle.Strikeout | DrawStyle.Underline, "Underline and strikeout combination");
		BaseLine -= LineSpacing;

		double TextPos = 0.1;
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, "Subscript example: H");
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, DrawStyle.Subscript, "2");
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, "O");
		BaseLine -= LineSpacing;

		TextPos = 0.1;
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, "Superscript example: A");
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, DrawStyle.Superscript, "2");
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, "+B");
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, DrawStyle.Superscript, "2");
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, "=C");
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, DrawStyle.Superscript, "2");
		BaseLine -= LineSpacing;

		SecondLevel.AddBookmark("Unicode characters and symbols", Page, PosX, PosY + BaseLine + ArialNormal.Ascent(FontSize), 2.0, true);

		Contents.DrawText(ArialNormal, FontSize, 0.1, BaseLine, "Euro over Cent: €");
		BaseLine -= 1.5 * LineSpacing;

		Contents.DrawText(Comic, 24, 0.1, BaseLine, "Comic Sans MS");
		BaseLine -= 1.5 * LineSpacing;

		SecondLevel.AddBookmark("Wingdings symbols", Page, PosX, PosY + BaseLine + Symbol.Ascent(24), 2.0, true);
		TextPos = 0.1;
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, "Wingdings");
		TextPos += Contents.DrawText(Symbol, 24, TextPos, BaseLine, "\u0022\u0024\u002a\u003a");
		BaseLine -= LineSpacing;
		TextPos = 0.1;
		SecondLevel.AddBookmark("Non latin АБВГДЕ αβγδεζ", Page, PosX, PosY + BaseLine + ArialNormal.Ascent(FontSize), 2.0, true);
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, "Non-Latin: ");
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, Contents.ReverseString( "עברית"));
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, " АБВГДЕ");
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, " αβγδεζ");

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 5b: DrawStyle examples");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 5c Text
	////////////////////////////////////////////////////////////////////

	public void Example5c
			(
			double PosX,
			double PosY
			)
		{
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.DrawText(ArialBold, 36.0, 0.1, 2.2, TextJustify.Left, 0.02, Color.RoyalBlue, Color.Empty, "Stroke Only");
		Contents.DrawText(ArialBold, 36.0, 0.1, 1.5, TextJustify.Left, 0.02, Color.DarkRed, Color.LightBlue, "Fill & Stroke");

		Contents.SaveGraphicsState();
		Contents.ClipText(ArialBold, 36.0, 0.1, 0.8, "Clip&Shading");
		Color[] ColorArray = new Color[] {Color.Red, Color.DarkOrange, Color.Green, Color.Turquoise, Color.Blue};
		PdfShadingFunction ShadingFunction = new PdfShadingFunction(Document, ColorArray);
		PdfAxialShading AxialShading = new PdfAxialShading(Document, 0.09, 0.5, 3.3, 1.0, ShadingFunction);
		Contents.DrawShading(AxialShading);
		Contents.RestoreGraphicsState();

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 5c: Draw text with special effects");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 5d Text
	////////////////////////////////////////////////////////////////////

	public void Example5d
			(
			double PosX,
			double PosY
			)
		{
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		double FontSize = 36.0;
		double BaseLine = AreaHeight / 2;
		string Text = "ATAWLTPAV";
		PdfRectangle Box = TimesNormal.TextBoundingBox(FontSize, Text);
		Contents.DrawRectangle(0.2 + Box.Left, BaseLine + Box.Bottom, Box.Right - Box.Left, Box.Top - Box.Bottom, PaintOp.CloseStroke);
		Contents.DrawText(TimesNormal, FontSize, 0.2, BaseLine, Text);

		BaseLine -= 0.75;
		Contents.DrawRectangle(0.2 + Box.Left, BaseLine + Box.Bottom, Box.Right - Box.Left, Box.Top - Box.Bottom, PaintOp.CloseStroke);
		double Width = Contents.DrawTextWithKerning(TimesNormal, FontSize, 0.2, BaseLine, Text);
		Contents.DrawLine(0.2 + Width, BaseLine + Box.Bottom, 0.2 + Width, BaseLine + Box.Top);

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 5d: Draw text with and without kerning");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 5e text box
	////////////////////////////////////////////////////////////////////

	public void Example5e
			(
			double PosX,
			double PosY
			)
		{
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SetColorStroking(Color.Gray);
		Contents.DrawRectangle(0.25, 0.5, AreaWidth - 0.5, AreaHeight - 0.75, PaintOp.CloseStroke);
		TextBox Box = new TextBox(AreaWidth - 0.5);
 
		// add text to the text box
		Box.AddText(ArialNormal, 11,
			"This area is an example of displaying text that is too long to fit within a fixed width " +
			"area. The text is displayed justified to right edge. You define a text box with the required " +
			"width and first line indent. You add text to this box. The box will divide the text into " + 
			"lines. Each line is made of segments of text. For each segment, you define font, font " +
			"size, drawing style and color. After loading all the text, the program will draw the formatted text.");

		double PosYText = AreaHeight - 0.25;
		Contents.DrawText(0.25, ref PosYText, 0.5, 0, 0.015, 0.06, TextBoxJustify.FitToWidth, Box);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 5e: TextBox class example");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 5f Text
	////////////////////////////////////////////////////////////////////

	public void Example5f
			(
			double PosX,
			double PosY
			)
		{
		// draw three lines
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		double FontSize = 14.0;
		double BaseLine = AreaHeight - 0.4;
		double LineSpacing = ArialBold.LineSpacing(FontSize);
		string Str = "Test word and char spacing";

		Contents.DrawText(ArialNormal, NoteSize, 0.1, BaseLine, "Draw text normal");
		BaseLine -= LineSpacing;
		Contents.DrawText(ArialNormal, FontSize, 0.1, BaseLine, TextJustify.Left, Str);
		BaseLine -= LineSpacing + 0.1;

		Contents.DrawText(ArialNormal, NoteSize, 0.1, BaseLine, "Draw text with character spacing 0.02\"");
		BaseLine -= LineSpacing;
		Contents.SaveGraphicsState();
		Contents.SetCharacterSpacing(0.02);
		Contents.DrawText(ArialNormal, FontSize, 0.1, BaseLine, TextJustify.Left, Str);
		Contents.RestoreGraphicsState();
		BaseLine -= LineSpacing + 0.1;

		Contents.DrawText(ArialNormal, NoteSize, 0.1, BaseLine, "Draw text with word spacing 0.1\"");
		BaseLine -= LineSpacing;
		Contents.SaveGraphicsState();
		Contents.SetWordSpacing(0.1);
		Contents.DrawText(ArialNormal, FontSize, 0.1, BaseLine, TextJustify.Left, Str);
		Contents.RestoreGraphicsState();
		BaseLine -= LineSpacing + 0.1;

		Contents.DrawText(ArialNormal, NoteSize, 0.1, BaseLine, "Draw text with word and charater spacing");
		BaseLine -= LineSpacing;
		Contents.SaveGraphicsState();
		ArialNormal.TextFitToWidth(FontSize, AreaWidth - 0.2, out double WordSpacing, out double CharSpacing, Str);
		Contents.SetWordSpacing(WordSpacing);
		Contents.SetCharacterSpacing(CharSpacing);
		Contents.DrawText(ArialNormal, FontSize, 0.1, BaseLine, TextJustify.Left, Str);
		Contents.RestoreGraphicsState();

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 5f: Word and character extra spacing");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 6a Barcode 128
	////////////////////////////////////////////////////////////////////

	public void Example6a
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

		// define a web link area coinsiding with the qr code
		double Height = Width * Pdf417.ImageHeight / Pdf417.ImageWidth;
		Page.AddWebLink(PosX + 0.2, PosY + BaseLine, PosX + 0.2 + Width, PosY + BaseLine + Height, ArticleLink);

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 6a: PDF417 Barcode");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 6b Barcode 39
	////////////////////////////////////////////////////////////////////

	public void Example6b
			(
			double PosX,
			double PosY
			)
		{
		// draw three solid lines
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);

		double BaseLine = AreaHeight - 1.0;
		double LineSpacing = ArialNormal.LineSpacing(NoteSize);

		Contents.LayerStart(BarcodeLayer);

		Barcode128 Barcode128 = new Barcode128("PDF File Writer");
		Contents.DrawBarcode(0.25, BaseLine, 0.012, 0.5, Barcode128, ArialNormal, 8.0);

		BaseLine -= 1.0;
		Barcode39 Barcode39 = new Barcode39("123456789012");
		Contents.DrawBarcode(0.25, BaseLine, 0.012, 0.5, Barcode39, ArialNormal, 8.0);

		Contents.LayerEnd();

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 6b: Barcode128 at the top");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "and Barcode 39 at the bottom");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 6a Barcode EAN-13 or UPC-A
	////////////////////////////////////////////////////////////////////

	public void Example6c
			(
			double PosX,
			double PosY
			)
		{
		// draw three solid lines
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);

		double BaseLine = AreaHeight - 1.0;
		double LineSpacing = ArialNormal.LineSpacing(NoteSize);

		Contents.LayerStart(BarcodeLayer);
		BarcodeEAN13 Barcode = new BarcodeEAN13("9876543210980");
		Contents.DrawBarcode(0.25, BaseLine, 0.014, 0.75, Barcode, ArialNormal, 8.0);
		BaseLine -= 1.3 * LineSpacing;
		Contents.DrawText(ArialNormal, NoteSize, 0.25, BaseLine, "EAN-13");
		BaseLine -= 0.9;
		BarcodeEAN13 Barcode2 = new BarcodeEAN13("123456789010");
		Contents.DrawBarcode(0.25, BaseLine, 0.014, 0.75, Barcode2, ArialNormal, 8.0);
		BaseLine -= 1.3 * LineSpacing;
		Contents.DrawText(ArialNormal, NoteSize, 0.25, BaseLine, "UPC-A");
		Contents.LayerEnd();

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY2, "Example 6c: Barcode EAN-13 and UPC-A");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "UPC-A is a special case of EAN-13.");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "It is 12 digits or 13 digits with leading zero");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 6d Vertical Text
	////////////////////////////////////////////////////////////////////

	public void Example6d
			(
			double PosX,
			double PosY
			)
		{
		// add first level bookmark
		FirstLevelBookmark.AddBookmark("Example 6D QR Code and Web Link", Page, PosX, PosY + AreaHeight, 2.0, true);

		Contents.SaveGraphicsState();

		// draw QR Code with web link to this article
		QREncoder QREncoder = new QREncoder();
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

		// define a web link area coinsiding with the qr code
		Page.AddWebLink(PosX + 0.75, PosY + 0.6, PosX + 0.75 + QRCodeWidth, PosY + 0.6 + QRCodeWidth, ArticleLink);

		Contents.Translate(PosX, PosY);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 6d: QR Code and Web Link");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 6e text box
	////////////////////////////////////////////////////////////////////

	public void Example6e
			(
			double PosX,
			double PosY
			)
		{
		// add first level bookmark
		FirstLevelBookmark.AddBookmark("Example 6E Text Box and Web Link", Page, PosX, PosY + AreaHeight, 2.0, true);

		Contents.SaveGraphicsState();
		TextBox Box = new TextBox(AreaWidth - 0.2);
 
		// add text to the text box
		Box.AddText(ArialNormal, 11, "Articles by Uzi Granot\n");
		Box.AddText(ArialNormal, 11, "Section Files and Folders, Subsection File Formats: ");
		Box.AddText(ArialNormal, 11, DrawStyle.Underline, Color.Blue, "PDF File Writer C# Class Library (Version 1.8)\n", ArticleLink);
		Box.AddText(ArialNormal, 11, "Section Files and Folders, Subsection File Formats: ");
		Box.AddText(ArialNormal, 11, "PDF File Analyzer With C# Parsing Classes (Version 1.2)\n",
			"http://www.codeproject.com/Articles/450254/PDF-File-Analyzer-With-Csharp-Parsing-Classes-Vers");
		Box.AddText(ArialNormal, 11, "Section Files and Folders, Subsection Compression: ");
		Box.AddText(ArialNormal, 11, "Processing Standard Zip Files with C# compression/decompression classes\n",
			"http://www.codeproject.com/Articles/359758/Processing-Standard-Zip-Files-with-Csharp-compress");

		double PosYText = PosY + AreaHeight - 0.1;
		Contents.DrawText(PosX + 0.1, ref PosYText, PosY + 0.3, 0, 0.015, 0.06, TextBoxJustify.Left, Box, Page);

		Contents.DrawText(ArialNormal, NoteSize, PosX + NoteX, PosY + NoteY0, "Example 6e: TextBox Web Link example");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 6f Vertical Text
	////////////////////////////////////////////////////////////////////

	public void Example6f
			(
			double PosX,
			double PosY
			)
		{
		// draw three solid lines
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);

		double LineSpacing = ArialNormal.LineSpacing(14);
		double BaseLine = AreaHeight - 0.5;
		string Str = "VERTICAL";
		foreach(char C in Str)
			{
			Contents.DrawText(ArialNormal, 14, 1.0, BaseLine, TextJustify.Center, C.ToString());
			BaseLine -= LineSpacing;
			}
		BaseLine = AreaHeight - 0.5 - 2 * LineSpacing;
		Str = "TEXT";
		foreach(char C in Str)
			{
			Contents.DrawText(ArialNormal, 14, 1.5, BaseLine, TextJustify.Center, C.ToString());
			BaseLine -= LineSpacing;
			}

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 6f: Vertical Text");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 7a 
	////////////////////////////////////////////////////////////////////

	public void Example7a
			(
			double PosX,
			double PosY
			)
		{
		// available area for video display in abs coordinates
		double AreaLeft = PosX + 0.1;
		double AreaBottom = PosY + NoteY0 + ArialNormal.AscentPlusLeading(NoteSize) + 0.1;
		double AreaRight = PosX + AreaWidth - 0.1;
		double AreaTop = PosY + AreaHeight - 0.1;

		// define annotation rectangle that has the same aspect ratio as the video
		PdfRectangle AnnotRect = ImageSizePos.ImageArea(480, 360, AreaLeft, AreaBottom, AreaRight - AreaLeft, AreaTop - AreaBottom, ContentAlignment.MiddleCenter);

		// create display media object
		PdfDisplayMedia DisplayMedia = new PdfDisplayMedia(PdfEmbeddedFile.CreateEmbeddedFile(Document, "LooneyTunes.mp4"));

		// create annotation object
		PdfAnnotation Annotation = Page.AddScreenAction(AnnotRect, DisplayMedia);

		// display media layer control
		Annotation.LayerControl = AnnotationLayer;

		// activate the video when the page becomes visible
//		Annotation.ActivateActionWhenPageIsVisible(true);

		// define X Object to paint the annotation area when the video is not playing
		PdfXObject AnnotArea = AnnotationArea(AnnotRect.Width, AnnotRect.Height, Color.Lavender, Color.Indigo, "Click here to play the video");
		Annotation.Appearance(AnnotArea);

		// example note
		Contents.DrawText(ArialNormal, NoteSize, PosX + NoteX, PosY + NoteY0, "Example 7a: Play video within the document");
		return;
		}

	private PdfXObject AnnotationArea
			(
			double Width,
			double Height,
			Color FillColor,
			Color BorderColor,
			string Text
			)
		{
		PdfXObject XObject = new PdfXObject(Document, Width, Height);
		XObject.SaveGraphicsState();
		XObject.SetColorNonStroking(FillColor);
		XObject.SetColorStroking(BorderColor);
		XObject.DrawRectangle(0.02 * Width, 0.02 * Height, 0.96 * Width, 0.96 * Height, PaintOp.CloseFillStroke);
		XObject.SetColorNonStroking(BorderColor);
		XObject.DrawText(ArialNormal, 14.0, 0.5 * Width, 0.5 * Height, TextJustify.Center, Text);
		XObject.RestoreGraphicsState();
		return XObject;
		}

	////////////////////////////////////////////////////////////////////
	// Example 7b
	////////////////////////////////////////////////////////////////////

	public void Example7b
			(
			double PosX,
			double PosY
			)
		{
		// create display media object
		PdfDisplayMedia DisplayMedia = new PdfDisplayMedia(PdfEmbeddedFile.CreateEmbeddedFile(Document, "Omega.mp4"));

		// activate display controls
		DisplayMedia.DisplayControls(true);

		// repeat video indefinitly
		DisplayMedia.RepeatCount(0);

		// display in floating window
		DisplayMedia.SetMediaWindow(MediaWindow.Floating, 640, 360, WindowPosition.Center,
			WindowTitleBar.TitleBarWithCloseButton, WindowResize.KeepAspectRatio, "Floating Window Example");

		double LineSpacing = ArialNormal.LineSpacing(12.0);
		double TextPosX = PosX + 0.5 * AreaWidth;
		double TextPosY = PosY + 0.5 * AreaHeight + LineSpacing;
		double TextWidth = Contents.DrawText(ArialNormal, 12.0, TextPosX, TextPosY, TextJustify.Center, "Click this text to play video");
		TextPosY -= LineSpacing;
		Contents.DrawText(ArialNormal, 12.0, TextPosX, TextPosY, TextJustify.Center, "in floating window");

		// create annotation object
		PdfRectangle AnnotRect = new PdfRectangle(TextPosX - 0.5 * TextWidth, TextPosY - ArialNormal.DescentPlusLeading(12.0),
			TextPosX + 0.5 * TextWidth, TextPosY + ArialNormal.AscentPlusLeading(12.0) + LineSpacing);
		PdfAnnotation Annotation = Page.AddScreenAction(AnnotRect, DisplayMedia);

		// layer control
		Annotation.LayerControl = AnnotationLayer;

		// example note
		Contents.DrawText(ArialNormal, NoteSize, PosX + NoteX, PosY + NoteY0, "Example 7b: Play video in a floating window");
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 7c 
	////////////////////////////////////////////////////////////////////

	public void Example7c
			(
			double PosX,
			double PosY
			)
		{
		// create embedded media file
		PdfDisplayMedia DisplayMedia = new PdfDisplayMedia(PdfEmbeddedFile.CreateEmbeddedFile(Document, "Ring01.wav"));
		DisplayMedia.SetMediaWindow(MediaWindow.Hidden);
		AnnotDisplayMedia RingSound = new AnnotDisplayMedia(DisplayMedia);

		// display text area to activate the sound
		double LineSpacing = ArialNormal.LineSpacing(12.0);
		double TextPosX = PosX + 0.5 * AreaWidth;
		double TextPosY = PosY + 0.7 * AreaHeight + LineSpacing;
		Contents.DrawTextWithAnnotation(Page, ArialNormal, 12.0, TextPosX, TextPosY,
			TextJustify.Center, DrawStyle.Normal, Color.Red, "Click this text to play", RingSound);
		TextPosY -= LineSpacing;
		Contents.DrawTextWithAnnotation(Page, ArialNormal, 12.0, TextPosX, TextPosY,
			TextJustify.Center, DrawStyle.Normal, Color.Red, "Ringing sound", RingSound);

		Contents.DrawText(ArialNormal, 12.0, PosX + 0.5, PosY + 0.4 * AreaHeight, "Sticky note example");

		// sticky note annotation
		PdfAnnotation StickyNote = Page.AddStickyNote(PosX + AreaWidth - 1.0, PosY + 0.5 * AreaHeight, "My first sticky note", StickyNoteIcon.Note);
		StickyNote.LayerControl = AnnotationLayer;

		// example note
		Contents.DrawText(ArialNormal, NoteSize, PosX + NoteX, PosY + NoteY1, "Example 7c: Play ringing sound (top)");
		Contents.DrawText(ArialNormal, NoteSize, PosX + NoteX, PosY + NoteY0, "Example 7c: Sticky note (bottom)");
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 7d 
	////////////////////////////////////////////////////////////////////

	public void Example7d
			(
			double PosX,
			double PosY
			)
		{
		// create embedded media file
		PdfEmbeddedFile EmbeddedFile = PdfEmbeddedFile.CreateEmbeddedFile(Document, "BookList.txt");
		AnnotFileAttachment FileIcon = new AnnotFileAttachment(EmbeddedFile, FileAttachIcon.Paperclip);

		// display text area to activate the file attachment
		double LineSpacing = ArialNormal.LineSpacing(12.0);
		double TextPosX = PosX + 0.5 * AreaWidth;
		double TextPosY = PosY + 0.5 * AreaHeight + LineSpacing;
		Contents.DrawText(ArialNormal, 12.0, TextPosX, TextPosY, TextJustify.Center, "Right click on the paper clip");
		TextPosY -= LineSpacing;
		double TextWidth = Contents.DrawText(ArialNormal, 12.0, TextPosX, TextPosY, TextJustify.Center, "to open or save the attached file");

		// annotation
		double IconPosX = TextPosX + 0.5 * TextWidth + 0.1;
		double IconPosY = TextPosY;
		PdfRectangle AnnotRect = new PdfRectangle(IconPosX, IconPosY, IconPosX + 0.15, IconPosY + 0.4);
		Page.AddFileAttachment(AnnotRect, EmbeddedFile, FileAttachIcon.Paperclip);

		TextPosY -= 2 * LineSpacing;
		AnnotFileAttachment FileText = new AnnotFileAttachment(EmbeddedFile, FileAttachIcon.NoIcon);

		Contents.DrawTextWithAnnotation(Page, ArialNormal, 12.0,
			TextPosX, TextPosY, TextJustify.Center, DrawStyle.Underline, Color.Red, "File attachment (right click)", FileText);

		// example note
		Contents.DrawText(ArialNormal, NoteSize, PosX + NoteX, PosY + NoteY0, "Example 7d: File attachment");
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 7e
	////////////////////////////////////////////////////////////////////

	public void Example7e
			(
			double PosX,
			double PosY
			)
		{
		Contents.DrawTextWithAnnotation(Page, ArialNormal, 12, PosX + 0.5 * AreaWidth, PosY + 0.5 * AreaHeight,
			TextJustify.Center, DrawStyle.Normal, Color.DarkViolet,
			"Right click to open or save the attached file,",
			new AnnotFileAttachment(PdfEmbeddedFile.CreateEmbeddedFile(Document, "BookList.txt")));

		// example note
		Contents.DrawText(ArialNormal, NoteSize, PosX + NoteX, PosY + NoteY0, "Example 7e: File attachment without icon");
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 7F text box
	////////////////////////////////////////////////////////////////////

	public void Example7f
			(
			double PosX,
			double PosY
			)
		{
		Contents.SaveGraphicsState();
		TextBox Box = new TextBox(AreaWidth - 0.2);
 
		// create display media object
		PdfDisplayMedia Omega = new PdfDisplayMedia(PdfEmbeddedFile.CreateEmbeddedFile(Document, "Omega.mp4"));
		Omega.DisplayControls(true);
		Omega.RepeatCount(0);
		Omega.SetMediaWindow(MediaWindow.Floating, 640, 360, WindowPosition.Center,
			WindowTitleBar.TitleBarWithCloseButton, WindowResize.KeepAspectRatio, "Floating Window Example");
		Box.AddText(ArialNormal, 11, "Floating video: ");
		Box.AddText(ArialNormal, 11, DrawStyle.Underline, Color.Blue, "Omega commercial\n", new AnnotDisplayMedia(Omega));

		// create embedded media file
		PdfDisplayMedia RingSound = new PdfDisplayMedia(PdfEmbeddedFile.CreateEmbeddedFile(Document, "Ring01.wav"));
		RingSound.SetMediaWindow(MediaWindow.Hidden);
		Box.AddText(ArialNormal, 11, "Sound file: ");
		Box.AddText(ArialNormal, 11, "Ring Tone: \n", new AnnotDisplayMedia(RingSound));

		Box.AddText(ArialNormal, 11, "Activate web link: ");
		Box.AddText(ArialNormal, 11, "Processing Standard Zip Files with C# compression/decompression classes\n",
			"http://www.codeproject.com/Articles/359758/Processing-Standard-Zip-Files-with-Csharp-compress");

		Box.AddText(ArialNormal, 11, "Link action: ");
		Box.AddText(ArialNormal, 11, "Page 2: \n", new AnnotLinkAction("Page2"));

		Box.AddText(ArialNormal, 11, "Link action: ");
		Box.AddText(ArialNormal, 11, "Page 8: \n", new AnnotLinkAction("Page8"));

		// create embedded media file
		PdfEmbeddedFile EmbeddedFile = PdfEmbeddedFile.CreateEmbeddedFile(Document, "BookList.txt");
		Box.AddText(ArialNormal, 11, "View attached file: ");
		Box.AddText(ArialNormal, 11, "Book List: \n", new AnnotFileAttachment(EmbeddedFile, FileAttachIcon.NoIcon));

		double PosYText = PosY + AreaHeight - 0.1;
		Contents.DrawText(PosX + 0.1, ref PosYText, PosY + 0.3, 0, 0.01, 0.10, TextBoxJustify.Left, Box, Page);

		Contents.DrawText(ArialNormal, NoteSize, PosX + NoteX, PosY + NoteY0, "Example 6e: TextBox annotation actions examples");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 1a Draw three circles fully opaque
	////////////////////////////////////////////////////////////////////

	public void Example8a
			(
			double PosX,
			double PosY
			)
		{
		// draw three solid lines
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SaveGraphicsState();

		double Radius = (AreaWidth - 1.0) / 3.0;
		Contents.DrawText(ArialBold, 36, 0.5 + 1.5 * Radius, 0.35 + Radius, TextJustify.Center, "Transparency");

		Contents.SetColorNonStroking(Color.Red);
		Contents.DrawOval(0.5, 0.35, 2 * Radius, 2 * Radius, PaintOp.Fill);
		Contents.SetColorNonStroking(Color.Blue);
		Contents.DrawOval(0.5 + Radius, 0.35, 2 * Radius, 2 * Radius, PaintOp.Fill);
		Contents.SetColorNonStroking(Color.Green);
		Contents.DrawOval(0.5 + 0.5 * Radius, 0.35 + Radius, 2 * Radius, 2 * Radius, PaintOp.Fill);

		Contents.RestoreGraphicsState();
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 8a: Draw Alpha 1.0 full opaque");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 1a Draw three circles partial opaque
	////////////////////////////////////////////////////////////////////

	public void Example8b
			(
			double PosX,
			double PosY
			)
		{
		// draw three solid lines
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SaveGraphicsState();

		double Radius = (AreaWidth - 1.0) / 3.0;
		Contents.DrawText(ArialBold, 36, 0.5 + 1.5 * Radius, 0.35 + Radius, TextJustify.Center, "Transparency");

		Contents.SetAlphaNonStroking(0.5);
		Contents.SetColorNonStroking(Color.Red);
		Contents.DrawOval(0.5, 0.35, 2 * Radius, 2 * Radius, PaintOp.Fill);
		Contents.SetColorNonStroking(Color.Blue);
		Contents.DrawOval(0.5 + Radius, 0.35, 2 * Radius, 2 * Radius, PaintOp.Fill);
		Contents.SetColorNonStroking(Color.Green);
		Contents.DrawOval(0.5 + 0.5 * Radius, 0.35 + Radius, 2 * Radius, 2 * Radius, PaintOp.Fill);

		Contents.RestoreGraphicsState();
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 8b: Draw Alpha 0.5 transparent");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 1a Draw three circles fully opaque
	////////////////////////////////////////////////////////////////////

	public void Example8c
			(
			double PosX,
			double PosY
			)
		{
		// draw three solid lines
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SaveGraphicsState();

		double Radius = (AreaWidth - 1.0) / 3.0;
		Contents.DrawText(ArialBold, 36, 0.5 + 1.5 * Radius, 0.35 + Radius, TextJustify.Center, "Transparency");

		Contents.SetAlphaNonStroking(0.8);
		Contents.SetBlendMode(BlendMode.Difference);
		Contents.SetColorNonStroking(Color.Red);
		Contents.DrawOval(0.5, 0.35, 2 * Radius, 2 * Radius, PaintOp.Fill);
		Contents.SetColorNonStroking(Color.Blue);
		Contents.DrawOval(0.5 + Radius, 0.35, 2 * Radius, 2 * Radius, PaintOp.Fill);
		Contents.SetColorNonStroking(Color.Green);
		Contents.DrawOval(0.5 + 0.5 * Radius, 0.35 + Radius, 2 * Radius, 2 * Radius, PaintOp.Fill);

		Contents.RestoreGraphicsState();
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 8c: Blend mode is Difference");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 1a Draw three circles partial opaque
	////////////////////////////////////////////////////////////////////

	public void Example8d
			(
			double PosX,
			double PosY
			)
		{
		// draw three solid lines
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SaveGraphicsState();

		double Radius = (AreaWidth - 1.0) / 3.0;
		Contents.DrawText(ArialBold, 36, 0.5 + 1.5 * Radius, 0.35 + Radius, TextJustify.Center, "Transparency");

		Contents.SetBlendMode(BlendMode.Screen);
		Contents.SetColorNonStroking(Color.Red);
		Contents.DrawOval(0.5, 0.35, 2 * Radius, 2 * Radius, PaintOp.Fill);
		Contents.SetColorNonStroking(Color.Blue);
		Contents.DrawOval(0.5 + Radius, 0.35, 2 * Radius, 2 * Radius, PaintOp.Fill);
		Contents.SetColorNonStroking(Color.Green);
		Contents.DrawOval(0.5 + 0.5 * Radius, 0.35 + Radius, 2 * Radius, 2 * Radius, PaintOp.Fill);

		Contents.RestoreGraphicsState();
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 8d: Blend mode is screen");
		Contents.RestoreGraphicsState();
		return;
		}

    ////////////////////////////////////////////////////////////////////
    // Example 8f
    ////////////////////////////////////////////////////////////////////

    public void Example8e
			(
			double PosX,
			double PosY
			)
		{
		// fish artwork from your favorite wpf or svg editing software (AI, Blend, Expression Design)
		// for hand writing minipath strings please see SVG or WPF reference on it (for example, https://developer.mozilla.org/en-US/docs/Web/SVG/Tutorial/Paths)
		string FishPathText = "M 73.302,96.9831C 88.1275,96.9831 100.146,109.002 100.146,123.827C 100.146,138.653 88.1275,150.671 73.302,150.671" +
			"C 58.4764,150.671 46.458,138.653 46.458,123.827C 46.458,109.002 58.4764,96.9831 73.302,96.9831 Z M 80.3771,118.625" +
			"C 87.8473,118.625 93.9031,124.681 93.9031,132.151C 93.9031,139.621 87.8473,145.677 80.3771,145.677C 72.9068,145.677 66.851,139.621 66.851,132.151" +
			"C 66.851,124.681 72.9069,118.625 80.3771,118.625 Z M 124.936,229.489L 124.936,230.05C 142.757,230.05 157.205,187.542 157.205,135.105" +
			"C 157.205,82.6682 142.757,40.1597 124.936,40.1597L 124.936,40.7208C 140.016,40.7208 152.241,82.9781 152.241,135.105" +
			"C 152.241,187.232 140.016,229.489 124.936,229.489 Z M 155.904,33.5723C 168.593,40.8964 181.282,48.2205 184.749,59.0803" +
			"C 188.216,69.9401 182.461,84.3356 176.705,98.7312C 187.217,82.7698 197.73,66.8085 194.263,55.9487C 190.796,45.0889 173.35,39.3306 155.904,33.5723 Z " +
			"M 221.06,47.217C 231.336,54.9565 241.612,62.6958 243.473,72.5309C 245.334,82.366 238.779,94.2968 232.224,106.228" +
			"C 243.092,93.4406 253.96,80.6536 252.099,70.8185C 250.238,60.9834 235.649,54.1002 221.06,47.217 Z M 190.088,103.489" +
			"C 200.631,113.663 211.175,123.836 211.914,135.212C 212.654,146.588 203.591,159.166 194.527,171.744C 208.585,158.796 222.643,145.848 221.903,134.472" +
			"C 221.163,123.096 205.625,113.293 190.088,103.489 Z M 227.222,175.988C 233.667,185.231 240.112,194.474 238.981,203.168" +
			"C 237.849,211.862 229.142,220.007 220.434,228.153C 232.965,220.47 245.497,212.787 246.628,204.093C 247.759,195.399 237.49,185.693 227.222,175.988 Z " +
			"M 176.183,170.829C 182.085,184.24 187.987,197.65 184.36,208.457C 180.734,219.265 167.58,227.47 154.426,235.675C 172.342,229.02 190.258,222.366 193.884,211.558" +
			"C 197.511,200.75 186.847,185.79 176.183,170.829 Z M 253.24,114.388C 261.541,123.744 269.842,133.1 269.72,142.831" +
			"C 269.598,152.561 261.052,162.667 252.506,172.773C 265.327,162.683 278.148,152.592 278.27,142.861C 278.392,133.13 265.816,123.759 253.24,114.388 Z " +
			"M 19.3722,114.348C 33.8527,95.7363 61.0659,59.7511 97.8151,40.6822C 117.532,30.4513 139.994,25.0899 164.816,24.6372" +
			"C 165.876,24.1644 167.083,23.6525 168.454,23.0983C 181.841,17.6879 210.836,8.25439 232.2,4.09256C 253.564,-0.0693054 267.298,1.04053 273.749,4.99429" +
			"C 280.2,8.94803 279.368,15.7458 278.743,24.4856C 278.119,33.2255 277.703,43.9076 276.94,49.1099C 276.927,49.2001 276.913,49.2887 276.9,49.3756" +
			"C 318.05,66.1908 360.168,89.8268 395.044,112.964C 408.876,122.14 421.569,131.238 433.26,140.058C 439.423,134.13 445.322,128.267 450.904,122.587" +
			"C 478.22,94.7909 497.963,71.3744 513.5,56.0696C 529.037,40.7648 540.368,33.5717 541.331,39.3597C 542.295,45.1478 532.891,63.9171 528.998,87.7075" +
			"C 525.105,111.498 526.722,140.309 533.661,167.068C 540.599,193.827 552.858,218.532 549.803,224.507C 546.748,230.482 528.378,217.727 502.239,196.166" +
			"C 483.768,180.932 461.418,161.301 433.26,140.058C 409.264,163.142 381.252,187.219 352.261,205.363C 315.824,228.167 277.841,241.6 230.108,245.486" +
			"C 182.376,249.372 124.895,243.713 84.9205,225.782C 44.946,207.851 22.4781,177.648 11.4752,160.545C 0.472214,143.443 0.934143,139.44 2.03903,136.819" +
			"C 3.14392,134.199 4.89172,132.96 19.3722,114.348 Z ";

        // water artwork 		
        string WavePathText = "M 0,724L 1106,724L 1106,617C 1025,656 942,687 846,680C 722,670 576,595 426,589C 287,583 143,636 0,694L 0,724 Z " +
			"M 423,26C 574,32 719,107 844,117C 940,125 1025,93 1106,53L 1106,0L 0,0L 0,130C 143,73 285,20 423,26 Z " +
			"M 0,546C 143,489 287,435 426,441C 576,447 722,522 846,532C 942,540 1025,509 1106,469L 1106,200C 1025,241 940,272 844,265" +
			"C 719,255 574,180 423,174C 285,168 143,220 0,277L 0,546 Z";

		// translate origin
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);

		// load fish path
		DrawWPFPath FishPath = new DrawWPFPath(FishPathText, YAxisDirection.Down);

		// set pen for both fish
		FishPath.SetPen(Color.FromArgb(0xff, 0xff, 0x50, 0));
		FishPath.SetPenWidth(0.01);

		// draw small fish
		FishPath.SetBrush(Color.FromArgb(255, 67, 211, 216));
		Contents.DrawWPFPath(FishPath, 1.5, 1.8, 1.65, 0.75, ContentAlignment.BottomLeft);

        // Radial gradient brush for the big fish
        SysMedia.RadialGradientBrush BigFishBrush = new SysMedia.RadialGradientBrush();

        // colors for gradient
        BigFishBrush.GradientStops.Add(new SysMedia.GradientStop(SysMedia.Color.FromRgb(0xff, 0x50, 0), 1.0));
        BigFishBrush.GradientStops.Add(new SysMedia.GradientStop(SysMedia.Color.FromRgb(0x27, 0xda, 0xff), 0.0));

        // origin and center relative to fish bounds
		BigFishBrush.MappingMode = SysMedia.BrushMappingMode.RelativeToBoundingBox;

        // outer center will be 1/4 of fish width and 1/2 of fish height
        BigFishBrush.Center = new SysWin.Point(.15, .5);

        // X radius half the fish width, Y radius equal to fish height
        BigFishBrush.GradientOrigin = new SysWin.Point(0.25, 0.5);
        BigFishBrush.RadiusX = 1.2;
        BigFishBrush.RadiusY = 1.2;
		FishPath.SetBrush(BigFishBrush);
		Contents.DrawWPFPath(FishPath, 0.3, 0.4, 2.74, 1.23, ContentAlignment.BottomLeft);

		// load wave
		DrawWPFPath WavePath = new DrawWPFPath(WavePathText, YAxisDirection.Up);

		// draw wave
		Color[] WaveBrushColor = new Color[] {Color.Cyan, Color.DarkBlue};
		PdfAxialShading AxialShading = new PdfAxialShading(Document, new PdfShadingFunction(Document, WaveBrushColor));
		AxialShading.SetAxisDirection(0.0, 1.0, 1.0, 0.0, MappingMode.Relative);
		WavePath.SetBrush(AxialShading, 0.55);
		Contents.DrawWPFPath(WavePath, 0.1, 0.25, AreaWidth - 0.2, AreaHeight - 0.35);

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 8e: Draw WPF grapics");

		// restore graphics state
		Contents.RestoreGraphicsState();
		return;
		}
	
   ////////////////////////////////////////////////////////////////////
    // Example 8f
    ////////////////////////////////////////////////////////////////////

    public void Example8f
			(
			double PosX,
			double PosY
			)
		{
		// draw three solid lines
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		string TestDraw1 = "M 0 200 L 200 0 A 200 200 0 1 1 0 200 Z";
		DrawWPFPath TestPath1 = new DrawWPFPath(TestDraw1, YAxisDirection.Up);
		TestPath1.SetPenWidth(0.05);
		TestPath1.SetPen(Color.DarkBlue);
		TestPath1.SetBrush(Color.LightSeaGreen);
		Contents.DrawWPFPath(TestPath1, 0.1, 0.25, AreaWidth - 0.2, AreaHeight - 0.35, ContentAlignment.MiddleCenter);

		string TestDraw2 = "M 0 200 L 200 0 A 300 200 0 1 1 0 200 Z";
		DrawWPFPath TestPath2 = new DrawWPFPath(TestDraw2, YAxisDirection.Up);
		TestPath2.SetPenWidth(0.02);
		TestPath2.SetPen(Color.Red);
		TestPath2.SetBrush(Color.FromArgb(140, Color.Pink));
		Contents.DrawWPFPath(TestPath2, 0.1, 0.25, AreaWidth - 0.2, AreaHeight - 0.35, ContentAlignment.MiddleCenter);

		string TestDraw3 = "M 0 200 L 200 0 A 300 200 0.52 1 1 0 200 Z";
		DrawWPFPath TestPath3 = new DrawWPFPath(TestDraw3, YAxisDirection.Up);
		TestPath3.SetPenWidth(0.02);
		TestPath3.SetPen(Color.Purple);
		TestPath3.SetBrush(Color.FromArgb(80, Color.MediumPurple));
		Contents.DrawWPFPath(TestPath3, 0.1, 0.25, AreaWidth - 0.2, AreaHeight - 0.35, ContentAlignment.MiddleCenter);

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 8f: Eliptical arc");
		Contents.RestoreGraphicsState();
		return;
		}
	}
}
