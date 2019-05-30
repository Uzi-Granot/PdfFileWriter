/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfImageControl
//	PDF Image control.
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
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace PdfFileWriter
{
/// <summary>
/// Save image as enumeration
/// </summary>
public enum SaveImageAs
	{
	/// <summary>
	/// Jpeg format (default)
	/// </summary>
	Jpeg,

	/// <summary>
	/// PDF indexed bitmap format
	/// </summary>
	IndexedImage,

	/// <summary>
	/// convert to gray image
	/// </summary>
	GrayImage,

	/// <summary>
	/// Black and white format from bool array
	/// </summary>
	BWImage,
	}

/// <summary>
/// Image drawing control
/// </summary>
public class PdfImageControl
	{
	/// <summary>
	/// Crop image rectangle (image pixels)
	/// </summary>
	public Rectangle CropRect;

	/// <summary>
	/// Crop image rectangle (percent of image size)
	/// </summary>
	public RectangleF CropPercent;

	/// <summary>
	/// Reverse black and white (SaveImageAs.BWImage)
	/// </summary>
	public bool ReverseBW;

	/// <summary>
	/// Set output resolution 
	/// </summary>
	public double Resolution;

	/// <summary>
	/// Save image as
	/// </summary>
	public SaveImageAs SaveAs;

	/// <summary>
	/// Default Jpeg image quality
	/// </summary>
	public const int DefaultQuality = -1;

	/// <summary>
	/// Image control default constructor
	/// </summary>
	public PdfImageControl()
		{
		CropRect = Rectangle.Empty;
		CropPercent = RectangleF.Empty;
		ReverseBW = false;
		_GrayToBWCutoff = 50;
		Resolution = 0.0;
		_ImageQuality = DefaultQuality;
		SaveAs = SaveImageAs.Jpeg;
		return;
		}

	/// <summary>
	/// Gets or sets Jpeg image quality
	/// </summary>
	public int ImageQuality
		{
		get
			{
			return(_ImageQuality);
			}
		set
			{
			// set image quality
			if(value != DefaultQuality && (value < 0 || value > 100)) throw new ApplicationException("PdfImageControl.ImageQuality must be DefaultQuality or 0 to 100");
			_ImageQuality = value;
			return;
			}
		}
	internal int _ImageQuality;

	/// <summary>
	/// Gray to BW cutoff level
	/// </summary>
	public int GrayToBWCutoff
		{
		get
			{
			return(_GrayToBWCutoff);
			}
		set
			{
			if(value < 1 || value > 99) throw new ApplicationException("PdfImageControl.GrayToBWCutoff must be 1 to 99");
			_GrayToBWCutoff = value;
			}
		}
	internal int _GrayToBWCutoff;
	}
}
