/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfTableCell
//	Data table cell support.
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
using System.Globalization;

namespace PdfFileWriter
{
////////////////////////////////////////////////////////////////////
/// <summary>
/// Cell type enumeration
/// </summary>
////////////////////////////////////////////////////////////////////
public enum CellType
	{
	/// <summary>
	/// Cell's value is null.
	/// </summary>
	Empty,

	/// <summary>
	/// Cell's value is String and Style.MultiLineText is false.
	/// </summary>
	Text,

	/// <summary>
	/// Cell's value is TextBox or String with Style.MultiLineText is true.
	/// </summary>
	TextBox,

	/// <summary>
	/// Cell's value is image.
	/// </summary>
	Image,

	/// <summary>
	/// Cell's value is barcode.
	/// </summary>
	Barcode,
	}

////////////////////////////////////////////////////////////////////
/// <summary>
/// PDF table cell class
/// </summary>
/// <remarks>
/// <para>
/// The PDF table cell class represent one cell in the table.
/// </para>
/// <para>
/// For more information go to <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#DataTableSupport">2.12 Data Table Support</a>
/// </para>
/// </remarks>
////////////////////////////////////////////////////////////////////
public class PdfTableCell
	{
	/// <summary>
	/// Gets cell's index position within Table.Cell array.
	/// </summary>
	/// <remarks>
	/// It is the cell's column number starting with zero.
	/// </remarks>
	public int Index {get; internal set;}

	/// <summary>
	/// Cell is a header.
	/// </summary>
	/// <remarks>
	/// If this property is true, the PdfTableCell is a header otherwise it is a cell.
	/// </remarks>
	public bool Header {get; internal set;}

	/// <summary>
	/// Gets or sets cell's style.
	/// </summary>
	/// <remarks>
	/// <para>
	/// If Style was not set by the caller, this value is the default cell style.
	/// Any change to the properties will affect all cells without cell style.
	/// </para>
	/// <para>
	/// If Style was set by the caller to a private style, this value is the private cell style.
	/// Any change to the properties will affect all other cells sharing this private cell style.
	/// </para>
	/// </remarks>
	public PdfTableStyle Style {get; set;}

	/// <summary>
	/// Gets cell's enumeration type.
	/// </summary>
	/// <remarks>
	/// <para>
	/// CellType will be Text for String and MultiLineText set to false plus all basic numeric values.
	/// </para>
	/// <para>
	/// CallType will be TextBox for String and MultiLineText set to true or Value set to TextBox.
	/// </para>
	/// <para>
	/// CallType will be set ti Image or Barcode if Value is set accordingly.
	/// </para>
	/// </remarks>
	public CellType Type {get; internal set;}

	/// <summary>
	/// Gets or sets cell's value
	/// </summary>
	/// <remarks>
	/// <para>
	/// Value can be set to String, basic numeric values, bool, Char, TextBox, PdfImage or Barcode.
	/// </para>
	/// <para>
	/// If value is set to String and MultiLineText is set to true, 
	/// the String will be converted to TextBox.
	/// </para>
	/// <para>
	/// All basic numeric values will be converted to String.
	/// </para>
	/// <para>
	/// Value will be reset to null after each row drawing.
	/// </para>
	/// </remarks>
	public object Value {get; set;}

	/// <summary>
	/// Gets cell's formatted value.
	/// </summary>
	/// <remarks>
	/// If Value is a numeric type, it is converted to formatted text
	/// using Value.ToString(Format, NumberFormat) method.
	/// </remarks>
	public string FormattedText {get; internal set;}

	/// <summary>
	/// Gets TextBox if Type is TextBox.
	/// </summary>
	/// <remarks>
	/// TextBox will be set if Value is a String and Style.MultiLine is true,
	/// or Value is a TextBox.
	/// </remarks>
	public TextBox TextBox {get; internal set;}

	/// <summary>
	/// Text box height including extra space
	/// </summary>
	/// <remarks>
	/// TextBoxHeight Value is calculated within DrawRow method. 
	/// It is valid for CustomDrawCellEvent.
	/// </remarks>
	public double TextBoxHeight {get; internal set;}

