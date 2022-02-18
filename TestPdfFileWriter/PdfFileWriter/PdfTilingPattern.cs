/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PdfTilingPattern
//	PDF tiling pattern resource class.
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
	/// PDF tiling type enumeration
	/// </summary>
	public enum TilingType
		{
		/// <summary>
		/// Constant
		/// </summary>
		Constant = 1,

		/// <summary>
		/// No distortion
		/// </summary>
		NoDistortion,

		/// <summary>
		/// Constant and fast
		/// </summary>
		ConstantAndFast,
		}

	/// <summary>
	/// PDF tiling pattern resource class
	/// </summary>
	/// <remarks>
	/// <para>
	/// <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#TilingPattern">For example of using tiling pattern see 3.3. Tiling Pattern</a>
	/// </para>
	/// <para>
	/// <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#TilingPattern2">or 3.8. Draw Rectangle with Rounded Corners and Filled with Brick Pattern</a>
	/// </para>
	/// </remarks>
	public class PdfTilingPattern : PdfContents
		{
		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// PDF Tiling pattern constructor.
		/// </summary>
		/// <param name="Document">Document object parent of the object.</param>
		/// <remarks>
		/// This program support only color tiling pattern: PaintType = 1.
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public PdfTilingPattern
				(
				PdfDocument Document
				) : base(Document, "/Pattern")
			{
			// create resource code
			ResourceCode = Document.GenerateResourceNumber('P');

			// add items to dictionary
			Dictionary.Add("/PatternType", "1"); // Tiling pattern
			Dictionary.Add("/PaintType", "1"); // color
			Dictionary.Add("/TilingType", "1"); // constant
			Dictionary.AddFormat("/BBox", "[0 0 {0} {1}]", ToPt(1.0), ToPt(1.0));
			Dictionary.AddReal("/XStep", ToPt(1.0));
			Dictionary.AddReal("/YStep", ToPt(1.0));
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Set tiling type
		/// </summary>
		/// <param name="TilingType">Tiling type</param>
		////////////////////////////////////////////////////////////////////
		public void SetTilingType
				(
				TilingType TilingType
				)
			{
			// by default the constructor set tiling type to 1 = constant
			Dictionary.AddInteger("/TilingType", (int) TilingType);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Set tile box
		/// </summary>
		/// <param name="Side">Length of one side.</param>
		/// <remarks>
		/// Set square bounding box and equal step
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public void SetTileBox
				(
				double Side
				)
			{
			SetTileBox(Side, Side, Side, Side);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Set tile box
		/// </summary>
		/// <param name="Width">Box width.</param>
		/// <param name="Height">Box height.</param>
		/// <remarks>
		/// Set rectangle bounding box and equal step.
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public void SetTileBox
				(
				double Width,
				double Height
				)
			{
			SetTileBox(Width, Height, Width, Height);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Set bounding box and step 
		/// </summary>
		/// <param name="Width">Box width.</param>
		/// <param name="Height">Box height.</param>
		/// <param name="StepX">Horizontal step</param>
		/// <param name="StepY">Vertical step</param>
		/// <remarks>
		/// Set rectangle bounding box and independent step size.
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public void SetTileBox
				(
				double Width,
				double Height,
				double StepX,
				double StepY
				)
			{
			// by default XStep == Width
			Dictionary.AddFormat("/BBox", "[0 0 {0} {1}]", ToPt(Width), ToPt(Height));
			Dictionary.AddReal("/XStep", ToPt(StepX));
			Dictionary.AddReal("/YStep", ToPt(StepY));
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Set scale
		/// </summary>
		/// <param name="Scale">Scale factor.</param>
		/// <remarks>
		/// Warning: the program replaces the transformation matrix
		/// with a new one [Scale 0 0 Scale 0 0].
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public void SetScale
				(
				double Scale
				)
			{
			// add items to dictionary
			Dictionary.AddFormat("/Matrix", "[{0} 0 0 {0} 0 0]", Round(Scale));
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Set scale
		/// </summary>
		/// <param name="ScaleX">Horizontal scale factor.</param>
		/// <param name="ScaleY">Vertical scale factor.</param>
		/// <remarks>
		/// Warning: the program replaces the transformation matrix
		/// with a new one [ScaleX 0 0 ScaleY 0 0].
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public void SetScale
				(
				double ScaleX,
				double ScaleY
				)
			{
			// add items to dictionary
			Dictionary.AddFormat("/Matrix", "[{0} 0 0 {1} 0 0]", Round(ScaleX), Round(ScaleY));
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Set scale and origin
		/// </summary>
		/// <param name="OriginX">Origin X</param>
		/// <param name="OriginY">Origin Y</param>
		/// <param name="ScaleX">Scale X</param>
		/// <param name="ScaleY">Scale Y</param>
		/// <remarks>
		/// Warning: the program replaces the transformation matrix
		/// with a new one [ScaleX 0 0 ScaleY OriginX OriginY].
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public void SetScaleAndOrigin
				(
				double OriginX,
				double OriginY,
				double ScaleX,
				double ScaleY
				)
			{
			// add items to dictionary
			Dictionary.AddFormat("/Matrix", "[{0} 0 0 {1} {2} {3}]", Round(ScaleX), Round(ScaleY), ToPt(OriginX), ToPt(OriginY));
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Set pattern transformation matrix
		/// </summary>
		/// <param name="a">A</param>
		/// <param name="b">B</param>
		/// <param name="c">C</param>
		/// <param name="d">D</param>
		/// <param name="e">E</param>
		/// <param name="f">F</param>
		/// <remarks>
		/// Xpage = a * Xuser + c * Yuser + e
		/// Ypage = b * Xuser + d * Yuser + f
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public void SetPatternMatrix
				(
				double a,
				double b,
				double c,
				double d,
				double e,
				double f
				)
			{
			// create full pattern transformation matrix
			Dictionary.AddFormat("/Matrix", "[{0} {1} {2} {3} {4} {5}]", Round(a), Round(b), Round(c), Round(d), ToPt(e), ToPt(f));
			return;
			}

		//////////////////////////////////////////////////////////////////// 
		/// <summary>
		/// Create new PdfTilingPattern class with brick pattern.
		/// </summary>
		/// <param name="Document">Current document object.</param>
		/// <param name="Scale">Scale factor.</param>
		/// <param name="BorderColor">Stroking color.</param>
		/// <param name="FillColor">Non-stroking color.</param>
		/// <returns>PDF tiling pattern</returns>
		/// <remarks>
		/// <para>
		/// The pattern is a square with one user unit side.
		/// </para>
		/// <para>
		/// The bottom half is one brick. The top half is two half bricks.
		/// </para>
		/// <para>
		/// Arguments:
		/// </para>
		/// <para>
		/// Scale the pattern to your requirements.
		/// </para>
		/// <para>
		/// Stroking color is the mortar color.
		/// </para>
		/// <para>
		/// Nonstroking color is the brick color.
		/// </para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public static PdfTilingPattern SetBrickPattern
				(
				PdfDocument Document,
				double Scale,
				Color BorderColor,
				Color FillColor
				)
			{
			// create tilling pattern
			PdfTilingPattern Pattern = new PdfTilingPattern(Document);

			// set scale
			Pattern.SetScale(Scale);

			// draw rectrangle control
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Paint = DrawPaint.BorderAndFill;
			DrawCtrl.BorderWidth = 0.05;
			DrawCtrl.BorderColor = BorderColor;
			DrawCtrl.BackgroundTexture = FillColor;

			// draw rectangle 1
			PdfRectangle Rect1 = new PdfRectangle(0.025, 0.025, 0.975, 0.475);
			Pattern.DrawGraphics(DrawCtrl, Rect1);

			// draw rectangle 2
			PdfRectangle Rect2 = new PdfRectangle(-0.475, 0.525, 0.475, 0.975);
			Pattern.DrawGraphics(DrawCtrl, Rect2);

			// draw rectangle 3
			PdfRectangle Rect3 = new PdfRectangle(0.525, 0.525, 1.475, 0.975);
			Pattern.DrawGraphics(DrawCtrl, Rect3);
			return Pattern;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Create new PdfTilingPattern class with weave pattern.
		/// </summary>
		/// <param name="Document">Current PDF document.</param>
		/// <param name="Scale">Scale factor</param>
		/// <param name="Background">Background color.</param>
		/// <param name="Horizontal">Horizontal line color.</param>
		/// <param name="Vertical">Vertical line color.</param>
		/// <returns>PDF tiling pattern</returns>
		/// <remarks>
		/// <para>
		/// The pattern in a square with one user unit side.
		/// </para>
		/// <para>
		/// It is made of horizontal and vertical rectangles.
		/// </para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public static PdfTilingPattern SetWeavePattern
				(
				PdfDocument Document,
				double Scale,
				Color Background,
				Color Horizontal,
				Color Vertical
				)
			{
			// constants
			const double RectSide1 = 4.0 / 6.0;
			const double RectSide2 = 2.0 / 6.0;
			const double LineWidth = 0.2 / 6.0;
			const double HalfWidth = 0.5 * LineWidth;
			const double OneSixthPlusHalf = (1.0 / 6.0) + HalfWidth;
			const double ThreeSixthPlusHalf = (3.0 / 6.0) + HalfWidth;
			const double ThreeSixthMinusHalf = (3.0 / 6.0) - HalfWidth;
			const double FourSixthPlusHalf = (4.0 / 6.0) + HalfWidth;

			// create tilling patterd object
			PdfTilingPattern Pattern = new PdfTilingPattern(Document);

			// set scale
			Pattern.SetScale(Scale);
			Pattern.SetTileBox(1.0);

			// create draw rectangle control
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Paint = DrawPaint.Fill;
			DrawCtrl.BackgroundTexture = Background;

			// fill the entire area with background color
			PdfRectangle Rect1 = new PdfRectangle(0.0, 0.0, 1.0, 1.0);
			Pattern.DrawGraphics(DrawCtrl, Rect1);

			// adjust draw rectangle contrl
			DrawCtrl.Paint = DrawPaint.BorderAndFill;
			DrawCtrl.BorderWidth = LineWidth;
			DrawCtrl.BorderColor = Background;
			DrawCtrl.BackgroundTexture = Horizontal;

			// draw rectangle 2
			PdfRectangle Rect2 = new PdfRectangle(HalfWidth, OneSixthPlusHalf,
				RectSide1 - HalfWidth, OneSixthPlusHalf + RectSide2 - LineWidth);
			Pattern.DrawGraphics(DrawCtrl, Rect2);

			// draw rectangle 3
			PdfRectangle Rect3 = new PdfRectangle(-ThreeSixthMinusHalf, FourSixthPlusHalf,
				-ThreeSixthMinusHalf + RectSide1 - LineWidth,
				FourSixthPlusHalf + RectSide2 - LineWidth);
			Pattern.DrawGraphics(DrawCtrl, Rect3);

			// draw rectangle 4
			PdfRectangle Rect4 = new PdfRectangle(ThreeSixthPlusHalf, FourSixthPlusHalf,
				ThreeSixthPlusHalf + RectSide1 - LineWidth,
				FourSixthPlusHalf + RectSide2 - LineWidth);
			Pattern.DrawGraphics(DrawCtrl, Rect4);

			// adjust draw rectangle control
			DrawCtrl.BackgroundTexture = Vertical;

			// Draw rectangle 5
			PdfRectangle Rect5 = new PdfRectangle(FourSixthPlusHalf, HalfWidth,
				FourSixthPlusHalf + RectSide2 - LineWidth,
				HalfWidth + RectSide1 - LineWidth);
			Pattern.DrawGraphics(DrawCtrl, Rect5);

			// draw rectangle 6
			PdfRectangle Rect6 = new PdfRectangle(OneSixthPlusHalf, -ThreeSixthMinusHalf,
				OneSixthPlusHalf + RectSide2 - LineWidth,
				-ThreeSixthMinusHalf + RectSide1 - LineWidth);
			Pattern.DrawGraphics(DrawCtrl, Rect6);

			// draw rectangle 7
			PdfRectangle Rect7 = new PdfRectangle(OneSixthPlusHalf, ThreeSixthPlusHalf,
				OneSixthPlusHalf + RectSide2 - LineWidth,
				ThreeSixthPlusHalf + RectSide1 - LineWidth);
			Pattern.DrawGraphics(DrawCtrl, Rect7);
			return Pattern;
			}
		}
	}
