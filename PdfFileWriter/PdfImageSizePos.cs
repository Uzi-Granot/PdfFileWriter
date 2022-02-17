/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	ImageSizePoos
//	Support class for image aspect ratio calculations.
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

namespace PdfFileWriter
	{
	/////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Image size and position class
	/// </summary>
	/// <remarks>
	/// Delta X and Y are the adjustments to image position to
	/// meet the content alignment request.
	/// </remarks>
	/////////////////////////////////////////////////////////////////////
	public static class PdfImageSizePos
		{
		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Calculates image size to preserve aspect ratio.
		/// </summary>
		/// <param name="Image">PdfImage defining image width and height.</param>
		/// <param name="DrawingArea">Image display area.</param>
		/// <returns>Adjusted image display area.</returns>
		/// <remarks>
		/// Calculates best fit to preserve aspect ratio.
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public static SizeD ImageSize
				(
				PdfImage Image,
				SizeD DrawingArea
				)
			{
			return ImageSize(Image.WidthPix, Image.HeightPix, DrawingArea);
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Calculate best fit to preserve aspect ratio
		/// </summary>
		/// <param name="ImageWidthPix">Image width in pixels.</param>
		/// <param name="ImageHeightPix">Image height in pixels.</param>
		/// <param name="DrawingArea">Drawing area.</param>
		/// <returns>Image size in user units.</returns>
		////////////////////////////////////////////////////////////////////
		public static SizeD ImageSize
				(
				int ImageWidthPix,
				int ImageHeightPix,
				SizeD DrawingArea
				)
			{
			SizeD AdjustedArea = new SizeD();
			AdjustedArea.Height = DrawingArea.Width * ImageHeightPix / ImageWidthPix;
			if(AdjustedArea.Height <= DrawingArea.Height)
				{
				AdjustedArea.Width = DrawingArea.Width;
				}
			else
				{
				AdjustedArea.Width = DrawingArea.Height * ImageWidthPix / ImageHeightPix;
				AdjustedArea.Height = DrawingArea.Height;
				}
			return AdjustedArea;
			}

		/// <summary>
		/// Adjust image drawing area for both aspect ratio and content alignment
		/// </summary>
		/// <param name="Image">PdfImage with width and height in pixels.</param>
		/// <param name="DrawArea">Drawing area rectangle</param>
		/// <param name="Alignment">Content alignment.</param>
		/// <returns>Adjusted drawing area rectangle</returns>
		public static PdfRectangle ImageArea
				(
				PdfImage Image,
				PdfRectangle DrawArea,
				ContentAlignment Alignment
				)
			{
			return ImageArea(Image.WidthPix, Image.HeightPix, DrawArea, Alignment);
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Adjust image drawing area for both aspect ratio and content alignment
		/// </summary>
		/// <param name="ImageWidthPix">Image width in pixels.</param>
		/// <param name="ImageHeightPix">Image height in pixels.</param>
		/// <param name="DrawingArea">Drawing area.</param>
		/// <param name="Alignment">Content alignment.</param>
		/// <returns>Adjusted drawing area rectangle</returns>
		////////////////////////////////////////////////////////////////////
		public static PdfRectangle ImageArea
				(
				int ImageWidthPix,
				int ImageHeightPix,
				PdfRectangle DrawingArea,
				ContentAlignment Alignment
				)
			{
			// calculate adjusted area to maintain aspect ratio
			SizeD AdjustedSize = ImageSize(ImageWidthPix, ImageHeightPix, new SizeD(DrawingArea.Width, DrawingArea.Height));

			// bottom left corner
			PdfRectangle Result = new PdfRectangle(DrawingArea.Left, DrawingArea.Bottom,
				DrawingArea.Left + AdjustedSize.Width, DrawingArea.Bottom + AdjustedSize.Height);

			// switch based on alignment
			switch(Alignment)
				{
				case ContentAlignment.BottomLeft:
					break;

				case ContentAlignment.BottomCenter:
					return Result.Move(0.5 * (DrawingArea.Width - AdjustedSize.Width), 0);

				case ContentAlignment.BottomRight:
					return Result.Move(DrawingArea.Width - AdjustedSize.Width, 0);

				case ContentAlignment.MiddleLeft:
					return Result.Move(0, 0.5 * (DrawingArea.Height - AdjustedSize.Height));

				case ContentAlignment.MiddleCenter:
					return Result.Move(0.5 * (DrawingArea.Width - AdjustedSize.Width), 0.5 * (DrawingArea.Height - AdjustedSize.Height));

				case ContentAlignment.MiddleRight:
					return Result.Move(DrawingArea.Width - AdjustedSize.Width, 0.5 * (DrawingArea.Height - AdjustedSize.Height));

				case ContentAlignment.TopLeft:
					return Result.Move(0, DrawingArea.Height - AdjustedSize.Height);

				case ContentAlignment.TopCenter:
					return Result.Move(0.5 * (DrawingArea.Width - AdjustedSize.Width), DrawingArea.Height - AdjustedSize.Height);

				case ContentAlignment.TopRight:
					return Result.Move(DrawingArea.Width - AdjustedSize.Width, DrawingArea.Height - AdjustedSize.Height);
				}

			return null;
			}

		}
	}
