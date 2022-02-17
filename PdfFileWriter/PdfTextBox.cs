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
	/// TextBox class
	/// </summary>
	/// <remarks>
	/// <para>
	/// <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#DrawTextBox">For example of drawing TextBox see 3.12. Draw Text Box</a>
	/// </para>
	/// </remarks>
	public class PdfTextBox
		{
		/// <summary>
		/// Gets box width.
		/// </summary>
		public double BoxWidth { get; internal set; }

		/// <summary>
		/// Gets box height.
		/// </summary>
		public double BoxHeight { get; internal set; }

		/// <summary>
		/// Gets line count.
		/// </summary>
		public int LineCount { get { return LineArray.Count; } }

		/// <summary>
		/// Gets paragraph count.
		/// </summary>
		public int ParagraphCount { get; internal set; }

		/// <summary>
		/// Gets first line is indented.
		/// </summary>
		public double FirstLineIndent { get; internal set; }

		private readonly double LineBreakFactor;  // should be >= 0.1 and <= 0.9
		private char PrevChar;
		private double LineWidth;
		private double LineBreakWidth;
		private int BreakSegIndex;
		private int BreakPtr;
		private double BreakWidth;
		private readonly List<PdfTextBoxSeg> SegArray;
		private readonly List<PdfTextBoxLine> LineArray;

		/// <summary>
		/// TextBox constructor
		/// </summary>
		/// <param name="BoxWidth">Box width.</param>
		/// <param name="FirstLineIndent">First line is indented.</param>
		/// <param name="LineBreakFactor">Line break factor.</param>
		public PdfTextBox
				(
				double BoxWidth,
				double FirstLineIndent = 0.0,
				double LineBreakFactor = 0.5
				)
			{
			if(BoxWidth <= 0.0) throw new ApplicationException("Box width must be greater than zero");
			this.BoxWidth = BoxWidth;
			this.FirstLineIndent = FirstLineIndent;
			if(LineBreakFactor < 0.1 || LineBreakFactor > 0.9) throw new ApplicationException("LineBreakFactor must be between 0.1 and 0.9");
			this.LineBreakFactor = LineBreakFactor;
			SegArray = new List<PdfTextBoxSeg>();
			LineArray = new List<PdfTextBoxLine>();
			Clear();
			return;
			}

		/// <summary>
		/// Clear TextBox
		/// </summary>
		public void Clear()
			{
			BoxHeight = 0.0;
			ParagraphCount = 0;
			PrevChar = ' ';
			LineWidth = 0.0;
			LineBreakWidth = 0.0;
			BreakSegIndex = 0;
			BreakPtr = 0;
			BreakWidth = 0;
			SegArray.Clear();
			LineArray.Clear();
			return;
			}

		/// <summary>
		/// Access TextBoxLine array.
		/// </summary>
		/// <param name="Index">Index</param>
		/// <returns>TextBoxLine</returns>
		public PdfTextBoxLine this[int Index]
			{
			get
				{
				return LineArray[Index];
				}
			}

		/// <summary>
		/// TextBox height including extra line and paragraph space.
		/// </summary>
		/// <param name="LineExtraSpace">Extra line space.</param>
		/// <param name="ParagraphExtraSpace">Extra paragraph space.</param>
		/// <returns>Height</returns>
		public double BoxHeightExtra
				(
				double LineExtraSpace,
				double ParagraphExtraSpace
				)
			{
			double Height = BoxHeight;
			if(LineArray.Count > 1 && LineExtraSpace != 0.0) Height += LineExtraSpace * (LineArray.Count - 1);
			if(ParagraphCount > 1 && ParagraphExtraSpace != 0.0) Height += ParagraphExtraSpace * (ParagraphCount - 1);
			return Height;
			}

		/// <summary>
		/// Thwe height of the first LineCount lines including extra line and paragraph space.
		/// </summary>
		/// <param name="LineCount">The requested number of lines.</param>
		/// <param name="LineExtraSpace">Extra line space.</param>
		/// <param name="ParagraphExtraSpace">Extra paragraph space.</param>
		/// <returns>Height</returns>
		public double BoxHeightExtra
				(
				int LineCount,
				double LineExtraSpace,
				double ParagraphExtraSpace
				)
			{
			// textbox is empty
			if(LineArray.Count == 0) return 0.0;

			// line count is greater than available lines
			if(LineCount >= LineArray.Count) return BoxHeightExtra(LineExtraSpace, ParagraphExtraSpace);

			// calculate height for requested line count
			double Height = 0;
			for(int Index = 0; ; Index++)
				{
				PdfTextBoxLine Line = LineArray[Index];
				Height += Line.LineHeight;
				if(Index + 1 == LineCount) break;
				Height += LineExtraSpace;
				if(Line.EndOfParagraph) Height += ParagraphExtraSpace;
				}
			return Height;
			}

		/// <summary>
		/// The height of a block of lines within TextBox not excedding request height.
		/// </summary>
		/// <param name="LineStart">Start line</param>
		/// <param name="LineEnd">End line</param>
		/// <param name="RequestHeight">Requested height</param>
		/// <param name="LineExtraSpace">Extra line space.</param>
		/// <param name="ParagraphExtraSpace">Extra paragraph space.</param>
		/// <returns>Height</returns>
		/// <remarks>
		/// LineStart will be adjusted forward to skip blank lines. LineEnd 
		/// will be one after a non blank line. 
		/// </remarks>
		public double BoxHeightExtra
				(
				ref int LineStart,
				out int LineEnd,
				double RequestHeight,
				double LineExtraSpace,
				double ParagraphExtraSpace
				)
			{
			// skip blank lines
			for(; LineStart < LineArray.Count; LineStart++)
				{
				PdfTextBoxLine Line = LineArray[LineStart];
				if(!Line.EndOfParagraph || Line.SegArray.Length > 1 || Line.SegArray[0].SegWidth != 0) break;
				}

			// end of textbox
			if(LineStart >= LineArray.Count)
				{
				LineStart = LineEnd = LineArray.Count;
				return 0.0;
				}

			// calculate height for requested line count
			double Total = 0.0;
			double Height = 0.0;
			int End = LineEnd = LineStart;
			for(;;)
				{
				PdfTextBoxLine Line = LineArray[End];
				if(Total + Line.LineHeight > RequestHeight) break;
				Total += Line.LineHeight;
				End++;
				if(!Line.EndOfParagraph || Line.SegArray.Length > 1 || Line.SegArray[0].SegWidth != 0)
					{
					LineEnd = End;
					Height = Total;
					}

				if(End == LineCount) break;

				Total += LineExtraSpace;
				if(Line.EndOfParagraph) Total += ParagraphExtraSpace;
				}

			return Height;
			}

		/// <summary>
		/// Longest line width
		/// </summary>
		public double LongestLineWidth
			{
			get
				{
				double MaxWidth = 0;
				foreach(PdfTextBoxLine Line in LineArray)
					{
					double LineWidth = 0;
					foreach(PdfTextBoxSeg Seg in Line.SegArray) LineWidth += Seg.SegWidth;
					if(LineWidth > MaxWidth) MaxWidth = LineWidth;
					}
				return MaxWidth;
				}
			}

		/// <summary>
		/// Terminate TextBox
		/// </summary>
		public void Terminate()
			{
			// terminate last line
			if(SegArray.Count != 0) AddLine(true);

			// remove trailing empty paragraphs
			for(int Index = LineArray.Count - 1; Index >= 0; Index--)
				{
				PdfTextBoxLine Line = LineArray[Index];
				if(!Line.EndOfParagraph || Line.SegArray.Length > 1 || Line.SegArray[0].SegWidth != 0) break;
				BoxHeight -= Line.Ascent + Line.Descent;
				ParagraphCount--;
				LineArray.RemoveAt(Index);
				}

			// exit
			return;
			}

		/// <summary>
		/// Add text to text box.
		/// </summary>
		/// <param name="TextCtrl">PDF draw text control</param>
		/// <param name="Text">Text</param>
		public void AddText
				(
				PdfDrawTextCtrl TextCtrl,
				string Text
				)
			{
			// text is null or empty
			if(string.IsNullOrEmpty(Text)) return;

			// text segment, either a new one or the previous one
			PdfTextBoxSeg Seg;

			// segment array is empty or new segment is different than last one
			if(SegArray.Count == 0 || !SegArray[SegArray.Count - 1].IsEqual(TextCtrl))
				{
				Seg = new PdfTextBoxSeg(TextCtrl);
				SegArray.Add(Seg);
				}

			// add new text to most recent text segment
			else
				{
				Seg = SegArray[SegArray.Count - 1];
				}

			// save text start pointer
			int TextStart = 0;

			// loop for characters
			for(int TextPtr = 0; TextPtr < Text.Length; TextPtr++)
				{
				// shortcut to current character
				char CurChar = Text[TextPtr];

				// end of paragraph
				if(CurChar == '\n' || CurChar == '\r')
					{
					// append text to current segemnt
					Seg.Text += Text.Substring(TextStart, TextPtr - TextStart);

					// test for new line after carriage return
					if(CurChar == '\r' && TextPtr + 1 < Text.Length && Text[TextPtr + 1] == '\n') TextPtr++;

					// move pointer to one after the eol
					TextStart = TextPtr + 1;

					// add line
					AddLine(true);

					// update last character
					PrevChar = ' ';

					// end of text
					if(TextPtr + 1 == Text.Length) return;

					// add new empty segment copy of current segment
					Seg = new PdfTextBoxSeg(Seg);
					SegArray.Add(Seg);
					continue;
					}

				// character width
				double CharWidth = Seg.CharWidth(CurChar);

				// space
				if(CurChar == ' ')
					{
					// test for transition from non space to space
					// this is a potential line break point
					if(PrevChar != ' ')
						{
						// save potential line break information
						LineBreakWidth = LineWidth;
						BreakSegIndex = SegArray.Count - 1;
						BreakPtr = Seg.Text.Length + TextPtr - TextStart;
						BreakWidth = Seg.SegWidth;
						}

					// add to line width
					LineWidth += CharWidth;
					Seg.SegWidth += CharWidth;

					// update last character
					PrevChar = CurChar;
					continue;
					}

				// add current segment width and to overall line width
				Seg.SegWidth += CharWidth;
				LineWidth += CharWidth;

				// for next loop set last character
				PrevChar = CurChar;

				// box width
				double Width = BoxWidth;
				if(FirstLineIndent != 0 && (LineArray.Count == 0 || LineArray[LineArray.Count - 1].EndOfParagraph))
					Width -= FirstLineIndent;

				// current line width is less than or equal box width
				if(LineWidth <= Width) continue;

				// append text to current segemnt
				Seg.Text += Text.Substring(TextStart, TextPtr - TextStart + 1);
				TextStart = TextPtr + 1;

				// there are no breaks in this line or last segment is too long
				if(LineBreakWidth < LineBreakFactor * Width)
					{
					BreakSegIndex = SegArray.Count - 1;
					BreakPtr = Seg.Text.Length - 1;
					BreakWidth = Seg.SegWidth - CharWidth;
					}

				// break line
				BreakLine();

				// add line up to break point
				AddLine(false);
				}

			// save text
			Seg.Text += Text.Substring(TextStart);

			// exit
			return;
			}

		private void BreakLine()
			{
			// break segment at line break seg index into two segments
			PdfTextBoxSeg BreakSeg = SegArray[BreakSegIndex];

			// add extra segment to segment array
			if(BreakPtr != 0)
				{
				PdfTextBoxSeg ExtraSeg = new PdfTextBoxSeg(BreakSeg);
				ExtraSeg.SegWidth = BreakWidth;
				ExtraSeg.Text = BreakSeg.Text.Substring(0, BreakPtr);
				SegArray.Insert(BreakSegIndex, ExtraSeg);
				BreakSegIndex++;
				}

			// remove blanks from the area between the two sides of the segment
			for(; BreakPtr < BreakSeg.Text.Length && BreakSeg.Text[BreakPtr] == ' '; BreakPtr++);

			// save the area after the first line
			if(BreakPtr < BreakSeg.Text.Length)
				{
				BreakSeg.Text = BreakSeg.Text.Substring(BreakPtr);
				BreakSeg.SegWidth = BreakSeg.Font.TextWidth(BreakSeg.FontSize, BreakSeg.Text);
				}
			else
				{
				BreakSeg.Text = string.Empty;
				BreakSeg.SegWidth = 0.0;
				}
			BreakPtr = 0;
			BreakWidth = 0.0;
			return;
			}

		// start a new line
		private void AddLine
				(
				bool EndOfParagraph
				)
			{
			// end of paragraph
			if(EndOfParagraph) BreakSegIndex = SegArray.Count;

			// test for box too narrow
			if(BreakSegIndex < 1) throw new ApplicationException("TextBox is too narrow.");

			// test for possible trailing blanks
			if(SegArray[BreakSegIndex - 1].Text.EndsWith(" "))
				{
				// remove trailing blanks
				while(BreakSegIndex > 0)
					{
					PdfTextBoxSeg TempSeg = SegArray[BreakSegIndex - 1];
					TempSeg.Text = TempSeg.Text.TrimEnd(new char[] { ' ' });
					TempSeg.SegWidth = TempSeg.Font.TextWidth(TempSeg.FontSize, TempSeg.Text);
					if(TempSeg.Text.Length != 0 || BreakSegIndex == 1 && EndOfParagraph) break;
					BreakSegIndex--;
					SegArray.RemoveAt(BreakSegIndex);
					}
				}

			// test for abnormal case of a blank line and not end of paragraph
			if(BreakSegIndex > 0)
				{
				// allocate segment array
				PdfTextBoxSeg[] LineSegArray = new PdfTextBoxSeg[BreakSegIndex];

				// copy segments
				SegArray.CopyTo(0, LineSegArray, 0, BreakSegIndex);

				// line ascent and descent
				double LineAscent = 0;
				double LineDescent = 0;

				// loop for segments until line break segment index
				foreach(PdfTextBoxSeg Seg in LineSegArray)
					{
					double Ascent = Seg.TextAscent;
					if(Ascent > LineAscent) LineAscent = Ascent;
					double Descent = Seg.TextDescent;
					if(Descent > LineDescent) LineDescent = Descent;

					int SpaceCount = 0;
					foreach(char Chr in Seg.Text) if(Chr == ' ') SpaceCount++;
					Seg.SpaceCount = SpaceCount;
					}

				// add line
				LineArray.Add(new PdfTextBoxLine(LineAscent, LineDescent, EndOfParagraph, LineSegArray));

				// update column height
				BoxHeight += LineAscent + LineDescent;

				// update paragraph count
				if(EndOfParagraph) ParagraphCount++;

				// remove segments
				SegArray.RemoveRange(0, BreakSegIndex);
				}

			// switch to next line
			LineBreakWidth = 0.0;
			BreakSegIndex = 0;

			// new line width
			LineWidth = 0.0;
			foreach(PdfTextBoxSeg Seg in SegArray) LineWidth += Seg.SegWidth;
			return;
			}
		}
	}
