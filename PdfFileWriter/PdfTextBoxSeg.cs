/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	TextBox
//  Support class for PdfContents class. Format text to fit column.
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
	/// TextBox line segment class
	/// </summary>
	public class PdfTextBoxSeg : PdfDrawTextCtrl
		{
		/// <summary>
		/// Gets segment width.
		/// </summary>
		public double SegWidth { get; internal set; }

		/// <summary>
		/// Gets segment space character count.
		/// </summary>
		public int SpaceCount { get; internal set; }

		/// <summary>
		/// Gets segment text.
		/// </summary>
		public string Text { get; internal set; }

		/// <summary>
		/// Text box segment constructor.
		/// </summary>
		/// <param name="TextCtrl">Segment text control.</param>
		public PdfTextBoxSeg
				(
				PdfDrawTextCtrl TextCtrl
				) : base(TextCtrl)
			{
			Justify = TextJustify.Left;
			Text = string.Empty;
			return;
			}

		/// <summary>
		/// Text box segment copy constructor.
		/// </summary>
		/// <param name="Other">Segment text control.</param>
		public PdfTextBoxSeg
				(
				PdfTextBoxSeg Other
				) : base(Other)
			{
			Justify = TextJustify.Left;
			Text = string.Empty;
			Annotation = null;

			// if there is annotation, a copy must be created
			if(Other.Annotation != null)
				{
				if(Other.Annotation.GetType() == typeof(PdfAnnotWebLink))
					{
					Annotation = new PdfAnnotWebLink((PdfAnnotWebLink) Other.Annotation);
					}
				else if(Other.Annotation.GetType() == typeof(PdfAnnotLinkAction))
					{
					Annotation = new PdfAnnotLinkAction((PdfAnnotLinkAction) Other.Annotation);
					}
				else if(Other.Annotation.GetType() == typeof(PdfAnnotStickyNote))
					{
					Annotation = new PdfAnnotStickyNote((PdfAnnotStickyNote) Other.Annotation);
					}
				else if(Other.Annotation.GetType() == typeof(PdfAnnotFileAttachment))
					{
					Annotation = new PdfAnnotFileAttachment((PdfAnnotFileAttachment) Other.Annotation);
					}
				else if(Other.Annotation.GetType() == typeof(PdfAnnotDisplayMedia))
					{
					Annotation = new PdfAnnotDisplayMedia((PdfAnnotDisplayMedia) Other.Annotation);
					}
				else
					{
					throw new ApplicationException("Annotation copy error");
					}
				}

			return;
			}
		}
	}
