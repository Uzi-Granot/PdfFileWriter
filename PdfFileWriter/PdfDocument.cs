/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfDocument
//	The main class of PDF object.
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
//	Version History:
//
//	Version 1.0 2013/04/01
//		Original revision
//	Version 1.1 2013/04/09
//		Allow program to be compiled in regions that define
//		decimal separator to be non period (comma)
//	Version 1.2 2013/07/21
//		The original revision supported image resources with
//		jpeg file format only.
//		Version 1.2 support all image files acceptable to Bitmap class.
//		See ImageFormat class. The program was tested with:
//		Bmp, Gif, Icon, Jpeg, Png and Tiff.
//	Version 1.3 2014/02/07
//		Fix bug in PdfContents.DrawBezierNoP2(PointD P1, PointD P3)
//	Version 1.4 2014/03/01
//		PdfContents class
//		Add method: public void TranslateScaleRotate(double OrigX,
//			double OrigY, double ScaleX, double ScaleY, double Rotate);
//		Add method: public string ReverseString(Strint Text);
//		Fix some problems with DrawXObject(...); methods
//		PdfFont
//		Extensive changes to font substitution (see article)
//		PdfImage
//		Add method: public SizeD ImageSizeAndDensity(double Width,
//			double Height, double Density);
//		This method controls the size of the bitmap (see article)
//		Add method: public void SetImageQuality(int ImageQuality);
//		This method controls the image quality (see article)
//		PdfTilingPattern
//		Fix bug in public static PdfTilingPattern SetWeavePattern(...);
//	Version 1.5 2014/05/05
//		Barcode feature. Supported barcodes are:
//		Code-128, Code39, UPC-A, EAN-13
//	Version 1.6 2014/07/09
//		Fix FontApi unanaged code resource disposition.
//		Clear PdfDocument object after CreateFile.
//	Version 1.7 2014/08/25
//		Encryption support
//		Web link support
//		QRCode support
//		Change compression to .net System.io.compression
//	Version 1.8 2014/09/12
//		Bookmark (document outline) support
//	Version 1.9 2014/10/06
//		New features
//		Support for Microsoft Charting
//		Support for Metafile images
//		Support for image cropping
//		Support for PrintDocument output to PDF file
//		Fixs
//		Font loading. Fix the problem of missing table.
//	Version 1.9.1 2014/10/12
//		Fix decimal separator problem in regions that define
//		decimal separator to be non period (comma) in the
//		ChartExample.cs code
//	Version 1.10.0 2014/12/02
//		Support for data tables. Add source code documentation.
//		Increase maximum number of images per document.
//	Version 1.11.0 2015/01/19
//		Support for video, sound and attached files.
//	Version 1.12.0 2015/04/13
//		Page order control.
//		Rewrite of table borders and grid lines.
//	Version 1.13.0 2015/05/08
//		The resulted Pdf document can be saved to a file or to a stream.
//		Encryption support for Standard 128 mode.
//		DrawRow method of PdfTable can force a new page.
//		Image quality control.
//	Version 1.14.0 2015/06/08
//		PdfTable will split large text columns into separate pages.
//	Version 1.14.1 2015/06/09
//		PdfTableStyle fix Copy method.
//	Version 1.15.0 2015/06/17
//		PDF document information dictionary (Title, Author, Subject,
//		Keywords, Creator, Producer, Creation date, Modify date).
//		PdfImage class add support for indexed bitmap, gray image,
//		black and white image.
//	Version 1.15.1 2015/06/18
//		Remove unused source from solution explorer
//	Version 1.16.0 2015/07/27
//		Unicode support. Commit page method.
//	Version 1.16.1 2015/08/06
//		Fix problem of converting small real numbers (<0.0001) to string.
//	Version 1.16.2 2015/09/01
//		Fix problem related to undefined character.
//	Version 1.16.3 2015/09/22
//		PdfTable constructor uses current page size to calculate
//		the table area rectangle. When PdfTable creates a new page,
//		it copies page size from previous page.
//	Version 1.16.4 2015/09/30
//		Consistent use of IDisposable interface to release
//		unmanaged resources.
//	Version 1.17.0 2016/01/26
//		New features added to the library:
//		WPF graphics path drawing.
//		Elliptical arcs drawing.
//		Support for color alpha component. In other words, support for transparency or opacity.
//		Support for color bland.
//		Support for quadratic Bezier curves.
//	Version 1.17.1 2016/02/29
//		Fix PdfTable. Header column 0 is TextBox.
//	Version 1.17.2 2016/03/22
//		Fix PdfInfo. Document properties.
//	Version 1.17.3 2016/04/14
//		Fix problem with non integer font size in regions that define
//		decimal separator to be non period (comma)
//	Version 1.18.0 2016/05/24
//		Named destinations and creation of PdfFont resource.
//	Version 1.18.1 2016/06/02 (same as 1.17.3)
//		Fix problem with non integer font size in regions that define
//		decimal separator to be non period (comma)
//	Version 1.19.0 2016/06/13
//		Document links and enhanced multi media and file attachment.
//	Version 1.19.1 2016/07/27
//		Fix location marker problem.
//	Version 1.19.2 2017/08/30
//		Remove debug working directory from project settings
//	Version 1.19.3 2018/06/24
//		Fix PdfFontFile.BuildLocaTable method. Long format buffer
//		pointer initialization.
//		Fix PdfTableCell add value type of DBNull.
//	Version 1.20.0 2018/07/15
//		Modify PdfQRCode class to include optional ModuleSize argument.
//	Version 1.21.0 2019/02/06
//		Add support for PDF417 barcode.
//	Version 1.21.1 2019/02/13
//		Fix PDF417 barcode quiet zone.
//	Version 1.22.0 2019/02/18
//		Add support for sticky notes.
//	Version 1.23.0 2019/05/26
//		Add support for sticky notes.
//	Version 1.24.0 2019/06/06
//		Support for layers control of images and annotations.
//	Version 1.24.1 2019/06/20
//		Add meter as unit of measue and fix version number and data strings.
//	Version 1.25.0 2019/07/15
//		Support for collection of fonts. Support for non ascii font names
//	Version 1.26.0 2019/07/28
//		Support for Metadata and support for QRCode ECI Assignment Value
//	Version 1.27.0 2019/09/09
//		Fix for Out of Memory error when using PDF417 barcode
//		This error occurs under unusual conditions
//	Version 1.28.0 2021/03/31
//		Change internal PDF file structure to object streams and
//		cross reference streams
//
/////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace PdfFileWriter
	{
	/////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Unit of measure enumeration
	/// </summary>
	/// <remarks>
	/// User unit of measure enumeration.
	/// </remarks>
	/////////////////////////////////////////////////////////////////////
	public enum UnitOfMeasure
		{
		/// <summary>
		/// Point
		/// </summary>
		Point,

		/// <summary>
		/// Inch
		/// </summary>
		Inch,

		/// <summary>
		/// CM
		/// </summary>
		cm,

		/// <summary>
		/// MM
		/// </summary>
		mm,

		/// <summary>
		/// Meter
		/// </summary>
		m,
		}

	/////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Standard paper size enumeration 
	/// </summary>
	/////////////////////////////////////////////////////////////////////
	public enum PaperType
		{
		/// <summary>
		/// Letter
		/// </summary>
		Letter,

		/// <summary>
		/// Legal
		/// </summary>
		Legal,

		/// <summary>
		/// A3
		/// </summary>
		A3, // 297mm 420mm

		/// <summary>
		/// A4
		/// </summary>
		A4, // 210mm 297mm

		/// <summary>
		/// A5
		/// </summary>
		A5, // 148mm 210mm
		}

	/// <summary>
	/// Initial document display enumeration
	/// </summary>
	public enum InitialDocDisplay
		{
		/// <summary>
		/// Take no action
		/// </summary>
		UseNone,

		/// <summary>
		/// Display bookmarks panel
		/// </summary>
		UseBookmarks,

		/// <summary>
		/// Display thumbnail panel
		/// </summary>
		UseThumbs,

		/// <summary>
		/// Full screen
		/// </summary>
		FullScreen,

		/// <summary>
		/// Display layers panel
		/// </summary>
		UseLayers,

		/// <summary>
		/// Display attachment panel
		/// </summary>
		UseAttachments,
		}

	/////////////////////////////////////////////////////////////////////
	/// <summary>
	/// PDF document class
	/// </summary>
	/// <remarks>
	/// <para>
	/// The main class for controlling the production of the PDF document.
	/// </para>
	/// <para>
	/// Creating a PDF is a six steps process.
	/// </para>
	/// <para>
	/// Step 1: Create one document object this PdfDocument class.
	/// </para>
	/// <para>
	/// Step 2: Create resource objects such as fonts or images (i.e. PdfFont or PdfImage).
	/// </para>
	/// <para>
	/// Step 3: Create page object PdfPage.
	/// </para>
	/// <para>
	/// Step 4: Create contents object PdfContents.
	/// </para>
	/// <para>
	/// Step 5: Add text and graphics to the contents object (using PdfContents methods).
	/// </para>
	/// <para>
	/// Repeat steps 3, 4 and 5 for additional pages
	/// </para>
	/// <para>
	/// Step 6: Create your PDF document file by calling CreateFile method of PdfDocument.
	/// </para>
	/// <para>
	/// <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#DocumentCreation">For example of document creation see 3.1. Document Creation Overview</a>
	/// </para>
	/// </remarks>
	/////////////////////////////////////////////////////////////////////
	public class PdfDocument : IDisposable
		{
		/// <summary>
		/// Library revision number
		/// </summary>
		public static readonly string RevisionNumber = "1.28.0";

		/// <summary>
		/// Library revision date
		/// </summary>
		public static readonly string RevisionDate = "2021/03/31";

		/// <summary>
		/// Scale factor
		/// </summary>
		/// <remarks>
		/// From user unit of measure to points.
		/// </remarks>
		public double ScaleFactor { get; internal set; }

		/// <summary>
		/// Epsilon 1/300 of an inch in user units
		/// </summary>
		/// <remarks>
		/// Small distance for compare calculation to eliminate rounding errors.
		/// </remarks>
		public double Epsilon { get; internal set; }

		/// <summary>
		/// Initial document display
		/// </summary>
		public InitialDocDisplay InitialDocDisplay { get; set; }

		/// <summary>
		/// Page count
		/// </summary>
		/// <remarks>
		/// Current page count
		/// </remarks>
		public int PageCount
			{
			get
				{
				return PageArray.Count;
				}
			}

		/// <summary>
		/// Get page object
		/// </summary>
		/// <param name="Index">Page index (zero based)</param>
		/// <returns>PdfPage object</returns>
		public PdfPage GetPage
				(
				int Index
				)
			{
			if(Index >= 0 || Index < PageArray.Count) return PageArray[Index];
			throw new ApplicationException("GetPage invalid argument");
			}

		internal string FileName;           // PDF document file name
		internal PdfBinaryWriter PdfFile;   // PDF document file stream
		internal SizeD PageSize;            // in points
		internal List<PdfObject> ObjectArray = new List<PdfObject>(); // list of all PDF indirect objects for this document
		internal List<PdfPage> PageArray = new List<PdfPage>();
		internal PdfLayers Layers;          // Layers control
		internal PdfObject CatalogObject;   // catalog object
		internal PdfObject PagesObject;     // parent object of all pages
		internal PdfEncryption Encryption;  // encryption dictionary
		internal PdfBookmark BookmarksRoot; // bookmarks (document outline) dictionary
		internal int[] ResCodeNo = new int[(int) ResCode.Length]; // resource code next number
		internal PdfInfo InfoObject;
		internal byte[] DocumentID;         // document ID

		internal List<PdfEmbeddedFile> EmbeddedFileArray;
		internal List<PdfExtGState> ExtGStateArray;
		internal List<PdfFont> FontArray;
		internal List<PdfAnnotation> LinkAnnotArray;
		internal List<LocationMarker> LocMarkerArray;
		internal List<PdfWebLink> WebLinkArray;

		internal string[] InitDocDispText = new string[]
			{
			"/UseNone",
			"/UseOutlines",
			"/UseThumbs",
			"/FullScreen",
			"/UseOC",
			"/UseAttachments",
			};

		/// <summary>
		/// Debug flag
		/// </summary>
		/// <remarks>
		/// Debug flag. Default is false. The program will generate normal PDF file.
		/// If debug flag is true, the library will not compress contents, will replace images and font file with text place holder.
		/// The generated file can be viewed with a text editor but cannot be loaded into PDF reader.
		/// </remarks>
		public bool Debug = false;

		// translation of user units to points
		// must agree with UnitOfMeasure enumeration
		internal static double[] UnitInPoints = new double[]
			{
			1.0,			// Point
			72.0,			// Inch
			72.0 / 2.54,	// cm
			72.0 / 25.4,	// mm
			72.0 / 0.0254,  // meter
			};

		// standard paper sizes (in points)
		// must agree with PaperType enumeration
		internal static SizeD[] PaperTypeSize = new SizeD[]
			{
			new SizeD(8.5 * 72, 11.0 * 72),					// letter
			new SizeD(8.5 * 72, 14.0 * 72),					// legal
			new SizeD(29.7 * 72 / 2.54, 42.0 * 72 / 2.54),	// A3
			new SizeD(21.0 * 72 / 2.54, 29.7 * 72 / 2.54),	// A4
			new SizeD(14.8 * 72 / 2.54, 21.0 * 72 / 2.54),  // A5
			};

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// PDF document constructor.
		/// </summary>
		/// <param name="FileName">Document file name.</param>
		/// <remarks>
		/// <para>This constructor generates a document with default letter size
		/// page (height 11”, width 8.5”).</para>
		/// <para>Page orientation is portrait.</para>
		/// <para>Unit of measure is points (1/72 inch).</para>
		/// <para>Scale factor is 1.0.</para>
		/// <para>The PDF document will be saved in a file named FileName.</para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public PdfDocument
				(
				string FileName
				)
			{
			// constructor helper
			ConstructorHelper(8.5 * 72.0, 11.0 * 72.0, 1.0, FileName, null);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// PDF document constructor.
		/// </summary>
		/// <param name="Stream">File or memory stream.</param>
		/// <remarks>
		/// <para>This constructor generates a document with default letter size
		/// page (height 11”, width 8.5”).</para>
		/// <para>Page orientation is portrait.</para>
		/// <para>Unit of measure is points (1/72 inch).</para>
		/// <para>Scale factor is 1.0.</para>
		/// <para>The PDF document will be saved in the stream argument. The stream can 
		/// be either a MemoryStream or a FileStream. It is the calling program
		/// responsibiliy to close the stream after CreateFile() method
		/// is called.</para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public PdfDocument
				(
				Stream Stream
				)
			{
			// constructor helper
			ConstructorHelper(8.5 * 72.0, 11.0 * 72.0, 1.0, null, Stream);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// PDF document constructor.
		/// </summary>
		/// <param name="Width">Page Width</param>
		/// <param name="Height">Page height</param>
		/// <param name="ScaleFactor">Scale factor</param>
		/// <param name="FileName">Document file name.</param>
		/// <remarks>
		/// <para>This constructor generates a document with default page size
		/// as per width and height arguments in user units.</para>
		/// <para>Page orientation is portrait if width is less than height.
		/// Otherwise it is landscape.</para>
		/// <para>Scale factor is user unit of measure expressed in points.
		/// For example, Inch has scale factor of 72.0.</para>
		/// <para>The PDF document will be saved in a file named FileName.</para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public PdfDocument
				(
				double Width, // page width
				double Height, // page height
				double ScaleFactor, // scale factor from user units to points (i.e. 72.0 for inch)
				string FileName
				)
			{
			// constructor helper
			ConstructorHelper(ScaleFactor * Width, ScaleFactor * Height, ScaleFactor, FileName, null);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// PDF document constructor.
		/// </summary>
		/// <param name="Width">Page Width</param>
		/// <param name="Height">Page height</param>
		/// <param name="ScaleFactor">Scale factor</param>
		/// <param name="Stream">File or memory stream.</param>
		/// <remarks>
		/// <para>This constructor generates a document with default page size
		/// as per width and height arguments in user units.</para>
		/// <para>Page orientation is portrait if width is less than height.
		/// Otherwise it is landscape.</para>
		/// <para>Scale factor is user unit of measure expressed in points.
		/// For example, Inch has scale factor of 72.0.</para>
		/// <para>The PDF document will be saved in the stream argument. The stream can 
		/// be either a MemoryStream or a FileStream. It is the calling program
		/// responsibiliy to close the stream after CreateFile() method
		/// is called.</para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public PdfDocument
				(
				double Width, // page width
				double Height, // page height
				double ScaleFactor, // scale factor from user units to points (i.e. 72.0 for inch)
				Stream Stream
				)
			{
			// constructor helper
			ConstructorHelper(ScaleFactor * Width, ScaleFactor * Height, ScaleFactor, null, Stream);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// PDF document constructor
		/// </summary>
		/// <param name="Width">Page width.</param>
		/// <param name="Height">Page height.</param>
		/// <param name="UnitOfMeasure">Unit of measure code.</param>
		/// <param name="FileName">Document file name.</param>
		/// <remarks>
		/// <para>This constructor generates a document with default page size
		/// as per width and height arguments in user units.</para>
		/// <para>Page orientation is portrait if width is less than height.
		/// Otherwise it is landscape.</para>
		/// <para>Unit of measure is a an enumeration constant (Point, Inch, cm, mm, m)</para>
		/// <para>The PDF document will be saved in a file named FileName.</para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public PdfDocument
				(
				double Width, // page width
				double Height, // page height
				UnitOfMeasure UnitOfMeasure, // unit of measure: Point, Inch, cm, mm, m
				string FileName
				)
			{
			// constructor helper
			double Scale = UnitInPoints[(int) UnitOfMeasure];
			ConstructorHelper(Scale * Width, Scale * Height, Scale, FileName, null);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// PDF document constructor
		/// </summary>
		/// <param name="Width">Page width.</param>
		/// <param name="Height">Page height.</param>
		/// <param name="UnitOfMeasure">Unit of measure code.</param>
		/// <param name="Stream">File or memory stream.</param>
		/// <remarks>
		/// <para>This constructor generates a document with default page size
		/// as per width and height arguments in user units.</para>
		/// <para>Page orientation is portrait if width is less than height.
		/// Otherwise it is landscape.</para>
		/// <para>Unit of measure is a an enumeration constant (Point, Inch, cm, mm, m)</para>
		/// <para>The PDF document will be saved in the stream argument. The stream can 
		/// be either a MemoryStream or a FileStream. It is the calling program
		/// responsibiliy to close the stream after CreateFile() method
		/// is called.</para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public PdfDocument
				(
				double Width, // page width
				double Height, // page height
				UnitOfMeasure UnitOfMeasure, // unit of measure: Point, Inch, cm, mm, m
				Stream Stream
				)
			{
			// constructor helper
			double Scale = UnitInPoints[(int) UnitOfMeasure];
			ConstructorHelper(Scale * Width, Scale * Height, Scale, null, Stream);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// PDF document constructor
		/// </summary>
		/// <param name="PaperType">Paper type</param>
		/// <param name="Landscape">True for landscape, false for portrait.</param>
		/// <param name="UnitOfMeasure">Unit of measure code.</param>
		/// <param name="FileName">Document file name.</param>
		/// <remarks>
		/// <para>This constructor generates a document with default page size
		/// of Letter, Leagal, A3, A4 or A5.</para>
		/// <para>Page orientation is determined by the landscape argument.</para>
		/// <para>Unit of measure is a an enumeration constant (Point, Inch, cm, mm, m)</para>
		/// <para>The PDF document will be saved in a file named FileName.</para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public PdfDocument
				(
				PaperType PaperType,
				bool Landscape,
				UnitOfMeasure UnitOfMeasure, // unit of measure: Point, Inch, cm, mm, m
				string FileName
				)
			{
			// set scale factor (user units to points)
			double Scale = UnitInPoints[(int) UnitOfMeasure];
			double Width = PaperTypeSize[(int) PaperType].Width;
			double Height = PaperTypeSize[(int) PaperType].Height;

			// for landscape swap width and height
			if(Landscape)
				ConstructorHelper(Height, Width, Scale, FileName, null);
			else
				ConstructorHelper(Width, Height, Scale, FileName, null);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// PDF document constructor
		/// </summary>
		/// <param name="PaperType">Paper type</param>
		/// <param name="Landscape">True for landscape, false for portrait.</param>
		/// <param name="UnitOfMeasure">Unit of measure code.</param>
		/// <param name="Stream">File or memory stream.</param>
		/// <remarks>
		/// <para>This constructor generates a document with default page size
		/// of Letter, Leagal, A3, A4 or A5.</para>
		/// <para>Page orientation is determined by the landscape argument.</para>
		/// <para>Unit of measure is a an enumeration constant (Point, Inch, cm, mm, m)</para>
		/// <para>The PDF document will be saved in the stream argument. The stream can 
		/// be either a MemoryStream or a FileStream. It is the calling program
		/// responsibiliy to close the stream after CreateFile() method
		/// is called.</para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public PdfDocument
				(
				PaperType PaperType,
				bool Landscape,
				UnitOfMeasure UnitOfMeasure, // unit of measure: Point, Inch, cm, mm, m
				Stream Stream
				)
			{
			// set scale factor (user units to points)
			double Scale = UnitInPoints[(int) UnitOfMeasure];
			double Width = PaperTypeSize[(int) PaperType].Width;
			double Height = PaperTypeSize[(int) PaperType].Height;

			// for landscape swap width and height
			if(Landscape)
				ConstructorHelper(Height, Width, Scale, null, Stream);
			else
				ConstructorHelper(Width, Height, Scale, null, Stream);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Initial Object Array
		////////////////////////////////////////////////////////////////////
		private void ConstructorHelper
				(
				double Width, // page width
				double Height, // page height
				double ScaleFactor, // scale factor from user units to points (i.e. 72.0 for inch)
				string FileName, // file name or null
				Stream OutputStream // output stream or null
				)
			{
			// set scale factor (user units to points)
			this.ScaleFactor = ScaleFactor;

			// set epsilon (1/300 of an inch in user units)
			Epsilon = 72.0 / (300.0 * ScaleFactor);

			// save page default size
			PageSize = new SizeD(Width, Height);

			// create empty indirect object array list
			ObjectArray = new List<PdfObject>();

			// first object (zero index) must be an empty object
			new PdfObject(this, ObjectType.Free);

			// PDF document root object the Catalog object
			CatalogObject = new PdfObject(this, ObjectType.Dictionary, "/Catalog");

			// catalog object must be in file
			CatalogObject.XRefType = XRefObjType.InFile;

			// add viewer preferences
			CatalogObject.Dictionary.Add("/ViewerPreferences", "<</PrintScaling/None>>");

			// Parent object for all pages
			PagesObject = new PdfObject(this, ObjectType.Dictionary, "/Pages");

			// add indirect reference to pages within the catalog object
			CatalogObject.Dictionary.AddIndirectReference("/Pages", PagesObject);

			// document id
			DocumentID = RandomByteArray(16);

			// create file using file name
			if(FileName != null)
				{
				// save file name
				this.FileName = FileName;

				// constructor helper
				PdfFile = new PdfBinaryWriter(new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None));
				}

			// write to caller's file or memory stream
			else
				{
				PdfFile = new PdfBinaryWriter(OutputStream);
				}

			// write PDF version number
			PdfFile.WriteString("%PDF-1.7\n");

			// add this comment to tell compression programs that this is a binary file
			PdfFile.WriteString("%\u00b5\u00b5\u00b5\u00b5\n");

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Set encryption
		/// </summary>
		/// <remarks>
		/// The PDF File Writer library will encrypt the PDF document
		/// using AES-128 encryption. User password set to default. Owner 
		/// password is set to a random number.
		/// A PDF reader such as Acrobat will open the document with the 
		/// default user password. Permissions flags are set to allow all.
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public void SetEncryption()
			{
			SetEncryption(null, null, Permission.All, EncryptionType.Aes128);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Set encryption
		/// </summary>
		/// <param name="Permissions">Permission flags.</param>
		/// <remarks>
		/// The PDF File Writer library will encrypt the PDF document
		/// using AES-128 encryption. User password set to default. Owner 
		/// password is set to a random number.
		/// A PDF reader such as Acrobat will open the document with the 
		/// default user password. Permissions flags are set as per argument.
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public void SetEncryption
				(
				Permission Permissions
				)
			{
			SetEncryption(null, null, Permissions, EncryptionType.Aes128);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Set encryption
		/// </summary>
		/// <param name="UserPassword">User password</param>
		/// <param name="Permissions">Permission flags</param>
		/// <remarks>
		/// The PDF File Writer library will encrypt the PDF document
		/// using AES-128 encryption. User password is as per argument. Owner 
		/// password is set to a random number.
		/// A PDF reader such as Acrobat will request the user to enter a password.
		/// The document can only be opened with the user password. The owner password
		/// being random is effectively unknown.
		/// Permissions flags are set as per argument.
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public void SetEncryption
				(
				string UserPassword,
				Permission Permissions
				)
			{
			SetEncryption(UserPassword, null, Permissions, EncryptionType.Aes128);
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Set encryption
		/// </summary>
		/// <param name="UserPassword">User password</param>
		/// <param name="OwnerPassword">Owner password</param>
		/// <param name="Permissions">Permission flags</param>
		/// <param name="EncryptionType">Encryption type</param>
		/// <remarks>
		/// The PDF File Writer library will encrypt the PDF document
		/// using either AES-128 encryption or standard 128 (RC4) encryption.
		/// Encryption type is specified by the last argument. Note: the 
		/// standard 128 (RC4) is considered unsafe and should not be used.
		/// User and owner passwords are as per
		/// the two arguments. A PDF reader such as Acrobat will request the 
		/// user to enter a password. The user can supply either the user
		/// or the owner password. Permissions flags are set as per argument.
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public void SetEncryption
				(
				string UserPassword,
				string OwnerPassword,
				Permission Permissions,
				EncryptionType EncryptionType = EncryptionType.Aes128
				)
			{
			// encryption can be set only once
			if(Encryption != null) throw new ApplicationException("Encryption is already set");

			// create encryption dictionary object
			Encryption = new PdfEncryption(this, UserPassword, OwnerPassword, Permissions, EncryptionType);

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Gets bookmarks root
		/// </summary>
		/// <returns>Root bookmark object</returns>
		////////////////////////////////////////////////////////////////////
		public PdfBookmark GetBookmarksRoot()
			{
			// create bookmarks root node if this is the first time
			if(BookmarksRoot == null) BookmarksRoot = new PdfBookmark(this);

			// return bookmarks node to the user
			return BookmarksRoot;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Move page to another position
		/// </summary>
		/// <param name="SourceIndex">Page's current position</param>
		/// <param name="DestinationIndex">Page's new position</param>
		////////////////////////////////////////////////////////////////////
		public void MovePage
				(
				int SourceIndex,
				int DestinationIndex
				)
			{
			if(SourceIndex < 0 || SourceIndex >= PageCount || DestinationIndex < 0 || DestinationIndex > PageCount)
				throw new ApplicationException("Move page invalid argument");

			// there is only one page or no move
			if(DestinationIndex != SourceIndex && DestinationIndex != SourceIndex + 1)
				{
				PdfPage SourcePage = PageArray[SourceIndex];
				PageArray.RemoveAt(SourceIndex);
				if(DestinationIndex > SourceIndex) DestinationIndex--;
				PageArray.Insert(DestinationIndex, SourcePage);
				}
			return;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Create PDF document file
		/// </summary>
		/// <remarks>
		/// <para>The last step of document creation after all pages were constructed.</para>
		/// <para>If PdfDocument was constructed with a file name,
		/// the CreateFile method will close the file after the file is
		/// written to. If the PdfDocument was constructed with a stream,
		/// the CreateFile does not close the stream. It is the user application
		/// that should close the stream after the stream was used.</para>
		/// </remarks>
		////////////////////////////////////////////////////////////////////
		public void CreateFile()
			{
			// finalize file structure
			FinalizeFileStructure();

			// write all stream objects (except object streams and XRef stream) plus root and encryption
			foreach(PdfObject Obj in ObjectArray) Obj.WriteToPdfFile();

			// first object stream to object array
			PdfObject ObjStm = new PdfObject(this, ObjectType.Stream, "/ObjStm");

			// contol variables
			List<byte> StreamData = new List<byte>();
			int ObjectIndex = 0;

			// write all non stream objects to one stream objects
			for(int Index = 0; Index < ObjectArray.Count; Index++)
				{
				// shortcut
				PdfObject Obj = ObjectArray[Index];

				// stream object is already in the output file,
				if(Obj.FilePosition != 0 || Obj == ObjStm || Obj.XRefType == XRefObjType.Free) continue;

				// test for maximum 200 objects per one stream
				if(ObjectIndex == 200)
					{
					// output one full object stream
					WriteStreamObject(ObjStm, ObjectIndex, StreamData);

					// next object stream
					ObjStm = new PdfObject(this, ObjectType.Stream, "/ObjStm");
					StreamData.Clear();
					ObjectIndex = 0;
					}

				// object number and relative position
				ObjStm.ObjectValueList.AddRange(PdfDocument.FormatToByteArray("{0} {1} ", Obj.ObjectNumber, StreamData.Count));

				// add object
				if(Obj.ObjectType == ObjectType.Dictionary)
					{
					StreamData.AddRange(Obj.Dictionary.ToByteArray());
					}
				else if(Obj.ObjectType != ObjectType.Free)
					{
					// convert byte list to array
					if(Obj.ObjectValueList.Count > 0) Obj.ObjectValueArray = Obj.ObjectValueList.ToArray();

					// we have contents but no dictionary
					// write content to pdf file
					StreamData.AddRange(Obj.ObjectValueArray);
					}

				// set object stream flag
				Obj.StrmParent = ObjStm.ObjectNumber;
				Obj.StrmIndexNo = ObjectIndex;

				// update object index
				ObjectIndex++;
				}

			// output last object stream
			WriteStreamObject(ObjStm, ObjectIndex, StreamData);

			// save position
			int StrmXRefPos = (int) PdfFile.BaseStream.Position;

			// write cross reference stream
			WriteStreamXRef();

			// write PDF end of file marker
			PdfFile.WriteFormat("startxref\n{0}\n%%EOF\n", StrmXRefPos);

			// close file and dispose all open resources
			Dispose();

			// successful exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Finalize PDF file structure
		////////////////////////////////////////////////////////////////////
		internal void FinalizeFileStructure()
			{
			// add destinations to link annotation
			if(LinkAnnotArray != null) AddDestToLinkAnnot();

			// create named destinations
			if(LocMarkerArray != null) CreateNamedDestinations();

			// Optional content properties dictionary
			if(Layers != null && Layers.LayerList.Count > 0)
				{
				// create optional content dictionary
				Layers.CreateDictionary();

				// add to catalog object
				CatalogObject.Dictionary.AddIndirectReference("/OCProperties", Layers);
				}

			// create page array
			StringBuilder Kids = new StringBuilder("[");
			for(int Index = 0; Index < PageArray.Count; Index++)
				Kids.AppendFormat("{0} 0 R ", PageArray[Index].ObjectNumber);
			if(Kids.Length > 1) Kids.Length--;
			Kids.Append("]");
			PagesObject.Dictionary.Add("/Kids", Kids.ToString());

			// page count
			PagesObject.Dictionary.AddInteger("/Count", PageArray.Count);

			// page mode
			if(InitialDocDisplay != InitialDocDisplay.UseNone)
				CatalogObject.Dictionary.Add("/PageMode", InitDocDispText[(int) InitialDocDisplay]);

			// close stream objects
			for(int Index = 0; Index < ObjectArray.Count; Index++)
				{
				// shortcut
				PdfObject Obj = ObjectArray[Index];

				// write stream object if not already in the output file,
				if(Obj.FilePosition == 0) Obj.CloseObject();
				}

			return;
			}

		////////////////////////////////////////////////////////////////////
		// write stream object to output PDF file
		////////////////////////////////////////////////////////////////////
		void WriteStreamObject
				(
				PdfObject ObjStm,
				int ObjectsCount,
				List<byte> Stream
				)
			{
			// position of first object
			int First = ObjStm.ObjectValueList.Count;

			// copy stream data
			ObjStm.ObjectValueList.AddRange(Stream);

			// add N pairs and first position
			ObjStm.Dictionary.AddInteger("/N", ObjectsCount);
			ObjStm.Dictionary.AddInteger("/First", First);

			// write to file
			ObjStm.WriteToPdfFile();

			// exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// output stream cross reference
		////////////////////////////////////////////////////////////////////
		internal void WriteStreamXRef()
			{
			// create object for stream cross reference
			PdfObject StrmXRef = new PdfObject(this, ObjectType.Stream, "/XRef");

			// add /Root
			StrmXRef.Dictionary.AddIndirectReference("/Root", CatalogObject);

			// add /ID
			StrmXRef.Dictionary.AddFormat("/ID", "[{0}{0}]", ByteArrayToPdfHexString(DocumentID));

			// add /Size (number of objects)
			StrmXRef.Dictionary.AddInteger("/Size", ObjectArray.Count);

			// add /Info
			if(InfoObject != null) StrmXRef.Dictionary.AddIndirectReference("/Info", InfoObject);

			// add encryption to trailer dictionary
			if(Encryption != null) StrmXRef.Dictionary.AddIndirectReference("/Encrypt", Encryption);

			// current file position for this object
			int FilePosition = (int) PdfFile.BaseStream.Position;

			// how many bytes we need for position
			int W2;
			if(FilePosition < 65536) W2 = 2;
			else if(FilePosition < 16777216) W2 = 3;
			else W2 = 4;

			// width array
			StrmXRef.Dictionary.AddFormat("/W", "[1 {0} 1]", W2);

			// save objects position in cross reference stream
			for(int ObjectIndex = 0; ObjectIndex < ObjectArray.Count; ObjectIndex++)
				{
				// current object
				PdfObject Obj = ObjectArray[ObjectIndex];

				// field 1
				StrmXRef.ObjectValueList.Add((byte) Obj.XRefType);

				int Field2 = 0;
				int Field3 = 0;
				switch(Obj.XRefType)
					{
					case XRefObjType.Free:
						break;

					case XRefObjType.InFile:
						Field2 = (int) Obj.FilePosition;
						break;

					case XRefObjType.ObjStm:
						Field2 = Obj.StrmParent;
						Field3 = Obj.StrmIndexNo;
						break;
					}

				// field 2
				switch(W2)
					{
					case 4:
						StrmXRef.ObjectValueList.Add((byte) (Field2 >> 24));
						goto case 3;

					case 3:
						StrmXRef.ObjectValueList.Add((byte) (Field2 >> 16));
						goto case 2;

					case 2:
						StrmXRef.ObjectValueList.Add((byte) (Field2 >> 8));
						StrmXRef.ObjectValueList.Add((byte) Field2);
						break;
					}

				// field 3
				StrmXRef.ObjectValueList.Add((byte) Field3);
				}

			// disable encription for xref stream
			PdfEncryption Save = Encryption;
			Encryption = null;

			// write cross reference object to output file
			StrmXRef.WriteToPdfFile();

			// restore encryption
			Encryption = Save;

			// successful exit
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Add destinations to link annotations
		////////////////////////////////////////////////////////////////////
		internal void AddDestToLinkAnnot()
			{
			foreach(PdfAnnotation Annot in LinkAnnotArray)
				{
				// search for location marker name
				string LocMarkerName = ((AnnotLinkAction) ((PdfAnnotation) Annot).AnnotAction).LocMarkerName;
				int Index = LocMarkerArray.BinarySearch(new LocationMarker(LocMarkerName));

				// no location marker was defined for this name
				if(Index < 0) throw new ApplicationException("No location marker was defined for: " + LocMarkerName);

				// add action
				Annot.Dictionary.AddFormat("/A", "<</Type/Action/S/GoTo/D{0}>>", Annot.Document.LocMarkerArray[Index].DestStr);
				}
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Create named destinations
		////////////////////////////////////////////////////////////////////
		internal void CreateNamedDestinations()
			{
			PdfObject NamedDest = null;
			StringBuilder DestStr = null;
			foreach(LocationMarker LocMarker in LocMarkerArray)
				{
				if(LocMarker.Scope != LocMarkerScope.NamedDest) continue;
				if(NamedDest == null)
					{
					NamedDest = new PdfObject(this);
					DestStr = new StringBuilder("[");
					}
				DestStr.AppendFormat("{0}{1}", TextToPdfString(LocMarker.LocMarkerName, NamedDest), LocMarker.DestStr);
				}
			if(NamedDest == null) return;

			// add one dictionary entry
			DestStr.Append("]");
			NamedDest.Dictionary.Add("/Names", DestStr.ToString());

			// attach it to PDF catalog
			CatalogObject.Dictionary.AddFormat("/Names", "<</Dests {0} 0 R>>", NamedDest.ObjectNumber);
			return;
			}

		////////////////////////////////////////////////////////////////////
		// Convert byte array to PDF string
		// used for document id and encryption
		////////////////////////////////////////////////////////////////////
		internal string ByteArrayToPdfHexString
				(
				byte[] ByteArray
				)
			{
			// convert to hex string
			StringBuilder HexText = new StringBuilder("<");
			for(int index = 0; index < ByteArray.Length; index++) HexText.AppendFormat("{0:x2}", (int) ByteArray[index]);
			HexText.Append(">");
			return HexText.ToString();
			}

		////////////////////////////////////////////////////////////////////
		// C# string text to PDF strings only
		////////////////////////////////////////////////////////////////////
		internal string TextToPdfString
				(
				string Text,
				PdfObject Parent
				)
			{
			// convert C# string to byte array
			byte[] ByteArray = TextToByteArray(Text);

			// encryption is active. PDF string must be encrypted except for encryption dictionary
			if(Encryption != null && Parent != null && Encryption != Parent && Parent.XRefType == XRefObjType.InFile)
				ByteArray = Encryption.EncryptByteArray(Parent.ObjectNumber, ByteArray);

			// convert byte array to PDF string format
			return ByteArrayToPdfString(ByteArray);
			}

		////////////////////////////////////////////////////////////////////
		// C# string text to byte array
		// This method is used for PDF strings only
		////////////////////////////////////////////////////////////////////
		internal byte[] TextToByteArray
				(
				string Text
				)
			{
			// scan input text for Unicode characters and for non printing characters
			bool Unicode = false;
			foreach(char TestChar in Text)
				{
				// test for non printable characters
				if(TestChar < ' ' || TestChar > '~' && TestChar < 160)
					throw new ApplicationException("Text string must be made of printable characters");

				// test for Unicode string
				if(TestChar > 255) 	Unicode = true;
				}

			// declare output byte array
			byte[] ByteArray = null;

			// all characters are one byte long
			if(!Unicode)
				{
				// save each imput character in one byte
				ByteArray = new byte[Text.Length];
				int Index = 0;
				foreach(char TestChar in Text) ByteArray[Index++] = (byte) TestChar;
				}

			// Unicode case. we have some two bytes characters
			else
				{
				// allocate output byte array
				ByteArray = new byte[2 * Text.Length + 2];

				// add Unicode marker at the start of the string
				ByteArray[0] = 0xfe;
				ByteArray[1] = 0xff;

				// save each character as two bytes
				int Index = 2;
				foreach(char TestChar in Text)
					{
					ByteArray[Index++] = (byte) (TestChar >> 8);
					ByteArray[Index++] = (byte) TestChar;
					}
				}

			// return output byte array
			return ByteArray;
			}

		////////////////////////////////////////////////////////////////////
		// format short string to byte array 
		////////////////////////////////////////////////////////////////////
		internal static byte[] FormatToByteArray
				(
				string FormatStr,
				params object[] List
				)
			{
			// string format
			return ToByteArray(string.Format(FormatStr, List));
			}

		////////////////////////////////////////////////////////////////////
		// format short string to byte array 
		////////////////////////////////////////////////////////////////////
		internal static byte[] ToByteArray
				(
				string Str
				)
			{
			// byte array
			byte[] ByteArray = new byte[Str.Length];

			// convert content from string to binary
			// do not use Encoding.ASCII.GetBytes(...)
			for(int Index = 0; Index < ByteArray.Length; Index++) ByteArray[Index] = (byte) Str[Index];
			return ByteArray;
			}

		////////////////////////////////////////////////////////////////////
		// byte array to PDF string
		// This method is used for PDF strings only
		////////////////////////////////////////////////////////////////////

		internal string ByteArrayToPdfString
				(
				byte[] ByteArray
				)
			{
			// create output string with open and closing parenthesis
			StringBuilder Str = new StringBuilder("(");
			foreach(byte TestByte in ByteArray)
				{
				// CR and NL must be replaced by \r and \n
				// Otherwise PDF readers will convert CR or NL or CR-NL to NL
				if(TestByte == '\r') Str.Append("\\r");
				else if(TestByte == '\n') Str.Append("\\n");

				// the three characters \ ( ) must be preceded by \
				else
					{
					if(TestByte == (byte) '\\' || TestByte == (byte) '(' || TestByte == (byte) ')') Str.Append('\\');
					Str.Append((char) TestByte);
					}
				}
			Str.Append(')');
			return Str.ToString();
			}

		////////////////////////////////////////////////////////////////////
		// Create random byte array
		////////////////////////////////////////////////////////////////////
		internal static byte[] RandomByteArray
				(
				int Length
				)
			{
			byte[] ByteArray = new byte[Length];
#if NET6_0_OR_GREATER
			using (RandomNumberGenerator RandNumGen = RandomNumberGenerator.Create())
				{
				RandNumGen.GetBytes(ByteArray);
				}
#else
			using (RNGCryptoServiceProvider RandNumGen = new RNGCryptoServiceProvider())
				{
				RandNumGen.GetBytes(ByteArray);
				}
#endif
			return ByteArray;
			}

		////////////////////////////////////////////////////////////////////
		// Generate unique resource number
		////////////////////////////////////////////////////////////////////
		internal string GenerateResourceNumber
				(
				char Code       // one letter code for each type of resource
				)
			{
			// create resource code
			return string.Format("/{0}{1}", Code, ++ResCodeNo[PdfObject.ResCodeLetter.IndexOf(Code)]);
			}

		////////////////////////////////////////////////////////////////////
		// Compress byte array
		////////////////////////////////////////////////////////////////////
		internal static bool CompressStream
				(
				ref byte[] InputBuf
				)
			{
			// input length
			int InputLen = InputBuf.Length;

			// input buffer too small to compress
			if(InputLen < 16) return false;

			// create output memory stream to receive the compressed buffer
			MemoryStream OutputStream = new MemoryStream();

			// deflate compression object
			DeflateStream Deflate = new DeflateStream(OutputStream, CompressionMode.Compress, true);

			// load input buffer into the compression class
			Deflate.Write(InputBuf, 0, InputBuf.Length);

			// compress, flush and close
			Deflate.Close();

			// compressed file length
			int OutputLen = (int) OutputStream.Length;

			// make sure compressed stream is shorter than input stream
			if(OutputLen + 6 >= InputLen) return false;

			// create output buffer
			byte[] OutputBuf = new byte[OutputLen + 6];

			// write two bytes in most significant byte first
			OutputBuf[0] = (byte) 0x78;
			OutputBuf[1] = (byte) 0x9c;

			// copy the compressed result
			OutputStream.Seek(0, SeekOrigin.Begin);
			OutputStream.Read(OutputBuf, 2, OutputLen);
			OutputStream.Close();

			// reset adler32 checksum
			uint ReadAdler32 = Adler32Checksum(InputBuf);

			// ZLib checksum is Adler32 write it big endian order, high byte first
			OutputLen += 2;
			OutputBuf[OutputLen++] = (byte) (ReadAdler32 >> 24);
			OutputBuf[OutputLen++] = (byte) (ReadAdler32 >> 16);
			OutputBuf[OutputLen++] = (byte) (ReadAdler32 >> 8);
			OutputBuf[OutputLen] = (byte) ReadAdler32;

			// successful exit
			InputBuf = OutputBuf;
			return true;
			}

		/////////////////////////////////////////////////////////////////////
		// Accumulate Adler Checksum
		/////////////////////////////////////////////////////////////////////
		internal static uint Adler32Checksum
				(
				byte[] Buffer
				)
			{
			const uint Adler32Base = 65521;

			// split current Adler checksum into two 
			uint AdlerLow = 1; // AdlerValue & 0xFFFF;
			uint AdlerHigh = 0; // AdlerValue >> 16;

			int Len = Buffer.Length;
			int Pos = 0;
			while(Len > 0)
				{
				// We can defer the modulo operation:
				// Under worst case the starting value of the two halves is 65520 = (AdlerBase - 1)
				// each new byte is maximum 255
				// The low half grows AdlerLow(n) = AdlerBase - 1 + n * 255
				// The high half grows AdlerHigh(n) = (n + 1)*(AdlerBase - 1) + n * (n + 1) * 255 / 2
				// The maximum n before overflow of 32 bit unsigned integer is 5552
				// it is the solution of the following quadratic equation
				// 255 * n * n + (2 * (AdlerBase - 1) + 255) * n + 2 * (AdlerBase - 1 - uint.MaxValue) = 0
				int n = Len < 5552 ? Len : 5552;
				Len -= n;
				while(--n >= 0)
					{
					AdlerLow += (uint) Buffer[Pos++];
					AdlerHigh += AdlerLow;
					}
				AdlerLow %= Adler32Base;
				AdlerHigh %= Adler32Base;
				}
			return (AdlerHigh << 16) | AdlerLow;
			}

		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Dispose PDF document object
		/// </summary>
		////////////////////////////////////////////////////////////////////
		public void Dispose()
			{
			// close output file
			// Note: stream input will not be closed
			if(FileName != null && PdfFile != null)
				{
				PdfFile.Close();
				PdfFile = null;
				}

			// dispose all objects with IDisposable interface
			foreach(PdfObject Obj in ObjectArray)
				if(Obj is IDisposable) ((IDisposable) Obj).Dispose();
			return;
			}
		}
	}
