/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter II
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	Trace
//	Trace errors.
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

#if DEBUG

namespace TestPdfFileWriter
	{
	/// <summary>
	/// Trase class for debugging
	/// </summary>
	static public class Trace
		{
		private static string TraceFileName; // trace file name
		private static int MaxAllowedFileSize = 0x10000;

		/// <summary>
		/// Open trace file
		/// </summary>
		/// <param name="FileName">Trace file name</param>
		public static void Open
				(
				string FileName
				)
			{
			// save full file name
			TraceFileName = Path.GetFullPath(FileName);
			Trace.Write("----");
			return;
			}

		/// <summary>
		/// Write to trace file 
		/// </summary>
		/// <param name="Message">Trace message</param>
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
#endif
