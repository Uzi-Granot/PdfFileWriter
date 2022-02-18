/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PdfContents
//	PDF contents indirect object. Support for page contents,
//  X Objects and Tilling Patterns.
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
	/// Barcode justify enumeration
	/// </summary>
	public enum BarcodeJustify
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
		private static readonly string[] PaintStr =
			new string[] { "", "n", "S", "s", "f", "f*", "B", "B*", "b", "b*", "h W n", "h W* n", "h" };

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

		//////////////////////////////////////////////////////////
		// Low level Graphics Methods							//
		//////////////////////////////////////////////////////////

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
		/// Set paint operator
		/// </summary>
		/// <param name="PP">Paint operator</param>
		public void SetPaintOp
				(
				PaintOp PP
				)
			{
			// apply paint operator
			if(PP != PaintOp.NoOperator)
				ObjectValueFormat("{0}\n", PaintStr[(int) PP]);
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
		/// Set color for non stroking (fill or brush) operations
		/// </summary>
		/// <param name="Color">Color</param>
		/// <remarks>Set red, green and blue components. Alpha is ignored</remarks>
		public void SetColorNonStroking
				(
				Color Color
				)
			{
			ObjectValueAppend(ColorToString(Color, ColorToStr.NonStroking) + "\n");
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
			ObjectValueAppend(ColorToString(Color, ColorToStr.Stroking) + "\n");
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
			SetAlpha(Alpha, "/CA");
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
			SetAlpha(Alpha, "/ca");
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
		/// Begin marked content
		/// </summary>
		/// <param name="Tag">Tag (must start with /)</param>
		public void BeginMarkedContent
				(
				string Tag
				)
			{
			ObjectValueFormat("{0} BMC\n", Tag);
			return;
			}

		/// <summary>
		/// Begin marked content with resources
		/// </summary>
		/// <param name="Tag">Tag (must start with /)</param>
		/// <param name="Resource">Resource</param>
		public void BeginMarkedContent
				(
				string Tag,
				PdfObject Resource
				)
			{
			AddToUsedResources(Resource);
			ObjectValueFormat("{0} {1} BDC\n", Tag, Resource.ResourceCode);
			return;
			}

		/// <summary>
		/// End marked content (Layer end)
		/// </summary>
		public void EndMarkedContent()
			{
			ObjectValueFormat("EMC\n");
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
			AddToUsedResources(Layer);
			ObjectValueFormat("/OC {0} BDC\n", Layer.ResourceCode);
			return;
			}

		/// <summary>
		/// End marked content (Layer end)
		/// </summary>
		public void LayerEnd()
			{
			ObjectValueFormat("EMC\n");
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
				Round(ScaleX * Math.Cos(Rotate)), Round(ScaleY * Math.Sin(Rotate)),
				Round(ScaleX * Math.Sin(-Rotate)), Round(ScaleY * Math.Cos(Rotate)));
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
			ObjectValueFormat("{0} {1} m\n", ToPt(Point.X), ToPt(Point.Y));
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
			ObjectValueFormat("{0} {1} l\n", ToPt(Point.X), ToPt(Point.Y));
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
			ObjectValueFormat("{0} {1} m {2} {3} l S\n", ToPt(Line.P1.X), ToPt(Line.P1.Y), ToPt(Line.P2.X), ToPt(Line.P2.Y));
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
			ObjectValueFormat("q {0} w {1} {2} m {3} {4} l S Q\n", ToPt(LineWidth), ToPt(Line.P1.X), ToPt(Line.P1.Y), ToPt(Line.P2.X), ToPt(Line.P2.Y));
			return;
			}

		/// <summary>
		/// Draw line with given line width
		/// </summary>
		/// <param name="Line">Line</param>
		/// <param name="LineWidth">Line width</param>
		/// <param name="LineColor">Line color</param>
		public void DrawLine
				(
				LineD Line,
				double LineWidth,
				Color LineColor
				)
			{
			ObjectValueFormat("q {0} w {1} 0 J {2} {3} m {4} {5} l S Q\n", ToPt(LineWidth),
				ColorToString(LineColor, ColorToStr.Stroking), ToPt(Line.P1.X), ToPt(Line.P1.Y), ToPt(Line.P2.X), ToPt(Line.P2.Y));
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
			switch (Point1Action)
				{
				case BezierPointOne.MoveTo:
					MoveTo(Bezier.P1);
					break;

				case BezierPointOne.LineTo:
					LineTo(Bezier.P1);
					break;
				}

			DrawBezier(Bezier.P2, Bezier.P3, Bezier.P4);
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
			ObjectValueFormat("{0} {1} {2} {3} {4} {5} c\n", ToPt(P1.X), ToPt(P1.Y), ToPt(P2.X), ToPt(P2.Y), ToPt(P3.X), ToPt(P3.Y));
			return;
			}

		/// <summary>
		/// Draw Bezier cubic path (P1 is the same as current point)
		/// </summary>
		/// <param name="P2">Point 2</param>
		/// <param name="P3">Point 3</param>
		public void DrawBezierP2andP3
				(
				PointD P2,
				PointD P3
				)
			{
			ObjectValueFormat("{0} {1} {2} {3} v\n", ToPt(P2.X), ToPt(P2.Y), ToPt(P3.X), ToPt(P3.Y));
			return;
			}

		/// <summary>
		/// Draw Bezier cubic path (P2 is the same as P3)
		/// </summary>
		/// <param name="P1">Point 1</param>
		/// <param name="P3">Point 3</param>
		public void DrawBezierP1andP3
				(
				PointD P1,
				PointD P3
				)
			{
			ObjectValueFormat("{0} {1} {2} {3} y\n", ToPt(P1.X), ToPt(P1.Y), ToPt(P3.X), ToPt(P3.Y));
			return;
			}

		/// <summary>
		/// Begin text mode
		/// </summary>
		public void BeginTextMode()
			{
			ObjectValueAppend("BT\n");
			return;
			}

		/// <summary>
		/// End text mode
		/// </summary>
		public void EndTextMode()
			{
			ObjectValueAppend("ET\n");
			return;
			}

		/// <summary>
		/// Set font resource and font size
		/// </summary>
		/// <param name="TypeOneFont">Type one font</param>
		/// <param name="FontSize">Font size in points</param>
		public void SetFontAndSize
				(
				PdfFontTypeOne TypeOneFont,
				double FontSize
				)
			{
			// convert font size to string
			ObjectValueFormat("{0} {1} Tf\n", TypeOneFont.ResourceCode,
				string.Format(NFI.PeriodDecSep, "{0}", Round(FontSize)));
			return;
			}

		/// <summary>
		/// Set text position
		/// </summary>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		public void SetTextPosition
				(
				double PosX,
				double PosY
				)
			{
			ObjectValueFormat("{0} {1} Td\n", ToPt(PosX), ToPt(PosY));
			return;
			}

		/// <summary>
		/// Set text rendering mode
		/// </summary>
		/// <param name="TR">Text rendering mode enumeration</param>
		public void SetTextRenderingMode
				(
				TextRendering TR
				)
			{
			ObjectValueFormat("{0} Tr\n", (int)TR);
			return;
			}

		/// <summary>
		/// Set character extra spacing
		/// </summary>
		/// <param name="ExtraSpacing">Character extra spacing</param>
		public void SetCharacterSpacing
				(
				double ExtraSpacing
				)
			{
			ObjectValueFormat("{0} Tc\n", ToPt(ExtraSpacing));
			return;
			}

		/// <summary>
		/// Set word extra spacing
		/// </summary>
		/// <param name="Spacing">Word extra spacing</param>
		public void SetWordSpacing
				(
				double Spacing
				)
			{
			ObjectValueFormat("{0} Tw\n", ToPt(Spacing));
			return;
			}

		//////////////////////////////////////////////////////////
		// Draw Polygon, Arc, Heart, Bezier Graphics Methods	//
		//////////////////////////////////////////////////////////
		
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
			if (PathArray.Length < 2)
				throw new ApplicationException("DrawPolygon: path must have at least two points");

			// move to first point
			ObjectValueFormat("{0} {1} m\n", ToPt(PathArray[0].X), ToPt(PathArray[0].Y));

			// draw lines		
			for (int Index = 1; Index < PathArray.Length; Index++)
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
			if (PathArray.Length < 4 || (PathArray.Length & 1) != 0)
				throw new ApplicationException("DrawPolygon: Path must be even and have at least 4 items");

			// move to first point
			ObjectValueFormat("{0} {1} m\n", ToPt(PathArray[0]), ToPt(PathArray[1]));

			// draw lines		
			for (int Index = 2; Index < PathArray.Length; Index += 2)
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
			if (Sides < 3)
				throw new ApplicationException("DrawRegularPolygon. Must have 3 or more sides");

			// polygon angle
			double DeltaAlpha = 2.0 * Math.PI / Sides;

			// first corner coordinates
			MoveTo(new PointD(Center, Radius, Alpha));

			for (int Side = 1; Side < Sides; Side++)
				{
				Alpha += DeltaAlpha;
				LineTo(new PointD(Center, Radius, Alpha));
				}

			// set paint operator
			SetPaintOp(PP);
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
			switch (OutputStartPoint)
				{
				case BezierPointOne.MoveTo:
					MoveTo(ArcStart);
					break;

				case BezierPointOne.LineTo:
					LineTo(ArcStart);
					break;
				}

			// create arc
			PointD[] SegArray = PdfArcToBezier.CreateArc(ArcStart, ArcEnd, Radius, Rotate, Type);

			// output
			for (int Index = 1; Index < SegArray.Length;)
				DrawBezier(SegArray[Index++], SegArray[Index++], SegArray[Index++]);
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
			double Radius1;

			// for polygon with less than 5, set inner radius to half the main radius
			if (Sides < 5)
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
			if (Sides < 3)
				throw new ApplicationException("DrawStar. Must have 3 or more sides");

			// move to first point
			MoveTo(new PointD(Center, Radius1, Alpha));

			// increment angle
			double DeltaAlpha = Math.PI / Sides;

			// double number of sides
			Sides *= 2;

			// line to the rest of the points
			for (int Side = 1; Side < Sides; Side++)
				{
				Alpha += DeltaAlpha;
				LineTo(new PointD(Center, (Side & 1) != 0 ? Radius2 : Radius1, Alpha));
				}

			// set paint operator
			SetPaintOp(PP);
			return;
			}

		//////////////////////////////////////////////////////////
		// Higher level Graphics Methods						//
		//////////////////////////////////////////////////////////

		/// <summary>
		/// Draw graphics: rectangle, rounded rectangle, oval
		/// </summary>
		/// <param name="Rect">PDF Rectangle</param>
		/// <param name="DrawCtrl">Draw rectangle control</param>
		public void DrawGraphics
				(
				PdfDrawCtrl DrawCtrl,
				PdfRectangle Rect
				)
			{
			// fill rectangle
			PdfRectangle FillRect;

			// adjust fill rectangle if there is a border with non zero width
			if((DrawCtrl.Paint == DrawPaint.Border || DrawCtrl.Paint == DrawPaint.BorderAndFill) && DrawCtrl.BorderWidth > 0)
				{
				FillRect = Rect.AddMargin(-0.5 * DrawCtrl.BorderWidth);
				}
			else
				{
				FillRect = Rect;
				}

			// save graphics state
			SaveGraphicsState();

			// blend mode
			if(DrawCtrl.Blend != BlendMode.Normal) SetBlendMode(DrawCtrl.Blend);

			switch(DrawCtrl._BackgroundTextureType)
				{
				case BackgroundTextureType.Color:
				case BackgroundTextureType.TilingPattern:
					{
					// set paint operator
					PaintOp PP;
					switch(DrawCtrl.Paint)
						{
						case DrawPaint.Border:
							PP = PaintOp.CloseStroke;
							SetBorder(DrawCtrl);
							break;

						case DrawPaint.Fill:
							PP = PaintOp.Fill;
							SetFillPaint(DrawCtrl);
							break;

						case DrawPaint.BorderAndFill:
							PP = PaintOp.CloseFillStroke;
							SetBorder(DrawCtrl);
							SetFillPaint(DrawCtrl);
							break;

						default:
							throw new ApplicationException("Draw rectangle invalid paint operator");
						}

					// draw shape
					DrawShape(FillRect, DrawCtrl, PP);
					}
					break;

				case BackgroundTextureType.Image:
					{ 
					// clip shape area
					if(DrawCtrl.Shape != PdfFileWriter.DrawShape.Rectangle) ClipGraphics(Rect, DrawCtrl);

					DrawImage((PdfImage) DrawCtrl._BackgroundTexture, FillRect);

					// add border
					if(DrawCtrl.Paint == DrawPaint.Border || DrawCtrl.Paint == DrawPaint.BorderAndFill)
						{
						// save graphics state
						SaveGraphicsState();

						// border
						SetBorder(DrawCtrl);

						// draw border
						DrawShape(FillRect, DrawCtrl, PaintOp.CloseStroke);

						// restore graphics state
						RestoreGraphicsState();
						}
					}
					break;

				case BackgroundTextureType.Shading:
					{ 
					// clip overall area
					ClipGraphics(FillRect, DrawCtrl);

					// set bounding box
					if(DrawCtrl._BackgroundTexture.GetType() == typeof(PdfAxialShading))
						((PdfAxialShading) DrawCtrl._BackgroundTexture).BBox = FillRect;
					else
						((PdfRadialShading) DrawCtrl._BackgroundTexture).BBox = FillRect;

					// draw shading
					AddToUsedResources((PdfObject) DrawCtrl._BackgroundTexture);
					ObjectValueFormat("{0} sh\n", ((PdfObject) DrawCtrl._BackgroundTexture).ResourceCode);

					// add border
					if(DrawCtrl.Paint == DrawPaint.Border || DrawCtrl.Paint == DrawPaint.BorderAndFill)
						{
						// save graphics state
						SaveGraphicsState();

						// border
						SetBorder(DrawCtrl);

						// draw border
						DrawShape(FillRect, DrawCtrl, PaintOp.CloseStroke);

						// restore graphics state
						RestoreGraphicsState();
						}
					}
					break;
				}

			// restore graphics state
			RestoreGraphicsState();
			return;
			}

		/// <summary>
		/// Clip closed shape
		/// </summary>
		/// <param name="Rect">Rectangle</param>
		/// <param name="DrawCtrl">Draw control</param>
		public void ClipGraphics
				(
				PdfRectangle Rect,
				PdfDrawCtrl DrawCtrl
				)
			{
			DrawShape(Rect, DrawCtrl, PaintOp.ClipPathEor);
			return;
			}

		/// <summary>
		/// Draw one character as a symbol
		/// </summary>
		/// <param name="Symbol">Symbol class</param>
		/// <param name="Rect">Drawing rectangle</param>
		/// <param name="FillColor">Fill or non stroking color</param>
		public void DrawSymbol
				(
				PdfSymbol Symbol,
				PdfRectangle Rect,
				Color FillColor
				)
			{
			// symbol bounds
			RectangleF Bounds = Symbol.Bounds;

			// scale factor from grapics path to page coordinates
			double Scale = Rect.Width / Bounds.Width;
			double ScaleY = Rect.Height / Bounds.Height;
			if(ScaleY < Scale) Scale = ScaleY;

			// origin of path in page rectangle 
			double OrgX = Rect.Left + (Rect.Width - Scale * Bounds.Width) / 2;
			double OrgY = Rect.Top - (Rect.Height - Scale * Bounds.Height) / 2;

			// number of points, type and points arrays
			int Len = Symbol.Len;
			byte[] Types = Symbol.Types;
			PointF[] Points = Symbol.Points;

			// save grphics state
			SaveGraphicsState();

			// set fill color
			SetColorNonStroking(FillColor);

			// loop for all points
			for(int Index = 0; Index < Len; Index++)
				{
				int Type = Types[Index];
				double PointX = OrgX + Scale * (Points[Index].X - Bounds.X);
				double PointY = OrgY - Scale * (Points[Index].Y - Bounds.Top);

				switch(Type & (int) SymbolPointType.PathTypeMask)
					{
					case (int) SymbolPointType.Start:
						ObjectValueFormat("{0} {1} m\n", ToPt(PointX), ToPt(PointY));
						break;

					case (int) SymbolPointType.Line:
						ObjectValueFormat("{0} {1} l\n", ToPt(PointX), ToPt(PointY));
						break;

					case (int) SymbolPointType.Bezier:
						ObjectValueFormat("{0} {1} ", ToPt(PointX), ToPt(PointY));

						// second bezier point
						Index++;
						double PointX1 = OrgX + Scale * (Points[Index].X - Bounds.X);
						double PointY1 = OrgY - Scale * (Points[Index].Y - Bounds.Top);
						ObjectValueFormat("{0} {1} ", ToPt(PointX1), ToPt(PointY1));

						// third bezier point
						Index++;
						PointX1 = OrgX + Scale * (Points[Index].X - Bounds.X);
						PointY1 = OrgY - Scale * (Points[Index].Y - Bounds.Top);
						ObjectValueFormat("{0} {1} c\n", ToPt(PointX1), ToPt(PointY1));
						break;
					}
				}

			// paint the symbol
			SetPaintOp(PaintOp.Fill);

			// save grphics state
			SaveGraphicsState();
			return;
			}

		//////////////////////////////////////////////////////////
		// Draw one line of text Methods						//
		//////////////////////////////////////////////////////////

		/// <summary>
		/// Draw one line of text
		/// </summary>
		/// <param name="TextCtrl">PDF draw text control</param>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		/// <param name="Text">Text</param>
		/// <returns>Text width</returns>
		public double DrawText
				(
				PdfDrawTextCtrl TextCtrl,
				double PosX,
				double PosY,
				string Text
				)
			{
			// text is null or empty
			if(string.IsNullOrEmpty(Text)) return 0;

			// text width
			double TextWidth = 0;

			// we have color
			if(TextCtrl.TextColor != Color.Empty)
				{
				// save graphics state
				SaveGraphicsState();

				// change non stroking color
				SetColorNonStroking(TextCtrl.TextColor);
				}

			// not subscript or superscript
			if((TextCtrl.DrawStyle & (DrawStyle.Subscript | DrawStyle.Superscript)) == 0)
				{
				// draw text string
				TextWidth = DrawTextNormal(TextCtrl, PosX, PosY, Text);

				// not regular style
				if(TextCtrl.DrawStyle != DrawStyle.Normal)
					{
					// change stroking color
					if(TextCtrl.TextColor != Color.Empty) SetColorStroking(TextCtrl.TextColor);

					// adjust X position for line drawing
					double LinePosX = PosX;
					switch(TextCtrl.Justify)
						{
						// right
						case TextJustify.Right:
							LinePosX -= TextWidth;
							break;

						// center
						case TextJustify.Center:
							LinePosX -= 0.5 * TextWidth;
							break;
						}

					// underline
					if((TextCtrl.DrawStyle & DrawStyle.Underline) != 0)
						{
						double UnderlinePos = PosY + TextCtrl.UnderlinePosition;
						LineD UnderlineLine = new LineD(LinePosX, UnderlinePos, LinePosX + TextWidth, UnderlinePos);
						DrawLine(UnderlineLine, TextCtrl.UnderlineWidth);
						}

					// strikeout
					if((TextCtrl.DrawStyle & DrawStyle.Strikeout) != 0)
						{
						double StrikeoutPos = PosY + TextCtrl.StrikeoutPosition;
						LineD StrikeoutLine = new LineD(LinePosX, StrikeoutPos, LinePosX + TextWidth, StrikeoutPos);
						DrawLine(StrikeoutLine, TextCtrl.StrikeoutWidth);
						}
					}
				}

			// subscript or superscript
			else
				{
				// subscript
				if((TextCtrl.DrawStyle & (DrawStyle.Subscript | DrawStyle.Superscript)) == DrawStyle.Subscript)
					{
					// subscript font size and location
					PosY -= TextCtrl.SubscriptPosition;
					PdfDrawTextCtrl SubTextCtrl = new PdfDrawTextCtrl(TextCtrl);
					SubTextCtrl.FontSize = TextCtrl.SubscriptSize;

					// draw text string
					TextWidth = DrawTextNormal(SubTextCtrl, PosX, PosY, Text);
					}

				// superscript
				if((TextCtrl.DrawStyle & (DrawStyle.Subscript | DrawStyle.Superscript)) == DrawStyle.Superscript)
					{
					// superscript font size and location
					PosY += TextCtrl.SuperscriptPosition;
					PdfDrawTextCtrl SuperTextCtrl = new PdfDrawTextCtrl(TextCtrl);
					SuperTextCtrl.FontSize = TextCtrl.SuperscriptSize;

					// draw text string
					TextWidth = DrawTextNormal(SuperTextCtrl, PosX, PosY, Text);
					}
				}

			// we have color restore graphics state
			if(TextCtrl.TextColor != Color.Empty) RestoreGraphicsState();

			// annotation
			if(TextCtrl.Annotation != null)
				{
				// adjust position
				switch(TextCtrl.Justify)
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

				TextCtrl.Annotation.AnnotRect = new PdfRectangle(PosX, PosY - TextCtrl.TextDescent, PosX + TextWidth, PosY + TextCtrl.TextAscent);
				}

			// return text width
			return TextWidth;
			}

		/// <summary>
		/// Draw text with kerning
		/// </summary>
		/// <param name="TextCtrl">PDF draw text control</param>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		/// <param name="Text">Text</param>
		/// <returns>Text width</returns>
		public double DrawTextWithKerning
				(
				PdfDrawTextCtrl TextCtrl,
				double PosX,
				double PosY,
				string Text
				)
			{
			// text is null or empty
			if(string.IsNullOrEmpty(Text)) return 0;

			// create text position adjustment array based on kerning information
			PdfKerningAdjust[] KernArray = TextCtrl.Font.TextKerning(Text);

			// no kerning
			if(KernArray == null) return DrawText(TextCtrl, PosX, PosY, Text);

			// draw text with adjustment
			return DrawTextWithKerning(TextCtrl, PosX, PosY, KernArray);
			}

		/// <summary>
		/// Draw text with kerning array
		/// </summary>
		/// <param name="TextCtrl">PDF draw text control</param>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		/// <param name="KerningArray">Kerning array</param>
		/// <returns>Text width</returns>
		/// <remarks>
		/// Each kerning item consists of text and position adjustment.
		/// The adjustment is a negative number.
		/// </remarks>
		public double DrawTextWithKerning
				(
				PdfDrawTextCtrl TextCtrl,
				double PosX,
				double PosY,
				PdfKerningAdjust[] KerningArray
				)
			{
			// text is null or empty
			if(KerningArray == null || KerningArray.Length == 0) return 0;

			// add font code to current list of font codes
			AddToUsedResources(TextCtrl.Font);

			// draw text initialization
			BeginTextMode();
			SetTextPosition(PosX, PosY);

			// draw text with kerning
			double Width = DrawTextWithKerning(TextCtrl, KerningArray);

			// draw text termination
			EndTextMode();

			// exit
			return Width;
			}

		/// <summary>
		/// Draw text with special effects
		/// </summary>
		/// <param name="TextCtrl">Pdf draw text control</param>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		/// <param name="OutlineWidth">Outline width</param>
		/// <param name="OutlineColor">Stoking (outline) color</param>
		/// <param name="Text">Text</param>
		/// <returns>Text width</returns>
		public double DrawText
				(
				PdfDrawTextCtrl TextCtrl,
				double PosX,
				double PosY,
				double OutlineWidth,
				Color OutlineColor,
				string Text
				)
			{
			// text is null or empty
			if(string.IsNullOrEmpty(Text)) return 0;

			// add font code to current list of font codes
			AddToUsedResources(TextCtrl.Font);

			// save graphics state
			SaveGraphicsState();

			// create text position adjustment array based on kerning information
			PdfKerningAdjust[] KernArray = TextCtrl.Font.TextKerning(Text);

			// text width
			double Width = KernArray == null ? TextCtrl.TextWidth(Text) : TextCtrl.Font.TextKerningWidth(TextCtrl.FontSize, KernArray);

			// adjust position
			switch(TextCtrl.Justify)
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
			if(!OutlineColor.IsEmpty)
				{
				SetLineWidth(OutlineWidth);
				SetColorStroking(OutlineColor);
				TR = TextRendering.Stroke;
				}

			if(!TextCtrl.TextColor.IsEmpty)
				{
				SetColorNonStroking(TextCtrl.TextColor);
				TR = OutlineColor.IsEmpty ? TextRendering.Fill : TextRendering.FillStroke;
				}

			// draw text initialization
			BeginTextMode();
			SetTextPosition(PosX, PosY);
			SetTextRenderingMode(TR);

			// draw text without kerning
			if(KernArray == null)
				{
				DrawTextNoPos(TextCtrl, Text);
				}

			// draw text with kerning
			else
				{
				DrawTextWithKerning(TextCtrl, KernArray);
				}

			// draw text termination
			EndTextMode();

			// restore graphics state
			RestoreGraphicsState();

			// add rectangle to annotation
			if(TextCtrl.Annotation != null)
				{
				TextCtrl.Annotation.AnnotRect = new PdfRectangle(PosX, PosY - TextCtrl.TextDescent, PosX + Width, PosY + TextCtrl.TextAscent);
				}

			// exit
			return Width;
			}

		/// <summary>
		/// Clip text exposing area underneath
		/// </summary>
		/// <param name="TextCtrl">PDF draw text control</param>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		/// <param name="Text">Text</param>
		public void ClipText
				(
				PdfDrawTextCtrl TextCtrl,
				double PosX,
				double PosY,
				string Text
				)
			{
			// text is null or empty
			if (string.IsNullOrEmpty(Text)) return;

			// add font code to current list of font codes
			AddToUsedResources(TextCtrl.Font);

			// draw text
			BeginTextMode();
			SetTextPosition(PosX, PosY);
			SetTextRenderingMode(TextRendering.Clip);
			DrawTextNoPos(TextCtrl, Text);
			EndTextMode();
			return;
			}


		//////////////////////////////////////////////////////////
		//	Draw text box methods								//
		//////////////////////////////////////////////////////////

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
		public int DrawText
				(
				double PosX,
				ref double PosYTop,
				double PosYBottom,
				int LineNo,
				PdfTextBox TextBox,
				PdfPage Page = null
				)
			{
			return DrawText(PosX, ref PosYTop, PosYBottom, LineNo, 0.0, 0.0, TextBoxJustify.Left, TextBox, Page);
			}

		// Draw Text for TextBox with web link
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
		public int DrawText
				(
				double PosX,
				ref double PosYTop,
				double PosYBottom,
				int LineNo,
				double LineExtraSpace,
				double ParagraphExtraSpace,
				TextBoxJustify Justify,
				PdfTextBox TextBox,
				PdfPage Page = null
				)
			{
			TextBox.Terminate();
			for(; LineNo < TextBox.LineCount; LineNo++)
				{
				// short cut
				PdfTextBoxLine Line = TextBox[LineNo];

				// break out of the loop if printing below bottom line
				if(PosYTop - Line.LineHeight < PosYBottom) break;

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
				if(Line.EndOfParagraph) PosYTop -= ParagraphExtraSpace;
				}
			return LineNo;
			}

		//////////////////////////////////////////////////////////
		// Draw barcode methods									//
		//////////////////////////////////////////////////////////

		/// <summary>
		/// Draw barcode
		/// </summary>
		/// <param name="BarcodeCtrl">Barcode draw control</param>
		/// <param name="PosX">Position X</param>
		/// <param name="PosY">Position Y</param>
		/// <param name="Barcode">Derived barcode class</param>
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
		public double DrawBarcode
				(
				PdfDrawBarcodeCtrl BarcodeCtrl,
				double PosX,
				double PosY,
				PdfBarcode Barcode
				)
			{
			// set barcode color
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Paint = DrawPaint.Fill;
			DrawCtrl.BackgroundTexture = BarcodeCtrl.Color;

			// barcode width
			double TotalWidth = BarcodeCtrl.NarrowBarWidth * Barcode.TotalWidth;

			// switch for adjustment
			switch(BarcodeCtrl.Justify)
				{
				case BarcodeJustify.Center:
					PosX -= 0.5 * TotalWidth;
					break;

				case BarcodeJustify.Right:
					PosX -= TotalWidth;
					break;
				}

			// initial bar position
			double BarPosX = PosX;

			// initialize bar color to black
			bool Bar = true;

			// all barcodes except EAN-13 or UPC-A
			if(Barcode.GetType() != typeof(PdfBarcodeEAN13))
				{
				// loop for all bars
				for(int Index = 0; Index < Barcode.BarCount; Index++)
					{
					// bar width in user units
					double Width = BarcodeCtrl.NarrowBarWidth * Barcode.BarWidth(Index);

					// draw black bars
					if(Bar)
						{
						PdfRectangle Rect = new PdfRectangle(BarPosX, PosY, BarPosX + Width, PosY + BarcodeCtrl.Height);
						DrawGraphics(DrawCtrl, Rect);
						}

					// update bar position and color
					BarPosX += Width;
					Bar = !Bar;
					}

				// display text centered, if font is specified
				if(BarcodeCtrl.TextCtrl != null)
					{
					BarcodeCtrl.TextCtrl.Justify = TextJustify.Center;
					DrawBarcodeText(BarcodeCtrl.TextCtrl, PosX + 0.5 * TotalWidth, PosY, Barcode.Text);
					}
				}

			// EAN-13 or UPC-A
			else
				{
				// loop for all bars
				for(int Index = 0; Index < Barcode.BarCount; Index++)
					{
					// bar width in user units
					double Width = BarcodeCtrl.NarrowBarWidth * Barcode.BarWidth(Index);

					// adjust vertical position
					double DeltaY = Index < 7 || Index >= 27 && Index < 32 || Index >= 52 ? 0.0 : 5 * BarcodeCtrl.NarrowBarWidth;

					// draw black bars
					if(Bar)
						{
						PdfRectangle Rect = new PdfRectangle(BarPosX, PosY + DeltaY,
							BarPosX + Width, PosY + BarcodeCtrl.Height);
						DrawGraphics(DrawCtrl, Rect);
						}

					// update bar position and color
					BarPosX += Width;
					Bar = !Bar;
					}

				// display text if font is specified
				if(BarcodeCtrl.TextCtrl != null)
					{
					// substrings positions
					double PosX1 = PosX - 2.0 * BarcodeCtrl.NarrowBarWidth;
					double PosX2 = PosX + 27.5 * BarcodeCtrl.NarrowBarWidth;
					double PosX3 = PosX + 67.5 * BarcodeCtrl.NarrowBarWidth;
					double PosX4 = PosX + 97.0 * BarcodeCtrl.NarrowBarWidth;
					double PosY1 = PosY + 5.0 * BarcodeCtrl.NarrowBarWidth;

					// UPC-A
					if(Barcode.Text.Length == 12)
						{
						BarcodeCtrl.TextCtrl.Justify = TextJustify.Right;
						DrawBarcodeText(BarcodeCtrl.TextCtrl, PosX1, PosY1, Barcode.Text.Substring(0, 1));
						BarcodeCtrl.TextCtrl.Justify = TextJustify.Center;
						DrawBarcodeText(BarcodeCtrl.TextCtrl, PosX2, PosY1, Barcode.Text.Substring(1, 5));
						DrawBarcodeText(BarcodeCtrl.TextCtrl, PosX3, PosY1, Barcode.Text.Substring(6, 5));
						BarcodeCtrl.TextCtrl.Justify = TextJustify.Left;
						DrawBarcodeText(BarcodeCtrl.TextCtrl, PosX4, PosY1, Barcode.Text.Substring(11));
						}
					// EAN-13
					else
						{
						BarcodeCtrl.TextCtrl.Justify = TextJustify.Right;
						DrawBarcodeText(BarcodeCtrl.TextCtrl, PosX1, PosY1, Barcode.Text.Substring(0, 1));
						BarcodeCtrl.TextCtrl.Justify = TextJustify.Center;
						DrawBarcodeText(BarcodeCtrl.TextCtrl, PosX2, PosY1, Barcode.Text.Substring(1, 6));
						DrawBarcodeText(BarcodeCtrl.TextCtrl, PosX3, PosY1, Barcode.Text.Substring(7));
						}
					}
				}

			// return width
			return TotalWidth;
			}

		//////////////////////////////////////////////////////////
		// Draw image methods									//
		//////////////////////////////////////////////////////////

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

		/// <summary>
		/// Draw image
		/// </summary>
		/// <param name="Image">PdfImage resource</param>
		/// <param name="ImageRect">Image display rectangle</param>
		/// <remarks>
		/// The chart will be stretched or shrunk to fit the display width
		/// and display height. Use PdfImage.ImageSize(...) or 
		/// PdfImage.ImageSizePosition(...) to ensure correct aspect ratio 
		/// and positioning.
		/// </remarks>
		public void DrawImage
				(
				PdfImage Image,
				PdfRectangle ImageRect
				)
			{
			// add image code to current list of resources
			AddToUsedResources(Image);

			// draw image
			ObjectValueFormat("q {0} 0 0 {1} {2} {3} cm {4} Do Q\n",
				ToPt(ImageRect.Width), ToPt(ImageRect.Height), ToPt(ImageRect.Left), ToPt(ImageRect.Bottom), Image.ResourceCode);
			return;
			}

		//////////////////////////////////////////////////////////
		// Draw X Object methods								//
		//////////////////////////////////////////////////////////

		/// <summary>
		/// Draw X Object
		/// </summary>
		/// <param name="XObject">X Object resource</param>
		/// <remarks>
		/// X object is displayed at current position. X object Size
		/// is as per X object.
		/// </remarks>
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

		/// <summary>
		/// Draw X Object
		/// </summary>
		/// <param name="XObject">X Object resource</param>
		/// <param name="OriginX">Origin X</param>
		/// <param name="OriginY">Origin Y</param>
		/// <remarks>
		/// X object Size is as per X object.
		/// </remarks>
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

		/// <summary>
		/// Draw X Object
		/// </summary>
		/// <param name="XObject">X Object resource</param>
		/// <param name="OriginX">Origin X</param>
		/// <param name="OriginY">Origin Y</param>
		/// <param name="ScaleX">Horizontal scale factor</param>
		/// <param name="ScaleY">Vertical scale factor</param>
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

		/// <summary>
		/// Draw X Object
		/// </summary>
		/// <param name="XObject">X Object resource</param>
		/// <param name="OriginX">Origin X</param>
		/// <param name="OriginY">Origin Y</param>
		/// <param name="ScaleX">Horizontal scale factor</param>
		/// <param name="ScaleY">Vertical scale factor</param>
		/// <param name="Alpha">Rotation angle</param>
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

		//////////////////////////////////////////////////////////
		// Misc methods											//
		//////////////////////////////////////////////////////////

		/// <summary>
		/// Reverse characters in a string
		/// </summary>
		/// <param name="Text">Input string</param>
		/// <returns>Output string</returns>
		public static string ReverseString
				(
				string Text
				)
			{
			char[] RevText = Text.ToCharArray();
			Array.Reverse(RevText);
			return new string(RevText);
			}

		/// <summary>
		/// Make a color darker
		/// </summary>
		/// <param name="BaseColor">Base color</param>
		/// <param name="Factor">Factor 0 to 1</param>
		/// <returns>Darker color</returns>
		public static Color MakeDarker
				(
				Color BaseColor,
				double Factor
				)
			{
			// test argument
			if (Factor < 0 || Factor > 1) throw new ApplicationException("Make darker factor must be 0 to 1");

			// two extreme cases
			if (Factor == 0) return BaseColor;
			if (Factor == 1) return Color.Black;

			// test for gray
			if (BaseColor.R == BaseColor.G && BaseColor.R == BaseColor.B)
				{
				int Gray = (int)(BaseColor.R * (1 - Factor));
				return Color.FromArgb(Gray, Gray, Gray);
				}

			// color
			int Red = (int)(BaseColor.R * (1 - Factor));
			int Green = (int)(BaseColor.G * (1 - Factor));
			int Blue = (int)(BaseColor.B * (1 - Factor));
			return Color.FromArgb(Red, Green, Blue);
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
				WriteToPdfFile();

				// activate garbage collector
				if(GCCollect) GC.Collect();
				}
			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Set alpha channel
		////////////////////////////////////////////////////////////////////
		private void SetAlpha
				(
				double Alpha,
				string Code
				)
			{
			string AlphaStr = Alpha < 0.001 ? "0" : (Alpha > 0.999 ? "1" : Alpha.ToString("0.0##", NFI.PeriodDecSep));
			PdfExtGState ExtGState = PdfExtGState.CreateExtGState(Document, Code, AlphaStr);
			AddToUsedResources(ExtGState);
			ObjectValueFormat("{0} gs\n", ExtGState.ResourceCode);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Color to string
		////////////////////////////////////////////////////////////////////
		internal static string ColorToString
				(
				Color Color,
				ColorToStr Code
				)
			{
			// test for no color: transparent
			if (Color == Color.Empty)
				{
				return Code == ColorToStr.Array ? "[]" : "";
				}

			// test for gray
			if (Color.R == Color.G && Color.R == Color.B)
				{
				string CompStr = ColorComponent(Color.R);
				switch (Code)
					{
					case ColorToStr.Stroking:
						return CompStr + " G";
					case ColorToStr.NonStroking:
						return CompStr + " g";
					case ColorToStr.Array:
						return "[" + CompStr + "]";
					}
				}

			// color
			else
				{
				string CompRStr = ColorComponent(Color.R);
				string CompGStr = ColorComponent(Color.G);
				string CompBStr = ColorComponent(Color.B);
				switch (Code)
					{
					case ColorToStr.Stroking:
						return CompRStr + " " + CompGStr + " " + CompBStr + " RG";
					case ColorToStr.NonStroking:
						return CompRStr + " " + CompGStr + " " + CompBStr + " rg";
					case ColorToStr.Array:
						return "[" + CompRStr + " " + CompGStr + " " + CompBStr + "]";
					}
				}
			return "";
			}

		////////////////////////////////////////////////////////////////////
		// Color components
		////////////////////////////////////////////////////////////////////
		internal static string ColorComponent
				(
				int Comp
				)
			{
			// black
			if (Comp <= 0) return "0";

			// white
			if (Comp >= 255) return "1";

			// all other values 1 to 254
			return ((double)Comp / 255.0).ToString("0.0###", NFI.PeriodDecSep);
			}

		////////////////////////////////////////////////////////////////////
		// set border
		////////////////////////////////////////////////////////////////////
		private void SetBorder
				(
				PdfDrawCtrl DrawCtrl
				)
			{
			// set border width
			SetLineWidth(DrawCtrl.BorderWidth);

			// set border color
			SetColorStroking(DrawCtrl.BorderColor == Color.Empty ? Color.Black : DrawCtrl.BorderColor);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// set fill paint
		////////////////////////////////////////////////////////////////////
		private void SetFillPaint
				(
				PdfDrawCtrl DrawCtrl
				)
			{
			// set fill color with color
			if (DrawCtrl._BackgroundTextureType == BackgroundTextureType.Color)
				{
				if (DrawCtrl.BackgroundAlpha != 1) SetAlphaNonStroking(DrawCtrl.BackgroundAlpha);
				SetColorNonStroking((DrawCtrl._BackgroundTexture == null || (Color)DrawCtrl._BackgroundTexture == Color.Empty) ?
					Color.LightGray : (Color)DrawCtrl._BackgroundTexture);
				return;
				}

			// set fill color with tiling pattern
			SetPatternNonStroking((PdfTilingPattern)DrawCtrl._BackgroundTexture);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Draw textat current position
		////////////////////////////////////////////////////////////////////
		internal double DrawTextNoPos
				(
				PdfDrawTextCtrl TextCtrl,
				string Text
				)
			{
			// set last use glyph index
			bool GlyphIndexFlag = false;

			// text width
			int Width = 0;

			// shortcut for font
			PdfFont Font = TextCtrl.Font;

			// loop for all text characters
			for (int Ptr = 0; Ptr < Text.Length; Ptr++)
				{
				// get character information
				CharInfo CharInfo = Font.GetCharInfo(Text[Ptr]);

				// set active flag
				CharInfo.ActiveChar = true;

				// accumulate width
				Width += CharInfo.DesignWidth;

				// change between 0-255 and 255-65535
				if (Ptr == 0 || CharInfo.Type0Font != GlyphIndexFlag)
					{
					// ")Tj"
					if (Ptr != 0)
						{
						ObjectValueList.Add((byte)')');
						ObjectValueList.Add((byte)'T');
						ObjectValueList.Add((byte)'j');
						}

					// save glyph index
					GlyphIndexFlag = CharInfo.Type0Font;

					// ouput font resource and font size
					if (!GlyphIndexFlag)
						{
						Font.FontResCodeUsed = true;
						ObjectValueAppend(TextCtrl.FontResourceCode);
						}
					else
						{
						if (!Font.FontResGlyphUsed)
							{
							Font.GlyphIndexFont = new PdfObject(Document, ObjectType.Dictionary, "/Font");
							Font.FontResGlyphUsed = true;
							}
						ObjectValueAppend(TextCtrl.FontResourceGlyph);
						}
					ObjectValueList.Add((byte)'(');
					}

				// output character code
				if (!GlyphIndexFlag)
					{
					OutputOneByte(CharInfo.CharCode);
					}

				// output glyph index
				else
					{
					if (CharInfo.NewGlyphIndex < 0)
						CharInfo.NewGlyphIndex = Font.EmbeddedFont ? Font.NewGlyphIndex++ : CharInfo.GlyphIndex;
					OutputOneByte(CharInfo.NewGlyphIndex >> 8);
					OutputOneByte(CharInfo.NewGlyphIndex & 0xff);
					}
				}

			// ")Tj"
			ObjectValueList.Add((byte)')');
			ObjectValueList.Add((byte)'T');
			ObjectValueList.Add((byte)'j');
			ObjectValueList.Add((byte)'\n');
			return TextCtrl.FontDesignToUserUnits(Width);
			}

		////////////////////////////////////////////////////////////////////
		// output one byte of text
		////////////////////////////////////////////////////////////////////
		internal void OutputOneByte
				(
				int CharCode
				)
			{
			switch (CharCode)
				{
				case '\r':
					ObjectValueList.Add((byte)'\\');
					ObjectValueList.Add((byte)'r');
					return;
				case '\n':
					ObjectValueList.Add((byte)'\\');
					ObjectValueList.Add((byte)'n');
					return;
				case '(':
					ObjectValueList.Add((byte)'\\');
					ObjectValueList.Add((byte)'(');
					return;
				case ')':
					ObjectValueList.Add((byte)'\\');
					ObjectValueList.Add((byte)')');
					return;
				case '\\':
					ObjectValueList.Add((byte)'\\');
					ObjectValueList.Add((byte)'\\');
					return;
				default:
					ObjectValueList.Add((byte)CharCode);
					return;
				}
			}

		////////////////////////////////////////////////////////////////////
		// Draw text with kerning
		////////////////////////////////////////////////////////////////////
		internal double DrawTextWithKerning
				(
				PdfDrawTextCtrl TextCtrl,
				PdfKerningAdjust[] KerningArray
				)
			{
			// set last use glyph index
			bool GlyphIndexFlag = false;

			// text width
			int Width = 0;

			// loop for kerning pairs
			int Index = 0;
			for (; ; )
				{
				PdfKerningAdjust KA = KerningArray[Index];
				string Text = KA.Text;

				// loop for all text characters
				for (int Ptr = 0; Ptr < Text.Length; Ptr++)
					{
					// get character information
					CharInfo CharInfo = TextCtrl.Font.GetCharInfo(Text[Ptr]);

					// set active flag
					CharInfo.ActiveChar = true;

					// accumulate width
					Width += CharInfo.DesignWidth;

					// change between 0-255 and 255-65535
					if (Index == 0 && Ptr == 0 || CharInfo.Type0Font != GlyphIndexFlag)
						{
						// close partial string
						if (Ptr != 0) ObjectValueList.Add((byte)')');

						// close code/glyph area
						if (Index != 0)
							{
							ObjectValueList.Add((byte)']');
							ObjectValueList.Add((byte)'T');
							ObjectValueList.Add((byte)'J');
							}

						// save glyph index
						GlyphIndexFlag = CharInfo.Type0Font;

						// ouput font resource and font size
						if (!GlyphIndexFlag)
							{
							ObjectValueAppend(TextCtrl.FontResourceCode);
							}
						else
							{
							ObjectValueAppend(TextCtrl.FontResourceGlyph);
							}

						ObjectValueList.Add((byte)'[');
						ObjectValueList.Add((byte)'(');
						}

					else if (Ptr == 0)
						{
						ObjectValueList.Add((byte)'(');
						}

					// output character code
					if (!GlyphIndexFlag)
						{
						OutputOneByte(CharInfo.CharCode);
						}

					// output glyph index
					else
						{
						if (CharInfo.NewGlyphIndex < 0)
							CharInfo.NewGlyphIndex = TextCtrl.Font.EmbeddedFont ? TextCtrl.Font.NewGlyphIndex++ : CharInfo.GlyphIndex;
						OutputOneByte(CharInfo.NewGlyphIndex >> 8);
						OutputOneByte(CharInfo.NewGlyphIndex & 0xff);
						}
					}

				ObjectValueList.Add((byte)')');

				// test for end of kerning array
				Index++;
				if (Index == KerningArray.Length) break;

				// add adjustment
				ObjectValueFormat("{0}", Round(-KA.Adjust));

				// convert the adjustment width to font design width
				Width += (int)Math.Round(KA.Adjust * TextCtrl.Font.DesignHeight / 1000.0, 0, MidpointRounding.AwayFromZero);
				}

			// "]Tj"
			ObjectValueList.Add((byte)']');
			ObjectValueList.Add((byte)'T');
			ObjectValueList.Add((byte)'J');
			ObjectValueList.Add((byte)'\n');
			return TextCtrl.FontDesignToUserUnits(Width);
			}

		////////////////////////////////////////////////////////////////////
		// Draw shape
		////////////////////////////////////////////////////////////////////
		private void DrawShape
				(
				PdfRectangle Rect,
				PdfDrawCtrl DrawCtrl,
				PaintOp PP
				)
			{
			// shape
			switch (DrawCtrl.Shape)
				{
				case PdfFileWriter.DrawShape.Rectangle:
					ObjectValueFormat("{0} {1} {2} {3} re {4}\n",
						ToPt(Rect.Left), ToPt(Rect.Bottom), ToPt(Rect.Right - Rect.Left),
						ToPt(Rect.Top - Rect.Bottom), PaintStr[(int)PP]);
					return;

				case PdfFileWriter.DrawShape.RoundedRect:
						{
						double OriginX = Rect.Left;
						double OriginY = Rect.Bottom;
						double Width = Rect.Right - OriginX;
						double Height = Rect.Top - OriginY;

						// make sure radius is not too big
						double Radius = DrawCtrl.Radius;
						double RadiusMax = 0.5 * Math.Min(Width, Height);
						if (Radius <= 0) Radius = 0.5 * RadiusMax;
						else if (Radius > RadiusMax) Radius = RadiusMax;

						// draw path and set paint operator
						MoveTo(new PointD(OriginX + Radius, OriginY));
						DrawBezier(BezierD.CircleFourthQuarter(OriginX + Width - Radius, OriginY + Radius, Radius), BezierPointOne.LineTo);
						DrawBezier(BezierD.CircleFirstQuarter(OriginX + Width - Radius, OriginY + Height - Radius, Radius), BezierPointOne.LineTo);
						DrawBezier(BezierD.CircleSecondQuarter(OriginX + Radius, OriginY + Height - Radius, Radius), BezierPointOne.LineTo);
						DrawBezier(BezierD.CircleThirdQuarter(OriginX + Radius, OriginY + Radius, Radius), BezierPointOne.LineTo);
						SetPaintOp(PP);
						}
					return;

				case PdfFileWriter.DrawShape.InvRoundedRect:
						{
						double OriginX = Rect.Left;
						double OriginY = Rect.Bottom;
						double Width = Rect.Right - OriginX;
						double Height = Rect.Top - OriginY;

						// make sure radius is not too big
						double Radius = DrawCtrl.Radius;
						double RadiusMax = 0.5 * Math.Min(Width, Height);
						if (Radius <= 0) Radius = 0.5 * RadiusMax;
						else if (Radius > RadiusMax) Radius = RadiusMax;

						// draw path
						MoveTo(new PointD(OriginX, OriginY + Radius));
						DrawBezier(BezierD.CircleFourthQuarter(OriginX, OriginY + Height, Radius), BezierPointOne.LineTo);
						DrawBezier(BezierD.CircleThirdQuarter(OriginX + Width, OriginY + Height, Radius), BezierPointOne.LineTo);
						DrawBezier(BezierD.CircleSecondQuarter(OriginX + Width, OriginY, Radius), BezierPointOne.LineTo);
						DrawBezier(BezierD.CircleFirstQuarter(OriginX, OriginY, Radius), BezierPointOne.LineTo);
						SetPaintOp(PP);
						}
					return;

				case PdfFileWriter.DrawShape.Oval:
						{
						double Width = 0.5 * (Rect.Right - Rect.Left);
						double Height = 0.5 * (Rect.Top - Rect.Bottom);
						double OriginX = Rect.Left + Width;
						double OriginY = Rect.Bottom + Height;
						DrawBezier(BezierD.OvalFirstQuarter(OriginX, OriginY, Width, Height), BezierPointOne.MoveTo);
						DrawBezier(BezierD.OvalSecondQuarter(OriginX, OriginY, Width, Height), BezierPointOne.Ignore);
						DrawBezier(BezierD.OvalThirdQuarter(OriginX, OriginY, Width, Height), BezierPointOne.Ignore);
						DrawBezier(BezierD.OvalFourthQuarter(OriginX, OriginY, Width, Height), BezierPointOne.Ignore);
						SetPaintOp(PP);
						}
					return;
				}
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Draw one line of text
		////////////////////////////////////////////////////////////////////
		private double DrawTextNormal
				(
				PdfDrawTextCtrl TextCtrl,
				double PosX,
				double PosY,
				string Text
				)
			{
			// text is null or empty
			if (string.IsNullOrEmpty(Text)) return 0;

			// add font code to current list of font codes
			AddToUsedResources(TextCtrl.Font);

			// adjust position
			switch (TextCtrl.Justify)
				{
				// right
				case TextJustify.Right:
					PosX -= TextCtrl.TextWidth(Text);
					break;

				// center
				case TextJustify.Center:
					PosX -= 0.5 * TextCtrl.TextWidth(Text);
					break;
				}

			// draw text
			BeginTextMode();
			SetTextPosition(PosX, PosY);
			double Width = DrawTextNoPos(TextCtrl, Text);
			EndTextMode();

			// return text width
			return Width;
			}

		////////////////////////////////////////////////////////////////////
		// Draw text within text box left justified
		////////////////////////////////////////////////////////////////////
		private double DrawText
				(
				double PosX,
				double PosY,
				PdfTextBoxLine Line,
				PdfPage Page
				)
			{
			double SegPosX = PosX;
			foreach(PdfTextBoxSeg Seg in Line.SegArray)
				{
				if(Seg.Annotation != null && Page != null) Seg.Annotation.AnnotPage = Page;
				double SegWidth = DrawText(Seg, SegPosX, PosY, Seg.Text);
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
				PdfTextBoxLine Line,
				PdfPage Page
				)
			{
			// line width
			double LineWidth = 0;
			foreach (PdfTextBoxSeg Seg in Line.SegArray) LineWidth += Seg.SegWidth;

			// x position
			double SegPosX = PosX;
			if (Justify == TextBoxJustify.Right) SegPosX += Width - LineWidth;
			else SegPosX += 0.5 * (Width - LineWidth);

			foreach (PdfTextBoxSeg Seg in Line.SegArray)
				{
				if (Seg.Annotation != null && Page != null) Seg.Annotation.AnnotPage = Page;
				double SegWidth = DrawText(Seg, SegPosX, PosY, Seg.Text);
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
				PdfTextBoxLine Line,
				PdfPage Page
				)
			{
			if (!TextFitToWidth(Width, out double WordSpacing, out double CharSpacing, Line))
				return DrawText(PosX, PosY, Line, Page);

			SaveGraphicsState();
			SetWordSpacing(WordSpacing);
			SetCharacterSpacing(CharSpacing);

			double SegPosX = PosX;
			foreach (PdfTextBoxSeg Seg in Line.SegArray)
				{
				double SegWidth = DrawText(Seg, SegPosX, PosY, Seg.Text) + Seg.SpaceCount * WordSpacing + Seg.Text.Length * CharSpacing;
				if (Seg.Annotation != null)
					{
//					if (Page == null) throw new ApplicationException("TextBox with WebLink. You must call DrawText with PdfPage");
					if(Page != null) Seg.Annotation.AnnotPage = Page;
					Seg.Annotation.AnnotRect = new PdfRectangle(SegPosX, PosY - Seg.TextDescent,
						SegPosX + SegWidth, PosY + Seg.TextAscent);
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
				PdfTextBoxLine Line
				)
			{
			WordSpacing = 0;
			CharSpacing = 0;

			int CharCount = 0;
			double Width = 0;
			int SpaceCount = 0;
			double SpaceWidth = 0;
			foreach (PdfTextBoxSeg Seg in Line.SegArray)
				{
				// accumulate line width
				CharCount += Seg.Text.Length;
				Width += Seg.SegWidth;

				// count spaces
				SpaceCount += Seg.SpaceCount;

				// accumulate space width
				SpaceWidth += Seg.SpaceCount * Seg.CharWidth(' ');
				}

			// reduce character count by one
			CharCount--;
			if (CharCount <= 0) return false;

			// extra spacing required
			double ExtraSpace = ReqWidth - Width;

			// highest possible output device resolution (12000 dots per inch)
			double MaxRes = 0.006 / ScaleFactor;

			// string is too wide
			if (ExtraSpace < (-MaxRes)) return false;

			// string is just right
			if (ExtraSpace < MaxRes) return true;

			// String does not have any blank characters
			if (SpaceCount == 0)
				{
				CharSpacing = ExtraSpace / CharCount;
				return true;
				}

			// extra space per word
			WordSpacing = ExtraSpace / SpaceCount;

			// extra space is equal or less than one blank
			if (WordSpacing <= SpaceWidth / SpaceCount) return true;

			// extra space is larger that one blank
			// increase character and word spacing
			CharSpacing = ExtraSpace / (10 * SpaceCount + CharCount);
			WordSpacing = 10 * CharSpacing;
			return true;
			}

		////////////////////////////////////////////////////////////////////
		// Draw barcode text
		////////////////////////////////////////////////////////////////////
		private void DrawBarcodeText
				(
				PdfDrawTextCtrl TextCtrl,
				double CenterPos,
				double TopPos,
				string Text
				)
			{
			// test for non printable characters
			int Index;
			for (Index = 0; Index < Text.Length && Text[Index] >= ' ' && Text[Index] <= '~'; Index++) ;
			if (Index < Text.Length)
				{
				StringBuilder Str = new StringBuilder(Text);
				for (; Index < Text.Length; Index++) if (Str[Index] < ' ' || Str[Index] > '~') Str[Index] = ' ';
				Text = Str.ToString();
				}

			// draw the text
			DrawText(TextCtrl, CenterPos, TopPos - TextCtrl.TextAscent, Text);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Add resource to list of used resources
		////////////////////////////////////////////////////////////////////
		internal void AddToUsedResources
				(
				PdfObject ResObject
				)
			{
			if (ResObjects == null) ResObjects = new List<PdfObject>();
			int Index = ResObjects.BinarySearch(ResObject);
			if (Index < 0) ResObjects.Insert(~Index, ResObject);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// close object before writing to PDF file
		////////////////////////////////////////////////////////////////////
		internal override void CloseObject()
			{
			// build resource dictionary for non page contents
			if(!PageContents) Dictionary.Add("/Resources", BuildResourcesDictionary(ResObjects));

			// exit
			return;
			}
		}
	}
