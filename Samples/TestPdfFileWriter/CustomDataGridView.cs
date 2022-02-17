/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter II
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	CustomDataGridView
//	Custom DataGridView class.
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

namespace TestPdfFileWriter
	{
	/// <summary>
	/// Custom data grid view
	/// </summary>
	public class CustomDataGridView : DataGridView
		{
		/// <summary>
		/// Custom data grid view constructor
		/// </summary>
		/// <param name="ParentForm">Parent windows form</param>
		/// <param name="Center">Alignment is middle center</param>
		public CustomDataGridView
				(
				Form ParentForm,
				bool Center
				)
			{
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
			for(int Col = 0; Col < Columns.Count - 1; Col++)
				Columns[Col].Width = ColWidth;
			return;
			}

		/// <summary>
		/// user pressed view button to display character 
		/// </summary>
		/// <returns>Selection object</returns>
		public object GetSelection()
			{
			DataGridViewSelectedRowCollection Rows = SelectedRows;
			return (Rows == null || Rows.Count == 0) ? null : Rows[0].Tag;
			}
		}
	}
