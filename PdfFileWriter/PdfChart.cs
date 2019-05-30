/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfChart
//	Display charts in the PDF document.
//  Charts are displayed as images.
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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Text;

namespace PdfFileWriter
{
/// <summary>
/// Font size units for PdfChart.CreateFont method enumeration
/// </summary>
public enum FontSizeUnit
	{
	/// <summary>
	/// Pixel
	/// </summary>
	Pixel,

	/// <summary>
	/// Point
	/// </summary>
	Point,

	/// <summary>
	/// PDF document user unit
	/// </summary>
	UserUnit,

	/// <summary>
	/// Inch
	/// </summary>
	Inch,

	/// <summary>
	/// CM
	/// </summary>
	cm,

	/// <summary>
	/// MM
	/// </summary>
	mm
	}

/// <summary>
/// PDF chart resource class
/// </summary>
/// <remarks>
/// <para>
/// For more information go to <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#ChartingSupport">2.10 Charting Support</a>
/// </para>
/// <para>
/// <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#DrawChart">For example of drawing image see 3.11. Draw Pie Chart</a>
/// </para>
/// </remarks>
public class PdfChart : PdfImage
	{
	/// <summary>
	/// Chart object (.NET).
	/// </summary>
	public Chart Chart {get; private set;}			// chart object

	/// <summary>
	/// Chart width in user units.
	/// </summary>
	public double Width {get; private set;}			// width in user units

	/// <summary>
	/// Chart height in user units.
	/// </summary>
	public double Height {get; private set;}		// height in user units

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// PDF chart constructor
	/// </summary>
	/// <param name="Document">Document object parent of this chart.</param>
	/// <param name="Chart">.NET Chart object.</param>
	/// <param name="ImageControl">Chart display image control</param>
	/// <remarks>
	/// It is the responsibility of the calling program to release
	/// the resources of the input chart object. After PdfChart
	/// is commited to the PDF file
	/// </remarks>
	/// <example> 
    /// <code>
    ///	// create chart
	///	Chart MyChart = new Chart();
	///	// build chart
	///	// ...
	///	// ...
	/// PdfImageControl ImageControl = new PdfImageControl();
	///	ImageControl.SaveAs = SaveImageAs.IndexedImage;
	///	PdfChart MyPdfChart = new PdfChart(Document, MyChart, ImageControl);
	///	MyPdfChart.CommitToPdfFile();
	///	MyChart.Dispose();
    /// </code>
    /// </example>
	////////////////////////////////////////////////////////////////////
	public PdfChart
			(
			PdfDocument		Document,
			Chart			Chart,
			PdfImageControl	ImageControl = null
			) : base(Document)
		{
		// image control
		if(ImageControl == null) ImageControl = new PdfImageControl();
		this.ImageControl = ImageControl;

		// save chart
		this.Chart = Chart;
		this.WidthPix = Chart.Width;
		this.HeightPix = Chart.Height;

		// save resolution
		if(ImageControl.Resolution != 0)
			{
			// chart resolution in pixels per inch
			this.Chart.RenderingDpiY = ImageControl.Resolution;
			}
		else
			{
			ImageControl.Resolution = this.Chart.RenderingDpiY;
			}

		// calculate chart size in user coordinates
		Width = (double) WidthPix * 72.0 / (ImageControl.Resolution * Document.ScaleFactor);
		Height = (double) HeightPix * 72.0 / (ImageControl.Resolution * Document.ScaleFactor);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Static method to create .NET Chart object.
	/// </summary>
	/// <param name="Document">Current document object.</param>
	/// <param name="Width">Chart width in user units.</param>
	/// <param name="Height">Chart height in user units.</param>
	/// <param name="Resolution">Resolution in pixels per inch (optional argument).</param>
	/// <returns>.NET Chart object</returns>
	/// <remarks>
	/// The returned chart has the correct width and height in pixels.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public static Chart CreateChart
			(
			PdfDocument		Document,
			double			Width,
			double			Height,
			double			Resolution = 0.0
			)
		{
		// create chart
		Chart Chart = new Chart();

		// save resolution
		if(Resolution != 0) Chart.RenderingDpiY = Resolution;

		// image size in pixels
		Chart.Width = (int) (Chart.RenderingDpiY * Width * Document.ScaleFactor / 72.0 + 0.5);
		Chart.Height = (int) (Chart.RenderingDpiY * Height * Document.ScaleFactor / 72.0 + 0.5);

		// return chart
		return(Chart);
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Helper method to create a font for chart drawing.
	/// </summary>
	/// <param name="FontFamilyName">Font family name.</param>
	/// <param name="FontStyle">Font style.</param>
	/// <param name="FontSize">Font size per unit argument.</param>
	/// <param name="Unit">Font size unit.</param>
	/// <returns>.NET font</returns>
	////////////////////////////////////////////////////////////////////
	public Font CreateFont
			(
			string		FontFamilyName,	// font family name
			FontStyle	FontStyle,		// font style
			double		FontSize,		// as per units below
			FontSizeUnit Unit			// unit of measure
			)
		{
		// calculate size
		int SizeInPixels = 0;
		switch(Unit)
			{
			case FontSizeUnit.Pixel:
				SizeInPixels = (int) (FontSize + 0.5);
				break;

			case FontSizeUnit.Point:
				SizeInPixels = (int) (FontSize * ImageControl.Resolution / 72.0 + 0.5);
				break;

			case FontSizeUnit.UserUnit:
				SizeInPixels = (int) (FontSize * ImageControl.Resolution * Document.ScaleFactor / 72.0 + 0.5);
				break;

			case FontSizeUnit.Inch:
				SizeInPixels = (int) (FontSize * ImageControl.Resolution + 0.5);
				break;

			case FontSizeUnit.cm:
				SizeInPixels = (int) (FontSize * ImageControl.Resolution / 2.54 + 0.5);
				break;

			case FontSizeUnit.mm:
				SizeInPixels = (int) (FontSize * ImageControl.Resolution / 25.4 + 0.5);
				break;
			}

		// create font
		return(new Font(FontFamilyName, SizeInPixels, FontStyle, GraphicsUnit.Pixel));
		}

	/// <summary>
	/// Commit object to PDF file
	/// </summary>
	/// <param name="DisposeChart">Dispose Chart object</param>
	/// <param name="GCCollect">Activate Garbage Collector</param>
	public void CommitToPdfFile
			(
			bool DisposeChart,
			bool GCCollect
			)
		{
		// make sure not to do it twice
		if(FilePosition == 0)
			{
			// call PdfObject routine
			WriteObjectToPdfFile();

			// dispose chart
			if(DisposeChart)
				{
				Chart.Dispose();
				Chart = null;
				}

			// activate garbage collector
			if(GCCollect) GC.Collect();
			}

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Write object to PDF file
	////////////////////////////////////////////////////////////////////

	internal override void WriteObjectToPdfFile()
		{
		// convert chart to bitmap
		Picture = new Bitmap(WidthPix, HeightPix);
		Chart.DrawToBitmap(Picture, new Rectangle(0, 0, WidthPix, HeightPix));
		DisposePicture = true;

		// call Image class WriteObjectToPdfFile
		base.WriteObjectToPdfFile();
		return;
		}
	}
}
