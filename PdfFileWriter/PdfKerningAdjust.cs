/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PdfKerningAdjust class
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
	/////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Kerning adjustment class
	/// </summary>
	/// <remarks>
	/// Text position adjustment for TJ operator.
	/// The adjustment is for a font height of one point.
	/// Mainly used for font kerning.
	/// </remarks>
	/////////////////////////////////////////////////////////////////////
	public class PdfKerningAdjust
		{
		/// <summary>
		/// Gets or sets Text
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets adjustment
		/// </summary>
		/// <remarks>
		/// Adjustment units are in PDF design unit. To convert to user units: Adjust * FontSize / (1000.0 * ScaleFactor)
		/// </remarks>
		public double Adjust { get; set; }

		/// <summary>
		/// Kerning adjustment constructor
		/// </summary>
		/// <param name="Text">Text</param>
		/// <param name="Adjust">Adjustment</param>
		public PdfKerningAdjust
				(
				string Text,
				double Adjust
				)
			{
			this.Text = Text;
			this.Adjust = Adjust;
			return;
			}
		}
	}
