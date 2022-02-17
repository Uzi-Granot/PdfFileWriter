/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter II
//	PDF File Write C# Class Library.
//
//	ArcToBezier
//	Convert eliptical arc to Bezier segments.
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
	/// Arc type for DrawArc method
	/// </summary>
	public enum ArcType
		{
		/// <summary>
		/// Small arc drawn in counter clock wise direction
		/// </summary>
		SmallCounterClockWise,

		/// <summary>
		/// Small arc drawn in clock wise direction
		/// </summary>
		SmallClockWise,

		/// <summary>
		/// Large arc drawn in counter clock wise direction
		/// </summary>
		LargeCounterClockWise,

		/// <summary>
		/// Large arc drawn in clock wise direction
		/// </summary>
		LargeClockWise,
		}

	/// <summary>
	/// Convert eliptical arc to Bezier segments
	/// </summary>
	public static class PdfArcToBezier
		{
		////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Create eliptical arc
		/// </summary>
		/// <param name="ArcStart">Arc start point</param>
		/// <param name="ArcEnd">Arc end point</param>
		/// <param name="Radius">RadiusX as width and RadiusY as height</param>
		/// <param name="Rotate">X axis rotation angle in radians</param>
		/// <param name="Type">Arc type enumeration</param>
		/// <returns>Array of points.</returns>
		////////////////////////////////////////////////////////////////////
		public static PointD[] CreateArc
				(
				PointD ArcStart,
				PointD ArcEnd,
				SizeD Radius,
				double Rotate,
				ArcType Type
				)
			{
			PointD[] SegArray;
			double ScaleX = Radius.Width / Radius.Height;

			// circular arc
			if(Math.Abs(ScaleX - 1.0) < 0.000001)
				{
				SegArray = CircularArc(ArcStart, ArcEnd, Radius.Height, Type);
				}
			// eliptical arc
			else if(Rotate == 0.0)
				{
				PointD ScaleStart = new PointD(ArcStart.X / ScaleX, ArcStart.Y);
				PointD ScaleEnd = new PointD(ArcEnd.X / ScaleX, ArcEnd.Y);
				SegArray = CircularArc(ScaleStart, ScaleEnd, Radius.Height, Type);
				foreach(PointD Seg in SegArray) Seg.X *= ScaleX;
				}
			// eliptical arc rotated
			else
				{
				double CosR = Math.Cos(Rotate);
				double SinR = Math.Sin(Rotate);
				PointD ScaleStart = new PointD((CosR * ArcStart.X - SinR * ArcStart.Y) / ScaleX, SinR * ArcStart.X + CosR * ArcStart.Y);
				PointD ScaleEnd = new PointD((CosR * ArcEnd.X - SinR * ArcEnd.Y) / ScaleX, SinR * ArcEnd.X + CosR * ArcEnd.Y);
				SegArray = CircularArc(ScaleStart, ScaleEnd, Radius.Height, Type);
				foreach(PointD Seg in SegArray)
					{
					double X = Seg.X * ScaleX;
					Seg.X = CosR * X + SinR * Seg.Y;
					Seg.Y = -SinR * X + CosR * Seg.Y;
					}
				}

			// replace start and end with original points to eliminate rounding errors
			SegArray[0].X = ArcStart.X;
			SegArray[0].Y = ArcStart.Y;
			SegArray[SegArray.Length - 1].X = ArcEnd.X;
			SegArray[SegArray.Length - 1].Y = ArcEnd.Y;
			return SegArray;
			}

		/// <summary>
		/// Create circular arc
		/// </summary>
		/// <param name="ArcStart">Arc starting point</param>
		/// <param name="ArcEnd">Arc ending point</param>
		/// <param name="Radius">Arc radius</param>
		/// <param name="Type">Arc type</param>
		/// <returns>Array of points.</returns>
		internal static PointD[] CircularArc
				(
				PointD ArcStart,
				PointD ArcEnd,
				double Radius,
				ArcType Type
				)
			{
			// chord line from start point to end point
			double ChordDeltaX = ArcEnd.X - ArcStart.X;
			double ChordDeltaY = ArcEnd.Y - ArcStart.Y;
			double ChordLength = Math.Sqrt(ChordDeltaX * ChordDeltaX + ChordDeltaY * ChordDeltaY);

			// test radius
			if(2 * Radius < ChordLength) throw new ApplicationException("Radius too small.");

			// line perpendicular to chord at mid point
			// distance from chord mid point to center of circle
			double ChordToCircleLen = Math.Sqrt(Radius * Radius - ChordLength * ChordLength / 4);
			double ChordToCircleCos = -ChordDeltaY / ChordLength;
			double ChordToCircleSin = ChordDeltaX / ChordLength;
			if(Type == ArcType.SmallClockWise || Type == ArcType.LargeCounterClockWise)
				{
				ChordToCircleCos = -ChordToCircleCos;
				ChordToCircleSin = -ChordToCircleSin;
				}

			// circle center
			double CenterX = (ArcStart.X + ArcEnd.X) / 2 + ChordToCircleLen * ChordToCircleCos;
			double CenterY = (ArcStart.Y + ArcEnd.Y) / 2 + ChordToCircleLen * ChordToCircleSin;

			// arc angle
			double ArcAngle = 2 * Math.Asin(ChordLength / (2 * Radius));
			if(ArcAngle < 0.001) throw new ApplicationException("Angle too small");
			if(Type == ArcType.LargeCounterClockWise || Type == ArcType.LargeClockWise) ArcAngle = 2 * Math.PI - ArcAngle;

			// segment array
			PointD[] SegArray;

			// one segment equal or less than 90 deg
			if(ArcAngle < Math.PI / 2 + 0.001)
				{
				double K1 = 4 * (1 - Math.Cos(ArcAngle / 2)) / (3 * Math.Sin(ArcAngle / 2));
				if(Type == ArcType.LargeClockWise || Type == ArcType.SmallClockWise) K1 = -K1;
				SegArray = new PointD[4];
				SegArray[0] = ArcStart;
				SegArray[1] = new PointD(ArcStart.X - K1 * (ArcStart.Y - CenterY), ArcStart.Y + K1 * (ArcStart.X - CenterX));
				SegArray[2] = new PointD(ArcEnd.X + K1 * (ArcEnd.Y - CenterY), ArcEnd.Y - K1 * (ArcEnd.X - CenterX));
				SegArray[3] = ArcEnd;
				return SegArray;
				}

			// 2, 3 or 4 segments
			int Segments = (int) (ArcAngle / (0.5 * Math.PI)) + 1;
			double SegAngle = ArcAngle / Segments;
			double K = 4 * (1 - Math.Cos(SegAngle / 2)) / (3 * Math.Sin(SegAngle / 2));
			if(Type == ArcType.LargeClockWise || Type == ArcType.SmallClockWise)
				{
				K = -K;
				SegAngle = -SegAngle;
				}

			// segments array
			SegArray = new PointD[3 * Segments + 1];
			SegArray[0] = new PointD(ArcStart.X, ArcStart.Y);

			// angle from cricle center to start point
			double SegStartX = ArcStart.X;
			double SegStartY = ArcStart.Y;
			double StartAngle = Math.Atan2(ArcStart.Y - CenterY, ArcStart.X - CenterX);

			// process all segments
			int SegEnd = SegArray.Length;
			for(int Seg = 1; Seg < SegEnd; Seg += 3)
				{
				double EndAngle = StartAngle + SegAngle;
				double SegEndX = CenterX + Radius * Math.Cos(EndAngle);
				double SegEndY = CenterY + Radius * Math.Sin(EndAngle);
				SegArray[Seg] = new PointD(SegStartX - K * (SegStartY - CenterY), SegStartY + K * (SegStartX - CenterX));
				SegArray[Seg + 1] = new PointD(SegEndX + K * (SegEndY - CenterY), SegEndY - K * (SegEndX - CenterX));
				SegArray[Seg + 2] = new PointD(SegEndX, SegEndY);
				SegStartX = SegEndX;
				SegStartY = SegEndY;
				StartAngle = EndAngle;
				}
			return SegArray;
			}
		}
	}
