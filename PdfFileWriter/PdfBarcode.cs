/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Barcode
//	Single diminsion barcode class.
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
	/// One dimension barcode base class
	/// </summary>
	/// <remarks>
	/// <para>
	/// For more information go to <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#BarcodeSupport">2.5 Barcode Support</a>
	/// </para>
	/// </remarks>
	/////////////////////////////////////////////////////////////////////
	public class PdfBarcode
		{
		/// <summary>
		/// Gets a copy of CodeArray
		/// </summary>
		public int[] CodeArray
			{
			get
				{
				return (int[]) _CodeArray.Clone();
				}
			}
		internal int[] _CodeArray;

		/// <summary>
		/// Text string
		/// </summary>
		public string Text { get; protected set; }

		/// <summary>
		/// Total number of black and white bars
		/// </summary>
		public int BarCount { get; protected set; }

		/// <summary>
		/// Total barcode width in narrow bar units.
		/// </summary>
		public int TotalWidth { get; protected set; }

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Protected barcode constructor
		/// </summary>
		/// <remarks>This class cannot be instantiated by itself.</remarks>
		/////////////////////////////////////////////////////////////////////
		protected PdfBarcode() {}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Width of single bar code at indexed position expressed in narrow bar units.
		/// </summary>
		/// <param name="Index">Bar's index number.</param>
		/// <returns>Bar's width in narrow bar units.</returns>
		/// <remarks>This virtual function must be implemented by derived class 
		/// Index range is 0 to BarCount - 1</remarks>
		/////////////////////////////////////////////////////////////////////
		public virtual int BarWidth
				(
				int Index
				)
			{
			throw new ApplicationException("Barcode.BarWidth: Not defined in derived class");
			}

		/// <summary>
		/// Calculate total barcode height including text
		/// </summary>
		/// <param name="BarcodeCtrl">Draw barcode control</param>
		/// <returns>Bounding box rectangle</returns>
		public virtual PdfRectangle GetBarcodeBox
				(
				PdfDrawBarcodeCtrl BarcodeCtrl
				)
			{
			// draw barcode with no text
			// the bounding box is equal to the barcode rectangle
			if(BarcodeCtrl.TextCtrl == null) return new PdfRectangle(0, 0, BarcodeCtrl.NarrowBarWidth * TotalWidth, BarcodeCtrl.Height);

			// barcode width and height
			double BarcodeWidth = BarcodeCtrl.NarrowBarWidth * TotalWidth;
			double BarcodeHeight = BarcodeCtrl.Height;

			// text width and height
			double TextWidth = BarcodeCtrl.TextCtrl.TextWidth(Text);
			double TextHeight = BarcodeCtrl.TextCtrl.LineSpacing;

			// barcode origin within bounds rectangle
			double OriginX = 0;
			double OriginY = TextHeight;

			// if text width is greater than barcode width, adjust left origin
			if(TextWidth > BarcodeWidth)
				{
				OriginX = 0.5 * (TextWidth - BarcodeWidth);
				BarcodeWidth = TextWidth;
				}

			// Barcode bounding box
			return new PdfRectangle(-OriginX, -OriginY, BarcodeWidth - OriginX, BarcodeHeight);
			}
		}
	}
