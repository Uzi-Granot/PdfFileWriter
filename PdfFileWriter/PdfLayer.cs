/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfLayer
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

using System;

namespace PdfFileWriter
	{
	/// <summary>
	/// Lock/unlock layer enumeration
	/// </summary>
	public enum LockLayer
		{
		/// <summary>
		/// Unlock layer (default)
		/// </summary>
		Unlocked,

		/// <summary>
		/// Lock layer
		/// </summary>
		Locked,
		}

	/// <summary>
	/// Layer state
	/// </summary>
	public enum LayerState
		{
		/// <summary>
		/// Layer state is ON
		/// </summary>
		On,

		/// <summary>
		/// Layer state is OFF
		/// </summary>
		Off,
		}

	/// <summary>
	/// PdfLayer class
	/// </summary>
	public class PdfLayer : PdfObject, IComparable<PdfLayer>
		{
		/// <summary>
		/// Layer name
		/// </summary>
		public string Name {get; private set;}

		/// <summary>
		/// Layer locked or unlocked
		/// </summary>
		public LockLayer Locked {get; set;}

		/// <summary>
		/// Initial layer state (on or off)
		/// </summary>
		public LayerState State {get; set;}

		/// <summary>
		/// Layer is a radio button
		/// </summary>
		public string RadioButton {get; set;}

		internal PdfLayers LayersParent;

		/// <summary>
		/// Layer constructor
		/// </summary>
		/// <param name="LayersParent">Layers parent</param>
		/// <param name="Name">Layer's name</param>
		public PdfLayer
				(
				PdfLayers LayersParent,
				string Name
				) : base(LayersParent.Document, ObjectType.Dictionary, "/OCG")
			{
			// save arguments
			this.Name = Name;

			// save layers parent
			this.LayersParent = LayersParent;

			// create resource code
			ResourceCode = Document.GenerateResourceNumber('O');

			// add layer name to the dictionary
			Dictionary.AddPdfString("/Name", Name);

			// add to the list of all layers
			LayersParent.LayerList.Add(this);

			// exit
			return;
			}

		/// <summary>
		/// CompareTo for IComparabler
		/// </summary>
		/// <param name="Other">Other layer</param>
		/// <returns>Compare result</returns>
		public int CompareTo
				(
				PdfLayer Other
				)
			{
			int Cmp = string.Compare(RadioButton, Other.RadioButton);
			if(Cmp != 0) return Cmp;
			return ObjectNumber - Other.ObjectNumber;
			}
		}
	}
