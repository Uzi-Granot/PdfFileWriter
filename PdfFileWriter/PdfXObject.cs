/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PdfXObject
//	PDF X Object resource class.
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
	/// PDF X object resource class
	/// </summary>
	public class PdfXObject : PdfContents
		{
		// bounding rectangle
		/// <summary>
		/// XObject bounding rectangle
		/// </summary>
		public PdfRectangle BBox { get;}

		/// <summary>
		/// PDF X Object constructor
		/// </summary>
		/// <param name="Document">PDF document</param>
		/// <param name="Width">X Object width</param>
		/// <param name="Height">X Object height</param>
		public PdfXObject
				(
				PdfDocument Document,
				double Width = 1.0,
				double Height = 1.0
				) : base(Document, "/XObject")
			{
			// create resource code
			ResourceCode = Document.GenerateResourceNumber('X');

			// add subtype to dictionary
			Dictionary.Add("/Subtype", "/Form");
//			Dictionary.Add("/FormType", "1");
//			Dictionary.Add("/Matrix", "[1 0 0 1 0 0]");

			// set boundig box rectangle
			BBox = new PdfRectangle(0, 0, Width, Height);
			Dictionary.AddRectangle("/BBox", BBox);
			return;
			}

		/// <summary>
		/// Layer control
		/// </summary>
		/// <param name="Layer">PdfLayer object</param>
		public void LayerControl
				(
				PdfObject Layer
				)
			{
			Dictionary.AddIndirectReference("/OC", Layer);
			return;
			}
		}
	}
