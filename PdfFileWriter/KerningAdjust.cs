/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	KerningAdjust class
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
	public class KerningAdjust
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
		public KerningAdjust
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
