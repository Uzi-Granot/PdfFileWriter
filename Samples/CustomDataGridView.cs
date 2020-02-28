/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	CustomDataGridView
//	Custom DataGridView class.
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
using System.Drawing;
using System.Windows.Forms;

namespace TestPdfFileWriter
{
public class CustomDataGridView : DataGridView
	{
	////////////////////////////////////////////////////////////////////
	// constructor
	////////////////////////////////////////////////////////////////////

	public CustomDataGridView
			(
			Form	ParentForm,
			bool	Center
			)
		{
//		this.ParentForm = ParentForm;

		Name = "DataGrid";
		AllowUserToAddRows = false;
		AllowUserToDeleteRows = false;
		AllowUserToOrderColumns = true;
		AllowUserToResizeRows = false;
		RowHeadersVisible = false;
		MultiSelect = false;
		SelectionMode = DataGridViewSelectionMode.FullRowSelect;
		BackgroundColor = SystemColors.GradientInactiveCaption;
		BorderStyle = BorderStyle.Fixed3D;
		ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
		ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		EditMode = DataGridViewEditMode.EditProgrammatically;
		AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
		TabStop = true;
		DefaultCellStyle.Alignment = Center ? DataGridViewContentAlignment.MiddleCenter : DataGridViewContentAlignment.MiddleLeft;
		DefaultCellStyle.WrapMode = DataGridViewTriState.False;
		ColumnHeadersDefaultCellStyle.Alignment = Center ? DataGridViewContentAlignment.MiddleCenter : DataGridViewContentAlignment.MiddleLeft;
		Resize += new EventHandler(OnResize);

		// add the data grid to the list of controls of the parent form
		ParentForm.Controls.Add(this);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// On resize, divide columns width
	////////////////////////////////////////////////////////////////////

	private void OnResize
			(
			object sender,
			EventArgs e
			)
		{
		int ColWidth = (ClientSize.Width - SystemInformation.VerticalScrollBarWidth) / Columns.Count;
		for(int Col = 0; Col < Columns.Count - 1; Col++) Columns[Col].Width = ColWidth;
		return;
		}

	////////////////////////////////////////////////////////////////////
	// user pressed view button to display character
	////////////////////////////////////////////////////////////////////

	public object GetSelection()
		{
		DataGridViewSelectedRowCollection Rows = SelectedRows;
		return (Rows == null || Rows.Count == 0) ? null : Rows[0].Tag;
		}
	}
}