	/// <summary>
	/// Gets Image if Type is Image.
	/// </summary>
	/// <seealso cref="PdfTableCell.ImageWidth"/>
	/// <seealso cref="PdfTableCell.ImageHeight"/>
	/// <remarks>
	/// <para>
	///	If ImageWidth and ImageHeight were not set by the user,
	///	the image width will be set to ClientWidth and the height
	///	will be calculated to preserve image's aspect ratio.
	///	</para>
	///	<para>
	///	If ImageWidth was not set by the user and ImageHeight
	///	was set by the user. ImageWidth will be calculated to 
	///	preserve image's aspect ratio.
	///	</para>
	///	<para>
	///	If ImageWidth was set by the user and ImageHeight was
	///	not set by the user, ImageHeight will be calculated to 
	///	preserve image's aspect ratio.
	///	</para>
	/// <para>
	///	If both ImageWidth and ImageHeight were set by the user,
	///	the aspect ratio of the image will be ignored.
	///	</para>
	/// <para>
	/// If ImageWidth is wider than Client width, both ImageWidth
	/// and ImageHeight will be adjusted to fit the available width.
	/// </para>
	/// <para>
	/// ImageWidth and ImageHeight will be reset to zero after each row drawing.
	/// </para>
	/// </remarks>
	public PdfImage Image {get; internal set;}

	/// <summary>
	/// Gets or sets image width in user units.
	/// </summary>
	/// <seealso cref="PdfTableCell.Image"/>
	/// <remarks>
	/// Please note "Remarks" in Image property for description
	/// of ImageWidth and ImageHeight.
	/// </remarks>
	public double ImageWidth {get; set;}

	/// <summary>
	/// Gets or sets image height in user units.
	/// </summary>
	/// <seealso cref="PdfTableCell.Image"/>
	/// <remarks>
	/// Please note "Remarks" in Image property for description
	/// of ImageWidth and ImageHeight.
	/// </remarks>
	public double ImageHeight {get; set;}

	/// <summary>
	/// Gets barcode if type is Barcode
	/// </summary>
	public Barcode Barcode {get; internal set;}

	// total barcode plus text height
	private BarcodeBox BarcodeBox;

	/// <summary>
	/// Sets a web link for this cell.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The web ling string is converted to AnnotAction object.
	/// </para>
	/// </remarks>
	public string WebLink
		{
		set
			{
			AnnotAction = new AnnotWebLink(value);
			}
		}

	/// <summary>
	/// Gets or sets annotation action derived classes
	/// </summary>
	/// <remarks>
	/// <para>The user can activate the annotation action by clicking anywhere in the cell area.
	/// Right click for attached file.</para>
	/// <list type="table">
	/// <item><description>Weblink action to activate web browser.</description></item>
	/// <item><description>Go to action to jump to another page in the document.</description></item>
	/// <item><description>Display media action to isplay video or play sound.</description></item>
	/// <item><description>File attachment to save or view embedded file.</description></item>
	/// </list>
	/// </remarks>
	public AnnotAction AnnotAction {get; set;}

	/// <summary>
	/// Gets cell's frame left side (grid line).
	/// </summary>
	public double FrameLeft {get; internal set;}

	/// <summary>
	/// Gets cell's frame width (grid line to grid line).
	/// </summary>
	public double FrameWidth {get; internal set;}

	/// <summary>
	/// Gets client area left side.
	/// </summary>
	/// <remarks>
	/// ClientLeft is FrameLeft + Margin.Left.
	/// </remarks>
	public double ClientLeft {get; internal set;}

	/// <summary>
	/// Gets client area bottom side.
	/// </summary>
	/// <remarks>
	/// ClientBottom is Table.RowBottomPosition + Margin.Bottom
	/// </remarks>
	public double ClientBottom {get; internal set;}

	/// <summary>
	/// Gets client area right side.
	/// </summary>
	/// <remarks>
	/// ClientRight is FrameLeft + FrameWidth - Margin.Right.
	/// </remarks>
	public double ClientRight {get; internal set;}

