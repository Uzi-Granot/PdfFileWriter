/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PdfWebLink
//	PDF weblink class. 
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
	/// PDF Weblink class
	/// </summary>
	/// <remarks>
	/// <para>
	/// The library will make sure that all weblinks in the PDF file are unique.
	/// To create a weblink class you must use a static menthod. This method will
	/// create a new object for a new weblink. The mothod will return an 
	/// existing object if it is a duplicate.
	/// </para>
	/// </remarks>
	public class PdfWebLink : PdfObject, IComparable<PdfWebLink>
		{
		internal string WebLinkStr;

		// for search only
		private PdfWebLink
				(
				string WebLinkStr
				)
			{
			// save string
			this.WebLinkStr = WebLinkStr;

			// exit
			return;
			}

		// create new web link
		private PdfWebLink
				(
				PdfDocument Document,
				string WebLinkStr
				) : base(Document)
			{
			// save string
			this.WebLinkStr = WebLinkStr;

			// type of action uniform resource identifier
			Dictionary.Add("/S", "/URI");

			// uniform resource identifier
			Dictionary.AddPdfString("/URI", WebLinkStr);

			// exit
			return;
			}

		/// <summary>
		/// Add a weblink
		/// </summary>
		/// <param name="Document">PDF document</param>
		/// <param name="WebLinkStr">Weblink text</param>
		/// <returns>Weblink object</returns>
		/// <remarks>
		/// <para>
		/// The library will make sure that all weblinks in the PDF file are unique.
		/// To create a weblink class you must use a static menthod. This method will
		/// create a new object for a new weblink. The mothod will return an 
		/// existing object if it is a duplicate.
		/// </para>
		/// </remarks>
		public static PdfWebLink Create
				(
				PdfDocument Document,
				string WebLinkStr
				)
			{
			// first time
			if(Document.WebLinkArray == null) Document.WebLinkArray = new List<PdfWebLink>();

			// search list for a duplicate
			int Index = Document.WebLinkArray.BinarySearch(new PdfWebLink(WebLinkStr));

			// this string is a duplicate
			if(Index >= 0) return Document.WebLinkArray[Index];

			// new link
			PdfWebLink WebLink = new PdfWebLink(Document, WebLinkStr);

			// save new string in array
			Document.WebLinkArray.Insert(~Index, WebLink);

			// exit
			return WebLink;
			}

		/// <summary>
		/// Compare two WebLinkStr objects.
		/// </summary>
		/// <param name="Other">Other object.</param>
		/// <returns>Compare result.</returns>
		public int CompareTo
				(
				PdfWebLink Other
				)
			{
			return string.Compare(WebLinkStr, Other.WebLinkStr);
			}
		}
	}
