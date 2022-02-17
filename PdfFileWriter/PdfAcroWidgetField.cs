/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Acro widget field base class
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
	internal enum FieldFlag
		{
		ReadOnly = 1,
		Required = 2,
		NoExport = 4,

		// text fields
		Multiline = (1 << (13 - 1)),
		Password = (1 << (14 - 1)),
		FileSelect = (1 << (21 - 1)),
		DoNotSpellCheck = (1 << (23 - 1)),
		DoNotScroll = (1 << (24 - 1)),
		Comb = (1 << (25 - 1)),
		RichText = (1 << (26 -1)),

		// choice fields
		ComboBox = (1 << (18 - 1)),
		Edit = (1 << (19 -1)),
		Sort = (1 << (20 - 1)),
		MultiSelect = (1 << (21 - 1)),
		CommitOnSelChange = (1 << (27 - 1)),

		// button fields
		NoToggleToOff = (1 << (15 - 1)),
		Radio = (1 << (16 - 1)),
		PushButton = (1 << (17 - 1)),
		RadioInUnison = (1 << (26 - 1)),
		}

	/// <summary>
	/// Widget field class
	/// </summary>
	public class PdfAcroWidgetField : PdfAnnotWidget
		{
		/// <summary>
		/// Field partial name (/T)
		/// </summary>
		internal string FieldName;

		/// <summary>
		/// Field flags
		/// </summary>
		internal FieldFlag FieldFlags; // (/Ff)

		/// <summary>
		/// Alternate field name (/TU)
		/// </summary>
		public string AlternateName;

		/// <summary>
		/// Mapping field name (/TM)
		/// </summary>
		public string MappingName;

		/// <summary>
		/// AA dictionary format event (/F)
		/// </summary>
		public string FieldFormatEvent;

		/// <summary>
		/// AA dictionary keystroke event (/K)
		/// </summary>
		public string FieldKeystrokeEvent;

		/// <summary>
		/// Text appearace font
		/// </summary>
		public PdfDrawTextCtrl TextCtrl;

		/// <summary>
		/// Set field to read only
		/// </summary>
		public bool ReadOnly
			{
			get
				{
				return (FieldFlags & FieldFlag.ReadOnly) != 0;
				}
			set
				{
				if(value)
					{
					FieldFlags |= FieldFlag.ReadOnly;
					}
				else
					{
					FieldFlags &= ~FieldFlag.ReadOnly;
					}
				return;
				}
			}

		/// <summary>
		/// Set field to required (cannot be blank)
		/// </summary>
		public bool Required
			{
			get
				{
				return (FieldFlags & FieldFlag.Required) != 0;
				}
			set
				{
				if(value)
					{
					FieldFlags |= FieldFlag.Required;
					}
				else
					{
					FieldFlags &= ~FieldFlag.Required;
					}
				return;
				}
			}

		/// <summary>
		/// Set field to no export
		/// </summary>
		public bool NoExport
			{
			get
				{
				return (FieldFlags & FieldFlag.NoExport) != 0;
				}
			set
				{
				if(value)
					{
					FieldFlags |= FieldFlag.NoExport;
					}
				else
					{
					FieldFlags &= ~FieldFlag.NoExport;
					}
				return;
				}
			}

		internal PdfAcroWidgetField
				(
				PdfDocument Document,
				string FieldName
				) : base(Document)
			{
			// test argument
			if(string.IsNullOrWhiteSpace(FieldName)) throw new ApplicationException("Invalid field name");

			// save field name
			this.FieldName = FieldName;
			return;
			}

		/// <summary>
		/// close object before writing to PDF file
		/// </summary>
		internal override void CloseObject()
			{
			if(this.GetType() != typeof(PdfAcroRadioButton))
				{ 
				// partial field name
				if(FieldName != null) Dictionary.AddPdfString("/T", FieldName);

				// field flags
				if(FieldFlags != 0) Dictionary.AddInteger("/Ff", (int) FieldFlags);

				// Alternate field name (/TU)
				if(AlternateName != null) Dictionary.AddPdfString("/TU", AlternateName);
				else if (FieldName != null) Dictionary.AddPdfString("/TU", FieldName);

				// Mapping field name (/TM)
				if (MappingName != null) Dictionary.AddPdfString("/TM", MappingName);
				}

			// all fields except checkbox and radio button
			if(this.GetType() != typeof(PdfAcroCheckBoxField) && this.GetType() != typeof(PdfAcroRadioButton) && TextCtrl != null)
				{
				// create default appearance string
				string AppStr = string.Format("{0} {1} Tf {2}", TextCtrl.Font.ResourceCode, TextCtrl.FontSizeStr,
					PdfContents.ColorToString(TextCtrl.TextColor, ColorToStr.NonStroking));
				Dictionary.AddPdfString("/DA", AppStr);
				}

			// additional action dictionary
			if(FieldFormatEvent != null || FieldKeystrokeEvent != null)
				{ 
				// create Additional Actions dictionary and attach to annotection dictionary
				PdfDictionary AADict = Dictionary.GetOrAddDictionary("/AA");

				// AA dictionary format event (/F)
				if(FieldFormatEvent != null) AADict.Add("/F", string.Format("<</S/JavaScript/JS {0}>>", TextToPdfString(FieldFormatEvent, this)));

				// AA dictionary keystroke event (/K)
				if(FieldKeystrokeEvent != null)  AADict.Add("/K", string.Format("<</S/JavaScript/JS {0}>>", TextToPdfString(FieldKeystrokeEvent, this)));
				}

			// close PdfAnnotation object
			base.CloseObject();
			return;
			}
		}
	}
