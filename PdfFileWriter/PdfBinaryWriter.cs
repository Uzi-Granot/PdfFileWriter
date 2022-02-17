/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PdfBinaryWriter
//	Extension to standard C# BinaryWriter class.
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
	/// PDF binary writer class
	/// </summary>
	/// <remarks>
	/// Extends .NET BinaryWriter class.
	/// </remarks>
	public class PdfBinaryWriter : BinaryWriter
		{
		/// <summary>
		/// PDF binary writer constructor
		/// </summary>
		/// <param name="Stream">File or memory stream</param>
		public PdfBinaryWriter
				(
				Stream Stream
				) : base(Stream, Encoding.UTF8) {}

		/// <summary>
		/// Write String.
		/// </summary>
		/// <param name="Str">Input string</param>
		/// <remarks>
		/// Convert each character from two bytes to one byte.
		/// </remarks>
		public void WriteString
				(
				string Str
				)
			{
			// write to pdf file
			Write(PdfByteArrayMethods.ToByteArray(Str));
			return;
			}

		/// <summary>
		/// Write String.
		/// </summary>
		/// <param name="Str">Input string</param>
		/// <remarks>
		/// Convert each character from two bytes to one byte.
		/// </remarks>
		public void WriteString
				(
				StringBuilder Str
				)
			{
			// write to pdf file
			Write(PdfByteArrayMethods.ToByteArray(Str.ToString()));
			return;
			}

		/// <summary>
		/// Combine format string with write string.
		/// </summary>
		/// <param name="FormatStr">Standard format string</param>
		/// <param name="List">Array of objects</param>
		public void WriteFormat
				(
				string FormatStr,
				params object[] List
				)
			{
			// write to pdf file
			Write(PdfByteArrayMethods.ToByteArray(string.Format(FormatStr, List)));
			return;
			}
		}
	}
