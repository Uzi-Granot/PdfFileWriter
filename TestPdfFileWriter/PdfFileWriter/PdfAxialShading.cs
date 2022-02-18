/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PdfAxialShading
//	PDF Axial shading indirect object.
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
		/// <summary>
		/// Bounding box rectangle
		/// </summary>
		public PdfRectangle BBox { get; set; }

		/// <summary>
		/// Direction rectangle
		/// </summary>
		public PdfRectangle Direction { get; set;}

		/// <summary>
		/// Mapping mode
		/// </summary>
		public MappingMode Mapping { get;set; }

		private bool ExtendShadingBefore = true;
		private bool ExtendShadingAfter = true;

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// PDF axial shading constructor.
		/// </summary>
		/// <param name="Document">Parent PDF document object</param>
		/// <param name="ShadingFunction">Shading function</param>
		////////////////////////////////////////////////////////////////////
		public PdfAxialShading
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
			Dictionary.Add("/ShadingType", "2");

			// add shading function to shading dictionary
			Dictionary.AddIndirectReference("/Function", ShadingFunction);

			// bounding box
			BBox = new PdfRectangle(0, 0, 1, 1);

			// assume the direction of color change is along x axis
			Direction = new PdfRectangle(0, 0, 1, 0);
			Mapping = MappingMode.Relative;
			return;
			}

/*
		/// <summary>
		/// PDF axial shading constructor for unit bounding box
		/// </summary>
		/// <param name="Document">Parent PDF document object</param>
		/// <param name="MediaBrush">System.Windows.Media brush</param>
		public PdfAxialShading
				(
				PdfDocument Document,
				SysMedia.LinearGradientBrush MediaBrush
				) : this(Document, new PdfShadingFunction(Document, MediaBrush))
			{
			Direction = new PdfRectangle(MediaBrush.StartPoint.X, MediaBrush.StartPoint.Y, MediaBrush.EndPoint.X, MediaBrush.EndPoint.Y);
			Mapping = MediaBrush.MappingMode == SysMedia.BrushMappingMode.RelativeToBoundingBox ? MappingMode.Relative : MappingMode.Absolute;
			return;
			}
*/

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
			Dictionary.AddRectangle("/BBox", BBox);

			// relative axit direction
			if(Mapping == MappingMode.Relative)
				{
				Direction = new PdfRectangle(BBox.Left * (1.0 - Direction.Left) + BBox.Right * Direction.Left,
					BBox.Bottom * (1.0 - Direction.Bottom) + BBox.Top * Direction.Bottom,
					BBox.Left * (1.0 - Direction.Right) + BBox.Right * Direction.Right,
					BBox.Bottom * (1.0 - Direction.Top) + BBox.Top * Direction.Top);
				}

			// direction rectangle
			Dictionary.AddRectangle("/Coords", Direction);

			// extend shading
			Dictionary.AddFormat("/Extend", "[{0} {1}]", ExtendShadingBefore ? "true" : "false", ExtendShadingAfter ? "true" : "false");

			// exit
			return;
			}
		}
	}
