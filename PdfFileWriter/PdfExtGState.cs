/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PdfExtGState
//	External graphics state dictionary.
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
	internal class PdfExtGState : PdfObject, IComparable<PdfExtGState>
		{
		internal string Key;
		internal string Value;

		// search constructor
		internal PdfExtGState
				(
				string Key,
				string Value
				)
			{
			// save value
			this.Key = Key;
			this.Value = Value;

			// exit
			return;
			}

		// object constructor
		internal PdfExtGState
				(
				PdfDocument Document,
				string Key,
				string Value
				) : base(Document, ObjectType.Dictionary, "/ExtGState")
			{
			// save value
			this.Key = Key;
			this.Value = Value;

			// create resource code
			ResourceCode = Document.GenerateResourceNumber('G');
			return;
			}

		internal static PdfExtGState CreateExtGState
				(
				PdfDocument Document,
				string Key,
				string Value
				)
			{
			if(Document.ExtGStateArray == null) Document.ExtGStateArray = new List<PdfExtGState>();

			// search list for a duplicate
			int Index = Document.ExtGStateArray.BinarySearch(new PdfExtGState(Key, Value));

			// this value is a duplicate
			if(Index >= 0) return Document.ExtGStateArray[Index];

			// new blend object
			PdfExtGState ExtGState = new PdfExtGState(Document, Key, Value);

			// save new string in array
			Document.ExtGStateArray.Insert(~Index, ExtGState);

			// update dictionary
			ExtGState.Dictionary.Add(Key, Value);

			// exit
			return ExtGState;
			}

		/// <summary>
		/// Compare two PdfExtGState objects.
		/// </summary>
		/// <param name="Other">Other object.</param>
		/// <returns>Compare result.</returns>
		public int CompareTo
				(
				PdfExtGState Other
				)
			{
			int Cmp = string.Compare(this.Key, Other.Key);
			if(Cmp != 0) return Cmp;
			return string.Compare(this.Value, Other.Value);
			}
		}
	}
