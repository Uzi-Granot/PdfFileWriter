/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	AnnotAction
//	Annotation action classes 
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

using System;

namespace PdfFileWriter
{
/// <summary>
/// File attachement icon
/// </summary>
public enum FileAttachIcon
	{
	/// <summary>
	/// Graph
	/// </summary>
	Graph,

	/// <summary>
	/// Paperclip
	/// </summary>
	Paperclip,

	/// <summary>
	/// PushPin (default)
	/// </summary>
	PushPin,

	/// <summary>
	/// Tag
	/// </summary>
	Tag,

	/// <summary>
	/// no icon 
	/// </summary>
	NoIcon,
	}

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
	}

/// <summary>
/// Annotation action base class
/// </summary>
public class AnnotAction
	{
	/// <summary>
	/// Gets the PDF PdfAnnotation object subtype
	/// </summary>
	public string Subtype {get; internal set;}

	internal AnnotAction
			(
			string Subtype
			)
		{
		this.Subtype = Subtype;
		return;
		}

	internal virtual bool IsEqual
			(
			AnnotAction Other
			)
		{
		throw new ApplicationException("AnnotAction IsEqual not implemented");
		}

	internal static bool IsEqual
			(
			AnnotAction One,
			AnnotAction Two
			)
		{
		if(One == null && Two == null) return true;
		if(One == null && Two != null || One != null && Two == null || One.GetType() != Two.GetType()) return false;
		return One.IsEqual(Two);
		}
	}

/// <summary>
/// Web link annotation action
/// </summary>
public class AnnotWebLink : AnnotAction
	{
	/// <summary>
	/// Gets or sets web link string
	/// </summary>
	public string WebLinkStr {get; set;}

	/// <summary>
	/// Web link constructor
	/// </summary>
	/// <param name="WebLinkStr">Web link string</param>
	public AnnotWebLink
			(
			string WebLinkStr
			) : base("/Link")
		{
		this.WebLinkStr = WebLinkStr;
		return;
		}

	internal override bool IsEqual
			(
			AnnotAction Other
			)
		{
		return WebLinkStr == ((AnnotWebLink) Other).WebLinkStr;
		}
	}

/// <summary>
/// Link to location marker within the document
/// </summary>
public class AnnotLinkAction : AnnotAction
	{
	/// <summary>
	/// Gets or sets the location marker name
	/// </summary>
	public string LocMarkerName {get; set;}

	/// <summary>
	/// Go to annotation action constructor
	/// </summary>
	/// <param name="LocMarkerName">Location marker name</param>
	public AnnotLinkAction
			(
			string LocMarkerName
			) : base("/Link")
		{
		this.LocMarkerName = LocMarkerName;
		return;
		}

	internal override bool IsEqual
			(
			AnnotAction Other
			)
		{
		return this.LocMarkerName == ((AnnotLinkAction) Other).LocMarkerName;
		}
	}

/// <summary>
/// Display video or play sound class
/// </summary>
public class AnnotDisplayMedia : AnnotAction
	{
	/// <summary>
	/// Gets or sets PdfDisplayMedia object
	/// </summary>
	public PdfDisplayMedia DisplayMedia {get; set;}

	/// <summary>
	/// Display media annotation action constructor
	/// </summary>
	/// <param name="DisplayMedia">PdfDisplayMedia</param>
	public AnnotDisplayMedia
			(
			PdfDisplayMedia		DisplayMedia
			) : base("/Screen")
		{
		this.DisplayMedia = DisplayMedia;
		return;
		}

	internal override bool IsEqual
			(
			AnnotAction Other
			)
		{
		return this.DisplayMedia.MediaFile.FileName == ((AnnotDisplayMedia) Other).DisplayMedia.MediaFile.FileName;
		}
	}

/// <summary>
/// Save or view embedded file
/// </summary>
public class AnnotFileAttachment : AnnotAction
	{
	/// <summary>
	/// Gets or sets embedded file
	/// </summary>
	public PdfEmbeddedFile EmbeddedFile {get; set;}

	/// <summary>
	/// Gets or sets associated icon
	/// </summary>
	public FileAttachIcon Icon;

	/// <summary>
	/// File attachement constructor
	/// </summary>
	/// <param name="EmbeddedFile">Embedded file</param>
	/// <param name="Icon">Icon enumeration</param>
	public AnnotFileAttachment
			(
			PdfEmbeddedFile EmbeddedFile,
			FileAttachIcon Icon
			) : base("/FileAttachment")
		{
		this.EmbeddedFile = EmbeddedFile;
		this.Icon = Icon;
		return;
		}

	/// <summary>
	/// File attachement constructor (no icon)
	/// </summary>
	/// <param name="EmbeddedFile">Embedded file</param>
	public AnnotFileAttachment
			(
			PdfEmbeddedFile EmbeddedFile
			) : base("/FileAttachment")
		{
		this.EmbeddedFile = EmbeddedFile;
		Icon = FileAttachIcon.NoIcon;
		return;
		}

	internal override bool IsEqual
			(
			AnnotAction Other
			)
		{
		AnnotFileAttachment FileAttach = (AnnotFileAttachment) Other;
		return EmbeddedFile.FileName == FileAttach.EmbeddedFile.FileName && Icon == FileAttach.Icon;
		}
	}

/// <summary>
/// Display sticky note
/// </summary>
public class AnnotStickyNote : AnnotAction
	{
	internal string Note;

	internal StickyNoteIcon Icon;

	/// <summary>
	/// Sticky note annotation action constructor
	/// </summary>
	/// <param name="Note">Sticky note text</param>
	/// <param name="Icon">Sticky note icon</param>
	public AnnotStickyNote
			(
			string Note,
			StickyNoteIcon Icon
			) : base("/Text")
		{
		this.Note = Note;
		this.Icon = Icon;
		return;
		}

	internal override bool IsEqual
			(
			AnnotAction Other
			)
		{
		AnnotStickyNote StickyNote = (AnnotStickyNote) Other;
		return Note == StickyNote.Note && Icon == StickyNote.Icon;
		}
	}
}
