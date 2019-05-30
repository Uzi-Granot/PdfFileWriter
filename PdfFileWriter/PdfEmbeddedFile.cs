/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfEmbeddedFile
//	PDF embedded file class. 
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
using System.IO;
using System.Collections.Generic;

namespace PdfFileWriter
{
/// <summary>
/// PDF Embedded file class
/// </summary>
public class PdfEmbeddedFile : PdfObject, IComparable<PdfEmbeddedFile>
	{
	/// <summary>
	/// Gets file name
	/// </summary>
	public string FileName {get; private set;}

	/// <summary>
	/// Gets Mime type
	/// </summary>
	/// <remarks>
	/// <para>
	/// The PDF embedded file translates the file extension into mime type string.
	/// If the translation fails the MimeType is set to null.
	/// </para>
	/// </remarks>
	public string MimeType {get; private set;}

	private PdfEmbeddedFile() {}

	private PdfEmbeddedFile
			(
			PdfDocument		Document,
			string			FileName,
			string			PdfFileName
			) : base(Document, ObjectType.Dictionary, "/Filespec")
		{
		// save file name
		this.FileName = FileName;

		// test exitance
		if(!File.Exists(FileName)) throw new ApplicationException("Embedded file " + FileName + " does not exist");

		// get file length
		FileInfo FI = new FileInfo(FileName);
		if(FI.Length > int.MaxValue - 4095) throw new ApplicationException("Embedded file " + FileName + " too long");
		int FileLength = (int) FI.Length;

		// translate file extension to mime type string
		MimeType = ExtToMime.TranslateExtToMime(FI.Extension);

		// create embedded file object
		PdfObject EmbeddedFile = new PdfObject(Document, ObjectType.Stream, "/EmbeddedFile");

		// save uncompressed file length
		EmbeddedFile.Dictionary.AddFormat("/Params", "<</Size {0}>>", FileLength);

		// file data content byte array
		EmbeddedFile.ObjectValueArray = new byte[FileLength];

		// load all the file's data
		FileStream DataStream = null;
		try
			{
			// open the file
			DataStream = new FileStream(FileName, FileMode.Open, FileAccess.Read);

			// read all the file
			if(DataStream.Read(EmbeddedFile.ObjectValueArray, 0, FileLength) != FileLength) throw new Exception();
			}

		// loading file failed
		catch(Exception)
			{
			throw new ApplicationException("Invalid media file: " + FileName);
			}

		// close the file
		DataStream.Close();

		// debug
		if(Document.Debug) EmbeddedFile.ObjectValueArray = Document.TextToByteArray("*** MEDIA FILE PLACE HOLDER ***");

		// write stream
		EmbeddedFile.WriteObjectToPdfFile();

 		// file spec object type
		Dictionary.Add("/Type", "/Filespec");

		// PDF file name
		if(string.IsNullOrWhiteSpace(PdfFileName)) PdfFileName = FI.Name;
		Dictionary.AddPdfString("/F", PdfFileName);
		Dictionary.AddPdfString("/UF", PdfFileName);

		// add reference
		Dictionary.AddFormat("/EF", "<</F {0} 0 R /UF {0} 0 R>>", EmbeddedFile.ObjectNumber);
		return;
		}

	private PdfEmbeddedFile
			(
			string FileName
			)
		{
		// save file name
		this.FileName = FileName;
		return;
		}

	/// <summary>
	/// PDF embedded file class constructor
	/// </summary>
	/// <param name="Document">Current document</param>
	/// <param name="FileName">File name</param>
	/// <param name="PdfFileName">PDF file name (see remarks)</param>
	/// <returns>PdfEmbeddedFile object</returns>
	/// <remarks>
	/// <para>
	/// FileName is the name of the source file on the hard disk.
	/// PDFFileName is the name of the as saved within the PDF document file.
	/// If PDFFileName is not given or it is set to null, the class takes
	/// the hard disk's file name without the path.
	/// </para>
	/// </remarks>
	public static PdfEmbeddedFile CreateEmbeddedFile
			(
			PdfDocument		Document,
			string			FileName,
			string			PdfFileName = null
			)
		{
		// first time
		if(Document.EmbeddedFileArray == null) Document.EmbeddedFileArray = new List<PdfEmbeddedFile>();

		// search list for a duplicate
		int Index = Document.EmbeddedFileArray.BinarySearch(new PdfEmbeddedFile(FileName));

		// this is a duplicate
		if(Index >= 0) return Document.EmbeddedFileArray[Index];

		// new object
		PdfEmbeddedFile EmbeddedFile = new PdfEmbeddedFile(Document, FileName, PdfFileName);

		// save new string in array
		Document.EmbeddedFileArray.Insert(~Index, EmbeddedFile);

		// exit
		return EmbeddedFile;
		}

	/// <summary>
	/// Compare two PdfEmbededFile objects
	/// </summary>
	/// <param name="Other">Other argument</param>
	/// <returns>Compare result</returns>
	public int CompareTo
			(
			PdfEmbeddedFile Other
			)
		{
		return string.Compare(this.FileName, Other.FileName, true);
		}
	}

internal class ExtToMime : IComparable<ExtToMime>
	{
	internal string	Ext;
	internal string	Mime;

	internal ExtToMime
			(
			string	Ext,
			string	Mime
			)
		{
		this.Ext = Ext;
		this.Mime = Mime;
		return;
		}

	internal static string TranslateExtToMime
			(
			string Ext
			)
		{
		int Index = Array.BinarySearch(ExtToMimeArray, new ExtToMime(Ext, null));
		return(Index >= 0 ? ExtToMimeArray[Index].Mime : null);
		}

	/// <summary>
	/// Compare ExtToMime records
	/// </summary>
	/// <param name="Other">Other record</param>
	/// <returns></returns>
	public int CompareTo
			(
			ExtToMime Other
			)
		{
		return(string.Compare(this.Ext, Other.Ext, true));
		}

	private static ExtToMime[]	ExtToMimeArray =
		{
		new ExtToMime(".avi", "video/avi"),			// Covers most Windows-compatible formats including .avi and .divx
		new ExtToMime(".divx", "video/avi"),		// Covers most Windows-compatible formats including .avi and .divx
		new ExtToMime(".mpg", "video/mpeg"),		// MPEG-1 video with multiplexed audio; Defined in RFC 2045 and RFC 2046
		new ExtToMime(".mpeg", "video/mpeg"),		// MPEG-1 video with multiplexed audio; Defined in RFC 2045 and RFC 2046
		new ExtToMime(".mp4", "video/mp4"),			// MP4 video; Defined in RFC 4337
		new ExtToMime(".mov", "video/quicktime"),	// QuickTime video .mov
		new ExtToMime(".wav", "audio/wav"),			// audio
		new ExtToMime(".wma", "audio/x-ms-wma"),	// audio
		new ExtToMime(".mp3", "audio/mpeg"),		// audio
		};

	static ExtToMime()
		{
		Array.Sort(ExtToMimeArray);
		return;
		}
	}
}
