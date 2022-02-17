/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Draw barcode control class
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
	/// <summary>
	/// Draw barcode control class
	/// </summary>
	public class PdfDrawBarcodeCtrl
		{
		/// <summary>
		/// Narrow bar width
		/// </summary>
		public double NarrowBarWidth;

		/// <summary>
		/// Bar code height
		/// </summary>
		public double Height;

		/// <summary>
		/// Bar code justify (left, center, right)
		/// </summary>
		public BarcodeJustify Justify;

		/// <summary>
		/// Bar code color.
		/// The default is black and it is recommended to keep it black
		/// </summary>
		public Color Color = Color.Black;

		/// <summary>
		/// Bar code value draw text control
		/// </summary>
		public PdfDrawTextCtrl TextCtrl;

		/// <summary>
		/// Default constructor
		/// </summary>
		public PdfDrawBarcodeCtrl() {}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="Other">Bar code control to duplicate</param>
		public PdfDrawBarcodeCtrl
				(
				PdfDrawBarcodeCtrl Other
				)
			{
			NarrowBarWidth = Other.NarrowBarWidth;
			Height = Other.Height;
			Justify = Other.Justify;
			Color = Other.Color;
			TextCtrl = Other.TextCtrl;
			return;
			}
		}
	}
