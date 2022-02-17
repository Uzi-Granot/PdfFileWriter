/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PdfTypeOneFontInfo
//  Support class for type one font information.
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
	/// Type one font information class
	/// </summary>
	public class PdfTypeOneFontInfo
		{
		/// <summary>
		/// Font name
		/// </summary>
		public string FontName;

		/// <summary>
		/// Font bounding box left side
		/// </summary>
		public int BBoxLeft;

		/// <summary>
		/// Font bounding box bottom side
		/// </summary>
		public int BBoxBottom;

		/// <summary>
		/// Font bounding box right side
		/// </summary>
		public int BBoxRight;

		/// <summary>
		/// Font bounding box top side
		/// </summary>
		public int BBoxTop;

		/// <summary>
		/// Characters array of information
		/// </summary>
		public short[,] CharInfo;

		/// <summary>
		/// Type one font info constructor
		/// </summary>
		/// <param name="FontName">Font name</param>
		/// <param name="BBoxLeft">Bounding box left</param>
		/// <param name="BBoxBottom">Bounding box bottom</param>
		/// <param name="BBoxRight">Bounding box right</param>
		/// <param name="BBoxTop">Bounding box top</param>
		/// <param name="CharInfo">Characters information array</param>
		public PdfTypeOneFontInfo
				(
				string FontName,
				int BBoxLeft,
				int BBoxBottom,
				int BBoxRight,
				int BBoxTop,
				short[,] CharInfo
				)
			{
			this.FontName = FontName;
			this.BBoxLeft = BBoxLeft;
			this.BBoxBottom = BBoxBottom;
			this.BBoxRight = BBoxRight;
			this.BBoxTop = BBoxTop;
			this.CharInfo = CharInfo;
			return;
			}

		/// <summary>
		/// Character width
		/// </summary>
		/// <param name="Chr">Character code</param>
		/// <returns>Character width</returns>
		public int CharWidth
				(
				char Chr
				)
			{
			return CharInfo[(int) Chr, 0];
			}

		/// <summary>
		/// Character bounding box left side
		/// </summary>
		/// <param name="Chr">Character code</param>
		/// <returns>Bounding box left</returns>
		public int CharBBoxLeft
				(
				char Chr
				)
			{
			return CharInfo[(int) Chr, 1];
			}

		/// <summary>
		/// Character bounding box bottom side
		/// </summary>
		/// <param name="Chr">Character code</param>
		/// <returns>Bounding box bottom</returns>
		public int CharBBoxBottom
				(
				char Chr
				)
			{
			return CharInfo[(int) Chr, 2];
			}

		/// <summary>
		/// Character bounding box right side
		/// </summary>
		/// <param name="Chr">Character code</param>
		/// <returns>Bounding box right</returns>
		public int CharBBoxRight
				(
				char Chr
				)
			{
			return CharInfo[(int) Chr, 3];
			}

		/// <summary>
		/// Character bounding box top side
		/// </summary>
		/// <param name="Chr">Character code</param>
		/// <returns>Bounding box top</returns>
		public int CharBBoxTop
				(
				char Chr
				)
			{
			return CharInfo[(int) Chr, 4];
			}
		}
	}
