/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	TestPdfFileWriter main program
//	This is the test program of the PDF file writer C# Class Library
//  project. It is a windows form class. It allows the operator to
//  create two PDF files and to list all Truetype fonts available.
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

using PdfFileWriter;
using System;
using System.IO;
using System.Windows.Forms;

namespace TestPdfFileWriter
	{
	public partial class TestPdfFileWriter : Form
		{
		public TestPdfFileWriter()
			{
			InitializeComponent();
			}

		private void OnLoad
				(
				object sender,
				EventArgs e
				)
			{
			Text = "PdfFileWriter-Revision " + PdfDocument.RevisionNumber + " " + PdfDocument.RevisionDate + "-\u00a9 2013-2019 Uzi Granot";

			#if DEBUG
			// current directory
			string CurDir = Environment.CurrentDirectory;
			string WorkDir = CurDir.Replace("bin\\Debug", "Work");
			if(WorkDir != CurDir && Directory.Exists(WorkDir)) Environment.CurrentDirectory = WorkDir;

			// open trace file
			Trace.Open("PdfFileWriterTrace.txt");

			// program title
			Trace.Write(Text);
			#endif

			// copyright box
			CopyrightTextBox.Rtf =
				"{\\rtf1\\ansi\\deff0\\deftab720{\\fonttbl{\\f0\\fswiss\\fprq2 Verdana;}}" +
				"\\par\\plain\\fs24\\b PdfFileWriter\\plain \\fs20 \\par\\par \n" +
				"PDF File Writer C# class library.\\par \n" +
				"Create PDF files directly from your .net application.\\par\\par \n" +
				"Revision Number: " + PdfDocument.RevisionNumber + "\\par \n" +
				"Revision Date: " + PdfDocument.RevisionDate + "\\par \n" +
				"Author: Uzi Granot\\par\\par \n" +
				"Copyright \u00a9 2013-2019 Uzi Granot. All rights reserved.\\par\\par \n" +
				"Free software distributed under the Code Project Open License (CPOL) 1.02.\\par \n" +
				"As per PdfFileWriterReadmeAndLicense.pdf file attached to this distribution.\\par \n" +
				"You must read and agree with the terms specified to use this program.}";

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Article example
		////////////////////////////////////////////////////////////////////

		private void OnArticleExample
				(
				object sender,
				EventArgs e
				)
			{
			try
				{
				ArticleExample AE = new ArticleExample();
				AE.Test(DebugCheckBox.Checked, "ArticleExample.pdf");
				return;
				}

			catch (Exception Ex)
				{
				// error exit
				string[] ExceptionStack = ExceptionReport.GetMessageAndStack(Ex);
				MessageBox.Show(this, "PDF Document creation falied\n" + ExceptionStack[0] + "\n" + ExceptionStack[1],
					"PDFDocument Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
				}
			}

		////////////////////////////////////////////////////////////////////
		// Other example
		////////////////////////////////////////////////////////////////////

		private void OnOtherExample
				(
				object sender,
				EventArgs e
				)
			{
			try
				{
				OtherExample OE = new OtherExample();
				OE.Test(DebugCheckBox.Checked, "OtherExample.pdf");
				return;
				}

			catch (Exception Ex)
				{
				// error exit
				string[] ExceptionStack = ExceptionReport.GetMessageAndStack(Ex);
				MessageBox.Show(this, "PDF Document creation falied\n" + ExceptionStack[0] + "\n" + ExceptionStack[1],
					"PDFDocument Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
				}
			}

		////////////////////////////////////////////////////////////////////
		// Chart Example
		////////////////////////////////////////////////////////////////////

		private void OnChartExample(object sender, EventArgs e)
			{
			try
				{
				ChartExample CE = new ChartExample();
				CE.Test(DebugCheckBox.Checked, "ChartExample.pdf");
				return;
				}

			catch (Exception Ex)
				{
				// error exit
				string[] ExceptionStack = ExceptionReport.GetMessageAndStack(Ex);
				MessageBox.Show(this, "PDF Document creation falied\n" + ExceptionStack[0] + "\n" + ExceptionStack[1],
					"PDFDocument Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
				}
			}

		////////////////////////////////////////////////////////////////////
		// Print Example
		////////////////////////////////////////////////////////////////////

		private void OnPrintExample(object sender, EventArgs e)
			{
			try
				{
				PrintExample PE = new PrintExample();
				PE.Test(DebugCheckBox.Checked, "PrintExample.pdf");
				return;
				}

			catch (Exception Ex)
				{
				// error exit
				string[] ExceptionStack = ExceptionReport.GetMessageAndStack(Ex);
				MessageBox.Show(this, "PDF Document creation falied\n" + ExceptionStack[0] + "\n" + ExceptionStack[1],
					"PDFDocument Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
				}
			}

		////////////////////////////////////////////////////////////////////
		// Layers Example
		////////////////////////////////////////////////////////////////////

		private void OnLayersExample(object sender, EventArgs e)
			{
			try
				{
				LayersExample PTE = new LayersExample();
				PTE.Test(DebugCheckBox.Checked, "LayersExample.pdf");
				return;
				}

			catch (Exception Ex)
				{
				// error exit
				string[] ExceptionStack = ExceptionReport.GetMessageAndStack(Ex);
				MessageBox.Show(this, "PDF Document creation falied\n" + ExceptionStack[0] + "\n" + ExceptionStack[1],
					"PDFDocument Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
				}

			}

		////////////////////////////////////////////////////////////////////
		// Table Example
		////////////////////////////////////////////////////////////////////

		private void OnTableExample(object sender, EventArgs e)
			{
			try
				{
				TableExample TE = new TableExample();
				TE.Test(DebugCheckBox.Checked, "TableExample.pdf");
				return;
				}

			catch (Exception Ex)
				{
				// error exit
				string[] ExceptionStack = ExceptionReport.GetMessageAndStack(Ex);
				MessageBox.Show(this, "PDF Document creation falied\n" + ExceptionStack[0] + "\n" + ExceptionStack[1],
					"PDFDocument Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
				}
			}

		////////////////////////////////////////////////////////////////////
		// Display font families
		////////////////////////////////////////////////////////////////////

		private void OnFontFamilies
				(
				object sender,
				EventArgs e
				)
			{
			EnumFontFamilies Dialog = new EnumFontFamilies();
			Dialog.ShowDialog();
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Debug check mark is on
		////////////////////////////////////////////////////////////////////
		private void OnDebugCheck(object sender, EventArgs e)
			{
			if(DebugCheckBox.Checked) MessageBox.Show("WARNING\r\n" +
				"You have checked the Debug check box.\r\n" +
				"The example PDF file will be made for debugging\r\n" +
				"and not for viewing.");
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Program is closing
		////////////////////////////////////////////////////////////////////

		private void OnClosing
				(
				object sender,
				FormClosingEventArgs e
				)
			{
			#if DEBUG
			Trace.Write("PDF file writer is closing");
			#endif
			return;
			}
		}
	}
