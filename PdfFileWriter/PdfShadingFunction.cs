/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfShadingFunction
//	Support class for both axial and radial shading resources.
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

namespace PdfFileWriter
{
////////////////////////////////////////////////////////////////////
/// <summary>
/// PDF shading function class
/// </summary>
/// <remarks>
/// PDF function to convert a number between 0 and 1 into a
/// color red green and blue based on the sample color array.
/// </remarks>
////////////////////////////////////////////////////////////////////
public class PdfShadingFunction : PdfObject
	{
	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// PDF Shading function constructor
	/// </summary>
	/// <param name="Document">Document object parent of this function.</param>
	/// <param name="ColorArray">Array of colors.</param>
	////////////////////////////////////////////////////////////////////
	public PdfShadingFunction
			(
			PdfDocument		Document,		// PDF document object
			Color[]			ColorArray		// Array of colors. Minimum 2.
			) : base(Document, ObjectType.Stream)
		{
		// build dictionary
		Constructorhelper(ColorArray.Length);

		// add color array to contents stream
		foreach(Color Color in ColorArray)
			{
			ObjectValueList.Add(Color.R);	// red
			ObjectValueList.Add(Color.G);	// green
			ObjectValueList.Add(Color.B);	// blue
			}
		return;
		}

	/// <summary>
	/// PDF Shading function constructor
	/// </summary>
	/// <param name="Document">Document object parent of this function.</param>
	/// <param name="Brush">System.Windows.Media gradient brush</param>
	public PdfShadingFunction
			(
			PdfDocument Document,
			SysMedia.GradientBrush Brush
			) : base(Document, ObjectType.Stream)
		{
		// build dictionary
		Constructorhelper(Brush.GradientStops.Count);

		// add color array to contents stream
		foreach(System.Windows.Media.GradientStop Stop in Brush.GradientStops)
			{
			ObjectValueList.Add(Stop.Color.R);	// red
			ObjectValueList.Add(Stop.Color.G);	// green
			ObjectValueList.Add(Stop.Color.B);	// blue
			}
		return;
		}

	private void Constructorhelper
			(
			int	Length
			)
		{
		// test for error
		if(Length < 2) throw new ApplicationException("Shading function color array must have two or more items");

		// the shading function is a sampled function
		Dictionary.Add("/FunctionType", "0");

		// input variable is between 0 and 1
		Dictionary.Add("/Domain", "[0 1]");

		// output variables are red, green and blue color components between 0 and 1
		Dictionary.Add("/Range", "[0 1 0 1 0 1]");

		// each color components in the stream is 8 bits
		Dictionary.Add("/BitsPerSample", "8");

		// number of colors in the stream must be two or more
		Dictionary.AddFormat("/Size", "[{0}]", Length);
		return;
		}
	}
}