	/// <summary>
	/// Gets client area top side.
	/// </summary>
	/// <remarks>
	/// ClientTop is Table.RowTopPosition - Margin.Top.
	/// </remarks>
	public double ClientTop {get; internal set;}

	/// <summary>
	/// Gets Client area width.
	/// </summary>
	/// <remarks>
	/// <para>
	/// ClientWidth is FrameWidth - Margin.Left - Margin.Right.
	/// </para>
	/// <para>
	/// Calling client width before initialization will force initialization.
	/// Table.PdfTableInitialization() method will be called.
	/// </para>
	/// </remarks>
	public double ClientWidth
		{
		get
			{
			if(FrameWidth == 0.0) Parent.PdfTableInitialization();
			return FrameWidth - Style.Margin.Left - Style.Margin.Right;
			}
		}

	/// <summary>
	/// Gets parent PdfTable.
	/// </summary>
	public PdfTable Parent {get; internal set;}

	internal double CellHeight;
	internal double TextBoxCellHeight;
	internal int TextBoxLineNo;

	// internal constructor
	// PdfTable creates two PdfTableCell arrays.
	internal PdfTableCell
			(
			PdfTable	Parent,
			int		Index,
			bool		Header
			)
		{
		this.Parent = Parent;
		this.Index = Index;
		this.Header = Header;
		Style = Header ? Parent.DefaultHeaderStyle : Parent.DefaultCellStyle;
		return;
		}

