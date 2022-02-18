/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PdfRadialShading
//	PDF radial shading resource class.
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
	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// PDF radial shading resource class
	/// </summary>
	/// <remarks>
	/// Derived class from PdfObject
	/// </remarks>
	public class PdfRadialShading : PdfObject
		{
		/// <summary>
		/// Bounding box
		/// </summary>
		public PdfRectangle BBox { get; set;}

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
		/// <param name="ShadingFunction">Shading function</param>
		////////////////////////////////////////////////////////////////////
		public PdfRadialShading
				(
				PdfDocument Document,
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

			// set boundig box to unit matrix
			BBox = new PdfRectangle(0, 0, 1, 1);

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
			Dictionary.AddRectangle("/BBox", BBox);

			// absolute mapping mode
			if(Mapping == MappingMode.Absolute)
				{
				Dictionary.AddFormat("/Coords", "[{0} {1} {2} {3} {4} {5}]",
					ToPt(StartCenterX), ToPt(StartCenterY), ToPt(StartRadius), ToPt(EndCenterX), ToPt(EndCenterY), ToPt(EndRadius));
				}

			// relative mapping mode
			else
				{
				double RelStartCenterX = BBox.Left * (1.0 - StartCenterX) + BBox.Right * StartCenterX;
				double RelStartCenterY = BBox.Bottom * (1.0 - StartCenterY) + BBox.Top * StartCenterY;
				double BBoxSide = Math.Min(Math.Abs(BBox.Right - BBox.Left), Math.Abs(BBox.Top - BBox.Bottom));
				double RelStartRadius = BBoxSide * StartRadius;
				double RelEndCenterX = BBox.Left * (1.0 - EndCenterX) + BBox.Right * EndCenterX;
				double RelEndCenterY = BBox.Bottom * (1.0 - EndCenterY) + BBox.Top * EndCenterY;
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
