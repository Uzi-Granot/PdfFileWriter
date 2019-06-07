/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	DrawWPFPath
//	Draw Windows Presentation Foundation path.
//
///////////////////////////////////////////////////////////////////////
//	The PDF File Writer library was enhanced to allow drawing of graphic
//	artwork using Windows Presentation Foundation (WPF) classes.
//	These enhancements were proposed by Elena Malnati elena@yelleaf.com.
//	I would like to thank her for providing me with the source code
//	to implement them. Further I would like to thank Joe Cridge for
//	his contribution of code to convert elliptical arc to Bezier curve.
//	The source code was modified to be consistent in style to the rest
//	of the library. Developers of Windows Forms application can benefit
//	from all of these enhancements
//	For further information visit www.joecridge.me/bezier.pdf.
//	Also visit http://p5js.org/ for some coolness
///////////////////////////////////////////////////////////////////////
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
using SysMedia = System.Windows.Media;
using SysWin = System.Windows;

namespace PdfFileWriter
	{
	/// <summary>
	/// Y Axis direction
	/// </summary>
	public enum YAxisDirection
	{
	/// <summary>
	/// Down as per Microsoft convention.
	/// </summary>
	Down,
	/// <summary>
	/// Up as per PDF convention
	/// </summary>
	Up,
	}

/// <summary>
/// Fill rule enumeration
/// </summary>
public enum FillRule
	{
	/// <summary>
	/// Even odd rule
	/// </summary>
	EvenOdd,
	/// <summary>
	/// Non zero rule
	/// </summary>
	NonZero,
	}

/// <summary>
/// Draw WPF path
/// </summary>
public class DrawWPFPath
	{
	/// <summary>
	/// Fill rule
	/// </summary>
	public FillRule FillRule {get; set;}
	/// <summary>
	/// Blend mode
	/// </summary>
	public BlendMode BlendMode {get; set;}
	/// <summary>
	/// Brush opacity
	/// </summary>
	public double BrushOpacity {get; set;}
	/// <summary>
	/// Pen opacity
	/// </summary>
	public double PenOpacity {get; set;}
	/// <summary>
	/// Pen width
	/// </summary>
	public double PenWidth {get; set;}
	/// <summary>
	/// Line cap
	/// </summary>
	public PdfLineCap LineCap {get; set;}
	/// <summary>
	/// Line join
	/// </summary>
	public PdfLineJoin LineJoin {get; set;}
	/// <summary>
	/// Miter limit
	/// </summary>
	public double MiterLimit {get; set;}
	/// <summary>
	/// Pen's dash array
	/// </summary>
	public double[] DashArray {get; set;}
	/// <summary>
	/// Pen's dash starting phase
	/// </summary>
	public double DashPhase {get; set;}

	/// <summary>
	/// Input path bounding box y axis direction (default is down)
	/// </summary>
	public YAxisDirection PathYAxis {get; internal set;}
	/// <summary>
	/// Input path bounding box left position
	/// </summary>
	public double PathBBoxX {get; internal set;}
	/// <summary>
	/// Input path bounding box top (y axis down) or bottom (y axis up) position
	/// </summary>
	public double PathBBoxY {get; internal set;}
	/// <summary>
	/// Input path bounding box width
	/// </summary>
	public double PathBBoxWidth {get; internal set;}
	/// <summary>
	/// Input path bounding box height
	/// </summary>
	public double PathBBoxHeight {get; internal set;}

	internal SysMedia.PathGeometry MediaPath;
	internal double DrawRectX;
	internal double DrawRectY;
	internal double DrawRectWidth;
	internal double DrawRectHeight;
	internal double ScaleX;
	internal double ScaleY;
	internal double TransX;
	internal double TransY;
	internal object NonStroking;
	internal object Stroking;

	private class UseCurrent {}

	/// <summary>
	/// Set path geometry
	/// </summary>
	/// <param name="PathString">Path geometry text string</param>
	/// <param name="YAxis">Y Axix direction</param>
	public DrawWPFPath
			(
			string PathString,
			YAxisDirection	YAxis
			) : this(SysMedia.PathGeometry.CreateFromGeometry(SysMedia.PathGeometry.Parse(PathString)), YAxis) {}

	/// <summary>
	/// Draw WPF path constructor
	/// </summary>
	/// <param name="MediaPath">System.Windows.Media path geometry</param>
	/// <param name="PathYAxis">Y Axix direction</param>
	public DrawWPFPath
			(
			SysMedia.PathGeometry MediaPath,
			YAxisDirection PathYAxis
			)
		{
		// save media path
		this.MediaPath = MediaPath;

		// save path rectangle and y axis direction
		PathBBoxX = MediaPath.Bounds.X;
		PathBBoxY = MediaPath.Bounds.Y;
		PathBBoxWidth = MediaPath.Bounds.Width;
		PathBBoxHeight = MediaPath.Bounds.Height;
		this.PathYAxis = PathYAxis;

		// test arguments
		if(PathBBoxWidth == 0 && PathBBoxHeight == 0) throw new ApplicationException("DrawWPFPath: Path bounding box is empty");

		// initialization values
		FillRule = MediaPath.FillRule == SysMedia.FillRule.EvenOdd ? FillRule.EvenOdd : FillRule.NonZero;
		BlendMode = BlendMode.Normal;
		BrushOpacity = 1.0;
		PenOpacity = 1.0;
		PenWidth = -1;
		LineCap = (PdfLineCap) (-1);
		LineJoin = (PdfLineJoin) (-1);
		MiterLimit = -1;
		return;
		}

	/// <summary>
	/// Reset non-stroking brush to no fill
	/// </summary>
	public void ResetBrush()
		{
		NonStroking = null;
		BrushOpacity = 1.0;
		return;
		}

	/// <summary>
	/// Filling the path will use the current color
	/// </summary>
	public void UseCurrectBrush()
		{
		ResetBrush();
		NonStroking = new UseCurrent();
		return;
		}

	/// <summary>
	/// Set brush to solid color
	/// </summary>
	/// <param name="BrushColor">Solid color</param>
	/// <remarks>
	/// <para>The method sets all 4 color components: Alpha Red Green and Blue.</para>
	/// </remarks>
	public void SetBrush
			(
			Color BrushColor
			)
		{
		NonStroking = BrushColor;
		BrushOpacity = (double) BrushColor.A / 255.0;
		return;
		}

	/// <summary>
	/// Set brush to solid color
	/// </summary>
	/// <param name="SolidColorBrush">Media solid color brush</param>
	public void SetBrush
			(
			SysMedia.SolidColorBrush SolidColorBrush
			)
		{
		NonStroking = Color.FromArgb(SolidColorBrush.Color.R, SolidColorBrush.Color.G, SolidColorBrush.Color.B);
		BrushOpacity = (SolidColorBrush.Color.A / 255.0) * SolidColorBrush.Opacity;
		return;
		}

	/// <summary>
	/// Set brush
	/// </summary>
	/// <param name="AxialShading">Axial shading</param>
	/// <param name="BrushOpacity">Brush opacity (0.0 to 1.0)</param>
	public void SetBrush
			(
			PdfAxialShading AxialShading,
			double BrushOpacity = 1.0
			)
		{
		NonStroking = AxialShading;
		this.BrushOpacity = BrushOpacity;
		return;
		}

	/// <summary>
	/// Set brush
	/// </summary>
	/// <param name="LinearGradientBrush">Linear gradient brush</param>
	/// <remarks>This method sets BrushOpacity.</remarks>
	public void SetBrush
			(
			SysMedia.LinearGradientBrush LinearGradientBrush
			)
		{
		NonStroking = LinearGradientBrush;
		BrushOpacity = LinearGradientBrush.Opacity;
		return;
		}

	/// <summary>
	/// Set brush
	/// </summary>
	/// <param name="RadialShading">PDF radial shading brush</param>
	/// <param name="BrushOpacity">Brush opacity</param>
	public void SetBrush
			(
			PdfRadialShading RadialShading,
			double BrushOpacity = 1.0
			)
		{
		NonStroking = RadialShading;
		this.BrushOpacity = BrushOpacity;
		return;
		}

	/// <summary>
	/// Set brush
	/// </summary>
	/// <param name="RadialGradientBrush">Radial gradient brush</param>
	/// <remarks>This method sets BrushOpacity.</remarks>
	public void SetBrush
			(
			SysMedia.RadialGradientBrush RadialGradientBrush
			)
		{
		NonStroking = RadialGradientBrush;
		BrushOpacity = RadialGradientBrush.Opacity;
		return;
		}

	/// <summary>
	/// Set brush
	/// </summary>
	/// <param name="TilingPattern">PDF tiling pattern resource</param>
	/// <param name="BrushOpacity">Brush opacity</param>
	public void SetBrush
			(
			PdfTilingPattern TilingPattern,
			double BrushOpacity = 1.0
			)
		{
		NonStroking = TilingPattern;
		this.BrushOpacity = BrushOpacity;
		return;
		}

	/// <summary>
	/// Reset pen
	/// </summary>
	/// <remarks>Pen is not defined.</remarks>
	public void ResetPen()
		{
		Stroking = null;
		PenOpacity = 1.0;
		PenWidth = -1;
		return;
		}

	/// <summary>
	/// Pen color will use the current color
	/// </summary>
	public void UseCurrectPen()
		{
		ResetPen();
		Stroking = new UseCurrent();
		return;
		}

	/// <summary>
	/// Set pen color
	/// </summary>
	/// <param name="PenColor">Pen color</param>
	public void SetPen
			(
			Color PenColor
			)
		{
		Stroking = PenColor;
		PenOpacity = (double) PenColor.A / 255.0;
		return;
		}

	/// <summary>
	/// Set media pen
	/// </summary>
	/// <param name="MediaPen">Media pen</param>
	public void SetPen
			(
			SysMedia.Pen MediaPen
			)
		{
		if(MediaPen.Brush == null || MediaPen.Brush.GetType() != typeof(SysMedia.SolidColorBrush) ||
			((SysMedia.SolidColorBrush) MediaPen.Brush).Color == SysMedia.Colors.Transparent)
				throw new ApplicationException("DrawWPFPath: System media pen must be SolidColorBrush");
		Stroking = MediaPen;
		return;
		}

	/// <summary>
	/// Pen width
	/// </summary>
	/// <param name="PenWidth">Pen width in user coordinates</param>
	public void SetPenWidth
			(
			double PenWidth
			)
		{
		this.PenWidth = PenWidth;
		return;
		}

	internal void Draw
			(
			PdfContents Contents,
			double		DrawRectX,
			double		DrawRectY,
			double		DrawRectWidth,
			double		DrawRectHeight,
			ContentAlignment Alignment = (ContentAlignment) 0
			)
		{
		// save drawing rectangle in user coordinates
		this.DrawRectX = DrawRectX;
		this.DrawRectY = DrawRectY;
		this.DrawRectWidth = DrawRectWidth;
		this.DrawRectHeight = DrawRectHeight;

		// test arguments
		if(DrawRectWidth == 0 && DrawRectHeight == 0 || DrawRectWidth == 0 && PathBBoxWidth != 0 || DrawRectHeight == 0 && PathBBoxHeight != 0)
			throw new ApplicationException("DrawWPFPath: Drawing rectangle is empty");

		// set transformation matrix
		SetTransformation(Alignment);

		// clip
		if(Stroking == null && NonStroking == null)
			{
			// build clipping path 
			BuildPath(Contents, FillRule == FillRule.EvenOdd ? PaintOp.ClipPathEor : PaintOp.ClipPathWnr);
			return;
			}

		// paint operator
		PaintOp PaintOperator;

		// brush is defined as shading
		if(NonStroking != null && (NonStroking.GetType() == typeof(SysMedia.LinearGradientBrush) || NonStroking.GetType() == typeof(SysMedia.RadialGradientBrush) ||
			NonStroking.GetType() == typeof(PdfAxialShading) || NonStroking.GetType() == typeof(PdfRadialShading)))
			{
			// save graphics state
			Contents.SaveGraphicsState();

			// build clipping path 
			BuildPath(Contents, FillRule == FillRule.EvenOdd ? PaintOp.ClipPathEor : PaintOp.ClipPathWnr);

			// set bland mode
			if(BlendMode != BlendMode.Normal) Contents.SetBlendMode(BlendMode);

			// set opacity
			Contents.SetAlphaNonStroking(BrushOpacity);

			// draw linera gradient brush shading bounded by clip path
			if(NonStroking.GetType() == typeof(SysMedia.LinearGradientBrush))
				{ 
				PdfAxialShading AxialShading = new PdfAxialShading(Contents.Document, (SysMedia.LinearGradientBrush) NonStroking);
				AxialShading.SetBoundingBox(DrawRectX, DrawRectY, DrawRectWidth, DrawRectHeight);
				Contents.DrawShading(AxialShading);
				}

			// draw axial shading bounded by clip path
			else if(NonStroking.GetType() == typeof(PdfAxialShading))
				{
				((PdfAxialShading) NonStroking).SetBoundingBox(DrawRectX, DrawRectY, DrawRectWidth, DrawRectHeight);
				Contents.DrawShading((PdfAxialShading) NonStroking);
				}

			// draw radial gradient brush shading bounded by clip path
			else if(NonStroking.GetType() == typeof(SysMedia.RadialGradientBrush))
				{
				PdfRadialShading RadialShading = new PdfRadialShading(Contents.Document, (SysMedia.RadialGradientBrush) NonStroking);
				RadialShading.SetBoundingBox(DrawRectX, DrawRectY, DrawRectWidth, DrawRectHeight);
				Contents.DrawShading(RadialShading);
				}

			// draw radial shading bounded by clip path
			else
				{
				((PdfRadialShading) NonStroking).SetBoundingBox(DrawRectX, DrawRectY, DrawRectWidth, DrawRectHeight);
				Contents.DrawShading((PdfRadialShading) NonStroking);
				}

			// remove clipping path
			Contents.RestoreGraphicsState();

			// no pen defined
			if(Stroking == null) return;

			// pen is defined
			PaintOperator = PaintOp.Stroke;
			}

		// set paint operator for all other cases (no shading)
		else
			{
			// we have pen and no brush 
			if(NonStroking == null)
				{
				PaintOperator = PaintOp.Stroke;
				}
			// we have brush but no pen
			else if(Stroking == null)
				{
				PaintOperator = FillRule == FillRule.EvenOdd ? PaintOp.FillEor : PaintOp.Fill;
				}
			// we have brush and pen
			else
				{
				PaintOperator = FillRule == FillRule.EvenOdd ? PaintOp.CloseFillStrokeEor: PaintOp.CloseFillStroke;
				}
			}

		// save graphics state
		Contents.SaveGraphicsState();

		// set bland mode
		if(BlendMode != BlendMode.Normal) Contents.SetBlendMode(BlendMode);

		// stroking (pen) is defined
		if(Stroking != null)
			{
			if(Stroking.GetType() == typeof(Color))
				{
				// pen color
				Contents.SetColorStroking((Color) Stroking);

				// set opacity
				if(PenOpacity != 1.0) Contents.SetAlphaStroking(PenOpacity);

				// pen width
				if(PenWidth >= 0) Contents.SetLineWidth(PenWidth);

				// line cap
				if(LineCap != (PdfLineCap) (-1)) Contents.SetLineCap(LineCap);

				// line join
				if(LineJoin != (PdfLineJoin) (-1)) Contents.SetLineJoin(LineJoin);

				// Miter
				if(MiterLimit != -1) Contents.SetMiterLimit(MiterLimit);

				// line is made of dashes
				if(DashArray != null) Contents.SetDashLine(DashArray, DashPhase);
				}

			else if(Stroking.GetType() == typeof(SysMedia.Pen))
				{
				// media pen short cut
				SysMedia.Pen Pen = (SysMedia.Pen) Stroking;

				// media brush shortcut
				SysMedia.SolidColorBrush Brush = (SysMedia.SolidColorBrush) Pen.Brush;

				// media pen color short cut
				SysMedia.Color PenColor = Brush.Color;

				// pen color
				Contents.SetColorStroking(Color.FromArgb(PenColor.R, PenColor.G, PenColor.B));

				// pen opacity
				if(PenColor.A != 255 || Brush.Opacity != 1.0) Contents.SetAlphaStroking((PenColor.A / 255.0) * Brush.Opacity);

				// pen thickness converted to user units
				double Thickness = Pen.Thickness * Math.Max(Math.Abs(ScaleX), Math.Abs(ScaleY));
				Contents.SetLineWidth(Thickness);
				
				// line cap
				// Note: PDF line cap is the same for start and end. We will ignore EndLineCap
				// Triangle line cap will be round
				switch(Pen.StartLineCap)
					{
					case SysMedia.PenLineCap.Flat: Contents.SetLineCap(PdfLineCap.Butt); break;
					case SysMedia.PenLineCap.Square: Contents.SetLineCap(PdfLineCap.Square); break;
					default: Contents.SetLineCap(PdfLineCap.Round); break;
					}

				// line join
				switch(Pen.LineJoin)
					{
					case SysMedia.PenLineJoin.Bevel: Contents.SetLineJoin(PdfLineJoin.Bevel); break;
					case SysMedia.PenLineJoin.Miter: Contents.SetLineJoin(PdfLineJoin.Miter); break;
					default: Contents.SetLineJoin(PdfLineJoin.Round); break;
					}

				// Miter
				Contents.SetMiterLimit(Pen.MiterLimit);

				// dash pattern
				if(Pen.DashStyle.Dashes.Count > 0)
					{
					int End = Pen.DashStyle.Dashes.Count;
					double[] PenDashArray = new double[End];
					for(int Index = 0; Index < End; Index++) PenDashArray[Index] = Thickness * Pen.DashStyle.Dashes[Index];
					Contents.SetDashLine(PenDashArray, Thickness * Pen.DashStyle.Offset);
					}
				}
			}

		// non-stroking (brush) is defined
		// note shading brush was handled above
		if(NonStroking != null)
			{
			// set opacity
			if(BrushOpacity != 1.0) Contents.SetAlphaNonStroking(BrushOpacity);

			// brush color
			if(NonStroking.GetType() == typeof(Color))
				{ 
				Contents.SetColorNonStroking((Color) NonStroking);
				}

			else if(NonStroking.GetType() == typeof(PdfTilingPattern))
				{
				Contents.SetPatternNonStroking((PdfTilingPattern) NonStroking);				
				}
			}

		// build path
		BuildPath(Contents, PaintOperator);

		// restore graphics state
		Contents.RestoreGraphicsState();
		return;
		}

	private void BuildPath
			(
			PdfContents Contents,
			PaintOp		PaintOperator
			)
		{
		// every figure is a separated subpath and contains some segments
		foreach(SysMedia.PathFigure SubPath in MediaPath.Figures)
			{
			// get start of sub-path point
			PointD CurPoint = PathToDrawing(SubPath.StartPoint);
			PointD StartPoint = CurPoint;
			Contents.MoveTo(CurPoint);

			// process all points of one sub-path
			foreach(SysMedia.PathSegment Seg in SubPath.Segments)
				{
				// line segment
				if(Seg.GetType() == typeof(SysMedia.LineSegment))
					{
					CurPoint = PathToDrawing(((SysMedia.LineSegment) Seg).Point);
					Contents.LineTo(CurPoint);
					}

				// polygon
				else if(Seg.GetType() == typeof(SysMedia.PolyLineSegment))
					{
					SysMedia.PolyLineSegment LineSegArray = (SysMedia.PolyLineSegment) Seg;
					foreach(SysWin.Point PolyPoint in LineSegArray.Points)
						{
						CurPoint = PathToDrawing(PolyPoint);
						Contents.LineTo(CurPoint);
						}
					}

				// cubic bezier segment
				else if(Seg.GetType() == typeof(SysMedia.BezierSegment))
					{
					SysMedia.BezierSegment BezierSeg = (SysMedia.BezierSegment) Seg;
					CurPoint = PathToDrawing(BezierSeg.Point3);
					Contents.DrawBezier(PathToDrawing(BezierSeg.Point1), PathToDrawing(BezierSeg.Point2), CurPoint);
					}

				// cubic bezier multi segments
				else if(Seg.GetType() == typeof(SysMedia.PolyBezierSegment))
					{
					SysMedia.PolyBezierSegment BezierSegArray = (SysMedia.PolyBezierSegment) Seg;
					int Count = BezierSegArray.Points.Count;
                    for(int Index = 0; Index < Count; Index += 3)
						{
						CurPoint = PathToDrawing(BezierSegArray.Points[Index + 2]);
						Contents.DrawBezier(PathToDrawing(BezierSegArray.Points[Index]), PathToDrawing(BezierSegArray.Points[Index + 1]), CurPoint);
						}
					}

				// quadratic bezier segment
				else if(Seg.GetType() == typeof(SysMedia.QuadraticBezierSegment))
					{
					SysMedia.QuadraticBezierSegment BezierSeg = (SysMedia.QuadraticBezierSegment) Seg;
					PointD NextPoint = PathToDrawing(BezierSeg.Point2);
					Contents.DrawBezier(new BezierD(CurPoint, PathToDrawing(BezierSeg.Point1), NextPoint), BezierPointOne.Ignore);
					CurPoint = NextPoint;
					}

				// quadratic bezier multi segments
				else if(Seg.GetType() == typeof(SysMedia.PolyQuadraticBezierSegment))
					{
					SysMedia.PolyQuadraticBezierSegment BezierSegArray = (SysMedia.PolyQuadraticBezierSegment) Seg;
					int Count = BezierSegArray.Points.Count;
                    for(int Index = 0; Index < Count; Index += 2)
						{
						PointD NextPoint = PathToDrawing(BezierSegArray.Points[Index + 1]);
						Contents.DrawBezier(new BezierD(CurPoint, PathToDrawing(BezierSegArray.Points[Index]), NextPoint), BezierPointOne.Ignore);
						CurPoint = NextPoint;
						}
					}

				// draw arc
				else if(Seg.GetType() == typeof(SysMedia.ArcSegment))
					{
					SysMedia.ArcSegment Arc = (SysMedia.ArcSegment) Seg;
					PointD NextPoint = PathToDrawing(Arc.Point);
					ArcType ArcType;
					if(Arc.SweepDirection == (PathYAxis == YAxisDirection.Down ? SysMedia.SweepDirection.Counterclockwise : SysMedia.SweepDirection.Clockwise))
						{
						ArcType = Arc.IsLargeArc ? ArcType.LargeCounterClockWise : ArcType.SmallCounterClockWise;
						}
					else
						{
						ArcType = Arc.IsLargeArc ? ArcType.LargeClockWise : ArcType.SmallClockWise;
						}
					Contents.DrawArc(CurPoint, NextPoint, SizeToDrawing(Arc.Size), Arc.RotationAngle, ArcType, BezierPointOne.Ignore);
					CurPoint = NextPoint;
					}

				// should no happen
				else
					{
					throw new ApplicationException("Windows Media path: unknown path segment.");
					}
				}

			// for stroke set paint operator for each sub-path
			if(SubPath.IsClosed) Contents.SetPaintOp(PaintOp.CloseSubPath);
			}

		// paint operator
		Contents.SetPaintOp(PaintOperator);
		return;
		}

	// transformation coefficients from path to drawing
	internal void SetTransformation
			(
			ContentAlignment Alignment
			)
		{
		// preserve aspect ratio
		if(Alignment != 0) SetAspectRatio(Alignment);

		// calculate transformation for x axis
		ScaleX = DrawRectWidth / PathBBoxWidth;
		if(double.IsNaN(ScaleX) || double.IsInfinity(ScaleX)) ScaleX = 0;
		TransX = DrawRectX - PathBBoxX * ScaleX;
		
		// calculate transformation for y axis in down direction
		ScaleY = DrawRectHeight / PathBBoxHeight;
		if(double.IsNaN(ScaleY) || double.IsInfinity(ScaleY)) ScaleY = 0;
		if(PathYAxis == YAxisDirection.Down)
			{
			ScaleY = -ScaleY;
			TransY = DrawRectY - ScaleY * (PathBBoxY + PathBBoxHeight);
			}
		// calculate transformation for y axis in up direction
		else
			{
			TransY = DrawRectY - PathBBoxY * ScaleY;
			}
		return;
		}

	// preserve aspect ratio
	internal void SetAspectRatio
			(
			ContentAlignment Alignment
			)
		{
		// calculate height to fit aspect ratio
		double RatioHeight = DrawRectWidth * PathBBoxHeight / PathBBoxWidth;
		if(RatioHeight == DrawRectHeight) return;
		if(!double.IsNaN(RatioHeight) && !double.IsInfinity(RatioHeight) && RatioHeight < DrawRectHeight)
			{
			if(Alignment == ContentAlignment.MiddleLeft || Alignment == ContentAlignment.MiddleCenter || Alignment == ContentAlignment.MiddleRight)
				{
				DrawRectY += 0.5 * (DrawRectHeight - RatioHeight);
				}
			else if(Alignment == ContentAlignment.TopLeft || Alignment == ContentAlignment.TopCenter || Alignment == ContentAlignment.TopRight)
				{
				DrawRectY += DrawRectHeight - RatioHeight;
				}
			DrawRectHeight = RatioHeight;
			}

		// calculate width to fit aspect ratio
		else
			{
			double RatioWidth = DrawRectHeight * PathBBoxWidth / PathBBoxHeight;
			if(!double.IsNaN(RatioWidth) && !double.IsInfinity(RatioWidth) && RatioWidth < DrawRectWidth)
				{
				if(Alignment == ContentAlignment.TopCenter || Alignment == ContentAlignment.MiddleCenter || Alignment == ContentAlignment.BottomCenter)
					{
					DrawRectX += 0.5 * (DrawRectWidth - RatioWidth);
					}
				else if(Alignment == ContentAlignment.TopRight || Alignment == ContentAlignment.MiddleRight || Alignment == ContentAlignment.BottomRight)
					{
					DrawRectX += DrawRectWidth - RatioWidth;
					}
				}
			DrawRectWidth = RatioWidth;
			}
		return;
		}

	// Transform path point to drawing point
	internal PointD PathToDrawing
			(
			SysWin.Point PathPoint
			)
		{
		return new PointD(ScaleX * PathPoint.X + TransX, ScaleY * PathPoint.Y + TransY);
		}

	internal SizeD SizeToDrawing
			(
			SysWin.Size PathSize
			)
		{
		return new SizeD(Math.Abs(ScaleX) * PathSize.Width, Math.Abs(ScaleY) * PathSize.Height);
		}
	}
}
