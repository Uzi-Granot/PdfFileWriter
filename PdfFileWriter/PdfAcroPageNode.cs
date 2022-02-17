/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Acro fields page node
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
	/// List of all fields on one page
	/// </summary>
	public class PdfAcroPageNode : PdfObject
		{
		internal int PageNo;
		internal List<string> FieldNames;
		internal List<int> FieldObjectNo;
		internal List<string> RadioButtonGroupNames;
		internal List<PdfAcroRadioButtonGroup> RadioButtonGroups;

		/// <summary>
		/// List of acro fields for one page
		/// </summary>
		/// <param name="Document">PDF parent document</param>
		internal PdfAcroPageNode
				(
				PdfDocument Document
				) : base(Document)
			{
			FieldNames = new List<string>();
			FieldObjectNo = new List<int>();
			return;
			}

		/// <summary>
		/// Add acro field except radio button field
		/// </summary>
		/// <param name="Field">Acro widget field</param>
		internal void AddField
				(
				PdfAcroWidgetField Field
				)
			{
			// test for duplicate field name on the same page
			int Index;
			for(Index = 0; Index < FieldNames.Count && FieldNames[Index] != Field.FieldName; Index++);

			// we have a duplicate 
			if(Index < FieldNames.Count)
				{
				throw new ApplicationException("Duplicate field name on the same page");
				}

			// add the field in user order
			FieldObjectNo.Add(Field.ObjectNumber);
			FieldNames.Add(Field.FieldName);

			// add /Parent to field
			Field.Dictionary.AddIndirectReference("/Parent", this);
			return;
			}

		internal void AddRadioButtonField
				(
				PdfAcroRadioButton RadioButtonWidget
				)
			{
			// short cut for group button name
			string GroupName = RadioButtonWidget.FieldName;

			// test for group name on the this page
			// note: FieldNames is for all types of fields
			int Index;
			for(Index = 0; Index < FieldNames.Count && FieldNames[Index] != GroupName; Index++);

			// first time for this group of buttons name 
			if(Index == FieldNames.Count)
				{
				// this is the first radio button on this page
				if(RadioButtonGroupNames == null)
					{ 
					// radio buttons group names
					RadioButtonGroupNames = new List<string>();
					RadioButtonGroups = new List<PdfAcroRadioButtonGroup>();
					}

				// add field name as a group name
				RadioButtonGroupNames.Add(GroupName);

				// create radio button field with no widget annotation 
				PdfAcroRadioButtonGroup RadioButtonGroup = new PdfAcroRadioButtonGroup(Document, GroupName);

				// add radio button group
				RadioButtonGroups.Add(RadioButtonGroup);

				// add /Parent to field
				RadioButtonGroup.Dictionary.AddIndirectReference("/Parent", this);

				// add radio button to radio button group
				RadioButtonGroup.AddRadioButton(RadioButtonWidget);

				// add the button group name to the names of all fields in this page in user order
				FieldObjectNo.Add(RadioButtonGroup.ObjectNumber);
				FieldNames.Add(GroupName);
				}

			// second or more button of this group on this page
			else
				{ 
				// test for group name on this page
				for(Index = 0; Index < RadioButtonGroupNames.Count && RadioButtonGroupNames[Index] != GroupName; Index++);

				if(Index == RadioButtonGroupNames.Count) throw new ApplicationException("Program error radio button group");

				// add radio button to radio button group
				RadioButtonGroups[Index].AddRadioButton(RadioButtonWidget);
				}
			return;
			}

		/// <summary>
		/// close object before writing to PDF file
		/// </summary>
		internal override void CloseObject()
			{ 
			// Kids array
			StringBuilder KidsStr = new StringBuilder("[");

			// loop for all page nodes
			for(int Index = 0; Index < FieldObjectNo.Count; Index++)
				{ 
				// add first page fields object to acro form fields array
				KidsStr.AppendFormat("{0} 0 R ", FieldObjectNo[Index]);
				}

			// save kids array			
			KidsStr[^1] = ']';
			Dictionary.Add("/Kids", KidsStr.ToString());

			// partial name
			Dictionary.AddPdfString("/T", string.Format("Page{0}", PageNo));
			return;
			}
		}
	}
