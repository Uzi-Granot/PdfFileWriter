/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PDF417 Barcode Encoder
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

using System.Numerics;
using System.Text;

//namespace Pdf417EncoderLibrary
namespace PdfFileWriter
	{
	/// <summary>
	/// PDF417 Encoding control
	/// </summary>
	public enum EncodingControl
		{
		/// <summary>
		/// Auto encoding control
		/// </summary>
		Auto,

		/// <summary>
		/// Encode all as bytes
		/// </summary>
		ByteOnly,

		/// <summary>
		/// Encode all as text and bytes
		/// </summary>
		TextAndByte,
		}

	/// <summary>
	/// PDF417 Error correction level
	/// </summary>
	public enum ErrorCorrectionLevel
		{
		/// <summary>
		/// Error correction level 0 (2 correction codewords)
		/// </summary>
		Level_0,
		/// <summary>
		/// Error correction level 1 (4 correction codewords)
		/// </summary>
		Level_1,
		/// <summary>
		/// Error correction level 2 (8 correction codewords)
		/// </summary>
		Level_2,
		/// <summary>
		/// Error correction level 3 (16 correction codewords)
		/// </summary>
		Level_3,
		/// <summary>
		/// Error correction level 4 (32 correction codewords)
		/// </summary>
		Level_4,
		/// <summary>
		/// Error correction level 5 (64 correction codewords)
		/// </summary>
		Level_5,
		/// <summary>
		/// Error correction level 6 (128 correction codewords)
		/// </summary>
		Level_6,
		/// <summary>
		/// Error correction level 7 (256 correction codewords)
		/// </summary>
		Level_7,
		/// <summary>
		/// Error correction level 8 (512 correction codewords)
		/// </summary>
		Level_8,
		/// <summary>
		/// Recommended level less one
		/// </summary>
		AutoLow,
		/// <summary>
		/// Recomended level based on number of codewords
		/// </summary>
		AutoNormal,
		/// <summary>
		/// Recommended level plus one
		/// </summary>
		AutoMedium,
		/// <summary>
		/// Recommended level plus two
		/// </summary>
		AutoHigh
		}

	/// <summary>
	/// PDF417 Encoder class
	/// </summary>
	public class Pdf417Encoder : Pdf417EncoderTables
		{
		// encoding mode
		private enum EncodingMode
			{
			Byte,
			Text,
			Numeric,
			}

		// text encoding sub-mode
		private enum TextEncodingMode
			{
			Upper,
			Lower,
			Mixed,
			Punct,
			ShiftUpper,
			ShiftPunct,
			}

		/// <summary>
		/// Data rows
		/// </summary>
		public int DataRows { get; private set; }

		/// <summary>
		/// Data columns
		/// </summary>
		public int DataColumns { get; private set; }

		/// <summary>
		/// Returns the barcode width in terms of narrow bars
		/// </summary>
		public int BarColumns { get; private set; }

		/// <summary>
		/// Barcode image width in pixels
		/// </summary>
		public int ImageWidth { get; private set; }

		/// <summary>
		/// Barcode image height in pixels
		/// </summary>
		public int ImageHeight { get; private set; }

		/// <summary>
		/// Barcode matrix (each bar is one bool item)
		/// </summary>
		public bool[,] Pdf417BarcodeMatrix;

		// Pdf417 constants
		private const int MaxCodewords = 929;
		private const int MOD = MaxCodewords;
		private const int ModulesInCodeword = 17;

		private const int DataRowsMin = 3;
		private const int DataRowsMax = 90;

		private const int DataColumnsMin = 1;
		private const int DataColumnsMax = 30;

		private static readonly bool[] StartCodeword = {true, true, true, true, true, true, true, true, false,
											true, false, true, false, true, false, false, false};
		private const int StartPatternLen = ModulesInCodeword;

		private static readonly bool[] StopCodeword = {true, true, true, true, true, true, true, false, true, false,
										   false, false, true, false, true, false, false, true};
		private const int StopPatternLen = ModulesInCodeword + 1;

		// Control codewords
		private const int SwitchToTextMode = 900;
		private const int SwitchToByteMode = 901;
		private const int SwitchToNumericMode = 902;
		private const int ShiftToByteMode = 913;
		private const int SwitchToByteModeForSix = 924;

		// User-Defined GLis:
		// Codeword 925 followed by one data codeword.
		// The data codeword has a range of 0 to 899.
		private const int GliUserDefined = 925;

		// General Purpose GLis:
		// Codeword 926 followed by two data codewords.
		// The two data codewords have a range of 0 to 899 each.
		private const int GliGeneralPurpose = 926;

		// International character set:
		// Codeword 927 followed by a single data codeword.
		// The data codeword has a range of 0 to 899.
		// In this class the data codeword is a function
		// of ISO 8859 part number. The class supports
		// ISO-8859-n (1-9, 13, 15) The default is ISO-8859-1.
		// The codeword value is Part + 2.
		private const int GliCharacterSet = 927;

		// saved barcode string and binary data
		private byte[] BarcodeBinaryData;
		private int BarcodeDataLength;
		private int BarcodeDataPos;

		// encoding input data into codewords
		private EncodingMode _EncodingMode;
		private TextEncodingMode _TextEncodingMode;
		private List<int> DataCodewords;

		// error correction level, length and codewords
		private ErrorCorrectionLevel ErrorCorrectionLevel;
		private int ErrorCorrectionLength;
		private int[] ErrCorrCodewords;

		/// <summary>
		/// Encoding control (Default: Auto)
		/// </summary>
		public EncodingControl EncodingControl
			{
			get
				{
				return _EncodingControl;
				}
			set
				{
				// test symbol encoding
				if(value != EncodingControl.Auto &&
					value != EncodingControl.ByteOnly &&
					value != EncodingControl.TextAndByte)
					throw new ArgumentException("PDF417 Encoding control must be Auto,\r\n" +
													"ByteOnly or TextAndByte. Default is Auto.");

				// save error correction level
				_EncodingControl = value;
				return;
				}
			}
		private EncodingControl _EncodingControl = EncodingControl.Auto;

		/// <summary>
		/// Error correction level (Default: AutoNormal)
		/// </summary>
		public ErrorCorrectionLevel ErrorCorrection
			{
			get
				{
				return _ErrorCorrection;
				}
			set
				{
				// test error correction
				if(value < ErrorCorrectionLevel.Level_0 || value > ErrorCorrectionLevel.AutoHigh)
					throw new ArgumentException("PDF417 Error correction request is invalid.\r\n" +
													"Default is Auto normal.");

				// save error correction level
				_ErrorCorrection = value;
				return;
				}
			}
		private ErrorCorrectionLevel _ErrorCorrection = ErrorCorrectionLevel.AutoNormal;

		/// <summary>
		/// Narrow bar width in pixels (Default: 2)
		/// </summary>
		public int NarrowBarWidth
			{
			get
				{
				return _BarWidthPix;
				}
			set
				{
				if(value < 1 || value > 100)
					throw new ArgumentException("PDF417 Narrow bar width must be one or more\r\n" +
													"Default is two.");
				_BarWidthPix = value;

				// row height must be at least 3 times the width of narrow bar
				if(_RowHeightPix < 3 * value)
					_RowHeightPix = 3 * value;

				// quiet zone must be at least 2 times the width of narrow bar
				if(_QuietZonePix < 2 * value)
					_QuietZonePix = 2 * value;

				// set image width and height
				SetImageWidthAndHeight();
				return;
				}
			}
		private int _BarWidthPix = 2;

		/// <summary>
		/// Row height in pixels (Default: 6)
		/// </summary>
		public int RowHeight
			{
			get
				{
				return _RowHeightPix;
				}
			set
				{
				if(value < 3 * _BarWidthPix || value > 300)
					throw new ArgumentException("PDF417 Row height must be at least 3 times the narrow bar width.\r\n" +
													"Default is six.");
				_RowHeightPix = value;

				// set image width and height
				SetImageWidthAndHeight();
				return;
				}
			}
		private int _RowHeightPix = 6;

		/// <summary>
		/// Quiet zone around the barcode in pixels (Default: 4)
		/// </summary>
		public int QuietZone
			{
			get
				{
				return _QuietZonePix;
				}
			set
				{
				if(value < 2 * _BarWidthPix || value > 200)
					throw new ArgumentException("PDF417 Quiet zone must be at least 2 times the narrow bar width.\r\n" +
													"Default is four.");
				_QuietZonePix = value;

				// set image width and height
				SetImageWidthAndHeight();
				return;
				}
			}
		private int _QuietZonePix = 4;

		/// <summary>
		/// Default number of data columns (Default: 3)
		/// </summary>
		public int DefaultDataColumns
			{
			get
				{
				return _DefaultDataColumns;
				}
			set
				{
				if(value < DataColumnsMin || value > DataColumnsMax)
					throw new ArgumentException("PDF417 Default data columns must be 1 to 30.\r\n" +
													"Default is three.");
				_DefaultDataColumns = value;
				return;
				}
			}
		private int _DefaultDataColumns = 3;

		/// <summary>
		/// ISO character set ISO-8859-n (n=1 to 9, 13 and 15) (Default: null)
		/// </summary>
		public string GlobalLabelIDCharacterSet
			{
			get
				{
				return _GlobalLabelIDCharacterSet;
				}
			set
				{
				if(string.IsNullOrWhiteSpace(value))
					{
					_GlobalLabelIDCharacterSet = null;
					return;
					}

				if(string.Compare(value.Substring(0, 9), "ISO-8859-", true) != 0 ||
					!int.TryParse(value.Substring(9), out int Part) || Part < 1 || Part > 9 && Part != 13 && Part != 15)
					{
					throw new ArgumentException("PDF417 Character set code in error.\r\n" +
													"Must be ISO-8859-n (1-9, 13, 15).\r\n" +
													"Default is ISO-8859-1.");
					}
				_GlobalLabelIDCharacterSet = value;
				return;
				}
			}
		// Global Label Identifier character set
		private string _GlobalLabelIDCharacterSet;

		/// <summary>
		/// Global label ID user defined (Default: 0)
		/// </summary>
		public int GlobalLabelIDUserDefined
			{
			get
				{
				return _GlobalLabelIDUserDefined;
				}
			set
				{
				if(value != 0 && (value < 810900 || value > 811799))
					throw new ArgumentException("PDF417 Global label identifier user defined value.\r\n" +
													"Must be 810900 to 811799 or zero\r\n" +
													"Default is not used or zero value");
				_GlobalLabelIDUserDefined = value;
				return;
				}
			}
		private int _GlobalLabelIDUserDefined = 0;

		/// <summary>
		/// Global label ID general purpose (Default: 0)
		/// </summary>
		public int GlobalLabelIDGeneralPurpose
			{
			get
				{
				return _GlobalLabelIDGeneralPurpose;
				}
			set
				{
				if(value != 0 && (value < 900 || value > 810899))
					throw new ArgumentException("PDF417 Global label identifier general purpose value.\r\n" +
													"Must be 900 to 810899 or zero\r\n" +
													"Default is not used or zero value");
				_GlobalLabelIDGeneralPurpose = value;
				return;
				}
			}
		// code word 926 value
		private int _GlobalLabelIDGeneralPurpose = 0;

		/// <summary>
		/// Encode unicode string
		/// </summary>
		/// <param name="StringData">Input text string</param>
		public void Encode
				(
				string StringData
				)
			{
			// argument error
			if(string.IsNullOrEmpty(StringData))
				throw new ArgumentException("PDF417 Input barcode data string is null or empty.");

			// convert text string to byte array
			Encoding ISO = Encoding.GetEncoding(_GlobalLabelIDCharacterSet ?? "ISO-8859-1");
			byte[] UtfBytes = Encoding.UTF8.GetBytes(StringData);
			byte[] IsoBytes = Encoding.Convert(Encoding.UTF8, ISO, UtfBytes);

			// encode binary data
			Encode(IsoBytes);
			return;
			}

		/// <summary>
		/// Encode binary bytes array
		/// </summary>
		/// <param name="BinaryData">Input binary byte array</param>
		public void Encode
				(
				byte[] BinaryData
				)
			{
			// reset barcode matrix
			Pdf417BarcodeMatrix = null;
			DataRows = 0;
			DataColumns = 0;

			// test data segments array
			if(BinaryData == null || BinaryData.Length == 0)
				throw new ArgumentNullException("PDF417 Input binary barcode data is null or empty");

			// save data segments array
			BarcodeBinaryData = BinaryData;

			// data encoding
			// convert the byte array into Pdf417 codewords
			DataEncoding();

			// set error correction level
			SetErrorCorrectionLevel();

			// set data columns and data rows based on the default data columns
			// this calculation adds DataColumns - 1 for rounding up
			int dataColumns = _DefaultDataColumns;
			int dataRows = (DataCodewords.Count + ErrorCorrectionLength + dataColumns - 1) / dataColumns;

			// if data rows exceeds the maximum allowed,
			// adjust data columns to reduce the number of rows
			if(dataRows > DataRowsMax)
				{
				dataRows = DataRowsMax;
				dataColumns = (DataCodewords.Count + ErrorCorrectionLength + dataRows - 1) / dataRows;
				if(dataColumns > DataColumnsMax)
					throw new ApplicationException("PDF417 barcode data overflow");
				}

			// save data rows and data columns in the class
			SetDataRowsAndColumns(dataRows, dataColumns);
			return;
			}

		/*
		Documentation for the quadratic equation solution below
		-------------------------------------------------------
		ImageWidth = _BarWidthPix * (ModulesInCodeword * (DataColumns + 4) + 1) + 2 * _QuietZonePix;
		ImageHeight = _RowHeightPix * DataRows + 2 * _QuietZonePix;

		Ratio = ImageWidth / ImageHeight;

		Ratio * (_RowHeightPix * DataRows + 2 * _QuietZonePix) = _BarWidthPix * (ModulesInCodeword * (DataColumns + 4) + 1) + 2 * _QuietZonePix

		Ratio * _RowHeightPix * DataRows + 2 * Ratio * _QuietZonePix =
			 _BarWidthPix * ModulesInCodeword * (DataColumns + 4) + _BarWidthPix + 2 * _QuietZonePix

		Ratio * _RowHeightPix * DataRows + 2 * Ratio * _QuietZonePix =
			 _BarWidthPix * ModulesInCodeword * DataColumns + 4 * _BarWidthPix * ModulesInCodeword + _BarWidthPix + 2 * _QuietZonePix

		Ratio * _RowHeightPix * DataRows - _BarWidthPix * ModulesInCodeword * DataColumns =
			 4 * _BarWidthPix * ModulesInCodeword + _BarWidthPix + 2 * _QuietZonePix - 2 * Ratio * _QuietZonePix

		DataRows * DataColumns = DataCodewords.Count + ErrorCorrectionLength

		Total = DataCodewords.Count + ErrorCorrectionLength

		DataRows * DataColumns = Total

		A = _BarWidthPix * ModulesInCodeword
		B = _BarWidthPix * (4 * ModulesInCodeword + 1) + 2 * _QuietZonePix * (1 - Ratio)
		C = Ratio * _RowHeightPix

		C * DataRows - A * DataColumns = B;
		C * Total / DataColumns - A * DataColumns = B;
		A * DataColumns**2 + B * DataColumns - C * Total = 0;
		*/

		/// <summary>
		/// Adjust rows and columns to achive width to height ratio
		/// </summary>
		/// <param name="Ratio">Requested width to height ratio</param>
		/// <returns>Success or failure result</returns>
		public bool WidthToHeightRatio
				(
				double Ratio
				)
			{
			try
				{
				// total of data and error correction but no padding
				int Total = DataCodewords.Count + ErrorCorrectionLength;

				double A = _BarWidthPix * ModulesInCodeword;
				double B = _BarWidthPix * (4 * ModulesInCodeword + 1) - 2 * _QuietZonePix * (Ratio - 1);
				double C = Total * Ratio * _RowHeightPix;

				// initial guess for columns
				double Columns = (-B + Math.Sqrt(B * B + 4 * A * C)) / (2 * A);

				// calculated data columns and rows to meet the width to height ratio
				// this calculation adds DataColumns - 1 for rounding up
				int dataColumns = (int) Math.Round(Columns, 0, MidpointRounding.AwayFromZero);
				if(dataColumns < DataColumnsMin || dataColumns > DataColumnsMax)
					return false;

				int dataRows = (Total + dataColumns - 1) / dataColumns;
				if(dataRows < DataRowsMin || dataRows > DataRowsMax)
					return false;

				// save data rows and data columns in the class
				SetDataRowsAndColumns(dataRows, dataColumns);
				return true;
				}
			catch
				{
				return false;
				}
			}

		/// <summary>
		/// Set number of data columns and data rows 
		/// </summary>
		/// <param name="dataColumns">Data columns</param>
		/// <returns>Success or failure result</returns>
		public bool SetDataColumns
				(
				int dataColumns
				)
			{
			try
				{
				// columns outside valid range
				if(dataColumns < DataColumnsMin || dataColumns > DataColumnsMax)
					return false;

				// calculate rows
				int dataRows = (DataCodewords.Count + ErrorCorrectionLength + dataColumns - 1) / dataColumns;
				if(dataRows < DataRowsMin || dataRows > DataRowsMax)
					return false;

				// save data rows and data columns in the class
				SetDataRowsAndColumns(dataRows, dataColumns);
				Pdf417BarcodeMatrix = null;
				return true;
				}
			catch
				{
				return false;
				}
			}

		/// <summary>
		/// Set number of data rows and data columns 
		/// </summary>
		/// <param name="dataRows">Data rowss</param>
		/// <returns>Success or failure result</returns>
		public bool SetDataRows
				(
				int dataRows
				)
			{
			try
				{
				// rows outside valid range
				if(dataRows < DataRowsMin || dataRows > DataRowsMax)
					return false;

				// calculate columns
				int dataColumns = (DataCodewords.Count + ErrorCorrectionLength + dataRows - 1) / dataRows;
				if(dataColumns < DataColumnsMin || dataColumns > DataColumnsMax)
					return false;

				// set rows and columns
				SetDataRowsAndColumns(dataRows, dataColumns);
				return true;
				}
			catch
				{
				return false;
				}
			}

		private void SetDataRowsAndColumns
				(
				int dataRows,
				int dataColumns
				)
			{
			// test for change
			if(dataColumns != DataColumns || dataRows != DataRows)
				{
				// set rows and columns
				DataRows = dataRows;
				DataColumns = dataColumns;
				Pdf417BarcodeMatrix = null;
				SetImageWidthAndHeight();
				}
			return;
			}

		private void SetImageWidthAndHeight()
			{
			BarColumns = ModulesInCodeword * (DataColumns + 4) + 1;
			ImageWidth = _BarWidthPix * BarColumns + 2 * _QuietZonePix;
			ImageHeight = _RowHeightPix * DataRows + 2 * _QuietZonePix;
			return;
			}

		/// <summary>
		/// convert black and white matrix to black and white image
		/// </summary>
		/// <returns>Black and white image in pixels</returns>
		public bool[,] ConvertBarcodeMatrixToPixels()
			{
			// create barcode matrix
			if(Pdf417BarcodeMatrix == null)
				Pdf417BarcodeMatrix = CreateBarcodeMatrix();

			// Pdf417Matrix width and height
			int MatrixWidth = BarColumns;
			int MatrixHeight = DataRows;

			// output matrix size in pixels all matrix elements are white (false)
			bool[,] BWImage = new bool[ImageHeight, ImageWidth];

			int XOffset = _QuietZonePix;
			int YOffset = _QuietZonePix;

			// convert result matrix to output matrix
			for(int Row = 0; Row < MatrixHeight; Row++)
				{
				for(int Col = 0; Col < MatrixWidth; Col++)
					{
					// bar is black
					if(Pdf417BarcodeMatrix[Row, Col])
						{
						for(int Y = 0; Y < RowHeight; Y++)
							{
							for(int X = 0; X < _BarWidthPix; X++)
								BWImage[YOffset + Y, XOffset + X] = true;
							}
						}
					XOffset += _BarWidthPix;
					}
				XOffset = _QuietZonePix;
				YOffset += RowHeight;
				}
			return BWImage;
			}

		/// <summary>
		/// Create black and white boolean matrix of the barcode image
		/// </summary>
		/// <returns>bool matrix [row, col] true=black, false=white</returns>
		public bool[,] CreateBarcodeMatrix()
			{
			// create empty codewords array
			int[] Codewords = new int[DataRows * DataColumns];

			// length of data codewords plus length codeword
			int DataLength = DataCodewords.Count;

			// move data codewords
			for(int Index = 0; Index < DataLength; Index++)
				Codewords[Index] = DataCodewords[Index];

			// calculate padding
			int PaddingCodewordsCount = DataRows * DataColumns - DataCodewords.Count - ErrorCorrectionLength;

			// add padding if needed
			for(int Index = 0; Index < PaddingCodewordsCount; Index++)
				Codewords[DataLength++] = SwitchToTextMode;

			// set length (codewords plus padding)
			Codewords[0] = DataLength;

			// calculate error correction and move it to codewords array
			CalculateErrorCorrection(Codewords);

			// create output boolean matrix (Black=true, White=false)
			// not including quiet zone
			bool[,] Pdf417Matrix = new bool[DataRows, BarColumns];

			// last data row
			int LastRow = DataRows - 1;

			// fill output boolean matrix
			int StopCol = BarColumns - StopPatternLen;
			int RightRowIndCol = StopCol - ModulesInCodeword;
			for(int Row = 0; Row < DataRows; Row++)
				{
				// move start codeword at the start of each row
				for(int Col = 0; Col < StartPatternLen; Col++)
					Pdf417Matrix[Row, Col] = StartCodeword[Col];

				// move stop codeword at the end of each row
				for(int Col = 0; Col < StopPatternLen; Col++)
					Pdf417Matrix[Row, StopCol + Col] = StopCodeword[Col];

				// calculate left and right row indicators
				int LeftRowInd = 30 * (Row / 3);
				int RightRowInd = LeftRowInd;
				int Cluster = Row % 3;

				// cluster 0
				if(Cluster == 0)
					{
					LeftRowInd += LastRow / 3;
					RightRowInd += DataColumns - 1;
					}

				// cluster 1
				else if(Cluster == 1)
					{
					LeftRowInd += ((int) ErrorCorrectionLevel * 3) + (LastRow % 3);
					RightRowInd += LastRow / 3;
					}

				// cluster 2
				else
					{
					LeftRowInd += DataColumns - 1;
					RightRowInd += ((int) ErrorCorrectionLevel * 3) + (LastRow % 3);
					}

				// move left and right row indicators to barcode matrix
				CodewordToModules(Row, StartPatternLen, LeftRowInd, Pdf417Matrix);
				CodewordToModules(Row, RightRowIndCol, RightRowInd, Pdf417Matrix);

				// loop for all codewords of this row
				for(int Col = 0; Col < DataColumns; Col++)
					{
					// code word pointer
					int Ptr = DataColumns * Row + Col;

					// move codeword to barcode matrix
					CodewordToModules(Row, ModulesInCodeword * (Col + 2), Codewords[Ptr], Pdf417Matrix);
					}
				}

			// save barcode matrix
			Pdf417BarcodeMatrix = Pdf417Matrix;

			// exit
			return Pdf417Matrix;
			}

		// set error correction level 
		// it is either absolute or relative to data length
		private void SetErrorCorrectionLevel()
			{
			// fixed error correction
			if(_ErrorCorrection >= ErrorCorrectionLevel.Level_0 && _ErrorCorrection <= ErrorCorrectionLevel.Level_8)
				{
				// error correction level
				ErrorCorrectionLevel = _ErrorCorrection;
				}
			else
				{
				// recommended normal values
				int DataLength = DataCodewords.Count;
				if(DataLength <= 40)
					ErrorCorrectionLevel = ErrorCorrectionLevel.Level_2;
				else if(DataLength <= 160)
					ErrorCorrectionLevel = ErrorCorrectionLevel.Level_3;
				else if(DataLength <= 320)
					ErrorCorrectionLevel = ErrorCorrectionLevel.Level_4;
				else if(DataLength <= 863)
					ErrorCorrectionLevel = ErrorCorrectionLevel.Level_5;
				else
					ErrorCorrectionLevel = ErrorCorrectionLevel.Level_6;

				if(_ErrorCorrection == ErrorCorrectionLevel.AutoLow)
					ErrorCorrectionLevel--;
				else if(_ErrorCorrection == ErrorCorrectionLevel.AutoMedium)
					ErrorCorrectionLevel++;
				else if(_ErrorCorrection == ErrorCorrectionLevel.AutoHigh)
					ErrorCorrectionLevel += 2;
				}

			// number of error correction codewords
			ErrorCorrectionLength = 1 << ((int) ErrorCorrectionLevel + 1);
			return;
			}

		// perform data encoding.
		// input text string or binary array to Pdf417 codewords
		private void DataEncoding()
			{
			// create empty codewords array
			DataCodewords = new List<int>();

			// add the data codewords length codeword
			DataCodewords.Add(0);

			// character set
			if(_GlobalLabelIDCharacterSet != null)
				{
				int Part = int.Parse(_GlobalLabelIDCharacterSet.Substring(9));
				DataCodewords.Add(GliCharacterSet);
				DataCodewords.Add(Part + 2);
				}

			// general purpose
			if(_GlobalLabelIDGeneralPurpose != 0)
				{
				DataCodewords.Add(GliGeneralPurpose);

				// (G2 + 1) * 900 + G3
				int G2 = Math.DivRem(_GlobalLabelIDGeneralPurpose, 900, out int G3) - 1;
				DataCodewords.Add(G2);
				DataCodewords.Add(G3);
				}

			// user defined
			if(_GlobalLabelIDUserDefined != 0)
				{
				DataCodewords.Add(GliUserDefined);
				DataCodewords.Add(_GlobalLabelIDUserDefined - 810900);
				}

			// barcode data pointer
			BarcodeDataPos = 0;
			BarcodeDataLength = BarcodeBinaryData.Length;

			// requested data encoding is byte only
			if(_EncodingControl == EncodingControl.ByteOnly)
				{
				EncodeByteSegment(BarcodeDataLength);
				return;
				}

			// User selected encoding mode is auto or text plus byte (no numeric)
			// set the default encoding mode and text sub mode
			_EncodingMode = EncodingMode.Text;
			_TextEncodingMode = TextEncodingMode.Upper;

			// scan the barcode data
			while(BarcodeDataPos < BarcodeDataLength)
				{
				// test for numeric encoding if request is auto
				if(_EncodingControl == EncodingControl.Auto)
					{
					// count consequtive digits at this point
					int Digits = CountDigits();
					if(Digits >= 13)
						{
						EncodeNumericSegment(Digits);
						continue;
						}
					}

				// count text
				int TextChars = CountText();
				if(TextChars >= 5)
					{
					EncodeTextSegment(TextChars);
					continue;
					}

				// count binary
				int Bytes = CountBytes();

				// encode binary
				EncodeByteSegment(Bytes);
				}
			return;
			}

		// count digits at the data pointer position
		private int CountDigits()
			{
			int Ptr;
			for(Ptr = BarcodeDataPos; Ptr < BarcodeDataLength && BarcodeBinaryData[Ptr] >= '0' && BarcodeBinaryData[Ptr] <= '9'; Ptr++)
				;
			return Ptr - BarcodeDataPos;
			}

		// count ASCII text characters at the data pointer position
		private int CountText()
			{
			int DigitsCount = 0;
			int Ptr;
			for(Ptr = BarcodeDataPos; Ptr < BarcodeDataLength; Ptr++)
				{
				// current character
				int Chr = BarcodeBinaryData[Ptr];

				// not part of text subset
				if(Chr < ' ' && Chr != '\r' && Chr != '\n' && Chr != '\t' || Chr > '~') break;

				// not a digits
				if(Chr < '0' || Chr > '9')
					{
					DigitsCount = 0;
					continue;
					}

				// digit
				DigitsCount++;

				// we have less than 13 digits
				if(DigitsCount < 13) continue;

				// terminate text mode if there is a block of 13 digits
				Ptr -= 12;
				break;
				}

			// return textbytes
			return Ptr - BarcodeDataPos;
			}

		// count punctuation marks at the data pointer position
		private int CountPunctuation
				(
				int CurrentTextCount
				)
			{
			int Count = 0;
			while(CurrentTextCount > 0)
				{
				int NextChr = BarcodeBinaryData[BarcodeDataPos + Count];
				int NextCode = TextToPunct[NextChr];
				if(NextCode == 127) return 0;
				Count++;
				if(Count == 3) return 3;
				}
			return 0;
			}

		// count bytes at the data pointer position
		private int CountBytes()
			{
			int TextCount = 0;
			int Ptr;
			for(Ptr = BarcodeDataPos; Ptr < BarcodeDataLength; Ptr++)
				{
				// current character
				int Chr = BarcodeBinaryData[Ptr];

				// not part of text subset
				if(Chr < ' ' && Chr != '\r' && Chr != '\n' && Chr != '\t' || Chr > '~')
					{
					TextCount = 0;
					continue;
					}

				// update text count
				TextCount++;

				// we have less than 5 text characters
				if(TextCount < 5) continue;

				// terminate binary mode if there is a block of 5 text characters
				Ptr -= 4;
				break;
				}

			// the result must be at least 1
			return Math.Max(Ptr - BarcodeDataPos, 1);
			}

		// encode numeric data segment
		private void EncodeNumericSegment
				(
				int TotalCount
				)
			{
			// set numeric mode
			DataCodewords.Add(SwitchToNumericMode);
			_EncodingMode = EncodingMode.Numeric;

			while(TotalCount > 0)
				{
				// work in segment no more than 44 digits
				int SegCount = Math.Min(TotalCount, 44);

				// build a string with initial value 1
				StringBuilder SegStr = new StringBuilder("1");

				// add data digits
				for(int Index = 0; Index < SegCount; Index++)
					SegStr.Append((char) BarcodeBinaryData[BarcodeDataPos++]);

				// convert to big integer
				BigInteger Temp = BigInteger.Parse(SegStr.ToString());

				// find the highest factor
				int Fact;
				for(Fact = 0; Fact < 15 && Temp >= FactBigInt900[Fact]; Fact++);

				// convert to module 900
				for(int Index = Fact - 1; Index > 0; Index--)
					{
					DataCodewords.Add((int) BigInteger.DivRem(Temp, FactBigInt900[Index], out Temp));
					}
				DataCodewords.Add((int) Temp);

				// update total count
				TotalCount -= SegCount;
				}
			return;
			}

		// encode text segment
		private void EncodeTextSegment
				(
				int TotalCount
				)
			{
			// note first time this is the default
			if(_EncodingMode != EncodingMode.Text)
				{
				DataCodewords.Add(SwitchToTextMode);
				_EncodingMode = EncodingMode.Text;
				_TextEncodingMode = TextEncodingMode.Upper;
				}

			List<int> Temp = new List<int>();
			int Code;

			while(TotalCount > 0)
				{
				int Chr = BarcodeBinaryData[BarcodeDataPos++];
				TotalCount--;

				switch(_TextEncodingMode)
					{
					case TextEncodingMode.Upper:
						Code = TextToUpper[Chr];
						if(Code != 127)
							{
							Temp.Add(Code);
							continue;
							}
						Code = TextToLower[Chr];
						if(Code != 127)
							{
							Temp.Add(27); // Lower Latch
							Temp.Add(Code);
							_TextEncodingMode = TextEncodingMode.Lower;
							continue;
							}
						Code = TextToMixed[Chr];
						if(Code != 127)
							{
							Temp.Add(28); // Mixed Latch
							Temp.Add(Code);
							_TextEncodingMode = TextEncodingMode.Mixed;
							continue;
							}
						Code = TextToPunct[Chr];
						if(Code != 127)
							{
							// count how many more punctuations after this one
							int PunctCount = CountPunctuation(TotalCount);

							// if next character is panctuation too, we latch to punctuation
							if(PunctCount > 0)
								{
								Temp.Add(28); // mixed latch
								Temp.Add(25); // punctuation latch
								Temp.Add(Code);
								_TextEncodingMode = TextEncodingMode.Punct;
								continue;
								}

							// one to three punctuation marks at this point
							Temp.Add(29); // punctuation shift
							Temp.Add(Code);
							continue;
							}
						throw new ApplicationException("Program error: Text upper submode.");

					case TextEncodingMode.Lower:
						Code = TextToLower[Chr];
						if(Code != 127)
							{
							Temp.Add(Code);
							continue;
							}
						Code = TextToUpper[Chr];
						if(Code != 127)
							{
							Temp.Add(27); // upper shift
							Temp.Add(Code);
							continue;
							}
						Code = TextToMixed[Chr];
						if(Code != 127)
							{
							Temp.Add(28); // mixed Latch
							Temp.Add(Code);
							_TextEncodingMode = TextEncodingMode.Mixed;
							continue;
							}
						Code = TextToPunct[Chr];
						if(Code != 127)
							{
							// count how many more punctuations after this one
							int PunctCount = CountPunctuation(TotalCount);

							// if next character is panctuation too, we latch to punctuation
							if(PunctCount > 0)
								{
								Temp.Add(28); // mixed latch
								Temp.Add(25); // punctuation latch
								Temp.Add(Code);
								_TextEncodingMode = TextEncodingMode.Punct;
								continue;
								}

							// one to three punctuation marks at this point
							Temp.Add(29); // punctuation shift
							Temp.Add(Code);
							continue;
							}
						throw new ApplicationException("Program error: Text lower submode.");

					case TextEncodingMode.Mixed:
						Code = TextToMixed[Chr];
						if(Code != 127)
							{
							Temp.Add(Code);
							continue;
							}
						Code = TextToLower[Chr];
						if(Code != 127)
							{
							Temp.Add(27); // lower Latch
							Temp.Add(Code);
							_TextEncodingMode = TextEncodingMode.Lower;
							continue;
							}
						Code = TextToUpper[Chr];
						if(Code != 127)
							{
							Temp.Add(28); // upper latch
							Temp.Add(Code);
							_TextEncodingMode = TextEncodingMode.Upper;
							continue;
							}
						Code = TextToPunct[Chr];
						if(Code != 127)
							{
							// count how many more punctuations after this one
							int PunctCount = CountPunctuation(TotalCount);

							// if next character is panctuation too, we latch to punctuation
							if(PunctCount > 0)
								{
								Temp.Add(25); // punctuation latch
								Temp.Add(Code);
								_TextEncodingMode = TextEncodingMode.Punct;
								continue;
								}

							// single punctuation
							Temp.Add(29); // punctuation shift
							Temp.Add(Code);
							continue;
							}
						throw new ApplicationException("Program error: Text mixed submode.");

					case TextEncodingMode.Punct:
						Code = TextToPunct[Chr];
						if(Code != 127)
							{
							Temp.Add(Code);
							continue;
							}
						Temp.Add(29); // upper latch
						_TextEncodingMode = TextEncodingMode.Upper;
						goto case TextEncodingMode.Upper;
					}
				}

			// convert to codewords
			int TempEnd = Temp.Count & ~1;
			for(int Index = 0; Index < TempEnd; Index += 2)
				{
				DataCodewords.Add(30 * Temp[Index] + Temp[Index + 1]);
				}
			if((Temp.Count & 1) != 0)
				{
				DataCodewords.Add(30 * Temp[TempEnd] + 29);
				}
			return;
			}

		// encode byte segment
		private void EncodeByteSegment
				(
				int Count
				)
			{
			// special case one time shift
			if(Count == 1 && _EncodingMode == EncodingMode.Text)
				{
				DataCodewords.Add(ShiftToByteMode);
				DataCodewords.Add(BarcodeBinaryData[BarcodeDataPos++]);
				return;
				}

			// add shift to byte mode code
			DataCodewords.Add(Count % 6 == 0 ? SwitchToByteModeForSix : SwitchToByteMode);

			// set byte encoding mode
			_EncodingMode = EncodingMode.Byte;

			// end position
			int EndPos = BarcodeDataPos + Count;

			// encode six data bytes into five codewords
			if(Count >= 6)
				{
				while((EndPos - BarcodeDataPos) >= 6)
					{
					// load 6 data bytes into temp long integer
					long Temp = ((long) BarcodeBinaryData[BarcodeDataPos++] << 40) |
						((long) BarcodeBinaryData[BarcodeDataPos++] << 32) |
						((long) BarcodeBinaryData[BarcodeDataPos++] << 24) |
						((long) BarcodeBinaryData[BarcodeDataPos++] << 16) |
						((long) BarcodeBinaryData[BarcodeDataPos++] << 8) |
						BarcodeBinaryData[BarcodeDataPos++];

					// convert to 4 digits base 900 number
					for(int Index = 4; Index > 0; Index--)
						{
						DataCodewords.Add((int) Math.DivRem(Temp, Fact900[Index], out Temp));
						}

					// add the fifth one
					DataCodewords.Add((int) Temp);
					}
				}

			// encode the last 5 oe less bytes
			while(BarcodeDataPos < EndPos)
				{
				DataCodewords.Add(BarcodeBinaryData[BarcodeDataPos++]);
				}
			return;
			}

		// calculate error correction codewords
		private void CalculateErrorCorrection
				(
				int[] Codewords
				)
			{
			// shortcut for the selected error correction table
			int[] ErrCorrTable = ErrorCorrectionTables[(int) ErrorCorrectionLevel];

			// create empty error correction array
			ErrCorrCodewords = new int[ErrorCorrectionLength];

			// pointer to last error correction codeword
			int ErrorCorrectionEnd = ErrorCorrectionLength - 1;

			// do the magic polynomial divide
			int DataCodewordsLength = Codewords[0];
			for(int CWIndex = 0; CWIndex < DataCodewordsLength; CWIndex++)
				{
				int Temp = (Codewords[CWIndex] + ErrCorrCodewords[ErrorCorrectionEnd]) % MOD;
				for(int Index = ErrorCorrectionEnd; Index > 0; Index--)
					{
					ErrCorrCodewords[Index] = (MOD + ErrCorrCodewords[Index - 1] - Temp * ErrCorrTable[Index]) % MOD;
					}
				ErrCorrCodewords[0] = (MOD - Temp * ErrCorrTable[0]) % MOD;
				}

			// last step of the division
			for(int Index = ErrorCorrectionEnd; Index >= 0; Index--)
				{
				ErrCorrCodewords[Index] = (MOD - ErrCorrCodewords[Index]) % MOD;
				}

			// copy error codewords in reverse order
			for(int Index = 0; Index < ErrorCorrectionLength; Index++)
				Codewords[DataCodewordsLength + Index] = ErrCorrCodewords[ErrorCorrectionEnd - Index];

			// move error correction
			return;
			}

		// convert one codeword to barcode modules
		private static void CodewordToModules
				(
				int Row,
				int Col,
				int Codeword,
				bool[,] Matrix
				)
			{
			// leading black module
			Matrix[Row, Col] = true;

			// translate to modules
			int Modules = CodewordTable[Row % 3, Codeword];

			int Mask = 0x4000;
			for(int Index = 1; Index < ModulesInCodeword; Index++)
				{
				if((Modules & Mask) != 0) Matrix[Row, Col + Index] = true;
				Mask >>= 1;
				}
			return;
			}
		}
	}
