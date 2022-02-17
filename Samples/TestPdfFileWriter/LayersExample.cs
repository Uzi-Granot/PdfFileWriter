/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter II
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	Layer example
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
	/// Layers example
	/// </summary>
	public class LayersExample
		{
		private PdfDocument Document;

		private static readonly string
			QRCodeArticle = "https://www.codeproject.com/Articles/1250071/QR-Code-Encoder-and-Decoder-NET-class-library-writ";
		private static readonly string
			Pdf417Article = "https://www.codeproject.com/Articles/1347529/PDF417-Barcode-Encoder-NET-Class-Library-and-Demo";

		/// <summary>
		/// Create layers example PDF document
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
				// set or clear debug flag
				Document.Debug = Debug;

				// set document page mode to open the layers panel
				Document.InitialDocDisplay = InitialDocDisplay.UseLayers;

				// define font
				PdfDrawTextCtrl ArialFont = new PdfDrawTextCtrl(Document, "Arial", FontStyle.Bold, 14);

				// open layer control object (in PDF terms optional content object)
				PdfLayers Layers = new PdfLayers(Document, "PDF layers group");

				// set layer panel to incluse all layers including ones that are not visible
				Layers.ListMode = ListMode.AllPages;

				// Add new page
				PdfPage Page = new PdfPage(Document);

				// Add contents to page
				PdfContents Contents = new PdfContents(Page);

				// heading
				PdfDrawTextCtrl HeadingFont = new PdfDrawTextCtrl(ArialFont);
				HeadingFont.FontSize = 24;
				HeadingFont.Justify = TextJustify.Center;
				Contents.DrawText(HeadingFont, 4.25, 10, "PDF File Writer Layer Test/Demo");

				// define layers
				PdfLayer DrawingTest = new PdfLayer(Layers, "Drawing Test");
				PdfLayer Rectangle = new PdfLayer(Layers, "Rectangle");
				PdfLayer HorLines = new PdfLayer(Layers, "Horizontal Lines");
				PdfLayer VertLines = new PdfLayer(Layers, "Vertical Lines");
				PdfLayer QRCodeLayer = new PdfLayer(Layers, "QRCode barcode");
				PdfLayer Pdf417Layer = new PdfLayer(Layers, "PDF417 barcode");
				PdfLayer NoBarcodeLayer = new PdfLayer(Layers, "No barcode");

				// combine three layers into one group of radio buttons
				QRCodeLayer.RadioButton = "Barcode";
				Pdf417Layer.RadioButton = "Barcode";
				NoBarcodeLayer.RadioButton = "Barcode";

				// set the order of layers in the layer pane
				Layers.DisplayOrder(DrawingTest);
				Layers.DisplayOrder(Rectangle);
				Layers.DisplayOrder(HorLines);
				Layers.DisplayOrder(VertLines);
				Layers.DisplayOrderStartGroup("Barcode group");
				Layers.DisplayOrder(QRCodeLayer);
				Layers.DisplayOrder(Pdf417Layer);
				Layers.DisplayOrder(NoBarcodeLayer);
				Layers.DisplayOrderEndGroup();

				// start a group layer
				Contents.LayerStart(DrawingTest);

				// sticky note annotation
				Contents.DrawText(ArialFont, 1, 8.85, "Sticky note");
				PdfAnnotStickyNote StickyNote = new PdfAnnotStickyNote(Document, "My sticky note", StickyNoteIcon.Note);
				StickyNote.AnnotRect = new PdfRectangle(2.2, 9, 2.2, 9);
				StickyNote.OptionalContent = DrawingTest;
				StickyNote.ColorSpecific = Color.Red;

				// draw a single layer
				Contents.LayerStart(Rectangle);
				Contents.DrawText(ArialFont, 1, 8, "Draw rectangle");
				Contents.LayerEnd();

				// draw a single layer
				Contents.LayerStart(HorLines);
				Contents.DrawText(ArialFont, 1, 7.5, "Draw horizontal lines");
				Contents.LayerEnd();

				// draw a single layer
				Contents.LayerStart(VertLines);
				Contents.DrawText(ArialFont, 1, 7, "Draw vertical lines");
				Contents.LayerEnd();

				double Left = 4.0;
				double Right = 7.0;
				double Top = 9.0;
				double Bottom = 6.0;

				// draw a single layer
				Contents.LayerStart(Rectangle);

				// draw rectangle
				PdfRectangle Rect = new PdfRectangle(Left, Bottom, Left + 3, Bottom + 3);
				PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
				DrawCtrl.Paint = DrawPaint.BorderAndFill;
				DrawCtrl.BackgroundTexture = Color.LightBlue;
				DrawCtrl.BorderWidth = 0.1;
				Contents.DrawGraphics(DrawCtrl, Rect);
				Contents.LayerEnd();

				// save graphics state
				Contents.SaveGraphicsState();

				// draw a single layer
				Contents.SetLineWidth(0.02);
				Contents.LayerStart(HorLines);
				for(int Row = 1; Row < 6; Row++)
					{ 
					LineD HorLine = new LineD(Left, Bottom + 0.5 * Row, Right, Bottom + 0.5 * Row); 
					Contents.DrawLine(HorLine);
					}
				Contents.LayerEnd();

				// draw a single layer
				Contents.LayerStart(VertLines);
				for(int Col = 1; Col < 6; Col++)
					{ 
					LineD VertLine = new LineD(Left + 0.5 * Col, Bottom, Left + 0.5 * Col, Top);
					Contents.DrawLine(VertLine);
					}
				Contents.LayerEnd();

				// restore graphics state
				Contents.RestoreGraphicsState();

				// terminate a group of layers
				Contents.LayerEnd();

				// define QRCode barcode
				PdfQREncoder QREncoder = new PdfQREncoder();
				QREncoder.ErrorCorrection = ErrorCorrection.M;
				QREncoder.Encode(QRCodeArticle);
				PdfImage QRImage = new PdfImage(Document);
				QRImage.LoadImage(QREncoder);

				// define PDF417 barcode
				Pdf417Encoder Pdf417Encoder = new Pdf417Encoder();
				Pdf417Encoder.ErrorCorrection = ErrorCorrectionLevel.AutoMedium;
				Pdf417Encoder.Encode(Pdf417Article);
				PdfImage Pdf417Image = new PdfImage(Document);
				Pdf417Image.LoadImage(Pdf417Encoder);

				// draw a single layer
				Contents.LayerStart(QRCodeLayer);
				Contents.DrawText(ArialFont, 1, 2.5, "QRCode Barcode");
				Contents.DrawImage(QRImage, 3.7, 2.5 - 1.75, 3.5);
				Contents.LayerEnd();

				// draw a single layer
				Contents.LayerStart(Pdf417Layer);
				Contents.DrawText(ArialFont, 1, 2.5, "PDF417 Barcode");
				Contents.DrawImage(Pdf417Image, 3.7, 2.5 - 1.75 * Pdf417Encoder.ImageHeight / Pdf417Encoder.ImageWidth, 3.5);
				Contents.LayerEnd();

				// draw a single layer
				Contents.LayerStart(NoBarcodeLayer);
				Contents.DrawText(ArialFont, 1, 3, "Display no barcode");
				Contents.LayerEnd();

				// create pdf file
				Document.CreateFile();

				// start default PDF reader and display the file
				Process Proc = new Process();
				Proc.StartInfo = new ProcessStartInfo(FileName) {UseShellExecute = true};
				Proc.Start();
				}
			return;
			}
		}
	}
