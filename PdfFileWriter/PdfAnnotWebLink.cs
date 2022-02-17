/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Annotation web link
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
	/// Web link annotation action
	/// </summary>
	public class PdfAnnotWebLink : PdfAnnotation
		{
		internal PdfWebLink WebLink;

		/// <summary>
		/// Web link constructor
		/// </summary>
		/// <param name="Document">PDF document</param>
		/// <param name="WebLinkStr">Web link string</param>
		public PdfAnnotWebLink
				(
				PdfDocument Document,
				string WebLinkStr
				) : base(Document, "/Link")
			{
			// encode unicode characters
			StringBuilder OutputLink = new StringBuilder();
			foreach(char Chr in WebLinkStr)
				{
				if(Chr <= ' ') OutputLink.AppendFormat("%{0:x2}", (int) Chr);
				else if(Chr <= '~') OutputLink.Append(Chr);
				else if(Chr <= 255) OutputLink.AppendFormat("%{0:x2}", (int) Chr);
				else
					{
					byte[] UtfBytes = Encoding.UTF8.GetBytes(Chr.ToString());
					foreach(byte Byte in UtfBytes)
						{
						OutputLink.AppendFormat("%{0:x2}", (int) Byte);
						}
					}
				}

			// save normalized web link
			WebLink = PdfWebLink.Create(base.Document, OutputLink.ToString());

			// add web link to annotation dictionary
			Dictionary.AddIndirectReference("/A", WebLink);
			return;
			}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="Other">Annotation to copy</param>
		public PdfAnnotWebLink
				(
				PdfAnnotWebLink Other
				) : base(Other.Document, "/Link")
			{
			// save location marker
			WebLink = Other.WebLink;

			// add web link to annotation dictionary
			Dictionary.AddIndirectReference("/A", WebLink);

			// call base create copy
			base.CreateCopy(Other);
			return;
			}
		}
	}
