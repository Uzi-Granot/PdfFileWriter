/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PdfTableBorder
//	Data table border lines support.
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
	/// <summary>
	/// Border line style class
	/// </summary>
	public class PdfTableBorderStyle
		{
		/// <summary>
		/// Gets display border line flag
		/// </summary>
		public bool Display { get; internal set; }

		/// <summary>
		/// Gets border line width
		/// </summary>
		public double Width { get; internal set; }

		/// <summary>
		/// Gets border line color
		/// </summary>
		public Color Color { get; internal set; }

		/// <summary>
		/// Gets border line half width
		/// </summary>
		/// <remarks>
		/// If display flag is false, the returned value is zero
		/// </remarks>
		public double HalfWidth
			{
			get
				{
				return Display ? 0.5 * Width : 0.0;
				}
			}
		/// <summary>
		/// PdfTableBorderStyle default constructor
		/// </summary>
		public PdfTableBorderStyle() { }

		/// <summary>
		/// PdfTableBorderStyle constructor
		/// </summary>
		/// <param name="Width">Border line width</param>
		public PdfTableBorderStyle
				(
				double Width
				)
			{
			this.Width = Width;
			Display = true;
			Color = Color.Black;
			return;
			}

		/// <summary>
		/// PdfTableBorderStyle constructor
		/// </summary>
		/// <param name="Width">Border line width</param>
		/// <param name="Color">Border line color</param>
		public PdfTableBorderStyle
				(
				double Width,
				Color Color
				)
			{
			this.Width = Width;
			this.Color = Color;
			Display = true;
			return;
			}

		/// <summary>
		/// Clear border line style
		/// </summary>
		internal void Clear()
			{
			Display = false;
			Width = 0;
			Color = Color.Empty;
			return;
			}

		/// <summary>
		/// Set border line
		/// </summary>
		/// <param name="Width">Line width in user units</param>
		/// <param name="Color">Line color</param>
		internal void Set
				(
				double Width,
				Color Color
				)
			{
			this.Width = Width;
			this.Color = Color;
			Display = true;
			return;
			}

		/// <summary>
		/// Copy border line style
		/// </summary>
		/// <param name="Other">Border line template</param>
		internal void Copy
				(
				PdfTableBorderStyle Other
				)
			{
			Display = Other.Display;
			Width = Other.Width;
			Color = Other.Color;
			return;
			}
		}

	/// <summary>
	/// Table's borders control
	/// </summary>
	public class PdfTableBorder
		{
		/// <summary>
		/// Top border line
		/// </summary>
		public PdfTableBorderStyle TopBorder { get; internal set; }

		/// <summary>
		/// Clear top border line
		/// </summary>
		public void ClearTopBorder()
			{
			TestInit();
			TopBorder.Clear();
			return;
			}

		/// <summary>
		/// Set top border line
		/// </summary>
		/// <param name="Width">Line width</param>
		public void SetTopBorder
				(
				double Width
				)
			{
			TestInit();
			TopBorder.Set(Width, Color.Black);
			return;
			}

		/// <summary>
		/// Set top border line
		/// </summary>
		/// <param name="Width">Line width</param>
		/// <param name="Color">LineColor</param>
		public void SetTopBorder
				(
				double Width,
				Color Color
				)
			{
			TestInit();
			TopBorder.Set(Width, Color);
			return;
			}

		/// <summary>
		/// Bottom border line
		/// </summary>
		public PdfTableBorderStyle BottomBorder { get; internal set; }

		/// <summary>
		/// Clear bottom border line
		/// </summary>
		public void ClearBottomBorder()
			{
			TestInit();
			BottomBorder.Clear();
			return;
			}

		/// <summary>
		/// Set bottom border line
		/// </summary>
		/// <param name="Width">Line width</param>
		public void SetBottomBorder
				(
				double Width
				)
			{
			TestInit();
			BottomBorder.Set(Width, Color.Black);
			return;
			}

		/// <summary>
		/// Set bottom border line
		/// </summary>
		/// <param name="Width">Line width</param>
		/// <param name="Color">LineColor</param>
		public void SetBottomBorder
				(
				double Width,
				Color Color
				)
			{
			TestInit();
			BottomBorder.Set(Width, Color);
			return;
			}

		/// <summary>
		/// Header horizontal border
		/// </summary>
		/// <remarks>
		/// Border between headers and first row of cells.
		/// </remarks>
		public PdfTableBorderStyle HeaderHorBorder { get; internal set; }

		/// <summary>
		/// Clear header horizontal border line
		/// </summary>
		public void ClearHeaderHorBorder()
			{
			TestInit();
			HeaderHorBorder.Clear();
			return;
			}

		/// <summary>
		/// Set header horizontal border line
		/// </summary>
		/// <param name="Width">Line width</param>
		public void SetHeaderHorBorder
				(
				double Width
				)
			{
			TestInit();
			HeaderHorBorder.Set(Width, Color.Black);
			return;
			}

		/// <summary>
		/// Set header horizontal border line
		/// </summary>
		/// <param name="Width">Line width</param>
		/// <param name="Color">LineColor</param>
		public void SetHeaderHorBorder
				(
				double Width,
				Color Color
				)
			{
			TestInit();
			HeaderHorBorder.Set(Width, Color);
			return;
			}

		/// <summary>
		/// Cell horizontal border line
		/// </summary>
		/// <remarks>
		/// One border style for all horizontal borders between rows of cells.
		/// </remarks>
		public PdfTableBorderStyle CellHorBorder { get; internal set; }

		/// <summary>
		/// Clear cell horizontal border line
		/// </summary>
		public void ClearCellHorBorder()
			{
			TestInit();
			CellHorBorder.Clear();
			return;
			}

		/// <summary>
		/// Set cell horizontal border line
		/// </summary>
		/// <param name="Width">Line width</param>
		public void SetCellHorBorder
				(
				double Width
				)
			{
			TestInit();
			CellHorBorder.Set(Width, Color.Black);
			return;
			}

		/// <summary>
		/// Set cell horizontal border line
		/// </summary>
		/// <param name="Width">Line width</param>
		/// <param name="Color">LineColor</param>
		public void SetCellHorBorder
				(
				double Width,
				Color Color
				)
			{
			TestInit();
			CellHorBorder.Set(Width, Color);
			return;
			}

		/// <summary>
		/// Array of vertical borders between headers
		/// </summary>
		/// <remarks>
		/// Array of vertical borders between all headers.
		/// Array's size is Columns + 1.
		/// Array's item [0] is left border.
		/// Array's item [Coloumns] is right border.
		/// </remarks>
		public PdfTableBorderStyle[] HeaderVertBorder { get; internal set; }

		/// <summary>
		/// Clear header vertical border line
		/// </summary>
		/// <param name="Index">Border line index</param>
		public void ClearHeaderVertBorder
				(
				int Index
				)
			{
			TestInit();
			HeaderVertBorder[Index].Clear();
			return;
			}

		/// <summary>
		/// Set header vertical border line
		/// </summary>
		/// <param name="Index">Border line index</param>
		/// <param name="Width">Line width</param>
		public void SetHeaderVertBorder
				(
				int Index,
				double Width
				)
			{
			TestInit();
			HeaderVertBorder[Index].Set(Width, Color.Black);
			return;
			}

		/// <summary>
		/// Set header horizontal border line
		/// </summary>
		/// <param name="Index">Border line index</param>
		/// <param name="Width">Line width</param>
		/// <param name="Color">LineColor</param>
		public void SetHeaderVertBorder
				(
				int Index,
				double Width,
				Color Color
				)
			{
			TestInit();
			HeaderVertBorder[Index].Set(Width, Color);
			return;
			}

		/// <summary>
		/// At least one header vertical border is active
		/// </summary>
		public bool HeaderVertBorderActive { get; internal set; }

		/// <summary>
		/// Array of vertical borders between cells
		/// </summary>
		/// <remarks>
		/// Array of vertical borders between all cells.
		/// Array's size is Columns + 1.
		/// Array's item [0] is left border.
		/// Array's item [Coloumns] is right border.
		/// </remarks>
		public PdfTableBorderStyle[] CellVertBorder { get; internal set; }

		/// <summary>
		/// Clear cell vertical border line
		/// </summary>
		/// <param name="Index">Border line index</param>
		public void ClearCellVertBorder
				(
				int Index
				)
			{
			TestInit();
			CellVertBorder[Index].Clear();
			return;
			}

		/// <summary>
		/// Set cell vertical border line
		/// </summary>
		/// <param name="Index">Border line index</param>
		/// <param name="Width">Line width</param>
		public void SetCellVertBorder
				(
				int Index,
				double Width
				)
			{
			TestInit();
			CellVertBorder[Index].Set(Width, Color.Black);
			return;
			}

		/// <summary>
		/// Set cell horizontal border line
		/// </summary>
		/// <param name="Index">Border line index</param>
		/// <param name="Width">Line width</param>
		/// <param name="Color">LineColor</param>
		public void SetCellVertBorder
				(
				int Index,
				double Width,
				Color Color
				)
			{
			TestInit();
			CellVertBorder[Index].Set(Width, Color);
			return;
			}

		/// <summary>
		/// At least one cell vertical border is active
		/// </summary>
		public bool CellVertBorderActive { get; internal set; }

		internal PdfTable Parent;
		internal PdfDocument Document;
		internal int Columns;
		internal double[] VertBorderHalfWidth;

		internal PdfTableBorder
				(
				PdfTable Parent
				)
			{
			// save PdfTable parent and document
			this.Parent = Parent;
			this.Document = Parent.Document;
			return;
			}

		internal void BordersInitialization()
			{
			// save number of columns
			this.Columns = Parent.Columns;

			// define horizontal borders
			TopBorder = new PdfTableBorderStyle();
			BottomBorder = new PdfTableBorderStyle();
			HeaderHorBorder = new PdfTableBorderStyle();
			CellHorBorder = new PdfTableBorderStyle();

			// define vertical border lines
			HeaderVertBorder = new PdfTableBorderStyle[Columns + 1];
			CellVertBorder = new PdfTableBorderStyle[Columns + 1];
			for(int Index = 0; Index <= Columns; Index++)
				{
				HeaderVertBorder[Index] = new PdfTableBorderStyle();
				CellVertBorder[Index] = new PdfTableBorderStyle();
				}
			SetDefaultBorders();
			return;
			}

		/// <summary>
		/// Clear all borders
		/// </summary>
		/// <remarks>
		/// The table will be displayed with no borders or gris lines.
		/// </remarks>
		public void ClearAllBorders()
			{
			// set is not allowed
			if(Parent.Active || TopBorder == null)
				throw new ApplicationException("Set borders after SetColumnWidth and before table is active.");

			// clear all horizontal borders
			TopBorder.Clear();
			BottomBorder.Clear();
			HeaderHorBorder.Clear();
			CellHorBorder.Clear();

			// clear all vertical border lines
			for(int Index = 0; Index <= Columns; Index++)
				{
				HeaderVertBorder[Index].Clear();
				CellVertBorder[Index].Clear();
				}
			return;
			}

		/// <summary>
		/// Set all borders to default values.
		/// </summary>
		/// <remarks>
		/// All borders will be black.
		/// Frame line width is set to one point.
		/// Grids line width are set to 0.2 of one point
		/// </remarks>
		public void SetDefaultBorders()
			{
			// one point or 1/72 inch width
			double OnePoint = 1.0 / Document.ScaleFactor;
			SetAllBorders(OnePoint, Color.Black, 0.20 * OnePoint, Color.Black);
			return;
			}

		/// <summary>
		/// Set all borders to the same line width
		/// </summary>
		/// <param name="Width">Border line width</param>
		public void SetAllBorders
				(
				double Width
				)
			{
			SetAllBorders(Width, Color.Black, Width, Color.Black);
			return;
			}

		/// <summary>
		/// Set all borders to the same line width and color
		/// </summary>
		/// <param name="Width">Border line width</param>
		/// <param name="Color">Border line color</param>
		public void SetAllBorders
				(
				double Width,
				Color Color
				)
			{
			SetAllBorders(Width, Color, Width, Color);
			return;
			}

		/// <summary>
		/// Set all borders
		/// </summary>
		/// <param name="FrameWidth">Frame border line width</param>
		/// <param name="GridWidth">Grid borders line width</param>
		public void SetAllBorders
				(
				double FrameWidth,
				double GridWidth
				)
			{
			SetAllBorders(FrameWidth, Color.Black, GridWidth, Color.Black);
			return;
			}

		/// <summary>
		/// Set all borders
		/// </summary>
		/// <param name="FrameWidth">Frame border line width</param>
		/// <param name="FrameColor">Frame border color</param>
		/// <param name="GridWidth">Grid borders line width</param>
		/// <param name="GridColor">Grid line color</param>
		public void SetAllBorders
				(
				double FrameWidth,
				Color FrameColor,
				double GridWidth,
				Color GridColor
				)
			{
			// set is not allowed
			if(Parent.Active || TopBorder == null)
				throw new ApplicationException("Set borders after SetColumnWidth and before table is active.");

			// define default horizontal borders
			TopBorder.Set(FrameWidth, FrameColor);
			BottomBorder.Set(FrameWidth, FrameColor);
			HeaderHorBorder.Set(FrameWidth, FrameColor);
			CellHorBorder.Set(GridWidth, GridColor);

			// vertical border lines
			HeaderVertBorder[0].Set(FrameWidth, FrameColor);
			CellVertBorder[0].Set(FrameWidth, FrameColor);
			for(int Index = 1; Index < Columns; Index++)
				{
				HeaderVertBorder[Index].Set(GridWidth, GridColor);
				CellVertBorder[Index].Set(GridWidth, GridColor);
				}
			HeaderVertBorder[Columns].Set(FrameWidth, FrameColor);
			CellVertBorder[Columns].Set(FrameWidth, FrameColor);
			return;
			}

		/// <summary>
		/// Set frame border lines
		/// </summary>
		/// <param name="FrameWidth">Frame line width</param>
		public void SetFrame
				(
				double FrameWidth
				)
			{
			SetFrame(FrameWidth, Color.Black);
			return;
			}

		/// <summary>
		/// Set frame border lines
		/// </summary>
		/// <param name="FrameWidth">Frame line width</param>
		/// <param name="FrameColor">Frame line color</param>
		public void SetFrame
				(
				double FrameWidth,
				Color FrameColor
				)
			{
			// set is not allowed
			if(Parent.Active || TopBorder == null)
				throw new ApplicationException("Set borders after SetColumnWidth and before table is active.");

			// define default horizontal borders
			TopBorder.Set(FrameWidth, FrameColor);
			BottomBorder.Set(FrameWidth, FrameColor);
			HeaderHorBorder.Set(FrameWidth, FrameColor);
			CellHorBorder.Clear();

			// vertical border lines
			HeaderVertBorder[0].Set(FrameWidth, FrameColor);
			CellVertBorder[0].Set(FrameWidth, FrameColor);
			for(int Index = 1; Index < Columns; Index++)
				{
				HeaderVertBorder[Index].Clear();
				CellVertBorder[Index].Clear();
				}
			HeaderVertBorder[Columns].Set(FrameWidth, FrameColor);
			CellVertBorder[Columns].Set(FrameWidth, FrameColor);
			return;
			}

		internal double HorizontalBordersTotalWidth()
			{
			// look for at least one header vertical border
			for(int Index = 0; Index <= Columns; Index++)
				if(HeaderVertBorder[Index].Display)
					{
					HeaderVertBorderActive = true;
					break;
					}

			// look for at least one cell vertical border
			for(int Index = 0; Index <= Columns; Index++)
				if(CellVertBorder[Index].Display)
					{
					CellVertBorderActive = true;
					break;
					}

			// allocate array of overall half width
			VertBorderHalfWidth = new double[Columns + 1];

			// we have at least one vertical border
			if(HeaderVertBorderActive || CellVertBorderActive)
				{
				for(int Index = 0; Index <= Columns; Index++)
					VertBorderHalfWidth[Index] = Math.Max(HeaderVertBorder[Index].HalfWidth, CellVertBorder[Index].HalfWidth);

				double TotalWidth = VertBorderHalfWidth[0] + VertBorderHalfWidth[Columns];
				for(int Index = 1; Index < Columns; Index++)
					TotalWidth += 2.0 * VertBorderHalfWidth[Index];
				return TotalWidth;
				}

			// no borders
			return 0.0;
			}

		internal void TestInit()
			{
			// set is not allowed
			if(Parent.Active || TopBorder == null)
				throw new ApplicationException("Set borders after SetColumnWidth and before table is active.");
			return;
			}
		}
	}
