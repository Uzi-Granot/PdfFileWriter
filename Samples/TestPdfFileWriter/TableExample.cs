/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter II
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	TableExample
//	Produce PDF file when the Table Example is clicked.
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
using System.Text;

namespace TestPdfFileWriter
	{
	/// <summary>
	/// Table example
	/// </summary>
	public class TableExample
		{
		private PdfDocument Document;
		private PdfPage Page;
		private PdfContents Contents;
		private PdfFont NormalFont;
		private PdfDrawTextCtrl NormalTextCtrl8;
		private PdfDrawTextCtrl NormalTextCtrl9;
		private PdfFont TableTitleFont;

		////////////////////////////////////////////////////////////////////
		// Create data table examples PDF document
		////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Create table example PDF document
		/// </summary>
		/// <param name="FileName">PDF file name</param>
		/// <param name="Debug">Debug flag</param>
		public void Test
				(
				string FileName,
				bool Debug
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
			NormalTextCtrl8 = new PdfDrawTextCtrl(NormalFont, 8);
			NormalTextCtrl9 = new PdfDrawTextCtrl(NormalFont, 9);
			TableTitleFont = PdfFont.CreatePdfFont(Document, "Times New Roman", FontStyle.Bold, true);

			// book list table
			CreateBookList();

			// stock price table
			CreateStockTable();

			// display barcode within table
			TestBarcode();

			// textbox overflow example
			TestOverflow();

			// argument: PDF file name
			Document.CreateFile();

			// start default PDF reader and display the file
			Process Proc = new Process();
			Proc.StartInfo = new ProcessStartInfo(FileName) {UseShellExecute = true};
			Proc.Start();

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Create table example
		////////////////////////////////////////////////////////////////////

		private void CreateBookList()
			{
			// Add new page
			Page = new PdfPage(Document);

			// Add contents to page
			Contents = new PdfContents(Page);

			// font resources
			PdfDrawTextCtrl TitleTextCtrl = new PdfDrawTextCtrl(Document, "Verdana", FontStyle.Bold, 10);
			TitleTextCtrl.TextColor = Color.DarkBlue;
			PdfDrawTextCtrl AuthorTextCtrl = new PdfDrawTextCtrl(Document, "Verdana", FontStyle.Italic, 9);
			AuthorTextCtrl.TextColor = Color.DarkRed;

			// create stock table
			PdfTable BookList = new PdfTable(Page, Contents, NormalTextCtrl9);

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
			BookList.DefaultHeaderStyle.TextColor = Color.LightCyan;
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
			BookList.Cell[3].Style = BookList.CellStyle;
			BookList.Cell[3].Style.RaiseCustomDrawCellEvent = true;

			// create private style for in stock column
			BookList.Cell[4].Style = BookList.CellStyle;
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
				if(TextLine == null)
					break;

				// split to fields (must be 8 fields)
				string[] Fld = TextLine.Split(new char[] { '\t' });
				if(Fld.Length != 8)
					continue;

				// book cover
				PdfImage Cell0Image = new PdfImage(Document);
				Cell0Image.LoadImage(Fld[6]);
				BookList.Cell[0].Value = Cell0Image;

				// note create text box set Value field
				PdfTextBox Box = BookList.Cell[1].CreateTextBox();
				Box.AddText(TitleTextCtrl, Fld[0]);
				Box.AddText(NormalTextCtrl8, ", Author(s): ");
				Box.AddText(AuthorTextCtrl, Fld[2]);

				// date, type in-stock and price
				BookList.Cell[2].Value = Fld[1];
				BookList.Cell[3].Value = Fld[3];
				BookList.Cell[4].Value = int.Parse(Fld[5]);
				BookList.Cell[5].Value = double.Parse(Fld[4], NFI.PeriodDecSep);

				// QRCode and web link
				PdfQREncoder Encoder = new PdfQREncoder();
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
			// rounded rectangle filled and no border
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Shape = DrawShape.RoundedRect;
			DrawCtrl.Paint = DrawPaint.Fill;
			DrawCtrl.BackgroundTexture = (string) Cell.Value == "Paperback" ?
				Color.LightCyan : Color.LightPink;
			
			// rectangle 
			double Left = Cell.ClientLeft;
			double Bottom = 0.5 * (Cell.ClientBottom + Cell.ClientTop) - Cell.Style.LineSpacing;
			double Right = Cell.ClientRight;
			double Top = Bottom + 2.0 * Cell.Style.LineSpacing;
			PdfRectangle Rect = new PdfRectangle(Left, Bottom, Right, Top);

			// draw rectangle
			Table.Contents.DrawGraphics(DrawCtrl, Rect);
			return false;
			}

		private void BookListTableStart
				(
				PdfTable BookList,
				double TableStartPos
				)
			{
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(TableTitleFont, 16.0);
			TextCtrl.Justify = TextJustify.Center;
			TextCtrl.TextColor = Color.Chocolate;
			double PosX = 0.5 * (BookList.TableArea.Left + BookList.TableArea.Right);
			double PosY = TableStartPos + TextCtrl.LineSpacing - TextCtrl.TextAscent;
			BookList.Contents.DrawText(TextCtrl, PosX, PosY, "Book List PdfTable Example");
			return;
			}

		private void BookListTableEnd
				(
				PdfTable BookList,
				double TableEndPos
				)
			{
			double PosX = BookList.TableArea.Left;
			double PosY = TableEndPos - TableTitleFont.Ascent(12.0) - 0.05;
			PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(TableTitleFont, 12.0);
			TextCtrl.TextColor = Color.Chocolate;
			BookList.Contents.DrawText(TextCtrl, PosX, PosY, "Either scan the Web link or click the area for more info.");
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Create table example
		////////////////////////////////////////////////////////////////////

		private void CreateStockTable()
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
			PdfTable StockTable = new PdfTable(Page, Contents, NormalTextCtrl9);

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
			for(; ; )
				{
				string TextLine = Reader.ReadLine();
				if(TextLine == null)
					break;

				string[] Fld = TextLine.Split(new char[] { ',' });

				StockTable.Cell[ColDate].Value = Fld[ColDate];
				StockTable.Cell[ColOpen].Value = double.Parse(Fld[ColOpen], NFI.PeriodDecSep);
				StockTable.Cell[ColHigh].Value = double.Parse(Fld[ColHigh], NFI.PeriodDecSep);
				StockTable.Cell[ColLow].Value = double.Parse(Fld[ColLow], NFI.PeriodDecSep);
				StockTable.Cell[ColClose].Value = double.Parse(Fld[ColClose], NFI.PeriodDecSep);
				StockTable.Cell[ColVolume].Value = int.Parse(Fld[ColVolume]);
				StockTable.Cell[ColVolume].Style = (double) StockTable.Cell[ColClose].Value >= (double) StockTable.Cell[ColOpen].Value ? GoingUpStyle : GoingDownStyle;
				StockTable.DrawRow();
				}

			// close table object
			StockTable.Close();

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Create table with barcode example
		////////////////////////////////////////////////////////////////////
		private void TestBarcode()
			{
			// Add new page
			Page = new PdfPage(Document);

			// Add contents to page
			Contents = new PdfContents(Page);

			// create stock table
			PdfTable BarcodeTable = new PdfTable(Page, Contents, NormalTextCtrl9);

			// divide columns width in proportion to following values
			BarcodeTable.SetColumnWidth(1.0, 4.0);

			// set all borders
			BarcodeTable.Borders.SetAllBorders(0.012, Color.DarkGray, 0.0025, Color.DarkGray);

			BarcodeTable.Cell[0].Value = "Barcode128";
			BarcodeTable.Cell[0].Style = BarcodeTable.CellStyle;
			BarcodeTable.Cell[0].Style.Alignment = ContentAlignment.MiddleLeft;
			PdfBarcode128 Barcode128 = new PdfBarcode128("PDF File Writer");
			PdfDrawBarcodeCtrl BarcodeCtrl = new PdfDrawBarcodeCtrl();
			BarcodeCtrl.NarrowBarWidth = 0.012;
			BarcodeCtrl.Height = 0.5;
			BarcodeCtrl.TextCtrl = NormalTextCtrl9;
			BarcodeTable.Cell[1].Style = BarcodeTable.CellStyle;
			BarcodeTable.Cell[1].Style.Alignment = ContentAlignment.TopCenter;
			BarcodeTable.Cell[1].Style.BarcodeCtrl = BarcodeCtrl;
			BarcodeTable.Cell[1].Value = Barcode128;
			BarcodeTable.DrawRow();

			BarcodeTable.Cell[0].Value = "Barcode39";
			PdfBarcode128 Barcode39 = new PdfBarcode128("123456789012");
			BarcodeTable.Cell[1].Value = Barcode39;
			BarcodeTable.DrawRow();

			BarcodeTable.Cell[0].Value = "EAN-13";
			PdfBarcodeEAN13 BarcodeEAN13 = new PdfBarcodeEAN13("9876543210980");
			BarcodeCtrl = new PdfDrawBarcodeCtrl();
			BarcodeCtrl.NarrowBarWidth = 0.014;
			BarcodeCtrl.Height = 0.75;
			BarcodeCtrl.TextCtrl = NormalTextCtrl8;
			BarcodeTable.Cell[1].Style.BarcodeCtrl = BarcodeCtrl;
			BarcodeTable.Cell[1].Value = BarcodeEAN13;
			BarcodeTable.DrawRow();

			BarcodeTable.Cell[0].Value = "UPC-A";
			PdfBarcodeEAN13 BarcodeUPCA = new PdfBarcodeEAN13("123456789010");
			BarcodeTable.Cell[1].Value = BarcodeUPCA;
			BarcodeTable.DrawRow();

			// close table object
			BarcodeTable.Close();

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Create textbox overflow example
		////////////////////////////////////////////////////////////////////
		private void TestOverflow()
			{
			// Add new page
			Page = new PdfPage(Document);

			// Add contents to page
			Contents = new PdfContents(Page);

			// create table
			PdfTable Table = new PdfTable(Page, Contents, NormalTextCtrl9);

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

			int[] Lines1 = { 40, 90, 20 };
			int[] Lines2 = { 20, 50, 70 };

			for(int Row = 0; Row < 3; Row++)
				{
				Table.Cell[0].Value = string.Format("Row {0}", Row + 1);

				StringBuilder Text1 = new StringBuilder();
				for(int Line = 0; Line < Lines1[Row % 3]; Line++)
					Text1.AppendFormat("Line {0}\r\n", Line + 1);
				Table.Cell[1].Value = Text1.ToString();

				StringBuilder Text2 = new StringBuilder();
				for(int Line = 0; Line < Lines2[Row % 3]; Line++)
					Text2.AppendFormat("Line {0}\r\n", Line + 1);
				Table.Cell[2].Value = Text2.ToString();

				Table.DrawRow();
				}

			// close table object
			Table.Close();

			// exit
			return;
			}
		}
	}
