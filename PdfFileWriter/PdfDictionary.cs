/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	PdfDictionary
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
	internal enum ValueType
		{
		Other,
		String,
		Dictionary
		}

	/// <summary>
	/// PDF dictionary class
	/// </summary>
	/// <remarks>
	/// <para>
	/// Dictionary key value pair class. Holds one key value pair.
	/// </para>
	/// </remarks>
	public class PdfDictionary
		{
		internal List<PdfKeyValue> KeyValue;
		internal PdfObject Parent;

		internal PdfDictionary
				(
				PdfObject Parent
				)
			{
			if(Parent == null) throw new ApplicationException("PdfDictionary must have PdfObject as parent");
			this.Parent = Parent;
			KeyValue = new List<PdfKeyValue>();
			return;
			}

		internal void SetParent
				(
				PdfObject Parent
				)
			{
			this.Parent = Parent;
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Find key value pair in dictionary.
		// return index number or -1 if not found.
		////////////////////////////////////////////////////////////////////
		internal int Find
				(
				string Key      // key (first character must be forward slash /)
				)
			{
			// look through the dictionary
			for(int Index = 0; Index < KeyValue.Count; Index++) if(KeyValue[Index].Key == Key) return Index;

			// not found
			return -1;
			}

		////////////////////////////////////////////////////////////////////
		// Add key value pair to dictionary.
		// The value is string format
		////////////////////////////////////////////////////////////////////
		internal void Add
				(
				string Key, // key (first character must be forward slash /)
				string Str
				)
			{
			Add(Key, Str, ValueType.Other);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Add key value pair to dictionary.
		// The value is string format
		////////////////////////////////////////////////////////////////////
		internal void AddName
				(
				string Key, // key (first character must be forward slash /)
				string Str
				)
			{
			Add(Key, "/" + Str, ValueType.Other);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Add key value pair to dictionary.
		// The value is string format
		////////////////////////////////////////////////////////////////////
		internal void AddInteger
				(
				string Key, // key (first character must be forward slash /)
				int Value
				)
			{
			Add(Key, Value.ToString(), ValueType.Other);
			return;
			}

/*
		////////////////////////////////////////////////////////////////////
		// Add key value pair to dictionary.
		// The value is string format
		////////////////////////////////////////////////////////////////////
		internal void AddUnsignedInteger
				(
				string Key, // key (first character must be forward slash /)
				uint Value
				)
			{
			Add(Key, Value.ToString(), ValueType.Other);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Add key value pair to dictionary.
		// The value is string format
		////////////////////////////////////////////////////////////////////
		internal void AddLongInteger
				(
				string Key, // key (first character must be forward slash /)
				long Value
				)
			{
			Add(Key, Value.ToString(), ValueType.Other);
			return;
			}
*/
		////////////////////////////////////////////////////////////////////
		// Add key value pair to dictionary.
		// The value is string format
		////////////////////////////////////////////////////////////////////
		internal void AddReal
				(
				string Key, // key (first character must be forward slash /)
				double Real
				)
			{
			if(Math.Abs(Real) < 0.0001) Real = 0;
			Add(Key, string.Format(NFI.PeriodDecSep, "{0}", (float) Real), ValueType.Other);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Add key value pair to dictionary.
		// The value is string format
		////////////////////////////////////////////////////////////////////
		internal void AddReal
				(
				string Key, // key (first character must be forward slash /)
				float Real
				)
			{
			if(Math.Abs(Real) < 0.0001) Real = 0;
			Add(Key, string.Format(NFI.PeriodDecSep, "{0}", Real), ValueType.Other);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Add key value pair to dictionary.
		// The value is string format
		////////////////////////////////////////////////////////////////////
		internal void AddRectangle
				(
				string Key, // key (first character must be forward slash /)
				PdfRectangle Rect
				)
			{
			if(Parent == null) throw new ApplicationException("Add rectangle. Parent undefined");
			Add(Key, string.Format(NFI.PeriodDecSep, "[{0} {1} {2} {3}]",
				Parent.ToPt(Rect.Left), Parent.ToPt(Rect.Bottom), Parent.ToPt(Rect.Right), Parent.ToPt(Rect.Top)));
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Add key value pair to dictionary.
		// The value is string format
		////////////////////////////////////////////////////////////////////
		internal void AddBoolean
				(
				string Key, // key (first character must be forward slash /)
				bool Bool
				)
			{
			Add(Key, Bool ? "true" : "false", ValueType.Other);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Add key value pair to dictionary.
		// The value is string format
		////////////////////////////////////////////////////////////////////
		internal void AddPdfString
				(
				string Key, // key (first character must be forward slash /)
				string Str
				)
			{
			Add(Key, Str, ValueType.String);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Add key value pair to dictionary.
		// The value is string format
		////////////////////////////////////////////////////////////////////
		internal void AddFormat
				(
				string Key, // key (first character must be forward slash /)
				string FormatStr,
				params object[] FormatList
				)
			{
			Add(Key, string.Format(NFI.PeriodDecSep, FormatStr, FormatList), ValueType.Other);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Add key value pair to dictionary.
		// The value is a reference to indirect object number.
		////////////////////////////////////////////////////////////////////
		internal void AddIndirectReference
				(
				string Key, // key (first character must be forward slash /)
				PdfObject Obj // PdfObject. The method creates an indirect reference "n 0 R" to the object.
				)
			{
			Add(Key, string.Format("{0} 0 R", Obj.ObjectNumber), ValueType.Other);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Add key value pair to dictionary.
		// The value is a reference to indirect object number.
		////////////////////////////////////////////////////////////////////
		internal void AddRefNo
				(
				string Key, // key (first character must be forward slash /)
				int RefNo // The method creates an indirect reference "n 0 R" to the object.
				)
			{
			Add(Key, string.Format("{0} 0 R", RefNo), ValueType.Other);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Add key value pair to dictionary.
		// If dictionary does not exist, create it.
		// If key is not found, add the pair as new entry.
		// If key is found, replace old pair with new one.
		////////////////////////////////////////////////////////////////////
		internal void AddDictionary
				(
				string Key, // key (first character must be forward slash /)
				PdfDictionary Value // value
				)
			{
			Add(Key, Value, ValueType.Dictionary);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Add key value pair to dictionary.
		// If dictionary does not exist, create it.
		// If key is not found, add the pair as new entry.
		// If key is found, replace old pair with new one.
		////////////////////////////////////////////////////////////////////
		internal PdfDictionary GetOrAddDictionary
				(
				string Key // key (first character must be forward slash /)
				)
			{
			// search for existing key
			int Index = Find(Key);

			// found
			if(Index >= 0) return (PdfDictionary) KeyValue[Index].Value;

			// not found - create a dictionary and add it
			PdfDictionary Dict = new PdfDictionary(Parent);
			AddDictionary(Key, Dict);
			return Dict;
			}

		////////////////////////////////////////////////////////////////////
		// Add key value pair to dictionary.
		// If key value does not exist, create it.
		// If key is not found, add the pair as new entry.
		// If key is found, replace old pair with new one.
		////////////////////////////////////////////////////////////////////
		private void Add
				(
				string Key, // key (first character must be forward slash /)
				object Value, // value
				ValueType Type // value type
				)
			{
			// search for existing key
			int Index = Find(Key);

			// not found - add new pair
			if(Index < 0)
				{
				KeyValue.Add(new PdfKeyValue(Key, Value, Type));
				}

			// found replace value
			else
				{
				KeyValue[Index].Value = Value;
				KeyValue[Index].Type = Type;
				}

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Get value from the dictionary
		// Return string if key is found, null if not
		////////////////////////////////////////////////////////////////////
		internal PdfKeyValue GetKeyValue
				(
				string Key // key (first character must be forward slash /)
				)
			{
			int Index = Find(Key);
			return Index >= 0 ? KeyValue[Index] : null;
			}

		////////////////////////////////////////////////////////////////////
		// Remove key value pair from dictionary
		////////////////////////////////////////////////////////////////////
		internal void Remove
				(
				string Key      // key (first character must be forward slash /)
				)
			{
			int Index = Find(Key);
			if(Index >= 0) KeyValue.RemoveAt(Index);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Write dictionary to PDF file
		// Called from WriteObjectToPdfFile to output a dictionary
		////////////////////////////////////////////////////////////////////
		internal byte[] ToByteArray()
			{
			int EolMarker = 100;
			StringBuilder Str = new StringBuilder();

			BuildDictionary(Str, ref EolMarker);

			// byte array
			byte[] ByteArray = new byte[Str.Length];

			// convert content from string to binary
			// do not use Encoding.ASCII.GetBytes(...)
			for(int Index = 0; Index < ByteArray.Length; Index++) ByteArray[Index] = (byte) Str[Index];
			return ByteArray;
			}

		private void BuildDictionary(StringBuilder Str, ref int EolMarker)
			{
			Str.Append("<<");

			// output dictionary
			foreach(PdfKeyValue KeyValueItem in KeyValue)
				{
				// add new line to cut down very long lines (just appearance)
				if(Str.Length > EolMarker)
					{
					Str.Append("\n");
					EolMarker = Str.Length + 100;
					}

				// append the key
				Str.Append(KeyValueItem.Key);

				// dictionary type
				switch(KeyValueItem.Type)
					{
					// dictionary
					case ValueType.Dictionary:
						((PdfDictionary) KeyValueItem.Value).BuildDictionary(Str, ref EolMarker);
						break;

					// PDF string special case
					case ValueType.String:
						Str.Append(Parent.TextToPdfString((string) KeyValueItem.Value, Parent));
						break;

					// all other key value pairs
					default:
						// add one space between key and value unless value starts with a clear separator
						char FirstChar = ((string) KeyValueItem.Value)[0];
						if(FirstChar != '/' && FirstChar != '[' && FirstChar != '<' && FirstChar != '(') Str.Append(' ');

						// add value
						Str.Append(KeyValueItem.Value);
						break;
					}
				}

			// terminate dictionary
			Str.Append(">>");
			return;
			}
		}

	internal class PdfKeyValue
		{
		internal string Key; // key first character must be forward slash ?
		internal object Value; // value associated with key
		internal ValueType Type; // value is a PDF string

		////////////////////////////////////////////////////////////////////
		// Constructor
		////////////////////////////////////////////////////////////////////
		internal PdfKeyValue
				(
				string Key, // key first character must be forward slash ?
				object Value, // value associated with key
				ValueType Type // value type
				)
			{
			if(Key[0] != '/') throw new ApplicationException("Dictionary key must start with /");
			this.Key = Key;
			this.Value = Value;
			this.Type = Type;
			return;
			}
		}
	}
