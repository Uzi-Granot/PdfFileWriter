/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfTable
//	Data table support.
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

namespace PdfFileWriter
{
/// <summary>
/// PDF table start event handler delegate
/// </summary>
/// <param name="Table">Parent object.</param>
/// <param name="TableStartPos">Table top's position on the page.</param>
/// <remarks>
/// The purpose of the table start event handler is to allow custom 
/// drawing on the page before the header and the first row are drawn. 
///	For example, drawing a title above the table.
///	</remarks>
public delegate void PdfTableStart(PdfTable Table, double TableStartPos);

/// <summary>
/// PDF table end event handler delegate
/// </summary>
/// <param name="Table">Parent object.</param>
/// <param name="TableEndPos">Table bottom's position on the page.</param>
/// <remarks>
/// The purpose of the table end event handler is to allow custom 
/// drawing on the page after the last row was drawn. 
///	For example, drawing comment below the table.
///	</remarks>
public delegate void PdfTableEnd(PdfTable Table, double TableEndPos);

/// <summary>
/// PDF table custom draw cell event handler delegate
/// </summary>
/// <param name="Table">The current table object.</param>
/// <param name="Cell">The current's cell object.</param>
/// <returns>Action taken (see remarks).</returns>
/// <remarks>
/// <para>True if the event handler drew the cell.</para>
/// <para>False if the event handler did not draw the cell.</para>
/// </remarks>
public delegate bool PdfTableCustomDrawCell(PdfTable Table, PdfTableCell Cell);

/// <summary>
/// PDF data table drawing class
/// </summary>
/// <remarks>
/// <para>
/// The main class for drawing a data table within a PDF document.
/// </para>
/// <para>
/// For more information go to <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#DataTableSupport">2.12 Data Table Support</a>
/// </para>
/// <para>
/// <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#DrawDataTable">For example of drawing image see 3.13. Draw Book Order Form</a>
/// </para>
/// </remarks>
public class PdfTable
	{
	/// <summary>
	/// Gets the table is active flag.
	/// </summary>
	/// <remarks>
	/// The active flag is set by the PdfTableInitialization method.
	/// </remarks>
	public bool Active {get; internal set;}

	/// <summary>
	/// Gets array of cell items.
	/// </summary>
	/// <remarks>
	/// SetColumnWidth method creates the Cell array. 
	/// Cell array length is Columns. Each cell controls the drawing of one column.
	/// </remarks>
	public PdfTableCell[] Cell
		{
		get
			{
			return(_Cell);
			}
		}
	internal PdfTableCell[] _Cell;

	/// <summary>
	/// Creates a cell style object as a copy of the default cell style.
	/// </summary>
	public PdfTableStyle CellStyle
		{
		get
			{
			return(new PdfTableStyle(DefaultCellStyle));
			}
		}

	/// <summary>
	/// Gets array of column positions.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Column position is the centre of a border line or the centre of a grid line. 
	/// </para>
	/// <para>
	/// Position[0] is the left side of the table, and Position[Columns] is the right side of the table.
	/// </para>
	/// <para>
	/// The returned array is a copy of the internal array.
	/// </para>
	/// </remarks>
	public double[] ColumnPosition
		{
		get
			{
			return((double[]) _ColumnPosition.Clone());
			}
		}
	internal double[] _ColumnPosition;

	/// <summary>
	/// Gets the number of columns in the table.
	/// </summary>
	/// <value>The number of columns in the table.</value>
	/// <remarks>SetColumnWidth method sets this value.</remarks>
	public int Columns {get; internal set;}

	/// <summary>
	/// Gets array of column widths.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Column width is from the center of border line or a grid line
	/// to the center of the next border line or grid line. 
	/// </para>
	/// <para>
	/// The returned array is a copy of the internal array.
	/// </para>
	/// </remarks>
	public double[] ColumnWidth
		{
		get
			{
			return((double[]) _ColumnWidth.Clone());
			}
		}
	internal double[] _ColumnWidth;

	/// <summary>
	/// Commit page contents to PDF file
	/// </summary>
	public bool CommitToPdfFile { get; set;}

