/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfImage
//	PDF Image resource.
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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace PdfFileWriter
{
	/// <summary>
	/// PDF Image class
	/// </summary>
	/// <remarks>
	/// <para>
	/// For more information go to <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#ImageSupport">2.4 Image Support</a>
	/// </para>
	/// <para>
	/// <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#DrawImage">For example of drawing image see 3.9. Draw Image and Clip it</a>
	/// </para>
	/// </remarks>
	public class PdfImage : PdfObject, IDisposable
	{
	/// <summary>
	/// Gets image width in pixels
	/// </summary>
	public int WidthPix {get; internal set;}	// in pixels

	/// <summary>
	/// Gets image height in pixels
	/// </summary>
	public int HeightPix {get; internal set;}	// in pixels

	internal PdfImageControl ImageControl;
	internal Rectangle ImageRect;
	internal Bitmap Picture;
	internal bool DisposePicture;
	internal bool DisposeImage;
	internal bool[,] BWImage;

	internal byte[] OneBitMask = {0x80, 0x40, 0x20, 0x10, 8, 4, 2, 1};

	internal PdfImage
			(
			PdfDocument		Document
			) : base(Document, ObjectType.Stream, "/XObject")
		{
		// set subtype to /Image
		Dictionary.Add("/Subtype", "/Image");

		// create resource code
		ResourceCode = Document.GenerateResourceNumber('X');
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Constructor for image file
	/// </summary>
	/// <param name="Document">PDF document (parent object)</param>
	/// <param name="ImageFileName">Image file name</param>
	/// <param name="ImageControl">Image control</param>
	/// <remarks>
	/// <para>Image quality is a parameter that used by the .net framework
	/// during the compression of the image from bitmap to jpeg. If the parameter
	/// is missing or set to -1 the library saves the bitmap image as</para>
	/// <code>
	///	Bitmap.Save(MemoryStream, ImageFormat.Jpeg);
	///	</code>
	///	<para>If the ImageQuality parameter is 0 to 100, the library saves the bitmap image as</para>
	///	<code>
	///	EncoderParameters EncoderParameters = new EncoderParameters(1);
	/// EncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ImageQuality);
	/// Bitmap.Save(MemoryStream, GetEncoderInfo("image/jpeg"), EncoderParameters);
	///	</code>
	///	<para>Microsoft does not specify the image quality factor used in the 
	///	first method of saving. However, experimantaion and Internet comments shows that it is 75.</para>
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public PdfImage
			(
			PdfDocument Document,
			string ImageFileName,
			PdfImageControl ImageControl = null
			) : this(Document)
		{
		ConstructorHelper(LoadImageFromFile(ImageFileName), ImageControl);
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Constructor for image object
	/// </summary>
	/// <param name="Document">PDF document (parent object)</param>
	/// <param name="Image">Image bitmap or metafile</param>
	/// <param name="ImageControl">Image control (optional)</param>
	/// <remarks>
	/// <para>Image quality is a parameter that used by the .net framework
	/// during the compression of the image from bitmap to jpeg. If the parameter
	/// is missing or set to -1 the library saves the bitmap image as</para>
	/// <code>
	///	Bitmap.Save(MemoryStream, ImageFormat.Jpeg);
	///	</code>
	///	<para>If the ImageQuality parameter is 0 to 100, the library saves the bitmap image as</para>
	///	<code>
	///	EncoderParameters EncoderParameters = new EncoderParameters(1);
	/// EncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ImageQuality);
	/// Bitmap.Save(MemoryStream, GetEncoderInfo("image/jpeg"), EncoderParameters);
	///	</code>
	///	<para>Microsoft does not specify the image quality factor used in the 
	///	first method of saving. However, experimantaion and Internet comments shows that it is 75.</para>
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public PdfImage
			(
			PdfDocument Document,
			Image Image,
			PdfImageControl ImageControl = null
			) : this(Document)
		{
		ConstructorHelper(Image, ImageControl);
		return;
		}

	/// <summary>
	/// Black and white image
	/// </summary>
	/// <param name="Document">PDF document (parent object)</param>
	/// <param name="BWImage">Black and white image</param>
	/// <param name="ImageControl">Image control (optional)</param>
	public PdfImage
			(
			PdfDocument Document,
			bool[,] BWImage,
			PdfImageControl ImageControl = null
			) : this(Document)
		{
		// image dimensions
		WidthPix = BWImage.GetUpperBound(0) + 1;
		HeightPix = BWImage.GetUpperBound(1) + 1;

		// image represented as two dimension boolean array
		this.BWImage = BWImage;

		// default image control
		if(ImageControl == null) ImageControl = new PdfImageControl();
		ImageControl.SaveAs = SaveImageAs.BWImage;
		this.ImageControl = ImageControl;

		// write image stream to pdf file
		WriteObjectToPdfFile();
		return;
		}

	/// <summary>
	/// Display PDF417 barcode as a black and white image
	/// </summary>
	/// <param name="Document">PDF document (parent object)</param>
	/// <param name="Pdf417Encoder">PDF417 barcode encoder</param>
	public PdfImage
			(
			PdfDocument Document,
			Pdf417Encoder Pdf417Encoder
			) : this(Document)
		{
		// barcode width and height
		WidthPix = Pdf417Encoder.ImageWidth;
		HeightPix = Pdf417Encoder.ImageHeight;

		// black and white barcode image
		BWImage = Pdf417Encoder.ConvertBarcodeMatrixToPixels();

		// image control for PDF417 code
		ImageControl = new PdfImageControl();
		ImageControl.ReverseBW = true;
		ImageControl.SaveAs = SaveImageAs.BWImage;

		// write image stream to pdf file
		WriteObjectToPdfFile();
		return;
		}

	/// <summary>
	/// Display QRCode barcode as a black and white image
	/// </summary>
	/// <param name="Document">PDF document (parent object)</param>
	/// <param name="QREncoder">QRCode encoder</param>
	public PdfImage
			(
			PdfDocument Document,
			QREncoder QREncoder
			) : this(Document)
		{
		// barcode width and height
		WidthPix = QREncoder.QRCodeImageDimension;
		HeightPix = QREncoder.QRCodeImageDimension;

		// black and white barcode image
		BWImage = QREncoder.ConvertQRCodeMatrixToPixels();

		// image control for PDF417 code
		ImageControl = new PdfImageControl();
		ImageControl.ReverseBW = true;
		ImageControl.SaveAs = SaveImageAs.BWImage;

		// write image stream to pdf file
		WriteObjectToPdfFile();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Constructor helper method
	////////////////////////////////////////////////////////////////////

	private void ConstructorHelper
			(
			Image			Image,
			PdfImageControl	ImageControl
			)
		{
		// image control
		if(ImageControl == null) ImageControl = new PdfImageControl();
		this.ImageControl = ImageControl;

		// image rectangle
		ImageRectangle(Image);

		// image size in pixels
		ImageSizeInPixels(Image);

		// convert the image to bitmap
		ConvertImageToBitmap(Image);

		// write to output file
		WriteObjectToPdfFile();

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Load image from disk file into Image class
	////////////////////////////////////////////////////////////////////

	internal Image LoadImageFromFile
			(
			string ImageFileName
			)
		{
		// test exitance
		if(!File.Exists(ImageFileName)) throw new ApplicationException("Image file " + ImageFileName + " does not exist");

		// get file length
		FileInfo FI = new FileInfo(ImageFileName);
		long ImageFileLength = FI.Length;
		if(ImageFileLength >= int.MaxValue) throw new ApplicationException("Image file " + ImageFileName + " too long");

		// load the image file
		Image Image;
		try
			{
			// file is metafile format
			if(ImageFileName.EndsWith(".emf", StringComparison.OrdinalIgnoreCase) || ImageFileName.EndsWith(".wmf", StringComparison.OrdinalIgnoreCase))
				Image = new Metafile(ImageFileName);

			// all other image formats
			else
				Image = new Bitmap(ImageFileName);
			}

		// not image file
		catch(ArgumentException)
			{
			throw new ApplicationException("Invalid image file: " + ImageFileName);
			}

		// set dispose image flag
		DisposeImage = true;

		// return
		return(Image);
		}

	////////////////////////////////////////////////////////////////////
	// Create Image rectangle
	// some images have origin not at top left corner
	////////////////////////////////////////////////////////////////////

	internal void ImageRectangle
			(
			Image Image
			)
		{
		// image rectangle
		ImageRect = new Rectangle(0, 0, Image.Width, Image.Height);

		// some images have origin not at top left corner
		GraphicsUnit Unit = GraphicsUnit.Pixel;
		RectangleF ImageBounds = Image.GetBounds(ref Unit);
		if(ImageBounds.X != 0.0 || ImageBounds.Y != 0.0)
			{
			// set origin
			if(Unit == GraphicsUnit.Pixel)
				{
				ImageRect.X = (int) ImageBounds.X;
				ImageRect.Y = (int) ImageBounds.Y;
				}
			else
				{
				ImageRect.X = (int) (ImageBounds.X * Image.Width / ImageBounds.Width);
				ImageRect.Y = (int) (ImageBounds.Y * Image.Height / ImageBounds.Height);
				}
			}
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set image size in pixels
	// If crop is active adjust image size to crop rectangle
	////////////////////////////////////////////////////////////////////

	internal void ImageSizeInPixels
			(
			Image			Image
			)
		{
		// crop rectangle is given in percent width or height
		if(ImageControl.CropRect.IsEmpty && !ImageControl.CropPercent.IsEmpty)
			{
			ImageControl.CropRect = new Rectangle((int) (0.01 * Image.Width * ImageControl.CropPercent.X + 0.5),
				(int) (0.01 * Image.Height * ImageControl.CropPercent.Y + 0.5),
				(int) (0.01 * Image.Width * ImageControl.CropPercent.Width + 0.5),
				(int) (0.01 * Image.Height * ImageControl.CropPercent.Height + 0.5)); 
			}

		// no crop
		if(ImageControl.CropRect.IsEmpty)
			{
			// get image width and height in pixels
			WidthPix = Image.Width;
			HeightPix = Image.Height;
			return;
			}

		// crop
		// adjust origin
		if(ImageRect.X != 0 || ImageRect.Y != 0)
			{
			ImageControl.CropRect.X += ImageRect.X;
			ImageControl.CropRect.Y += ImageRect.Y;
			}

		// crop rectangle must be contained within image rectangle
		if(!ImageRect.Contains(ImageControl.CropRect)) throw new ApplicationException("PdfImage: Crop rectangle must be contained within image rectangle");

		// change image size to crop size
		WidthPix = ImageControl.CropRect.Width;
		HeightPix = ImageControl.CropRect.Height;

		// replace image rectangle with crop rectangle
		ImageRect = ImageControl.CropRect;
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Convert image to bitmap
	////////////////////////////////////////////////////////////////////

	internal void ConvertImageToBitmap
			(
			Image		Image
			)
		{
		// destination rectangle
		Rectangle DestRect = new Rectangle(0, 0, WidthPix, HeightPix);

		// resolution pixels per inch
		double HorizontalResolution = Image.HorizontalResolution;
		double VerticalResolution = Image.VerticalResolution;

		// adjust resolution if it is not zero or greater than exising resolution
		if(ImageControl.Resolution != 0)
			{
			// image resolution
			double ImageResolution = 0.5 * (HorizontalResolution + VerticalResolution);

			// requested resolution is less than image
			if(ImageControl.Resolution < ImageResolution)
				{
				// change in resolution 
				double Factor = ImageControl.Resolution / ImageResolution;

				// convert to pixels based on requested resolution
				int NewWidthPix = (int) (WidthPix * Factor + 0.5);
				int NewHeightPix = (int) (HeightPix * Factor + 0.5);

				// new size in pixels is must be smaller than image size or cropped image size
				if(NewWidthPix < WidthPix && NewHeightPix < HeightPix)
					{
					// new image size in pixels
					WidthPix = NewWidthPix;
					HeightPix = NewHeightPix;

					DestRect.Width = NewWidthPix;
					DestRect.Height = NewHeightPix;

					// adjust resolution
					HorizontalResolution *= Factor;
					VerticalResolution *= Factor;
					}
				else
					{
					ImageControl.Resolution = 0;
					}
				}
			else
				{
				ImageControl.Resolution = 0;
				}
			}

		// Assume we will need to dispose the Picture Bitmap
		DisposePicture = true;

		// image is Bitmap (not Metafile)
		if(Image.GetType() == typeof(Bitmap))
			{
			// no crop
			if(ImageControl.CropRect.IsEmpty)
				{
				// image is bitmap, no crop, no change in resolution
				if(ImageControl.Resolution == 0)
					{
					Picture = (Bitmap) Image;
					DisposePicture = DisposeImage;
					DisposeImage = false;
					}

				// image is bitmap, no crop, change to resolution
				else
					{
					// load bitmap into smaller bitmap
					Picture = new Bitmap(Image, WidthPix, HeightPix);
					}
				}

			// crop image
			else
				{
				// create bitmap
				Picture = new Bitmap(WidthPix, HeightPix);

				// create graphics object fill with white
				Graphics GR = Graphics.FromImage(Picture);

				// draw the image into the bitmap
				GR.DrawImage(Image, DestRect, ImageRect, GraphicsUnit.Pixel);

				// dispose of the graphics object
				GR.Dispose();
				}
			}

		// image is Metafile (not Bitmap)
		else
			{
			// create bitmap
			Picture = new Bitmap(WidthPix, HeightPix);

			// create graphics object fill with white
			Graphics GR = Graphics.FromImage(Picture);
			GR.Clear(Color.White);

			//GR.CompositingQuality = CompositingQuality.HighSpeed;
			//GR.InterpolationMode = InterpolationMode.Low;
			//GR.SmoothingMode = SmoothingMode.None;

			// draw the image into the bitmap
			GR.DrawImage(Image, DestRect, ImageRect, GraphicsUnit.Pixel);

			// dispose of the graphics object
			GR.Dispose();
			}

		// dispose image
		if(DisposeImage) Image.Dispose();

		// set resolution
		Picture.SetResolution((Single) HorizontalResolution, (Single) VerticalResolution);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Write object to PDF file
	////////////////////////////////////////////////////////////////////

	internal override void WriteObjectToPdfFile()
		{
		// add items to dictionary
		Dictionary.AddInteger("/Width", WidthPix);
		Dictionary.AddInteger("/Height", HeightPix);

		switch(ImageControl.SaveAs)
			{ 
			case SaveImageAs.Jpeg:
				PictureToJpeg();
				break;

			case SaveImageAs.IndexedImage:
				if(!PictureToIndexedImage()) goto case SaveImageAs.Jpeg;
				break;

			case SaveImageAs.GrayImage:
				if(!PictureToGrayImage()) goto case SaveImageAs.Jpeg;
				break;

			case SaveImageAs.BWImage:
				if(Picture != null)
					{
					if(!PictureToBWImage()) goto case SaveImageAs.Jpeg;
					}
				else
					{
					BooleanToBWImage();
					}
				break;
			}

		// dispose picture
		Dispose();

		// debug
		if(Document.Debug) ObjectValueArray = Document.TextToByteArray("*** IMAGE PLACE HOLDER ***");

		// write stream
		base.WriteObjectToPdfFile();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Convert .net bitmap image to PDF indexed bitmap image
	////////////////////////////////////////////////////////////////////

	internal void PictureToJpeg()
		{
		// create memory stream
		MemoryStream MS = new MemoryStream();

		// image quality is default
		if(ImageControl.ImageQuality == PdfImageControl.DefaultQuality)
			{
			// save in jpeg format with 75 quality
			Picture.Save(MS, ImageFormat.Jpeg);
			}

		// save image with defined quality
		else
			{
			// build EncoderParameter object for image quality
			EncoderParameters EncoderParameters = new EncoderParameters(1);
			EncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ImageControl.ImageQuality);

			// save in jpeg format with specified quality
			Picture.Save(MS, GetEncoderInfo("image/jpeg"), EncoderParameters);
			}

		// image byte array
		ObjectValueArray = MS.GetBuffer();

		// close and dispose memory stream
		MS.Close();
		MS = null;

		// no deflate compression
		NoCompression = true;

		// image dictionary
		Dictionary.Add("/Filter", "/DCTDecode");
		Dictionary.Add("/ColorSpace", "/DeviceRGB");
		Dictionary.Add("/BitsPerComponent", "8");
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Convert .net bitmap image to PDF indexed bitmap image
	////////////////////////////////////////////////////////////////////

	internal bool PictureToIndexedImage()
		{
		// if Picture Bitmap cannot be converted to RGB array, return with false
		BitmapData PictureData;
		try
			{
			// lock picture and get array of R G B bytes
			PictureData = Picture.LockBits(new Rectangle(0, 0, WidthPix, HeightPix), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			}
		catch
			{
			return(false);
			}

		// frame width in bytes
		int FrameWidth = Math.Abs(PictureData.Stride);

		// number of unused bytes at the end of the frame
		int PicDelta = FrameWidth - 3 * WidthPix;

		// allocate byte array for picture bytes
		byte[] PictureBytes = new byte[FrameWidth * HeightPix];

		// pointer to start of data in unmanaged memory
		IntPtr Scan0 = PictureData.Scan0;

		// copy RGB bytes from picture to local array
		Marshal.Copy(Scan0, PictureBytes, 0, PictureBytes.Length);

		// unlock picture
		Picture.UnlockBits(PictureData);

		// create indexed color array
		List<int> ColorArray = new List<int>();
		int PicPtr = 0;
		for(int Y = 0; Y < HeightPix; Y++)
			{ 
			for(int X = 0; X < WidthPix; X++)
				{
				if(ColorArray.Count == 256) return(false);
				// color order is blue, green and red
				int Pixel = PictureBytes[PicPtr++] | (PictureBytes[PicPtr++] << 8) | (PictureBytes[PicPtr++] << 16);
				int Index = ColorArray.BinarySearch(Pixel);
				if(Index >= 0) continue;
				ColorArray.Insert(~Index, Pixel);
				}
			PicPtr += PicDelta;
			}

		int BitPerComponent;

		// create stream for 1 or 2 colors
		if(ColorArray.Count <= 2)
			{
			// bits per component
			BitPerComponent = 1;

			// each row must be multiple of bytes
			int WidthBytes = (WidthPix + 7) / 8;
			int ObjDelta = (WidthPix & 7) == 0 ? 0 : 1;

			// creale empty object value array
			ObjectValueArray = new byte[WidthPix * WidthBytes];


			// convert picture in rgb to color index
			PicPtr = 0;
			int ObjPtr = 0;
			for(int Y = 0; Y < HeightPix; Y++)
				{ 
				for(int X = 0; X < WidthPix; X++)
					{
					int Pixel = PictureBytes[PicPtr++] | (PictureBytes[PicPtr++] << 8) | (PictureBytes[PicPtr++] << 16);
					int Index = ColorArray.BinarySearch(Pixel);
					if(Index != 0) ObjectValueArray[ObjPtr] |= OneBitMask[X & 7];
					if((X & 7) == 7) ObjPtr++;
					}
				PicPtr += PicDelta;
				ObjPtr += ObjDelta;
				}
			}

		// create stream for 3 to 4 colors
		else if(ColorArray.Count <= 4)
			{
			// bits per component
			BitPerComponent = 2;

			// each row must be multiple of bytes
			int WidthBytes = (WidthPix + 3) / 4;
			int ObjDelta = (WidthPix & 3) == 0 ? 0 : 1;

			// creale empty object value array
			ObjectValueArray = new byte[WidthBytes * HeightPix];

			// convert picture in rgb to color index
			PicPtr = 0;
			int ObjPtr = 0;
			for(int Y = 0; Y < HeightPix; Y++)
				{ 
				int Shift = 6;
				for(int X = 0; X < WidthPix; X++)
					{
					int Pixel = PictureBytes[PicPtr++] | (PictureBytes[PicPtr++] << 8) | (PictureBytes[PicPtr++] << 16);
					int Index = ColorArray.BinarySearch(Pixel);
					ObjectValueArray[ObjPtr] |= (byte) (Index << Shift);
					Shift -= 2;
					if(Shift < 0)
						{
						Shift = 6;
						ObjPtr++;
						}
					}
				PicPtr += PicDelta;
				ObjPtr += ObjDelta;
				}
			}

		// create stream for 5 or 16 colors
		else if(ColorArray.Count <= 16)
			{
			// bits per component
			BitPerComponent = 4;

			// each row must be multiple of bytes
			int WidthBytes = (WidthPix + 1) / 2;
			int ObjDelta = WidthPix & 1;

			// creale empty object value array
			ObjectValueArray = new byte[WidthBytes * HeightPix];

			// convert picture in rgb to color index
			PicPtr = 0;
			int ObjPtr = 0;
			for(int Y = 0; Y < HeightPix; Y++)
				{ 
				for(int X = 0; X < WidthPix; X++)
					{
					int Pixel = PictureBytes[PicPtr++] | (PictureBytes[PicPtr++] << 8) | (PictureBytes[PicPtr++] << 16);
					int Index = ColorArray.BinarySearch(Pixel);
					if((X & 1) == 0)
						{ 
						ObjectValueArray[ObjPtr] = (byte) (Index << 4);
						}
					else
						{ 
						ObjectValueArray[ObjPtr++] |= (byte) Index;
						}
					}
				PicPtr += PicDelta;
				ObjPtr += ObjDelta;
				}
			}

		// create stream for 17 to 256 colors
		else
			{
			// 8 bits per component
			BitPerComponent = 8;

			// allocate one byte per pixel array
			ObjectValueArray = new byte[WidthPix * HeightPix];

			// convert picture in rgb to color index
			PicPtr = 0;
			int ObjPtr = 0;
			for(int Y = 0; Y < HeightPix; Y++)
				{ 
				for(int X = 0; X < WidthPix; X++)
					{
					int Pixel = PictureBytes[PicPtr++] | (PictureBytes[PicPtr++] << 8) | (PictureBytes[PicPtr++] << 16);
					ObjectValueArray[ObjPtr++] = (byte) ColorArray.BinarySearch(Pixel);
					}
				PicPtr += PicDelta;
				}
			}

		// convert color array from int to byte
		byte[] ColorByteArray = new byte[ColorArray.Count * 3];
		int ColorPtr = 0;
		for(int Index = 0; Index < ColorArray.Count; Index++)
			{
			ColorByteArray[ColorPtr++] = (byte) (ColorArray[Index] >> 16);
			ColorByteArray[ColorPtr++] = (byte) (ColorArray[Index] >> 8);
			ColorByteArray[ColorPtr++] = (byte) ColorArray[Index];
			}

		// encryption is active. PDF string must be encrypted
		if(Document.Encryption != null) ColorByteArray = Document.Encryption.EncryptByteArray(ObjectNumber, ColorByteArray);

		// convert byte array to PDF string format
		string ColorStr = Document.ByteArrayToPdfString(ColorByteArray);

		// add items to dictionary
		Dictionary.AddFormat("/ColorSpace",  "[/Indexed /DeviceRGB {0} {1}]", ColorArray.Count - 1, ColorStr);	// R G B
		Dictionary.AddInteger("/BitsPerComponent", BitPerComponent); // 1 2 4 8 
		return(true);
		}

	////////////////////////////////////////////////////////////////////
	// Convert .net bitmap image to PDF indexed bitmap image
	////////////////////////////////////////////////////////////////////

	internal bool PictureToGrayImage()
		{
		// if Picture Bitmap cannot be converted to RGB array, return with false
		BitmapData PictureData;
		try
			{
			// lock picture and get array of Blue green and Red bytes
			PictureData = Picture.LockBits(new Rectangle(0, 0, WidthPix, HeightPix), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			}
		catch
			{
			return(false);
			}

		// frame width in bytes
		int FrameWidth = Math.Abs(PictureData.Stride);

		// number of unused bytes at the end of the frame
		int PicDelta = FrameWidth - 3 * WidthPix;

		// allocate byte array for picture bytes
		byte[] PictureBytes = new byte[FrameWidth * HeightPix];

		// pointer to start of data in unmanaged memory
		IntPtr Scan0 = PictureData.Scan0;

		// copy RGB bytes from picture to local array
		Marshal.Copy(Scan0, PictureBytes, 0, PictureBytes.Length);

		// unlock picture
		Picture.UnlockBits(PictureData);

		// allocate one byte per pixel array
		ObjectValueArray = new byte[WidthPix * HeightPix];

		// convert picture in rgb to shades of gray
		int PicPtr = 0;
		int ObjPtr = 0;
		for(int Y = 0; Y < HeightPix; Y++)
			{ 
			for(int X = 0; X < WidthPix; X++)
				{
				// bytes are in blue green red order
				int Pixel = (11 * PictureBytes[PicPtr++] + 59 * PictureBytes[PicPtr++] + 30 * PictureBytes[PicPtr++] + 50) / 100;
				ObjectValueArray[ObjPtr++] = (byte) Pixel;
				}
			PicPtr += PicDelta;
			}

		// add items to dictionary
		Dictionary.Add("/ColorSpace", "/DeviceGray");
		Dictionary.Add("/BitsPerComponent", "8");
		if(ImageControl.ReverseBW) Dictionary.Add("/Decode", "[1 0]");
		return(true);
		}

	////////////////////////////////////////////////////////////////////
	// Convert .net bitmap image to PDF indexed bitmap image
	////////////////////////////////////////////////////////////////////

	internal bool PictureToBWImage()
		{
		// if Picture Bitmap cannot be converted to RGB array, return with false
		BitmapData PictureData;
		try
			{
			// lock picture and get array of Blue green and Red bytes
			PictureData = Picture.LockBits(new Rectangle(0, 0, WidthPix, HeightPix), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			}
		catch
			{
			return(false);
			}

		// frame width in bytes
		int FrameWidth = Math.Abs(PictureData.Stride);

		// number of unused bytes at the end of the frame
		int PicDelta = FrameWidth - 3 * WidthPix;

		// allocate byte array for picture bytes
		byte[] PictureBytes = new byte[FrameWidth * HeightPix];

		// pointer to start of data in unmanaged memory
		IntPtr Scan0 = PictureData.Scan0;

		// copy RGB bytes from picture to local array
		Marshal.Copy(Scan0, PictureBytes, 0, PictureBytes.Length);

		// unlock picture
		Picture.UnlockBits(PictureData);

		// each row must be multiple of bytes
		int WidthBytes = (WidthPix + 7) / 8;

		// creale empty object value array
		ObjectValueArray = new byte[HeightPix * WidthBytes];

		// QRCode matrix to PDF bitmap
		int PicPtr = 0;
		int RowPtr = 0;
		int Cutoff = 255 * ImageControl._GrayToBWCutoff;
		for(int Row = 0; Row < HeightPix; Row++)
			{
			for(int Col = 0; Col < WidthPix; Col++)
				{
				if(11 * PictureBytes[PicPtr++] + 59 * PictureBytes[PicPtr++] + 30 * PictureBytes[PicPtr++] >= Cutoff)
					ObjectValueArray[RowPtr + (Col >> 3)] |= (byte) (1 << (7 - (Col & 7)));
				}
			PicPtr += PicDelta;
			RowPtr += WidthBytes;
			}

		// add items to dictionary
		Dictionary.Add("/ColorSpace", "/DeviceGray");
		Dictionary.Add("/BitsPerComponent", "1");
		if(ImageControl.ReverseBW) Dictionary.Add("/Decode", "[1 0]");
		return(true);
		}

	////////////////////////////////////////////////////////////////////
	// Convert .net bitmap image to PDF indexed bitmap image
	////////////////////////////////////////////////////////////////////

	internal void BooleanToBWImage()
		{
		// each row must be multiple of bytes
		int WidthBytes = (WidthPix + 7) / 8;

		// creale empty object value array
		ObjectValueArray = new byte[HeightPix * WidthBytes];

		// QRCode matrix to PDF bitmap
		int RowPtr = 0;
		for(int Row = 0; Row < HeightPix; Row++)
			{
			for(int Col = 0; Col < WidthPix; Col++)
				{
				if(BWImage[Row, Col]) ObjectValueArray[RowPtr + (Col >> 3)] |= (byte) (1 << (7 - (Col & 7)));
				}
			RowPtr += WidthBytes;
			}

		// add items to dictionary
		Dictionary.Add("/ColorSpace", "/DeviceGray");
		Dictionary.Add("/BitsPerComponent", "1");
		if(ImageControl.ReverseBW) Dictionary.Add("/Decode", "[1 0]");
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Write object to PDF file
	////////////////////////////////////////////////////////////////////

   private ImageCodecInfo GetEncoderInfo(string mimeType)
	    {
        ImageCodecInfo[] EncoderArray = ImageCodecInfo.GetImageEncoders();
        foreach(ImageCodecInfo Encoder in EncoderArray) if(Encoder.MimeType == mimeType) return(Encoder);
        throw new ApplicationException("GetEncoderInfo: image/jpeg encoder does not exist");;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Calculates image size to preserve aspect ratio.
	/// </summary>
	/// <param name="InputSize">Image display area.</param>
	/// <returns>Adjusted image display area.</returns>
	/// <remarks>
	/// Calculates best fit to preserve aspect ratio.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public SizeD ImageSize
			(
			SizeD InputSize
			)
		{
		return(ImageSizePos.ImageSize(WidthPix, HeightPix, InputSize.Width, InputSize.Height));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Calculates image size to preserve aspect ratio.
	/// </summary>
	/// <param name="Width">Image display width.</param>
	/// <param name="Height">Image display height.</param>
	/// <returns>Adjusted image display area.</returns>
	/// <remarks>
	/// Calculates best fit to preserve aspect ratio.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public SizeD ImageSize
			(
			double	Width,
			double	Height
			)
		{
		return(ImageSizePos.ImageSize(WidthPix, HeightPix, Width, Height));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Calculates image size to preserve aspect ratio and sets position.
	/// </summary>
	/// <param name="InputSize">Image display area</param>
	/// <param name="Alignment">Content alignment</param>
	/// <returns>Adjusted image size and position within area.</returns>
	/// <remarks>
	/// Calculates best fit to preserve aspect ratio and adjust
	/// position according to content alignment argument.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public PdfRectangle ImageSizePosition
			(
			SizeD				InputSize,
			ContentAlignment	Alignment
			)
		{
		return(ImageSizePos.ImageArea(WidthPix, HeightPix, 0.0, 0.0,  InputSize.Width, InputSize.Height, Alignment));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Calculates image size to preserve aspect ratio and sets position.
	/// </summary>
	/// <param name="Width">Image display width</param>
	/// <param name="Height">Image display height</param>
	/// <param name="Alignment">Content alignment</param>
	/// <returns>Adjusted image size and position within area.</returns>
	/// <remarks>
	/// Calculates best fit to preserve aspect ratio and adjust
	/// position according to content alignment argument.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public PdfRectangle ImageSizePosition
			(
			double				Width,
			double				Height,
			ContentAlignment	Alignment
			)
		{
		return(ImageSizePos.ImageArea(WidthPix, HeightPix, 0.0, 0.0,  Width, Height, Alignment));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Dispose unmanaged resources
	/// </summary>
	////////////////////////////////////////////////////////////////////
	public void Dispose()
		{
		// release bitmap
		if(DisposePicture && Picture != null)
			{
			Picture.Dispose();
			Picture = null;
			}

		// exit
		return;		
		}
	}
}
