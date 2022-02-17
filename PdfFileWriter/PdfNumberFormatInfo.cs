/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Number Format Information static class
//	Adobe readers expect decimal separator to be a period.
//	Some countries define decimal separator as a comma.
//	The project uses NFI.DecSep to force period for all regions.
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