	/// <summary>
	/// Call GC collect frequency
	/// </summary>
	/// <remarks>If zero or negative GC will not be called.</remarks>
	public int CommitGCCollectFreq { get; set;}

	/// <summary>
	/// Gets the current PDF contents object.
	/// </summary>
	/// <value>The current PDF contents object.</value>
	/// <remarks>
	/// The initial value is set by the constructor. 
	/// The value is updated as each new page is added.
	/// </remarks>
	public PdfContents Contents {get; internal set;}

	/// <summary>
	/// Gets the default cell style.
	/// </summary>
	/// <value>Default cell style.</value>
	/// <remarks>
	/// <para>
	/// The default cell style is used by cells without private style.
	/// </para>
	/// <para>
	/// The returned value is the default cell style object.
	/// A change to any of the syle properties will affect all cells
	/// without private style.
	/// </para>
	/// </remarks>
	public PdfTableStyle DefaultCellStyle {get; internal set;}

	/// <summary>
	/// Gets the default header style.
	/// </summary>
	/// <value>Default header style.</value>
	/// <remarks>
	/// <para>
	/// The default header style is used by headers without private style.
	/// </para>
	/// <para>
	/// The returned value is the default header style object.
	/// A change to any of the syle properties will affect all headers
	/// without private style.
	/// </para>
	/// </remarks>
	public PdfTableStyle DefaultHeaderStyle {get; internal set;}

	/// <summary>
	/// Gets the PDF document that owns this table.
	/// </summary>
	/// <valuw>PDF document the parent of this table.</valuw>
	public PdfDocument Document {get; internal set;}

	/// <summary>
	/// Gets array of header items.
	/// </summary>
	/// <remarks>
	/// SetColumnWidth method creates Header array. Array length is Columns.
	/// Each header controls the drawing of one column's header.
	/// </remarks>
	public PdfTableCell[] Header
		{
		get
			{
			return(_Header);
			}
		}
	internal PdfTableCell[] _Header;

	/// <summary>
	/// Gets the current header height.
	/// </summary>
	/// <value>Current header height.</value>
	/// <remarks>
	/// Header height is calculated once for each page.
	/// It is the height of the tallest header.
	/// </remarks>
	public double HeaderHeight {get; internal set;}

	/// <summary>
	/// Gets or sets the flag controlling the drawing of the header row.
	/// </summary>
	/// <remarks>
	/// If the flag is true (the default), the header is drawn on each page. 
	/// If the flag is false, the header is drawn one time only at the start of the table.
	/// </remarks>
	public bool HeaderOnEachPage {get; set;}

	/// <summary>
	/// Creates a header style object as a copy of the default header style.
	/// </summary>
	public PdfTableStyle HeaderStyle
		{
		get
			{
			return(new PdfTableStyle(DefaultHeaderStyle));
			}
		}

	/// <summary>
	/// Gets or sets minimum row height.
	/// </summary>
	/// <value>Minimum row height.</value>
	public double MinRowHeight {get; set;}

	/// <summary>
	/// Gets or sets minimum row height.
	/// </summary>
	/// <value>Minimum header height.</value>
	public double MinHeaderHeight {get; set;}

	/// <summary>
	/// Gets the current PDF page object.
	/// </summary>
	/// <value>The current PDF page object.</value>
	/// <remarks>
	/// The initial value is set by the constructor. 
	/// The value is updated as each new page is added.
	/// </remarks>
	public PdfPage Page {get; internal set;}

	/// <summary>
	/// Table's page number starting with zero
	/// </summary>
	public int TablePageNumber {get; internal set;}

	/// <summary>
	/// Gets the current row bottom position.
	/// </summary>
	/// <value>Row bottom position.</value>
	/// <remarks>
	/// Row bottom position is calculated for each row. It is RowTopPosition plus RowHeight. 
	/// The calculation is done within DrawRow method before any of the cells is drawn.
	/// </remarks>
	public double RowBottomPosition {get; internal set;}

	/// <summary>
	/// Gets the current row height.
	/// </summary>
	/// <value>Current row height.</value>
	/// <remarks>
	/// Row height is calculated for each row. It is the height of the tallest cell. 
	/// The calculation is done within DrawRow method before any of the cells is drawn.
	/// </remarks>
	public double RowHeight {get; internal set;}

