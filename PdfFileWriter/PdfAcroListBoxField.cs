/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Acro list box
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

using System.Text;

namespace PdfFileWriter
	{
	/// <summary>
	/// Acro field list box
	/// </summary>
	public class PdfAcroListBoxField : PdfAcroWidgetField
		{
		/// <summary>
		/// Sort indicator
		/// NOTE: PDF Readers ignore this flag
		/// The reader will not sort the combobox items
		/// </summary>
		public bool Sort
			{
			get
				{
				return (FieldFlags & FieldFlag.Sort) != 0;
				}
			set
				{
				if(value)
					{
					FieldFlags |= FieldFlag.Sort;
					}
				else
					{
					FieldFlags &= ~FieldFlag.Sort;
					}
				}
			}

		/// <summary>
		/// Enable multi-select
		/// </summary>
		public bool MultiSelect
			{
			get
				{
				return (FieldFlags & FieldFlag.MultiSelect) != 0;
				}
			set
				{
				if(value)
					{
					FieldFlags |= FieldFlag.MultiSelect;
					}
				else
					{
					FieldFlags &= ~FieldFlag.MultiSelect;
					}
				}
			}

		/// <summary>
		/// Commit on select change
		/// </summary>
		public bool CommitOnSelChange
			{
			get
				{
				return (FieldFlags & FieldFlag.CommitOnSelChange) != 0;
				}
			set
				{
				if(value)
					{
					FieldFlags |= FieldFlag.CommitOnSelChange;
					}
				else
					{
					FieldFlags &= ~FieldFlag.CommitOnSelChange;
					}
				}
			}

		/// <summary>
		/// Array of items to be displayed in the list box
		/// Items will be displayed in array's order
		/// </summary>
		public string[] Items { get; set; }

		/// <summary>
		/// Field value (/V) (selected item)
		/// </summary>
		public string FieldValue { get; set; }

		/// <summary>
		/// The index of the first visible item
		/// </summary>
		public int TopIndex { get; set; }

		internal PdfFontTypeOne FontTypeOne;
		internal double FontSize;
		
		internal int Ascender = 718;
		internal int Descender = -207;

		/// <summary>
		/// Acro field choice constructor
		/// </summary>
		/// <param name="AcroForm">Acro form parent</param>
		/// <param name="FieldName">Combobox field name</param>
		/// <param name="FontTypeOne">Font type one</param>
		/// <param name="FontSize">Font size in points</param>
		public PdfAcroListBoxField
				(
				PdfAcroForm AcroForm,
				string FieldName,
				PdfFontTypeOne FontTypeOne,
				double FontSize
				) : base(AcroForm.Document, FieldName)
			{
			// save arguments
			this.FontTypeOne = FontTypeOne;
			this.FontSize = FontSize;

			// field type is choice
			Dictionary.AddName("/FT", "Ch");

			// add the field to acro fields structure
			AcroForm.AddField(this);
			return;
			}

		/// <summary>
		/// Draw list box (Appearance XObject)
		/// </summary>
		public void DrawListBox()
			{
			// Create xobject
			PdfXObject XObject = new PdfXObject(Document, AnnotRect.Width, AnnotRect.Height);

			// clear field area
			PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
			DrawCtrl.Paint = DrawPaint.Fill;
			DrawCtrl.BackgroundTexture = BackgroundColor;
			XObject.DrawGraphics(DrawCtrl, XObject.BBox);

			// we have items
			if(Items != null && Items.Length > 0)
				{ 
				// add font code to current list of font codes
				XObject.AddToUsedResources(FontTypeOne);

				// line spacing (leading) in user units
				double LineSpacing = 1.2 * FontSize / Document.ScaleFactor;

				double Ascent = 0.75 * LineSpacing;

				// left and right limits
				double LeftPos = XObject.BBox.Left;
				double RightPos = XObject.BBox.Right;

				// begin marked content and save grapics state
				XObject.BeginMarkedContent("/Tx");
				XObject.SaveGraphicsState();

				// draw items
				double YPos = XObject.BBox.Top;
				for(int Index = TopIndex; Index < Items.Length && YPos > XObject.BBox.Bottom; Index++)
					{ 
					// draw highlighted item if field value is not empty
					if(!string.IsNullOrWhiteSpace(FieldValue) && FieldValue == Items[Index])
						{
						PdfRectangle ItemRect = new PdfRectangle(LeftPos, YPos, RightPos, YPos - LineSpacing);
						DrawCtrl.BackgroundTexture = Color.FromArgb(153, 193, 218);
						XObject.DrawGraphics(DrawCtrl, ItemRect);
						}

					// text mode
					XObject.BeginTextMode();
					XObject.SetColorNonStroking(Color.Black);
					XObject.SetFontAndSize(FontTypeOne, FontSize);
					XObject.SetTextPosition(LeftPos, YPos - Ascent);

					// output text
					XObject.ObjectValueList.Add((byte) '(');
					foreach(char Chr in Items[Index]) XObject.OutputOneByte(Chr);
					XObject.ObjectValueList.Add((byte) ')');
					XObject.ObjectValueList.Add((byte) 'T');
					XObject.ObjectValueList.Add((byte) 'j');
					XObject.ObjectValueList.Add((byte) '\n');

					// end text mode
					XObject.EndTextMode();

					// update y position
					YPos -= LineSpacing;
					}

				// restore grapics state and end marked content
				XObject.RestoreGraphicsState();
				XObject.EndMarkedContent();
				}

			// add to annotation appearance dictionary
			AddAppearance(XObject, AppearanceType.Normal);
			return;
			}

		/// <summary>
		/// close object before writing to PDF file
		/// </summary>
		internal override void CloseObject()
			{
			// build PDF options array
			StringBuilder OptStr = new StringBuilder("[");
			foreach(string Op in Items)
				{
				OptStr.Append(TextToPdfString(Op, this));
				}
			OptStr.Append(']');
			Dictionary.Add("/Opt", OptStr.ToString());

			// field value
			if(!string.IsNullOrWhiteSpace(FieldValue))
				{
				// save field value
				Dictionary.AddPdfString("/V", FieldValue);

				// selected index
				int Index;
				for(Index = 0; Index < Items.Length && FieldValue != Items[Index]; Index++);
				if(Index < Items.Length)
					{
					Dictionary.Add("/I", string.Format("[{0}]", Index));
					}
				}

			// top index
			if(TopIndex < 0 || TopIndex >= Items.Length) TopIndex = 0;
			Dictionary.AddInteger("/TI", TopIndex);

			// close PdfAnnotation object
			base.CloseObject();
			return;
			}
		}
	}
