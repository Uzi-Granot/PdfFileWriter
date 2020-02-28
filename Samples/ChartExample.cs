/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	ChartExample
//	Produce PDF file when the Chart Example is clicked.
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace TestPdfFileWriter
{
public class ChartExample
	{
	private PdfDocument		Document;
	private PdfPage			Page;
	private PdfContents		Contents;

	////////////////////////////////////////////////////////////////////
	// Create chart example
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

		// Step 3: Add new page
		Page = new PdfPage(Document);

		// Step 4: Add contents to page
		Contents = new PdfContents(Page);

		// Step 5: draw charts
		DrawPieChart();
		DrawColumnChart();

		// Step 3: Add new page
		Page = new PdfPage(Document);

		// Step 4: Add contents to page
		Contents = new PdfContents(Page);

		// Step 5: draw charts
		DrawStockChart();
		DrawPyramidChart();

		// Step 6: create pdf file
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
	// Draw Pie Chart
	////////////////////////////////////////////////////////////////////
	
   public void DrawPieChart()
		{
		// create Chart object
		Chart PieChart = PdfChart.CreateChart(Document, 6.5, 4.0, 300.0);

		// create PdfChart object
		PdfChart PiePdfChart = new PdfChart(Document, PieChart);
		PiePdfChart.SaveAs = SaveImageAs.IndexedImage;

		// make sure we have good quality image
		PieChart.AntiAliasing = AntiAliasingStyles.None; //.All;

		// set colors
		PieChart.BackColor = Color.FromArgb(220, 220, 255);
		PieChart.Palette = ChartColorPalette.BrightPastel;

		// title (font size is 0.25 inches)
		Font TitleFont1 = PiePdfChart.CreateFont("Verdana", FontStyle.Bold, 0.25, FontSizeUnit.UserUnit);
		Title Title1 = new Title("Pie Chart Example", Docking.Top, TitleFont1, Color.Purple);
		PieChart.Titles.Add(Title1);

		// title (font size is 0.25 inches)
		Font TitleFont2 = PiePdfChart.CreateFont("Verdana", FontStyle.Bold, 0.15, FontSizeUnit.UserUnit);
		Title Title2 = new Title("Percent Fruit Sales", Docking.Top, TitleFont2, Color.Purple);
		PieChart.Titles.Add(Title2);

		// default font
		Font DefaultFont = PiePdfChart.CreateFont("Verdana", FontStyle.Regular, 0.12, FontSizeUnit.UserUnit);

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
		Series1.Points.Add(20.0);
		Series1.Points[0].LegendText = "Apple";
		Series1.Points.Add(25.0);
		Series1.Points[1].LegendText = "Banana";
		Series1.Points.Add(10.0);
		Series1.Points[2].LegendText = "Pear";
		Series1.Points.Add(30.0);
		Series1.Points[3].LegendText = "Orange";
		Series1.Points.Add(15.0);
		Series1.Points[4].LegendText = "Grape";

		// draw chart into page contents
		Contents.DrawChart(PiePdfChart, 1.0, 6.0);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Column Chart
	////////////////////////////////////////////////////////////////////

	public void DrawColumnChart()
		{
		// create chart
		Chart ColChart = new Chart();

		// image resolution pixels per user units
		double ImageResolution = ColChart.RenderingDpiY * Document.ScaleFactor / 72.0;

		// image size in pixels
		// the equivalent of 6.5 by 4 inches in user units
		ColChart.Width = (int) (6.5 * ImageResolution + 0.5);
		ColChart.Height = (int) (4.0 * ImageResolution + 0.5);

		// make sure we have good quality image
		ColChart.AntiAliasing = AntiAliasingStyles.None; //.All;

		// set colors
		ColChart.BackColor = Color.FromArgb(220, 220, 255);
		ColChart.Palette = ChartColorPalette.Excel;

		// title (font size is 0.25 inches)
		Font TitleFont = new Font("Verdana", (float) (0.25 * ImageResolution), FontStyle.Bold, GraphicsUnit.Pixel);
		Title Title1 = new Title("Column Chart Example", Docking.Top, TitleFont, Color.Purple);
		ColChart.Titles.Add(Title1);

		// legend
		Font LegendFont = new Font("Times New Roman", (float) (0.125 * ImageResolution), FontStyle.Bold, GraphicsUnit.Pixel);
		Legend Legend1 = new Legend();
		ColChart.Legends.Add(Legend1);
		Legend1.BackColor = Color.FromArgb(230, 230, 255);
		Legend1.Docking = Docking.Bottom;
		Legend1.Font = LegendFont;

		// chart area
		ChartArea ChartArea1 = new ChartArea();
		ColChart.ChartAreas.Add(ChartArea1);

		// set font for axis
		Font TickMarkFont = new Font("Arial", (float) (0.1 * ImageResolution), FontStyle.Regular, GraphicsUnit.Pixel);
		ChartArea1.AxisX.LabelStyle.Font = TickMarkFont;
		ChartArea1.AxisY.LabelStyle.Font = TickMarkFont;

		// Y axis labels
		ChartArea1.AxisY.Title = "Y Axis Description";
		ChartArea1.AxisY.TitleFont = LegendFont;

		// chart area back color
		ChartArea1.BackColor = Color.FromArgb(255, 220, 255);

		// series 1
		Series Series1 = new Series();
		ColChart.Series.Add(Series1);
		Series1.ChartType = SeriesChartType.Column;
		Series1.LegendText = "Series One";
		Series1.IsValueShownAsLabel = true;
		Series1.Font = TickMarkFont;
		Series1.Points.AddXY("2010", 3.0);
		Series1.Points.AddXY("2011", 4.5);
		Series1.Points.AddXY("2012", 7.0);
		Series1.Points.AddXY("2013", 5.0);
		Series1.Points.AddXY("2014", 9.0);

		// series 2
		Series Series2 = new Series();
		ColChart.Series.Add(Series2);
		Series2.ChartType = SeriesChartType.Column;
		Series2.LegendText = "Series Two";
		Series2.IsValueShownAsLabel = true;
		Series2.Font = TickMarkFont;
		Series2.Points.AddXY("2010", 12.0);
		Series2.Points.AddXY("2011", 13.5);
		Series2.Points.AddXY("2012", 16.0);
		Series2.Points.AddXY("2013", 14.0);
		Series2.Points.AddXY("2014", 18.0);

		// draw chart into page contents
		PdfChart ColumnChart = new PdfChart(Document, ColChart);
		ColumnChart.SaveAs = SaveImageAs.IndexedImage;
		Contents.DrawChart(ColumnChart, 1.0, 1.0);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Candle Stick Chart
	////////////////////////////////////////////////////////////////////

	public void DrawStockChart()
		{
		// create Chart object
		Chart StockChart = PdfChart.CreateChart(Document, 6.5, 4.0, 300.0);

		// create PdfChart object
		PdfChart StockPdfChart = new PdfChart(Document, StockChart);
		StockPdfChart.SaveAs = SaveImageAs.IndexedImage;

		// make sure we have good quality image
		StockChart.AntiAliasing = AntiAliasingStyles.None; //.All;

		// set colors
		StockChart.BackColor = Color.FromArgb(220, 220, 255);
		StockChart.Palette = ChartColorPalette.BrightPastel;

		// title (font size is 0.25 inches)
		Font TitleFont1 = StockPdfChart.CreateFont("Verdana", FontStyle.Bold, 0.2, FontSizeUnit.UserUnit);
		Title Title1 = new Title("SP500 Daily Stock Price Chart Example", Docking.Top, TitleFont1, Color.Purple);
		StockChart.Titles.Add(Title1);

		// default font
		Font DefaultFont = StockPdfChart.CreateFont("Verdana", FontStyle.Regular, 0.12, FontSizeUnit.UserUnit);

		// chart area
		ChartArea ChartArea1 = new ChartArea();
		StockChart.ChartAreas.Add(ChartArea1);

		// chart area background color
		ChartArea1.BackColor = Color.FromArgb(255, 200, 255);

		// set font for axis
		Font TickMarkFont = StockPdfChart.CreateFont("Arial", FontStyle.Regular, 0.1, FontSizeUnit.Inch);
		ChartArea1.AxisX.LabelStyle.Font = TickMarkFont;
		ChartArea1.AxisY.LabelStyle.Font = TickMarkFont;
		ChartArea1.AxisY.LabelStyle.Format = "#,##0.00";

		// open stock daily price
		// takem from Yahoo Financial
		StreamReader Reader = new StreamReader("SP500.csv");

		// ignore header
		Reader.ReadLine();

		// data list
		List<DataPoint> DataArray = new List<DataPoint>();

		double Max = double.MinValue;
		double Min = double.MaxValue;

		// read all daily prices
		for(;;)
			{
			string TextLine = Reader.ReadLine();
			if(TextLine == null) break;

			string[] Fld = TextLine.Split(new char[] {','});

			string Date = Fld[0];
			double Open = double.Parse(Fld[1], NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, NFI.PeriodDecSep);
			double High = double.Parse(Fld[2], NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, NFI.PeriodDecSep);
			double Low = double.Parse(Fld[3], NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, NFI.PeriodDecSep);
			double Close = double.Parse(Fld[4], NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, NFI.PeriodDecSep);

			DataPoint Data = new DataPoint();
			Data.SetValueXY(Date, High, Low, Open, Close);
			DataArray.Add(Data);

			if(High > Max) Max = High;
			if(Low < Min) Min = Low;
			}

		// series 1
		Series Series1 = new Series();
		StockChart.Series.Add(Series1);
		Series1.ChartType = SeriesChartType.Candlestick;

		// setting bar colors
		Series1["PriceUpColor"] = "White"; 
		Series1["PriceDownColor"] = "Black";
		Series1.Color = Color.Black;

		// load data
		// note Yahoo provides in reversed order first line is most recent data
		DataArray.Reverse();
		foreach(DataPoint Data in DataArray) Series1.Points.Add(Data);

		// set min and max values for y axis
		ChartArea1.AxisY.Minimum = Math.Floor(Min);
		ChartArea1.AxisY.Maximum = Math.Ceiling(Max);

		// draw chart into page contents
		Contents.DrawChart(StockPdfChart, 1.0, 6.0);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw 3D Pyramin Chart
	////////////////////////////////////////////////////////////////////

	public void DrawPyramidChart()
		{
		// create Chart object
		Chart PyramidChart = PdfChart.CreateChart(Document, 6.5, 4.0, 300.0);

		// create PdfChart object
		PdfChart PyramidPdfChart = new PdfChart(Document, PyramidChart);
		PyramidPdfChart.SaveAs = SaveImageAs.IndexedImage;

		// make sure we have good quality image
		PyramidChart.AntiAliasing = AntiAliasingStyles.None; // .All;

		// set colors
		PyramidChart.BackColor = Color.FromArgb(220, 220, 255);
		PyramidChart.Palette = ChartColorPalette.BrightPastel;

		// title (font size is 0.25 inches)
		Font TitleFont1 = PyramidPdfChart.CreateFont("Verdana", FontStyle.Bold, 0.25, FontSizeUnit.UserUnit);
		Title Title1 = new Title("Pyramid 3D Chart Example", Docking.Top, TitleFont1, Color.Purple);
		PyramidChart.Titles.Add(Title1);

		// title (font size is 0.25 inches)
		Font TitleFont2 = PyramidPdfChart.CreateFont("Verdana", FontStyle.Bold, 0.15, FontSizeUnit.UserUnit);
		Title Title2 = new Title("Percent Fruit Sales", Docking.Top, TitleFont2, Color.Purple);
		PyramidChart.Titles.Add(Title2);

		// default font
		Font DefaultFont = PyramidPdfChart.CreateFont("Verdana", FontStyle.Regular, 0.12, FontSizeUnit.UserUnit);

		// legend
		Legend Legend1 = new Legend();
		PyramidChart.Legends.Add(Legend1);
		Legend1.BackColor = Color.FromArgb(230, 230, 255);
		Legend1.Docking = Docking.Bottom;
		Legend1.Font = DefaultFont;

		// chart area
		ChartArea ChartArea1 = new ChartArea();
		PyramidChart.ChartAreas.Add(ChartArea1);

		// 3d style
		ChartArea1.Area3DStyle.Enable3D = true;

		// chart area background color
		ChartArea1.BackColor = Color.FromArgb(255, 200, 255);

		// series 1
		Series Series1 = new Series();
		PyramidChart.Series.Add(Series1);
		Series1.ChartType = SeriesChartType.Pyramid;
		Series1.Font = DefaultFont;
		Series1.IsValueShownAsLabel = true;
		Series1.LabelFormat = "{0} %";
		Series1["Pyramid3DDrawingStyle"] = "SquareBase";
		Series1["Pyramid3DRotationAngle"] = "8";
		Series1["PyramidPointGap"] = "1";

		// series values
		Series1.Points.Add(20.0);
		Series1.Points[0].LegendText = "Apple";
		Series1.Points.Add(25.0);
		Series1.Points[1].LegendText = "Banana";
		Series1.Points.Add(10.0);
		Series1.Points[2].LegendText = "Pear";
		Series1.Points.Add(30.0);
		Series1.Points[3].LegendText = "Orange";
		Series1.Points.Add(15.0);
		Series1.Points[4].LegendText = "Grape";

		// draw chart into page contents
		Contents.DrawChart(PyramidPdfChart, 1.0, 1.0);
		return;
		}
	}
}