	/// <summary>
	/// Gets the current row number.
	/// </summary>
	/// <value>Row number starting with zero.</value>
	public int RowNumber {get; internal set;}

	/// <summary>
	/// Gets array of row positions.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Row position is the centre of a border line or the centre of a grid line. 
	/// </para>
	/// <para>
	/// Position[0] is the top side of the table, and Position[Rows] is the bottom side of the table.
	/// </para>
	/// <para>
	/// The returned array is valid after the last row on the page is processed.
	/// </para>
	/// </remarks>
	public double[] RowPosition
		{
		get
			{
			return((double[]) BorderYPos.ToArray());
			}
		}

	/// <summary>
	/// Gets or sets current row top position.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Row top position can only be set during initialization.
	/// It should be set by the caller if on the first page the 
	/// table does not start at the top of the page.
	/// </para>
	/// <para>
	/// Row top position is calculated for each row. It is the last RowBottomPosition plus GridLineWidth. 
	/// The calculation is done within DrawRow method before any of the cells is drawn.
	/// </para>
	/// </remarks>
	public double RowTopPosition
		{
		get
			{
			return(_RowTopPosition);
			}
		set
			{
			if(Active) throw new ApplicationException("PdfTable: Row position must be defined at initialization.");
			_RowTopPosition = value;
			}
		}
	internal double _RowTopPosition;

	/// <summary>
	/// Gets or sets table area rectangle.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Table area rectangle can only be set at initialization time. 
	/// </para>
	/// <para>
	/// The default table area is the default paper size less 1 inch margin. 
	/// </para>
	/// <para>
	/// Returned value is a copy of the internal rectangle.
	/// </para>
	/// </remarks>
	public PdfRectangle TableArea
		{
		get
			{
			return(new PdfRectangle(_TableArea));
			}
		set
			{
			if(Active) throw new ApplicationException("PdfTable: Table area must be defined at initialization.");
			_TableArea = value;
			}
		}
	internal PdfRectangle _TableArea;

	/// <summary>
	/// Borders control
	/// </summary>
	public PdfTableBorder Borders {get; internal set;}

	/// <summary>
	/// Sets the custom draw cell and header event handler.
	/// </summary>
	/// <remarks>
	/// The custom draw cell and header event handler is called each time a 
	/// cell or header is about to be drawn and RaiseCustomDrawCellEvent 
	/// property of cell style is set to true;
	/// </remarks>
	public event PdfTableCustomDrawCell CustomDrawCellEvent;

	/// <summary>
	/// Sets the table end event handler.
	/// </summary>
	/// <remarks>
	/// The table end event handler is called at the end of each page and at the end of the table drawing.
	/// </remarks>
	public event PdfTableEnd TableEndEvent;

	/// <summary>
	/// Sets the table start event handler.
	/// </summary>
	/// <remarks>
	/// The table start event handler is called at the start of the table and at the top each subsequent page.
	/// <code>
	/// // example of table start event handler to display
	/// // heading above the table on each page
	/// void BookListTableStart
	///		(
	///		PdfTable	BookList,
	///		double		TableStartPos
	///		)
	///	{
	///	double PosX = 0.5 * (BookList.TableArea.Left + BookList.TableArea.Right);
	///	double PosY = TableStartPos + TableTitleFont.Descent(16.0) + 0.05;
	///	BookList.Contents.DrawText(TableTitleFont, 16.0, PosX, PosY,
	///		TextJustify.Center, DrawStyle.Normal, Color.Chocolate, "Book List PdfTable Example"); 
	///	return;
	///	}
	///	</code>
	/// </remarks>
	public event PdfTableStart TableStartEvent;

	// internal members
	internal double			BorderLeftPos;
	internal double			BorderRightPos;
	internal List<double>	BorderYPos;
	internal bool		BorderHeaderActive;
	internal double			BorderRowTopPos;
	internal double			TableTopLimit;
	internal double			TableBottomLimit;
	internal double			TextBoxRowHeight;
	internal bool		TextBoxContinue;
	internal bool		DisplayHeader;
	internal bool		DrawingActive;
	internal double			Epsilon;

