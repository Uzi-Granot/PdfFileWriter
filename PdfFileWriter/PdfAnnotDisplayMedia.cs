/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	Annotation display media
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

namespace PdfFileWriter
	{
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
	/// Display video or play sound class
	/// </summary>
	public class PdfAnnotDisplayMedia : PdfAnnotation
		{
		internal PdfEmbeddedFile MediaFile;
		internal PdfObject DisplayMedia;
		internal PdfDictionary Rendition;
		internal PdfDictionary FloatingWindow;
		internal PdfDictionary MediaClip;
		internal PdfDictionary MediaPlay;
		internal PdfDictionary MediaPlayBE;
		internal PdfDictionary MediaScreenParam;
		internal PdfDictionary MediaScreenParamBE;
		internal PdfDictionary TempFilePermissions;

		/// <summary>
		/// Display media annotation action constructor
		/// </summary>
		/// <param name="Document">Annotation page</param>
		/// <param name="MediaFile">PdfEmbeddedFile media contents</param>
		public PdfAnnotDisplayMedia
				(
				PdfDocument Document,
				PdfEmbeddedFile MediaFile
				) : base(Document, "/Screen")
			{
			// save media file
			this.MediaFile = MediaFile;

			// display media object
			DisplayMedia = new PdfObject(Document);

			// rendition dictionary page 759 Section 9.1.2 Table 9.1
			Rendition = new PdfDictionary(this);
			DisplayMedia.Dictionary.AddDictionary("/R", Rendition);

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
			DisplayMedia.Dictionary.Add("/S", "/Rendition");

			// media clip data page 762
			Rendition.Add("/S", "/MR");

			// Table 9.6 page 762
			MediaClip.AddPdfString("/CT", MediaFile.MimeType);
			MediaClip.AddIndirectReference("/D", MediaFile);
			MediaClip.Add("/S", "/MCD");
			MediaClip.Add("/Type", "/MediaClip");

			// Operation to perform when action is triggered. Valid options are 0 or 4
			// OP=0 force the Rendition dictionary to take over the annotation
			DisplayMedia.Dictionary.Add("/OP", "0");

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
			MediaScreenParamBE.AddInteger("/W", (int) MediaWindow.Annotation);

			// action reference dictionary
			Dictionary.AddIndirectReference("/A", DisplayMedia);

			// add annotation reference
			DisplayMedia.Dictionary.AddIndirectReference("/AN", this);
			return;
			}

		/// <summary>
		/// Display media annotation action constructor
		/// </summary>
		public PdfAnnotDisplayMedia
				(
				PdfAnnotDisplayMedia Other
				) : base(Other.Document, "/Screen")
			{
			// save media file
			MediaFile = Other.MediaFile;

			// display media object
			DisplayMedia = new PdfObject(Other.Document);

			// rendition dictionary page 759 Section 9.1.2 Table 9.1
			Rendition = Other.Rendition;
			DisplayMedia.Dictionary.AddDictionary("/R", Rendition);

			// floating window
			FloatingWindow = Other.FloatingWindow;

			// media clip
			MediaClip = Other.MediaClip;

			// Media clip dictionary T 9.9
			TempFilePermissions = Other.TempFilePermissions;

			// media play
			MediaPlay = Other.MediaPlay;

			// media play BE
			MediaPlayBE = Other.MediaPlayBE;

			// media screen parameters
			MediaScreenParam = Other.MediaScreenParam;

			// media screen parameters BE
			MediaScreenParamBE = Other.MediaScreenParamBE;

			// Section 8.5 page 669 table 8.64
			// type of action playing multimedia content
			DisplayMedia.Dictionary.Add("/S", "/Rendition");

			// Operation to perform when action is triggered. Valid options are 0 or 4
			// OP=0 force the Rendition dictionary to take over the annotation
			DisplayMedia.Dictionary.Add("/OP", "0");

			// action reference dictionary
			Dictionary.AddIndirectReference("/A", DisplayMedia);

			// add annotation reference
			DisplayMedia.Dictionary.AddIndirectReference("/AN", this);

			// copy base values
			base.CreateCopy(Other);
			return;
			}

		/// <summary>
		/// Activate media when page becomes visible
		/// </summary>
		public void ActivateWhenPageIsVisible()
			{ 
			// add AA dictionary
			PdfDictionary AADict = Dictionary.GetOrAddDictionary("/AA");

			// add object reference number
			AADict.AddIndirectReference("/PV", DisplayMedia);
			return;
			}
	
		/// <summary>
		/// Media window type
		/// </summary>
		public MediaWindow MediaWindowType
			{
			set
				{
				// set media play window position
				MediaScreenParamBE.AddInteger("/W", (int) value);

				// floating window
				if(value == MediaWindow.Floating)
					{
					// play rendition in floating window
					// Table 9.19 page 774
					FloatingWindow = new PdfDictionary(this);

					// add default values
					FloatingWindow.Add("/D", "[640 360]");

					// position
					FloatingWindow.AddInteger("/P", (int) WindowPosition.Center);

					// title bar
					FloatingWindow.AddBoolean("/T", true); 

					// close window button
					FloatingWindow.AddBoolean("/UC", true);

					// resize
					FloatingWindow.AddInteger("/R", (int) WindowResize.KeepAspectRatio);

					// add to media screen parameters dictionary
					MediaScreenParamBE.AddDictionary("/F", FloatingWindow);
					}

				// all choices but floating window
				else
					{
					FloatingWindow = null;
					MediaScreenParamBE.Remove("/F");
					}
				return;
				}
			}

		/// <summary>
		/// Set floating window size
		/// </summary>
		/// <param name="Width">Width in pixels</param>
		/// <param name="Height">Height in pixels</param>
		public void FloatingWindowSize
				(
				int Width,
				int Height
				)
			{
			if(FloatingWindow == null) throw new ApplicationException("Set MediaWindowType = MediaWindow.Floating");
			FloatingWindow.AddFormat("/D", "[{0} {1}]", Width, Height);
			return;
			}

		/// <summary>
		/// Set floating window position
		/// </summary>
		public WindowPosition FloatingWindowPosition
			{
			set
				{
				if(FloatingWindow == null) throw new ApplicationException("Set MediaWindowType = MediaWindow.Floating");
				FloatingWindow.AddInteger("/P", (int) value);
				return;
				}
			}

		/// <summary>
		/// Set floating window title bar
		/// </summary>
		public bool FloatingWindowTitleBar
			{
			set
				{
				if(FloatingWindow == null) throw new ApplicationException("Set MediaWindowType = MediaWindow.Floating");
				FloatingWindow.AddBoolean("/T", value); 
				return;
				}
			}

		/// <summary>
		/// Set floating window close button
		/// </summary>
		public bool FloatingWindowCloseButton
			{
			set
				{
				if(FloatingWindow == null) throw new ApplicationException("Set MediaWindowType = MediaWindow.Floating");
				FloatingWindow.AddBoolean("/UC", value); 
				return;
				}
			}

		/// <summary>
		/// Set floating window's title text
		/// </summary>
		public string FloatingWindowTitleText
			{
			set
				{
				if(FloatingWindow == null) throw new ApplicationException("Set MediaWindowType = MediaWindow.Floating");
				FloatingWindow.AddFormat("/TT", "[{0} {1}]", TextToPdfString(string.Empty, this), TextToPdfString(value, this));
				return;
				}
			}

		/// <summary>
		/// Set window resize
		/// </summary>
		public WindowResize FloatingWindowResize
			{
			set
				{
				if(FloatingWindow == null) throw new ApplicationException("Set MediaWindowType = MediaWindow.Floating");
				FloatingWindow.AddInteger("/R", (int) value);
				return;
				}
			}
		/// <summary>
		/// Display media player controls
		/// </summary>
		public bool DisplayControls
			{
			set
				{
				MediaPlayBE.AddBoolean("/C", value);
				return;
				}
			}

		/// <summary>
		/// Repeat count
		/// </summary>
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
		public float RepeatCount
			{
			set
				{
				MediaPlayBE.AddReal("/RC", value);
				return;
				}
			}

		/// <summary>
		/// Scale media code
		/// </summary>
		public ScaleMediaCode ScaleMedia
			{
			set
				{
				// media scale and position within annotation rectangle
				// Value 0 to 5 How to scale the media to fit annotation area page 770 T 9.15
				MediaPlayBE.AddInteger("/F", (int) value);
				return;
				}
			}

		/// <summary>
		/// Initial media operation code
		/// </summary>
		public MediaOperation InitialMediaOperation
			{
			set
				{ 
				// Operation to perform when rendition action is triggered.
				// Page 669 T 8.64 S 8.5
				Dictionary.AddInteger("/OP", (int) value);
				return;
				}
			}

		/// <summary>
		/// Media temporary file permission flag
		/// </summary>
		/// <remarks><para>
		/// The PDF reader must save the media file to a temporary file
		/// in order for the player to play it.
		/// </para></remarks>
		public TempFilePermission MediaTempFilePermission
			{
			set
				{ 
				// allow reader to always create temporary file (other options do not work)
				// Media clip dictionary T 9.10 page 766
				TempFilePermissions.AddPdfString("/TF", value.ToString());
				return;
				}
			}
		}
	}
