/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	FontApi
//	Support for Windows API functions related to fonts and glyphs.
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

using System.Runtime.InteropServices;
using System.Text;

#pragma warning disable IDE1006

namespace PdfFileWriter
	{
	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// One character/Glyph information class
	/// </summary>
	/// <remarks>
	/// This class defines all the information required to display a
	/// character in the output document. Each character has an
	/// associated glyph. The glyph geometry is defined in a square.
	/// The square is DesignHeight by DesignHeight.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public class CharInfo : IComparable<CharInfo>
		{
		/// <summary>
		/// Character code
		/// </summary>
		public int CharCode { get; internal set; }

		/// <summary>
		/// Glyph index
		/// </summary>
		public int GlyphIndex { get; internal set; }

		/// <summary>
		/// Active character
		/// </summary>
		public bool ActiveChar { get; internal set; }

		/// <summary>
		/// Character code is greater than 255
		/// </summary>
		public bool Type0Font { get; internal set; }

		/// <summary>
		/// Bounding box left in design units
		/// </summary>
		public int DesignBBoxLeft { get; internal set; }

		/// <summary>
		/// Bounding box bottom in design units
		/// </summary>
		public int DesignBBoxBottom { get; internal set; }

		/// <summary>
		/// Bounding box right in design units
		/// </summary>
		public int DesignBBoxRight { get; internal set; }

		/// <summary>
		/// Bounding box top in design units
		/// </summary>
		public int DesignBBoxTop { get; internal set; }

		/// <summary>
		/// Character width in design units
		/// </summary>
		public int DesignWidth { get; internal set; }

		internal int NewGlyphIndex;
		internal byte[] GlyphData;
		internal bool Composite;

		////////////////////////////////////////////////////////////////////
		// constructor
		////////////////////////////////////////////////////////////////////
		internal CharInfo
				(
				int CharCode,
				int GlyphIndex,
				PdfFontApi DC
				)
			{
			// save char code and glyph index
			this.CharCode = CharCode;
			this.GlyphIndex = GlyphIndex;
			this.NewGlyphIndex = -1;
			Type0Font = CharCode >= 256 || GlyphIndex == 0;

			// Bounding Box
			int BBoxWidth = DC.ReadInt32();
			int BBoxHeight = DC.ReadInt32();
			DesignBBoxLeft = DC.ReadInt32();
			DesignBBoxTop = DC.ReadInt32();
			DesignBBoxRight = DesignBBoxLeft + BBoxWidth;
			DesignBBoxBottom = DesignBBoxTop - BBoxHeight;

			// glyph advance horizontal and vertical
			DesignWidth = DC.ReadInt16();
			//DesignHeight = DC.ReadInt16();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// constructor for search and sort
		////////////////////////////////////////////////////////////////////
		internal CharInfo
				(
				int GlyphIndex
				)
			{
			// save char code and glyph index
			this.GlyphIndex = GlyphIndex;
			return;
			}

		/// <summary>
		/// Compare two glyphs for sort and binary search
		/// </summary>
		/// <param name="Other">Other CharInfo</param>
		/// <returns>Compare result</returns>
		public int CompareTo
				(
				CharInfo Other
				)
			{
			return GlyphIndex - Other.GlyphIndex;
			}
		}

	////////////////////////////////////////////////////////////////////
	// IComparer class for new glyph index sort
	////////////////////////////////////////////////////////////////////
	internal class SortByNewIndex : IComparer<CharInfo>
		{
		public int Compare
				(
				CharInfo CharOne,
				CharInfo CharTwo
				)
			{
			return CharOne.NewGlyphIndex - CharTwo.NewGlyphIndex;
			}
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Font box class
	/// </summary>
	/// <remarks>
	/// FontBox class is part of OUTLINETEXTMETRIC structure
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public class FontBox
		{
		/// <summary>
		/// Gets left side.
		/// </summary>
		public int Left { get; private set; }

		/// <summary>
		/// Gets top side.
		/// </summary>
		public int Top { get; private set; }

		/// <summary>
		/// Gets right side.
		/// </summary>
		public int Right { get; private set; }

		/// <summary>
		/// Gets bottom side.
		/// </summary>
		public int Bottom { get; private set; }

		internal FontBox
				(
				PdfFontApi DC
				)
			{
			Left = DC.ReadInt32();
			Top = DC.ReadInt32();
			Right = DC.ReadInt32();
			Bottom = DC.ReadInt32();
			return;
			}
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Panose class
	/// </summary>
	/// <remarks>
	/// The PANOSE structure describes the PANOSE font-classification
	/// values for a TrueType font. These characteristics are then
	/// used to associate the font with other fonts of similar
	/// appearance but different names.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public class WinPanose
		{
		/// <summary>
		/// Panose family type
		/// </summary>
		public byte bFamilyType { get; private set; }

		/// <summary>
		/// Panose serif style
		/// </summary>
		public byte bSerifStyle { get; private set; }

		/// <summary>
		/// Panose weight
		/// </summary>
		public byte bWeight { get; private set; }

		/// <summary>
		/// Panose proportion
		/// </summary>
		public byte bProportion { get; private set; }

		/// <summary>
		/// Panose contrast
		/// </summary>
		public byte bContrast { get; private set; }

		/// <summary>
		/// Panose stroke variation
		/// </summary>
		public byte bStrokeVariation { get; private set; }

		/// <summary>
		/// Panose arm style
		/// </summary>
		public byte bArmStyle { get; private set; }

		/// <summary>
		/// Panose letter form
		/// </summary>
		public byte bLetterform { get; private set; }

		/// <summary>
		/// Panose mid line
		/// </summary>
		public byte bMidline { get; private set; }

		/// <summary>
		/// Panose X height
		/// </summary>
		public byte bXHeight { get; private set; }

		internal WinPanose
				(
				PdfFontApi DC
				)
			{
			bFamilyType = DC.ReadByte();
			bSerifStyle = DC.ReadByte();
			bWeight = DC.ReadByte();
			bProportion = DC.ReadByte();
			bContrast = DC.ReadByte();
			bStrokeVariation = DC.ReadByte();
			bArmStyle = DC.ReadByte();
			bLetterform = DC.ReadByte();
			bMidline = DC.ReadByte();
			bXHeight = DC.ReadByte();
			return;
			}
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Kerning pair class
	/// </summary>
	////////////////////////////////////////////////////////////////////
	public class WinKerningPair : IComparable<WinKerningPair>
		{
		/// <summary>
		/// Gets first character
		/// </summary>
		public char First { get; private set; }

		/// <summary>
		/// Gets second character
		/// </summary>
		public char Second { get; private set; }

		/// <summary>
		/// Gets kerning amount in design units
		/// </summary>
		public int KernAmount { get; private set; }

		internal WinKerningPair
				(
				PdfFontApi DC
				)
			{
			First = DC.ReadChar();
			Second = DC.ReadChar();
			KernAmount = DC.ReadInt32();
			return;
			}

		/// <summary>
		/// Kerning pair constructor
		/// </summary>
		/// <param name="First">First character</param>
		/// <param name="Second">Second character</param>
		public WinKerningPair
				(
				char First,
				char Second
				)
			{
			this.First = First;
			this.Second = Second;
			return;
			}

		/// <summary>
		/// Compare kerning pairs
		/// </summary>
		/// <param name="Other">Other pair</param>
		/// <returns>Compare result</returns>
		public int CompareTo
				(
				WinKerningPair Other
				)
			{
			return First != Other.First ? First - Other.First : Second - Other.Second;
			}
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// TextMetric class
	/// </summary>
	/// <remarks>
	/// The TEXTMETRIC structure contains basic information about a
	/// physical font. All sizes are specified in logical units;
	/// that is, they depend on the current mapping mode of the
	/// display context.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public class WinTextMetric
		{
		/// <summary>
		/// TextMetric height
		/// </summary>
		public int tmHeight { get; private set; }

		/// <summary>
		/// TextMetric ascent
		/// </summary>
		public int tmAscent { get; private set; }

		/// <summary>
		/// TextMetric descent
		/// </summary>
		public int tmDescent { get; private set; }

		/// <summary>
		/// TextMetric internal leading
		/// </summary>
		public int tmInternalLeading { get; private set; }

		/// <summary>
		/// TextMetric external leading
		/// </summary>
		public int tmExternalLeading { get; private set; }

		/// <summary>
		/// TextMetric average character width
		/// </summary>
		public int tmAveCharWidth { get; private set; }

		/// <summary>
		/// TextMetric maximum character width
		/// </summary>
		public int tmMaxCharWidth { get; private set; }

		/// <summary>
		/// TextMetric height
		/// </summary>
		public int tmWeight { get; private set; }

		/// <summary>
		/// TextMetric overhang
		/// </summary>
		public int tmOverhang { get; private set; }

		/// <summary>
		/// TextMetric digitize aspect X
		/// </summary>
		public int tmDigitizedAspectX { get; private set; }

		/// <summary>
		/// TextMetric digitize aspect Y
		/// </summary>
		public int tmDigitizedAspectY { get; private set; }

		/// <summary>
		/// TextMetric first character
		/// </summary>
		public ushort tmFirstChar { get; private set; }

		/// <summary>
		/// TextMetric last character
		/// </summary>
		public ushort tmLastChar { get; private set; }

		/// <summary>
		/// TextMetric default character
		/// </summary>
		public ushort tmDefaultChar { get; private set; }

		/// <summary>
		/// TextMetric break character
		/// </summary>
		public ushort tmBreakChar { get; private set; }

		/// <summary>
		/// TextMetric italic
		/// </summary>
		public byte tmItalic { get; private set; }

		/// <summary>
		/// TextMetric underlined
		/// </summary>
		public byte tmUnderlined { get; private set; }

		/// <summary>
		/// TextMetric struck out
		/// </summary>
		public byte tmStruckOut { get; private set; }

		/// <summary>
		/// TextMetric pitch and family
		/// </summary>
		public byte tmPitchAndFamily { get; private set; }

		/// <summary>
		/// TextMetric character set
		/// </summary>
		public byte tmCharSet { get; private set; }

		internal WinTextMetric
				(
				PdfFontApi DC
				)
			{
			tmHeight = DC.ReadInt32();
			tmAscent = DC.ReadInt32();
			tmDescent = DC.ReadInt32();
			tmInternalLeading = DC.ReadInt32();
			tmExternalLeading = DC.ReadInt32();
			tmAveCharWidth = DC.ReadInt32();
			tmMaxCharWidth = DC.ReadInt32();
			tmWeight = DC.ReadInt32();
			tmOverhang = DC.ReadInt32();
			tmDigitizedAspectX = DC.ReadInt32();
			tmDigitizedAspectY = DC.ReadInt32();
			tmFirstChar = DC.ReadUInt16();
			tmLastChar = DC.ReadUInt16();
			tmDefaultChar = DC.ReadUInt16();
			tmBreakChar = DC.ReadUInt16();
			tmItalic = DC.ReadByte();
			tmUnderlined = DC.ReadByte();
			tmStruckOut = DC.ReadByte();
			tmPitchAndFamily = DC.ReadByte();
			tmCharSet = DC.ReadByte();
			return;
			}
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Outline text metric class
	/// </summary>
	/// <remarks>
	/// The OUTLINETEXTMETRIC structure contains metrics describing
	/// a TrueType font.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public class WinOutlineTextMetric
		{
		/// <summary>
		/// Outline text metric size
		/// </summary>
		public uint otmSize { get; private set; }

		/// <summary>
		/// Outline text metric TextMetric
		/// </summary>
		public WinTextMetric otmTextMetric { get; private set; }

		/// <summary>
		/// Outline text metric panose number
		/// </summary>
		public WinPanose otmPanoseNumber { get; private set; }

		/// <summary>
		/// Outline text metric FS selection
		/// </summary>
		public uint otmfsSelection { get; private set; }

		/// <summary>
		/// Outline text metric FS type
		/// </summary>
		public uint otmfsType { get; private set; }

		/// <summary>
		/// Outline text metric char slope rise
		/// </summary>
		public int otmsCharSlopeRise { get; private set; }

		/// <summary>
		/// Outline text metric char slope run
		/// </summary>
		public int otmsCharSlopeRun { get; private set; }

		/// <summary>
		/// Outline text metric italic angle
		/// </summary>
		public int otmItalicAngle { get; private set; }

		/// <summary>
		/// Outline text metric EM square
		/// </summary>
		public uint otmEMSquare { get; private set; }

		/// <summary>
		/// Outline text metric ascent
		/// </summary>
		public int otmAscent { get; private set; }

		/// <summary>
		/// Outline text metric descent
		/// </summary>
		public int otmDescent { get; private set; }

		/// <summary>
		/// Outline text metric line gap
		/// </summary>
		public uint otmLineGap { get; private set; }

		/// <summary>
		/// Outline text metric capital M height
		/// </summary>
		public uint otmsCapEmHeight { get; private set; }

		/// <summary>
		/// Outline text metric X height
		/// </summary>
		public uint otmsXHeight { get; private set; }

		/// <summary>
		/// Outline text metric Font box class
		/// </summary>
		public FontBox otmrcFontBox { get; private set; }

		/// <summary>
		/// Outline text metric Mac ascent
		/// </summary>
		public int otmMacAscent { get; private set; }

		/// <summary>
		/// Outline text metric Mac descent
		/// </summary>
		public int otmMacDescent { get; private set; }

		/// <summary>
		/// Outline text metric Mac line gap
		/// </summary>
		public uint otmMacLineGap { get; private set; }

		/// <summary>
		/// Outline text metric minimum PPEM
		/// </summary>
		public uint otmusMinimumPPEM { get; private set; }

		/// <summary>
		/// Outline text metric subscript size
		/// </summary>
		public Point otmptSubscriptSize { get; private set; }

		/// <summary>
		/// Outline text metric subscript offset
		/// </summary>
		public Point otmptSubscriptOffset { get; private set; }

		/// <summary>
		/// Outline text metric superscript size
		/// </summary>
		public Point otmptSuperscriptSize { get; private set; }

		/// <summary>
		/// Outline text metric superscript offset
		/// </summary>
		public Point otmptSuperscriptOffset { get; private set; }

		/// <summary>
		/// Outline text metric strikeout size
		/// </summary>
		public uint otmsStrikeoutSize { get; private set; }

		/// <summary>
		/// Outline text metric strikeout position
		/// </summary>
		public int otmsStrikeoutPosition { get; private set; }

		/// <summary>
		/// Outline text metric underscore size
		/// </summary>
		public int otmsUnderscoreSize { get; private set; }

		/// <summary>
		/// Outline text metric underscore position
		/// </summary>
		public int otmsUnderscorePosition { get; private set; }

		/// <summary>
		/// Outline text metric family name
		/// </summary>
		public string otmpFamilyName { get; private set; }

		/// <summary>
		/// Outline text metric face name
		/// </summary>
		public string otmpFaceName { get; private set; }

		/// <summary>
		/// Outline text metric style name
		/// </summary>
		public string otmpStyleName { get; private set; }

		/// <summary>
		/// Outline text metric full name
		/// </summary>
		public string otmpFullName { get; private set; }

		internal WinOutlineTextMetric
				(
				PdfFontApi DC
				)
			{
			otmSize = DC.ReadUInt32();
			otmTextMetric = new WinTextMetric(DC);
			DC.Align4();
			otmPanoseNumber = new WinPanose(DC);
			DC.Align4();
			otmfsSelection = DC.ReadUInt32();
			otmfsType = DC.ReadUInt32();
			otmsCharSlopeRise = DC.ReadInt32();
			otmsCharSlopeRun = DC.ReadInt32();
			otmItalicAngle = DC.ReadInt32();
			otmEMSquare = DC.ReadUInt32();
			otmAscent = DC.ReadInt32();
			otmDescent = DC.ReadInt32();
			otmLineGap = DC.ReadUInt32();
			otmsCapEmHeight = DC.ReadUInt32();
			otmsXHeight = DC.ReadUInt32();
			otmrcFontBox = new FontBox(DC);
			otmMacAscent = DC.ReadInt32();
			otmMacDescent = DC.ReadInt32();
			otmMacLineGap = DC.ReadUInt32();
			otmusMinimumPPEM = DC.ReadUInt32();
			otmptSubscriptSize = DC.ReadWinPoint();
			otmptSubscriptOffset = DC.ReadWinPoint();
			otmptSuperscriptSize = DC.ReadWinPoint();
			otmptSuperscriptOffset = DC.ReadWinPoint();
			otmsStrikeoutSize = DC.ReadUInt32();
			otmsStrikeoutPosition = DC.ReadInt32();
			otmsUnderscoreSize = DC.ReadInt32();
			otmsUnderscorePosition = DC.ReadInt32();
			otmpFamilyName = DC.ReadString();
			otmpFaceName = DC.ReadString();
			otmpStyleName = DC.ReadString();
			otmpFullName = DC.ReadString();
			return;
			}
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Font API class
	/// </summary>
	/// <remarks>
	/// Windows API callable by C# program
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public class PdfFontApi : IDisposable
		{
		private readonly Bitmap BitMap;
		private readonly Graphics GDI;
		private readonly IntPtr GDIHandle;
		private readonly IntPtr FontHandle;
		private readonly IntPtr SavedFont;
		//private readonly int DesignHeight;
		private IntPtr Buffer;
		private int BufPtr;

		////////////////////////////////////////////////////////////////////
		// Device context constructor
		////////////////////////////////////////////////////////////////////

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		private static extern IntPtr SelectObject(IntPtr GDIHandle, IntPtr FontHandle);

		/// <summary>
		/// Font API constructor
		/// </summary>
		/// <param name="DesignFont">Design font</param>
		public PdfFontApi
				(
				Font DesignFont
				)
			{
			// define device context
			BitMap = new Bitmap(1, 1);
			GDI = Graphics.FromImage(BitMap);
			GDIHandle = (IntPtr) GDI.GetHdc();

			// select the font into the device context
			FontHandle = DesignFont.ToHfont();
			SavedFont = SelectObject(GDIHandle, FontHandle);

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Gets single glyph metric
		////////////////////////////////////////////////////////////////////

		private const uint GGO_METRICS = 0;
		//private const uint GGO_BITMAP = 1;
		//private const uint GGO_NATIVE = 2;
		//private const uint GGO_BEZIER = 3;
		private const uint GGO_GLYPH_INDEX = 128;

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		private static extern int GetGlyphOutline(IntPtr GDIHandle, int CharIndex,
			uint GgoFormat, IntPtr GlyphMetrics, uint Zero, IntPtr Null, IntPtr TransMatrix);

		/// <summary>
		/// Gets glyph metric
		/// </summary>
		/// <param name="CharCode">Character code</param>
		/// <returns>Character info class</returns>
		public CharInfo GetGlyphMetricsApiByCode
				(
				int CharCode
				)
			{
			// get glyph index for char code
			int[] GlyphIndexArray = GetGlyphIndicesApi(CharCode, CharCode);

			// get glyph outline
			CharInfo Info = GetGlyphMetricsApiByGlyphIndex(GlyphIndexArray[0]);
			Info.CharCode = CharCode;

			// exit
			return Info;
			}

		/// <summary>
		/// Gets glyph metric
		/// </summary>
		/// <param name="GlyphIndex">Character code</param>
		/// <returns>Character info class</returns>
		public CharInfo GetGlyphMetricsApiByGlyphIndex
				(
				int GlyphIndex
				)
			{
			// build unit matrix
			IntPtr UnitMatrix = BuildUnitMarix();

			// allocate buffer to receive glyph metrics information
			AllocateBuffer(20);

			// get one glyph
			if(GetGlyphOutline(GDIHandle, GlyphIndex, GGO_GLYPH_INDEX, Buffer, 0, IntPtr.Zero, UnitMatrix) < 0)
				ThrowSystemErrorException("Calling GetGlyphOutline failed");

			// create WinOutlineTextMetric class
			CharInfo Info = new CharInfo(0, GlyphIndex, this);

			// free buffer for glyph metrics
			FreeBuffer();

			// free unit matrix buffer
			Marshal.FreeHGlobal(UnitMatrix);

			// exit
			return Info;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Gets array of glyph metrics
		/// </summary>
		/// <param name="CharValue">Character code</param>
		/// <returns>Array of character infos</returns>
		////////////////////////////////////////////////////////////////////
		public CharInfo[] GetGlyphMetricsApi
				(
				int CharValue
				)
			{
			// first character of the 256 block
			int FirstChar = CharValue & 0xff00;

			// get character code to glyph index
			// if GlyphIndex[x] is zero glyph is undefined
			int[] GlyphIndexArray = GetGlyphIndicesApi(FirstChar, FirstChar + 255);

			// test for at least one valid glyph
			int Start;
			for(Start = 0; Start < 256 && GlyphIndexArray[Start] == 0; Start++);
			if(Start == 256) return null;

			// build unit matrix
			IntPtr UnitMatrix = BuildUnitMarix();

			// allocate buffer to receive glyph metrics information
			AllocateBuffer(20);

			// result array
			CharInfo[] CharInfoArray = new CharInfo[256];

			// loop for all characters
			for(int CharCode = Start; CharCode < 256; CharCode++)
				{
				// charater not defined
				int GlyphIndex = GlyphIndexArray[CharCode];
				if(GlyphIndex == 0) continue;

				// get one glyph
				if(GetGlyphOutline(GDIHandle, FirstChar + CharCode, GGO_METRICS, Buffer, 0, IntPtr.Zero, UnitMatrix) < 0)
					ThrowSystemErrorException("Calling GetGlyphOutline failed");

				// reset buffer pointer
				BufPtr = 0;

				// create WinOutlineTextMetric class
				CharInfoArray[CharCode] = new CharInfo(FirstChar + CharCode, GlyphIndex, this);
				}

			// free buffer for glyph metrics
			FreeBuffer();

			// free unit matrix buffer
			Marshal.FreeHGlobal(UnitMatrix);

			// exit
			return CharInfoArray;
			}

		////////////////////////////////////////////////////////////////////
		// Get kerning pairs array
		////////////////////////////////////////////////////////////////////

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		private static extern uint GetKerningPairs(IntPtr GDIHandle, uint NumberOfPairs, IntPtr PairArray);

		/// <summary>
		/// Gets kerning pairs array
		/// </summary>
		/// <param name="FirstChar">First character</param>
		/// <param name="LastChar">Last character</param>
		/// <returns>Array of kerning pairs</returns>
		public WinKerningPair[] GetKerningPairsApi
				(
				int FirstChar,
				int LastChar
				)
			{
			// get number of pairs
			int Pairs = (int) GetKerningPairs(GDIHandle, 0, IntPtr.Zero);
			if(Pairs == 0) return null;

			// allocate buffer to receive outline text metrics information
			AllocateBuffer(8 * Pairs);

			// get outline text metrics information
			if(GetKerningPairs(GDIHandle, (uint) Pairs, Buffer) == 0)
				ThrowSystemErrorException("Calling GetKerningPairs failed");

			// create list because the program will ignore pairs that are outside char range
			List<WinKerningPair> TempList = new List<WinKerningPair>();

			// kerning pairs from buffer
			for(int Index = 0; Index < Pairs; Index++)
				{
				WinKerningPair KPair = new WinKerningPair(this);
				if(KPair.First >= FirstChar && KPair.First <= LastChar && KPair.Second >= FirstChar && KPair.Second <= LastChar)
					TempList.Add(KPair);
				}

			// free buffer for outline text metrics
			FreeBuffer();

			// list is empty
			if(TempList.Count == 0) return null;

			// sort list
			TempList.Sort();

			// exit
			return TempList.ToArray();
			}

		////////////////////////////////////////////////////////////////////
		// Get OUTLINETEXTMETRICW structure
		////////////////////////////////////////////////////////////////////

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		private static extern int GetOutlineTextMetrics(IntPtr GDIHandle, int BufferLength, IntPtr Buffer);

		/// <summary>
		/// Gets OUTLINETEXTMETRICW structure
		/// </summary>
		/// <returns>Outline text metric class</returns>
		public WinOutlineTextMetric GetOutlineTextMetricsApi()
			{
			// get buffer size
			int BufSize = GetOutlineTextMetrics(GDIHandle, 0, IntPtr.Zero);
			if(BufSize == 0) ThrowSystemErrorException("Calling GetOutlineTextMetrics (get buffer size) failed");

			// allocate buffer to receive outline text metrics information
			AllocateBuffer(BufSize);

			// get outline text metrics information
			if(GetOutlineTextMetrics(GDIHandle, BufSize, Buffer) == 0)
				ThrowSystemErrorException("Calling GetOutlineTextMetrics failed");

			// create WinOutlineTextMetric class
			WinOutlineTextMetric WOTM = new WinOutlineTextMetric(this);

			// free buffer for outline text metrics
			FreeBuffer();

			// exit
			return WOTM;
			}

		////////////////////////////////////////////////////////////////////
		// Get TEXTMETRICW structure
		////////////////////////////////////////////////////////////////////

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		private static extern int GetTextMetrics(IntPtr GDIHandle, IntPtr Buffer);

		/// <summary>
		/// Gets TEXTMETRICW structure
		/// </summary>
		/// <returns>Text metric class</returns>
		public WinTextMetric GetTextMetricsApi()
			{
			// allocate buffer to receive outline text metrics information
			AllocateBuffer(57);

			// get outline text metrics information
			if(GetTextMetrics(GDIHandle, Buffer) == 0)
				ThrowSystemErrorException("Calling GetTextMetrics API failed.");

			// create WinOutlineTextMetric class
			WinTextMetric WTM = new WinTextMetric(this);

			// free buffer for outline text metrics
			FreeBuffer();

			// exit
			return WTM;
			}

		////////////////////////////////////////////////////////////////////
		// Get font data tables
		////////////////////////////////////////////////////////////////////

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		private static extern uint GetFontData(IntPtr DeviceContextHandle, uint Table, uint Offset, IntPtr Buffer, uint BufferLength);

		/// <summary>
		/// Gets font data tables
		/// </summary>
		/// <param name="TableTag">Table Tag</param>
		/// <param name="Offset">Table offset</param>
		/// <param name="BufSize">Table size</param>
		/// <returns>Table info as byte array</returns>
		public byte[] GetFontDataApi
				(
				uint TableTag,
				int Offset,
				int BufSize
				)
			{
			// empty table
			if(BufSize == 0)
				return null;

			// allocate buffer to receive outline text metrics information
			AllocateBuffer((int) BufSize);

			// microsoft tag is in little endian format
			uint MSTag = TableTag << 24 | (TableTag << 8) & 0xff0000 | (TableTag >> 8) & 0xff00 | (TableTag >> 24) & 0xff;

			// get outline text metrics information
			if((int) GetFontData(GDIHandle, MSTag, (uint) Offset, Buffer, (uint) BufSize) != BufSize)
				ThrowSystemErrorException("Get font data file header failed");

			// copy api result buffer to managed memory buffer
			byte[] DataBuffer = new byte[BufSize];
			Marshal.Copy(Buffer, DataBuffer, 0, BufSize);
			BufPtr = 0;

			// free unmanaged memory buffer
			FreeBuffer();

			// exit
			return DataBuffer;
			}

		////////////////////////////////////////////////////////////////////
		// Get glyph indices array
		////////////////////////////////////////////////////////////////////

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		private static extern int GetGlyphIndices(IntPtr GDIHandle, IntPtr CharBuffer, int CharCount,
			IntPtr GlyphArray, uint GlyphOptions);

		/// <summary>
		/// Gets glyph indices array
		/// </summary>
		/// <param name="FirstChar">First character</param>
		/// <param name="LastChar">Last character</param>
		/// <returns>Array of glyph indices.</returns>
		public int[] GetGlyphIndicesApi
				(
				int FirstChar,
				int LastChar
				)
			{
			// character count
			int CharCount = LastChar - FirstChar + 1;

			// allocate character table buffer in global memory (two bytes per char)
			IntPtr CharBuffer = Marshal.AllocHGlobal(2 * CharCount);

			// create array of all character codes between FirstChar and LastChar (we use short because of Unicode)
			for(int CharPtr = FirstChar; CharPtr <= LastChar; CharPtr++)
				Marshal.WriteInt16(CharBuffer, 2 * (CharPtr - FirstChar), (short) (CharPtr));

			// allocate memory for result
			AllocateBuffer(2 * CharCount);

			// get glyph numbers for all characters including non existing glyphs
			if(GetGlyphIndices(GDIHandle, CharBuffer, CharCount, Buffer, (uint) 0) != CharCount)
				ThrowSystemErrorException("Calling GetGlypeIndices failed");

			// get result array to managed code
			short[] GlyphIndex16 = ReadInt16Array(CharCount);

			// free local buffer
			Marshal.FreeHGlobal(CharBuffer);

			// free result buffer
			FreeBuffer();

			// convert to int
			int[] GlyphIndex32 = new int[GlyphIndex16.Length];
			for(int Index = 0; Index < GlyphIndex16.Length; Index++)
				GlyphIndex32[Index] = (ushort) GlyphIndex16[Index];

			// exit
			return GlyphIndex32;
			}

		////////////////////////////////////////////////////////////////////
		// Allocate API result buffer
		////////////////////////////////////////////////////////////////////

		private void AllocateBuffer
				(
				int Size
				)
			{
			// allocate memory for result
			Buffer = Marshal.AllocHGlobal(Size);
			BufPtr = 0;
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Free API result buffer
		////////////////////////////////////////////////////////////////////

		private void FreeBuffer()
			{
			// free buffer
			Marshal.FreeHGlobal(Buffer);
			Buffer = IntPtr.Zero;
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Align buffer pointer to 4 bytes boundry
		////////////////////////////////////////////////////////////////////

		internal void Align4()
			{
			BufPtr = (BufPtr + 3) & ~3;
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Read point (x, y) from data buffer
		////////////////////////////////////////////////////////////////////

		internal Point ReadWinPoint()
			{
			return new Point(ReadInt32(), ReadInt32());
			}

		////////////////////////////////////////////////////////////////////
		// Read byte from data buffer
		////////////////////////////////////////////////////////////////////

		internal byte ReadByte()
			{
			return Marshal.ReadByte(Buffer, BufPtr++);
			}

		////////////////////////////////////////////////////////////////////
		// Read character from data buffer
		////////////////////////////////////////////////////////////////////

		internal char ReadChar()
			{
			char Value = (char) Marshal.ReadInt16(Buffer, BufPtr);
			BufPtr += 2;
			return Value;
			}

		////////////////////////////////////////////////////////////////////
		// Read short integer from data buffer
		////////////////////////////////////////////////////////////////////

		internal short ReadInt16()
			{
			short Value = Marshal.ReadInt16(Buffer, BufPtr);
			BufPtr += 2;
			return Value;
			}

		////////////////////////////////////////////////////////////////////
		// Read unsigned short integer from data buffer
		////////////////////////////////////////////////////////////////////

		internal ushort ReadUInt16()
			{
			ushort Value = (ushort) Marshal.ReadInt16(Buffer, BufPtr);
			BufPtr += 2;
			return Value;
			}

		////////////////////////////////////////////////////////////////////
		// Read short array from result buffer
		////////////////////////////////////////////////////////////////////

		internal short[] ReadInt16Array
				(
				int Size
				)
			{
			// create active characters array
			short[] Result = new short[Size];
			Marshal.Copy(Buffer, Result, 0, Size);
			return Result;
			}

		////////////////////////////////////////////////////////////////////
		// Read integers from data buffer
		////////////////////////////////////////////////////////////////////

		internal int ReadInt32()
			{
			int Value = Marshal.ReadInt32(Buffer, BufPtr);
			BufPtr += 4;
			return Value;
			}

		////////////////////////////////////////////////////////////////////
		// Read int array from result buffer
		////////////////////////////////////////////////////////////////////

		internal int[] ReadInt32Array
				(
				int Size
				)
			{
			// create active characters array
			int[] Result = new int[Size];
			Marshal.Copy(Buffer, Result, 0, Size);
			return Result;
			}

		////////////////////////////////////////////////////////////////////
		// Read unsigned integers from data buffer
		////////////////////////////////////////////////////////////////////

		internal uint ReadUInt32()
			{
			uint Value = (uint) Marshal.ReadInt32(Buffer, BufPtr);
			BufPtr += 4;
			return Value;
			}

		////////////////////////////////////////////////////////////////////
		// Read string (null terminated) from data buffer
		////////////////////////////////////////////////////////////////////

		internal string ReadString()
			{
			int Ptr = Marshal.ReadInt32(Buffer, BufPtr);
			BufPtr += 4;
			StringBuilder Str = new StringBuilder();
			for(;;)
				{
				char Chr = (char) Marshal.ReadInt16(Buffer, Ptr);
				if(Chr == 0)
					break;
				Str.Append(Chr);
				Ptr += 2;
				}
			return Str.ToString();
			}

		////////////////////////////////////////////////////////////////////
		// Throw exception showing last system error
		////////////////////////////////////////////////////////////////////

		[DllImport("Kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		private static extern uint FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId,
			IntPtr lpBuffer, uint nSize, IntPtr Arguments);

		internal static void ThrowSystemErrorException
				(
				string AppMsg
				)
			{
			const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x1000;

			// error message
			StringBuilder ErrMsg = new StringBuilder(AppMsg);

			// get last system error
			uint ErrCode = (uint) Marshal.GetLastWin32Error(); // GetLastError();
			if(ErrCode != 0)
				{
				// allocate buffer
				IntPtr ErrBuffer = Marshal.AllocHGlobal(1024);

				// add error code
				ErrMsg.AppendFormat("\r\nSystem error [0x{0:X8}]", ErrCode);

				// convert error code to text
				int StrLen = (int) FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero, ErrCode, 0, ErrBuffer, 1024, IntPtr.Zero);
				if(StrLen > 0)
					{
					ErrMsg.Append(' ');
					ErrMsg.Append(Marshal.PtrToStringAuto(ErrBuffer, StrLen));
					while(ErrMsg[^1] <= ' ') ErrMsg.Length--;
					}

				// free buffer
				Marshal.FreeHGlobal(ErrBuffer);
				}

			// unknown error
			else
				{
				ErrMsg.Append("\r\nUnknown error.");
				}

			// exit
			throw new ApplicationException(ErrMsg.ToString());
			}

		////////////////////////////////////////////////////////////////////
		// Build unit matrix in unmanaged memory
		////////////////////////////////////////////////////////////////////

		private static IntPtr BuildUnitMarix()
			{
			// allocate buffer for transformation matrix
			IntPtr UnitMatrix = Marshal.AllocHGlobal(16);

			// set transformation matrix into unit matrix
			Marshal.WriteInt32(UnitMatrix, 0, 0x10000);
			Marshal.WriteInt32(UnitMatrix, 4, 0);
			Marshal.WriteInt32(UnitMatrix, 8, 0);
			Marshal.WriteInt32(UnitMatrix, 12, 0x10000);
			return UnitMatrix;
			}

		////////////////////////////////////////////////////////////////////
		// Dispose
		////////////////////////////////////////////////////////////////////

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		private static extern IntPtr DeleteObject(IntPtr Handle);

		/// <summary>
		/// Dispose unmanaged resources
		/// </summary>
		public void Dispose()
			{
			// free unmanaged buffer
			Marshal.FreeHGlobal(Buffer);

			// restore original font
			SelectObject(GDIHandle, SavedFont);

			// delete font handle
			DeleteObject(FontHandle);

			// release device context handle
			GDI.ReleaseHdc(GDIHandle);

			// release GDI resources
			GDI.Dispose();

			// release bitmap
			BitMap.Dispose();

			// exit
			return;
			}
		}
	}