	/// <summary>
	/// PdfTable constructor.
	/// </summary>
	/// <param name="Page">Current PdfPage.</param>
	/// <param name="Contents">Current PdfContents.</param>
	/// <param name="Font">Table's default font.</param>
	/// <param name="FontSize">Table's default font size.</param>
	public PdfTable
			(
			PdfPage		Page,
			PdfContents	Contents,
			PdfFont		Font = null,
			double		FontSize = 9.0
			)
		{
		// save arguments
		this.Document = Page.Document;
		this.Page = Page;
		this.Contents = Contents;

		// See if at least one font is defined. Make it the default font for the table
		if(Font == null) foreach(PdfObject Obj in Document.ObjectArray) if(Obj.GetType() == typeof(PdfFont))
			{
			Font = (PdfFont) Obj;
			break;
			}

		// initialize default cell style
		DefaultCellStyle = new PdfTableStyle();
		DefaultCellStyle.Font = Font;
		DefaultCellStyle.FontSize = FontSize;
		DefaultCellStyle.Margin.Left = DefaultCellStyle.Margin.Right = 3.0 / Document.ScaleFactor;
		DefaultCellStyle.Margin.Bottom = DefaultCellStyle.Margin.Top = 1.0 / Document.ScaleFactor;

		// initialize default header style
		DefaultHeaderStyle = CellStyle;
		DefaultHeaderStyle.BackgroundColor = Color.LightGray;

		// default table area
		TableArea = new PdfRectangle(72.0 / Page.ScaleFactor, 72.0 / Page.ScaleFactor,
			(Page.Width - 72.0) / Page.ScaleFactor, (Page.Height - 72.0) / Page.ScaleFactor);

		// set header on each page as the default
		HeaderOnEachPage = true;

		// create table border control
		Borders = new PdfTableBorder(this);

		// very small amount 1/300 of an inch
		// used to guard against rounding errors
		Epsilon = Document.Epsilon;
		return;
		}

	/// <summary>
	/// Sets the number of columns, and column's widths.
	/// </summary>
	/// <param name="ColumnWidth">Array of column widths.</param>
	/// <remarks>
	/// <para>
	/// The length of the array sets the number of columns.
	/// </para>
	/// <para>
	/// This method builds two PdfTableCell arrays. One array for data cells, and
	/// the other array for header cells.
	/// </para>
	/// <para>
	/// The actual column widths will be calculated to fit the width of the table. 
	/// </para>
	/// <para>
	/// The calculation is done as follows. First the library calculates the amount
	/// of net space available after border lines and grid lines width is taken off
	/// the width of the table as specified in TableArea. Second, the net space is divided
	/// in proportion to the requested widths.
	/// </para>
	/// </remarks>
	public void SetColumnWidth
			(
			params double[] ColumnWidth
			)
		{
		// save column width
		if(_ColumnWidth != null || ColumnWidth == null || ColumnWidth.Length == 0) throw new ApplicationException("PdfTable: SetColumnWidth invalid argument or already defined.");
		_ColumnWidth = ColumnWidth;

		// save number of columns
		Columns = _ColumnWidth.Length;

		// create cell and header arrays
		_Cell = new PdfTableCell[Columns];
		_Header = new PdfTableCell[Columns];
		for(int Index = 0; Index < Columns; Index++)
			{
			_Cell[Index] = new PdfTableCell(this, Index, false);
			_Header[Index] = new PdfTableCell(this, Index, true);
			}

		// vertical border control
		Borders.BordersInitialization();
		return;
		}

