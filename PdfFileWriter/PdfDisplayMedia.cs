/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfDisplayMedia
//	PDF display media class. 
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

namespace PdfFileWriter
{
/// <summary>
/// Temporary file permission enumeration
/// </summary>
public enum TempFilePermission
	{
	/// <summary>
	/// Never allow PDF reader to write temporary file.
	/// </summary>
	TEMPNEVER,

	/// <summary>
	/// Allow PDF reader to write temporary file based on extract permission flag.
	/// </summary>
	TEMPEXTRACT,

	/// <summary>
	/// Allow PDF reader to write temporary file based on access permission flag.
	/// </summary>
	TEMPACCESS,

	/// <summary>
	/// Always allow PDF reader to write temporary file.
	/// </summary>
	TEMPALWAYS,
	}

/// <summary>
/// Media window position
/// </summary>
public enum MediaWindow
	{
	/// <summary>
	/// Floating window
	/// </summary>
	Floating,

	/// <summary>
	/// Full screen
	/// </summary>
	FullScreen,

	/// <summary>
	/// Hidden
	/// </summary>
	Hidden,

	/// <summary>
	/// Annotation rectangle
	/// </summary>
	Annotation,
	}

/// <summary>
/// Media image position within window
/// </summary>
public enum WindowPosition
	{
	/// <summary>
	/// Upper left
	/// </summary>
	UpperLeft,

	/// <summary>
	/// Upper center
	/// </summary>
	UpperCenter,

	/// <summary>
	/// Upper right
	/// </summary>
	UpperRight,

	/// <summary>
	/// Center left
	/// </summary>
	CenterLeft,

	/// <summary>
	/// Center
	/// </summary>
	Center,

	/// <summary>
	/// Center right
	/// </summary>
	CenterRight,

	/// <summary>
	/// Lower left
	/// </summary>
	LowerLeft,

	/// <summary>
	/// lower center
	/// </summary>
	LowerCenter,

	/// <summary>
	/// Lower right
	/// </summary>
	LowerRight,
	}

/// <summary>
/// Floating window title bar
/// </summary>
public enum WindowTitleBar
	{
	/// <summary>
	/// No title bar
	/// </summary>
	NoTitleBar,

	/// <summary>
	/// Window has title bar
	/// </summary>
	TitleBar,

	/// <summary>
	/// Window has title bar with close button
	/// </summary>
	TitleBarWithCloseButton,
	}

/// <summary>
/// Floating window resize options
/// </summary>
public enum WindowResize
	{
	/// <summary>
	/// No resize
	/// </summary>
	NoResize,

	/// <summary>
	/// Resize with correct aspect ratio
	/// </summary>
	KeepAspectRatio,

	/// <summary>
	/// Resize without aspect ratio
	/// </summary>
	NoAspectRatio,
	}

/// <summary>
/// Media operation code
/// </summary>
/// <remarks>
/// <para>
/// Operation to perform when rendition action is triggered.
/// Page 669 T 8.64 S 8.5
/// </para>
/// </remarks>
public enum MediaOperation
	{
	/// <summary>
	/// Play
	/// </summary>
	Play,

	/// <summary>
	/// Stop
	/// </summary>
	Stop,

	/// <summary>
	/// Pause
	/// </summary>
	Pause,

	/// <summary>
	/// Resume
	/// </summary>
	Resume,

	/// <summary>
	/// Play after pause
	/// </summary>
	PlayAfterPause,
	}

/// <summary>
/// Scale media code
/// </summary>
/// <remarks>
/// <para>
/// Value 0 to 5 How to scale the media to fit annotation area page 770 T 9.15
/// </para>
/// </remarks>
public enum ScaleMediaCode
	{
	/// <summary>
	/// Keep aspect ratio and show all.
	/// </summary>
	KeepAspectRatioShowAll,

	/// <summary>
	/// Keep aspect ratio fit the one side and slice the other
	/// </summary>
	KeepAspectRatioSlice,

	/// <summary>
	/// Ignore aspect ratio and fill annotation rectangle
	/// </summary>
	FillAnotationRect,

	/// <summary>
	/// No scaling. Provide scroll if required
	/// </summary>
	NoScaleWithScroll,

