using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfFileWriter
	{
	public class PdfFontControl
		{
		public PdfFont Font { get; internal set; }

		public Color TextColor { get; set; }

		public DrawStyle DrawStyle { get; set; }

		public TextJustify TextJustify { get; set; }

		public AnnotAction AnnotAction { get; set; }

		private double _FontSize;
		private double _LineHeight;
		private double _TextAscent;
		private double _TextDescent;

		public PdfFontControl
				(
				PdfDocument Document,
				string FontFamilyName,
				FontStyle FontStyle
				) : this(PdfFont.CreatePdfFont(Document, FontFamilyName, FontStyle, true)) { }

		public PdfFontControl
				(
				PdfFont Font
				)
			{
			this.Font = Font;
			FontSize = 12;
			TextColor = Color.Empty;
			DrawStyle = DrawStyle.Normal;
			TextJustify = TextJustify.Left;
			AnnotAction = null;
			return;
			}

		public PdfFontControl
				(
				PdfFontControl Other
				)
			{
			Font = Other.Font;
			FontSize = Other.FontSize;
			_TextAscent = Other._TextAscent;
			_TextDescent = Other._TextDescent;
			_LineHeight = Other._LineHeight;
			DrawStyle = Other.DrawStyle;
			TextColor = Other.TextColor;
			AnnotAction = Other.AnnotAction;
			return;
			}

		public double FontSize
			{
			get
				{
				return _FontSize;
				}
			set
				{
				_FontSize = value;
				LineHeight = Font.LineSpacing(value);
				return;
				}
			}

		public double LineHeight
			{
			get
				{
				return _LineHeight;
				}
			set
				{
				_LineHeight = value;
				_TextAscent = 0.5 * (_LineHeight + Font.Ascent(_FontSize) - Font.Descent(_FontSize));
				_TextDescent = _LineHeight - _TextAscent;
				return;
				}
			}

		public double TextAscent
			{
			get
				{
				return _TextAscent;
				}
			}

		public double TextDescent
			{
			get
				{
				return _TextDescent;
				}
			}

		public void RelativeLineHeight
				(
				double Factor
				)
			{
			_LineHeight = Factor * Font.LineSpacing(_FontSize);
			_TextAscent = 0.5 * (_LineHeight + Font.Ascent(_FontSize) - Font.Descent(_FontSize));
			_TextDescent = _LineHeight - _TextAscent;
			return;
			}

		/// <summary>
		/// Font Family Name
		/// </summary>
		public string FontFamilyName
			{
			get
				{
				return Font.FontFamilyName;
				}
			}

		/// <summary>
		/// Font style
		/// </summary>
		public FontStyle FontStyle
			{
			get
				{
				return Font.FontStyle;
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
			return Font.FontDesignToUserUnits(FontSize, Value);
			}

		public double CharWidth
				(
				char CharValue
				)
			{
			return Font.CharWidth(FontSize, DrawStyle, CharValue);
			}

		public double TextWidth
				(
				string Text
				)
			{
			return Font.TextWidth(FontSize, Text);
			}

		public PdfRectangle TextBoundingBox
				(
				string Text
				)
			{
			return Font.TextBoundingBox(FontSize, Text);
			}

		/// <summary>
		/// Capital M height in user units
		/// </summary>
		/// <returns>Capital M height</returns>
		public double CapHeight
			{
			get
				{
				return Font.CapHeight(FontSize);
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
				return Font.StrikeoutPosition(FontSize);
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
				return Font.StrikeoutWidth(FontSize);
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
				return Font.UnderlinePosition(FontSize);
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
				return Font.UnderlineWidth(FontSize);
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
				return Font.SubscriptPosition(FontSize);
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
				return Font.SubscriptSize(FontSize);
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
				return Font.SuperscriptPosition(FontSize);
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
				return Font.SuperscriptSize(FontSize);
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
			return Font.TextFitToWidth(FontSize, ReqWidth, out WordSpacing, out CharSpacing, Text);
			}

		public bool IsEqual
				(
				PdfFontControl Other
				)
			{
			// test all but annotation action
			return Font == Other.Font && FontSize == Other.FontSize && DrawStyle == Other.DrawStyle &&
				TextColor == Other.TextColor && AnnotAction.IsEqual(AnnotAction, Other.AnnotAction);
			}
		}
	}