	/// <summary>
	/// PdfTable initialization.
	/// </summary>
	/// <remarks>
	/// Normally the system will call this method on first call to DrawRow(). 
	/// If called by user it must be called after initialization and before 
	/// the first row is drawn. This method sets the active flag.
	/// </remarks>
	public void PdfTableInitialization()
		{
		// initialize table is done
		if(Active) return;

		// make sure we have columns width array
		if(_ColumnWidth == null) throw new ApplicationException("PdfTable: SetColumnWidth array is missing.");

		// net table width
		double NetWidth = _TableArea.Width - Borders.HorizontalBordersTotalWidth();

		// calculate column width adjustment factor
		double Total = 0;
		foreach(double Width in _ColumnWidth) Total += Width;
		double Factor = NetWidth / Total;

		// create column position for border and grid lines
		_ColumnPosition = new double[Columns + 1];

		// initial border/grid position
		double Position = _TableArea.Left;

		// left border line position
		_ColumnPosition[0] = Position;

		// first column position and width
		Position += Borders.VertBorderHalfWidth[0];
		PdfTableCell CellZero = _Cell[0];
		CellZero.FrameLeft = Position;
		CellZero.FrameWidth = _ColumnWidth[0] * Factor;

		// first grid line position
		Position += CellZero.FrameWidth + Borders.VertBorderHalfWidth[1];

		// column width and position
		for(int Index = 1; Index < Columns; Index++)
			{
			// shortcut
			PdfTableCell Cell = _Cell[Index];

			// column net width
			Cell.FrameWidth = _ColumnWidth[Index] * Factor;

			// grid line position
			_ColumnPosition[Index] = Position;

			// cell left position
			Position += Borders.VertBorderHalfWidth[Index];
			Cell.FrameLeft = Position;

			// next grid line position
			Position += Cell.FrameWidth + Borders.VertBorderHalfWidth[Index + 1];
			}

		// last grid line position
		_ColumnPosition[Columns] = Position;

		// columns width after adjustments
		for(int Index = 0; Index < Columns; Index++) _ColumnWidth[Index] = _ColumnPosition[Index + 1] - _ColumnPosition[Index];

		// copy horizontal info from cell to header
		for(int Index = 0; Index < Columns; Index++)
			{
			// shortcut
			PdfTableCell Cell = _Cell[Index];
			PdfTableCell Header = _Header[Index];
			Header.FrameLeft = Cell.FrameLeft;
			Header.FrameWidth = Cell.FrameWidth;
			}

		// user did not define initial row position
		if(_RowTopPosition == 0) _RowTopPosition = _TableArea.Top;

		// make sure initial row position is within table area
		else if(_RowTopPosition < _TableArea.Bottom || _RowTopPosition > _TableArea.Top) throw new ApplicationException("PdfTable: Initial RowPosition outside table area.");

		// border positions for border drawing
		BorderHeaderActive = false;
		BorderYPos = new List<double>();
		BorderYPos.Add(_RowTopPosition);
		BorderRowTopPos = _RowTopPosition;
		BorderLeftPos = TableArea.Left - Borders.VertBorderHalfWidth[0];
		BorderRightPos = TableArea.Right + Borders.VertBorderHalfWidth[Columns];

		// initial row position
		_RowTopPosition -= Borders.TopBorder.HalfWidth;

		// table top and bottom limit for compare
		TableTopLimit = TableArea.Top - Borders.TopBorder.HalfWidth;
		TableBottomLimit = TableArea.Bottom + Borders.BottomBorder.HalfWidth;

		// initialization is done, PdfTable is ready to draw
		Active = true;
		return;
		}

	/// <summary>
	/// Draw one row.
	/// </summary>
	/// <param name="NewPage">Force new page.</param>
	/// <remarks>
	/// The DrawRow method must be called for each row in the table.
	/// Before calling this method all PdfTableCell values for the 
	/// current row must be set. If the NewPage argument is set to true,
	/// the software will print the row at the top of a new page.
	/// </remarks>
	public void DrawRow
			(
			bool NewPage = false
			)
		{
		// we are about to draw the first row
		if(!DrawingActive)
			{
			// one time PdfTable initialization
			PdfTableInitialization();

			// calculate header height
			int Index;
			for(Index = 0; Index < Columns && _Header[Index].Value == null; Index++);
			if(Index < Columns)
				{
				DisplayHeader = true;
				CalculateHeaderHeight();
				}

			// call user event handler for start of table on each page
			if(TableStartEvent != null) TableStartEvent(this, BorderRowTopPos);
			}

		// calculate row height
		CalculateRowHeight();

		// test for new page
		if(NewPage || _RowTopPosition - (TextBoxRowHeight + (HeaderHeight != 0.0 ? (HeaderHeight + 2.0 * Borders.HeaderHorBorder.HalfWidth) : 0.0)) < TableBottomLimit &&
			_RowTopPosition != TableTopLimit)
			{
			// create a new page
			CreateNewPage();
			}

		// draw header
		if(HeaderHeight != 0.0) DrawHeader();

		// draw row
		DrawOneRow();

		// set drawing active
		DrawingActive = true;

		// we have textboxes that did not fit on current page
		while(TextBoxContinue)
			{
			// create a new page
			CreateNewPage();

			// draw header
			if(HeaderHeight != 0.0) DrawHeader();

			// calculate row height
			CalculateRowHeight();

			// draw row
			DrawOneRow();
			}

		// update row number
		RowNumber++;
		return;
		}