	/// <summary>
	/// No scaling. Show what fits
	/// </summary>
	NoScaleSlice,

	/// <summary>
	/// Let media player handle it
	/// </summary>
	PlayerDefault,
	}

/// <summary>
/// PDF Screen annotation
/// </summary>
public class PdfDisplayMedia : PdfObject
	{
	/// <summary>
	/// Gets embedded media file class
	/// </summary>
	public PdfEmbeddedFile MediaFile {get; private set;}

	private PdfDictionary	Rendition;
	private PdfDictionary	MediaClip;
	private PdfDictionary	MediaPlay;
	private PdfDictionary	MediaPlayBE;
	private PdfDictionary	MediaScreenParam;
	private PdfDictionary	MediaScreenParamBE;
	private PdfDictionary	TempFilePermissions;

	/// <summary>
	/// Display media constructor
	/// </summary>
	/// <param name="MediaFile">Embedded media file</param>
	/// <param name="MimeType">Mime type</param>
	/// <remarks>
	/// <para>
	/// If mime type is null the program will try to convert file extension
	/// to mime type. If conversion is not available application exception will be raised.
	/// </para>
	/// </remarks>
	public PdfDisplayMedia
			(
			PdfEmbeddedFile	MediaFile,
			string			MimeType = null
			) : base(MediaFile.Document)
		{
		// save media file
		this.MediaFile = MediaFile;

		// save mimetype
		if(MimeType == null) MimeType = MediaFile.MimeType;
		if(string.IsNullOrWhiteSpace(MimeType)) throw new ApplicationException("MIME type is not defined");

		// rendition dictionary page 759 Section 9.1.2 Table 9.1
		Rendition = new PdfDictionary(this);
		Dictionary.AddDictionary("/R", Rendition);

		// media clip
		MediaClip = new PdfDictionary(this);
		Rendition.AddDictionary("/C", MediaClip);

		// Media clip dictionary T 9.9
		TempFilePermissions = new PdfDictionary(this);
		MediaClip.AddDictionary("/P", TempFilePermissions);

		// media play
		MediaPlay = new PdfDictionary(this);
		Rendition.AddDictionary("/P", MediaPlay);

		// media play BE
		MediaPlayBE = new PdfDictionary(this);
		MediaPlay.AddDictionary("/BE", MediaPlayBE);

		// media screen parameters
		MediaScreenParam = new PdfDictionary(this);
		Rendition.AddDictionary("/SP", MediaScreenParam);

		// media screen parameters BE
		MediaScreenParamBE = new PdfDictionary(this);
		MediaScreenParam.AddDictionary("/BE", MediaScreenParamBE);

		// Section 8.5 page 669 table 8.64
		// type of action playing multimedia content
		Dictionary.Add("/S", "/Rendition");

		// media clip data page 762
		Rendition.Add("/S", "/MR");

		// Table 9.6 page 762
		MediaClip.AddPdfString("/CT", MimeType);
		MediaClip.AddIndirectReference("/D", MediaFile);
		MediaClip.Add("/S", "/MCD");
		MediaClip.Add("/Type", "/MediaClip");

		// Operation to perform when action is triggered. Valid options are 0 or 4
		// OP=0 force the Rendition dictionary to take over the annotation
		Dictionary.Add("/OP", "0");

		// allow reader to always create temporary file (other options do not work)
		// Media clip dictionary T 9.10 page 766
		TempFilePermissions.AddPdfString("/TF", "TEMPALWAYS");

		// do not display control
		MediaPlayBE.AddBoolean("/C", false); 

		// repeat count of 1
		MediaPlayBE.Add("/RC", "1.0"); 

		// media scale and position within annotation rectangle PDF default is 5
		// /F=2 strech media to fit annotation
		MediaPlayBE.Add("/F", "2");

		// play rendition in annotation rectangle
		MediaScreenParamBE.Add("/W", "3");

		// exit
		return;
		}

	/// <summary>
	/// Display media player controls
	/// </summary>
	/// <param name="Display">Display/no display command</param>
	public void DisplayControls
			(
			bool		Display
			)
		{
		MediaPlayBE.AddBoolean("/C", Display); 
		return;
		}

