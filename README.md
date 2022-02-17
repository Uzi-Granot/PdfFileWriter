# PdfFileWriter
The PDF File Writer C# class library gives .NET applications the ability to produce PDF documents. The library shields the application from the details of the PDF file structure. The library supports: forms, text, images, tables, graphics, barcodes, web links, charts, sticky notes, encryption and more.

For documentation and further information visit [PDF File Writer C# Class Library (Version 2.0.0)](https://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version-2-0-0)

## Upgrade
PdfFileWriter 1.28.0 projects and before are not compatible with PdfFileWriter 2.0.0, however, they are close enough to allow for conversion with some effort. The following list shows most of the main changes.
- All class names start with `Pdf`.
- All methods related to drawing text strings are controled by `PdfDrawTextCtrl`.
- All methods related to drawing barcodes are controled by `PdfDrawBarcodeCtrl`.
- All methods related to drawing rectangles, rounded rectangles, inverted rounded rectangles, and ovals controled by `PdfDrawCtrl`.
- Creating image resources was changed from multiple constructors to one constructor and setting fields and methods.
- All annotation derived classes were rewriten.

## History
- 2013/04/01: Version 1.0 Original Version.
- 2013/04/09: Version 1.1 Support for countries with decimal separator other than period.
- 2013/07/21: Version 1.2 The original revision supported image resources with jpeg file format only. Version 1.2 support all image files acceptable to Bitmap class. See ImageFormat class. The program was tested with: Bmp, Gif, Icon, Jpeg, Png and Tiff. See Section 2.3 and Section 3.8 above.
- 2014/02/07: Version 1.3 Fix bug in PdfContents.DrawBezierNoP2(PointD P1, PointD P3).
- 2014/03/01: Version 1.4 Improved support for character substitution. Improved support for image inclusion. Some fixes related to PdfXObject.
- 2014/05/05: Version 1.5 Barcode support without use of fonts. Four barcodes are included: Code-128, Code-39, UPC-A and EAN-13. See Section 2.5 and Section 3.7.
- 2014/07/09: Version 1.6 (1) The CreateFile method resets the PdfDocument to initial condition after file creation. (2) The PdfFont object releases the unmanaged code resources properly.
- 2014/08/25: Version 1.7 Support for document encryption, web-links and QR Code.
- 2014/09/12: Version 1.8 Support for bookmarks.
- 2014/10/06: Version 1.9 Support for charting, PrintDocument and image Metafiles.
- 2014/10/12: Version 1.9.1 Fix to ChartExample. Parse numeric fields in regions with decimal separator other than period.
- 2014/12/02: Version 1.10.0 Support for data tables. Add source code documentation. Increase maximum number of images per document.
- 2015/01/12: Version 1.11.0 Support for video, sound, and attachment files. Add support for Interleave 2 of 5 barcode.
- 2015/04/13: Version 1.12.0 Support for reordering pages and enhance data table border lines support.
- 2015/05/05: Version 1.13.0 PDF document output to a stream. PDF table insert page break. Image quality enhancement. Support for Standard-128 (RC4) encryption.
- 2015/06/08: Version 1.14.0 Support for long text blocks or TextBox within PDF Table.
- 2015/06/09: Version 1.14.1 one-line change to Copy method of PdfTableStyle class.
- 2015/06/17: Version 1.15.0 Document information dictionary. PdfImage rewrite. Additional image saving options.
- 2015/06/18: Version 1.15.1 Remove unused source from solution explorer.
- 2015/07/27: Version 1.16.0 Unicode support. Commit page method.
- 2015/08/07: Version 1.16.1 Fix for small (<0.0001) real numbers conversion to string.
- 2015/09/01: Version 1.16.2 Fix for undefined characters. The selected font does not support characters used.
- 2015/09/22: Version 1.16.3 PdfTable constructor uses current page size to calculate the default table area rectangle. When PdfTable starts a new page the page type and orientation is taken from the previous page.
- 2015/09/30: Version 1.16.4 Consistent use of IDisposable interface to release unmanaged resources.
- 2016/01/26: Version 1.17.0 WPF graphics, transparency, color blending, elliptical arcs and quadratic Bezier curves.
- 2016/02/29: Version 1.17.1 PdfTable will display headers properly when the first column header is a TextBox.
- 2016/03/22: Version 1.17.2 PdfInfo PDF documents properties will be displayed properly.
- 2016/04/14: Version 1.17.3 Fix problem with non-integer font size in regions that define decimal separator to be non-period (comma).
- 2016/05/24: Version 1.18.0 Named destinations and creation of PdfFont resource.
- 2016/06/02: Version 1.18.1 Re-apply 1.17.3 fix.
- 2016/06/13: Version 1.19.0 Document links. Changes to named destination. Interactive features support to TextBox and PdfTable.
- 2016/07/27: Version 1.19.1 Fix: AddLocationMarker fix for regions with decimal separator not period.
- 2017/08/31: Version 1.19.2 Fix: Debug working directory is not saved as part of the project
- 2018/06/26: Version 1.19.3 Fix PdfFontFile.BuildLocaTable method. Long format buffer pointer initialization. Fix PdfTableCell add value type of DBNull.
- 2018/07/15: Version 1.20.0 Modify QR Code support by adding number of pixels per module.
- 2019/02/06: Version 1.21.0 Support for PDF417 barcode.
- 2019/02/13: Version 1.21.1 Fix for PDF417 barcode quiet zone.
- 2019/02/18: Version 1.22.0 Support for sticky notes.
- 2019/05/26: Version 1.23.0 Support for layers and changes to QRCode and Pdf417 barcode.
- 2019/06/06: Version 1.24.0 Support for layers control of images and annotations.
- 2019/06/20: Version 1.24.1 Support for meter as unit of measure.
- 2019/07/15: Version 1.25.0 Support for font collections (mainly CJK fonts) and for non-ASCII font names.
- 2019/07/28: Version 1.26.0 Support for XMP Metadata and QR Code ECI Assignment Number.
- 2020/09/09: Version 1.27.0 Fix out of memory problem related to PDF417 barcode. The problem only occurred under unusual circumstances.
- 2021/03/31: Version 1.28.0 Upgrade of the internal file structure to include object streams and cross reverence streams.
- 2022/02/01: Version 2.0.0 PDF File Writer II Version.