	private void DrawOneRow()
		{
		// row bottom position
		RowBottomPosition = _RowTopPosition - RowHeight;

		// row bottom position is below table bottom
		if(RowBottomPosition < TableBottomLimit - Epsilon)
			{
			// test the smaller row height (TextBox minimum lines)
			if(_RowTopPosition - TextBoxRowHeight < TableBottomLimit - Epsilon) throw new ApplicationException("Table row height is too big");

			// adjust bottom position
			RowBottomPosition = TableBottomLimit;
			}

		// reset texbox continue flag
		TextBoxContinue = false;

		// draw row of cells
		foreach(PdfTableCell Cell in _Cell)
			{
			// calculate top and bottom client space
			Cell.ClientBottom = RowBottomPosition + Cell.Style.Margin.Bottom;
			Cell.ClientTop = _RowTopPosition - Cell.Style.Margin.Top;

			// call custom draw cell if required and draw header cell
			if(CustomDrawCellEvent == null || !Cell.Style.RaiseCustomDrawCellEvent || !CustomDrawCellEvent(this, Cell)) Cell.DrawCell();
			}

		// adjust row position to next grid line
		_RowTopPosition = RowBottomPosition - Borders.CellHorBorder.HalfWidth;

		// save for next time
		BorderYPos.Add(_RowTopPosition);
		BorderRowTopPos = _RowTopPosition;

		// adjust row position to next row of cells
		_RowTopPosition -= Borders.CellHorBorder.HalfWidth;

		// reset cell value
		foreach(PdfTableCell Cell in _Cell) Cell.Reset();
		return;
		}

	private void DrawHeader()
		{
		// row bottom position
		RowBottomPosition = _RowTopPosition - HeaderHeight;
		if(RowBottomPosition <= _TableArea.Bottom + 2.0 * Borders.HeaderHorBorder.HalfWidth + Borders.BottomBorder.HalfWidth)
			throw new ApplicationException("Table header height is too big");

		// draw each column header
		foreach(PdfTableCell Cell in _Header)
			{
			// calculate top and bottom client space
			Cell.ClientBottom = RowBottomPosition + Cell.Style.Margin.Bottom;
			Cell.ClientTop = _RowTopPosition - Cell.Style.Margin.Top;

			// call custom draw cell if required and draw header cell
			if(CustomDrawCellEvent == null || !Cell.Style.RaiseCustomDrawCellEvent || !CustomDrawCellEvent(this, Cell)) Cell.DrawCell();
			}

		// adjust row position to next grid line
		_RowTopPosition = RowBottomPosition - Borders.HeaderHorBorder.HalfWidth;

		// save for next time
		BorderHeaderActive = true;
		BorderYPos.Add(_RowTopPosition);
		BorderRowTopPos = _RowTopPosition;

		// adjust row position to next row of cells
		_RowTopPosition -= Borders.HeaderHorBorder.HalfWidth;

		// reset header height
		HeaderHeight = 0.0;
		return;
		}

	/// <summary>
	/// Close table.
	/// </summary>
	/// <remarks>
	/// The Close method must be called after the last row was drawn.
	/// </remarks>
	public void Close()
		{
		// make sure at least one row was drawn
		if(DrawingActive)
			{
			// draw border and grid on current page
			DrawBorders();

			// call user event handler for end of table
			if(TableEndEvent != null) TableEndEvent(this, _RowTopPosition);
			}
		return;
		}

