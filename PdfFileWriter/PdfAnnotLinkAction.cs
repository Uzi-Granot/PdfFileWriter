/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Annotation link action
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
	/// Link to location marker within the document
	/// </summary>
	public class PdfAnnotLinkAction : PdfAnnotation
		{
		internal string LocMarkerName { get; set; }

		/// <summary>
		/// Go to annotation action constructor
		/// </summary>
		/// <param name="Document">PDF document</param>
		/// <param name="LocMarkerName">Location marker name</param>
		public PdfAnnotLinkAction
				(
				PdfDocument Document,
				string LocMarkerName
				) : base(Document, "/Link")
			{
			// save location marker
			this.LocMarkerName = LocMarkerName;

			// create a list of location links
			if(base.Document.LinkAnnotArray == null) base.Document.LinkAnnotArray = new List<PdfAnnotation>();
			base.Document.LinkAnnotArray.Add(this);
			return;
			}

		/// <summary>
		/// Duplicate annotation link action
		/// </summary>
		/// <param name="Other">Original link action</param>
		public PdfAnnotLinkAction
				(
				PdfAnnotLinkAction Other
				) : base(Other.Document, "/Link")
			{
			// save location marker
			LocMarkerName = Other.LocMarkerName;

			// create a list of location links
			if(Document.LinkAnnotArray == null) Document.LinkAnnotArray = new List<PdfAnnotation>();
			Document.LinkAnnotArray.Add(this);

			// copy base values
			base.CreateCopy(Other);
			return;
			}
		}
	}
