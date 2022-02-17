/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter II
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	TestPdfFileWriter main program
//	This is the test program of the PDF file writer C# Class Library
//  project. It is a windows form class. It allows the operator to
//  create two PDF files and to list all Truetype fonts available.
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

using PdfFileWriter;

namespace TestPdfFileWriter
	{
	/// <summary>
	/// Test PDF file writer demo program
	/// </summary>
	public partial class TestPdfFileWriter : Form
		{
		/// <summary>
		/// Test PDF file writer demo program constructor
		/// </summary>
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
			// program title
			Text = "PdfFileWriterII " + PdfDocument.RevisionNumber + " " + PdfDocument.RevisionDate + "-\u00a9 2013-2022 Uzi Granot";

#if DEBUG
			// debug mode
			// change current directory to work directory if exist
			string CurDir = Environment.CurrentDirectory;
			int Index = CurDir.IndexOf("bin\\Debug");
			if (Index > 0)
				{
				string WorkDir = CurDir.Substring(0, Index) + "Work";
				if (Directory.Exists(WorkDir)) Environment.CurrentDirectory = WorkDir;
				}

			// open trace file
			Trace.Open("PdfFileWriterTrace.txt");
			Trace.Write(Text);
#endif
			// make example directory
			Directory.CreateDirectory("Examples");

			// copyright box
			CopyrightTextBox.Rtf =
				"{\\rtf1\\ansi\\deff0\\deftab720{\\fonttbl{\\f0\\fswiss\\fprq2 Verdana;}}" +
				"\\par\\plain\\fs24\\b PdfFileWriter II\\plain \\fs20 \\par\\par \n" +
				"PDF File Writer II C# class library.\\par \n" +
				"Create PDF files directly from your .net application.\\par\\par \n" +
				"Revision Number: " + PdfDocument.RevisionNumber + "\\par \n" +
				"Revision Date: " + PdfDocument.RevisionDate + "\\par \n" +
				"Author: Uzi Granot\\par\\par \n" +
				"Copyright \u00a9 2013-2022 Uzi Granot. All rights reserved.\\par\\par \n" +
				"Free software distributed under the Code Project Open License (CPOL-1.02).\\par \n" +
				"As per PdfFileWriterLicense.pdf file attached to this distribution.\\par \n" +
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
				AE.Test("Examples\\ArticleExample.pdf", DebugCheckBox.Checked);
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
				OE.Test("Examples\\OtherExample.pdf", DebugCheckBox.Checked);
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
				TE.Test("Examples\\TableExample.pdf", DebugCheckBox.Checked);
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
		// PDF form Examples
		////////////////////////////////////////////////////////////////////
		private void OnAcroFields(object sender, EventArgs e)
			{
			try
				{
				// for debug only
				FormExample PTE = new FormExample();
				PTE.Test("Examples\\FormExample.pdf", DebugCheckBox.Checked);
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
				PTE.Test("Examples\\LayersExample.pdf", DebugCheckBox.Checked);
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
				PE.Test("Examples\\PrintExample.pdf", DebugCheckBox.Checked);

				// for debug only
//				ProgramTestExample PTE = new ProgramTestExample();
//				PTE.Test(DebugCheckBox.Checked, "ProgramTestExample.pdf");
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
