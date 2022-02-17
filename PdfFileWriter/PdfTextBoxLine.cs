/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	TextBox
//  Support class for PdfContents class. Format text to fit column.
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
	/// TextBoxLine class
	/// </summary>
	public class PdfTextBoxLine
		{
		/// <summary>
		/// Gets line ascent.
		/// </summary>
		public double Ascent { get; internal set; }

		/// <summary>
		/// Gets line descent.
		/// </summary>
		public double Descent { get; internal set; }

		/// <summary>
		/// Line is end of paragraph.
		/// </summary>
		public bool EndOfParagraph { get; internal set; }

		/// <summary>
		/// Gets array of line segments.
		/// </summary>
		public PdfTextBoxSeg[] SegArray { get; internal set; }

		/// <summary>
		/// Gets line height.
		/// </summary>
		public double LineHeight
			{
			get
				{
				return Ascent + Descent;
				}
			}

		/// <summary>
		/// TextBoxLine constructor.
		/// </summary>
		/// <param name="Ascent">Line ascent.</param>
		/// <param name="Descent">Line descent.</param>
		/// <param name="EndOfParagraph">Line is end of paragraph.</param>
		/// <param name="SegArray">Segments' array.</param>
		public PdfTextBoxLine
				(
				double Ascent,
				double Descent,
				bool EndOfParagraph,
				PdfTextBoxSeg[] SegArray
				)
			{
			this.Ascent = Ascent;
			this.Descent = Descent;
			this.EndOfParagraph = EndOfParagraph;
			this.SegArray = SegArray;
			return;
			}
		}
	}
