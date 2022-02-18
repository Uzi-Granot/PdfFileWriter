/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PrintPdfDocument
//	Create PDF document from PrintDocument page images.
//  Each page is saved as bitmap image.
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
		/// Document page crop rectangle
		/// </summary>
		/// <remarks>
		/// Dimensions are in user units. The origin is top left corner.
		/// </remarks>
		public RectangleF PageCropRect { get; set; }

		/// <summary>
		/// Image resolution in pixels per inch (default is 96)
		/// </summary>
		public double Resolution { get; set; }

		/// <summary>
		/// Save image as (default is jpeg)
		/// </summary>
		public SaveImageAs SaveAs { get; set; }

		/// <summary>
		/// Gets or sets Jpeg image quality
		/// </summary>
		public int ImageQuality
			{
			get
				{
				return _ImageQuality;
				}
			set
				{
				// set image quality
				if(value != PdfImage.DefaultQuality && (value < 0 || value > 100))
					throw new ApplicationException("PdfImageControl.ImageQuality must be PdfImage.DefaultQuality or 0 to 100");
				_ImageQuality = value;
				return;
				}
			}
		internal int _ImageQuality = PdfImage.DefaultQuality;

		/// <summary>
		/// Gray to BW cutoff level
		/// </summary>
		public int GrayToBWCutoff
			{
			get
				{
				return _GrayToBWCutoff;
				}
			set
				{
				if(value < 1 || value > 99) throw new ApplicationException("PdfImageControl.GrayToBWCutoff must be 1 to 99");
				_GrayToBWCutoff = value;
				}
			}
		internal int _GrayToBWCutoff = 50;

		/// <summary>
		/// Current PDF document
		/// </summary>
		protected PdfDocument Document;

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// PDF print document constructor
		/// </summary>
		/// <param name="Document">Current PDF document</param>
		////////////////////////////////////////////////////////////////////
		public PdfPrintDocument
				(
				PdfDocument Document
				)
			{
			// save document
			this.Document = Document;

			// set default resolution to 96 pixels per inch
			Resolution = 96.0;

			// save as jpeg
			SaveAs = SaveImageAs.Jpeg;

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
				return DefaultPageSettings.Color;
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
				return DefaultPageSettings.Margins;
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
				double LeftMargin,
				double TopMargin,
				double RightMargin,
				double BottomMargin
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

				// empty pdf image
				PdfImage PdfImage = new PdfImage(Contents.Document);
				PdfImage.Resolution = Resolution;
				PdfImage.SaveAs = SaveAs;
				PdfImage.ImageQuality = ImageQuality;
				PdfImage.GrayToBWCutoff = GrayToBWCutoff;
				PdfImage.CropRect = Rectangle.Empty;
				PdfImage.CropPercent = RectangleF.Empty;

				// no crop
				if(PageCropRect.IsEmpty)
					{
					// convert metafile image to PdfImage
					PdfImage.LoadImage(PageImage);

					// image rectangle
					PdfRectangle ImageRect = new PdfRectangle(0.0, 0.0, PageWidth, PageHeight);

					// draw the image
					Contents.DrawImage(PdfImage, ImageRect);
					}

				// crop
				else
					{
					int ImageWidth = PageImage.Width;
					int ImageHeight = PageImage.Height;
					PdfImage.CropRect.X = (int) (ImageWidth * PageCropRect.X / PageWidth + 0.5);
					PdfImage.CropRect.Y = (int) (ImageHeight * PageCropRect.Y / PageHeight + 0.5);
					PdfImage.CropRect.Width = (int) (ImageWidth * PageCropRect.Width / PageWidth + 0.5);
					PdfImage.CropRect.Height = (int) (ImageHeight * PageCropRect.Height / PageHeight + 0.5);

					// convert metafile image to PdfImage
					PdfImage.LoadImage(PageImage);

					// draw the image
					PdfRectangle ImageRect = new PdfRectangle(0, 0, PageCropRect.Width, PageCropRect.Height);
					ImageRect = ImageRect.Move(PageCropRect.X, PageHeight - PageCropRect.Y - PageCropRect.Height);
					Contents.DrawImage(PdfImage, ImageRect);
					}
				}
			return;
			}
		}
	}
