/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfTableStyle
//	Data table style support.
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
using System.Collections.Generic;
using System.Drawing;
using System.Text;
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
public class PdfTableStyle
	{
	/// <summary>
	/// Gets or sets content alignment.
	/// </summary>
	/// <remarks>
	/// Alignment property align the content within the client area of the cell.
	/// </remarks>
	public ContentAlignment Alignment {get; set;}

	/// <summary>
	/// Gets or sets background color.
	/// </summary>
	/// <remarks>
	/// If background color is not empty, the frame area of the cell will 
	/// be painted by this color. Default is Color.Empty.
	/// </remarks>
	public Color BackgroundColor {get; set;}

	/// <summary>
	/// Gets or sets barcode narrow bar width
	/// </summary>
	/// <remarks>
	/// The width of the bar code narrow bar.
	/// </remarks>
	public double BarcodeBarWidth {get; set;}

	/// <summary>
	/// Gets or sets barcode height
	/// </summary>
	/// <remarks>
	/// The height of the barcode excluding optional text.
	/// </remarks>
	public double BarcodeHeight {get; set;}

	/// <summary>
	/// Gets or sets first line indent for text box items.
	/// </summary>
	public double TextBoxFirstLineIndent {get; set;}

	/// <summary>
	/// Gets or sets text box line break factor.
	/// </summary>
	public double TextBoxLineBreakFactor {get; set;}

	/// <summary>
	/// Gets or sets extra line spacing for text box items.
	/// </summary>
	public double TextBoxLineExtraSpace {get; set;}

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
	public int TextBoxPageBreakLines {get; set;}

	/// <summary>
	/// Gets or sets extra paragraph spacing for text box items.
	/// </summary>
	public double TextBoxParagraphExtraSpace {get; set;}

	/// <summary>
	/// Gets or sets text justify within text box.
	/// </summary>
	public TextBoxJustify TextBoxTextJustify {get; set;}

	/// <summary>
	/// Gets or sets font.
	/// </summary>
	/// <remarks>
	/// If cell's value type is barcode, a null font signal no text under the barcode.
	/// </remarks>
	public PdfFont Font {get; set;}

	/// <summary>
	/// Gets or sets font size.
	/// </summary>
	public double FontSize {get; set;}

	/// <summary>
	/// Gets or sets foreground color.
	/// </summary>
	/// <remarks>
	/// Foreground color is used for text and Barcode.
	/// </remarks>
	public Color ForegroundColor {get; set;}

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
	public string Format {get; set;}

	/// <summary>
	/// Gets or sets cell's margins.
	/// </summary>
	public PdfRectangle Margin {get; set;}

	/// <summary>
	/// Gets or sets raise custom draw cell event flag.
	/// </summary>
	/// <remarks>
	/// With this flag you can control which columns call the draw cell event handler.
	/// </remarks>
	public bool RaiseCustomDrawCellEvent {get; set;}

	/// <summary>
	/// Gets or sets text draw style.
	/// </summary>
	public DrawStyle TextDrawStyle {get; set;}

	/// <summary>
	/// Gets or sets minimum cell height.
	/// </summary>
	public double MinHeight {get; set;}

	/// <summary>
	/// Gets or sets multi-line text flag.
	/// </summary>
	/// <remarks>
	/// String value will be converted to text box value.
	/// </remarks>
	public bool	MultiLineText {get; set;}

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
	public NumberFormatInfo NumberFormatInfo {get; set;}

	/// <summary>
	/// Gets font ascent for current font and font size.
	/// </summary>
	public double FontAscent
		{
		get
			{
			if(Font == null) throw new ApplicationException("PdfTableStyle: Font is not defined.");
			return Font.AscentPlusLeading(FontSize);
			}
		}

	/// <summary>
	/// Gets font descent for current font and font size.
	/// </summary>
	public double FontDescent
		{
		get
			{
			if(Font == null) throw new ApplicationException("PdfTableStyle: Font is not defined.");
			return Font.DescentPlusLeading(FontSize);
			}
		}

	/// <summary>
	/// Gets font line spacing for current font and font size.
	/// </summary>
	public double FontLineSpacing
		{
		get
			{
			if(Font == null) throw new ApplicationException("PdfTableStyle: Font is not defined.");
			return Font.LineSpacing(FontSize);
			}
		}

	/// <summary>
	/// PDF table style default constructor.
	/// </summary>
	/// <param name="Font">Font</param>
	public PdfTableStyle
			(
			PdfFont Font = null
			)
		{
		Alignment = ContentAlignment.TopLeft;
		BackgroundColor = Color.Empty;
		TextBoxLineBreakFactor = 0.5;
		TextBoxTextJustify = TextBoxJustify.Left;
		this.Font = Font;
		FontSize = 9.0;
		Margin = new PdfRectangle();
		ForegroundColor = Color.Black;
		TextDrawStyle = DrawStyle.Normal;
		return;
		}

	/// <summary>
	/// PDF table style constructor based on table's default cell style.
	/// </summary>
	/// <param name="Table">Table</param>
	public PdfTableStyle
			(
			PdfTable	Table
			)
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
			PdfTableStyle	Other
			)
		{
		Copy(Other);
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Copy one style to another 
	/// </summary>
	/// <param name="SourceStyle">Source style</param>
	////////////////////////////////////////////////////////////////////
	public void Copy
			(
			PdfTableStyle	SourceStyle
			)
		{
		Alignment = SourceStyle.Alignment;
		BackgroundColor = SourceStyle.BackgroundColor;
		BarcodeBarWidth = SourceStyle.BarcodeBarWidth;
		BarcodeHeight = SourceStyle.BarcodeHeight;
		Font = SourceStyle.Font;
		FontSize = SourceStyle.FontSize;
		ForegroundColor = SourceStyle.ForegroundColor;
		Format = SourceStyle.Format;
		Margin = new PdfRectangle(SourceStyle.Margin);
		MinHeight = SourceStyle.MinHeight;
		MultiLineText = SourceStyle.MultiLineText;
		NumberFormatInfo = SourceStyle.NumberFormatInfo;
		RaiseCustomDrawCellEvent = SourceStyle.RaiseCustomDrawCellEvent;
		TextBoxFirstLineIndent = SourceStyle.TextBoxFirstLineIndent;
		TextBoxLineBreakFactor = SourceStyle.TextBoxLineBreakFactor;
		TextBoxLineExtraSpace = SourceStyle.TextBoxLineExtraSpace;
		TextBoxPageBreakLines = SourceStyle.TextBoxPageBreakLines;
		TextBoxParagraphExtraSpace = SourceStyle.TextBoxParagraphExtraSpace;
		TextBoxTextJustify = SourceStyle.TextBoxTextJustify;
		TextDrawStyle = SourceStyle.TextDrawStyle;
		return;
		}
	}
}
