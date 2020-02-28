using System.Diagnostics;
using System.Drawing;
using PdfFileWriter;

namespace TestPdfFileWriter
	{
	public class LayersExample
	{
	// define font
	private PdfFont ArialFont;
	private PdfDocument Document;

	private static string QRCodeArticle = "https://www.codeproject.com/Articles/1250071/QR-Code-Encoder-and-Decoder-NET-class-library-writ";
	private static string Pdf417Article = "https://www.codeproject.com/Articles/1347529/PDF417-Barcode-Encoder-NET-Class-Library-and-Demo";

	public void Test
			(
			bool Debug,
			string	InputFileName
			)
		{
		// create document
		using(Document = new PdfDocument(PaperType.Letter, false, UnitOfMeasure.Inch, InputFileName))
			{
			// set document page mode to open the layers panel
			Document.InitialDocDisplay = InitialDocDisplay.UseLayers;

			// define font
			ArialFont = PdfFont.CreatePdfFont(Document, "Arial", FontStyle.Bold);

			// open layer control object (in PDF terms optional content object)
			PdfLayers Layers = new PdfLayers(Document, "PDF layers group");

			// set layer panel to incluse all layers including ones that are not visible
			Layers.ListMode = ListMode.AllPages;

			// Add new page
			PdfPage Page = new PdfPage(Document);

			// Add contents to page
			PdfContents Contents = new PdfContents(Page);

			// heading
			Contents.DrawText(ArialFont, 24, 4.25, 10, TextJustify.Center, "PDF File Writer Layer Test/Demo");

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
			PdfAnnotation StickyNote = Page.AddStickyNote(2.0, 9.0, "My sticky note", StickyNoteIcon.Note);
			StickyNote.LayerControl = DrawingTest;

			// draw a single layer
			Contents.LayerStart(Rectangle);
			Contents.DrawText(ArialFont, 14, 1.0, 8.0, TextJustify.Left, "Draw rectangle");
			Contents.LayerEnd();

			// draw a single layer
			Contents.LayerStart(HorLines);
			Contents.DrawText(ArialFont, 14, 1.0, 7.5, TextJustify.Left, "Draw horizontal lines");
			Contents.LayerEnd();

			// draw a single layer
			Contents.LayerStart(VertLines);
			Contents.DrawText(ArialFont, 14, 1.0, 7.0, TextJustify.Left, "Draw vertical lines");
			Contents.LayerEnd();

			double Left = 4.0;
			double Right = 7.0;
			double Top = 9.0;
			double Bottom = 6.0;

			// draw a single layer
			Contents.LayerStart(Rectangle);
			Contents.SaveGraphicsState();
			Contents.SetLineWidth(0.1);
			Contents.SetColorStroking(Color.Black);
			Contents.SetColorNonStroking(Color.LightBlue);
			Contents.DrawRectangle(Left, Bottom, 3.0, 3.0, PaintOp.CloseFillStroke);
			Contents.RestoreGraphicsState();
			Contents.LayerEnd();

			// save graphics state
			Contents.SaveGraphicsState();

			// draw a single layer
			Contents.SetLineWidth(0.02);
			Contents.LayerStart(HorLines);
			for(int Row = 1; Row < 6; Row++)
				{
				Contents.DrawLine(Left, Bottom + 0.5 * Row, Right, Bottom + 0.5 * Row);
				}
			Contents.LayerEnd();

			// draw a single layer
			Contents.LayerStart(VertLines);
			for(int Col = 1; Col < 6; Col++)
				{
				Contents.DrawLine(Left + 0.5 * Col, Bottom, Left + 0.5 * Col, Top);
				}
			Contents.LayerEnd();

			// restore graphics state
			Contents.RestoreGraphicsState();

			// terminate a group of layers
			Contents.LayerEnd();

			// define QRCode barcode
			QREncoder QREncoder = new QREncoder();
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
			Contents.DrawText(ArialFont, 14, 1.0, 2.5, TextJustify.Left, "QRCode Barcode");
			Contents.DrawImage(QRImage, 3.7, 2.5 - 1.75, 3.5);
			Contents.LayerEnd();

			// draw a single layer
			Contents.LayerStart(Pdf417Layer);
			Contents.DrawText(ArialFont, 14, 1.0, 2.5, TextJustify.Left, "PDF417 Barcode");
			Contents.DrawImage(Pdf417Image, 3.7, 2.5 - 1.75 * Pdf417Encoder.ImageHeight / Pdf417Encoder.ImageWidth, 3.5);
			Contents.LayerEnd();

			// draw a single layer
			Contents.LayerStart(NoBarcodeLayer);
			Contents.DrawText(ArialFont, 14, 1.0, 3.0, TextJustify.Left, "Display no barcode");
			Contents.LayerEnd();

			// create pdf file
			Document.CreateFile();

			// start default PDF reader and display the file
			Process Proc = new Process();
			Proc.StartInfo = new ProcessStartInfo(InputFileName);
			Proc.Start();
			}
		return;
		}
	}
}
