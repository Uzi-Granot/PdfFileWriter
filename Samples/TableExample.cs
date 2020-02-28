/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	TableExample
//	Produce PDF file when the Table Example is clicked.
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
using System.IO;
using System.Text;

namespace TestPdfFileWriter
{
public class TableExample
	{
	private PdfDocument		Document;
	private PdfPage			Page;
	private PdfContents		Contents;
	private PdfFont			NormalFont;
	private PdfFont			TableTitleFont;

	////////////////////////////////////////////////////////////////////
	// Create data table examples PDF document
	////////////////////////////////////////////////////////////////////
	
	public void Test
			(
			bool Debug,
			string	FileName
			)
		{
		// Create empty document
		// Arguments: page width: 8.5”, page height: 11”, Unit of measure: inches
		// Return value: PdfDocument main class
		Document = new PdfDocument(PaperType.Letter, false, UnitOfMeasure.Inch, FileName);

		// Debug property
		// By default it is set to false. Use it for debugging only.
		// If this flag is set, PDF objects will not be compressed, font and images will be replaced
		// by text place holder. You can view the file with a text editor but you cannot open it with PDF reader.
		Document.Debug = Debug;

		// define font resource
		NormalFont = PdfFont.CreatePdfFont(Document, "Arial", FontStyle.Regular, true);
		TableTitleFont = PdfFont.CreatePdfFont(Document, "Times New Roman", FontStyle.Bold, true);

		// book list table
		CreateBookList();

		// stock price table
		CreateStockTable();

		// textbox overflow example
		TestOverflow();

		// argument: PDF file name
		Document.CreateFile();

		// start default PDF reader and display the file
		Process Proc = new Process();
	    Proc.StartInfo = new ProcessStartInfo(FileName);
	    Proc.Start();

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Create table example
	////////////////////////////////////////////////////////////////////
	
	public void CreateBookList()
		{
		// Add new page
		Page = new PdfPage(Document);

		// Add contents to page
		Contents = new PdfContents(Page);

		PdfFont TitleFont = PdfFont.CreatePdfFont(Document, "Verdana", FontStyle.Bold);
		PdfFont AuthorFont = PdfFont.CreatePdfFont(Document, "Verdana", FontStyle.Italic);

		// create stock table
		PdfTable BookList = new PdfTable(Page, Contents, NormalFont, 9.0);

		// divide columns width in proportion to following values
		BookList.SetColumnWidth(1.0, 2.5, 1.2, 1.0, 0.5, 0.6, 1.2);

		// event handlers
		BookList.TableStartEvent += BookListTableStart;
		BookList.TableEndEvent += BookListTableEnd;
		BookList.CustomDrawCellEvent += BookListDrawCellEvent;

		// set display header at the top of each additional page
		BookList.HeaderOnEachPage = true;

		// make some changes to default header style
		BookList.DefaultHeaderStyle.Alignment = ContentAlignment.MiddleCenter;
		BookList.DefaultHeaderStyle.FontSize = 9.0;
		BookList.DefaultHeaderStyle.MultiLineText = true;
		BookList.DefaultHeaderStyle.TextBoxTextJustify = TextBoxJustify.Center;
		BookList.DefaultHeaderStyle.BackgroundColor = Color.Blue;
		BookList.DefaultHeaderStyle.ForegroundColor = Color.LightCyan;
		BookList.DefaultHeaderStyle.TextBoxLineBreakFactor = 0.2;

		// headers
		BookList.Header[0].Value = "Book Cover";
		BookList.Header[1].Value = "Book Title and Authors";
		BookList.Header[2].Value = "Date\nPublished";
		BookList.Header[3].Value = "Type";
		BookList.Header[4].Value = "In\nStock";
		BookList.Header[5].Value = "Price";
		BookList.Header[6].Value = "Weblink";

		// default cell style
		BookList.DefaultCellStyle.Alignment = ContentAlignment.MiddleCenter;

		// create private style for type column
		BookList.Cell[3].Style= BookList.CellStyle;
		BookList.Cell[3].Style.RaiseCustomDrawCellEvent = true;
	
		// create private style for in stock column
		BookList.Cell[4].Style= BookList.CellStyle;
		BookList.Cell[4].Style.Format = "#,##0";
		BookList.Cell[4].Style.Alignment = ContentAlignment.MiddleRight;
	
		// create private style for price column
		BookList.Cell[5].Style = BookList.CellStyle;
		BookList.Cell[5].Style.Format = "#,##0.00";
		BookList.Cell[5].Style.Alignment = ContentAlignment.MiddleRight;

		// book list text file
		StreamReader Reader = new StreamReader("BookList.txt");

		// loop for records
		for(;;)
			{
			// read one line
			string TextLine = Reader.ReadLine();
			if(TextLine == null) break;

			// split to fields (must be 8 fields)
			string[] Fld = TextLine.Split(new char[] {'\t'});
			if(Fld.Length != 8) continue;

			// book cover
			PdfImage Cell0Image = new PdfImage(Document);
			Cell0Image.LoadImage(Fld[6]);
			BookList.Cell[0].Value = Cell0Image;

			// note create text box set Value field
			TextBox Box = BookList.Cell[1].CreateTextBox();
			Box.AddText(TitleFont, 10.0, Color.DarkBlue, Fld[0]);
			Box.AddText(NormalFont, 8.0, Color.Black, ", Author(s): ");
			Box.AddText(AuthorFont, 9.0, Color.DarkRed, Fld[2]);

			// date, type in-stock and price
			BookList.Cell[2].Value = Fld[1];
			BookList.Cell[3].Value = Fld[3];
			BookList.Cell[4].Value = int.Parse(Fld[5]);
			BookList.Cell[5].Value = double.Parse(Fld[4], NFI.PeriodDecSep);

			// QRCode and web link
			QREncoder Encoder = new QREncoder();
			Encoder.ErrorCorrection = ErrorCorrection.M;
			Encoder.Encode(Fld[7]);
			PdfImage QRImage = new PdfImage(Document);
			QRImage.LoadImage(Encoder);

			BookList.Cell[6].Value = QRImage;
			BookList.Cell[6].WebLink = Fld[7];

			// draw it
			BookList.DrawRow();
			}

		// close book list
		BookList.Close();

		// exit
		return;
		}

	// draw cell event handler
	private bool BookListDrawCellEvent(PdfTable Table, PdfTableCell Cell)
		{
		Table.Contents.SaveGraphicsState();
		if((string) Cell.Value == "Paperback")
			Table.Contents.SetColorNonStroking(Color.LightCyan);
		else
			Table.Contents.SetColorNonStroking(Color.LightPink);
		double PosX = Cell.ClientLeft;
		double PosY = 0.5 * (Cell.ClientBottom + Cell.ClientTop) - Cell.Style.FontLineSpacing;
		double Width = Cell.ClientRight - Cell.ClientLeft;
		double Height = 2.0 * Cell.Style.FontLineSpacing;
		Table.Contents.DrawRoundedRectangle(PosX, PosY, Width, Height, 0.25, PaintOp.Fill);
		Table.Contents.RestoreGraphicsState();;
		return false;
		}

	private void BookListTableStart
			(
			PdfTable	BookList,
			double		TableStartPos
			)
		{
		double PosX = 0.5 * (BookList.TableArea.Left + BookList.TableArea.Right);
		double PosY = TableStartPos + TableTitleFont.Descent(16.0) + 0.05;
		BookList.Contents.DrawText(TableTitleFont, 16.0, PosX, PosY, TextJustify.Center, DrawStyle.Normal, Color.Chocolate, "Book List PdfTable Example"); 
		return;
		}

	private void BookListTableEnd
			(
			PdfTable	BookList,
			double		TableEndPos
			)
		{
		double PosX = BookList.TableArea.Left;
		double PosY = TableEndPos - TableTitleFont.Ascent(12.0) - 0.05;
		BookList.Contents.DrawText(TableTitleFont, 12.0, PosX, PosY, TextJustify.Left, DrawStyle.Normal, Color.Chocolate, "Either scan the Web link or click the area for more info."); 
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Create table example
	////////////////////////////////////////////////////////////////////
	
	public void CreateStockTable()
		{
		const int ColDate = 0;
		const int ColOpen = 1;
		const int ColHigh = 2;
		const int ColLow = 3;
		const int ColClose = 4;
		const int ColVolume = 5;

		// Add new page
		Page = new PdfPage(Document);

		// Add contents to page
		Contents = new PdfContents(Page);

		// create stock table
		PdfTable StockTable = new PdfTable(Page, Contents, NormalFont, 9.0);

		// divide columns width in proportion to following values
		StockTable.SetColumnWidth(1.2, 1.0, 1.0, 1.0, 1.0, 1.2);

		// set all borders
		StockTable.Borders.SetAllBorders(0.012, Color.DarkGray, 0.0025, Color.DarkGray);

		// make some changes to default header style
		StockTable.DefaultHeaderStyle.Alignment = ContentAlignment.BottomRight;

		// create private style for header first column
		StockTable.Header[ColDate].Style = StockTable.HeaderStyle;
		StockTable.Header[ColDate].Style.Alignment = ContentAlignment.MiddleLeft;

		StockTable.Header[ColDate].Value = "Date";
		StockTable.Header[ColOpen].Value = "Open";
		StockTable.Header[ColHigh].Value = "High";
		StockTable.Header[ColLow].Value = "Low";
		StockTable.Header[ColClose].Value = "Close";
		StockTable.Header[ColVolume].Value = "Volume";

		// make some changes to default cell style
		StockTable.DefaultCellStyle.Alignment = ContentAlignment.MiddleRight;
		StockTable.DefaultCellStyle.Format = "#,##0.00";

		// create private style for date column
		StockTable.Cell[ColDate].Style = StockTable.CellStyle;
		StockTable.Cell[ColDate].Style.Alignment = ContentAlignment.MiddleLeft;
		StockTable.Cell[ColDate].Style.Format = null;

		// create private styles for volume column
		PdfTableStyle GoingUpStyle = StockTable.CellStyle;
		GoingUpStyle.BackgroundColor = Color.LightGreen;
		GoingUpStyle.Format = "#,##0";
		PdfTableStyle GoingDownStyle = StockTable.CellStyle;
		GoingDownStyle.BackgroundColor = Color.LightPink;
		GoingDownStyle.Format = "#,##0";

		// open stock daily price
		// takem from Yahoo Financial
		StreamReader Reader = new StreamReader("SP500.csv");

		// ignore header
		Reader.ReadLine();

		// read all daily prices
		for(;;)
			{
			string TextLine = Reader.ReadLine();
			if(TextLine == null) break;

			string[] Fld = TextLine.Split(new char[] {','});

			StockTable.Cell[ColDate].Value = Fld[ColDate];
			StockTable.Cell[ColOpen].Value = double.Parse(Fld[ColOpen], NFI.PeriodDecSep);
			StockTable.Cell[ColHigh].Value = double.Parse(Fld[ColHigh], NFI.PeriodDecSep);
			StockTable.Cell[ColLow].Value = double.Parse(Fld[ColLow], NFI.PeriodDecSep);
			StockTable.Cell[ColClose].Value = double.Parse(Fld[ColClose], NFI.PeriodDecSep);
			StockTable.Cell[ColVolume].Value = int.Parse(Fld[ColVolume]);
			StockTable.Cell[ColVolume].Style = (double) StockTable.Cell[ColClose].Value >= (double) StockTable.Cell[ColOpen].Value ? GoingUpStyle : GoingDownStyle;
			StockTable.DrawRow();
			}

		StockTable.Close();

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Create textbox overflow example
	////////////////////////////////////////////////////////////////////

	public void TestOverflow()
		{
		// Add new page
		Page = new PdfPage(Document);

		// Add contents to page
		Contents = new PdfContents(Page);

		// create table
		PdfTable Table = new PdfTable(Page, Contents, NormalFont, 9.0);

		// Commit
		Table.CommitToPdfFile = true;
		Table.CommitGCCollectFreq = 1;

		// divide columns width in proportion to following values
		Table.SetColumnWidth(1.0, 2.75, 2.75);

		Table.Header[0].Value = "Column 1";
		Table.Header[1].Value = "Column 2";
		Table.Header[2].Value = "Column 3";

		Table.Cell[1].Style = Table.CellStyle;
		Table.Cell[1].Style.MultiLineText = true;
		Table.Cell[1].Style.TextBoxPageBreakLines = 4;

		Table.Cell[2].Style = Table.CellStyle;
		Table.Cell[2].Style.MultiLineText = true;
		Table.Cell[2].Style.TextBoxPageBreakLines = 8;

		int[] Lines1 = {40, 90, 20};
		int[] Lines2 = {20, 50, 70};

		for(int Row = 0; Row < 3; Row++)
			{
			Table.Cell[0].Value = string.Format("Row {0}", Row + 1);

			StringBuilder Text1 = new StringBuilder();
			for(int Line = 0; Line < Lines1[Row % 3]; Line++) Text1.AppendFormat("Line {0}\r\n", Line + 1);
			Table.Cell[1].Value = Text1.ToString();

			StringBuilder Text2 = new StringBuilder();
			for(int Line = 0; Line < Lines2[Row % 3]; Line++) Text2.AppendFormat("Line {0}\r\n", Line + 1);
			Table.Cell[2].Value = Text2.ToString();

			Table.DrawRow();

			// DEBUG
			//Trace.Write(string.Format("Total Memory: {0}", GC.GetTotalMemory(false)));
			}

		Table.Close();

		// exit
		return;
		}
	}
}
