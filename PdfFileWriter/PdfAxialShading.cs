/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfAxialShading
//	PDF Axial shading indirect object.
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
	/// <summary>
	/// Mapping mode for axial and radial shading
	/// </summary>
	public enum MappingMode
		{
		/// <summary>
		/// Relative to bounding box
		/// </summary>
		Relative,
		/// <summary>
		/// Absolute
		/// </summary>
		Absolute
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// PDF axial shading resource class
	/// </summary>
	/// <remarks>
	/// Derived class from PdfObject
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public class PdfAxialShading : PdfObject
		{
		private double BBoxLeft;
		private double BBoxBottom;
		private double BBoxRight;
		private double BBoxTop;

		private MappingMode Mapping;
		private double StartPointX;
		private double StartPointY;
		private double EndPointX;
		private double EndPointY;

		private bool ExtendShadingBefore = true;
		private bool ExtendShadingAfter = true;

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// PDF axial shading constructor.
		/// </summary>
		/// <param name="Document">Parent PDF document object</param>
		/// <param name="BBoxLeft">Bounding box left position</param>
		/// <param name="BBoxBottom">Bounding box bottom position</param>
		/// <param name="BBoxWidth">Bounding box width</param>
		/// <param name="BBoxHeight">Bounding box height</param>
		/// <param name="ShadingFunction">Shading function</param>
		////////////////////////////////////////////////////////////////////
		public PdfAxialShading
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
			Dictionary.Add("/ShadingType", "2");

			// add shading function to shading dictionary
			Dictionary.AddIndirectReference("/Function", ShadingFunction);

			// bounding box
			this.BBoxLeft = BBoxLeft;
			this.BBoxBottom = BBoxBottom;
			this.BBoxRight = BBoxLeft + BBoxWidth;
			this.BBoxTop = BBoxBottom + BBoxHeight;

			// assume the direction of color change is along x axis
			Mapping = MappingMode.Relative;
			StartPointX = 0.0;
			StartPointY = 0.0;
			EndPointX = 1.0;
			EndPointY = 0.0;
			return;
			}

		/// <summary>
		/// PDF axial shading constructor for unit bounding box
		/// </summary>
		/// <param name="Document">Parent PDF document object</param>
		/// <param name="ShadingFunction">Shading function</param>
		public PdfAxialShading
				(
				PdfDocument Document,
				PdfShadingFunction ShadingFunction
				) : this(Document, 0.0, 0.0, 1.0, 1.0, ShadingFunction) {}

		/// <summary>
		/// PDF axial shading constructor for unit bounding box
		/// </summary>
		/// <param name="Document">Parent PDF document object</param>
		/// <param name="MediaBrush">System.Windows.Media brush</param>
		public PdfAxialShading
				(
				PdfDocument Document,
				SysMedia.LinearGradientBrush MediaBrush
				) : this(Document, 0.0, 0.0, 1.0, 1.0, new PdfShadingFunction(Document, MediaBrush))
			{
			SetAxisDirection(MediaBrush.StartPoint.X, MediaBrush.StartPoint.Y, MediaBrush.EndPoint.X, MediaBrush.EndPoint.Y,
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
		/// Set gradient axis direction
		/// </summary>
		/// <param name="StartPointX">Start point x</param>
		/// <param name="StartPointY">Start point y</param>
		/// <param name="EndPointX">End point x</param>
		/// <param name="EndPointY">End point y</param>
		/// <param name="Mapping">Mapping mode (Relative or Absolute)</param>
		public void SetAxisDirection
				(
				double StartPointX,
				double StartPointY,
				double EndPointX,
				double EndPointY,
				MappingMode Mapping
				)
			{
			this.StartPointX = StartPointX;
			this.StartPointY = StartPointY;
			this.EndPointX = EndPointX;
			this.EndPointY = EndPointY;
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
			ExtendShadingBefore = Before;
			ExtendShadingAfter = After;
			return;
			}

		////////////////////////////////////////////////////////////////////
		// close object before writing to PDF file
		////////////////////////////////////////////////////////////////////
		internal override void CloseObject()
			{
			// bounding box
			Dictionary.AddRectangle("/BBox", BBoxLeft, BBoxBottom, BBoxRight, BBoxTop);

			// absolute axis direction
			if(Mapping == MappingMode.Absolute)
				{
				Dictionary.AddRectangle("/Coords", StartPointX, StartPointY, EndPointX, EndPointY);
				}

			// relative axit direction
			else
				{
				double RelStartPointX = BBoxLeft * (1.0 - StartPointX) + BBoxRight * StartPointX;
				double RelStartPointY = BBoxBottom * (1.0 - StartPointY) + BBoxTop * StartPointY;
				double RelEndPointX = BBoxLeft * (1.0 - EndPointX) + BBoxRight * EndPointX;
				double RelEndPointY = BBoxBottom * (1.0 - EndPointY) + BBoxTop * EndPointY;
				Dictionary.AddRectangle("/Coords", RelStartPointX, RelStartPointY, RelEndPointX, RelEndPointY);
				}

			// extend shading
			Dictionary.AddFormat("/Extend", "[{0} {1}]", ExtendShadingBefore ? "true" : "false", ExtendShadingAfter ? "true" : "false");

			// exit
			return;
			}
		}
	}
