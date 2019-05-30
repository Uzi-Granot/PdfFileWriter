/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfRectangle
//	PDF rectangle class. 
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

namespace PdfFileWriter
{
////////////////////////////////////////////////////////////////////
/// <summary>
/// PDF rectangle in double precision class
/// </summary>
/// <remarks>
/// Note: Microsoft rectangle is left, top, width and height.
/// PDF rectangle is left, bottom, right and top.
/// PDF numeric precision is double and Microsoft is Single.
/// </remarks>
////////////////////////////////////////////////////////////////////
public class PdfRectangle
	{
	/// <summary>
	/// Gets or sets Left side.
	/// </summary>
	public double		Left {get; set;}

	/// <summary>
	/// Gets or sets bottom side.
	/// </summary>
	public double		Bottom {get; set;}

	/// <summary>
	/// Gets or sets right side.
	/// </summary>
	public double		Right {get; set;}

	/// <summary>
	/// Gets or sets top side.
	/// </summary>
	public double		Top {get; set;}

	/// <summary>
	/// Default constructor.
	/// </summary>
	public PdfRectangle() {}

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="Left">Left side</param>
	/// <param name="Bottom">Bottom side</param>
	/// <param name="Right">Right side</param>
	/// <param name="Top">Top side</param>
	public PdfRectangle
			(
			double	Left,
			double	Bottom,
			double	Right,
			double	Top
			)
		{
		this.Left = Left;
		this.Bottom = Bottom;
		this.Right = Right;
		this.Top = Top;
		return;
		}

	/// <summary>
	/// Copy constructor
	/// </summary>
	/// <param name="Rect">Source rectangle</param>
	public PdfRectangle
			(
			PdfRectangle Rect
			)
		{
		this.Left = Rect.Left;
		this.Bottom = Rect.Bottom;
		this.Right = Rect.Right;
		this.Top = Rect.Top;
		return;
		}

	/// <summary>
	/// Constructor for margin
	/// </summary>
	/// <param name="AllTheSame">Single value for all sides</param>
	public PdfRectangle
			(
			double	AllTheSame
			)
		{
		Left = AllTheSame;
		Bottom = AllTheSame;
		Right = AllTheSame;
		Top = AllTheSame;
		return;
		}

	/// <summary>
	/// Constructor for margin
	/// </summary>
	/// <param name="Hor">Left and right value</param>
	/// <param name="Vert">Top and bottom value</param>
	public PdfRectangle
			(
			double	Hor,
			double	Vert
			)
		{
		Left = Hor;
		Bottom = Vert;
		Right = Hor;
		Top = Vert;
		return;
		}

	/// <summary>
	/// Gets width
	/// </summary>
	public double Width
		{
		get
			{
			return(Right - Left);
			}
		}

	/// <summary>
	/// Gets height
	/// </summary>
	public double Height
		{
		get
			{
			return(Top - Bottom);
			}
		}
	}
}
