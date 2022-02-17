/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PdfTableStyle
//	Data table style support.
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

using System.Globalization;

namespace PdfFileWriter
	{
	/// <summary>
	/// PDF table cell or header style class
	/// </summary>
	/// <remarks>
	/// <para>
	/// For more information go to <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#DataTableSupport">2.12 Data Table Support</a>
	/// </para>
	/// </remarks>
	public class PdfTableStyle : PdfDrawTextCtrl
		{
		/// <summary>
		/// Gets or sets content alignment.
		/// </summary>
		/// <remarks>
		/// Alignment property align the content within the client area of the cell.
		/// </remarks>
		public ContentAlignment Alignment { get; set; }

		/// <summary>
		/// Gets or sets background color.
		/// </summary>
		/// <remarks>
		/// If background color is not empty, the frame area of the cell will 
		/// be painted by this color. Default is Color.Empty.
		/// </remarks>
		public Color BackgroundColor { get; set; }

		/// <summary>
		/// Draw barcode control
		/// </summary>
		/// <remarks>
		/// Contains the width of the bar code narrow bar.
		/// and the height of the barcode
		/// </remarks>
		public PdfDrawBarcodeCtrl BarcodeCtrl;

		/// <summary>
		/// Gets or sets first line indent for text box items.
		/// </summary>
		public double TextBoxFirstLineIndent { get; set; }

		/// <summary>
		/// Gets or sets text box line break factor.
		/// </summary>
		public double TextBoxLineBreakFactor { get; set; }

		/// <summary>
		/// Gets or sets extra line spacing for text box items.
		/// </summary>
		public double TextBoxLineExtraSpace { get; set; }

		/// <summary>
		/// Gets or sets minimum text lines for page break calculations.
		/// </summary>
		/// <remarks>
		/// If TextBoxPageBreakLines is zero, the software will keep
		/// all of the TextBox together. If the TextBox height is too
		/// big to fit in the table, an exception will be raised. If
		/// TextBoxPageBreakLines is not zero and TextBox height is too
		/// big, the height of TextBoxPageBreakLines will be used
		/// to start a new page. The remaining lines will be printed
		/// on the next page or pages.
		/// </remarks>
		public int TextBoxPageBreakLines { get; set; }

		/// <summary>
		/// Gets or sets extra paragraph spacing for text box items.
		/// </summary>
		public double TextBoxParagraphExtraSpace { get; set; }

		/// <summary>
		/// Gets or sets text justify within text box.
		/// </summary>
		public TextBoxJustify TextBoxTextJustify { get; set; }

		/// <summary>
		/// Gets or sets format string.
		/// </summary>
		/// <remarks>
		/// <para>
		/// All basic numeric values are converted to string using: 
		/// </para>
		/// <code>
		/// Value.ToString(Format, NumberFormatInfo);
		/// </code>
		/// <para>
		/// The NumberFormatInfo allows for regional formatting.
		/// </para>
		/// <para>
		/// Both Format and NumberFormatInfo are set to null by default.
		/// In other words by default the conversion is:
		/// </para>
		/// <code>
		/// Value.ToString();
		/// </code>
		/// </remarks>
		public string Format { get; set; }

		/// <summary>
		/// Gets or sets cell's margins.
		/// </summary>
		public PdfRectangle Margin { get; set; }

		/// <summary>
		/// Gets or sets raise custom draw cell event flag.
		/// </summary>
		/// <remarks>
		/// With this flag you can control which columns call the draw cell event handler.
		/// </remarks>
		public bool RaiseCustomDrawCellEvent { get; set; }

		/// <summary>
		/// Gets or sets minimum cell height.
		/// </summary>
		public double MinHeight { get; set; }

		/// <summary>
		/// Gets or sets multi-line text flag.
		/// </summary>
		/// <remarks>
		/// String value will be converted to text box value.
		/// </remarks>
		public bool MultiLineText { get; set; }

		/// <summary>
		/// Gets or sets number format information.
		/// </summary>
		/// <remarks>
		/// <para>
		/// All basic numeric values are converted to string using: 
		/// </para>
		/// <code>
		/// Value.ToString(Format, NumberFormatInfo);
		/// </code>
		/// <para>
		/// The NumberFormatInfo allows for regional formatting.
		/// </para>
		/// <para>
		/// Both Format and NumberFormatInfo are set to null by default.
		/// In other words by default the conversion is:
		/// </para>
		/// <code>
		/// Value.ToString();
		/// </code>
		/// </remarks>
		public NumberFormatInfo NumberFormatInfo { get; set; }

		/// <summary>
		/// PDF table style default constructor.
		/// </summary>
		/// <param name="TextCtrl">Font</param>
		public PdfTableStyle
				(
				PdfDrawTextCtrl TextCtrl
				) : base(TextCtrl)
			{
			Alignment = ContentAlignment.TopLeft;
			BackgroundColor = Color.Empty;
			TextBoxLineBreakFactor = 0.5;
			TextBoxTextJustify = TextBoxJustify.Left;
			Margin = new PdfRectangle();
			return;
			}

		/// <summary>
		/// PDF table style constructor based on table's default cell style.
		/// </summary>
		/// <param name="Table">Table</param>
		public PdfTableStyle
				(
				PdfTable Table
				) : base(Table.DefaultCellStyle)
			{
			Copy(Table.DefaultCellStyle);
			return;
			}

		/// <summary>
		/// PDF table style constructor as a copy of another style.
		/// </summary>
		/// <param name="Other">Copy constructor.</param>
		public PdfTableStyle
				(
				PdfTableStyle Other
				) : base(Other)
			{
			Copy(Other);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Copy one style to another 
		/// </summary>
		/// <param name="Other">Source style</param>
		////////////////////////////////////////////////////////////////////
		public void Copy
				(
				PdfTableStyle Other
				)
			{
			Alignment = Other.Alignment;
			BackgroundColor = Other.BackgroundColor;
			BarcodeCtrl = Other.BarcodeCtrl;
			Format = Other.Format;
			Margin = new PdfRectangle(Other.Margin);
			MinHeight = Other.MinHeight;
			MultiLineText = Other.MultiLineText;
			NumberFormatInfo = Other.NumberFormatInfo;
			RaiseCustomDrawCellEvent = Other.RaiseCustomDrawCellEvent;
			TextBoxFirstLineIndent = Other.TextBoxFirstLineIndent;
			TextBoxLineBreakFactor = Other.TextBoxLineBreakFactor;
			TextBoxLineExtraSpace = Other.TextBoxLineExtraSpace;
			TextBoxPageBreakLines = Other.TextBoxPageBreakLines;
			TextBoxParagraphExtraSpace = Other.TextBoxParagraphExtraSpace;
			TextBoxTextJustify = Other.TextBoxTextJustify;
			return;
			}
		}
	}
