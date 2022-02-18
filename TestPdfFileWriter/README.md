# PdfFileWriterII

# Version 2.0.0 2022/02/01

The PDF File Writer II C# class library allows you to create PDF files directly from your .NET application. 
The library shields you from the details of the PDF file structure. This version of the PDF File Writer 
library was developed using Visual Studio 2022.

Please note these settings of the PdfFileWriter library properties:

Target framework: .NET 6.0

Target OS: Windows

Platform target: Any CPU

Nullable: Disable

Implicit global using: checked

Documentation file: checked

Package license: SPDX License Expression

License expression CPOL-1.02

Require license acceptance: not-checked

The repository is made of Visual Studio 2022 solution with two projects. A C# class library project and 
a test or demo project. If you want to create PDF documents from your project, you need to include the 
library with your code or include all the library sources with your application.

This project is a major revision of the original PDF File Writer published at Code Project website. 
Most of the source code is very similar. However, the two libraries are not compatible. There were 
many changes to classes and methods naming and calling sequences. For  further information of the 
original project visit 
<a href="https://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version">
PDF File Writer C# Class Library (Version 1.28.0)</a>

The most significant change to the library is the addition of PDF Forms.

The folder Examples has a number of PDF files showing the power of this library. The test and demo program 
TestPdfFileWriter produces these PDF files. All the source code is included. If you are looking for a 
particular feature, look through these examples. Once you find a close match go to the relevant source 
code to see how it is done.

I am working on rewriting the original article at Code Project, but it is not ready for publication yet.

# Hello PDF File Writer program

A very simple example of creating a PDF document with one line of text and one image.

<pre>
// create PDF document
public void CreatePdfDocument()
  {
  // Create empty document
  using(PdfDocument Document = new PdfDocument(PaperType.Letter, false, UnitOfMeasure.Inch, &quot;HelloPdfDocument.pdf&quot;))
    {
    // Add new page
    PdfPage Page = new PdfPage(Document);

    // Add contents to page
    PdfContents Contents = new PdfContents(Page);

    // create font
    PdfFont ArialNormal = PdfFont.CreatePdfFont(Document, &quot;Arial&quot;, FontStyle.Regular, true);
    PdfDrawTextCtrl TextCtrl = new PdfDrawTextCtrl(ArialNormal, 18.0);
				
    // draw text
    TextCtrl.Justify = TextJustify.Center;
    Contents.DrawText(TextCtrl, 4.5, 7, &quot;Hello PDF Document&quot;);

    // load image
    PdfImage Image = new PdfImage(Document);
    Image.LoadImage(&quot;..\\..\\..\\HappyFace.jpg&quot;);

    // draw image
    PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
    DrawCtrl.Paint = DrawPaint.Fill;
    DrawCtrl.BackgroundTexture = Image;
    Contents.DrawGraphics(DrawCtrl, new PdfRectangle(3.5, 4.8, 5.5, 6.8));

    // create pdf file
    Document.CreateFile();
    }

  // start default PDF reader and display the file
  Process Proc = new Process();
  Proc.StartInfo = new ProcessStartInfo(&quot;HelloPdfDocument.pdf&quot;) { UseShellExecute = true };
  Proc.Start();
  }
  </pre>
