/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfXObject
//	PDF X Object resource class.
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

namespace PdfFileWriter
{
/// <summary>
/// PDF X object resource class
/// </summary>
public class PdfXObject : PdfContents
	{
	/// <summary>
	/// Bounding box rectangle
	/// </summary>
	public PdfRectangle Rect
		{
		get
			{
			return new PdfRectangle(BBox);
			}
		set
			{
			BBox = value;
			Dictionary.AddRectangle("/BBox", BBox);
			}
		}

	/// <summary>
	/// Bounding box left side
	/// </summary>
	public double Left
		{
		get
			{
			return BBox.Left;
			}
		set
			{
			BBox.Left = value;
			Dictionary.AddRectangle("/BBox", BBox);
			}
		}

	/// <summary>
	/// Bounding box bottom side
	/// </summary>
	public double Bottom
		{
		get
			{
			return BBox.Bottom;
			}
		set
			{
			BBox.Bottom = value;
			Dictionary.AddRectangle("/BBox", BBox);
			}
		}

	/// <summary>
	/// Bounding box right side
	/// </summary>
	public double Right
		{
		get
			{
			return BBox.Right;
			}
		set
			{
			BBox.Right = value;
			Dictionary.AddRectangle("/BBox", BBox);
			}
		}

	/// <summary>
	/// Bounding box top side
	/// </summary>
	public double Top
		{
		get
			{
			return BBox.Top;
			}
		set
			{
			BBox.Top = value;
			Dictionary.AddRectangle("/BBox", BBox);
			}
		}

	// bounding rectangle
	internal PdfRectangle BBox;

	/// <summary>
	/// PDF X Object constructor
	/// </summary>
	/// <param name="Document">PDF document</param>
	/// <param name="Width">X Object width</param>
	/// <param name="Height">X Object height</param>
	public PdfXObject
			(
			PdfDocument		Document,
			double			Width = 1.0,
			double			Height = 1.0
			) : base(Document, "/XObject")
		{
		// create resource code
		ResourceCode = Document.GenerateResourceNumber('X');

		// add subtype to dictionary
		Dictionary.Add("/Subtype", "/Form");

		// set boundig box rectangle
		BBox = new PdfRectangle(0.0, 0.0, Width, Height);

		// bounding box
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
