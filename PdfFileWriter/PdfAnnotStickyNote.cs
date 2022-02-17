/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Annotation sticky note
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
	/// Sticky note icon
	/// </summary>
	public enum StickyNoteIcon
		{
		/// <summary>
		/// Comment (note: no icon)
		/// </summary>
		Comment,
		/// <summary>
		/// Key
		/// </summary>
		Key,
		/// <summary>
		/// Note (default)
		/// </summary>
		Note,
		/// <summary>
		/// Help
		/// </summary>
		Help,
		/// <summary>
		/// New paragraph
		/// </summary>
		NewParagraph,
		/// <summary>
		/// Paragraph
		/// </summary>
		Paragraph,
		/// <summary>
		/// Insert
		/// </summary>
		Insert,
		/// <summary>
		/// No icon
		/// </summary>
		NoIcon,
		}

	/// <summary>
	/// Display sticky note
	/// </summary>
	public class PdfAnnotStickyNote : PdfAnnotation
		{
		internal string Note;
		internal StickyNoteIcon Icon;

		/// <summary>
		/// Sticky note annotation action constructor
		/// </summary>
		/// <param name="Document">PDF document</param>
		/// <param name="Note">Sticky note text</param>
		/// <param name="Icon">Sticky note icon</param>
		public PdfAnnotStickyNote
				(
				PdfDocument Document,
				string Note,
				StickyNoteIcon Icon = StickyNoteIcon.NoIcon
				) : base(Document, "/Text")
			{
			// save arguments
			this.Note = Note;
			this.Icon = Icon;

			// call constructor
			Constructor();
			return;
			}

		/// <summary>
		/// Sticky note annotation action constructor
		/// </summary>
		/// <param name="Other">Other stick note object</param>
		public PdfAnnotStickyNote
				(
				PdfAnnotStickyNote Other
				) : base(Other.Document, "/Text")
			{
			// save arguments
			Note = Other.Note;
			Icon = Other.Icon;

			// call constructor
			Constructor();

			// copy base values
			base.CreateCopy(Other);
			return;
			}

		private void Constructor()
			{ 
			// action reference dictionary
			Dictionary.AddPdfString("/Contents", Note);

			// no icon
			if(Icon == StickyNoteIcon.NoIcon)
				{
				// no icon (override icon with empty appearance xobject)
				PdfXObject XObject = new PdfXObject(Document, 0, 0);
				AddAppearance(XObject, AppearanceType.Normal);
				}

			// icon
			else
				{ 
				Dictionary.AddName("/Name", Icon.ToString());
				}
			return;
			}
		}
	}
