/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	CreateMetafile
//	Create Metafile with graphics object.
//	It was used to test the PdfObject class with Metafile image.
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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace PdfFileWriter
{
/// <summary>
/// Create image metafile class
/// </summary>
public class CreateMetafile : IDisposable
	{
	/// <summary>
	/// Gets image metafile.
	/// </summary>
	public Metafile	Metafile {get; protected set;}

	/// <summary>
	/// Gets graphics object form image metafile.
	/// </summary>
	public Graphics	Graphics {get; protected set;}

	/// <summary>
	/// Create image metafile constructor
	/// </summary>
	/// <param name="Width">Image width in pixels.</param>
	/// <param name="Height">Image height in pixels.</param>
	public CreateMetafile
			(
			int	Width,
			int	Height
			)
		{
		using (MemoryStream Stream = new MemoryStream())
			{
			using (Graphics MemoryGraphics = Graphics.FromHwndInternal(IntPtr.Zero))
				{
				IntPtr deviceContextHandle = MemoryGraphics.GetHdc();
				Metafile = new Metafile(Stream, deviceContextHandle, new RectangleF(0, 0, Width, Height), MetafileFrameUnit.Pixel, EmfType.EmfPlusOnly);
				MemoryGraphics.ReleaseHdc();
				}
			}

		Graphics = Graphics.FromImage(Metafile);

		// Set everything to high quality
		Graphics.SmoothingMode = SmoothingMode.HighQuality;
		Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
		Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
		Graphics.CompositingQuality = CompositingQuality.HighQuality;
 		Graphics.PageUnit = GraphicsUnit.Pixel;
		return;
		}

    [DllImport("gdi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern IntPtr CopyEnhMetaFile(IntPtr MetaFileHandle, IntPtr FileName);

	/// <summary>
	/// Save image metafile
	/// </summary>
	/// <param name="FileName">File name</param>
	public void SaveMetafile
			(
			string FileName
			)
		{
		// Get a handle to the metafile
		IntPtr MetafileHandle = Metafile.GetHenhmetafile();

		// allocate character table buffer in global memory (two bytes per char)
		IntPtr CharBuffer = Marshal.AllocHGlobal(2 * FileName.Length + 2);

		// move file name inclusing terminating zer0 to the buffer
		for(int Index = 0; Index < FileName.Length; Index++) Marshal.WriteInt16(CharBuffer, 2 * Index, (short) FileName[Index]); 
		Marshal.WriteInt16(CharBuffer, 2 * FileName.Length, 0);

		// Export metafile to an image file
		CopyEnhMetaFile(MetafileHandle, CharBuffer);
 
		// free local buffer
		Marshal.FreeHGlobal(CharBuffer);
		return;
		}

    [DllImport("gdi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern IntPtr DeleteEnhMetaFile(IntPtr MetaFileHandle);

	/// <summary>
	/// Delete image metafile.
	/// </summary>
	public void DeleteMetafile()
		{
		// Get a handle to the metafile
		IntPtr MetafileHandle = Metafile.GetHenhmetafile();

		// Delete the metafile from memory
		DeleteEnhMetaFile(MetafileHandle);
		return;
		}

	/// <summary>
	/// Dispose object
	/// </summary>
	public void Dispose()
		{
		if(Graphics != null)
			{
			Graphics.Dispose();
			Graphics = null;
			}
		if(Metafile != null)
			{
			Metafile.Dispose();
			Metafile = null;
			}
		return;
		}
	}
}
