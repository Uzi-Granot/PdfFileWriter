/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	Number Format Information static class
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
//	Version History:
//
//	Version 1.0 2013/04/01
//		Original revision
/////////////////////////////////////////////////////////////////////

using System.Globalization;

namespace PdfFileWriter
	{
	/////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Number Format Information static class
	/// </summary>
	/// <remarks>
	/// Adobe readers expect decimal separator to be a period.
	/// Some countries define decimal separator as a comma.
	/// The project uses NFI.DecSep to force period for all regions.
	/// </remarks>
	/////////////////////////////////////////////////////////////////////
	public static class NFI
		{
		/// <summary>
		/// Define period as number decimal separator.
		/// </summary>
		/// <remarks>
		/// NumberFormatInfo is used with string formatting to set the
		/// decimal separator to a period regardless of region.
		/// </remarks>
		public static NumberFormatInfo PeriodDecSep { get; private set; }

		// static constructor
		static NFI()
			{
			// number format (decimal separator is period)
			PeriodDecSep = new NumberFormatInfo();
			PeriodDecSep.NumberDecimalSeparator = ".";
			return;
			}
		}
	}
