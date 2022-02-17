/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Draw text control class
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
	/// Draw text control class
	/// </summary>
	public class PdfDrawTextCtrl
		{
		private readonly PdfFont _Font;
		private double _FontSize;
		private string _FontSizeStr;
		private string _FontResourceCode;
		private string _FontResourceGlyph;
		private double _LineSpacing;
		private double _TextAscent;
		private double _TextInternalLead;
		private double _TextExternalLead;

		/// <summary>
		/// Text color
		/// </summary>
		public Color TextColor { get; set; }

		/// <summary>
		/// Draw style (normal, underline, strikeout, subscript, superscript)
		/// </summary>
		public DrawStyle DrawStyle { get; set; }

		/// <summary>
		/// Text justify (left, center, right)
		/// </summary>
		public TextJustify Justify { get; set; }

		/// <summary>
		/// Annotation action associated with this text segment
		/// </summary>
		public PdfAnnotation Annotation
			{
			get
				{
				return _Annotation;
				}
			set
				{
				_Annotation = value;
				if(value != null)
					{ 
					DrawStyle = DrawStyle.Underline;
					TextColor = Color.Blue;
					}
				}
			}
		private PdfAnnotation _Annotation;

		/// <summary>
		/// Get font
		/// </summary>
		public PdfFont Font
			{
			get
				{
				return _Font;
				}
			}

		/// <summary>
		/// Get or set font size in points
		/// (it will set line spacing, ascent and descent)
		/// </summary>
		public double FontSize
			{
			get
				{
				return _FontSize;
				}
			set
				{
				if(Font != null && value > 0)
					{ 
					_FontSize = value;
					_FontSizeStr = string.Format(NFI.PeriodDecSep, "{0}", Font.Round(value));
					_FontResourceCode = string.Format("{0} {1} Tf\n", Font.ResourceCode, _FontSizeStr);
					_FontResourceGlyph = string.Format("{0} {1} Tf\n", Font.ResourceCodeGlyph, _FontSizeStr);
					_LineSpacing = _Font.LineSpacing(value);
					_TextAscent = _Font.Ascent(value);
					_TextInternalLead = _Font.InternalLead(value);
					_TextExternalLead = _Font.ExternalLead(value);
					}
				else
					{
					_FontSize = 0;
					_FontSizeStr = null;
					_FontResourceCode = null;
					_FontResourceGlyph = null;
					_LineSpacing = 0;
					_TextAscent = 0;
					_TextInternalLead = 0;
					_TextExternalLead = 0;
					}
				return;
				}
			}

		/// <summary>
		/// Font size as a string
		/// </summary>
		internal string FontSizeStr
			{
			get
				{
				return _FontSizeStr;
				}
			}

		/// <summary>
		/// Font resource code and size as a string
		/// </summary>
		internal string FontResourceCode
			{
			get
				{
				return _FontResourceCode;
				}
			}

		/// <summary>
		/// Font resource code and size as a string
		/// </summary>
		internal string FontResourceGlyph
			{
			get
				{
				return _FontResourceGlyph;
				}
			}

		/// <summary>
		/// Line spacing for selected font and font size
		/// </summary>
		public double LineSpacing
			{
			get
				{
				return _LineSpacing;
				}
			}

		/// <summary>
		/// Text ascent for selected font and font size
		/// </summary>
		public double TextAscent
			{
			get
				{
				return _TextAscent;
				}
			}

		/// <summary>
		/// Text descent for selected font and font size
		/// It is DesignDescent plus ExternalLead
		/// </summary>
		public double TextDescent
			{
			get
				{
				return _LineSpacing - _TextAscent;
				}
			}

		/// <summary>
		/// Text internal lead (it is part of TextAscent)
		/// </summary>
		public double TextInternalLead
			{
			get
				{
				return _TextInternalLead;
				}
			}

		/// <summary>
		/// Text external lead (it is part of TextDescent)
		/// </summary>
		public double TextExternalLead
			{
			get
				{
				return _TextExternalLead;
				}
			}

		/// <summary>
		/// PdfDrawTextCtrl constructor
		/// </summary>
		/// <param name="Document">PDF document</param>
		/// <param name="FontFamilyName">Font family name</param>
		/// <param name="FontStyle">Font style (normal, bold, italic, bold+italic)</param>
		/// <param name="FontSize">Font size in points</param>
		public PdfDrawTextCtrl
				(
				PdfDocument Document,
				string FontFamilyName,
				FontStyle FontStyle,    // must be: normal, bold, italic or bold and italic
				double FontSize
				) : this(PdfFont.CreatePdfFont(Document, FontFamilyName, FontStyle, true), FontSize) { }

		/// <summary>
		/// PdfDrawTextCtrl constructor
		/// </summary>
		/// <param name="Font">PDF font class</param>
		/// <param name="FontSize">Font size in points</param>
		public PdfDrawTextCtrl
				(
				PdfFont Font,
				double FontSize
				)
			{
			_Font = Font;
			this.FontSize = FontSize;
			TextColor = Color.Black;
			return;
			}

		/// <summary>
		/// PdfDrawTextCtrl copy constructor
		/// </summary>
		/// <param name="Other">Existing PdfDrawTextCtrl class</param>
		public PdfDrawTextCtrl
				(
				PdfDrawTextCtrl Other
				)
			{
			_Font = Other._Font;
			FontSize = Other._FontSize;
			TextColor = Other.TextColor;
			DrawStyle = Other.DrawStyle;
			Justify = Other.Justify;
			_Annotation = Other._Annotation;
			return;
			}

		/// <summary>
		/// Compare two PdfDrawTextCtrl objects
		/// </summary>
		/// <param name="Other">Other object</param>
		/// <returns>Compare result</returns>
		public bool IsEqual
				(
				PdfDrawTextCtrl Other
				)
			{
			return _Font == Other._Font &&
				_FontSize == Other._FontSize &&
				TextColor == Other.TextColor &&
				DrawStyle == Other.DrawStyle &&
				Justify == Other.Justify &&
				_Annotation == Other._Annotation;
			}

		/// <summary>
		/// Font Family Name
		/// </summary>
		public string FontFamilyName
			{
			get
				{
				return _Font.FontFamilyName;
				}
			}

		/// <summary>
		/// Font units to user units
		/// </summary>
		/// <param name="Value">Design value</param>
		/// <returns>Design value in user units</returns>
		public double FontDesignToUserUnits
				(
				int Value
				)
			{
			return _Font.FontDesignToUserUnits(FontSize, Value);
			}

		/// <summary>
		/// Character width
		/// </summary>
		/// <param name="CharValue">Character</param>
		/// <returns>Width</returns>
		public double CharWidth
				(
				char CharValue
				)
			{
			return _Font.CharWidth(FontSize, DrawStyle, CharValue);
			}

		/// <summary>
		/// Text string width
		/// </summary>
		/// <param name="Text">Text string</param>
		/// <returns>Width</returns>
		public double TextWidth
				(
				string Text
				)
			{
			return _Font.TextWidth(FontSize, Text);
			}

		/// <summary>
		/// Text bounding box
		/// </summary>
		/// <param name="Text">Text string</param>
		/// <returns>Bounding box rectangle</returns>
		public PdfRectangle TextBoundingBox
				(
				string Text
				)
			{
			return _Font.TextBoundingBox(FontSize, Text);
			}

		/// <summary>
		/// Capital M height in user units
		/// </summary>
		/// <returns>Capital M height</returns>
		public double CapHeight
			{
			get
				{
				return _Font.CapHeight(FontSize);
				}
			}

		/// <summary>
		/// Strikeout position in user units
		/// </summary>
		/// <returns>Strikeout position</returns>
		public double StrikeoutPosition
			{
			get
				{
				return _Font.StrikeoutPosition(FontSize);
				}
			}

		/// <summary>
		/// Strikeout width in user units
		/// </summary>
		/// <returns>Strikeout line width.</returns>
		public double StrikeoutWidth
			{
			get
				{
				return _Font.StrikeoutWidth(FontSize);
				}
			}

		/// <summary>
		/// Underline position in user units
		/// </summary>
		/// <returns>Underline position</returns>
		public double UnderlinePosition
			{
			get
				{
				return _Font.UnderlinePosition(FontSize);
				}
			}

		/// <summary>
		/// Underline width in user units
		/// </summary>
		/// <returns>Underline line width.</returns>
		public double UnderlineWidth
			{
			get
				{
				return _Font.UnderlineWidth(FontSize);
				}
			}

		/// <summary>
		/// Subscript position in user units
		/// </summary>
		/// <returns>Subscript position</returns>
		public double SubscriptPosition
			{
			get
				{
				return _Font.SubscriptPosition(FontSize);
				}
			}

		/// <summary>
		/// Subscript character size in points
		/// </summary>
		/// <returns>Subscript font size</returns>
		public double SubscriptSize
			{
			get
				{
				return _Font.SubscriptSize(FontSize);
				}
			}

		/// <summary>
		/// Superscript character position
		/// </summary>
		/// <returns>Superscript position</returns>
		public double SuperscriptPosition
			{
			get
				{
				return _Font.SuperscriptPosition(FontSize);
				}
			}

		/// <summary>
		/// Superscript character size in points
		/// </summary>
		/// <returns>Superscript font size</returns>
		public double SuperscriptSize
			{
			get
				{
				return _Font.SuperscriptSize(FontSize);
				}
			}

		/// <summary>
		/// Word spacing to stretch text to given width
		/// </summary>
		/// <param name="ReqWidth">Required width</param>
		/// <param name="WordSpacing">Output word spacing</param>
		/// <param name="CharSpacing">Output character spacing</param>
		/// <param name="Text">Text</param>
		/// <returns>True-done, False-not done.</returns>
		public bool TextFitToWidth
				(
				double ReqWidth,
				out double WordSpacing,
				out double CharSpacing,
				string Text
				)
			{
			return _Font.TextFitToWidth(FontSize, ReqWidth, out WordSpacing, out CharSpacing, Text);
			}
		}
	}
