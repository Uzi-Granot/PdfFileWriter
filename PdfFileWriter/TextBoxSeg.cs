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

using System.Drawing;

namespace PdfFileWriter
	{
	/// <summary>
	/// TextBox line segment class
	/// </summary>
	public class TextBoxSeg
		{
		/// <summary>
		/// Gets segment font.
		/// </summary>
		public PdfFont Font { get; internal set; }

		/// <summary>
		/// Gets segment font size.
		/// </summary>
		public double FontSize { get; internal set; }

		/// <summary>
		/// Gets segment drawing style.
		/// </summary>
		public DrawStyle DrawStyle { get; internal set; }

		/// <summary>
		/// Gets segment color.
		/// </summary>
		public Color FontColor { get; internal set; }

		/// <summary>
		/// Gets segment width.
		/// </summary>
		public double SegWidth { get; internal set; }

		/// <summary>
		/// Gets segment space character count.
		/// </summary>
		public int SpaceCount { get; internal set; }

		/// <summary>
		/// Gets segment text.
		/// </summary>
		public string Text { get; internal set; }

		/// <summary>
		/// Gets annotation action
		/// </summary>
		public AnnotAction AnnotAction { get; internal set; }

		/// <summary>
		/// TextBox segment constructor.
		/// </summary>
		/// <param name="Font">Segment font.</param>
		/// <param name="FontSize">Segment font size.</param>
		/// <param name="DrawStyle">Segment drawing style.</param>
		/// <param name="FontColor">Segment color.</param>
		/// <param name="AnnotAction">Segment annotation action.</param>
		public TextBoxSeg
				(
				PdfFont Font,
				double FontSize,
				DrawStyle DrawStyle,
				Color FontColor,
				AnnotAction AnnotAction
				)
			{
			this.Font = Font;
			this.FontSize = FontSize;
			this.DrawStyle = DrawStyle;
			this.FontColor = FontColor;
			Text = string.Empty;
			this.AnnotAction = AnnotAction;
			return;
			}

		/// <summary>
		/// TextBox segment copy constructor.
		/// </summary>
		/// <param name="CopySeg">Source TextBox segment.</param>
		internal TextBoxSeg
				(
				TextBoxSeg CopySeg
				)
			{
			Font = CopySeg.Font;
			FontSize = CopySeg.FontSize;
			DrawStyle = CopySeg.DrawStyle;
			FontColor = CopySeg.FontColor;
			Text = string.Empty;
			AnnotAction = CopySeg.AnnotAction;
			return;
			}

		/// <summary>
		/// Compare two TextBox segments.
		/// </summary>
		/// <param name="Font">Segment font.</param>
		/// <param name="FontSize">Segment font size.</param>
		/// <param name="DrawStyle">Segment drawing style.</param>
		/// <param name="FontColor">Segment color.</param>
		/// <param name="AnnotAction">Segment annotation action.</param>
		/// <returns>Result</returns>
		internal bool IsEqual
				(
				PdfFont Font,
				double FontSize,
				DrawStyle DrawStyle,
				Color FontColor,
				AnnotAction AnnotAction
				)
			{
			// test all but annotation action
			return this.Font == Font && this.FontSize == FontSize && this.DrawStyle == DrawStyle &&
				this.FontColor == FontColor && AnnotAction.IsEqual(this.AnnotAction, AnnotAction);
			}
		}

	}
