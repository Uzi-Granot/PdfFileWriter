/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PdfShadingFunction
//	Support class for both axial and radial shading resources.
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
				PdfDocument Document,   // PDF document object
				Color[] ColorArray      // Array of colors. Minimum 2.
				) : base(Document, ObjectType.Stream)
			{
			// build dictionary
			Constructorhelper(ColorArray.Length);

			// add color array to contents stream
			foreach(Color Color in ColorArray)
				{
				ObjectValueList.Add(Color.R);   // red
				ObjectValueList.Add(Color.G);   // green
				ObjectValueList.Add(Color.B);   // blue
				}
			return;
			}

		private void Constructorhelper
				(
				int Length
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
