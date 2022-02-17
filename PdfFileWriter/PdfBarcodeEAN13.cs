/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Barcode EAN-13 or UPC-A class
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

using System.Text;

namespace PdfFileWriter
	{
	/////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Barcode EAN-13 or UPC-A class
	/// </summary>
	/// <remarks>
	/// Barcode EAN-13 or UPC-A
	/// Note UPC-A is a subset of EAN-13
	/// UPC-A is made of 12 digits
	/// EAN-13 is made of 13 digits
	/// If the first digit of EAN-13 is zero it is considered to be
	/// UPC-A. The zero will be eliminated.
	/// The barcode in both cases is made out of 12 symbols.
	/// </remarks>
	/////////////////////////////////////////////////////////////////////
	public class PdfBarcodeEAN13 : PdfBarcode
		{
		/// <summary>
		/// Barcode length
		/// </summary>
		/// <remarks>
		/// Each code EAN-13 or UPC-A code is encoded as 2 black bars and 2 white bars
		/// there are exactly 12 characters in a barcode.
		/// </remarks>
		public const int BARCODE_LEN = 12;

		/// <summary>
		/// Barcode half length
		/// </summary>
		/// <remarks>
		/// Each code EAN-13 or UPC-A code is encoded as 2 black bars and 2 white bars
		/// there are exactly 12 characters in a barcode
		/// </remarks>
		public const int BARCODE_HALF_LEN = 6;

		/// <summary>
		/// Lead bars
		/// </summary>
		public const int LEAD_BARS = 3;

		/// <summary>
		/// Separator bars
		/// </summary>
		public const int SEPARATOR_BARS = 5;

		/// <summary>
		/// Code character bars
		/// </summary>
		public const int CODE_CHAR_BARS = 4;

		/// <summary>
		/// Code character width
		/// </summary>
		public const int CODE_CHAR_WIDTH = 7;

		/// <summary>
		/// Code table for Barcode EAN-13 or UPC-A
		/// </summary>
		/// <remarks>Array size [20, 4]</remarks>
		public static readonly byte[,] CodeTable =
			{
			{3, 2, 1, 1},		// A-0 Odd parity
			{2, 2, 2, 1},		// A-1
			{2, 1, 2, 2},		// A-2
			{1, 4, 1, 1},		// A-3
			{1, 1, 3, 2},		// A-4
			{1, 2, 3, 1},		// A-5
			{1, 1, 1, 4},		// A-6
			{1, 3, 1, 2},		// A-7
			{1, 2, 1, 3},		// A-8
			{3, 1, 1, 2},		// A-9
			{1, 1, 2, 3},		// B-0 Even Parity
			{1, 2, 2, 2},		// B-1
			{2, 2, 1, 2},		// B-2
			{1, 1, 4, 1},		// B-3
			{2, 3, 1, 1},		// B-4
			{1, 3, 2, 1},		// B-5
			{4, 1, 1, 1},		// B-6
			{2, 1, 3, 1},		// B-7
			{3, 1, 2, 1},		// B-8
			{2, 1, 1, 3},		// B-9
			};

		/// <summary>
		/// Parity table
		/// </summary>
		/// <remarks>First digit of EAN-13 odd/even translation table</remarks>
		public static readonly byte[,] ParityTable =
			{
			{ 0,  0,  0,  0,  0},	// 0
			{ 0, 10,  0, 10, 10},	// 1
			{ 0, 10, 10,  0, 10},	// 2
			{ 0, 10, 10, 10,  0},	// 3
			{10,  0,  0, 10, 10},	// 4
			{10, 10,  0,  0, 10},	// 5
			{10, 10, 10,  0,  0},	// 6
			{10,  0, 10,  0, 10},	// 7
			{10,  0, 10, 10,  0},	// 8
			{10, 10,  0, 10,  0},	// 9
			};

		private int FirstDigit;

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Barcode width
		/// </summary>
		/// <param name="BarIndex">Code array index</param>
		/// <returns>Barcode EAN-13 single bar width</returns>
		////////////////////////////////////////////////////////////////////
		public override int BarWidth
				(
				int BarIndex
				)
			{
			// leading bars
			if(BarIndex < LEAD_BARS) return 1;

			// left side 6 digits
			if(BarIndex < LEAD_BARS + BARCODE_HALF_LEN * CODE_CHAR_BARS)
				{
				int Index = BarIndex - LEAD_BARS;
				return CodeTable[_CodeArray[Index / CODE_CHAR_BARS], Index % CODE_CHAR_BARS];
				}

			// separator bars
			if(BarIndex < LEAD_BARS + BARCODE_HALF_LEN * CODE_CHAR_BARS + SEPARATOR_BARS) return 1;

			// right side 6 digits
			if(BarIndex < LEAD_BARS + BARCODE_LEN * CODE_CHAR_BARS + SEPARATOR_BARS)
				{
				int Index = BarIndex - (LEAD_BARS + BARCODE_HALF_LEN * CODE_CHAR_BARS + SEPARATOR_BARS);
				return CodeTable[_CodeArray[BARCODE_HALF_LEN + Index / CODE_CHAR_BARS], Index % CODE_CHAR_BARS];
				}

			// trailing bars
			return 1;
			}

		/// <summary>
		/// Calculate total barcode height including text
		/// </summary>
		/// <param name="BarcodeCtrl">Draw barcode control</param>
		/// <returns>Bounding box rectangle</returns>
		public override PdfRectangle GetBarcodeBox
				(
				PdfDrawBarcodeCtrl BarcodeCtrl
				)
			{
			// draw barcode with no text
			// the bounding box is equal to the barcode rectangle
			if(BarcodeCtrl.TextCtrl == null) return new PdfRectangle(0, 0, BarcodeCtrl.NarrowBarWidth * TotalWidth, BarcodeCtrl.Height);

			// one digit width
			double OriginX = BarcodeCtrl.TextCtrl.TextWidth("0");

			// calculate bounding box width
			double BBoxWidth = OriginX + BarcodeCtrl.NarrowBarWidth * TotalWidth;
			if(Text.Length == 12) BBoxWidth += OriginX;

			// text height
			double OriginY = BarcodeCtrl.TextCtrl.LineSpacing - 5.0 * BarcodeCtrl.NarrowBarWidth;
			if(OriginY < 0) OriginY = 0;

			// Barcode bounding box
			return new PdfRectangle(-OriginX, -OriginY, BBoxWidth - OriginX, BarcodeCtrl.Height);
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Barcode EAN13 Constructor
		/// </summary>
		/// <param name="Text">Input text</param>
		/// <remarks>
		/// <para>
		/// Convert text to code EAN-13 or UPC-A.
		/// </para>
		/// <para>
		/// All characters must be digits.
		/// </para>
		/// <para>
		/// The code is EAN-13 if string length is 13 characters
		/// and first digit is not zero.
		/// </para>
		/// <para>
		/// The code is UPC-A if string length is 12 characters
		/// or string length is 13 and first character is zero.
		/// </para>
		/// <para>
		/// The last character is a checksum. The checksum must be
		/// given, however the constructor calculates the checksum and
		/// override the one given. In other words, if you do not
		/// know the checksum just set the last digit to 0.
		/// </para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public PdfBarcodeEAN13
				(
				string Text
				)
			{
			// test argument
			if(string.IsNullOrEmpty(Text))
				throw new ApplicationException("Barcode EAN-13/UPC-A: Text must not be null");

			// text length
			int Length = Text.Length;
			if(Length != 12 && Length != 13)
				throw new ApplicationException("Barcode EAN-13/UPC-A: Text must be 12 for UPC-A or 13 for EAN-13");

			// first digit
			FirstDigit = Length == 12 ? 0 : (int) Text[0] - '0';
			if(FirstDigit < 0 || FirstDigit > 9)
				throw new ApplicationException("Barcode EAN-13/UPC-A: Invalid character (must be 0 to 9)");

			// save text
			this.Text = Text;

			// barcode array
			_CodeArray = new int[BARCODE_LEN];

			// encode the text
			int CodePtr = 0;
			for(int Index = Length == 12 ? 0 : 1; Index < Length; Index++)
				{
				int CodeValue = (int) Text[Index] - '0';
				if(CodeValue < 0 || CodeValue > 9)
					throw new ApplicationException("Barcode EAN-13/UPC-A: Invalid character (must be 0 to 9)");
				if(FirstDigit != 0 && Index >= 2 && Index <= 6) CodeValue += ParityTable[FirstDigit, Index - 2];
				_CodeArray[CodePtr++] = CodeValue;
				}

			// calculate checksum
			Checksum();

			// add it to text
			this.Text = Text.Substring(0, Text.Length - 1) + ((char) ('0' + _CodeArray[BARCODE_LEN - 1])).ToString();

			// set number of bars
			BarCount = BARCODE_LEN * CODE_CHAR_BARS + 2 * LEAD_BARS + SEPARATOR_BARS;

			// set total width
			TotalWidth = BARCODE_LEN * CODE_CHAR_WIDTH + 2 * LEAD_BARS + SEPARATOR_BARS;

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Barcode EAN13 constructor.
		/// </summary>
		/// <param name="_CodeArray">Code array input.</param>
		///	<remarks>
		/// <para>
		/// The constructor sets CodeArray and converts it to text.
		/// </para>
		/// <para>
		/// CodeArray must be 12 elements long for both EAN-13 or UPC-A.
		/// </para>
		/// <para>
		/// In the case of UPC-A the 12 elements of code array correspond
		/// one to one with the 12 digits of the encoded value.
		/// </para>
		/// <para>
		/// In the case of EAN-13 the 12 code elements corresponds to
		/// element 2 to 13 of the text characters. The first text
		/// character controls how elements 2 to 5 of the code array are
		/// encoded. Please read the following article for full description.
		/// http://www.barcodeisland.com/ean13.phtml.
		/// </para>
		/// <para>
		/// In this class, odd parity encoding is one code element equals one digit.
		/// </para>
		/// <para>
		/// Even parity is code element equals digit plus 10.
		/// </para>
		/// <para>
		/// The last code element is a checksum. The checksum must be
		/// given however the constructor calculates the checksum and
		/// override the one given. In other words, if you do not
		/// know the checksum just set the last element to 0.
		/// </para>
		///	</remarks>
		////////////////////////////////////////////////////////////////////
		public PdfBarcodeEAN13
				(
				int[] _CodeArray
				)
			{
			// save code array
			this._CodeArray = _CodeArray;

			// test argument
			if(_CodeArray == null || _CodeArray.Length != BARCODE_LEN)
				throw new ApplicationException("Barcode EAN-13/UPC-A: Code array must be exactly 12 characters");

			StringBuilder Str = new StringBuilder();
			int[] ParityTest = new int[5];

			// convert code array to text
			for(int Index = 0; Index < BARCODE_LEN - 1; Index++)
				{
				int Code = _CodeArray[Index];
				if(Code < 0 || Code >= 20 || Code >= 10 && (Index == 0 || Index >= 6))
					throw new ApplicationException("Barcode EAN-13/UPC-A: Invalid code");

				if(Index >= 1 && Index < 6 && Code >= 10) ParityTest[Index - 1] = 10;

				if(Index == 5)
					{
					for(FirstDigit = 0; FirstDigit < 10; FirstDigit++)
						{
						int Scan;
						for(Scan = 0; Scan < 5 && ParityTable[FirstDigit, Scan] == ParityTest[Scan]; Scan++);
						if(Scan == 5) break;
						}
					if(FirstDigit == 10) throw new ApplicationException("Barcode EAN-13/UPC-A: Invalid code");
					if(FirstDigit != 0) Str.Insert(0, (char) ('0' + FirstDigit));
					}

				Str.Append((char) ('0' + (Code % 10)));
				}

			// calculate checksum
			Checksum();

			// add it to text
			Str.Append((char) ('0' + _CodeArray[BARCODE_LEN - 1]));

			// save text
			Text = Str.ToString();

			// set number of bars
			BarCount = BARCODE_LEN * CODE_CHAR_BARS + 2 * LEAD_BARS + SEPARATOR_BARS;

			// set total width
			TotalWidth = BARCODE_LEN * CODE_CHAR_WIDTH + 2 * LEAD_BARS + SEPARATOR_BARS;

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Code EAN-13 checksum calculations
		////////////////////////////////////////////////////////////////////

		private void Checksum()
			{
			// calculate checksum
			int ChkSum = FirstDigit;
			bool Odd = true;
			for(int Index = 0; Index < BARCODE_LEN - 1; Index++)
				{
				ChkSum += (Odd ? 3 : 1) * _CodeArray[Index];
				Odd = !Odd;
				}

			// final checksum
			ChkSum = ChkSum % 10;
			_CodeArray[BARCODE_LEN - 1] = ChkSum == 0 ? 0 : 10 - ChkSum;
			return;
			}
		}
	}
