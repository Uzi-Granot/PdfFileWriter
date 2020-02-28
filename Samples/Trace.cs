/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	Trace
//	Trace errors.
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
using System.Text;
using PdfFileWriter;

namespace TestPdfFileWriter
{
/////////////////////////////////////////////////////////////////////
// Trace Class
/////////////////////////////////////////////////////////////////////

static public class Trace
	{
	private static string	TraceFileName;		// trace file name
	private static int	MaxAllowedFileSize = 0x10000;

	/////////////////////////////////////////////////////////////////////
	// Open trace file
	/////////////////////////////////////////////////////////////////////

	public static void Open
			(
			string	FileName
			)
		{
		// save full file name
		TraceFileName = Path.GetFullPath(FileName);
		Trace.Write("----");
		return;
		}

	/////////////////////////////////////////////////////////////////////
	// write to trace file
	/////////////////////////////////////////////////////////////////////

	public static void Write
			(
			string Message
			)
		{
		// test file length
		TestSize();

		// open existing or create new trace file
		StreamWriter TraceFile = new StreamWriter(TraceFileName, true);

		// write date and time
		TraceFile.Write(string.Format("{0:yyyy}/{0:MM}/{0:dd} {0:HH}:{0:mm}:{0:ss} ", DateTime.Now));

		// write message
		TraceFile.WriteLine(Message);

		// close the file
		TraceFile.Close();

		// exit
		return;
		}


	/////////////////////////////////////////////////////////////////////
	// Barcode Trace
	/////////////////////////////////////////////////////////////////////

	public static void BarcodeTrace
			(
			Barcode Barcode 
			)
		{
		if(Barcode.GetType() == typeof(Barcode128)) Write("Barcode128");
		else if(Barcode.GetType() == typeof(Barcode39)) Write("Barcode39");
		else if(Barcode.GetType() == typeof(BarcodeEAN13)) Write(Barcode.Text.Length == 13 ? "BarcodeEAN-13" : "BarcodeUPC");
		else Write("Barcode Unknown");

		int Index;
		for(Index = 0; Index < Barcode.Text.Length && Barcode.Text[Index] >= ' ' && Barcode.Text[Index] <= '~'; Index++);
		if(Index < Barcode.Text.Length)
			{
			StringBuilder Str = new StringBuilder(Barcode.Text.Substring(0, Index));
			for(; Index < Barcode.Text.Length; Index++)
				{
				if(Barcode.Text[Index] >= ' ' && Barcode.Text[Index] <= '~')
					{
					Str.Append(Barcode.Text[Index]);
					}
				else
					{
					Str.AppendFormat("\\U{0:x4}", (int) Barcode.Text[Index]);
					}
				}
			Write(Str.ToString());
			}
		else
			{
			Write(Barcode.Text);
			}

		StringBuilder Code = new StringBuilder();
		for(Index = 0; Index < Barcode.CodeArray.Length; Index++)
			{
			if(Index != 0) Code.Append(", ");
			Code.Append(Barcode.CodeArray[Index].ToString());
			}
		Write(Code.ToString());
		return;
		}

	/////////////////////////////////////////////////////////////////////
	// Test file size
	// If file is too big, remove first quarter of the file
	/////////////////////////////////////////////////////////////////////

	private static void TestSize()
		{
		// get trace file info
		FileInfo TraceFileInfo = new FileInfo(TraceFileName);

		// if file does not exist or file length less than max allowed file size do nothing
		if(TraceFileInfo.Exists == false || TraceFileInfo.Length <= MaxAllowedFileSize) return;

		// create file info class
		FileStream TraceFile = new FileStream(TraceFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

		// seek to 25% length
		TraceFile.Seek(TraceFile.Length / 4, SeekOrigin.Begin);

		// new file length
		int NewFileLength = (int) (TraceFile.Length - TraceFile.Position);

		// new file buffer
		byte[] Buffer = new byte[NewFileLength];

		// read file to the end
		TraceFile.Read(Buffer, 0, NewFileLength);

		// search for first end of line
		int StartPtr = 0;
		while(StartPtr < 1024 && Buffer[StartPtr++] != '\n');
		if(StartPtr == 1024) StartPtr = 0;

		// seek to start of file
		TraceFile.Seek(0, SeekOrigin.Begin);

		// write 75% top part of file over the start of the file
		TraceFile.Write(Buffer, StartPtr, NewFileLength - StartPtr);

		// truncate the file
		TraceFile.SetLength(TraceFile.Position);

		// close the file
		TraceFile.Close();

		// exit
		return;
		}
	}
}