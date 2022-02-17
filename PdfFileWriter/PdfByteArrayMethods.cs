/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Byte array methods
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

using System.Security.Cryptography;
using System.Text;

namespace PdfFileWriter
	{
	/// <summary>
	/// Class to manipulate byte array
	/// </summary>
	public static class PdfByteArrayMethods
		{
		////////////////////////////////////////////////////////////////////
		// Convert byte array to PDF string
		// used for document id and encryption
		////////////////////////////////////////////////////////////////////
		internal static string ByteArrayToPdfHexString
				(
				byte[] ByteArray
				)
			{
			// convert to hex string
			StringBuilder HexText = new StringBuilder("<");
			for(int index = 0; index < ByteArray.Length; index++) HexText.AppendFormat("{0:x2}", (int) ByteArray[index]);
			HexText.Append(">");
			return HexText.ToString();
			}

		////////////////////////////////////////////////////////////////////
		// format short string to byte array 
		////////////////////////////////////////////////////////////////////
		internal static byte[] FormatToByteArray
				(
				string FormatStr,
				params object[] List
				)
			{
			// string format
			return ToByteArray(string.Format(FormatStr, List));
			}

		////////////////////////////////////////////////////////////////////
		// format short string to byte array 
		////////////////////////////////////////////////////////////////////
		internal static byte[] ToByteArray
				(
				string Str
				)
			{
			// byte array
			byte[] ByteArray = new byte[Str.Length];

			// convert content from string to binary
			// do not use Encoding.ASCII.GetBytes(...)
			for(int Index = 0; Index < ByteArray.Length; Index++) ByteArray[Index] = (byte) Str[Index];
			return ByteArray;
			}

		////////////////////////////////////////////////////////////////////
		// Create random byte array
		////////////////////////////////////////////////////////////////////
		internal static byte[] RandomByteArray
				(
				int Length
				)
			{
			byte[] ByteArray = new byte[Length];
//			using(RNGCryptoServiceProvider RandNumGen = new RNGCryptoServiceProvider())
			using(RandomNumberGenerator RandNumGen = RandomNumberGenerator.Create())
				{
				RandNumGen.GetBytes(ByteArray);
				}
			return ByteArray;
			}

		}
	}
