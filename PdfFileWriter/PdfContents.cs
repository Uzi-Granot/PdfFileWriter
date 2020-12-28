/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfContents
//	PDF contents indirect object. Support for page contents,
//  X Objects and Tilling Patterns.
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

namespace PdfFileWriter
	{
	/// <summary>
	/// PDF font style flags enumeration
	/// </summary>
	public enum DrawStyle
		{
		/// <summary>
		/// Normal
		/// </summary>
		Normal = 0,

		/// <summary>
		/// Underline
		/// </summary>
		Underline = 4,

		/// <summary>
		/// Strikeout
		/// </summary>
		Strikeout = 8,

		/// <summary>
		/// Subscript
		/// </summary>
		Subscript = 16,

		/// <summary>
		/// Superscript
		/// </summary>
		Superscript = 32
		}

	/// <summary>
	/// Path painting and clipping operators enumeration
	/// </summary>
	/// <remarks>
	/// <para>
	/// Note Special path paining considerations in section 4.4
	/// of the PDF specifications. EOR is even odd rule. Otherwise
	/// it is nonzero winding number rule.
	/// </para>
	/// </remarks>
	public enum PaintOp
		{
		/// <summary>
		/// No operator
		/// </summary>
		NoOperator,

		/// <summary>
		/// No paint
		/// </summary>
		NoPaint,            // n

		/// <summary>
		/// Stoke
		/// </summary>
		Stroke,             // S

		/// <summary>
		/// Close and stroke
		/// </summary>
		CloseStroke,        // s

		/// <summary>
		/// close and Fill
		/// </summary>
		Fill,               // f

		/// <summary>
		/// close and fill EOR
		/// </summary>
		FillEor,            // f*

		/// <summary>
		/// Fill and stoke
		/// </summary>
		FillStroke,         // B

		/// <summary>
		/// Fill and stroke EOR
		/// </summary>
		FillStrokeEor,      // B*

		/// <summary>
		/// Close, Fill and stroke
		/// </summary>
		CloseFillStroke,    // b

		/// <summary>
		/// Close, Fill and Stroke EOR
		/// </summary>
		CloseFillStrokeEor, // b*

		/// <summary>
		/// Clip path
		/// </summary>
		ClipPathWnr,        // h W n

		/// <summary>
		/// Clip path EOR
		/// </summary>
		ClipPathEor,        // h W* n

		/// <summary>
		/// Close sub-path
		/// </summary>
		CloseSubPath,       // h
		}

	/// <summary>
	/// PDF line cap enumeration
	/// </summary>
	public enum PdfLineCap
		{
		/// <summary>
		/// Butt
		/// </summary>
		Butt,

		/// <summary>
		/// Round
		/// </summary>
		Round,

		/// <summary>
		/// Square
		/// </summary>
		Square,
		}

	/// <summary>
	/// PDF line join enumeration
	/// </summary>
	public enum PdfLineJoin
		{
		/// <summary>
		/// Miter
		/// </summary>
		Miter,

		/// <summary>
		/// Round
		/// </summary>
		Round,

		/// <summary>
		/// Bevel
		/// </summary>
		Bevel,
		}

	/// <summary>
	/// Text rendering enumeration
	/// </summary>
	public enum TextRendering
		{
		/// <summary>
		/// Fill
		/// </summary>
		Fill,

		/// <summary>
		/// Stroke
		/// </summary>
		Stroke,

		/// <summary>
		/// Fill and stroke
		/// </summary>
		FillStroke,

		/// <summary>
		/// Invisible
		/// </summary>
		Invisible,

		/// <summary>
		/// Fill and clip
		/// </summary>
		FillClip,

		/// <summary>
		/// Stroke and clip
		/// </summary>
		StrokeClip,

		/// <summary>
		/// Fill, stroke and clip
		/// </summary>
		FillStrokeClip,

		/// <summary>
		/// Clip
		/// </summary>
		Clip
		}

	/// <summary>
	/// Text justify enumeration
	/// </summary>
	public enum TextJustify
		{
		/// <summary>
		/// Left
		/// </summary>
		Left,

		/// <summary>
		/// Center
		/// </summary>
		Center,

		/// <summary>
		/// Right
		/// </summary>
		Right,
		}

	/// <summary>
	/// TextBox justify enumeration
	/// </summary>
	/// <remarks>The first three must be the same as TextJustify
	/// </remarks>
	public enum TextBoxJustify
		{
		/// <summary>
		/// Left
		/// </summary>
		Left,

		/// <summary>
		/// Center
		/// </summary>
		Center,

		/// <summary>
		/// Right
		/// </summary>
		Right,

		/// <summary>
		/// Fit to width
		/// </summary>
		FitToWidth
		}

	/// <summary>
	/// Draw Bezier point one control enumeration
	/// </summary>
	public enum BezierPointOne
		{
		/// <summary>
		/// Ignore
		/// </summary>
		Ignore,

		/// <summary>
		/// Move to
		/// </summary>
		MoveTo,

		/// <summary>
		/// Line to
		/// </summary>
		LineTo
		}

	/// <summary>
	/// Blend mode enumeration
	/// </summary>
	/// <remarks>See Blend Mode section of the PDF specifications menual.</remarks>
	public enum BlendMode
		{
		/// <summary>
		/// Normal (no blend)
		/// </summary>
		Normal,
		/// <summary>
		/// Multiply
		/// </summary>
		Multiply,
		/// <summary>
		/// Screen
		/// </summary>
		Screen,
		/// <summary>
		/// Overlay
		/// </summary>
		Overlay,
		/// <summary>
		/// Darken
		/// </summary>
		Darken,
		/// <summary>
		/// Lighten
		/// </summary>
		Lighten,
		/// <summary>
		/// Color Dodge
		/// </summary>
		ColorDodge,
		/// <summary>
		/// Color burn
		/// </summary>
		ColorBurn,
		/// <summary>
		/// Hard light
		/// </summary>
		HardLight,
		/// <summary>
		/// Soft light
		/// </summary>
		SoftLight,
		/// <summary>
		/// Difference
		/// </summary>
		Difference,
		/// <summary>
		/// Exclusion
		/// </summary>
		Exclusion,
		}

	/// <summary>
	/// PDF contents class
	/// </summary>
	public class PdfContents : PdfObject
		{
		 // true for page contents, false for X objects and pattern
		internal bool PageContents;

		// used resource objects
		internal List<PdfObject> ResObjects;

		// must be in the same order as PaintOp enumeration
		private static string[] PaintStr = new string[] { "", "n", "S", "s", "f", "f*", "B", "B*", "b", "b*", "h W n", "h W* n", "h" };

		/// <summary>
		/// PdfContents constructor for page contents
		/// </summary>
		/// <param name="Page">Page parent</param>
		public PdfContents
				(
				PdfPage Page
				) : base(Page.Document, ObjectType.Stream)
			{
			// set page contents flag
			PageContents = true;

			// add contents to page's list of contents
			Page.AddContents(this);

			// exit
			return;
			}

		/// <summary>
		/// PdfContents constructor unattached
		/// </summary>
		/// <param name="Document">Current PdfDocument</param>
		/// <remarks>
		/// This contents object must be explicitly attached to a page object
		/// </remarks>
		public PdfContents
				(
				PdfDocument Document
				) : base(Document, ObjectType.Stream) { }

		// Constructor for XObject or Pattern
		internal PdfContents
				(
				PdfDocument Document,
				string PdfObjectType
				) : base(Document, ObjectType.Stream, PdfObjectType) { }

		/// <summary>
		/// Save graphics state
		/// </summary>
		public void SaveGraphicsState()
			{
			ObjectValueAppend("q\n");
			return;
			}

		/// <summary>
		/// Restore graphics state
		/// </summary>
		public void RestoreGraphicsState()
			{
			ObjectValueAppend("Q\n");
			return;
			}

		/// <summary>
		/// Layer start
		/// </summary>
		/// <param name="Layer">Layer object</param>
		public void LayerStart
				(
				PdfLayer Layer
				)
			{
			// add to list of resources
			AddToUsedResources(Layer);

			// write to content stream
			ObjectValueFormat("/OC {0} BDC\n", Layer.ResourceCode);
			return;
			}

		/// <summary>
		/// Layer end
		/// </summary>
		public void LayerEnd()
			{
			// write to content stream
			ObjectValueFormat("EMC\n");
			return;
			}

		/// <summary>
		/// Convert PaintOp enumeration to String
		/// </summary>
		/// <param name="PP">Paint operator</param>
		/// <returns>Paint operator string</returns>
		public string PaintOpStr
				(
				PaintOp PP
				)
			{
			// apply paint operator
			return PaintStr[(int) PP];
			}

		/// <summary>
		/// Set paint operator
		/// </summary>
		/// <param name="PP">Paint operator</param>
		public void SetPaintOp
				(
				PaintOp PP
				)
			{
			// apply paint operator
			if(PP != PaintOp.NoOperator) ObjectValueFormat("{0}\n", PaintStr[(int) PP]);
			return;
			}

		/// <summary>
		/// Set line width
		/// </summary>
		/// <param name="Width">Line width</param>
		/// <remarks>
		/// Set line width for future path operations
		/// </remarks>
		public void SetLineWidth
				(
				double Width
				)
			{
			ObjectValueFormat("{0} w\n", ToPt(Width));
			return;
			}

		/// <summary>
		/// Set line cap
		/// </summary>
		/// <param name="LineCap">Line cap enumeration</param>
		public void SetLineCap
				(
				PdfLineCap LineCap
				)
			{
			ObjectValueFormat("{0} J\n", (int) LineCap);
			return;
			}

		/// <summary>
		/// Set line join
		/// </summary>
		/// <param name="LineJoin">Set line join enumeration</param>
		public void SetLineJoin
				(
				PdfLineJoin LineJoin
				)
			{
			ObjectValueFormat("{0} j\n", (int) LineJoin);
			return;
			}

		/// <summary>
		/// Set miter limit
		/// </summary>
		/// <param name="MiterLimit">Miter limit</param>
		public void SetMiterLimit
				(
				double MiterLimit       // default 10.0
				)
			{
			ObjectValueFormat("{0} M\n", Round(MiterLimit));
			return;
			}

		/// <summary>
		/// Set dash line pattern
		/// </summary>
		/// <param name="DashArray">Dash array</param>
		/// <param name="DashPhase">Dash phase</param>
		public void SetDashLine
				(
				double[] DashArray,     // default []
				double DashPhase        // default 0
				)
			{
			// restore default condition of solid line
			if(DashArray == null || DashArray.Length == 0)
				{
				ObjectValueAppend("[] 0 d\n");
				}

			// dash line
			else
				{
				ObjectValueList.Add((byte) '[');
				foreach(double Value in DashArray) ObjectValueFormat("{0} ", ToPt(Value));
				ObjectValueFormat("] {0} d\n", ToPt(DashPhase));
				}
			return;
			}

		/// <summary>
		/// Set gray level for non stroking (fill or brush) operations
		/// </summary>
		/// <param name="GrayLevel">Gray level (0.0 to 1.0)</param>
		/// <remarks>
		/// Gray level must be 0.0 (black) to 1.0 (white).
		/// </remarks>
		public void GrayLevelNonStroking
				(
				double GrayLevel
				)
			{
			ObjectValueFormat("{0} g\n", Round(GrayLevel));
			return;
			}

		/// <summary>
		/// Set gray level for stroking (outline or pen) operations
		/// </summary>
		/// <param name="GrayLevel">Gray level (0.0 to 1.0)</param>
		/// <remarks>
		/// Gray level must be 0.0 (black) to 1.0 (white).
		/// </remarks>
		public void GrayLevelStroking
				(
				double GrayLevel
				)
			{
			ObjectValueFormat("{0} G\n", Round(GrayLevel));
			return;
			}

		/// <summary>
		/// Set color for non stroking (fill or brush) operations
		/// </summary>
		/// <param name="Color">Color</param>
		/// <remarks>Set red, green and blue components. Alpha is ignored</remarks>
		public void SetColorNonStroking
				(
				Color Color
				)
			{
			ObjectValueFormat("{0} {1} {2} rg\n", Round(Color.R / 255.0), Round(Color.G / 255.0), Round(Color.B / 255.0));
			return;
			}

		/// <summary>
		/// Set color for stroking (outline or pen) operations
		/// </summary>
		/// <param name="Color">Color</param>
		/// <remarks>Set red, green and blue components. Alpha is ignored</remarks>
		public void SetColorStroking
				(
				Color Color
				)
			{
			ObjectValueFormat("{0} {1} {2} RG\n", Round(Color.R / 255.0), Round(Color.G / 255.0), Round(Color.B / 255.0));
			return;
			}

		/// <summary>
		/// Set opacity value (alpha) of color of for stroking operations
		/// </summary>
		/// <param name="Color">Color value</param>
		/// <remarks>Set alpha component. Ignore red, green and blue.</remarks>
		public void SetAlphaStroking
				(
				Color Color
				)
			{
			SetAlphaStroking(Color.A / 255.0);
			return;
			}

		/// <summary>
		/// Set opacity value for stroking operations
		/// </summary>
		/// <param name="Alpha">Opacity value 0.0=transparent to 1.0=Opaque</param>
		public void SetAlphaStroking
				(
				double Alpha
				)
			{
			string AlphaStr;
			if(Alpha < 0.001) AlphaStr = "0";
			else if(Alpha > 0.999) AlphaStr = "1";
			else AlphaStr = Alpha.ToString("0.0##", NFI.PeriodDecSep);
			PdfExtGState ExtGState = PdfExtGState.CreateExtGState(Document, "/CA", AlphaStr);
			AddToUsedResources(ExtGState);
			ObjectValueFormat("{0} gs\n", ExtGState.ResourceCode);
			return;
			}

		/// <summary>
		/// Set opacity value (alpha) of color of for non-stroking operations
		/// </summary>
		/// <param name="Color">Color value</param>
		/// <remarks>Set alpha component. Ignore red, green and blue.</remarks>
		public void SetAlphaNonStroking
				(
				Color Color
				)
			{
			SetAlphaNonStroking(Color.A / 255.0);
			return;
			}

		/// <summary>
		/// Set opacity value for non-stroking operations
		/// </summary>
		/// <param name="Alpha">Opacity value 0.0=transparent to 1.0=Opaque</param>
		public void SetAlphaNonStroking
				(
				double Alpha
				)
			{
			string AlphaStr;
			if(Alpha < 0.001) AlphaStr = "0";
			else if(Alpha > 0.999) AlphaStr = "1";
			else AlphaStr = Alpha.ToString("0.0##", NFI.PeriodDecSep);
			PdfExtGState ExtGState = PdfExtGState.CreateExtGState(Document, "/ca", AlphaStr);
			AddToUsedResources(ExtGState);
			ObjectValueFormat("{0} gs\n", ExtGState.ResourceCode);
			return;
			}

		/// <summary>
		/// Set color blend mode
		/// </summary>
		/// <param name="Blend">Blend method enumeration</param>
		public void SetBlendMode
				(
				BlendMode Blend
				)
			{
			PdfExtGState ExtGState = PdfExtGState.CreateExtGState(Document, "/BM", "/" + Blend.ToString());
			AddToUsedResources(ExtGState);
			ObjectValueFormat("{0} gs\n", ExtGState.ResourceCode);
			return;
			}

		/// <summary>
		/// Set pattern for non stroking (fill) operations
		/// </summary>
		/// <param name="Pattern">Pattern resource</param>
		public void SetPatternNonStroking
				(
				PdfTilingPattern Pattern
				)
			{
			AddToUsedResources(Pattern);
			ObjectValueFormat("/Pattern cs {0} scn\n", Pattern.ResourceCode);
			return;
			}

		/// <summary>
		/// Set pattern for stroking (outline) operations
		/// </summary>
		/// <param name="Pattern">Pattern resource</param>
		public void SetPatternStroking
				(
				PdfContents Pattern
				)
			{
			AddToUsedResources(Pattern);
			ObjectValueFormat("/Pattern CS {0} SCN\n", Pattern.ResourceCode);
			return;
			}

		/// <summary>
		/// Draw axial shading pattern
		/// </summary>
		/// <param name="Shading">Axial shading resource</param>
		public void DrawShading
				(
				PdfAxialShading Shading
				)
			{
			AddToUsedResources(Shading);
			ObjectValueFormat("{0} sh\n", Shading.ResourceCode);
			return;
			}

		/// <summary>
		/// Draw radial shading pattern
		/// </summary>
		/// <param name="Shading">Radial shading resource</param>
		public void DrawShading
				(
				PdfRadialShading Shading
				)
			{
			AddToUsedResources(Shading);
			ObjectValueFormat("{0} sh\n", Shading.ResourceCode);
			return;
			}

		/// <summary>
		/// Set current transformation matrix
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
		public void SetTransMatrix
				(
				double a,   // ScaleX * Cos(Rotate)
				double b,   // ScaleX * Sin(Rotate)
				double c,   // ScaleY * (-Sin(Rotate))
				double d,   // ScaleY * Cos(Rotate)
				double e,
				double f
				)
			{
			ObjectValueFormat("{0} {1} {2} {3} {4} {5} cm\n", Round(a), Round(b), Round(c), Round(d), ToPt(e), ToPt(f));
			return;
			}

		/// <summary>
		/// Translate origin
		/// </summary>
		/// <param name="Orig">New origin</param>
		public void Translate
				(
				PointD Orig
				)
			{
			Translate(Orig.X, Orig.Y);
			return;
			}

		/// <summary>
		/// Translate origin
		/// </summary>
		/// <param name="OriginX">Origin X</param>
		/// <param name="OriginY">Origin Y</param>
		public void Translate
				(
				double OriginX,
				double OriginY
				)
			{
			ObjectValueFormat("1 0 0 1 {0} {1} cm\n", ToPt(OriginX), ToPt(OriginY));
			return;
			}

		/// <summary>
		/// Scale
		/// </summary>
		/// <param name="Scale">New scale</param>
		public void Scale
				(
				double Scale
				)
			{
			ObjectValueFormat("{0} 0 0 {0} 0 0 cm\n", Round(Scale));
			return;
			}

		/// <summary>
		/// Translate and scale
		/// </summary>
		/// <param name="Orig">Origin point</param>
		/// <param name="Scale">Scale</param>
		public void TranslateScale
				(
				PointD Orig,
				double Scale
				)
			{
			TranslateScale(Orig.X, Orig.Y, Scale);
			return;
			}

		/// <summary>
		/// Translate and scale
		/// </summary>
		/// <param name="OriginX">Origin X</param>
		/// <param name="OriginY">Origin Y</param>
		/// <param name="Scale">Scale</param>
		public void TranslateScale
				(
				double OriginX,
				double OriginY,
				double Scale
				)
			{
			ObjectValueFormat("{2} 0 0 {2} {0} {1} cm\n", ToPt(OriginX), ToPt(OriginY), Round(Scale));
			return;
			}

		/// <summary>
		/// Translate and scale
		/// </summary>
		/// <param name="Orig">Origin point</param>
		/// <param name="ScaleX">Horizontal scale</param>
		/// <param name="ScaleY">Vertical scale</param>
		public void TranslateScale
				(
				PointD Orig,
				double ScaleX,
				double ScaleY
				)
			{
			TranslateScale(Orig.X, Orig.Y, ScaleX, ScaleY);
			return;
			}

		/// <summary>
		/// Translate and scale
		/// </summary>
		/// <param name="OriginX">Origin X</param>
		/// <param name="OriginY">Origin Y</param>
		/// <param name="ScaleX">Horizontal scale</param>
		/// <param name="ScaleY">Vertical scale</param>
		public void TranslateScale
				(
				double OriginX,
				double OriginY,
				double ScaleX,
				double ScaleY
				)
			{
			ObjectValueFormat("{2} 0 0 {3} {0} {1} cm\n", ToPt(OriginX), ToPt(OriginY), Round(ScaleX), Round(ScaleY));
			return;
			}

		/// <summary>
		/// Translate, scale and rotate
		/// </summary>
		/// <param name="Orig">Origin point</param>
		/// <param name="Scale">Scale</param>
		/// <param name="Rotate">Rotate (radians)</param>
		public void TranslateScaleRotate
				(
				PointD Orig,
				double Scale,
				double Rotate       // radians
				)
			{
			TranslateScaleRotate(Orig.X, Orig.Y, Scale, Rotate);
			return;
			}

		/// <summary>
		/// Translate, scale and rotate
		/// </summary>
		/// <param name="OriginX">Origin X</param>
		/// <param name="OriginY">Origin Y</param>
		/// <param name="Scale">Scale</param>
		/// <param name="Rotate">Rotate (radians)</param>
		public void TranslateScaleRotate
				(
				double OriginX,
				double OriginY,
				double Scale,
				double Rotate
				)
			{
			ObjectValueFormat("{2} {3} {4} {2} {0} {1} cm\n", ToPt(OriginX), ToPt(OriginY),
				Round(Scale * Math.Cos(Rotate)), Round(Scale * Math.Sin(Rotate)), Round(Scale * Math.Sin(-Rotate)));
			return;
			}

		/// <summary>
		/// Translate, scale and rotate
		/// </summary>
		/// <param name="OriginX">Origin X</param>
		/// <param name="OriginY">Origin Y</param>
		/// <param name="ScaleX">Horizontal scale</param>
		/// <param name="ScaleY">Vertical scale</param>
		/// <param name="Rotate">Rotate (radians)</param>
		public void TranslateScaleRotate
				(
				double OriginX,
				double OriginY,
				double ScaleX,
				double ScaleY,
				double Rotate
				)
			{
			ObjectValueFormat("{2} {3} {4} {5} {0} {1} cm\n", ToPt(OriginX), ToPt(OriginY),
				Round(ScaleX * Math.Cos(Rotate)), Round(ScaleY * Math.Sin(Rotate)), Round(ScaleX * Math.Sin(-Rotate)), Round(ScaleY * Math.Cos(Rotate)));
			return;
			}

		/// <summary>
		/// Move current pointer to new position
		/// </summary>
		/// <param name="Point">New point</param>
		public void MoveTo
				(
				PointD Point
				)
			{
			MoveTo(Point.X, Point.Y);
			return;
			}

		/// <summary>
		/// Move current pointer to new position
		/// </summary>
		/// <param name="X">New X position</param>
		/// <param name="Y">New Y position</param>
		public void MoveTo
				(
				double X,
				double Y
				)
			{
			ObjectValueFormat("{0} {1} m\n", ToPt(X), ToPt(Y));
			return;
			}

		/// <summary>
		/// Draw line from last position to new position
		/// </summary>
		/// <param name="Point">New point</param>
		public void LineTo
				(
				PointD Point
				)
			{
			LineTo(Point.X, Point.Y);
			return;
			}

		/// <summary>
		/// Draw line from last position to new position
		/// </summary>
		/// <param name="X">New X position</param>
		/// <param name="Y">New Y position</param>
		public void LineTo
				(
				double X,
				double Y
				)
			{
			ObjectValueFormat("{0} {1} l\n", ToPt(X), ToPt(Y));
			return;
			}

		/// <summary>
		/// Draw Bezier cubic path
		/// </summary>
		/// <param name="Bezier">Bezier object</param>
		/// <param name="Point1Action">Point1 action</param>
		public void DrawBezier
				(
				BezierD Bezier,
				BezierPointOne Point1Action
				)
			{
			switch(Point1Action)
				{
				case BezierPointOne.MoveTo:
					MoveTo(Bezier.P1.X, Bezier.P1.Y);
					break;

				case BezierPointOne.LineTo:
					LineTo(Bezier.P1.X, Bezier.P1.Y);
					break;
				}

			DrawBezier(Bezier.P2.X, Bezier.P2.Y, Bezier.P3.X, Bezier.P3.Y, Bezier.P4.X, Bezier.P4.Y);
			return;
			}

		/// <summary>
		/// Draw Bezier cubic path
		/// </summary>
		/// <param name="P1">Point 1</param>
		/// <param name="P2">Point 2</param>
		/// <param name="P3">Point 3</param>
		public void DrawBezier
				(
				PointD P1,
				PointD P2,
				PointD P3
				)
			{
			DrawBezier(P1.X, P1.Y, P2.X, P2.Y, P3.X, P3.Y);
			return;
			}

		/// <summary>
		/// Draw Bezier cubic path
		/// </summary>
		/// <param name="X1">Point 1 X</param>
		/// <param name="Y1">Point 1 Y</param>
		/// <param name="X2">Point 2 X</param>
		/// <param name="Y2">Point 2 Y</param>
		/// <param name="X3">Point 3 X</param>
		/// <param name="Y3">Point 3 Y</param>
		public void DrawBezier
				(
				double X1,
				double Y1,
				double X2,
				double Y2,
				double X3,
				double Y3
				)
			{
			ObjectValueFormat("{0} {1} {2} {3} {4} {5} c\n", ToPt(X1), ToPt(Y1), ToPt(X2), ToPt(Y2), ToPt(X3), ToPt(Y3));
			return;
			}

		/// <summary>
		/// Draw Bezier cubic path (P1 is the same as current point)
		/// </summary>
		/// <param name="P2">Point 2</param>
		/// <param name="P3">Point 3</param>
		public void DrawBezierNoP1
				(
				PointD P2,
				PointD P3
				)
			{
			DrawBezierNoP1(P2.X, P2.Y, P3.X, P3.Y);
			return;
			}

		/// <summary>
		/// Draw Bezier cubic path (P1 is the same as current point)
		/// </summary>
		/// <param name="X2">Point 2 X</param>
		/// <param name="Y2">Point 2 Y</param>
		/// <param name="X3">Point 3 X</param>
		/// <param name="Y3">Point 3 Y</param>
		public void DrawBezierNoP1
				(
				double X2,
				double Y2,
				double X3,
				double Y3
				)
			{
			ObjectValueFormat("{0} {1} {2} {3} v\n", ToPt(X2), ToPt(Y2), ToPt(X3), ToPt(Y3));
			return;
			}

		/// <summary>
		/// Draw Bezier cubic path (P2 is the same as P3)
		/// </summary>
		/// <param name="P1">Point 1</param>
		/// <param name="P3">Point 3</param>
		public void DrawBezierNoP2
				(
				PointD P1,
				PointD P3
				)
			{
			DrawBezierNoP2(P1.X, P1.Y, P3.X, P3.Y);
			return;
			}

		/// <summary>
		/// Draw Bezier cubic path (P2 is the same as P3)
		/// </summary>
		/// <param name="X1">Point 1 X</param>
		/// <param name="Y1">Point 1 Y</param>
		/// <param name="X3">Point 3 X</param>
		/// <param name="Y3">Point 3 Y</param>
		public void DrawBezierNoP2
				(
				double X1,
				double Y1,
				double X3,
				double Y3
				)
			{
			ObjectValueFormat("{0} {1} {2} {3} y\n", ToPt(X1), ToPt(Y1), ToPt(X3), ToPt(Y3));
			return;
			}

		/// <summary>
		/// Draw arc
		/// </summary>
		/// <param name="ArcStart">Arc start point</param>
		/// <param name="ArcEnd">Arc end point</param>
		/// <param name="Radius">RadiusX as width and RadiusY as height</param>
		/// <param name="Rotate">X axis rotation angle in radians</param>
		/// <param name="Type">Arc type enumeration</param>
		/// <param name="OutputStartPoint">Output start point</param>
		public void DrawArc
				(
				PointD ArcStart,
				PointD ArcEnd,
				SizeD Radius,
				double Rotate,
				ArcType Type,
				BezierPointOne OutputStartPoint
				)
			{
			// starting point
			switch(OutputStartPoint)
				{
				case BezierPointOne.MoveTo:
					MoveTo(ArcStart.X, ArcStart.Y);
					break;

				case BezierPointOne.LineTo:
					LineTo(ArcStart.X, ArcStart.Y);
					break;
				}

			// create arc
			PointD[] SegArray = ArcToBezier.CreateArc(ArcStart, ArcEnd, Radius, Rotate, Type);

			// output
			for(int Index = 1; Index < SegArray.Length;)
				DrawBezier(SegArray[Index].X, SegArray[Index++].Y, SegArray[Index].X, SegArray[Index++].Y, SegArray[Index].X, SegArray[Index++].Y);
			return;
			}

		/// <summary>
		/// Draw line
		/// </summary>
		/// <param name="Line">Line object</param>
		public void DrawLine
				(
				LineD Line
				)
			{
			DrawLine(Line.P1.X, Line.P1.Y, Line.P2.X, Line.P2.Y);
			return;
			}

		/// <summary>
		/// Draw line
		/// </summary>
		/// <param name="P1">Point 1</param>
		/// <param name="P2">Point 2</param>
		public void DrawLine
				(
				PointD P1,
				PointD P2
				)
			{
			DrawLine(P1.X, P1.Y, P2.X, P2.Y);
			return;
			}

		/// <summary>
		/// Draw line
		/// </summary>
		/// <param name="X1">Point 1 X</param>
		/// <param name="Y1">Point 1 Y</param>
		/// <param name="X2">Point 2 X</param>
		/// <param name="Y2">Point 2 X</param>
		public void DrawLine
				(
				double X1,
				double Y1,
				double X2,
				double Y2
				)
			{
			ObjectValueFormat("{0} {1} m {2} {3} l S\n", ToPt(X1), ToPt(Y1), ToPt(X2), ToPt(Y2));
			return;
			}

		/// <summary>
		/// Draw line with given line width
		/// </summary>
		/// <param name="Line">Line</param>
		/// <param name="LineWidth">Line width</param>
		public void DrawLine
				(
				LineD Line,
				double LineWidth
				)
			{
			DrawLine(Line.P1.X, Line.P1.Y, Line.P2.X, Line.P2.Y, LineWidth);
			return;
			}

		/// <summary>
		/// Draw line with given line width
		/// </summary>
		/// <param name="P1">Point 1</param>
		/// <param name="P2">Point 2</param>
		/// <param name="LineWidth">Line width</param>
		public void DrawLine
				(
				PointD P1,
				PointD P2,
				double LineWidth
				)
			{
			DrawLine(P1.X, P1.Y, P2.X, P2.Y, LineWidth);
			return;
			}

		/// <summary>
		/// Draw line with given line width
		/// </summary>
		/// <param name="X1">Point 1 X</param>
		/// <param name="Y1">Point 1 Y</param>
		/// <param name="X2">Point 2 X</param>
		/// <param name="Y2">Point 2 X</param>
		/// <param name="LineWidth">Line width</param>
		public void DrawLine
				(
				double X1,
				double Y1,
				double X2,
				double Y2,
				double LineWidth
				)
			{
			ObjectValueFormat("q {0} w {1} {2} m {3} {4} l S Q\n", ToPt(LineWidth), ToPt(X1), ToPt(Y1), ToPt(X2), ToPt(Y2));
			return;
			}

		/// <summary>
		/// Draw border line 
		/// </summary>
		/// <param name="X1">Point 1 X</param>
		/// <param name="Y1">Point 1 Y</param>
		/// <param name="X2">Point 2 X</param>
		/// <param name="Y2">Point 2 X</param>
		/// <param name="BorderStyle">PdfTableBorderStyle</param>
		public void DrawLine
				(
				double X1,
				double Y1,
				double X2,
				double Y2,
				PdfTableBorderStyle BorderStyle
				)
			{
			if(BorderStyle.Display)
				{
				ObjectValueFormat("q {0} w {1} {2} {3} RG 0 J {4} {5} m {6} {7} l S Q\n",
					ToPt(BorderStyle.Width), Round((double) BorderStyle.Color.R / 255.0), Round((double) BorderStyle.Color.G / 255.0),
					Round((double) BorderStyle.Color.B / 255.0), ToPt(X1), ToPt(Y1), ToPt(X2), ToPt(Y2));
				}
			return;
			}

		/// <summary>
		/// Draw rectangle
		/// </summary>
		/// <param name="Origin">Origin (left-bottom)</param>
		/// <param name="Size">Size</param>
		/// <param name="PP">Paint operator</param>
		public void DrawRectangle
				(
				PointD Origin,
				SizeD Size,
				PaintOp PP
				)
			{
			DrawRectangle(Origin.X, Origin.Y, Size.Width, Size.Height, PP);
			return;
			}

		/// <summary>
		/// Draw Rectangle
		/// </summary>
		/// <param name="OriginX">Origin X (left)</param>
		/// <param name="OriginY">Origin Y (bottom)</param>
		/// <param name="Width">Width</param>
		/// <param name="Height">Height</param>
		/// <param name="PP">Paint operator</param>
		public void DrawRectangle
				(
				double OriginX,
				double OriginY,
				double Width,
				double Height,
				PaintOp PP
				)
			{
			// draw rectangle
			ObjectValueFormat("{0} {1} {2} {3} re {4}\n", ToPt(OriginX), ToPt(OriginY), ToPt(Width), ToPt(Height), PaintOpStr(PP));
			return;
			}

		/// <summary>
		/// Draw oval
		/// </summary>
		/// <param name="Origin">Origin (left-bottom)</param>
		/// <param name="Size">Size</param>
		/// <param name="PP">Paint operator</param>
		public void DrawOval
				(
				PointD Origin,
				SizeD Size,
				PaintOp PP
				)
			{
			DrawOval(Origin.X, Origin.Y, Size.Width, Size.Height, PP);
			return;
			}

		/// <summary>
		/// Draw oval
		/// </summary>
		/// <param name="OriginX">Origin X (left)</param>
		/// <param name="OriginY">Origin Y (bottom)</param>
		/// <param name="Width">Width</param>
		/// <param name="Height">Height</param>
		/// <param name="PP">Paint operator</param>
		public void DrawOval
				(
				double OriginX,
				double OriginY,
				double Width,
				double Height,
				PaintOp PP
				)
			{
			Width /= 2;
			Height /= 2;
			OriginX += Width;
			OriginY += Height;
			DrawBezier(BezierD.OvalFirstQuarter(OriginX, OriginY, Width, Height), BezierPointOne.MoveTo);
			DrawBezier(BezierD.OvalSecondQuarter(OriginX, OriginY, Width, Height), BezierPointOne.Ignore);
			DrawBezier(BezierD.OvalThirdQuarter(OriginX, OriginY, Width, Height), BezierPointOne.Ignore);
			DrawBezier(BezierD.OvalFourthQuarter(OriginX, OriginY, Width, Height), BezierPointOne.Ignore);
			SetPaintOp(PP);
			return;
			}

		/// <summary>
		/// Draw heart
		/// </summary>
		/// <param name="CenterLine">Center line</param>
		/// <param name="PP">Paint operator</param>
		public void DrawHeart
				(
				LineD CenterLine,
				PaintOp PP
				)
			{
			// PI / 1.5 = 120 deg and PI / 2 = 90 deg
			DrawDoubleBezierPath(CenterLine, 1.0, Math.PI / 1.5, 1.0, 0.5 * Math.PI, PP);
			return;
			}

		/// <summary>
		/// Draw heart
		/// </summary>
		/// <param name="CenterLineTopX">Center line top X</param>
		/// <param name="CenterLineTopY">Center line top Y</param>
		/// <param name="CenterLineBottomX">Center line bottom X</param>
		/// <param name="CenterLineBottomY">Center line bottom Y</param>
		/// <param name="PP">Paint operator</param>
		public void DrawHeart
				(
				double CenterLineTopX,
				double CenterLineTopY,
				double CenterLineBottomX,
				double CenterLineBottomY,
				PaintOp PP
				)
			{
			DrawHeart(new LineD(CenterLineTopX, CenterLineTopY, CenterLineBottomX, CenterLineBottomY), PP);
			return;
			}

		/// <summary>
		/// Draw double Bezier path
		/// </summary>
		/// <param name="CenterLine">Center line</param>
		/// <param name="Factor1">Factor 1</param>
		/// <param name="Alpha1">Alpha 1</param>
		/// <param name="Factor2">Factor 2</param>
		/// <param name="Alpha2">Alpha 2</param>
		/// <param name="PP">Paint operator</param>
		public void DrawDoubleBezierPath
				(
				LineD CenterLine,
				double Factor1,
				double Alpha1,
				double Factor2,
				double Alpha2,
				PaintOp PP
				)
			{
			// two symmetric Bezier curves
			DrawBezier(new BezierD(CenterLine.P1, Factor1, -0.5 * Alpha1, Factor2, -0.5 * Alpha2, CenterLine.P2), BezierPointOne.MoveTo);
			DrawBezier(new BezierD(CenterLine.P2, Factor2, Math.PI + 0.5 * Alpha2, Factor1, Math.PI + 0.5 * Alpha1, CenterLine.P1), BezierPointOne.Ignore);

			// set paint operator
			SetPaintOp(PP);
			return;
			}

		/// <summary>
		/// Draw Rounded Rectangle
		/// </summary>
		/// <param name="Origin">Origin (left-bottom)</param>
		/// <param name="Size">Size</param>
		/// <param name="Radius">Radius</param>
		/// <param name="PP">Paint operator</param>
		public void DrawRoundedRectangle
				(
				PointD Origin,
				SizeD Size,
				double Radius,
				PaintOp PP
				)
			{
			DrawRoundedRectangle(Origin.X, Origin.Y, Size.Width, Size.Height, Radius, PP);
			return;
			}

		/// <summary>
		/// Draw Rounded Rectangle
		/// </summary>
		/// <param name="OriginX">Origin X (left)</param>
		/// <param name="OriginY">Origin Y (right)</param>
		/// <param name="Width">Width</param>
		/// <param name="Height">Height</param>
		/// <param name="Radius">Radius</param>
		/// <param name="PP">Paint operator</param>
		public void DrawRoundedRectangle
				(
				double OriginX,
				double OriginY,
				double Width,
				double Height,
				double Radius,
				PaintOp PP
				)
			{
			// make sure radius is not too big
			if(Radius > 0.5 * Width) Radius = 0.5 * Width;
			if(Radius > 0.5 * Height) Radius = 0.5 * Height;

			// draw path
			MoveTo(OriginX + Radius, OriginY);
			DrawBezier(BezierD.CircleFourthQuarter(OriginX + Width - Radius, OriginY + Radius, Radius), BezierPointOne.LineTo);
			DrawBezier(BezierD.CircleFirstQuarter(OriginX + Width - Radius, OriginY + Height - Radius, Radius), BezierPointOne.LineTo);
			DrawBezier(BezierD.CircleSecondQuarter(OriginX + Radius, OriginY + Height - Radius, Radius), BezierPointOne.LineTo);
			DrawBezier(BezierD.CircleThirdQuarter(OriginX + Radius, OriginY + Radius, Radius), BezierPointOne.LineTo);

			// set paint operator
			SetPaintOp(PP);
			return;
			}

		/// <summary>
		/// Draw Rectangle with Inward Corners
		/// </summary>
		/// <param name="OriginX">Origin X (left)</param>
		/// <param name="OriginY">Origin Y (right)</param>
		/// <param name="Width">Width</param>
		/// <param name="Height">Height</param>
		/// <param name="Radius">Radius</param>
		/// <param name="PP">Paint operator</param>
		public void DrawInwardCornerRectangle
				(
				double OriginX,
				double OriginY,
				double Width,
				double Height,
				double Radius,
				PaintOp PP
				)
			{
			// make sure radius is not too big
			if(Radius > 0.5 * Width) Radius = 0.5 * Width;
			if(Radius > 0.5 * Height) Radius = 0.5 * Height;

			// draw path
			MoveTo(OriginX, OriginY + Radius);
			DrawBezier(BezierD.CircleFourthQuarter(OriginX, OriginY + Height, Radius), BezierPointOne.LineTo);
			DrawBezier(BezierD.CircleThirdQuarter(OriginX + Width, OriginY + Height, Radius), BezierPointOne.LineTo);
			DrawBezier(BezierD.CircleSecondQuarter(OriginX + Width, OriginY, Radius), BezierPointOne.LineTo);
			DrawBezier(BezierD.CircleFirstQuarter(OriginX, OriginY, Radius), BezierPointOne.LineTo);

			// set paint operator
			SetPaintOp(PP);
			return;
			}

		/// <summary>
		/// Draw polygon
		/// </summary>
		/// <param name="PathArray">Path array (min 2 points)</param>
		/// <param name="PP">Paint operator</param>
		public void DrawPolygon
				(
				PointF[] PathArray,
				PaintOp PP
				)
			{
			// program error
			if(PathArray.Length < 2) throw new ApplicationException("DrawPolygon: path must have at least two points");

			// move to first point
			ObjectValueFormat("{0} {1} m\n", ToPt(PathArray[0].X), ToPt(PathArray[0].Y));

			// draw lines		
			for(int Index = 1; Index < PathArray.Length; Index++)
				{
				ObjectValueFormat("{0} {1} l\n", ToPt(PathArray[Index].X), ToPt(PathArray[Index].Y));
				}

			// set paint operator
			SetPaintOp(PP);
			return;
			}

		/// <summary>
		/// Draw polygon
		/// </summary>
		/// <param name="PathArray">Path array of X and Y values (min 4 and even)</param>
		/// <param name="PP">Paint operator</param>
		public void DrawPolygon
				(
				float[] PathArray,  // pairs of x and y values
				PaintOp PP
				)
			{
			// program error
			if(PathArray.Length < 4 || (PathArray.Length & 1) != 0)
				throw new ApplicationException("DrawPolygon: Path must be even and have at least 4 items");

			// move to first point
			ObjectValueFormat("{0} {1} m\n", ToPt(PathArray[0]), ToPt(PathArray[1]));

			// draw lines		
			for(int Index = 2; Index < PathArray.Length; Index += 2)
				{
				ObjectValueFormat("{0} {1} l\n", ToPt(PathArray[Index]), ToPt(PathArray[Index + 1]));
				}

			// set paint operator
			SetPaintOp(PP);
			return;
			}

		/// <summary>
		/// Draw regular polygon
		/// </summary>
		/// <param name="CenterX">Center X</param>
		/// <param name="CenterY">Center Y</param>
		/// <param name="Radius">Radius</param>
		/// <param name="Alpha">Initial angle</param>
		/// <param name="Sides">Number of sides</param>
		/// <param name="PP">Paint operator</param>
		public void DrawRegularPolygon
				(
				double CenterX,
				double CenterY,
				double Radius,
				double Alpha,
				int Sides,
				PaintOp PP
				)
			{
			DrawRegularPolygon(new PointD(CenterX, CenterY), Radius, Alpha, Sides, PP);
			return;
			}

		/// <summary>
		/// Draw regular polygon
		/// </summary>
		/// <param name="Center">Center position</param>
		/// <param name="Radius">Radius</param>
		/// <param name="Alpha">Initial angle</param>
		/// <param name="Sides">Number of sides</param>
		/// <param name="PP">Paint operator</param>
		public void DrawRegularPolygon
				(
				PointD Center,
				double Radius,
				double Alpha,
				int Sides,
				PaintOp PP
				)
			{
			// validate sides
			if(Sides < 3) throw new ApplicationException("DrawRegularPolygon. Must have 3 or more sides");

			// polygon angle
			double DeltaAlpha = 2.0 * Math.PI / Sides;

			// first corner coordinates
			MoveTo(new PointD(Center, Radius, Alpha));

			for(int Side = 1; Side < Sides; Side++)
				{
				Alpha += DeltaAlpha;
				LineTo(new PointD(Center, Radius, Alpha));
				}

			// set paint operator
			SetPaintOp(PP);
			return;
			}

		/// <summary>
		/// Draw star
		/// </summary>
		/// <param name="CenterX">Center X</param>
		/// <param name="CenterY">Center Y</param>
		/// <param name="Radius">Radius</param>
		/// <param name="Alpha">Initial angle</param>
		/// <param name="Sides">Number of sides</param>
		/// <param name="PP">Paint operator</param>
		public void DrawStar
				(
				double CenterX,
				double CenterY,
				double Radius,
				double Alpha,
				int Sides,
				PaintOp PP
				)
			{
			DrawStar(new PointD(CenterX, CenterY), Radius, Alpha, Sides, PP);
			return;
			}

		/// <summary>
		/// Draw star
		/// </summary>
		/// <param name="Center">Center position</param>
		/// <param name="Radius">Radius</param>
		/// <param name="Alpha">Initial angle</param>
		/// <param name="Sides">Number of sides</param>
		/// <param name="PP">Paint operator</param>
		public void DrawStar
				(
				PointD Center,
				double Radius,
				double Alpha,
				int Sides,
				PaintOp PP
				)
			{
			// inner radius
			double Radius1 = 0;

			// for polygon with less than 5, set inner radius to half the main radius
			if(Sides < 5) 
				{
				Radius1 = 0.5 * Radius;
				}

			// for polygons with 5 sides, calculate inner radius
			else
				{
				// polygon angle
				double DeltaAlpha = 2.0 * Math.PI / Sides;

				// first line
				LineD L1 = new LineD(new PointD(Center, Radius, Alpha), new PointD(Center, Radius, Alpha + 2.0 * DeltaAlpha));

				// second line
				LineD L2 = new LineD(new PointD(Center, Radius, Alpha - DeltaAlpha), new PointD(Center, Radius, Alpha + DeltaAlpha));

				// inner radius
				Radius1 = (new LineD(new PointD(L1, L2), Center)).Length;
				}

			// draw star
			DrawStar(Center, Radius, Radius1, Alpha, Sides, PP);
			return;
			}

		/// <summary>
		/// Draw star
		/// </summary>
		/// <param name="CenterX">Center X</param>
		/// <param name="CenterY">Center Y</param>
		/// <param name="Radius1">Radius 1</param>
		/// <param name="Radius2">Radius 2</param>
		/// <param name="Alpha">Initial angle</param>
		/// <param name="Sides">Number of sides</param>
		/// <param name="PP">Paint operator</param>
		public void DrawStar
				(
				double CenterX,
				double CenterY,
				double Radius1,
				double Radius2,
				double Alpha,
				int Sides,
				PaintOp PP
				)
			{
			DrawStar(new PointD(CenterX, CenterY), Radius1, Radius2, Alpha, Sides, PP);
			return;
			}

		/// <summary>
		/// Draw star
		/// </summary>
		/// <param name="Center">Center point</param>
		/// <param name="Radius1">Radius 1</param>
		/// <param name="Radius2">Radius 2</param>
		/// <param name="Alpha">Initial angle</param>
		/// <param name="Sides">Number of sides</param>
		/// <param name="PP">Paint operator</param>
		public void DrawStar
				(
				PointD Center,
				double Radius1,
				double Radius2,
				double Alpha,
				int Sides,
				PaintOp PP
				)
			{
			// validate sides
			if(Sides < 3) throw new ApplicationException("DrawStar. Must have 3 or more sides");

			// move to first point
			MoveTo(new PointD(Center, Radius1, Alpha));

			// increment angle
			double DeltaAlpha = Math.PI / Sides;

			// double number of sides
			Sides *= 2;

			// line to the rest of the points
			for(int Side = 1; Side < Sides; Side++)
				{
				Alpha += DeltaAlpha;
				LineTo(new PointD(Center, (Side & 1) != 0 ? Radius2 : Radius1, Alpha));
				}

			// set paint operator
			SetPaintOp(PP);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Begin text mode
		/// </summary>
		////////////////////////////////////////////////////////////////////
		public void BeginTextMode()
			{
			ObjectValueAppend("BT\n");
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// End text mode
		/// </summary>
		////////////////////////////////////////////////////////////////////
		public void EndTextMode()
			{
			ObjectValueAppend("ET\n");
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Set text position
		/// </summary>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		////////////////////////////////////////////////////////////////////
		public void SetTextPosition
				(
				double PosX,
				double PosY
				)
			{
			ObjectValueFormat("{0} {1} Td\n", ToPt(PosX), ToPt(PosY));
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Set text rendering mode
		/// </summary>
		/// <param name="TR">Text rendering mode enumeration</param>
		////////////////////////////////////////////////////////////////////
		public void SetTextRenderingMode
				(
				TextRendering TR
				)
			{
			ObjectValueFormat("{0} Tr\n", (int) TR);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Set character extra spacing
		/// </summary>
		/// <param name="ExtraSpacing">Character extra spacing</param>
		////////////////////////////////////////////////////////////////////
		public void SetCharacterSpacing
				(
				double ExtraSpacing
				)
			{
			ObjectValueFormat("{0} Tc\n", ToPt(ExtraSpacing));
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Set word extra spacing
		/// </summary>
		/// <param name="Spacing">Word extra spacing</param>
		////////////////////////////////////////////////////////////////////
		public void SetWordSpacing
				(
				double Spacing
				)
			{
			ObjectValueFormat("{0} Tw\n", ToPt(Spacing));
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Reverse characters in a string
		/// </summary>
		/// <param name="Text">Input string</param>
		/// <returns>Output string</returns>
		////////////////////////////////////////////////////////////////////
		public string ReverseString
				(
				string Text
				)
			{
			char[] RevText = Text.ToCharArray();
			Array.Reverse(RevText);
			return new string(RevText);
			}

		public double DrawTextNew
				(
				PdfFontControl Control,
				double PosX,
				double PosY,
				string Text
				)
			{
			// text is null or empty
			if(string.IsNullOrEmpty(Text))
				return 0;

			// text width
			double TextWidth = 0;

			// we have color
			if(Control.TextColor != Color.Empty)
				{
				// save graphics state
				SaveGraphicsState();

				// change non stroking color
				SetColorNonStroking(Control.TextColor);
				}

			// not subscript or superscript
			if((Control.DrawStyle & (DrawStyle.Subscript | DrawStyle.Superscript)) == 0)
				{
				// draw text string
				TextWidth = DrawText(Control.Font, Control.FontSize, PosX, PosY, Control.TextJustify, Text);

				// not regular style
				if(Control.DrawStyle != DrawStyle.Normal)
					{
					// change stroking color
					if(Control.TextColor != Color.Empty)
						SetColorStroking(Control.TextColor);

					// adjust position
					switch(Control.TextJustify)
						{
						// right
						case TextJustify.Right:
							PosX -= TextWidth;
							break;

						// center
						case TextJustify.Center:
							PosX -= 0.5 * TextWidth;
							break;
						}

					// underline
					if((Control.DrawStyle & DrawStyle.Underline) != 0)
						{
						double UnderlinePos = PosY + Control.UnderlinePosition;
						DrawLine(PosX, UnderlinePos, PosX + TextWidth, UnderlinePos, Control.UnderlineWidth);
						}

					// strikeout
					if((Control.DrawStyle & DrawStyle.Strikeout) != 0)
						{
						double StrikeoutPos = PosY + Control.StrikeoutPosition;
						DrawLine(PosX, StrikeoutPos, PosX + TextWidth, StrikeoutPos, Control.StrikeoutWidth);
						}
					}
				}

			// subscript or superscript
			else
				{
				// subscript
				if((Control.DrawStyle & (DrawStyle.Subscript | DrawStyle.Superscript)) == DrawStyle.Subscript)
					{
					// subscript font size and location
					PosY -= Control.SubscriptPosition;

					// draw text string
					TextWidth = DrawText(Control.Font, Control.SubscriptSize, PosX, PosY, Control.TextJustify, Text);
					}

				// superscript
				if((Control.DrawStyle & (DrawStyle.Subscript | DrawStyle.Superscript)) == DrawStyle.Superscript)
					{
					// superscript font size and location
					PosY += Control.SuperscriptPosition;

					// draw text string
					TextWidth = DrawText(Control.Font, Control.SuperscriptSize, PosX, PosY, Control.TextJustify, Text);
					}
				}

			// we have color
			if(Control.TextColor != Color.Empty)
				{
				// save graphics state
				RestoreGraphicsState();
				}

			// return text width
			return TextWidth;
			}

		/// <summary>
		/// Draw text
		/// </summary>
		/// <param name="Font">Font</param>
		/// <param name="FontSize">Font size</param>
		/// <param name="Text">Text</param>
		/// <returns>Text width</returns>
		/// <remarks>
		/// This method must be used together with BeginTextMode,
		/// EndTextMode and SetTextPosition.
		/// </remarks>
		public double DrawText
				(
				PdfFont Font,           // font object
				double FontSize,        // in points
				string Text
				)
			{
			// text is null or empty
			if(string.IsNullOrEmpty(Text))
				return 0;

			// add font code to current list of font codes
			AddToUsedResources(Font);

			// draw text
			return DrawTextInternal(Font, FontSize, Text);
			}

		internal double DrawTextInternal
				(
				PdfFont Font,           // font object
				double FontSize,
				string Text
				)
			{
			byte[] FontResCode = null;
			byte[] FontResGlyph = null;

			// convert font sise to string
			string FontSizeStr = string.Format(NFI.PeriodDecSep, "{0}", Round(FontSize));

			// set last use glyph index
			bool GlyphIndexFlag = false;

			// text width
			int Width = 0;

			// loop for all text characters
			for(int Ptr = 0; Ptr < Text.Length; Ptr++)
				{
				// get character information
				CharInfo CharInfo = Font.GetCharInfo(Text[Ptr]);

				// set active flag
				CharInfo.ActiveChar = true;

				// accumulate width
				Width += CharInfo.DesignWidth;

				// change between 0-255 and 255-65535
				if(Ptr == 0 || CharInfo.Type0Font != GlyphIndexFlag)
					{
					// ")Tj"
					if(Ptr != 0)
						{
						ObjectValueList.Add((byte) ')');
						ObjectValueList.Add((byte) 'T');
						ObjectValueList.Add((byte) 'j');
						}

					// save glyph index
					GlyphIndexFlag = CharInfo.Type0Font;

					// ouput font resource and font size
					if(!GlyphIndexFlag)
						{
						if(FontResCode == null)
							{
							FontResCode = CreateFontResStr(Font.ResourceCode, FontSizeStr);
							Font.FontResCodeUsed = true;
							}
						ObjectValueList.AddRange(FontResCode);
						}
					else
						{
						if(FontResGlyph == null)
							{
							FontResGlyph = CreateFontResStr(Font.ResourceCodeGlyph, FontSizeStr);
							if(!Font.FontResGlyphUsed)
								Font.CreateGlyphIndexFont();
							}
						ObjectValueList.AddRange(FontResGlyph);
						}
					ObjectValueList.Add((byte) '(');
					}

				// output character code
				if(!GlyphIndexFlag)
					{
					OutputOneByte(CharInfo.CharCode);
					}

				// output glyph index
				else
					{
					if(CharInfo.NewGlyphIndex < 0)
						CharInfo.NewGlyphIndex = Font.EmbeddedFont ? Font.NewGlyphIndex++ : CharInfo.GlyphIndex;
					OutputOneByte(CharInfo.NewGlyphIndex >> 8);
					OutputOneByte(CharInfo.NewGlyphIndex & 0xff);
					}
				}

			// ")Tj"
			ObjectValueList.Add((byte) ')');
			ObjectValueList.Add((byte) 'T');
			ObjectValueList.Add((byte) 'j');
			ObjectValueList.Add((byte) '\n');
			return Font.FontDesignToUserUnits(FontSize, Width);
			}

		internal void OutputOneByte
				(
				int CharCode
				)
			{
			switch(CharCode)
				{
				case '\r':
					ObjectValueList.Add((byte) '\\');
					ObjectValueList.Add((byte) 'r');
					return;
				case '\n':
					ObjectValueList.Add((byte) '\\');
					ObjectValueList.Add((byte) 'n');
					return;
				case '(':
					ObjectValueList.Add((byte) '\\');
					ObjectValueList.Add((byte) '(');
					return;
				case ')':
					ObjectValueList.Add((byte) '\\');
					ObjectValueList.Add((byte) ')');
					return;
				case '\\':
					ObjectValueList.Add((byte) '\\');
					ObjectValueList.Add((byte) '\\');
					return;
				default:
					ObjectValueList.Add((byte) CharCode);
					return;
				}
			}

		internal byte[] CreateFontResStr
				(
				string ResCode,
				string SizeStr
				)
			{
			byte[] FontRes = new byte[ResCode.Length + SizeStr.Length + 4];
			int Index = 0;
			foreach(char TextChar in ResCode)
				FontRes[Index++] = (byte) TextChar;
			FontRes[Index++] = (byte) ' ';
			foreach(char TextChar in SizeStr)
				FontRes[Index++] = (byte) TextChar;
			FontRes[Index++] = (byte) ' ';
			FontRes[Index++] = (byte) 'T';
			FontRes[Index++] = (byte) 'f';
			return FontRes;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw one line of text left justified
		/// </summary>
		/// <param name="Font">Font</param>
		/// <param name="FontSize">Font size</param>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		/// <param name="Text">Text</param>
		/// <returns>Text width</returns>
		////////////////////////////////////////////////////////////////////
		public double DrawText
				(
				PdfFont Font,
				double FontSize,        // in points
				double PosX,
				double PosY,
				string Text
				)
			{
			return DrawText(Font, FontSize, PosX, PosY, TextJustify.Left, Text);
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw one line of text
		/// </summary>
		/// <param name="Font">Font</param>
		/// <param name="FontSize">Font size</param>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		/// <param name="Justify">Text justify enumeration</param>
		/// <param name="Text">Text</param>
		/// <returns>Text width</returns>
		////////////////////////////////////////////////////////////////////
		public double DrawText
				(
				PdfFont Font,
				double FontSize,        // in points
				double PosX,
				double PosY,
				TextJustify Justify,
				string Text
				)
			{
			// text is null or empty
			if(string.IsNullOrEmpty(Text))
				return 0;

			// add font code to current list of font codes
			AddToUsedResources(Font);

			// adjust position
			switch(Justify)
				{
				// right
				case TextJustify.Right:
					PosX -= Font.TextWidth(FontSize, Text);
					break;

				// center
				case TextJustify.Center:
					PosX -= 0.5 * Font.TextWidth(FontSize, Text);
					break;
				}

			// draw text
			BeginTextMode();
			SetTextPosition(PosX, PosY);
			double Width = DrawTextInternal(Font, FontSize, Text);
			EndTextMode();

			// return text width
			return Width;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw one line of text width draw style
		/// </summary>
		/// <param name="Font">Font</param>
		/// <param name="FontSize">Font size</param>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		/// <param name="DrawStyle">Drawing style enumeration</param>
		/// <param name="Text">Text</param>
		/// <returns>Text width</returns>
		////////////////////////////////////////////////////////////////////
		public double DrawText
				(
				PdfFont Font,
				double FontSize,        // in points
				double PosX,
				double PosY,
				DrawStyle DrawStyle,
				string Text
				)
			{
			return DrawText(Font, FontSize, PosX, PosY, TextJustify.Left, DrawStyle, Color.Empty, Text);
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw one line of text with a given color
		/// </summary>
		/// <param name="Font">Font</param>
		/// <param name="FontSize">Font size</param>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		/// <param name="TextColor">Color</param>
		/// <param name="Text">Text</param>
		/// <returns>Text width</returns>
		////////////////////////////////////////////////////////////////////
		public double DrawText
				(
				PdfFont Font,
				double FontSize,        // in points
				double PosX,
				double PosY,
				Color TextColor,
				string Text
				)
			{
			return DrawText(Font, FontSize, PosX, PosY, TextJustify.Left, DrawStyle.Normal, TextColor, Text);
			}

		////////////////////////////////////////////////////////////////////
		// Draw text width draw style
		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw one line of text with text justification, drawing style and color
		/// </summary>
		/// <param name="Font">Font</param>
		/// <param name="FontSize">Font size</param>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		/// <param name="Justify">Text justify enumeration</param>
		/// <param name="DrawStyle">Drawing style enumeration</param>
		/// <param name="TextColor">Color</param>
		/// <param name="Text">Text</param>
		/// <returns>Text width</returns>
		public double DrawText
				(
				PdfFont Font,
				double FontSize,        // in points
				double PosX,
				double PosY,
				TextJustify Justify,
				DrawStyle DrawStyle,
				Color TextColor,
				string Text
				)
			{
			// text is null or empty
			if(string.IsNullOrEmpty(Text))
				return 0;

			// text width
			double TextWidth = 0;

			// we have color
			if(TextColor != Color.Empty)
				{
				// save graphics state
				SaveGraphicsState();

				// change non stroking color
				SetColorNonStroking(TextColor);
				}

			// not subscript or superscript
			if((DrawStyle & (DrawStyle.Subscript | DrawStyle.Superscript)) == 0)
				{
				// draw text string
				TextWidth = DrawText(Font, FontSize, PosX, PosY, Justify, Text);

				// not regular style
				if(DrawStyle != DrawStyle.Normal)
					{
					// change stroking color
					if(TextColor != Color.Empty)
						SetColorStroking(TextColor);

					// adjust position
					switch(Justify)
						{
						// right
						case TextJustify.Right:
							PosX -= TextWidth;
							break;

						// center
						case TextJustify.Center:
							PosX -= 0.5 * TextWidth;
							break;
						}

					// underline
					if((DrawStyle & DrawStyle.Underline) != 0)
						{
						double UnderlinePos = PosY + Font.UnderlinePosition(FontSize);
						DrawLine(PosX, UnderlinePos, PosX + TextWidth, UnderlinePos, Font.UnderlineWidth(FontSize));
						}

					// strikeout
					if((DrawStyle & DrawStyle.Strikeout) != 0)
						{
						double StrikeoutPos = PosY + Font.StrikeoutPosition(FontSize);
						DrawLine(PosX, StrikeoutPos, PosX + TextWidth, StrikeoutPos, Font.StrikeoutWidth(FontSize));
						}
					}
				}

			// subscript or superscript
			else
				{
				// subscript
				if((DrawStyle & (DrawStyle.Subscript | DrawStyle.Superscript)) == DrawStyle.Subscript)
					{
					// subscript font size and location
					PosY -= Font.SubscriptPosition(FontSize);
					FontSize = Font.SubscriptSize(FontSize);

					// draw text string
					TextWidth = DrawText(Font, FontSize, PosX, PosY, Justify, Text);
					}

				// superscript
				if((DrawStyle & (DrawStyle.Subscript | DrawStyle.Superscript)) == DrawStyle.Superscript)
					{
					// superscript font size and location
					PosY += Font.SuperscriptPosition(FontSize);
					FontSize = Font.SuperscriptSize(FontSize);

					// draw text string
					TextWidth = DrawText(Font, FontSize, PosX, PosY, Justify, Text);
					}
				}

			// we have color
			if(TextColor != Color.Empty)
				{
				// save graphics state
				RestoreGraphicsState();
				}

			// return text width
			return TextWidth;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw text with kerning array
		/// </summary>
		/// <param name="Font">Font</param>
		/// <param name="FontSize">Font size</param>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		/// <param name="KerningArray">Kerning array</param>
		/// <returns>Text width</returns>
		/// <remarks>
		/// Each kerning item consists of text and position adjustment.
		/// The adjustment is a negative number.
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public double DrawText
				(
				PdfFont Font,
				double FontSize,        // in points
				double PosX,
				double PosY,
				KerningAdjust[] KerningArray
				)
			{
			// text is null or empty
			if(KerningArray == null || KerningArray.Length == 0)
				return 0;

			// add font code to current list of font codes
			AddToUsedResources(Font);

			// draw text initialization
			BeginTextMode();
			SetTextPosition(PosX, PosY);

			// draw text with kerning
			double Width = DrawTextWithKerning(Font, FontSize, KerningArray);

			// draw text termination
			EndTextMode();

			// exit
			return Width;
			}

		internal double DrawTextWithKerning
				(
				PdfFont Font,           // font object
				double FontSize,
				KerningAdjust[] KerningArray
				)
			{
			byte[] FontResCode = null;
			byte[] FontResGlyph = null;

			// convert font sise to string
			string FontSizeStr = string.Format(NFI.PeriodDecSep, "{0}", Round(FontSize));

			// set last use glyph index
			bool GlyphIndexFlag = false;

			// text width
			int Width = 0;

			// loop for kerning pairs
			int Index = 0;
			for(; ; )
				{
				KerningAdjust KA = KerningArray[Index];
				string Text = KA.Text;

				// loop for all text characters
				for(int Ptr = 0; Ptr < Text.Length; Ptr++)
					{
					// get character information
					CharInfo CharInfo = Font.GetCharInfo(Text[Ptr]);

					// set active flag
					CharInfo.ActiveChar = true;

					// accumulate width
					Width += CharInfo.DesignWidth;

					// change between 0-255 and 255-65535
					if(Index == 0 && Ptr == 0 || CharInfo.Type0Font != GlyphIndexFlag)
						{
						// close partial string
						if(Ptr != 0)
							{
							ObjectValueList.Add((byte) ')');
							}

						// close code/glyph area
						if(Index != 0)
							{
							ObjectValueList.Add((byte) ']');
							ObjectValueList.Add((byte) 'T');
							ObjectValueList.Add((byte) 'J');
							}

						// save glyph index
						GlyphIndexFlag = CharInfo.Type0Font;

						// ouput font resource and font size
						if(!GlyphIndexFlag)
							{
							if(FontResCode == null)
								{
								FontResCode = CreateFontResStr(Font.ResourceCode, FontSizeStr);
								Font.FontResCodeUsed = true;
								}
							ObjectValueList.AddRange(FontResCode);
							}
						else
							{
							if(FontResGlyph == null)
								{
								FontResGlyph = CreateFontResStr(Font.ResourceCodeGlyph, FontSizeStr);
								if(!Font.FontResGlyphUsed)
									Font.CreateGlyphIndexFont();
								}
							ObjectValueList.AddRange(FontResGlyph);
							}

						ObjectValueList.Add((byte) '[');
						ObjectValueList.Add((byte) '(');
						}

					else if(Ptr == 0)
						{
						ObjectValueList.Add((byte) '(');
						}

					// output character code
					if(!GlyphIndexFlag)
						{
						OutputOneByte(CharInfo.CharCode);
						}

					// output glyph index
					else
						{
						if(CharInfo.NewGlyphIndex < 0)
							CharInfo.NewGlyphIndex = Font.EmbeddedFont ? Font.NewGlyphIndex++ : CharInfo.GlyphIndex;
						OutputOneByte(CharInfo.NewGlyphIndex >> 8);
						OutputOneByte(CharInfo.NewGlyphIndex & 0xff);
						}
					}

				ObjectValueList.Add((byte) ')');

				// test for end of kerning array
				Index++;
				if(Index == KerningArray.Length)
					break;

				// add adjustment
				ObjectValueFormat("{0}", Round(-KA.Adjust));

				// convert the adjustment width to font design width
				Width += (int) Math.Round(KA.Adjust * Font.DesignHeight / 1000.0, 0, MidpointRounding.AwayFromZero);
				}

			// "]Tj"
			ObjectValueList.Add((byte) ']');
			ObjectValueList.Add((byte) 'T');
			ObjectValueList.Add((byte) 'J');
			ObjectValueList.Add((byte) '\n');
			return Font.FontDesignToUserUnits(FontSize, Width);
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw text with kerning
		/// </summary>
		/// <param name="Font">Font</param>
		/// <param name="FontSize">Font size</param>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		/// <param name="Text">Text</param>
		/// <returns>Text width</returns>
		////////////////////////////////////////////////////////////////////
		public double DrawTextWithKerning
				(
				PdfFont Font,
				double FontSize,        // in points
				double PosX,
				double PosY,
				string Text
				)
			{
			// text is null or empty
			if(string.IsNullOrEmpty(Text))
				return 0;

			// create text position adjustment array based on kerning information
			KerningAdjust[] KernArray = Font.TextKerning(Text);

			// no kerning
			if(KernArray == null)
				return DrawText(Font, FontSize, PosX, PosY, Text);

			// draw text with adjustment
			return DrawText(Font, FontSize, PosX, PosY, KernArray);
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw text with special effects
		/// </summary>
		/// <param name="Font">Font</param>
		/// <param name="FontSize">Font size</param>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		/// <param name="Justify">Text justify enumeration</param>
		/// <param name="OutlineWidth">Outline width</param>
		/// <param name="StrokingColor">Stoking (outline) color</param>
		/// <param name="NonStokingColor">Non stroking (fill) color</param>
		/// <param name="Text">Text</param>
		/// <returns>Text width</returns>
		////////////////////////////////////////////////////////////////////
		public double DrawText
				(
				PdfFont Font,
				double FontSize,        // in points
				double PosX,
				double PosY,
				TextJustify Justify,
				double OutlineWidth,
				Color StrokingColor,
				Color NonStokingColor,
				string Text
				)
			{
			// text is null or empty
			if(string.IsNullOrEmpty(Text))
				return 0;

			// add font code to current list of font codes
			AddToUsedResources(Font);

			// save graphics state
			SaveGraphicsState();

			// create text position adjustment array based on kerning information
			KerningAdjust[] KernArray = Font.TextKerning(Text);

			// text width
			double Width = KernArray == null ? Font.TextWidth(FontSize, Text) : Font.TextKerningWidth(FontSize, KernArray);

			// adjust position
			switch(Justify)
				{
				// right
				case TextJustify.Right:
					PosX -= Width;
					break;

				// center
				case TextJustify.Center:
					PosX -= 0.5 * Width;
					break;
				}

			// special effects
			TextRendering TR = TextRendering.Fill;
			if(!StrokingColor.IsEmpty)
				{
				SetLineWidth(OutlineWidth);
				SetColorStroking(StrokingColor);
				TR = TextRendering.Stroke;
				}
			if(!NonStokingColor.IsEmpty)
				{
				SetColorNonStroking(NonStokingColor);
				TR = StrokingColor.IsEmpty ? TextRendering.Fill : TextRendering.FillStroke;
				}

			// draw text initialization
			BeginTextMode();
			SetTextPosition(PosX, PosY);
			SetTextRenderingMode(TR);

			// draw text without kerning
			if(KernArray == null)
				{
				DrawTextInternal(Font, FontSize, Text);
				}

			// draw text with kerning
			else
				{
				DrawTextWithKerning(Font, FontSize, KernArray);
				}

			// draw text termination
			EndTextMode();

			// restore graphics state
			RestoreGraphicsState();

			// exit
			return Width;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw text with annotation action
		/// </summary>
		/// <param name="Page">Current page</param>
		/// <param name="Font">Font</param>
		/// <param name="FontSize">Font size</param>
		/// <param name="TextAbsPosX">Text absolute position X</param>
		/// <param name="TextAbsPosY">Text absolute position Y</param>
		/// <param name="Text">Text</param>
		/// <param name="AnnotAction">Annotation action</param>
		/// <returns>Text width</returns>
		/// <remarks>
		///	The position arguments are in relation to the
		///	bottom left corner of the paper.
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public double DrawTextWithAnnotation
				(
				PdfPage Page,
				PdfFont Font,
				double FontSize,        // in points
				double TextAbsPosX,
				double TextAbsPosY,
				string Text,
				AnnotAction AnnotAction
				)
			{
			return DrawTextWithAnnotation(Page, Font, FontSize, TextAbsPosX, TextAbsPosY, TextJustify.Left, DrawStyle.Underline, Color.DarkBlue, Text, AnnotAction);
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw web link with one line of text
		/// </summary>
		/// <param name="Page">Current page</param>
		/// <param name="Font">Font</param>
		/// <param name="FontSize">Font size</param>
		/// <param name="TextAbsPosX">Text absolute position X</param>
		/// <param name="TextAbsPosY">Text absolute position Y</param>
		/// <param name="Text">Text</param>
		/// <param name="WebLinkStr">Web link</param>
		/// <returns>Text width</returns>
		/// <remarks>
		///	The position arguments are in relation to the
		///	bottom left corner of the paper.
		///	Text will be drawn left justified, underlined and in dark blue.
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public double DrawWebLink
				(
				PdfPage Page,
				PdfFont Font,
				double FontSize,        // in points
				double TextAbsPosX,
				double TextAbsPosY,
				string Text,
				string WebLinkStr
				)
			{
			return DrawTextWithAnnotation(Page, Font, FontSize, TextAbsPosX, TextAbsPosY, TextJustify.Left,
				DrawStyle.Underline, Color.DarkBlue, Text, new AnnotWebLink(WebLinkStr));
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw web link with one line of text
		/// </summary>
		/// <param name="Page">Current page</param>
		/// <param name="Font">Font</param>
		/// <param name="FontSize">Font size</param>
		/// <param name="TextAbsPosX">Text absolute position X</param>
		/// <param name="TextAbsPosY">Text absolute position Y</param>
		/// <param name="Justify">Text justify enumeration.</param>
		/// <param name="DrawStyle">Draw style enumeration</param>
		/// <param name="TextColor">Color</param>
		/// <param name="Text">Text</param>
		/// <param name="WebLinkStr">Web link</param>
		/// <returns>Text width</returns>
		/// <remarks>
		///	The position arguments are in relation to the
		///	bottom left corner of the paper.
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public double DrawWebLink
				(
				PdfPage Page,
				PdfFont Font,
				double FontSize,        // in points
				double TextAbsPosX,
				double TextAbsPosY,
				TextJustify Justify,
				DrawStyle DrawStyle,
				Color TextColor,
				string Text,
				string WebLinkStr
				)
			{
			return DrawTextWithAnnotation(Page, Font, FontSize, TextAbsPosX, TextAbsPosY, Justify, DrawStyle, TextColor, Text, new AnnotWebLink(WebLinkStr));
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw text with annotation action
		/// </summary>
		/// <param name="Page">Current page</param>
		/// <param name="Font">Font</param>
		/// <param name="FontSize">Font size</param>
		/// <param name="TextAbsPosX">Text absolute position X</param>
		/// <param name="TextAbsPosY">Text absolute position Y</param>
		/// <param name="Justify">Text justify enumeration.</param>
		/// <param name="DrawStyle">Draw style enumeration</param>
		/// <param name="TextColor">Color</param>
		/// <param name="Text">Text</param>
		/// <param name="AnnotAction">Annotation action</param>
		/// <returns>Text width</returns>
		/// <remarks>
		///	The position arguments are in relation to the
		///	bottom left corner of the paper.
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public double DrawTextWithAnnotation
				(
				PdfPage Page,
				PdfFont Font,
				double FontSize,        // in points
				double TextAbsPosX,
				double TextAbsPosY,
				TextJustify Justify,
				DrawStyle DrawStyle,
				Color TextColor,
				string Text,
				AnnotAction AnnotAction
				)
			{
			double Width = DrawText(Font, FontSize, TextAbsPosX, TextAbsPosY, Justify, DrawStyle, TextColor, Text);
			if(Width == 0.0)
				return 0.0;

			// adjust position
			switch(Justify)
				{
				// right
				case TextJustify.Right:
					TextAbsPosX -= Width;
					break;

				// center
				case TextJustify.Center:
					TextAbsPosX -= 0.5 * Width;
					break;
				}

			PdfRectangle AnnotRect = new PdfRectangle(TextAbsPosX, TextAbsPosY - Font.DescentPlusLeading(FontSize), TextAbsPosX + Width, TextAbsPosY + Font.AscentPlusLeading(FontSize));
			Page.AddAnnotInternal(AnnotRect, AnnotAction);
			return Width;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw TextBox
		/// </summary>
		/// <param name="PosX">Position X</param>
		/// <param name="PosYTop">Position Y (by reference)</param>
		/// <param name="PosYBottom">Position Y bottom</param>
		/// <param name="LineNo">Start at line number</param>
		/// <param name="TextBox">TextBox</param>
		/// <param name="Page">Page if TextBox contains web link segment</param>
		/// <returns>Next line number</returns>
		/// <remarks>
		/// Before calling this method you must add text to a TextBox object.
		/// <para>
		/// Set the PosX and PosYTop to the left top corner of the text area.
		/// Note PosYTop is by reference. This variable will be updated to
		/// the next vertical line position after the method was executed.
		/// </para>
		/// <para>
		/// Set the PosYBottom to the bottom of your page. The method will
		/// not print below this value.
		/// </para>
		/// <para>
		/// Set the LineNo to the first line to be printed. Initially 
		/// this will be zero. After the method returns, PosYTop is set 
		/// to next print line on the page and LineNo is set to next line 
		/// within the box.
		/// </para>
		/// <para>
		/// If LineNo is equals to TextBox.LineCount the box was fully printed. 
		/// </para>
		/// <para>
		/// If LineNo is less than TextBox.LineCount box printing was not
		/// done. Start a new PdfPage and associated PdfContents. Set 
		/// PosYTop to desired start position. Set LineNo to the value
		/// returned by this method, and call the method again.
		/// </para>
		/// <para>
		/// If your TextBox contains WebLink segment you must supply
		/// Page argument and position X and Y must be relative to
		/// page bottom left corner.
		/// </para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public int DrawText
				(
				double PosX,
				ref double PosYTop,
				double PosYBottom,
				int LineNo,
				TextBox TextBox,
				PdfPage Page = null
				)
			{
			return DrawText(PosX, ref PosYTop, PosYBottom, LineNo, 0.0, 0.0, TextBoxJustify.Left, TextBox, Page);
			}

		////////////////////////////////////////////////////////////////////
		// Draw Text for TextBox with web link
		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw TextBox
		/// </summary>
		/// <param name="PosX">Position X</param>
		/// <param name="PosYTop">Position Y (by reference)</param>
		/// <param name="PosYBottom">Position Y bottom</param>
		/// <param name="LineNo">Start at line number</param>
		/// <param name="LineExtraSpace">Extra line spacing</param>
		/// <param name="ParagraphExtraSpace">Extra paragraph spacing</param>
		/// <param name="Justify">TextBox justify enumeration</param>
		/// <param name="TextBox">TextBox</param>
		/// <param name="Page">Page if TextBox contains web link segment</param>
		/// <returns>Next line number</returns>
		/// <remarks>
		/// Before calling this method you must add text to a TextBox object.
		/// <para>
		/// Set the PosX and PosYTop to the left top corner of the text area.
		/// Note PosYTop is by reference. This variable will be updated to
		/// the next vertical line position after the method was executed.
		/// </para>
		/// <para>
		/// Set the PosYBottom to the bottom of your page. The method will
		/// not print below this value.
		/// </para>
		/// <para>
		/// Set the LineNo to the first line to be printed. Initially 
		/// this will be zero. After the method returns, PosYTop is set 
		/// to next print line on the page and LineNo is set to next line 
		/// within the box.
		/// </para>
		/// <para>
		/// If LineNo is equals to TextBox.LineCount the box was fully printed. 
		/// </para>
		/// <para>
		/// If LineNo is less than TextBox.LineCount box printing was not
		/// done. Start a new PdfPage and associated PdfContents. Set 
		/// PosYTop to desired start position. Set LineNo to the value
		/// returned by this method, and call the method again.
		/// </para>
		/// <para>
		/// If your TextBox contains WebLink segment you must supply
		/// Page argument and position X and Y must be relative to
		/// page bottom left corner.
		/// </para>
		/// <para>
		/// TextBoxJustify controls horizontal justification. FitToWidth
		/// will display a straight right edge.
		/// </para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public int DrawText
				(
				double PosX,
				ref double PosYTop,
				double PosYBottom,
				int LineNo,
				double LineExtraSpace,
				double ParagraphExtraSpace,
				TextBoxJustify Justify,
				TextBox TextBox,
				PdfPage Page = null
				)
			{
			TextBox.Terminate();
			for(; LineNo < TextBox.LineCount; LineNo++)
				{
				// short cut
				TextBoxLine Line = TextBox[LineNo];

				// break out of the loop if printing below bottom line
				if(PosYTop - Line.LineHeight < PosYBottom)
					break;

				// adjust PosY to font base line
				PosYTop -= Line.Ascent;

				// text horizontal position
				double X = PosX;
				double W = TextBox.BoxWidth;

				// if we have first line indent, adjust text x position for first line of a paragraph
				if(TextBox.FirstLineIndent != 0 && (LineNo == 0 || TextBox[LineNo - 1].EndOfParagraph))
					{
					X += TextBox.FirstLineIndent;
					W -= TextBox.FirstLineIndent;
					}

				// draw text to fit box width
				if(Justify == TextBoxJustify.FitToWidth && !Line.EndOfParagraph)
					{
					DrawText(X, PosYTop, W, Line, Page);
					}

				// draw text center or right justified
				else if(Justify == TextBoxJustify.Center || Justify == TextBoxJustify.Right)
					{
					DrawText(X, PosYTop, W, Justify, Line, Page);
					}

				// draw text normal
				else
					{
					DrawText(X, PosYTop, Line, Page);
					}


				// advance position y to next line
				PosYTop -= Line.Descent + LineExtraSpace;
				if(Line.EndOfParagraph)
					PosYTop -= ParagraphExtraSpace;
				}
			return LineNo;
			}

		////////////////////////////////////////////////////////////////////
		// Draw text within text box left justified
		////////////////////////////////////////////////////////////////////

		private double DrawText
				(
				double PosX,
				double PosY,
				TextBoxLine Line,
				PdfPage Page
				)
			{
			double SegPosX = PosX;
			foreach(TextBoxSeg Seg in Line.SegArray)
				{
				double SegWidth = DrawText(Seg.Font, Seg.FontSize, SegPosX, PosY, TextJustify.Left, Seg.DrawStyle, Seg.FontColor, Seg.Text);
				if(Seg.AnnotAction != null)
					{
					if(Page == null)
						throw new ApplicationException("TextBox with WebLink. You must call DrawText with PdfPage");
					PdfRectangle AnnotRect = new PdfRectangle(SegPosX, PosY - Seg.Font.DescentPlusLeading(Seg.FontSize), SegPosX + SegWidth, PosY + Seg.Font.AscentPlusLeading(Seg.FontSize));
					Page.AddAnnotInternal(AnnotRect, Seg.AnnotAction);
					}

				SegPosX += SegWidth;
				}

			return SegPosX - PosX;
			}

		////////////////////////////////////////////////////////////////////
		// Draw text within text box center or right justified
		////////////////////////////////////////////////////////////////////

		private double DrawText
				(
				double PosX,
				double PosY,
				double Width,
				TextBoxJustify Justify,
				TextBoxLine Line,
				PdfPage Page
				)
			{
			double LineWidth = 0;
			foreach(TextBoxSeg Seg in Line.SegArray)
				LineWidth += Seg.SegWidth;

			double SegPosX = PosX;
			if(Justify == TextBoxJustify.Right)
				SegPosX += Width - LineWidth;
			else
				SegPosX += 0.5 * (Width - LineWidth);
			foreach(TextBoxSeg Seg in Line.SegArray)
				{
				double SegWidth = DrawText(Seg.Font, Seg.FontSize, SegPosX, PosY, TextJustify.Left, Seg.DrawStyle, Seg.FontColor, Seg.Text);
				if(Seg.AnnotAction != null)
					{
					if(Page == null)
						throw new ApplicationException("TextBox with WebLink. You must call DrawText with PdfPage");
					PdfRectangle AnnotRect = new PdfRectangle(SegPosX, PosY - Seg.Font.DescentPlusLeading(Seg.FontSize), SegPosX + SegWidth, PosY + Seg.Font.AscentPlusLeading(Seg.FontSize));
					Page.AddAnnotInternal(AnnotRect, Seg.AnnotAction);
					}

				SegPosX += SegWidth;
				}

			return SegPosX - PosX;
			}

		////////////////////////////////////////////////////////////////////
		// Draw text justify to width within text box
		////////////////////////////////////////////////////////////////////

		private double DrawText
				(
				double PosX,
				double PosY,
				double Width,
				TextBoxLine Line,
				PdfPage Page
				)
			{
			if(!TextFitToWidth(Width, out double WordSpacing, out double CharSpacing, Line))
				return DrawText(PosX, PosY, Line, Page);
			SaveGraphicsState();
			SetWordSpacing(WordSpacing);
			SetCharacterSpacing(CharSpacing);

			double SegPosX = PosX;
			foreach(TextBoxSeg Seg in Line.SegArray)
				{
				double SegWidth = DrawText(Seg.Font, Seg.FontSize, SegPosX, PosY, TextJustify.Left, Seg.DrawStyle, Seg.FontColor, Seg.Text) + Seg.SpaceCount * WordSpacing + Seg.Text.Length * CharSpacing;
				if(Seg.AnnotAction != null)
					{
					if(Page == null)
						throw new ApplicationException("TextBox with WebLink. You must call DrawText with PdfPage");
					PdfRectangle AnnotRect = new PdfRectangle(SegPosX, PosY - Seg.Font.DescentPlusLeading(Seg.FontSize), SegPosX + SegWidth, PosY + Seg.Font.AscentPlusLeading(Seg.FontSize));
					Page.AddAnnotInternal(AnnotRect, Seg.AnnotAction);
					}
				SegPosX += SegWidth;
				}
			RestoreGraphicsState();
			return SegPosX - PosX;
			}

		////////////////////////////////////////////////////////////////////
		// Stretch text to given width
		////////////////////////////////////////////////////////////////////

		private bool TextFitToWidth
				(
				double ReqWidth,
				out double WordSpacing,
				out double CharSpacing,
				TextBoxLine Line
				)
			{
			WordSpacing = 0;
			CharSpacing = 0;

			int CharCount = 0;
			double Width = 0;
			int SpaceCount = 0;
			double SpaceWidth = 0;
			foreach(TextBoxSeg Seg in Line.SegArray)
				{
				// accumulate line width
				CharCount += Seg.Text.Length;
				Width += Seg.SegWidth;

				// count spaces
				SpaceCount += Seg.SpaceCount;

				// accumulate space width
				SpaceWidth += Seg.SpaceCount * Seg.Font.CharWidth(Seg.FontSize, ' ');
				}

			// reduce character count by one
			CharCount--;
			if(CharCount <= 0)
				return false;

			// extra spacing required
			double ExtraSpace = ReqWidth - Width;

			// highest possible output device resolution (12000 dots per inch)
			double MaxRes = 0.006 / ScaleFactor;

			// string is too wide
			if(ExtraSpace < (-MaxRes))
				return false;

			// string is just right
			if(ExtraSpace < MaxRes)
				return true;

			// String does not have any blank characters
			if(SpaceCount == 0)
				{
				CharSpacing = ExtraSpace / CharCount;
				return true;
				}

			// extra space per word
			WordSpacing = ExtraSpace / SpaceCount;

			// extra space is equal or less than one blank
			if(WordSpacing <= SpaceWidth / SpaceCount)
				return true;

			// extra space is larger that one blank
			// increase character and word spacing
			CharSpacing = ExtraSpace / (10 * SpaceCount + CharCount);
			WordSpacing = 10 * CharSpacing;
			return true;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Clip text exposing area underneath
		/// </summary>
		/// <param name="Font">Font</param>
		/// <param name="FontSize">Font size</param>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		/// <param name="Text">Text</param>
		////////////////////////////////////////////////////////////////////
		public void ClipText
				(
				PdfFont Font,
				double FontSize,        // in points
				double PosX,
				double PosY,
				string Text
				)
			{
			// text is null or empty
			if(string.IsNullOrEmpty(Text))
				return;

			// add font code to current list of font codes
			AddToUsedResources(Font);

			// draw text
			BeginTextMode();
			SetTextPosition(PosX, PosY);
			SetTextRenderingMode(TextRendering.Clip);
			DrawTextInternal(Font, FontSize, Text);
			EndTextMode();
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw barcode
		/// </summary>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		/// <param name="BarWidth">Narrow bar width</param>
		/// <param name="BarHeight">Barcode height</param>
		/// <param name="Barcode">Derived barcode class</param>
		/// <param name="TextFont">Optional text font</param>
		/// <param name="FontSize">Optional text font size</param>
		/// <returns>Barcode width</returns>
		/// <remarks>
		/// <para>
		/// PosX can be the left, centre or right side of the barcode.
		/// The Justify argument controls the meaning of PosX.
		/// PosY is the position of the bottom side of the barcode. 
		/// If optional text is displayed it will be
		/// displayed below PosY. If optional text is wider than the
		/// barcode it will be extended to the left and right sides
		/// of the barcode.
		/// </para>
		/// <para>
		/// The BarWidth argument is the width of the narrow bar.
		/// </para>
		/// <para>
		/// The BarcodeHeight argument is the height of the barcode 
		/// excluding optional text.
		/// </para>
		/// <para>
		/// Set Barcode to one of the derived classes. 
		/// This library supports: Barcode128, Barcode39 and BarcodeEAN13.
		/// Note BarcodeEAN13 supports Barcode UPC-A.
		/// </para>
		/// <para>
		/// Barcode text is optional. If TextFont and FontSize are omitted 
		/// no text will be drawn under the barcode. If TextFont and
		/// FontSize are specified the barcode text will be displayed
		/// under the barcode. It will be horizontally centered in relation
		/// to the barcode.
		/// </para>
		/// <para>
		/// Barcode text is displayed below PosY. Make sure to leave
		/// space under the barcode.
		/// </para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public double DrawBarcode
				(
				double PosX,
				double PosY,
				double BarWidth,
				double BarHeight,
				Barcode Barcode,
				PdfFont TextFont = null,
				double FontSize = 0.0
				)
			{
			return DrawBarcode(PosX, PosY, TextJustify.Left, BarWidth, BarHeight, Color.Black, Barcode, TextFont, FontSize);
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw barcode
		/// </summary>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		/// <param name="Justify">Barcode justify (using TextJustify enumeration)</param>
		/// <param name="BarWidth">Narrow bar width</param>
		/// <param name="BarcodeHeight">Barcode height</param>
		/// <param name="Barcode">Derived barcode class</param>
		/// <param name="TextFont">Text font</param>
		/// <param name="FontSize">Text font size</param>
		/// <returns>Barcode width</returns>
		/// <remarks>
		/// <para>
		/// PosX can be the left, centre or right side of the barcode.
		/// The Justify argument controls the meaning of PosX.
		/// PosY is the position of the bottom side of the barcode. 
		/// If optional text is displayed it will be
		/// displayed below PosY. If optional text is wider than the
		/// barcode it will be extended to the left and right sides
		/// of the barcode.
		/// </para>
		/// <para>
		/// The BarWidth argument is the width of the narrow bar.
		/// </para>
		/// <para>
		/// The BarcodeHeight argument is the height of the barcode 
		/// excluding optional text.
		/// </para>
		/// <para>
		/// Set Barcode to one of the derived classes. 
		/// This library supports: Barcode128, Barcode39 and BarcodeEAN13.
		/// Note BarcodeEAN13 supports Barcode UPC-A.
		/// </para>
		/// <para>
		/// Barcode text is optional. If TextFont and FontSize are omitted 
		/// no text will be drawn under the barcode. If TextFont and
		/// FontSize are specified the barcode text will be displayed
		/// under the barcode. It will be horizontally centered in relation
		/// to the barcode.
		/// </para>
		/// <para>
		/// Barcode text is displayed below PosY. Make sure to leave
		/// space under the barcode.
		/// </para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public double DrawBarcode
				(
				double PosX,
				double PosY,
				TextJustify Justify,
				double BarWidth,
				double BarcodeHeight,
				Barcode Barcode,
				PdfFont TextFont = null,
				double FontSize = 0.0
				)
			{
			return DrawBarcode(PosX, PosY, Justify, BarWidth, BarcodeHeight, Color.Black, Barcode, TextFont, FontSize);
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw barcode
		/// </summary>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		/// <param name="Justify">Barcode justify (using TextJustify enumeration)</param>
		/// <param name="BarWidth">Narrow bar width</param>
		/// <param name="BarHeight">Barcode height</param>
		/// <param name="BarColor">Barcode color</param>
		/// <param name="Barcode">Derived barcode class</param>
		/// <param name="TextFont">Text font</param>
		/// <param name="FontSize">Text font size</param>
		/// <returns>Barcode width</returns>
		/// <remarks>
		/// <para>
		/// PosX can be the left, centre or right side of the barcode.
		/// The Justify argument controls the meaning of PosX.
		/// PosY is the position of the bottom side of the barcode. 
		/// If optional text is displayed it will be
		/// displayed below PosY. If optional text is wider than the
		/// barcode it will be extended to the left and right sides
		/// of the barcode.
		/// </para>
		/// <para>
		/// The BarWidth argument is the width of the narrow bar.
		/// </para>
		/// <para>
		/// The BarcodeHeight argument is the height of the barcode 
		/// excluding optional text.
		/// </para>
		/// <para>
		/// Set Barcode to one of the derived classes. 
		/// This library supports: Barcode128, Barcode39 and BarcodeEAN13.
		/// Note BarcodeEAN13 supports Barcode UPC-A.
		/// </para>
		/// <para>
		/// Barcode text is optional. If TextFont and FontSize are omitted 
		/// no text will be drawn under the barcode. If TextFont and
		/// FontSize are specified the barcode text will be displayed
		/// under the barcode. It will be horizontally centered in relation
		/// to the barcode.
		/// </para>
		/// <para>
		/// Barcode text is displayed below PosY. Make sure to leave
		/// space under the barcode.
		/// </para>
		/// <para>
		/// If color other than black is given make sure there is
		/// a good contrast to white.
		/// </para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public double DrawBarcode
				(
				double PosX,
				double PosY,
				TextJustify Justify,
				double BarWidth,
				double BarHeight,
				Color BarColor,
				Barcode Barcode,
				PdfFont TextFont = null,
				double FontSize = 0.0
				)
			{
			// save graphics state
			SaveGraphicsState();

			// set barcode color
			SetColorNonStroking(BarColor);

			// barcode width
			double TotalWidth = BarWidth * Barcode.TotalWidth;

			// switch for adjustment
			switch(Justify)
				{
				case TextJustify.Center:
					PosX -= 0.5 * TotalWidth;
					break;

				case TextJustify.Right:
					PosX -= TotalWidth;
					break;
				}

			// initial bar position
			double BarPosX = PosX;

			// initialize bar color to black
			bool Bar = true;

			// all barcodes except EAN-13 or UPC-A
			if(Barcode.GetType() != typeof(BarcodeEAN13))
				{
				// loop for all bars
				for(int Index = 0; Index < Barcode.BarCount; Index++)
					{
					// bar width in user units
					double Width = BarWidth * Barcode.BarWidth(Index);

					// draw black bars
					if(Bar)
						DrawRectangle(BarPosX, PosY, Width, BarHeight, PaintOp.Fill);

					// update bar position and color
					BarPosX += Width;
					Bar = !Bar;
					}

				// display text if font is specified
				if(TextFont != null)
					DrawBarcodeText(TextFont, FontSize, PosX + 0.5 * TotalWidth, PosY, TextJustify.Center, Barcode.Text);
				}

			// EAN-13 or UPC-A
			else
				{
				// loop for all bars
				for(int Index = 0; Index < Barcode.BarCount; Index++)
					{
					// bar width in user units
					double Width = BarWidth * Barcode.BarWidth(Index);

					// adjust vertical position
					double DeltaY = Index < 7 || Index >= 27 && Index < 32 || Index >= 52 ? 0.0 : 5 * BarWidth;

					// draw black bars
					if(Bar)
						DrawRectangle(BarPosX, PosY + DeltaY, Width, BarHeight - DeltaY, PaintOp.Fill);

					// update bar position and color
					BarPosX += Width;
					Bar = !Bar;
					}

				// display text if font is specified
				if(TextFont != null)
					{
					// substrings positions
					double PosX1 = PosX - 2.0 * BarWidth;
					double PosX2 = PosX + 27.5 * BarWidth;
					double PosX3 = PosX + 67.5 * BarWidth;
					double PosX4 = PosX + 97.0 * BarWidth;
					double PosY1 = PosY + 5.0 * BarWidth;

					// UPC-A
					if(Barcode.Text.Length == 12)
						{
						DrawBarcodeText(TextFont, FontSize, PosX1, PosY1, TextJustify.Right, Barcode.Text.Substring(0, 1));
						DrawBarcodeText(TextFont, FontSize, PosX2, PosY1, TextJustify.Center, Barcode.Text.Substring(1, 5));
						DrawBarcodeText(TextFont, FontSize, PosX3, PosY1, TextJustify.Center, Barcode.Text.Substring(6, 5));
						DrawBarcodeText(TextFont, FontSize, PosX4, PosY1, TextJustify.Left, Barcode.Text.Substring(11));
						}
					// EAN-13
					else
						{
						DrawBarcodeText(TextFont, FontSize, PosX1, PosY1, TextJustify.Right, Barcode.Text.Substring(0, 1));
						DrawBarcodeText(TextFont, FontSize, PosX2, PosY1, TextJustify.Center, Barcode.Text.Substring(1, 6));
						DrawBarcodeText(TextFont, FontSize, PosX3, PosY1, TextJustify.Center, Barcode.Text.Substring(7));
						}
					}
				}

			// restore graphics state
			RestoreGraphicsState();

			// return width
			return TotalWidth;
			}

		////////////////////////////////////////////////////////////////////
		// Draw barcode text
		////////////////////////////////////////////////////////////////////

		private void DrawBarcodeText
				(
				PdfFont Font,
				double FontSize,
				double CenterPos,
				double TopPos,
				TextJustify Justify,
				string Text
				)
			{
			// test for non printable characters
			int Index;
			for(Index = 0; Index < Text.Length && Text[Index] >= ' ' && Text[Index] <= '~'; Index++)
				;
			if(Index < Text.Length)
				{
				StringBuilder Str = new StringBuilder(Text);
				for(; Index < Text.Length; Index++)
					if(Str[Index] < ' ' || Str[Index] > '~')
						Str[Index] = ' ';
				Text = Str.ToString();
				}

			// draw the text
			DrawText(Font, FontSize, CenterPos, TopPos - Font.Ascent(FontSize), Justify, Text);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw image (Height is calculated from width as per aspect ratio)
		/// </summary>
		/// <param name="Image">PdfImage resource</param>
		/// <param name="OriginX">Origin X</param>
		/// <param name="OriginY">Origin Y</param>
		/// <param name="Width">Display width</param>
		/// <remarks>
		/// The chart will be stretched or shrunk to fit the display width
		/// and display height. Use PdfImage.ImageSize(...) or 
		/// PdfImage.ImageSizePosition(...) to ensure correct aspect ratio 
		/// and positioning.
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public void DrawImage
				(
				PdfImage Image,
				double OriginX,
				double OriginY,
				double Width
				)
			{
			// add image code to current list of resources
			AddToUsedResources(Image);

			// draw image
			ObjectValueFormat("q {0} 0 0 {1} {2} {3} cm {4} Do Q\n",
				ToPt(Width), ToPt(Width * Image.HeightPix / Image.WidthPix), ToPt(OriginX), ToPt(OriginY), Image.ResourceCode);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw image
		/// </summary>
		/// <param name="Image">PdfImage resource</param>
		/// <param name="OriginX">Origin X</param>
		/// <param name="OriginY">Origin Y</param>
		/// <param name="Width">Display width</param>
		/// <param name="Height">Display height</param>
		/// <remarks>
		/// The chart will be stretched or shrunk to fit the display width
		/// and display height. Use PdfImage.ImageSize(...) or 
		/// PdfImage.ImageSizePosition(...) to ensure correct aspect ratio 
		/// and positioning.
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public void DrawImage
				(
				PdfImage Image,
				double OriginX,
				double OriginY,
				double Width,
				double Height
				)
			{
			// add image code to current list of resources
			AddToUsedResources(Image);

			// draw image
			ObjectValueFormat("q {0} 0 0 {1} {2} {3} cm {4} Do Q\n",
				ToPt(Width), ToPt(Height), ToPt(OriginX), ToPt(OriginY), Image.ResourceCode);
			return;
			}

		/// <summary>
		/// Draw WPF path
		/// </summary>
		/// <param name="Path">DrawWPFPath class</param>
		/// <param name="OriginX">Origin X</param>
		/// <param name="OriginY">Origin Y</param>
		/// <param name="Width">Path area width</param>
		/// <param name="Height">Path area height</param>
		/// <param name="Alignment">Alignment of path bounding box in drawing area. 0=no alignment</param>
		public void DrawWPFPath
				(
				DrawWPFPath Path,
				double OriginX,
				double OriginY,
				double Width,
				double Height,
				ContentAlignment Alignment = (ContentAlignment) 0
				)
			{
			// draw WPF path
			Path.Draw(this, OriginX, OriginY, Width, Height, Alignment);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw chart
		/// </summary>
		/// <param name="PdfChart">PdfChart resource</param>
		/// <param name="OriginX">Origin X</param>
		/// <param name="OriginY">Origin Y</param>
		/// <param name="DisposeChart">Dispose chart</param>
		/// <param name="GCCollect">Run the garbage collector</param>
		/// <remarks>
		/// <para>
		/// The chart is saved in the PDF document as an image.
		/// </para>
		/// <para>
		/// The PdfChart resource contains a .NET Chart class. The .NET
		/// Chart defines width and height in pixels and image resolution
		/// in pixels per inch. This method calculates the chart's display
		/// width and height based on these values.
		/// </para>
		/// <para>
		/// The .NET Chart member is defined by the user. It must be
		/// disposed in order to free unmanaged resources. If
		/// DisposeChart is true the DrawChart will call the Chart.Dispose()
		/// method. If the DisposeChart is false it is the responsibility
		/// of the calling method to dispose of the chart.
		/// </para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public void DrawChart
				(
				PdfChart PdfChart,
				double OriginX,
				double OriginY,
				bool DisposeChart = true,
				bool GCCollect = true
				)
			{
			// write chart as an image to PDF output file
			PdfChart.CommitToPdfFile(DisposeChart, GCCollect);

			// add chart code to current list of resources
			AddToUsedResources(PdfChart);

			// draw chart image
			ObjectValueFormat("q {0} 0 0 {1} {2} {3} cm {4} Do Q\n",
				ToPt(PdfChart.Width), ToPt(PdfChart.Height), ToPt(OriginX), ToPt(OriginY), PdfChart.ResourceCode);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw chart
		/// </summary>
		/// <param name="PdfChart">PdfChart resource</param>
		/// <param name="OriginX">Origin X</param>
		/// <param name="OriginY">Origin Y</param>
		/// <param name="Width">Display width</param>
		/// <param name="Height">Display height</param>
		/// <param name="DisposeChart">Dispose chart</param>
		/// <param name="GCCollect">Run the garbage collector</param>
		/// <remarks>
		/// <para>
		/// The chart is saved in the PDF document as an image.
		/// </para>
		/// <para>
		/// The PdfChart resource contains a .NET Chart class. The .NET
		/// Chart defines width and height in pixels and image resolution
		/// in pixels per inch.
		/// </para>
		/// <para>
		/// The chart will be stretched or shrunk to fit the display width
		/// and display height. Use PdfChart.ImageSize(...) or 
		/// PdfChart.ImageSizePosition(...) to ensure correct aspect ratio 
		/// and positioning.
		/// </para>
		/// <para>
		/// The .NET Chart member is defined by the user. It must be
		/// disposed in order to free unmanaged resources. If
		/// DisposeChart is true the DrawChart will call the Chart.Dispose()
		/// method. If the DisposeChart is false it is the responsibility
		/// of the calling method to dispose of the chart.
		/// </para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public void DrawChart
				(
				PdfChart PdfChart,
				double OriginX,
				double OriginY,
				double Width,
				double Height,
				bool DisposeChart = true,
				bool GCCollect = true
				)
			{
			// write chart as an image to PDF output file
			PdfChart.CommitToPdfFile(DisposeChart, GCCollect);

			// add image code to current list of resources
			AddToUsedResources(PdfChart);

			// draw chart image
			ObjectValueFormat("q {0} 0 0 {1} {2} {3} cm {4} Do Q\n",
				ToPt(Width), ToPt(Height), ToPt(OriginX), ToPt(OriginY), PdfChart.ResourceCode);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw X Object
		/// </summary>
		/// <param name="XObject">X Object resource</param>
		/// <remarks>
		/// X object is displayed at current position. X object Size
		/// is as per X object.
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public void DrawXObject
				(
				PdfXObject XObject
				)
			{
			// add image code to current list of resources
			AddToUsedResources(XObject);

			// draw object
			ObjectValueFormat("{0} Do\n", XObject.ResourceCode);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw X Object
		/// </summary>
		/// <param name="XObject">X Object resource</param>
		/// <param name="OriginX">Origin X</param>
		/// <param name="OriginY">Origin Y</param>
		/// <remarks>
		/// X object Size is as per X object.
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public void DrawXObject
				(
				PdfXObject XObject,
				double OriginX,
				double OriginY
				)
			{
			SaveGraphicsState();
			Translate(OriginX, OriginY);
			DrawXObject(XObject);
			RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw X Object
		/// </summary>
		/// <param name="XObject">X Object resource</param>
		/// <param name="OriginX">Origin X</param>
		/// <param name="OriginY">Origin Y</param>
		/// <param name="ScaleX">Horizontal scale factor</param>
		/// <param name="ScaleY">Vertical scale factor</param>
		////////////////////////////////////////////////////////////////////
		public void DrawXObject
				(
				PdfXObject XObject,
				double OriginX,
				double OriginY,
				double ScaleX,
				double ScaleY
				)
			{
			SaveGraphicsState();
			TranslateScale(OriginX, OriginY, ScaleX, ScaleY);
			DrawXObject(XObject);
			RestoreGraphicsState();
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Draw X Object
		/// </summary>
		/// <param name="XObject">X Object resource</param>
		/// <param name="OriginX">Origin X</param>
		/// <param name="OriginY">Origin Y</param>
		/// <param name="ScaleX">Horizontal scale factor</param>
		/// <param name="ScaleY">Vertical scale factor</param>
		/// <param name="Alpha">Rotation angle</param>
		////////////////////////////////////////////////////////////////////
		public void DrawXObject
				(
				PdfXObject XObject,
				double OriginX,
				double OriginY,
				double ScaleX,
				double ScaleY,
				double Alpha
				)
			{
			SaveGraphicsState();
			TranslateScaleRotate(OriginX, OriginY, ScaleX, ScaleY, Alpha);
			DrawXObject(XObject);
			RestoreGraphicsState();
			return;
			}

		// Add resource to list of used resources
		internal void AddToUsedResources
				(
				PdfObject ResObject
				)
			{
			if(ResObjects == null) ResObjects = new List<PdfObject>();
			int Index = ResObjects.BinarySearch(ResObject);
			if(Index < 0) ResObjects.Insert(~Index, ResObject);
			return;
			}

		/// <summary>
		/// Commit object to PDF file
		/// </summary>
		/// <param name="GCCollect">Activate Garbage Collector</param>
		public void CommitToPdfFile
				(
				bool GCCollect
				)
			{
			// make sure object was not written before
			if(FilePosition == 0)
				{
				// call PdfObject routine
				WriteObjectToPdfFile();

				// activate garbage collector
				if(GCCollect)
					GC.Collect();
				}
			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Write object to PDF file
		////////////////////////////////////////////////////////////////////

		internal override void WriteObjectToPdfFile()
			{
			// build resource dictionary for non page contents
			if(!PageContents)
				Dictionary.Add("/Resources", BuildResourcesDictionary(ResObjects, false));

			// call PdfObject routine
			base.WriteObjectToPdfFile();

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Set paint operator
		/// </summary>
		/// <param name="PP">Paint operator from custom string</param>
		////////////////////////////////////////////////////////////////////
		public void SetPaintOp
				(
				string PP
				)
			{
			// apply paint operator
			ObjectValueFormat("{0}\n", PP);
			return;
			}
		}
	}
