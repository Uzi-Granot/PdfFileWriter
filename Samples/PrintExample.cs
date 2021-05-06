/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	PrintExample
//	Produce PDF file when the Print Example is clicked.
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
using System.Drawing.Drawing2D;
using System.Drawing.Printing;

namespace TestPdfFileWriter
{
public class PrintExample
	{
	private	PdfDocument Document;
	private	Font DefaultFont;
	private	int PageNo;

	////////////////////////////////////////////////////////////////////
	// Create print example
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

		// Debug property
		// By default it is set to false. Use it for debugging only.
		// If this flag is set, PDF objects will not be compressed, font and images will be replaced
		// by text place holder. You can view the file with a text editor but you cannot open it with PDF reader.
		Document.Debug = Debug;

		// create default font for printing
		DefaultFont = new Font("Arial", 10.0f, FontStyle.Regular);

		// start page number
		PageNo = 1;

		// create PrintPdfDocument
		PdfPrintDocument Print = new PdfPrintDocument(Document);
		Print.Resolution = 300.0;
		Print.SaveAs = SaveImageAs.BWImage;

		// the method that will print one page at a time to PrintDocument
		Print.PrintPage += PrintPage;

		// set margins 
		Print.SetMargins(1.0, 1.0, 1.0, 1.0);

		// crop the page image result to reduce PDF file size
		Print.PageCropRect = new RectangleF(0.95f, 0.95f, 6.6f, 9.1f);

		// initiate the printing process (calling the PrintPage method)
		// after the document is printed, add each page an an image to PDF file.
		Print.AddPagesToPdfDocument();

		// dispose of the PrintDocument object
		Print.Dispose();

		// create the PDF file
		Document.CreateFile();

		// start default PDF reader and display the file
		Process Proc = new Process();
	    Proc.StartInfo = new ProcessStartInfo(FileName);
	    Proc.Start();

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Print each page of the document to PrintDocument class
	// You can use standard PrintDocument.PrintPage(...) method.
	// NOTE: The graphics origin is top left and Y axis is pointing down.
	// In other words this is not PdfContents printing.
	////////////////////////////////////////////////////////////////////

	public void PrintPage(object sender, PrintPageEventArgs e)
		{
		// graphics object short cut
		Graphics G = e.Graphics;

		// Set everything to high quality
		G.SmoothingMode = SmoothingMode.HighQuality;
		G.InterpolationMode = InterpolationMode.HighQualityBicubic;
		G.PixelOffsetMode = PixelOffsetMode.HighQuality;
		G.CompositingQuality = CompositingQuality.HighQuality;

		// print area within margins
		Rectangle PrintArea = e.MarginBounds;

		// draw rectangle around print area
		G.DrawRectangle(Pens.Black, PrintArea);

		// line height
		int LineHeight = DefaultFont.Height + 8;
		Rectangle TextRect = new Rectangle(PrintArea.X + 4, PrintArea.Y + 4, PrintArea.Width - 8, LineHeight);

		// display page bounds
		string text = string.Format("Page Bounds: Left {0}, Top {1}, Right {2}, Bottom {3}", e.PageBounds.Left, e.PageBounds.Top, e.PageBounds.Right, e.PageBounds.Bottom);
		G.DrawString(text, DefaultFont, Brushes.Black, TextRect);
		TextRect.Y += LineHeight;

		// display print area
		text = string.Format("Page Margins: Left {0}, Top {1}, Right {2}, Bottom {3}", PrintArea.Left, PrintArea.Top, PrintArea.Right, PrintArea.Bottom);
		G.DrawString(text, DefaultFont, Brushes.Black, TextRect);
		TextRect.Y += LineHeight;

		// print some lines
		for(int LineNo = 1; ; LineNo++)
			{
			text = string.Format("Page {0}, Line {1}", PageNo, LineNo);
			G.DrawString(text, DefaultFont, Brushes.Black, TextRect);
			TextRect.Y += LineHeight;
			if(TextRect.Bottom > PrintArea.Bottom) break;
			}
                
		// move on to next page
		PageNo++;
		e.HasMorePages = PageNo <= 5;
		return;
		}
	}
}
