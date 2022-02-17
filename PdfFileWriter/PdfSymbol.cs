/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PdfSymbol
//	Convert character font to series of lines and Bezier courves 
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

using System.Drawing.Drawing2D;

namespace PdfFileWriter
	{
	internal enum SymbolPointType
		{ 
		Start = 0, // The starting point of a GraphicsPath object.
		Line = 1, // A line segment.
		Bezier = 3, // Bézier curve.
		PathTypeMask = 7, // A mask point.	
		DashMode = 16, // The corresponding segment is dashed.
		PathMarker = 32, // A path marker.
		CloseSubpath = 128, // The endpoint of a subpath.
		}

	/// <summary>
	/// PdfSymbol class
	/// </summary>
	public class PdfSymbol
		{
		internal int Len;
		internal RectangleF Bounds;
		internal PointF[] Points;
		internal byte[] Types;

		/// <summary>
		/// PdfSymbol class constructor
		/// </summary>
		/// <param name="FontFamilyName">Font family name</param>
		/// <param name="Style">Font style</param>
		/// <param name="CharCode">Character code</param>
		public PdfSymbol
				(
				string FontFamilyName,
				FontStyle Style,
				int CharCode
				)
			{
			// convert character to graphics path
			GraphicsPath GP = new GraphicsPath();
			GP.AddString(((char) CharCode).ToString(), new FontFamily(FontFamilyName), (int) Style, 1000, Point.Empty, StringFormat.GenericDefault);
			Bounds = GP.GetBounds();

			// number of points
			Len = GP.PointCount;

			// points coordinats
			Points = GP.PathPoints;
			Types = GP.PathTypes;
			return;
			}
		}
	}