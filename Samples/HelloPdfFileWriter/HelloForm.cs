namespace HelloPdfFileWriter
	{
	using System.Diagnostics;
	using PdfFileWriter;

	public partial class HelloForm : Form
		{
		public HelloForm()
			{
			InitializeComponent();
			}

		// create PDF document
		private void OnClick(object sender, EventArgs e)
			{
			// Create empty document
			using(PdfDocument Document = new PdfDocument(PaperType.Letter, false, UnitOfMeasure.Inch, "HelloPdfDocument.pdf"))
				{
				// Add new page
				PdfPage Page = new PdfPage(Document);

				// Add contents to page
				PdfContents Contents = new PdfContents(Page);

				// create font
				PdfFont ArialNormal = PdfFont.CreatePdfFont(Document, "Arial", FontStyle.Regular, true);
				PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialNormal, 18.0);
				
				// draw text
				TextCtrl.Justify = TextJustify.Center;
				Contents.DrawText(TextCtrl, 4.5, 7, "Hello PDF Document");

				// load image
				PdfImage Image = new PdfImage(Document);
				Image.LoadImage("..\\..\\..\\HappyFace.jpg");

				// draw image
				PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
				DrawCtrl.Paint = DrawPaint.Fill;
				DrawCtrl.BackgroundTexture = Image;
				Contents.DrawGraphics(DrawCtrl, new PdfRectangle(3.5, 4.8, 5.5, 6.8));

				// create pdf file
				Document.CreateFile();
				}

			// start default PDF reader and display the file
			Process Proc = new Process();
			Proc.StartInfo = new ProcessStartInfo("HelloPdfDocument.pdf") { UseShellExecute = true };
			Proc.Start();
			}
		}
	}