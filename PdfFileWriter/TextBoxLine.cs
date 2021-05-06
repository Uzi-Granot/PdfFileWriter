/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	TextBox
//  Support class for PdfContents class. Format text to fit column.
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

namespace PdfFileWriter
	{
	/// <summary>
	/// TextBoxLine class
	/// </summary>
	public class TextBoxLine
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
		public TextBoxSeg[] SegArray { get; internal set; }

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
		public TextBoxLine
				(
				double Ascent,
				double Descent,
				bool EndOfParagraph,
				TextBoxSeg[] SegArray
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
