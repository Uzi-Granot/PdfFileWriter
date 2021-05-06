/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfRadialShading
//	PDF radial shading resource class.
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
using SysMedia = System.Windows.Media;

namespace PdfFileWriter
	{
	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// PDF radial shading resource class
	/// </summary>
	/// <remarks>
	/// Derived class from PdfObject
	/// </remarks>
	public class PdfRadialShading : PdfObject
		{
		private double BBoxLeft;
		private double BBoxBottom;
		private double BBoxRight;
		private double BBoxTop;

		private MappingMode Mapping;
		private double StartCenterX;
		private double StartCenterY;
		private double StartRadius;
		private double EndCenterX;
		private double EndCenterY;
		private double EndRadius;

		private bool ExtendShadingBefore = true;
		private bool ExtendShadingAfter = true;

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// PDF radial shading constructor.
		/// </summary>
		/// <param name="Document">Parent PDF document object</param>
		/// <param name="BBoxLeft">Bounding box left position</param>
		/// <param name="BBoxBottom">Bounding box bottom position</param>
		/// <param name="BBoxWidth">Bounding box width</param>
		/// <param name="BBoxHeight">Bounding box height</param>
		/// <param name="ShadingFunction">Shading function</param>
		////////////////////////////////////////////////////////////////////
		public PdfRadialShading
				(
				PdfDocument Document,
				double BBoxLeft,
				double BBoxBottom,
				double BBoxWidth,
				double BBoxHeight,
				PdfShadingFunction ShadingFunction
				) : base(Document)
			{
			// create resource code
			ResourceCode = Document.GenerateResourceNumber('S');

			// color space red, green and blue
			Dictionary.Add("/ColorSpace", "/DeviceRGB");

			// shading type axial
			Dictionary.Add("/ShadingType", "3");

			// add shading function to shading dictionary
			Dictionary.AddIndirectReference("/Function", ShadingFunction);

			// bounding box
			this.BBoxLeft = BBoxLeft;
			this.BBoxBottom = BBoxBottom;
			this.BBoxRight = BBoxLeft + BBoxWidth;
			this.BBoxTop = BBoxBottom + BBoxHeight;

			// assume the direction of color change is along x axis
			this.Mapping = MappingMode.Relative;
			this.StartCenterX = 0.5;
			this.StartCenterY = 0.5;
			this.StartRadius = 0.0;
			this.EndCenterX = 0.5;
			this.EndCenterY = 0.5;
			this.EndRadius = Math.Sqrt(0.5);
			return;
			}

		/// <summary>
		/// PDF radial shading constructor for one unit bounding box
		/// </summary>
		/// <param name="Document">Parent PDF document object.</param>
		/// <param name="ShadingFunction">Shading function.</param>
		public PdfRadialShading
				(
				PdfDocument Document,
				PdfShadingFunction ShadingFunction
				) : this(Document, 0.0, 0.0, 1.0, 1.0, ShadingFunction) {}

		/// <summary>
		/// PDF radial shading constructor for one unit bounding box
		/// </summary>
		/// <param name="Document">Parent PDF document object.</param>
		/// <param name="MediaBrush">System.Windows.Media brush</param>
		/// <remarks>Support for WPF media</remarks>
		public PdfRadialShading
				(
				PdfDocument Document,
				SysMedia.RadialGradientBrush MediaBrush
				) : this(Document, 0.0, 0.0, 1.0, 1.0, new PdfShadingFunction(Document, MediaBrush))
			{
			SetGradientDirection(MediaBrush.Center.X, MediaBrush.Center.Y, 0.0,
				MediaBrush.GradientOrigin.X, MediaBrush.GradientOrigin.Y, 0.5 * (MediaBrush.RadiusX + MediaBrush.RadiusY),
				MediaBrush.MappingMode == SysMedia.BrushMappingMode.RelativeToBoundingBox ? MappingMode.Relative : MappingMode.Absolute);
			return;
			}

		/// <summary>
		/// Set bounding box
		/// </summary>
		/// <param name="BBoxLeft">Bounding box left</param>
		/// <param name="BBoxBottom">Bounding box bottom</param>
		/// <param name="BBoxWidth">Bounding box width</param>
		/// <param name="BBoxHeight">Bounding box height</param>
		public void SetBoundingBox
				(
				double BBoxLeft,
				double BBoxBottom,
				double BBoxWidth,
				double BBoxHeight
				)
			{
			// bounding box
			this.BBoxLeft = BBoxLeft;
			this.BBoxBottom = BBoxBottom;
			this.BBoxRight = BBoxLeft + BBoxWidth;
			this.BBoxTop = BBoxBottom + BBoxHeight;
			return;
			}

		/// <summary>
		/// Set gradient direction
		/// </summary>
		/// <param name="StartCenterX">Start circle center x position</param>
		/// <param name="StartCenterY">Start circle center y position</param>
		/// <param name="StartRadius">Start circle center radius</param>
		/// <param name="EndCenterX">End circle center x position</param>
		/// <param name="EndCenterY">End circle center y position</param>
		/// <param name="EndRadius">End circle center radius</param>
		/// <param name="Mapping">Mapping mode (relative absolute)</param>
		public void SetGradientDirection
				(
				double StartCenterX,
				double StartCenterY,
				double StartRadius,
				double EndCenterX,
				double EndCenterY,
				double EndRadius,
				MappingMode Mapping
				)
			{
			this.StartCenterX = StartCenterX;
			this.StartCenterY = StartCenterY;
			this.StartRadius = StartRadius;
			this.EndCenterX = EndCenterX;
			this.EndCenterY = EndCenterY;
			this.EndRadius = EndRadius;
			this.Mapping = Mapping;
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Sets anti-alias parameter
		/// </summary>
		/// <param name="Value">Anti-alias true or false</param>
		////////////////////////////////////////////////////////////////////
		public void AntiAlias
				(
				bool Value
				)
			{
			Dictionary.AddBoolean("/AntiAlias", Value);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Extend shading beyond axis
		/// </summary>
		/// <param name="Before">Before (true or false)</param>
		/// <param name="After">After (true or false)</param>
		////////////////////////////////////////////////////////////////////
		public void ExtendShading
				(
				bool Before,
				bool After
				)
			{
			this.ExtendShadingBefore = Before;
			this.ExtendShadingAfter = After;
			return;
			}

		////////////////////////////////////////////////////////////////////
		// close object before writing to PDF file
		////////////////////////////////////////////////////////////////////
		internal override void CloseObject()
			{
			// bounding box
			Dictionary.AddRectangle("/BBox", BBoxLeft, BBoxBottom, BBoxRight, BBoxTop);

			// absolute mapping mode
			if(Mapping == MappingMode.Absolute)
				{
				Dictionary.AddFormat("/Coords", "[{0} {1} {2} {3} {4} {5}]",
					ToPt(StartCenterX), ToPt(StartCenterY), ToPt(StartRadius), ToPt(EndCenterX), ToPt(EndCenterY), ToPt(EndRadius));
				}
			// relative mapping mode
			else
				{
				double RelStartCenterX = BBoxLeft * (1.0 - StartCenterX) + BBoxRight * StartCenterX;
				double RelStartCenterY = BBoxBottom * (1.0 - StartCenterY) + BBoxTop * StartCenterY;
				double BBoxSide = Math.Min(Math.Abs(BBoxRight - BBoxLeft), Math.Abs(BBoxTop - BBoxBottom));
				double RelStartRadius = BBoxSide * StartRadius;
				double RelEndCenterX = BBoxLeft * (1.0 - EndCenterX) + BBoxRight * EndCenterX;
				double RelEndCenterY = BBoxBottom * (1.0 - EndCenterY) + BBoxTop * EndCenterY;
				double RelEndRadius = BBoxSide * EndRadius;
				Dictionary.AddFormat("/Coords", "[{0} {1} {2} {3} {4} {5}]",
					ToPt(RelStartCenterX), ToPt(RelStartCenterY), ToPt(RelStartRadius), ToPt(RelEndCenterX), ToPt(RelEndCenterY), ToPt(RelEndRadius));
				}

			// extend shading
			Dictionary.AddFormat("/Extend", "[{0} {1}]", ExtendShadingBefore ? "true" : "false", ExtendShadingAfter ? "true" : "false");

			// exit
			return;
			}
		}
	}