	// calculate row height
	private void CalculateRowHeight()
		{
		// initial row height
		RowHeight = MinRowHeight;
		TextBoxRowHeight = MinRowHeight;

		// loop through all cells
		foreach(PdfTableCell Cell in _Cell)
			{
			// calculate cell height
			Cell.DrawCellInitialization();

			// adjust row height if required
			if(Cell.CellHeight > RowHeight) RowHeight = Cell.CellHeight;
			if(Cell.TextBoxCellHeight > TextBoxRowHeight) TextBoxRowHeight = Cell.TextBoxCellHeight;
			}
		return;
		}

	// calculate header height
	private void CalculateHeaderHeight()
		{
		// initial row height
		HeaderHeight = MinHeaderHeight;

		// loop through all cells
		foreach(PdfTableCell Cell in _Header)
			{
			// calculate cell height
			Cell.DrawCellInitialization();

			// adjust row height if required
			if(Cell.CellHeight > HeaderHeight) HeaderHeight = Cell.CellHeight;
			}

		return;
		}

	// start a new page
	private void CreateNewPage()
		{
		// terminate activity on current page
		if(DrawingActive)
			{
			// draw border and grid on current page
			DrawBorders();

			// call user event handler for end of table on each page
			if(TableEndEvent != null) TableEndEvent(this, _RowTopPosition);

			// update page number
			TablePageNumber++;
			}

		// commit to PDF file
		if(CommitToPdfFile)
			{
			Contents.CommitToPdfFile(CommitGCCollectFreq > 0 && (CommitGCCollectFreq == 1 || (TablePageNumber % CommitGCCollectFreq) == (CommitGCCollectFreq - 1)));
			}

		// create a new page as a clone of the previous page
		Page = new PdfPage(Page);
		Contents = new PdfContents(Page);

		// reset border lines
		BorderRowTopPos = _TableArea.Top;
		BorderYPos.Clear();
		BorderYPos.Add(BorderRowTopPos);

		// initial row position on the page
		_RowTopPosition = TableTopLimit;

		// calculate header height
		if(DrawingActive && DisplayHeader && HeaderOnEachPage) CalculateHeaderHeight();

		// call user event handler for start of table on each page
		if(TableStartEvent != null) TableStartEvent(this, BorderRowTopPos);
		return;
		}

	// Draw borders and grid lines after the last row on a page is drawn
	// or the last row of the table is drawn.
	private void DrawBorders()
		{
		// draw top border line
		Contents.DrawLine(BorderLeftPos, BorderYPos[0], BorderRightPos, BorderYPos[0], Borders.TopBorder);

		// row
		int RowStart = 1;
		int RowEnd = BorderYPos.Count -1;

		// draw horizontal header border line
		if(BorderHeaderActive)
			{
			Contents.DrawLine(BorderLeftPos, BorderYPos[1], BorderRightPos, BorderYPos[1], Borders.HeaderHorBorder);
			RowStart++;
			}			

		// draw horizontal cells border lines
		for(int Row = RowStart; Row < RowEnd; Row++)
			{
			Contents.DrawLine(BorderLeftPos, BorderYPos[Row], BorderRightPos, BorderYPos[Row], Borders.CellHorBorder);
			}

		// draw horizontal bottom border line
		Contents.DrawLine(BorderLeftPos, BorderRowTopPos, BorderRightPos, BorderRowTopPos, Borders.BottomBorder);

		// draw each vertical border line for header style
		if(BorderHeaderActive && Borders.HeaderVertBorderActive) for(int Col = 0; Col <= Columns; Col++)
			{
			Contents.DrawLine(ColumnPosition[Col], BorderYPos[0], ColumnPosition[Col], BorderYPos[1], Borders.HeaderVertBorder[Col]);
			}

		// draw each vertical line between cells
		if(Borders.CellVertBorderActive)
			{
			double Top = BorderHeaderActive ? BorderYPos[1] : BorderYPos[0];
			for(int Col = 0; Col <= Columns; Col++)
				{
				Contents.DrawLine(ColumnPosition[Col], Top, ColumnPosition[Col], BorderRowTopPos, Borders.CellVertBorder[Col]);
				}
			}
		return;
		}
	}
}
