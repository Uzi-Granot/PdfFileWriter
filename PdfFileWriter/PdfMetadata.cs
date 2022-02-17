/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PdfMetadata
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
	/// PDF metadata class
	/// </summary>
	public class PdfMetadata : PdfObject
		{
		/// <summary>
		/// PDF metadata constructor
		/// </summary>
		/// <param name="Document">PDF document</param>
		/// <param name="FileName">Metadata file name</param>
		public PdfMetadata
				(
				PdfDocument Document,
				string FileName
				) : base(Document, ObjectType.Stream, "/Metadata")
			{
			// test exitance
			if(!File.Exists(FileName)) throw new ApplicationException("Metadata file " + FileName + " does not exist");

			// get file length
			FileInfo FI = new FileInfo(FileName);
			if(FI.Length > int.MaxValue - 4095) throw new ApplicationException("Metadata file " + FileName + " too long");
			int FileLength = (int) FI.Length;

			// file data content byte array
			byte[] Metadata = new byte[FileLength];

			// load all the file's data
			FileStream DataStream;
			try
				{
				// open the file
				DataStream = new FileStream(FileName, FileMode.Open, FileAccess.Read);

				// read all the file
				if(DataStream.Read(Metadata, 0, FileLength) != FileLength) throw new Exception();
				}

			// loading file failed
			catch(Exception)
				{
				throw new ApplicationException("Reading metadata file: " + FileName + " failed");
				}

			// close the file
			DataStream.Close();

			// create object
			CreateObject(Metadata);
			return;
			}

		/// <summary>
		/// PDF metadata constructor
		/// </summary>
		/// <param name="Document">PDF document</param>
		/// <param name="Metadata">Metadata binary array</param>
		public PdfMetadata
				(
				PdfDocument Document,
				byte[] Metadata
				) : base(Document, ObjectType.Stream, "/Metadata")
			{
			CreateObject(Metadata);
			return;
			}

		// add metadata to PDF file
		private void CreateObject
				(
				byte[] Metadata
				)
			{ 
			// test for first time
			if(Document.CatalogObject.Dictionary.Find("/Metadata") >= 0) throw new ApplicationException("Metadata is already defined");

			// add metadata object to catalog object
			Document.CatalogObject.Dictionary.AddIndirectReference("/Metadata", this);

			// add subtype
			Dictionary.Add("/Subtype", "/XML");

			// metadata to object value array
			ObjectValueArray = Metadata; 

			// no compression
			NoCompression = true;

			// no encryption
			PdfEncryption SaveEncryption = Document.Encryption;
			Document.Encryption = null;

			// write stream
			WriteToPdfFile();

			// restore encryption
			Document.Encryption = SaveEncryption;
			return;
			}
		}
	}