	/// <summary>
	/// Repeat count
	/// </summary>
	/// <param name="Count">Count</param>
	/// <remarks>
	///	<para>
	///	Count of zero means replay indefinitly.
	///	</para>
	///	<para>
	///	Negative count is an error.
	///	</para>
	///	<para>
	///	Count is a real (float) number. The PDF specification does not
	///	define how non integers are treated.
	///	</para>
	/// </remarks>
	public void RepeatCount
			(
			float		Count
			)
		{
		MediaPlayBE.AddReal("/RC", Count); 
		return;
		}

	/// <summary>
	/// Set media window
	/// </summary>
	/// <param name="MediaWindow">Media window</param>
	/// <param name="Width">Floating window width</param>
	/// <param name="Height">Floating window height</param>
	/// <param name="Position">Floating window position</param>
	/// <param name="TitleBar">Floating window title bar</param>
	/// <param name="Resize">Floating window resize</param>
	/// <param name="Title">Floating window title</param>
	/// <remarks>
	/// <para>
	/// All optional arguments are applicable to floating window only.
	/// </para>
	/// </remarks>
	public void SetMediaWindow
			(
			MediaWindow			MediaWindow,
			int				Width = 0,
			int				Height = 0,
			WindowPosition		Position = WindowPosition.Center,
			WindowTitleBar		TitleBar = WindowTitleBar.TitleBarWithCloseButton,
			WindowResize		Resize = WindowResize.KeepAspectRatio,
			string				Title = null
			)
		{
		// set media play window code
		MediaScreenParamBE.AddInteger("/W", (int) MediaWindow);

		// all choices but floating window
		if(MediaWindow != MediaWindow.Floating)
			{
			MediaScreenParamBE.Remove("/F");
			return;
			}

		// play rendition in floating window
		// Table 9.19 page 774
		PdfDictionary FloatingWindow = new PdfDictionary(this);
		MediaScreenParamBE.AddDictionary("/F", FloatingWindow);

		// window's dimensions
		if(Width == 0 || Height == 0)
			{
			Width = 320;
			Height = 180;
			}
		FloatingWindow.AddFormat("/D", "[{0} {1}]", Width, Height);

		FloatingWindow.AddInteger("/P", (int) Position);

		FloatingWindow.AddBoolean("/T", TitleBar != WindowTitleBar.NoTitleBar);
		if(TitleBar == WindowTitleBar.NoTitleBar) return;

		FloatingWindow.AddInteger("/R", (int) Resize);

		if(Title != null)
			{
			FloatingWindow.AddFormat("/TT", "[{0} {1}]", Document.TextToPdfString(string.Empty, this), Document.TextToPdfString(Title, this));
			}

		return;
		}

	/// <summary>
	/// Scale media
	/// </summary>
	/// <param name="ScaleCode">Scale media code</param>
	public void ScaleMedia
			(
			ScaleMediaCode	ScaleCode
			)
		{
		// media scale and position within annotation rectangle
		// Value 0 to 5 How to scale the media to fit annotation area page 770 T 9.15
		MediaPlayBE.AddInteger("/F", (int) ScaleCode);
		return;
		}

	/// <summary>
	/// Initial media operation
	/// </summary>
	/// <param name="OperationCode">Media operation code enumeration</param>
	public void InitialMediaOperation
			(
			MediaOperation	OperationCode
			)
		{
		// Operation to perform when rendition action is triggered.
		// Page 669 T 8.64 S 8.5
		Dictionary.AddInteger("/OP", (int) OperationCode);
		return;
		}

	/// <summary>
	/// Media temporary file permission
	/// </summary>
	/// <param name="Permission">Permissions flags</param>
	/// <remarks><para>
	/// The PDF reader must save the media file to a temporary file
	/// in order for the player to play it.
	/// </para></remarks>
	public void MediaTempFilePermission
			(
			TempFilePermission	Permission
			)
		{
		// allow reader to always create temporary file (other options do not work)
		// Media clip dictionary T 9.10 page 766
		TempFilePermissions.AddPdfString("/TF", Permission.ToString());
		return;
		}
	}
}
