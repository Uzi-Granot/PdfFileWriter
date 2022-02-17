/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Acro radio button group of fields
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
	/// Radio buttons group class
	/// </summary>
	public class PdfAcroRadioButtonGroup : PdfObject
		{
		/// <summary>
		/// Field partial name (/T)
		/// </summary>
		internal string GroupName;

		/// <summary>
		/// List of all buttons in this group
		/// </summary>
		internal List<PdfAcroRadioButton> RadioButtonsList;

		internal PdfAcroRadioButtonGroup
				(
				PdfDocument Document,
				string GroupName
				) : base(Document)
			{
			// save group/field name
			this.GroupName = GroupName;

			// field type is button
			Dictionary.AddName("/FT", "Btn");

			// field type is radio button
			Dictionary.AddInteger("/Ff", (int) (FieldFlag.Radio | FieldFlag.NoToggleToOff));

			// partial field name
			Dictionary.AddPdfString("/T", GroupName);

			// create an empty list of all buttons of this group (PdfAcroRadioButton)
			RadioButtonsList = new List<PdfAcroRadioButton>();
			return;
			}

		internal void AddRadioButton
				(
				PdfAcroRadioButton RadioButtonWidget
				)
			{
			// add the button in user order
			RadioButtonsList.Add(RadioButtonWidget);

			// add /Parent to field
			RadioButtonWidget.Dictionary.AddIndirectReference("/Parent", this);
			return;
			}

		/// <summary>
		/// close object before writing to PDF file
		/// </summary>
		internal override void CloseObject()
			{ 
			//if(RadioButtonsList.Count < 2) throw new ApplicationException("Radio buttons " + GroupName + " group must have at least 2 buttons");

			int SelectedIndex = -1;

			// Kids array
			StringBuilder KidsStr = new StringBuilder("[");

			// loop for all page nodes
			for(int Index = 0; Index < RadioButtonsList.Count; Index++)
				{
				// save first selected button
				if(RadioButtonsList[Index].Check && SelectedIndex < 0) SelectedIndex = Index;

				// add first page fields object to acro form fields array
				KidsStr.AppendFormat("{0} 0 R ", RadioButtonsList[Index].ObjectNumber);
				}

			// save kids array			
			KidsStr[^1] = ']';
			Dictionary.Add("/Kids", KidsStr.ToString());

			// button was selected
			if(SelectedIndex >= 0)
				{
				// field value
				Dictionary.AddName("/V", RadioButtonsList[SelectedIndex].OnStateName);

				// Alternate field name (/TU)
				if(RadioButtonsList[SelectedIndex].AlternateName != null) Dictionary.AddPdfString("/TU", RadioButtonsList[SelectedIndex].AlternateName);

				// Mapping field name (/TM)
				if(RadioButtonsList[SelectedIndex].MappingName != null) Dictionary.AddPdfString("/TM", RadioButtonsList[SelectedIndex].MappingName);
				}
			return;
			}
		}
	}
