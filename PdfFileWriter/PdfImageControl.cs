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
	/// PdfImageControl is obolete. See latest documentation
	/// </summary>
	public class PdfImageControl
		{
		#pragma warning disable 1591
		private const bool ObsoleteError = false;
		private const string ObsoleteMsg = "This PdfImageControl class is obsolete. See latest documentation.";

		[Obsolete(ObsoleteMsg, ObsoleteError)]
		public Rectangle CropRect;

		[Obsolete(ObsoleteMsg, ObsoleteError)]
		public RectangleF CropPercent;

		[Obsolete(ObsoleteMsg, ObsoleteError)]
		public bool ReverseBW;

		[Obsolete(ObsoleteMsg, ObsoleteError)]
		public double Resolution;

		[Obsolete(ObsoleteMsg, ObsoleteError)]
		public SaveImageAs SaveAs;

		[Obsolete(ObsoleteMsg, ObsoleteError)]
		public const int DefaultQuality = -1;

		[Obsolete(ObsoleteMsg, ObsoleteError)]
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

		[Obsolete(ObsoleteMsg, ObsoleteError)]
		public int ImageQuality
			{
			get
				{
				return _ImageQuality;
				}
			set
				{
				// set image quality
				if(value != DefaultQuality && (value < 0 || value > 100))
					throw new ApplicationException("PdfImageControl.ImageQuality must be DefaultQuality or 0 to 100");
				_ImageQuality = value;
				return;
				}
			}
		internal int _ImageQuality;

		[Obsolete(ObsoleteMsg, ObsoleteError)]
		public int GrayToBWCutoff
			{
			get
				{
				return _GrayToBWCutoff;
				}
			set
				{
				if(value < 1 || value > 99)
					throw new ApplicationException("PdfImageControl.GrayToBWCutoff must be 1 to 99");
				_GrayToBWCutoff = value;
				}
			}
		internal int _GrayToBWCutoff;
		}
	}
