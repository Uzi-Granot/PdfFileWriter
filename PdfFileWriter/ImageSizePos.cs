/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	ImageSizePoos
//	Support class for image aspect ratio calculations.
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
public static class ImageSizePos
	{
	/// <summary>
	/// Adjust image drawing area for both aspect ratio and content alignment
	/// </summary>
	/// <param name="ImageWidthPix">Image width in pixels.</param>
	/// <param name="ImageHeightPix">Image height in pixels.</param>
	/// <param name="DrawArea">Drawing area rectangle</param>
	/// <param name="Alignment">Content alignment.</param>
	/// <returns>Adjusted drawing area rectangle</returns>
	public static PdfRectangle ImageArea
			(
			int ImageWidthPix,
			int ImageHeightPix,
			PdfRectangle DrawArea,
			ContentAlignment Alignment
			)
		{
		return(ImageArea(ImageWidthPix, ImageHeightPix, DrawArea.Left, DrawArea.Bottom, DrawArea.Width, DrawArea.Height, Alignment));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Adjust image drawing area for both aspect ratio and content alignment
	/// </summary>
	/// <param name="ImageWidthPix">Image width in pixels.</param>
	/// <param name="ImageHeightPix">Image height in pixels.</param>
	/// <param name="DrawAreaLeft">Drawing area left side.</param>
	/// <param name="DrawAreaBottom">Drawing area bottom side.</param>
	/// <param name="DrawAreaWidth">Drawing area width.</param>
	/// <param name="DrawAreaHeight">Drawing area height.</param>
	/// <param name="Alignment">Content alignment.</param>
	/// <returns>Adjusted drawing area rectangle</returns>
	////////////////////////////////////////////////////////////////////
	public static PdfRectangle ImageArea
			(
			int ImageWidthPix,
			int ImageHeightPix,
			double DrawAreaLeft,
			double DrawAreaBottom,
			double DrawAreaWidth,
			double DrawAreaHeight,
			ContentAlignment Alignment
			)
		{
		double	DeltaX = 0;
		double	DeltaY = 0;
		double	Width;
		double	Height;

		// calculate height to fit aspect ratio
		Height = DrawAreaWidth * ImageHeightPix / ImageWidthPix;
		if(Height <= DrawAreaHeight)
			{
			Width = DrawAreaWidth;
			if(Height < DrawAreaHeight)
				{
				if(Alignment == ContentAlignment.MiddleLeft || Alignment == ContentAlignment.MiddleCenter || Alignment == ContentAlignment.MiddleRight)
					{
					DeltaY = 0.5 * (DrawAreaHeight - Height);
					}
				else if(Alignment == ContentAlignment.TopLeft || Alignment == ContentAlignment.TopCenter || Alignment == ContentAlignment.TopRight)
					{
					DeltaY = DrawAreaHeight - Height;
					}
				}
			}
		// calculate width to fit aspect ratio
		else
			{
			Width = DrawAreaHeight * ImageWidthPix / ImageHeightPix;
			Height = DrawAreaHeight;
			if(Width < DrawAreaWidth)
				{
				if(Alignment == ContentAlignment.TopCenter || Alignment == ContentAlignment.MiddleCenter || Alignment == ContentAlignment.BottomCenter)
					{
					DeltaX = 0.5 * (DrawAreaWidth - Width);
					}
				else if(Alignment == ContentAlignment.TopRight || Alignment == ContentAlignment.MiddleRight || Alignment == ContentAlignment.BottomRight)
					{
					DeltaX = DrawAreaWidth - Width;
					}
				}
			}

		// position rectangle
		return(new PdfRectangle(DrawAreaLeft + DeltaX, DrawAreaBottom + DeltaY, DrawAreaLeft + DeltaX + Width, DrawAreaBottom + DeltaY + Height));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Calculate best fit to preserve aspect ratio
	/// </summary>
	/// <param name="ImageWidthPix">Image width in pixels.</param>
	/// <param name="ImageHeightPix">Image height in pixels.</param>
	/// <param name="DrawAreaWidth">Drawing area width.</param>
	/// <param name="DrawAreaHeight">Drawing area height.</param>
	/// <returns>Image size in user units.</returns>
	////////////////////////////////////////////////////////////////////
	public static SizeD ImageSize
			(
			int ImageWidthPix,
			int ImageHeightPix,
			double DrawAreaWidth,
			double DrawAreaHeight
			)
		{
		SizeD OutputSize = new SizeD();
		OutputSize.Height = DrawAreaWidth * ImageHeightPix / ImageWidthPix;
		if(OutputSize.Height <= DrawAreaHeight)
			{
			OutputSize.Width = DrawAreaWidth;
			}
		else
			{
			OutputSize.Width = DrawAreaHeight * ImageWidthPix / ImageHeightPix;
			OutputSize.Height = DrawAreaHeight;
			}
		return(OutputSize);
		}
	}
}
