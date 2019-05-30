/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PrintPdfDocument
//	Create PDF document from PrintDocument page images.
//  Each page is saved as bitmap image.
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
using System.Drawing;
using System.Drawing.Printing;

namespace PdfFileWriter
{
/// <summary>
/// PDF print document class
/// </summary>
/// <remarks>
/// <para>
/// It is a derived class of PrintDocument.
/// The class converts the metafile output of PrintDocument
/// to an image. The image is displayed in the PDF document.
/// </para>
/// <para>
/// For more information go to <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#PrintDocumentSupport">2.11 Print Document Support</a>
/// </para>
/// </remarks>
public class PdfPrintDocument : PrintDocument
	{
	/// <summary>
	/// Image control
	/// </summary>
	public PdfImageControl ImageControl;

	/// <summary>
	/// Document page crop rectangle
	/// </summary>
	/// <remarks>
	/// Dimensions are in user units. The origin is top left corner.
	/// </remarks>
	public RectangleF PageCropRect {get; set;}

	/// <summary>
	/// Current PDF document
	/// </summary>
	protected PdfDocument Document;

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// PDF print document constructor
	/// </summary>
	/// <param name="Document">Current PDF document</param>
	/// <remarks>
	/// Set resolution to 96 pixels per inch
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public PdfPrintDocument
			(
			PdfDocument Document
			) : this(Document, null) {}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// PDF print document constructor
	/// </summary>
	/// <param name="Document">Current PDF document</param>
	/// <param name="ImageControl">Image control</param>
	////////////////////////////////////////////////////////////////////
	public PdfPrintDocument
			(
			PdfDocument		Document,
			PdfImageControl ImageControl
			)
		{
		// save document
		this.Document = Document;

		// save image control
		if(ImageControl == null) ImageControl = new PdfImageControl();
		this.ImageControl = ImageControl;

		// set default resolution to 96 pixels per inch
		if(ImageControl.Resolution == 0) ImageControl.Resolution = 96.0;

		// make sure image control crop rectangles are empty
		this.ImageControl.CropRect = Rectangle.Empty;
		this.ImageControl.CropPercent = RectangleF.Empty;

		// create print document and preview controller objects
		PrintController = new PreviewPrintController();

		// copy document's page size to default settings
		// convert page size from points to 100th of inch
		// do not set lanscape flag
		PaperSize PSize = new PaperSize();
		PSize.Width = (int) (Document.PageSize.Width / 0.72 + 0.5);
		PSize.Height = (int) (Document.PageSize.Height / 0.72 + 0.5);
		DefaultPageSettings.PaperSize = PSize;

		// assume document is in color
		DefaultPageSettings.Color = true;
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Gets or sets DocumentInColor flag.
	/// </summary>
	////////////////////////////////////////////////////////////////////
	public bool DocumentInColor
		{
		set
			{
			DefaultPageSettings.Color = value;
			return;
			}
		get
			{
			return(DefaultPageSettings.Color);
			}
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Gets margins in 100th of an inch
	/// </summary>
	////////////////////////////////////////////////////////////////////
	public Margins GetMargins
		{
		get
			{
			return(DefaultPageSettings.Margins);
			}
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Sets margins in user units.
	/// </summary>
	/// <param name="LeftMargin">Left margin</param>
	/// <param name="TopMargin">Top margin</param>
	/// <param name="RightMargin">Right margin</param>
	/// <param name="BottomMargin">Bottom margin</param>
	////////////////////////////////////////////////////////////////////
	public void SetMargins
			(
			double			LeftMargin,
			double			TopMargin,
			double			RightMargin,
			double			BottomMargin
			)
		{
		Margins Margins = DefaultPageSettings.Margins;
		Margins.Left = (int) (LeftMargin * Document.ScaleFactor / 0.72 + 0.5);
		Margins.Top = (int) (TopMargin * Document.ScaleFactor / 0.72 + 0.5);
		Margins.Right = (int) (RightMargin * Document.ScaleFactor / 0.72 + 0.5);
		Margins.Bottom = (int) (BottomMargin * Document.ScaleFactor / 0.72 + 0.5);
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Add pages to PDF document
	/// </summary>
	/// <remarks>
	/// The PrintDoc.Print method will call BeginPrint method,
	/// next it will call multiple times PrintPage method and finally
	/// it will call EndPrint method. 
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public void AddPagesToPdfDocument()
		{
		// print the document by calling BeginPrint, PrintPage multiple times and finally EndPrint
		Print();

		// get printing results in the form of array of images one per page
		// image format is Metafile
		PreviewPageInfo[] PageInfo = ((PreviewPrintController) PrintController).GetPreviewPageInfo();

		// page size in user units
		double PageWidth = Document.PageSize.Width / Document.ScaleFactor;
		double PageHeight = Document.PageSize.Height / Document.ScaleFactor;

		// add pages to pdf document
		for(int ImageIndex = 0; ImageIndex < PageInfo.Length; ImageIndex++)
			{
			// add page to document
			PdfPage Page = new PdfPage(Document);

			// add contents to the page
			PdfContents Contents = new PdfContents(Page);

			// page image
			Image PageImage = PageInfo[ImageIndex].Image;

			// no crop
			if(PageCropRect.IsEmpty)
				{
				// convert metafile image to PdfImage
				PdfImage Image = new PdfImage(Contents.Document, PageImage, ImageControl);

				// draw the image
				Contents.DrawImage(Image, 0.0, 0.0, PageWidth, PageHeight);
				}

			// crop
			else
				{
				int ImageWidth = PageImage.Width;
				int ImageHeight = PageImage.Height;
				ImageControl.CropRect.X = (int) (ImageWidth * PageCropRect.X / PageWidth + 0.5);
				ImageControl.CropRect.Y = (int) (ImageHeight * PageCropRect.Y / PageHeight + 0.5);
				ImageControl.CropRect.Width = (int) (ImageWidth * PageCropRect.Width / PageWidth + 0.5);
				ImageControl.CropRect.Height = (int) (ImageHeight * PageCropRect.Height / PageHeight + 0.5);
				
				// convert metafile image to PdfImage
				PdfImage PdfPageImage = new PdfImage(Contents.Document, PageImage, ImageControl);

				// draw the image
				Contents.DrawImage(PdfPageImage, PageCropRect.X, PageHeight - PageCropRect.Y - PageCropRect.Height, PageCropRect.Width, PageCropRect.Height);
				}
			}
		return;
		}
	}
}