	/// <summary>
	/// Creates an empty text box with client width.
	/// </summary>
	/// <returns>Empty text box with client width.</returns>
	/// <remarks>
	/// <para>
	/// The newly created TextBox will have the correct client width.
	/// First line indent and line break factor will be taken from cell's style.
	/// </para>
	/// <para>
	/// CreateTextBox() method sets the Value property of this cell
	/// to the returned TextBox value;
	/// </para>
	/// </remarks>
	public TextBox CreateTextBox()
		{
		Value = new TextBox(ClientWidth, Style.TextBoxFirstLineIndent, Style.TextBoxLineBreakFactor);
		return (TextBox) Value;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Cell Initialization
	////////////////////////////////////////////////////////////////////
	internal void DrawCellInitialization()
		{
		// calculate left and right client space
		ClientLeft = FrameLeft + Style.Margin.Left;
		ClientRight = FrameLeft + FrameWidth - Style.Margin.Right;

		// initialize cell height to top and bottom margins
		CellHeight = Style.Margin.Top + Style.Margin.Bottom;

		// we have something to draw
		if(Value != null)
			{
			// assume cell type to be text
			Type = CellType.Text;

			// get object type
			Type ValueType = Value.GetType();

			// value is string
			if(ValueType == typeof(string))
				{
				// multi line text
				if(Style.MultiLineText)
					{
					// convert string to TextBox
					TextBox = new TextBox(ClientRight - ClientLeft, Style.TextBoxFirstLineIndent, Style.TextBoxLineBreakFactor);
					TextBox.AddText(Style.Font, Style.FontSize, Style.ForegroundColor, (string) Value);

					// textbox initialization
					TextBoxInitialization();
					}

				// single line text
				else
					{
					// save value as string
					FormattedText = (string) Value;

					// add line spacing
					CellHeight += Style.FontLineSpacing;
					}
				}
			
			// value is text box
			else if(ValueType == typeof(TextBox))
				{
				// set TextBox
				TextBox = (TextBox) Value;

				// test width
				if(TextBox.BoxWidth - (ClientRight - ClientLeft) > Parent.Epsilon) throw new ApplicationException("PdfTableCell: TextBox width is greater than column width");

				// textbox initialization
				TextBoxInitialization();
				}

			// value is PdfImage
			else if(ValueType == typeof(PdfImage))
				{
				// set image
				Image = (PdfImage) Value;

				// calculate client width
				double Width = ClientWidth;

				// calculate image width and height
				if(ImageWidth == 0.0)
					{
					if(ImageHeight == 0.0)
						{
						ImageWidth = Width;
						ImageHeight = ImageWidth * (double) Image.HeightPix / (double) Image.WidthPix;
						}
					else
						{
						ImageWidth = ImageHeight * (double) Image.WidthPix / (double) Image.HeightPix;
						}
					}
				else if(ImageHeight == 0.0)
					{
					ImageHeight = ImageWidth * (double) Image.HeightPix / (double) Image.WidthPix;
					}

				// image width is too wide
				if(ImageWidth > Width)
					{
					ImageHeight = Width * ImageHeight / ImageWidth;
					ImageWidth = Width;
					}

				// adjust cell's height
				CellHeight += ImageHeight;

				// set type to image
				Type = CellType.Image;
				}

			// value is a derived class of barcode
			else if(ValueType.BaseType == typeof(Barcode))
				{
				// set barcode
				Barcode = (Barcode) Value;

				// test barcode height
				if(Style.BarcodeHeight <= 0.0) throw new ApplicationException("PdfTableStyle: BarcodeHeight must be defined.");

				// calculate total barcode height
				BarcodeBox = Barcode.GetBarcodeBox(Style.BarcodeBarWidth, Style.BarcodeHeight, Style.Font, Style.FontSize);

				// adjust cell's height
				CellHeight += BarcodeBox.TotalHeight;

				// set type to barcode
				Type = CellType.Barcode;
				}

			// value is basic mostly numeric object
			else
				{
				string Format = Style.Format;
				NumberFormatInfo NumberFormat = Style.NumberFormatInfo;
				if(ValueType == typeof(int)) FormattedText = ((int) Value).ToString(Format, NumberFormat);
				else if(ValueType == typeof(float)) FormattedText = ((float) Value).ToString(Format, NumberFormat);
				else if(ValueType == typeof(double)) FormattedText = ((double) Value).ToString(Format, NumberFormat);
				else if(ValueType == typeof(bool)) FormattedText = ((bool) Value).ToString();
				else if(ValueType == typeof(char)) FormattedText = ((char) Value).ToString();
				else if(ValueType == typeof(byte)) FormattedText = ((byte) Value).ToString(Format, NumberFormat);
				else if(ValueType == typeof(sbyte)) FormattedText = ((sbyte) Value).ToString(Format, NumberFormat);
				else if(ValueType == typeof(short)) FormattedText = ((short) Value).ToString(Format, NumberFormat);
				else if(ValueType == typeof(ushort)) FormattedText = ((ushort) Value).ToString(Format, NumberFormat);
				else if(ValueType == typeof(uint)) FormattedText = ((uint) Value).ToString(Format, NumberFormat);
				else if(ValueType == typeof(long)) FormattedText = ((long) Value).ToString(Format, NumberFormat);
				else if(ValueType == typeof(ulong)) FormattedText = ((ulong) Value).ToString(Format, NumberFormat);
				else if(ValueType == typeof(decimal)) FormattedText = ((decimal) Value).ToString(Format, NumberFormat);
				else if(ValueType == typeof(DBNull)) FormattedText = string.Empty;
				else throw new ApplicationException("PdfTableCell: Unknown object type");

				// add line spacing
				CellHeight += Style.FontLineSpacing;
				}
			}

		// value is null and textbox continuation is required
		else if(Type == CellType.TextBox && TextBoxLineNo != 0)
				{
				CellHeight += TextBox.BoxHeightExtra(ref TextBoxLineNo, out int LineEnd,
					Parent._RowTopPosition - Parent.TableBottomLimit - (Style.Margin.Top + Style.Margin.Bottom),
						Style.TextBoxLineExtraSpace, Style.TextBoxParagraphExtraSpace);
				TextBoxCellHeight = CellHeight;
				}
		else
			{
			// reset cell type
			Type = CellType.Empty;
			}

		// test for minimum height requirement
		if(CellHeight < Style.MinHeight) CellHeight = Style.MinHeight;

		// cell minimum height for all types but textbox
		if(Type != CellType.TextBox) TextBoxCellHeight = CellHeight;

		// return result
		return;
		}

	internal void TextBoxInitialization()
		{
		// terminate TextBox
		TextBox.Terminate();

		// calculate overall TextBox height
		TextBoxHeight = TextBox.BoxHeightExtra(Style.TextBoxLineExtraSpace, Style.TextBoxParagraphExtraSpace);
		double TextBoxHeightPageBreak = TextBoxHeight;

		// textbox minimum height for page break calculations
		if(!Header && (Style.TextBoxPageBreakLines != 0 && Style.TextBoxPageBreakLines < TextBox.LineCount))
			{ 
			// calculate TextBox height and add to cell height
			TextBoxHeightPageBreak = TextBox.BoxHeightExtra(Style.TextBoxPageBreakLines, Style.TextBoxLineExtraSpace, Style.TextBoxParagraphExtraSpace);
			}

		// required cell height for page break calculations and for full textbox height
		TextBoxCellHeight = CellHeight + TextBoxHeightPageBreak;
		CellHeight += TextBoxHeight;

		// reset textbox line number
		TextBoxLineNo = 0;

		// set type to text box
		Type = CellType.TextBox;
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Cell
	////////////////////////////////////////////////////////////////////

	internal void DrawCell()
		{
		// draw background color
		if(Style.BackgroundColor != Color.Empty)
			{
			Parent.Contents.SaveGraphicsState();
			Parent.Contents.SetColorNonStroking(Style.BackgroundColor); 
			Parent.Contents.DrawRectangle(Parent._ColumnPosition[Index], Parent.RowBottomPosition,
				Parent._ColumnWidth[Index], Header ? Parent.HeaderHeight : Parent.RowHeight, PaintOp.Fill);
			Parent.Contents.RestoreGraphicsState();
			}

		// switch based on cell type
		switch(Type)
			{
			// one line of text
			case CellType.Text:
				TextJustify TextJustify;
				Parent.Contents.DrawText(Style.Font, Style.FontSize, TextHorPos(out TextJustify), TextVerPos(),
					TextJustify, Style.TextDrawStyle, Style.ForegroundColor, FormattedText);
				break;

			// text box
			case CellType.TextBox:
				// calculate textbox size and position
				int LineEnd;
				if(TextBoxLineNo != 0 || TextBoxHeight > ClientTop - ClientBottom + Parent.Epsilon)
					TextBoxHeight = TextBox.BoxHeightExtra(ref TextBoxLineNo, out LineEnd,
						ClientTop - ClientBottom + Parent.Epsilon, Style.TextBoxLineExtraSpace, Style.TextBoxParagraphExtraSpace);
				double YPos = TopPos(TextBoxHeight);

				// draw textbox
				Parent.Contents.SaveGraphicsState();
				LineEnd = Parent.Contents.DrawText(LeftPos(TextBox.BoxWidth), ref YPos, ClientBottom - Parent.Epsilon, TextBoxLineNo,
					Style.TextBoxLineExtraSpace, Style.TextBoxParagraphExtraSpace, Style.TextBoxTextJustify, TextBox);
				Parent.Contents.RestoreGraphicsState();

				// textbox did not fit in current page
				if(LineEnd < TextBox.LineCount)
					{
					TextBoxLineNo = LineEnd;
					Parent.TextBoxContinue = true;
					}
				// textbox drawing is done
				else
					{
					TextBoxLineNo = 0;
					}
				break;

			// image
			case CellType.Image:
				Parent.Contents.DrawImage(Image, LeftPos(ImageWidth), TopPos(ImageHeight) - ImageHeight, ImageWidth, ImageHeight);
				break;

			// barcode
			case CellType.Barcode:
				Parent.Contents.DrawBarcode(LeftPos(BarcodeBox.TotalWidth) + BarcodeBox.OriginX,
					TopPos(BarcodeBox.TotalHeight) - BarcodeBox.TotalHeight + BarcodeBox.OriginY,
						TextJustify.Left, Style.BarcodeBarWidth, Style.BarcodeHeight, Style.ForegroundColor, Barcode, Style.Font, Style.FontSize);;
				break;
			}

		// cell has a web link
		if(AnnotAction != null) Parent.Page.AddAnnotation(new PdfRectangle(ClientLeft, ClientBottom, ClientRight, ClientTop), AnnotAction);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Calculate text horizontal position
	////////////////////////////////////////////////////////////////////
	private double TextHorPos
			(
			out TextJustify Justify
			)
		{
		if((Style.Alignment & (ContentAlignment.TopLeft | ContentAlignment.MiddleLeft | ContentAlignment.BottomLeft)) != 0)
			{
			Justify = TextJustify.Left;
			return ClientLeft;
			}
		if((Style.Alignment & (ContentAlignment.TopRight | ContentAlignment.MiddleRight | ContentAlignment.BottomRight)) != 0)
			{
			Justify = TextJustify.Right;
			return ClientRight;
			}
		if((Style.Alignment & (ContentAlignment.TopCenter | ContentAlignment.MiddleCenter | ContentAlignment.BottomCenter)) != 0)
			{
			Justify = TextJustify.Center;
			return 0.5 * (ClientLeft + ClientRight);
			}
		Justify = TextJustify.Left;
		return ClientLeft;
		}

	////////////////////////////////////////////////////////////////////
	// Calculate left side
	////////////////////////////////////////////////////////////////////
	private double LeftPos
			(
			double	Width
			)
		{
		if((Style.Alignment & (ContentAlignment.TopLeft | ContentAlignment.MiddleLeft | ContentAlignment.BottomLeft)) != 0)
				return ClientLeft;
		if((Style.Alignment & (ContentAlignment.TopCenter | ContentAlignment.MiddleCenter | ContentAlignment.BottomCenter)) != 0)
				return 0.5 * (ClientLeft + ClientRight - Width);
		if((Style.Alignment & (ContentAlignment.TopRight | ContentAlignment.MiddleRight | ContentAlignment.BottomRight)) != 0)
				return ClientRight - Width;
		return ClientLeft;
		}

	////////////////////////////////////////////////////////////////////
	// Calculate text vertical position
	////////////////////////////////////////////////////////////////////
	private double TextVerPos()
		{
		if((Style.Alignment & (ContentAlignment.TopLeft | ContentAlignment.TopCenter | ContentAlignment.TopRight)) != 0)
				return ClientTop - Style.FontAscent;
		if((Style.Alignment & (ContentAlignment.BottomLeft | ContentAlignment.BottomCenter | ContentAlignment.BottomRight)) != 0)
				return ClientBottom + Style.FontDescent;
		if((Style.Alignment & (ContentAlignment.MiddleLeft | ContentAlignment.MiddleCenter | ContentAlignment.MiddleRight)) != 0)
				return 0.5 * (ClientTop + ClientBottom - Style.FontAscent + Style.FontDescent);
		return ClientTop - Style.FontAscent;
		}

	////////////////////////////////////////////////////////////////////
	// Calculate top side
	////////////////////////////////////////////////////////////////////
	private double TopPos
			(
			double	Height
			)
		{
		if((Style.Alignment & (ContentAlignment.TopLeft | ContentAlignment.TopCenter | ContentAlignment.TopRight)) != 0)
				return ClientTop;
		if((Style.Alignment & (ContentAlignment.MiddleLeft | ContentAlignment.MiddleCenter | ContentAlignment.MiddleRight)) != 0)
				return 0.5 * (ClientTop + ClientBottom + Height);
		if((Style.Alignment & (ContentAlignment.BottomLeft | ContentAlignment.BottomCenter | ContentAlignment.BottomRight)) != 0)
				return ClientBottom + Height;
		return ClientTop;
		}

	////////////////////////////////////////////////////////////////////
	// Reset cell after the current row was drawn
	////////////////////////////////////////////////////////////////////
	internal void Reset()
		{
		Value = null;
		AnnotAction = null;
		ImageWidth = 0.0;
		ImageHeight = 0.0;
		return;
		}
	}
}
