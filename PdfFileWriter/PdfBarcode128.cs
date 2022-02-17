/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Barcode 128 class
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
	/// Barcode 128 Class
	/// </summary>
	/// <remarks>
	/// This program supports ASCII range of 0 to 127. 
	/// Character range 128 to 255 is not supported.
	/// </remarks>
	/////////////////////////////////////////////////////////////////////
	public class PdfBarcode128 : PdfBarcode
		{
		/// <summary>
		/// Each code128 character is encoded as 3 black bars and 3 white bars.
		/// </summary>
		public const int CODE_CHAR_BARS = 6;

		/// <summary>
		/// Each code128 character width is 11 narrow bars.
		/// </summary>
		public const int CODE_CHAR_WIDTH = 11;

		/// <summary>
		/// Function character FNC1.
		/// </summary>
		public const char FNC1_CHAR = (char) 256;

		/// <summary>
		/// Function character FNC2.
		/// </summary>
		public const char FNC2_CHAR = (char) 257;

		/// <summary>
		/// Function character FNC3.
		/// </summary>
		public const char FNC3_CHAR = (char) 258;

		/// <summary>
		/// Special code FNC1.
		/// </summary>
		public const int FNC1 = 102;

		/// <summary>
		/// Special code FNC2.
		/// </summary>
		public const int FNC2 = 97;

		/// <summary>
		/// Special code FNC3.
		/// </summary>
		public const int FNC3 = 96;

		/// <summary>
		/// Special code SHIFT.
		/// </summary>
		public const int SHIFT = 98;

		/// <summary>
		/// Special code CODEA (or FN4 for code set A).
		/// </summary>
		public const int CODEA = 101;

		/// <summary>
		/// Special code CODEB (or FN4 for code set B).
		/// </summary>
		public const int CODEB = 100;

		/// <summary>
		/// Special code CODEC.
		/// </summary>
		public const int CODEC = 99;

		/// <summary>
		/// Special code STARTA.
		/// </summary>
		public const int STARTA = 103;

		/// <summary>
		/// Special code STARTB.
		/// </summary>
		public const int STARTB = 104;

		/// <summary>
		/// Special code STARTC.
		/// </summary>
		public const int STARTC = 105;

		/// <summary>
		/// Special code STOP.
		/// </summary>
		public const int STOP = 106;

		/// <summary>
		/// Code table for barcode 128
		/// </summary>
		/// <Remarks>
		/// <para>
		/// Barcode 128 consists of 107 codes.
		/// </para>
		/// <para>
		/// Each code is made of 6 bars, three black bars and three white bars.
		/// Each bar is expressed as multiple of the narrow bar.
		/// </para>
		/// <para>
		/// Total width of one bar code is always 11 narrow bar units.
		/// </para>
		/// <para>
		/// After the stop code there is always one more black bar
		/// with width of two units.
		/// </para>
		/// <para>
		/// Each code can have one of three possible meanings
		/// depending on the mode (CODEA, CODEB, CODEC).
		/// </para>
		/// <para>
		/// The CodeTable array dimensions are [107, 6].
		/// </para>
		/// </Remarks>
		public static readonly byte[,] CodeTable =
			{
								//        CODEA   CODEB   CODEC 
			{2, 1, 2, 2, 2, 2},	// 0		SP		SP		0
			{2, 2, 2, 1, 2, 2},	// 1		!		!		1
			{2, 2, 2, 2, 2, 1},	// 2		"		"		2
			{1, 2, 1, 2, 2, 3},	// 3		#		#		3
			{1, 2, 1, 3, 2, 2},	// 4		$		$		4
			{1, 3, 1, 2, 2, 2},	// 5		%		%		5
			{1, 2, 2, 2, 1, 3},	// 6		&		&		6
			{1, 2, 2, 3, 1, 2},	// 7		'		'		7
			{1, 3, 2, 2, 1, 2},	// 8		(		(		8
			{2, 2, 1, 2, 1, 3},	// 9		)		)		9
			{2, 2, 1, 3, 1, 2},	// 10		*		*		10
			{2, 3, 1, 2, 1, 2},	// 11		+		+		11
			{1, 1, 2, 2, 3, 2},	// 12		,		,		12
			{1, 2, 2, 1, 3, 2},	// 13		-		-		13
			{1, 2, 2, 2, 3, 1},	// 14		.		.		14
			{1, 1, 3, 2, 2, 2},	// 15		/		/		15
			{1, 2, 3, 1, 2, 2},	// 16		0		0		16
			{1, 2, 3, 2, 2, 1},	// 17		1		1		17
			{2, 2, 3, 2, 1, 1},	// 18		2		2		18
			{2, 2, 1, 1, 3, 2},	// 19		3		3		19
			{2, 2, 1, 2, 3, 1},	// 20		4		4		20
			{2, 1, 3, 2, 1, 2},	// 21		5		5		21
			{2, 2, 3, 1, 1, 2},	// 22		6		6		22
			{3, 1, 2, 1, 3, 1},	// 23		7		7		23
			{3, 1, 1, 2, 2, 2},	// 24		8		8		24
			{3, 2, 1, 1, 2, 2},	// 25		9		9		25
			{3, 2, 1, 2, 2, 1},	// 26		:		:		26
			{3, 1, 2, 2, 1, 2},	// 27		;		;		27
			{3, 2, 2, 1, 1, 2},	// 28		<		<		28
			{3, 2, 2, 2, 1, 1},	// 29		=		=		29
			{2, 1, 2, 1, 2, 3},	// 30		>		>		30
			{2, 1, 2, 3, 2, 1},	// 31		?		?		31
			{2, 3, 2, 1, 2, 1},	// 32		@		@		32
			{1, 1, 1, 3, 2, 3},	// 33		A		A		33
			{1, 3, 1, 1, 2, 3},	// 34		B		B		34
			{1, 3, 1, 3, 2, 1},	// 35		C		C		35
			{1, 1, 2, 3, 1, 3},	// 36		D		D		36
			{1, 3, 2, 1, 1, 3},	// 37		E		E		37
			{1, 3, 2, 3, 1, 1},	// 38		F		F		38
			{2, 1, 1, 3, 1, 3},	// 39		G		G		39
			{2, 3, 1, 1, 1, 3},	// 40		H		H		40
			{2, 3, 1, 3, 1, 1},	// 41		I		I		41
			{1, 1, 2, 1, 3, 3},	// 42		J		J		42
			{1, 1, 2, 3, 3, 1},	// 43		K		K		43
			{1, 3, 2, 1, 3, 1},	// 44		L		L		44
			{1, 1, 3, 1, 2, 3},	// 45		M		M		45
			{1, 1, 3, 3, 2, 1},	// 46		N		N		46
			{1, 3, 3, 1, 2, 1},	// 47		O		O		47
			{3, 1, 3, 1, 2, 1},	// 48		P		P		48
			{2, 1, 1, 3, 3, 1},	// 49		Q		Q		49
			{2, 3, 1, 1, 3, 1},	// 50		R		R		50
			{2, 1, 3, 1, 1, 3},	// 51		S		S		51
			{2, 1, 3, 3, 1, 1},	// 52		T		T		52
			{2, 1, 3, 1, 3, 1},	// 53		U		U		53
			{3, 1, 1, 1, 2, 3},	// 54		V		V		54
			{3, 1, 1, 3, 2, 1},	// 55		W		W		55
			{3, 3, 1, 1, 2, 1},	// 56		X		X		56
			{3, 1, 2, 1, 1, 3},	// 57		Y		Y		57
			{3, 1, 2, 3, 1, 1},	// 58		Z		Z		58
			{3, 3, 2, 1, 1, 1},	// 59		[		[		59
			{3, 1, 4, 1, 1, 1},	// 60		\		\		60
			{2, 2, 1, 4, 1, 1},	// 61		]		]		61
			{4, 3, 1, 1, 1, 1},	// 62		^		^		62
			{1, 1, 1, 2, 2, 4},	// 63		_		_		63
			{1, 1, 1, 4, 2, 2},	// 64		NUL		`		64
			{1, 2, 1, 1, 2, 4},	// 65		SOH		a		65
			{1, 2, 1, 4, 2, 1},	// 66		STX		b		66
			{1, 4, 1, 1, 2, 2},	// 67		ETX		c		67
			{1, 4, 1, 2, 2, 1},	// 68		EOT		d		68
			{1, 1, 2, 2, 1, 4},	// 69		ENQ		e		69
			{1, 1, 2, 4, 1, 2},	// 70		ACK		f		70
			{1, 2, 2, 1, 1, 4},	// 71		BEL		g		71
			{1, 2, 2, 4, 1, 1},	// 72		BS		h		72
			{1, 4, 2, 1, 1, 2},	// 73		HT		i		73
			{1, 4, 2, 2, 1, 1},	// 74		LF		j		74
			{2, 4, 1, 2, 1, 1},	// 75		VT		k		75
			{2, 2, 1, 1, 1, 4},	// 76		FF		I		76
			{4, 1, 3, 1, 1, 1},	// 77		CR		m		77
			{2, 4, 1, 1, 1, 2},	// 78		SO		n		78
			{1, 3, 4, 1, 1, 1},	// 79		SI		o		79
			{1, 1, 1, 2, 4, 2},	// 80		DLE		p		80
			{1, 2, 1, 1, 4, 2},	// 81		DC1		q		81
			{1, 2, 1, 2, 4, 1},	// 82		DC2		r		82
			{1, 1, 4, 2, 1, 2},	// 83		DC3		s		83
			{1, 2, 4, 1, 1, 2},	// 84		DC4		t		84
			{1, 2, 4, 2, 1, 1},	// 85		NAK		u		85
			{4, 1, 1, 2, 1, 2},	// 86		SYN		v		86
			{4, 2, 1, 1, 1, 2},	// 87		ETB		w		87
			{4, 2, 1, 2, 1, 1},	// 88		CAN		x		88
			{2, 1, 2, 1, 4, 1},	// 89		EM		y		89
			{2, 1, 4, 1, 2, 1},	// 90		SUB		z		90
			{4, 1, 2, 1, 2, 1},	// 91		ESC		{		91
			{1, 1, 1, 1, 4, 3},	// 92		FS		|		92
			{1, 1, 1, 3, 4, 1},	// 93		GS		}		93
			{1, 3, 1, 1, 4, 1},	// 94		RS		~		94
			{1, 1, 4, 1, 1, 3},	// 95		US		DEL		95
			{1, 1, 4, 3, 1, 1},	// 96		FNC 3	FNC 3	96
			{4, 1, 1, 1, 1, 3},	// 97		FNC 2	FNC 2	97
			{4, 1, 1, 3, 1, 1},	// 98		SHIFT	SHIFT	98
			{1, 1, 3, 1, 4, 1},	// 99		CODE C	CODE C	99
			{1, 1, 4, 1, 3, 1},	// 100		CODE B	FNC 4	CODE B
			{3, 1, 1, 1, 4, 1},	// 101		FNC 4	CODE A	CODE A
			{4, 1, 1, 1, 3, 1},	// 102		FNC 1	FNC 1	FNC 1
			{2, 1, 1, 4, 1, 2},	// 103		Start A	Start A	Start A
			{2, 1, 1, 2, 1, 4},	// 104		Start B	Start B	Start B
			{2, 1, 1, 2, 3, 2},	// 105		Start C	Start C	Start C
 			{2, 3, 3, 1, 1, 1},	// 106		Stop	Stop	Stop
			};

		// code set
		private enum CodeSet
			{
			Undefined,
			CodeA,
			CodeB,
			CodeC,
			ShiftA,
			ShiftB,
			};

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Width of one bar at indexed position in narrow bar units.
		/// </summary>
		/// <param name="Index">Bar's index number.</param>
		/// <returns>Bar's width in narrow bar units.</returns>
		/// <remarks>This virtual function must be implemented by derived class 
		/// Index range is 0 to BarCount - 1</remarks>
		/////////////////////////////////////////////////////////////////////
		public override int BarWidth
				(
				int Index
				)
			{
			return Index + 1 < BarCount ? CodeTable[_CodeArray[Index / CODE_CHAR_BARS], Index % CODE_CHAR_BARS] : 2;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Barcode 128 constructor
		/// </summary>
		/// <param name="Text">Input text</param>
		/// <remarks>
		/// <para>
		/// Convert text to code 128.
		/// </para>
		/// <para>>
		/// Valid input characters are ASCII 0 to 127.
		/// </para>
		/// <para>>
		/// In addition three control function codes are available
		/// </para>
		/// <para>>
		///	FNC1_CHAR = (char) 256;
		/// </para>
		/// <para>>
		///	FNC2_CHAR = (char) 257;
		/// </para>
		/// <para>>
		///	FNC3_CHAR = (char) 258;
		/// </para>
		/// <para>>
		/// The constructor will optimize the translation of text to code.
		/// The code array will be divided into segments of
		/// CODEA, CODEB and CODEC
		/// </para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public PdfBarcode128
				(
				string Text
				)
			{
			// test argument
			if(string.IsNullOrEmpty(Text)) throw new ApplicationException("Barcode128: Text is null or empty");

			// save text
			this.Text = Text;

			// text length
			int TextLen = Text.Length;

			// leading FNC1
			int LeadFnc1End;
			for(LeadFnc1End = 0; LeadFnc1End < TextLen && Text[LeadFnc1End] == FNC1_CHAR; LeadFnc1End++);

			// leading digits
			int LeadDigitsEnd;
			for(LeadDigitsEnd = LeadFnc1End; LeadDigitsEnd < TextLen && Text[LeadDigitsEnd] >= '0' && Text[LeadDigitsEnd] <= '9'; LeadDigitsEnd++);

			// lead digits count
			int LeadDigitsCount = LeadDigitsEnd - LeadFnc1End;

			// if leading digits is odd remove the last one
			if((LeadDigitsCount & 1) != 0)
				{
				LeadDigitsEnd--;
				LeadDigitsCount--;
				}

			// trailing FNC1
			int TrailFnc1Start;
			for(TrailFnc1Start = TextLen - 1; TrailFnc1Start >= LeadDigitsEnd && Text[TrailFnc1Start] == FNC1_CHAR; TrailFnc1Start--);
			TrailFnc1Start++;

			// trailing digits
			int TrailDigitsStart;
			for(TrailDigitsStart = TrailFnc1Start - 1; TrailDigitsStart >= LeadDigitsEnd && Text[TrailDigitsStart] >= '0' && Text[TrailDigitsStart] <= '9'; TrailDigitsStart--)
				;
			TrailDigitsStart++;

			// trailing digits count
			int TrailDigitsCount = TrailFnc1Start - TrailDigitsStart;

			// if trailing digits is odd remove the first one
			if((TrailDigitsCount & 1) != 0)
				{
				TrailDigitsStart++;
				TrailDigitsCount--;
				}

			// initialize code array end pointer
			int CodeEnd = 0;

			// test for all digits with or without leading and or trailing FNC1
			if(LeadDigitsEnd == TrailDigitsStart && LeadDigitsCount != 0)
				{
				// create code array
				_CodeArray = new int[1 + LeadFnc1End + (LeadDigitsEnd - LeadFnc1End) / 2 + (TextLen - TrailFnc1Start) + 2];

				// start with code set C
				_CodeArray[CodeEnd] = CodeEnd == 0 ? STARTC : CODEC;
				CodeEnd++;

				// add FNC1 if required
				for(int Index = 0; Index < LeadFnc1End; Index++) _CodeArray[CodeEnd++] = FNC1;

				// convert to pairs of digits
				EncodeDigits(LeadFnc1End, LeadDigitsEnd, ref CodeEnd);

				// add FNC1 if required
				for(int Index = TrailFnc1Start; Index < TextLen; Index++) _CodeArray[CodeEnd++] = FNC1;
				}

			// text has digits and non digits
			else
				{
				// remove leading digits if less than 4
				if(LeadDigitsCount < 4)
					{
					LeadDigitsEnd = 0;
					LeadFnc1End = 0;
					LeadDigitsCount = 0;
					}

				// remove traling digits if less than 4
				if(TrailDigitsCount < 4)
					{
					TrailDigitsStart = TextLen;
					TrailFnc1Start = TextLen;
					TrailDigitsCount = 0;
					}

				// create code array (worst case length)
				_CodeArray = new int[2 * TextLen + 2];

				// lead digits
				if(LeadDigitsCount != 0)
					{
					// start with code set C
					_CodeArray[CodeEnd] = CodeEnd == 0 ? STARTC : CODEC;
					CodeEnd++;

					// add FNC1 if required
					for(int Index = 0; Index < LeadFnc1End; Index++) _CodeArray[CodeEnd++] = FNC1;

					// convert to pairs of digits
					EncodeDigits(LeadFnc1End, LeadDigitsEnd, ref CodeEnd);
					}

				int StartOfNonDigits = LeadDigitsEnd;
				int StartOfDigits = LeadDigitsEnd;
				int EndOfDigits;

				// scan text between end of leading digits to start of trailing digits
				for(;;)
					{
					// look for a digit
					for(; StartOfDigits < TrailDigitsStart && (Text[StartOfDigits] < '0' || Text[StartOfDigits] > '9'); StartOfDigits++);
					EndOfDigits = StartOfDigits;

					// we have at least one
					if(StartOfDigits < TrailDigitsStart)
						{
						// count how many digits we have
						for(EndOfDigits++; EndOfDigits < TrailDigitsStart && Text[EndOfDigits] >= '0' && Text[EndOfDigits] <= '9'; EndOfDigits++);

						// test for odd number of digits
						if(((EndOfDigits - StartOfDigits) & 1) != 0) StartOfDigits++;

						// if we have less than 6 process digits as non digits
						if(EndOfDigits - StartOfDigits < 6)
							{
							StartOfDigits = EndOfDigits;
							continue;
							}
						}

					// process non digits up to StartOfDigits
					EncodeNonDigits(StartOfNonDigits, StartOfDigits, ref CodeEnd);

					// if there are no digits at the end, get out of the loop
					if(StartOfDigits == TrailDigitsStart) break;

					// add code set C
					_CodeArray[CodeEnd] = CodeEnd == 0 ? STARTC : CODEC;
					CodeEnd++;

					// convert to pairs of digits
					EncodeDigits(StartOfDigits, EndOfDigits, ref CodeEnd);

					// adjust start of digits and non digits
					StartOfDigits = EndOfDigits;
					StartOfNonDigits = EndOfDigits;
					}

				// trailing digits
				if(TrailDigitsCount != 0)
					{
					// add code set C
					_CodeArray[CodeEnd++] = CODEC;

					// convert to pairs of digits
					EncodeDigits(TrailDigitsStart, TrailFnc1Start, ref CodeEnd);

					// add trailing FNC1 if required
					for(int Index = TrailFnc1Start; Index < TextLen; Index++) _CodeArray[CodeEnd++] = FNC1;
					}

				// adjust code array to right length
				Array.Resize<int>(ref _CodeArray, CodeEnd + 2);
				}

			// checksum and STOP
			Checksum();

			// set number of bars for enumeration
			BarCount = CODE_CHAR_BARS * _CodeArray.Length + 1;

			// save total width
			TotalWidth = CODE_CHAR_WIDTH * _CodeArray.Length + 2;

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Barcode 128 constructor
		/// </summary>
		/// <param name="_CodeArray">Code array</param>
		/// <remarks>
		/// <para>
		/// Set Code Array and convert it to text.
		/// </para>
		/// <para>
		/// Each code must be 0 to 106.
		/// </para>
		/// <para>
		/// The first code must be 103, 104 or 105.
		/// </para>
		/// <para>
		/// The stop code 106 if present must be the last code.
		/// </para>
		/// <para>
		/// If the last code is not 106, the method calculates the checksum
		/// and appends the checksum and the stop character to the end of the array.
		/// </para>
		/// <para>
		/// If the stop code is missing you must not have a checksum.
		/// If the last code is 106, the method recalculates the checksum
		/// and replaces the existing checksum.
		/// </para>
		/// <para>
		/// The text output is made of ASCII characters 0 to 127 and
		/// three function characters 256, 257 and 258.
		/// </para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public PdfBarcode128
				(
				int[] _CodeArray
				)
			{
			// save code array
			this._CodeArray = _CodeArray;

			// test argument
			if(_CodeArray == null || _CodeArray.Length < 2)
				throw new ApplicationException("Barcode128: Code array is null or empty");

			// code array length
			int Length = _CodeArray.Length;

			// if last element is not stop, add two more codes
			if(_CodeArray[Length - 1] != STOP)
				{
				// add two elements to the array
				Length += 2;
				Array.Resize<int>(ref _CodeArray, Length);
				}

			// checksum (we ignore user supplied checksum and override it with our own)
			// and add STOP at the end
			Checksum();

			// set number of bars
			BarCount = CODE_CHAR_BARS * Length + 1;

			// save total width
			TotalWidth = CODE_CHAR_WIDTH * Length + 2;

			// convert code array to text
			StringBuilder Str = new StringBuilder();

			// conversion state
			CodeSet CodeSet;

			// start code
			switch(_CodeArray[0])
				{
				case STARTA:
					CodeSet = CodeSet.CodeA;
					break;

				case STARTB:
					CodeSet = CodeSet.CodeB;
					break;

				case STARTC:
					CodeSet = CodeSet.CodeC;
					break;

				default:
					// first code must be FNC1, FNC2 or FNC3
					throw new ApplicationException("Barcode128: Code array first element must be start code (103, 104, 105)");
				}

			// loop for all characters except for start, checksum and stop
			int End = Length - 2;
			for(int Index = 1; Index < End; Index++)
				{
				int Code = _CodeArray[Index];
				if(Code < 0 || Code > FNC1) throw new ApplicationException("Barcode128: Code array has invalid codes (not 0 to 106)");
				switch(CodeSet)
					{
					case CodeSet.CodeA:
						if(Code == CODEA) throw new ApplicationException("Barcode128: No support for FNC4");
						else if(Code == CODEB) CodeSet = CodeSet.CodeB;
						else if(Code == CODEC) CodeSet = CodeSet.CodeC;
						else if(Code == SHIFT) CodeSet = CodeSet.ShiftB;
						else if(Code == FNC1) Str.Append(FNC1_CHAR);
						else if(Code == FNC2) Str.Append(FNC2_CHAR);
						else if(Code == FNC3) Str.Append(FNC3_CHAR);
						else if(Code < 64) Str.Append((char) (' ' + Code));
						else Str.Append((char) (Code - 64));
						break;

					case CodeSet.CodeB:
						if(Code == CODEA) CodeSet = CodeSet.CodeA;
						else if(Code == CODEB) throw new ApplicationException("Barcode128: No support for FNC4");
						else if(Code == CODEC) CodeSet = CodeSet.CodeC;
						else if(Code == SHIFT) CodeSet = CodeSet.ShiftB;
						else if(Code == FNC1) Str.Append(FNC1_CHAR);
						else if(Code == FNC2) Str.Append(FNC2_CHAR);
						else if(Code == FNC3) Str.Append(FNC3_CHAR);
						else Str.Append((char) (' ' + Code));
						break;

					case CodeSet.ShiftA:
						if(Code < 64) Str.Append((char) (' ' + Code));
						else if(Code < 96) Str.Append((char) (Code - 64));
						else throw new ApplicationException("Barcode128: SHIFT error");
						CodeSet = CodeSet.CodeB;
						break;

					case CodeSet.ShiftB:
						if(Code < 96) Str.Append((char) (' ' + Code));
						else throw new ApplicationException("Barcode128: SHIFT error");
						CodeSet = CodeSet.CodeA;
						break;

					case CodeSet.CodeC:
						if(Code == CODEA) CodeSet = CodeSet.CodeA;
						else if(Code == CODEB) CodeSet = CodeSet.CodeB;
						else if(Code == FNC1) Str.Append(FNC1_CHAR);
						else
							{
							Str.Append((char) ('0' + Code / 10));
							Str.Append((char) ('0' + Code % 10));
							}
						break;
					}

				}

			// save text
			Text = Str.ToString();

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Process block of digits
		////////////////////////////////////////////////////////////////////
		private void EncodeDigits
				(
				int TextStart,
				int TextEnd,
				ref int CodeEnd
				)
			{
			// convert to pairs of digits
			for(int Index = TextStart; Index < TextEnd; Index += 2)
				_CodeArray[CodeEnd++] = 10 * (Text[Index] - '0') + (Text[Index + 1] - '0');
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Process block of non-digits
		////////////////////////////////////////////////////////////////////
		private void EncodeNonDigits
				(
				int TextStart,
				int TextEnd,
				ref int CodeEnd
				)
			{
			// assume code set B
			int CodeSeg = CodeEnd;
			_CodeArray[CodeEnd++] = CodeSeg == 0 ? STARTB : CODEB;
			CodeSet CurCodeSet = CodeSet.Undefined;

			for(int Index = TextStart; Index < TextEnd; Index++)
				{
				// get char
				int CurChar = Text[Index];

				// currect character is part of code set A
				if(CurChar < 32)
					{
					switch(CurCodeSet)
						{
						// current segment is undefined
						// all characters up to this point are 32 to 95 eigther A or B
						case CodeSet.Undefined:
							// change first segemnt to be code set A
							_CodeArray[CodeSeg] = CodeSeg == 0 ? STARTA : CODEA;
							CurCodeSet = CodeSet.CodeA;
							break;

						// currect segment is code B
						case CodeSet.CodeB:
							// save current location as start of new segment
							CodeSeg = CodeEnd;

							// one time shift to A
							_CodeArray[CodeEnd++] = SHIFT;
							CurCodeSet = CodeSet.ShiftA;
							break;

						// currect segment is Code B with one time shift to A
						case CodeSet.ShiftA:
							// convert the last shift A to code A
							_CodeArray[CodeSeg] = CODEA;
							CurCodeSet = CodeSet.CodeA;
							break;

						// currect segment is Code A with one time shift to B
						case CodeSet.ShiftB:
							// disable the Shift B. this is a code A segment with one shift B
							CurCodeSet = CodeSet.CodeA;
							break;
						}

					// save character
					_CodeArray[CodeEnd++] = CurChar + 64;
					continue;
					}

				// current character is part of either code set A or code set B
				if(CurChar < 96)
					{
					_CodeArray[CodeEnd++] = CurChar - ' ';
					continue;
					}

				// currect character is part of code set B
				if(CurChar < 128)
					{
					switch(CurCodeSet)
						{
						// current segment is undefined
						// all characters up to this point are 32 to 95 eigther A or B
						case CodeSet.Undefined:
							// make first segemnt to be code set B
							CurCodeSet = CodeSet.CodeB;
							break;

						// currect segment is code A
						case CodeSet.CodeA:
							// save current location as start of new segment
							CodeSeg = CodeEnd;

							// one time shift to B
							_CodeArray[CodeEnd++] = SHIFT;
							CurCodeSet = CodeSet.ShiftB;
							break;

						// currect segment is Code B with one time shift to A
						case CodeSet.ShiftA:
							// disable the ShiftA. this is a code B segment with one shift A
							CurCodeSet = CodeSet.CodeB;
							break;

						// currect segment is Code A with one time shift to B
						case CodeSet.ShiftB:
							// convert the last shift B to code B
							_CodeArray[CodeSeg] = CODEB;
							CurCodeSet = CodeSet.CodeB;
							break;
						}

					// save character
					_CodeArray[CodeEnd++] = CurChar - ' ';
					continue;
					}

				// function code
				if(CurChar >= FNC1_CHAR && CurChar <= FNC3_CHAR)
					{
					_CodeArray[CodeEnd++] = CurChar == FNC1_CHAR ? FNC1 : (CurChar == FNC2_CHAR ? FNC2 : FNC3);
					continue;
					}

				// invalid character
				throw new ApplicationException("FormaCode128 input characters must be 0 to 127 or function code (256, 257, 258)");
				}
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Code 128 checksum calculations
		// The method stores the checksum and STOP character
		////////////////////////////////////////////////////////////////////
		private void Checksum()
			{
			// calculate checksum
			int Length = _CodeArray.Length - 2;
			int ChkSum = _CodeArray[0];
			for(int Index = 1; Index < Length; Index++) ChkSum += Index * _CodeArray[Index];

			// final checksum
			_CodeArray[Length] = ChkSum % 103;

			// stop code
			_CodeArray[Length + 1] = STOP;
			return;
			}
		}
	}
