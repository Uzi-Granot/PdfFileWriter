/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Annotations file attachment
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
	/// File attachement icon
	/// </summary>
	public enum FileAttachIcon
		{
		/// <summary>
		/// PushPin (28 by 40) (default)
		/// </summary>
		PushPin,

		/// <summary>
		/// Graph (40 by 40)
		/// </summary>
		Graph,

		/// <summary>
		/// Paperclip (14 by 34)
		/// </summary>
		Paperclip,

		/// <summary>
		/// Tag (40 by 32)
		/// </summary>
		Tag,

		/// <summary>
		/// no icon 
		/// </summary>
		NoIcon,
		}

	/// <summary>
	/// Save or view embedded file
	/// </summary>
	public class PdfAnnotFileAttachment : PdfAnnotation
		{
		internal PdfEmbeddedFile EmbeddedFile;
		internal FileAttachIcon Icon;

		/// <summary>
		/// Icon aspect ratio
		/// </summary>
		public static readonly double[] IconAspectRatio =
			{
			0.7, // push pin
			1.0, // graph
			0.4128, // paper clip
			1.25, // tag
			};

		/// <summary>
		/// File attachement constructor
		/// </summary>
		/// <param name="Document">PDF document</param>
		/// <param name="EmbeddedFile">Embedded file</param>
		/// <param name="Icon">Icon enumeration</param>
		public PdfAnnotFileAttachment
				(
				PdfDocument Document,
				PdfEmbeddedFile EmbeddedFile,
				FileAttachIcon Icon = FileAttachIcon.NoIcon
				) : base(Document, "/FileAttachment")
			{
			// save embeded file and icon
			this.EmbeddedFile = EmbeddedFile;
			this.Icon = Icon;

			// common constructor code
			Constructor();
			return;
			}

		/// <summary>
		/// File attachment copy constructor
		/// </summary>
		/// <param name="Other">Another file attachment</param>
		public PdfAnnotFileAttachment
				(
				PdfAnnotFileAttachment Other
				) : base(Other.Document, "/FileAttachment")
			{
			// save embeded file and icon
			EmbeddedFile = Other.EmbeddedFile;
			Icon = Other.Icon;

			// common constructor code
			Constructor();

			// copy base values
			base.CreateCopy(Other);
			return;
			}

		private void Constructor()
			{ 
			// add embedded file to annotation dictionary
			Dictionary.AddIndirectReference("/FS", EmbeddedFile);

			// no icon
			if(Icon == FileAttachIcon.NoIcon)
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
