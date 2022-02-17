/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Barcode 39 class
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
	/// Barcode 39 class
	/// </summary>
	/////////////////////////////////////////////////////////////////////
	public class PdfBarcode39 : PdfBarcode
		{
		/// <summary>
		/// Each code39 code is encoded as 5 black bars and 5 white bars.
		/// </summary>
		public const int CODE_CHAR_BARS = 10;

		/// <summary>
		/// Total length expressed in narrow bar units.
		/// </summary>
		public const int CODE_CHAR_WIDTH = 16;

		/// <summary>
		/// Barcode39 start and stop character (normally displayed as *).
		/// </summary>
		public const int START_STOP_CODE = 43;

		/// <summary>
		/// Barcode39 supported characters.
		/// </summary>
		public const string CharSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%*";

		/// <summary>
		/// Code table for barcode 39
		/// </summary>
		/// <remarks>Array size [44, 10]</remarks>
		public static readonly byte[,] CodeTable =
			{
			{1, 1, 1, 3, 3, 1, 3, 1, 1, 1},		// 0  0
			{3, 1, 1, 3, 1, 1, 1, 1, 3, 1},		// 1  1
			{1, 1, 3, 3, 1, 1, 1, 1, 3, 1},		// 2  2
			{3, 1, 3, 3, 1, 1, 1, 1, 1, 1},		// 3  3
			{1, 1, 1, 3, 3, 1, 1, 1, 3, 1},		// 4  4
			{3, 1, 1, 3, 3, 1, 1, 1, 1, 1},		// 5  5
			{1, 1, 3, 3, 3, 1, 1, 1, 1, 1},		// 6  6
			{1, 1, 1, 3, 1, 1, 3, 1, 3, 1},		// 7  7
			{3, 1, 1, 3, 1, 1, 3, 1, 1, 1},		// 8  8
			{1, 1, 3, 3, 1, 1, 3, 1, 1, 1},		// 9  9
			{3, 1, 1, 1, 1, 3, 1, 1, 3, 1},		// 10 A
			{1, 1, 3, 1, 1, 3, 1, 1, 3, 1},		// 11 B
			{3, 1, 3, 1, 1, 3, 1, 1, 1, 1},		// 12 C
			{1, 1, 1, 1, 3, 3, 1, 1, 3, 1},		// 13 D
			{3, 1, 1, 1, 3, 3, 1, 1, 1, 1},		// 14 E
			{1, 1, 3, 1, 3, 3, 1, 1, 1, 1},		// 15 F
			{1, 1, 1, 1, 1, 3, 3, 1, 3, 1},		// 16 G
			{3, 1, 1, 1, 1, 3, 3, 1, 1, 1},		// 17 H
			{1, 1, 3, 1, 1, 3, 3, 1, 1, 1},		// 18 I
			{1, 1, 1, 1, 3, 3, 3, 1, 1, 1},		// 19 J
			{3, 1, 1, 1, 1, 1, 1, 3, 3, 1},		// 20 K
			{1, 1, 3, 1, 1, 1, 1, 3, 3, 1},		// 21 L
			{3, 1, 3, 1, 1, 1, 1, 3, 1, 1},		// 22 M
			{1, 1, 1, 1, 3, 1, 1, 3, 3, 1},		// 23 N
			{3, 1, 1, 1, 3, 1, 1, 3, 1, 1},		// 24 O
			{1, 1, 3, 1, 3, 1, 1, 3, 1, 1},		// 25 P
			{1, 1, 1, 1, 1, 1, 3, 3, 3, 1},		// 26 Q
			{3, 1, 1, 1, 1, 1, 3, 3, 1, 1},		// 27 R
			{1, 1, 3, 1, 1, 1, 3, 3, 1, 1},		// 28 S
			{1, 1, 1, 1, 3, 1, 3, 3, 1, 1},		// 29 T
			{3, 3, 1, 1, 1, 1, 1, 1, 3, 1},		// 30 U
			{1, 3, 3, 1, 1, 1, 1, 1, 3, 1},		// 31 V
			{3, 3, 3, 1, 1, 1, 1, 1, 1, 1},		// 32 W
			{1, 3, 1, 1, 3, 1, 1, 1, 3, 1},		// 33 X
			{3, 3, 1, 1, 3, 1, 1, 1, 1, 1},		// 34 Y
			{1, 3, 3, 1, 3, 1, 1, 1, 1, 1},		// 35 Z
			{1, 3, 1, 1, 1, 1, 3, 1, 3, 1},		// 36 -
			{3, 3, 1, 1, 1, 1, 3, 1, 1, 1},		// 37 .
			{1, 3, 3, 1, 1, 1, 3, 1, 1, 1},		// 38 (space)
			{1, 3, 1, 3, 1, 3, 1, 1, 1, 1},		// 39 $
			{1, 3, 1, 3, 1, 1, 1, 3, 1, 1},		// 40 /
			{1, 3, 1, 1, 1, 3, 1, 3, 1, 1},		// 41 +
			{1, 1, 1, 3, 1, 3, 1, 3, 1, 1},		// 42 %
			{1, 3, 1, 1, 3, 1, 3, 1, 1, 1},		// 43 *
			};

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Bar width as function of position in the barcode 39 
		/// </summary>
		/// <param name="Index">Array index.</param>
		/// <returns>Width of one bar</returns>
		////////////////////////////////////////////////////////////////////
		public override int BarWidth
				(
				int Index
				)
			{
			return CodeTable[_CodeArray[Index / CODE_CHAR_BARS], Index % CODE_CHAR_BARS];
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Barcode 39 constructor
		/// </summary>
		/// <param name="Text">Barcode text</param>
		/// <remarks>
		/// <para>
		/// The constructor converts the text into code.
		/// </para>
		/// <para>
		/// Valid characters are:
		/// </para>
		/// <list type="table">
		/// <item><description>Digits 0 to 9</description></item>
		/// <item><description>Capital Letters A to Z</description></item>
		/// <item><description>Dash '-'</description></item>
		/// <item><description>Period '.'</description></item>
		/// <item><description>Space ' '</description></item>
		/// <item><description>Dollar '$'</description></item>
		/// <item><description>Slash '/'</description></item>
		/// <item><description>Plus '+'</description></item>
		/// <item><description>Percent '%'</description></item>
		/// <item><description>Asterisk '*' (This is the start and stop
		///	character. It cannot be in the middle of the text).</description></item>
		/// </list>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public PdfBarcode39
				(
				string Text
				)
			{
			// test argument
			if(string.IsNullOrEmpty(Text)) throw new ApplicationException("Barcode39: Text cannot be null or empty");

			// save text
			this.Text = Text;

			// barcode array
			_CodeArray = new int[Text.Length + 2];

			// put * at the begining
			int CodePtr = 0;
			_CodeArray[CodePtr++] = START_STOP_CODE;

			// encode the text
			for(int Index = 0; Index < Text.Length; Index++)
				{
				int Code = CharSet.IndexOf(Text[Index]);
				if(Code == START_STOP_CODE)
					{
					if(Index == 0 || Index == Text.Length - 1) continue;
					throw new ApplicationException("Barcode39: Start/Stop character (asterisk *) is not allowed in the middle of the text");
					}
				if(Code < 0) throw new ApplicationException("Barcode39: Invalid character");
				_CodeArray[CodePtr++] = Code;
				}

			// put * at the end
			_CodeArray[CodePtr] = START_STOP_CODE;

			// set number of bars for enumeration
			BarCount = CODE_CHAR_BARS * _CodeArray.Length - 1;

			// set total width
			TotalWidth = CODE_CHAR_WIDTH * _CodeArray.Length - 1;

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Barcode 39 constructor
		/// </summary>
		/// <param name="_CodeArray">Code array</param>
		/// <remarks>
		/// <para>
		/// Sets code array and converts to equivalent text.
		/// </para>
		/// <para>
		/// If the code array is missing the start and/or stop characters,
		/// the constructor will add them.
		/// </para>
		/// <para>
		/// Valid codes are:
		/// </para>
		/// <list type="table">
		/// <item><term>0 to 9</term><description>Digits 0 to 9</description></item>
		/// <item><term>10 to 35</term><description>Capital Letters A to Z</description></item>
		/// <item><term>36</term><description>Dash '-'</description></item>
		/// <item><term>37</term><description>Period '.'</description></item>
		/// <item><term>38</term><description>Space ' '</description></item>
		/// <item><term>39</term><description>Dollar '$'</description></item>
		/// <item><term>40</term><description>Slash '/'</description></item>
		/// <item><term>41</term><description>Plus '+'</description></item>
		/// <item><term>42</term><description>Percent '%'</description></item>
		/// <item><term>43</term><description>Asterisk '*' (This is the start and stop
		///	character. It cannot be in the middle of the text)</description></item>
		/// </list>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public PdfBarcode39
				(
				int[] _CodeArray
				)
			{
			// save code array
			this._CodeArray = _CodeArray;

			// test argument
			if(_CodeArray == null || _CodeArray.Length == 0)
				throw new ApplicationException("Barcode39: Code array is null or empty");

			// test for start code
			if(_CodeArray[0] != START_STOP_CODE)
				{
				int[] TempArray = new int[_CodeArray.Length + 1];
				TempArray[0] = START_STOP_CODE;
				Array.Copy(_CodeArray, 0, TempArray, 1, _CodeArray.Length);
				_CodeArray = TempArray;
				}

			// test for stop code
			if(_CodeArray[_CodeArray.Length - 1] != START_STOP_CODE)
				{
				Array.Resize<int>(ref _CodeArray, _CodeArray.Length + 1);
				_CodeArray[^1] = START_STOP_CODE;
				}

			// set number of bars
			BarCount = CODE_CHAR_BARS * _CodeArray.Length - 1;

			// set total width
			TotalWidth = CODE_CHAR_WIDTH * _CodeArray.Length - 1;

			// convert code array to text without start or stop characters
			StringBuilder Str = new StringBuilder();
			for(int Index = 1; Index < _CodeArray.Length - 2; Index++)
				{
				int Code = _CodeArray[Index];
				if(Code < 0 || Code >= START_STOP_CODE)
					throw new ApplicationException("Barcode39: Code array contains invalid code (0 to 42)");
				Str.Append(CharSet[Code]);
				}

			// convert str to text
			Text = Str.ToString();

			// exit
			return;
			}
		}
	}
