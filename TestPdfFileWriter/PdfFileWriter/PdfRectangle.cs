/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PdfRectangle
//	PDF rectangle class. 
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
		public double Left { get; set; }

		/// <summary>
		/// Gets or sets bottom side.
		/// </summary>
		public double Bottom { get; set; }

		/// <summary>
		/// Gets or sets right side.
		/// </summary>
		public double Right { get; set; }

		/// <summary>
		/// Gets or sets top side.
		/// </summary>
		public double Top { get; set; }

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
				double Left,
				double Bottom,
				double Right,
				double Top
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
			Left = Rect.Left;
			Bottom = Rect.Bottom;
			Right = Rect.Right;
			Top = Rect.Top;
			return;
			}

		/// <summary>
		/// Constructor for margin
		/// </summary>
		/// <param name="AllTheSame">Single value for all sides</param>
		public PdfRectangle
				(
				double AllTheSame
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
				double Hor,
				double Vert
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
				return Right - Left;
				}
			}

		/// <summary>
		/// Gets height
		/// </summary>
		public double Height
			{
			get
				{
				return Top - Bottom;
				}
			}

		/// <summary>
		/// Move rectangle
		/// </summary>
		/// <param name="DeltaX">Delta X displacement</param>
		/// <param name="DeltaY">Delta Y displacement</param>
		/// <returns>New rectangle</returns>
		public PdfRectangle Move
				(
				double DeltaX,
				double DeltaY
				)
			{
			return new PdfRectangle(Left + DeltaX, Bottom + DeltaY, Right + DeltaX, Top + DeltaY);
			}

		/// <summary>
		/// Add margin to all sides
		/// </summary>
		/// <param name="Margin">Margin</param>
		/// <returns>Rew rectangle</returns>
		public PdfRectangle AddMargin
				(
				double Margin
				)
			{
			return new PdfRectangle(Left - Margin, Bottom - Margin, Right + Margin, Top + Margin);
			}
		}
	}
