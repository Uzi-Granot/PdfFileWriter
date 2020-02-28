using PdfFileWriter;
using System;
using System.Diagnostics;
using System.Drawing;
using SysMedia = System.Windows.Media;

namespace TestPdfFileWriter
{
class ProgramTestExample
	{
	// define font
	PdfFont ArialFont;
	double FontSize;
	double Ascent;
	double Descent;
	double FontHeight;

	PdfDocument Document;
	double CenterX = 4.25;
	double CenterY = 5.5;

	public void Test
			(
			Boolean Debug,
			String	InputFileName
			)
		{
		// create document
		using(Document = new PdfDocument(PaperType.Letter, false, UnitOfMeasure.Inch, InputFileName))
			{
			// define font
			ArialFont = PdfFont.CreatePdfFont(Document, "Arial", FontStyle.Bold);

			FontSize = 24;
			Ascent = ArialFont.AscentPlusLeading(FontSize);
			Descent = ArialFont.DescentPlusLeading(FontSize);
			FontHeight = ArialFont.LineSpacing(FontSize);

			OnePage(3, 5);
			OnePage(4, 4);
			OnePage(4, 6);
			OnePage(5, 7);

			// create pdf file
			Document.CreateFile();

			// start default PDF reader and display the file
			Process Proc = new Process();
			Proc.StartInfo = new ProcessStartInfo(InputFileName);
			Proc.Start();
			}
		return;
		}

	public void OnePage
			(
			int Cols,
			int Rows
			)
		{
		// Add new page
		PdfPage Page = new PdfPage(Document);

		// Add contents to page
		PdfContents Contents = new PdfContents(Page);

		double Left = CenterX - 0.5 * Cols;
		double Right = CenterX + 0.5 * Cols;
		double Top = CenterY + 0.5 * Rows;
		double Bottom = CenterY - 0.5 * Rows;

		Contents.DrawText(ArialFont, FontSize, CenterX, Top + Descent + 0.2, TextJustify.Center, Cols == Rows ? "Square" : "Rectangle");

		// save state
		Contents.SaveGraphicsState();

		Contents.SetLineWidth(0.1);
		Contents.SetColorStroking(Color.Black);
		Contents.SetColorNonStroking(Color.LightBlue);
		Contents.DrawRectangle(Left, Bottom, Cols, Rows, PaintOp.CloseFillStroke);
		Contents.RestoreGraphicsState();

		Contents.SaveGraphicsState();
		Contents.SetLineWidth(0.04);
		for(int Row = 0; Row <= Rows; Row++)
			{
			Contents.DrawLine(Left - 0.25, Bottom + Row, Right, Bottom + Row);
			Contents.DrawText(ArialFont, FontSize, Left - 0.35, Bottom + Row - Descent, TextJustify.Right, Row.ToString());
			}

		for(int Col = 0; Col <= Cols; Col++)
			{
			Contents.DrawLine(Left + Col, Bottom - 0.25, Left + Col, Top);
			Contents.DrawText(ArialFont, FontSize, Left + Col, Bottom - 0.25 - FontHeight, TextJustify.Center, Col.ToString());
			}

		for(int Index = 0; Index < Rows * Cols; Index++)
			{
			int Row = Index / Cols;
			int Col = Index % Cols;
			Contents.DrawText(ArialFont, FontSize, Left + 0.5 + Col, Top - 0.5 - Descent - Row, TextJustify.Center, (Index + 1).ToString());
			}

		Contents.DrawText(ArialFont, FontSize, CenterX, Bottom - 1.0, TextJustify.Center, "Area");
		Contents.DrawText(ArialFont, FontSize, CenterX, Bottom - 1.4, TextJustify.Center, string.Format("{0} X {1} = {2}", Rows, Cols, Rows * Cols));

		// restore graphics state
		Contents.RestoreGraphicsState();
		return;
		}

/*
	public void Test
			(
			Boolean Debug,
			String	InputFileName
			)
		{
		// fish artwork from your favorite wpf or svg editing software (AI, Blend, Expression Design)
		// for hand writing minipath strings please see SVG or WPF reference on it (for example, https://developer.mozilla.org/en-US/docs/Web/SVG/Tutorial/Paths)
		string FishPathText = "M 73.302,96.9831C 88.1275,96.9831 100.146,109.002 100.146,123.827C 100.146,138.653 88.1275,150.671 73.302,150.671" +
			"C 58.4764,150.671 46.458,138.653 46.458,123.827C 46.458,109.002 58.4764,96.9831 73.302,96.9831 Z M 80.3771,118.625" +
			"C 87.8473,118.625 93.9031,124.681 93.9031,132.151C 93.9031,139.621 87.8473,145.677 80.3771,145.677C 72.9068,145.677 66.851,139.621 66.851,132.151" +
			"C 66.851,124.681 72.9069,118.625 80.3771,118.625 Z M 124.936,229.489L 124.936,230.05C 142.757,230.05 157.205,187.542 157.205,135.105" +
			"C 157.205,82.6682 142.757,40.1597 124.936,40.1597L 124.936,40.7208C 140.016,40.7208 152.241,82.9781 152.241,135.105" +
			"C 152.241,187.232 140.016,229.489 124.936,229.489 Z M 155.904,33.5723C 168.593,40.8964 181.282,48.2205 184.749,59.0803" +
			"C 188.216,69.9401 182.461,84.3356 176.705,98.7312C 187.217,82.7698 197.73,66.8085 194.263,55.9487C 190.796,45.0889 173.35,39.3306 155.904,33.5723 Z " +
			"M 221.06,47.217C 231.336,54.9565 241.612,62.6958 243.473,72.5309C 245.334,82.366 238.779,94.2968 232.224,106.228" +
			"C 243.092,93.4406 253.96,80.6536 252.099,70.8185C 250.238,60.9834 235.649,54.1002 221.06,47.217 Z M 190.088,103.489" +
			"C 200.631,113.663 211.175,123.836 211.914,135.212C 212.654,146.588 203.591,159.166 194.527,171.744C 208.585,158.796 222.643,145.848 221.903,134.472" +
			"C 221.163,123.096 205.625,113.293 190.088,103.489 Z M 227.222,175.988C 233.667,185.231 240.112,194.474 238.981,203.168" +
			"C 237.849,211.862 229.142,220.007 220.434,228.153C 232.965,220.47 245.497,212.787 246.628,204.093C 247.759,195.399 237.49,185.693 227.222,175.988 Z " +
			"M 176.183,170.829C 182.085,184.24 187.987,197.65 184.36,208.457C 180.734,219.265 167.58,227.47 154.426,235.675C 172.342,229.02 190.258,222.366 193.884,211.558" +
			"C 197.511,200.75 186.847,185.79 176.183,170.829 Z M 253.24,114.388C 261.541,123.744 269.842,133.1 269.72,142.831" +
			"C 269.598,152.561 261.052,162.667 252.506,172.773C 265.327,162.683 278.148,152.592 278.27,142.861C 278.392,133.13 265.816,123.759 253.24,114.388 Z " +
			"M 19.3722,114.348C 33.8527,95.7363 61.0659,59.7511 97.8151,40.6822C 117.532,30.4513 139.994,25.0899 164.816,24.6372" +
			"C 165.876,24.1644 167.083,23.6525 168.454,23.0983C 181.841,17.6879 210.836,8.25439 232.2,4.09256C 253.564,-0.0693054 267.298,1.04053 273.749,4.99429" +
			"C 280.2,8.94803 279.368,15.7458 278.743,24.4856C 278.119,33.2255 277.703,43.9076 276.94,49.1099C 276.927,49.2001 276.913,49.2887 276.9,49.3756" +
			"C 318.05,66.1908 360.168,89.8268 395.044,112.964C 408.876,122.14 421.569,131.238 433.26,140.058C 439.423,134.13 445.322,128.267 450.904,122.587" +
			"C 478.22,94.7909 497.963,71.3744 513.5,56.0696C 529.037,40.7648 540.368,33.5717 541.331,39.3597C 542.295,45.1478 532.891,63.9171 528.998,87.7075" +
			"C 525.105,111.498 526.722,140.309 533.661,167.068C 540.599,193.827 552.858,218.532 549.803,224.507C 546.748,230.482 528.378,217.727 502.239,196.166" +
			"C 483.768,180.932 461.418,161.301 433.26,140.058C 409.264,163.142 381.252,187.219 352.261,205.363C 315.824,228.167 277.841,241.6 230.108,245.486" +
			"C 182.376,249.372 124.895,243.713 84.9205,225.782C 44.946,207.851 22.4781,177.648 11.4752,160.545C 0.472214,143.443 0.934143,139.44 2.03903,136.819" +
			"C 3.14392,134.199 4.89172,132.96 19.3722,114.348 Z ";

        // water artwork 		
		string WavePathOriginal = "M 0.000854492,723.999L 1106,723.999L 1106,616.629C 1025.42,656.405 941.978,687.324 846.084,679.721C 721.562,669.847 576.045,595.015 425.822,588.779" +
			"C 286.673,583.003 143.486,636.082 0.000854492,693.5L 0.000854492,723.999 Z M 423.35,26.0787C 573.573,32.3146 719.09,107.146 843.612,117.02C 940.487,124.701 1024.65,93.0672 1106,52.7042" +
			"L 1106,-1.90735e-005L 0.000854492,-1.90735e-005L 0.000854492,129.811C 142.658,72.7739 285,20.3355 423.35,26.0787 Z M 6.10352e-005,545.976C 143.485,488.558 286.672,435.478 425.822,441.255" +
			"C 576.045,447.491 721.562,522.322 846.084,532.196C 941.978,539.8 1025.42,508.88 1106,469.104L 1106,200.228C 1024.65,240.592 940.486,272.226 843.611,264.544" +
			"C 719.089,254.671 573.572,179.839 423.349,173.603C 284.999,167.86 142.657,220.298 6.10352e-005,277.335L 6.10352e-005,545.976 Z";


		// create document
		using(PdfDocument Document = new PdfDocument(PaperType.Letter, false, UnitOfMeasure.Inch, InputFileName))
			{
			// define font
			PdfFont ArialNormal = PdfFont.CreatePdfFont(Document, "Arial", FontStyle.Regular);

			// Add new page
			PdfPage Page = new PdfPage(Document);

			// Add contents to page
			PdfContents Contents = new PdfContents(Page);

			// translate origin
			Contents.SaveGraphicsState();

			// test draw
//			string TestDraw1 = "M 0 0 L 1000 0";
			string TestDraw1 = "M 0 0 L 100 200 L 200 0 Z M 0 200 L 100 0 L 200 200 Z";
//			string TestDraw1 = "M 0 0 A 400 400 0 0 0 100 200 A 400 400 0 0 0 200 0 A 400 400 0 0 0 0 0 Z" +
//				"M 0 250 A 400 400 0 0 1 100 -50 A 400 400 0 0 1 250 250 A 400 400 0 0 1 0 250 Z";
			//string TestDraw1 = "M 0 0 L 100 200 L 200 0";
			DrawWPFPath TestPath1 = new DrawWPFPath(TestDraw1, YAxisDirection.Up);
			TestPath1.SetBrush(Color.DarkMagenta);
			SysMedia.Pen Pen = new SysMedia.Pen(new SysMedia.SolidColorBrush(SysMedia.Colors.DarkBlue), 2);
			Pen.StartLineCap = SysMedia.PenLineCap.Flat;
			Pen.LineJoin = SysMedia.PenLineJoin.Bevel;
			Pen.DashStyle = new SysMedia.DashStyle(new double[] {1.0}, 0.0);
			TestPath1.SetPen(Pen);
			Contents.DrawWPFPath(TestPath1, 1.0, 4.0, 5.0, 3.0);

			string TestDraw2 = "M 0 0 L 200 200 A 200 200 0 1 0 0 0 Z";
			DrawWPFPath TestPath2 = new DrawWPFPath(TestDraw2, YAxisDirection.Up);
//			string TestDraw = "M 0 200 L 200 0 A 200 200 0 1 1 0 200";
//			DrawWPFPath TestPath = new DrawWPFPath(TestDraw, YAxisDirection.Down);
			TestPath2.SetPenWidth(0.05);
			TestPath2.SetPen(Color.Chocolate);
			Contents.DrawWPFPath(TestPath2, 4.25, 4.0, 3.0, 3.0, ContentAlignment.BottomLeft);

			// load fish path
			DrawWPFPath FishPath = new DrawWPFPath(FishPathText, YAxisDirection.Down);

			// set pen for both fish
			FishPath.SetPen(Color.FromArgb(255, 255, 80, 0));
			FishPath.SetPenWidth(0.02);

//			PdfTilingPattern BrickPattern = PdfTilingPattern.SetBrickPattern(Document, 0.25, Color.LightYellow, Color.SandyBrown);
//			FishPath.SetBrush(BrickPattern);

			// draw small fish
			FishPath.SetBrush(Color.FromArgb(255, 67, 211, 216));
			Contents.DrawWPFPath(FishPath, 2.5, 5.75, 4.5, 3.0, ContentAlignment.TopRight);

			// big fish drawing area
			Color[] BigFishBrushColor = new Color[] {Color.FromArgb(0xff, 0xff, 0x50, 0), Color.FromArgb(0xff, 0x27, 0xda, 0xff)};
			PdfRadialShading RadialShading = new PdfRadialShading(Document, new PdfShadingFunction(Document, BigFishBrushColor));
			RadialShading.SetGradientDirection(0.15, 0.5, 0.0, 0.25, 0.5, 1.3, MappingMode.Relative);
			FishPath.SetBrush(RadialShading, 1.0);
			Contents.DrawWPFPath(FishPath, 1.5, 2.0, 5.5, 5.5, ContentAlignment.BottomLeft);

			// load wave
			DrawWPFPath WavePath = new DrawWPFPath(WavePathOriginal, YAxisDirection.Up);

			// draw wave
			Color[] WaveBrushColor = new Color[] {Color.Cyan, Color.DarkBlue};
			PdfAxialShading AxialShading = new PdfAxialShading(Document, new PdfShadingFunction(Document, WaveBrushColor));
			AxialShading.SetAxisDirection(0.0, 1.0, 1.0, 0.0, MappingMode.Relative);
			WavePath.SetBrush(AxialShading, 0.55);
			Contents.DrawWPFPath(WavePath, 1.0, 1.0, 6.5, 9.0);

			// restore graphics state
			Contents.RestoreGraphicsState();

			// create pdf file
			Document.CreateFile();

			// start default PDF reader and display the file
			Process Proc = new Process();
			Proc.StartInfo = new ProcessStartInfo(InputFileName);
			Proc.Start();
			}

		return;
		}
*/
	/*
	////////////////////////////////////////////////////////////////////
	// Create article's example test PDF document
	////////////////////////////////////////////////////////////////////
	
	public void Test
			(
			Boolean Debug,
			String	FileName
			)
		{
		String Block1 =
			"The PDF File Writer C# class library PdfFileWriter allows you to create PDF files " +
			"directly from your.net application. The library shields you from the details of " +
			"the PDF file structure. To use the library, you need to add a reference to the " +
			"attached PdfFileWriter.dll class library file, add a using PdfFileWriter statement " +
			"in every source file that is using the library and include the PdfFileWriter.dll " +
			"with your distribution. For more details go to 2.18 Installation. Alternatively, " +
			"you can include the source code of the library with your application and avoid " +
			"the need to distribute a separate data link library file. The minimum development " +
			"requirement is.NET Framework 4.0 (Visual Studio 2010).\n" +
			"01234567890\n\n";

		String Block2 =
			"Το αρχείο PDF συγγραφέας C # τάξη PdfFileWriter βιβλιοθήκη σας επιτρέπει να δημιουργήσετε " +
			"αρχεία PDF απευθείας από την εφαρμογή.net σας. Η βιβλιοθήκη σας προστατεύει από τις " +
			"λεπτομέρειες της δομής του αρχείου PDF. Για να χρησιμοποιήσετε τη βιβλιοθήκη , θα πρέπει " +
			"να προσθέσετε μια αναφορά στο συνημμένο αρχείο PdfFileWriter.dll τη βιβλιοθήκη της τάξης , " +
			"προσθέστε μια δήλωση χρησιμοποιώντας PdfFileWriter σε κάθε αρχείο πηγαίου κώδικα που " +
			"χρησιμοποιεί τη βιβλιοθήκη και περιλαμβάνουν την PdfFileWriter.dll με τη διανομή σας. " +
			"Για περισσότερες λεπτομέρειες πηγαίνετε στο 2.18 Εγκατάσταση. Εναλλακτικά , μπορείτε " +
			"να συμπεριλάβετε τον πηγαίο κώδικα της βιβλιοθήκης με την αίτησή σας και να αποφύγετε την " +
			"ανάγκη να διανείμει ένα ξεχωριστό αρχείο της βιβλιοθήκης ζεύξης δεδομένων. Η ελάχιστη " +
			"απαίτηση της ανάπτυξης είναι το.NET Framework 4.0 (Visual Studio 2010).\n\n";

	string Block3 =
			"Le fichier PDF Writer C # bibliothèque de classes PdfFileWriter vous permet de créer des fichiers PDF " +
			"directement à partir de votre application.NET. La bibliothèque vous protège des détails de la PDF " +
			"structure de fichier. Pour utiliser la bibliothèque , vous devez ajouter une référence à la joint " +
			"Fichier de bibliothèque de classe PdfFileWriter.dll , ajouter une déclaration PdfFileWriter aide dans toutes les sources " +
			"fichier qui utilise la bibliothèque et inclure la PdfFileWriter.dll avec votre distribution. pour " +
			"plus de détails, allez à 2.18 Installation. Alternativement, vous pouvez inclure le code source de " +
			"la bibliothèque avec votre demande et éviter la nécessité de distribuer une liaison de données séparée " +
			"fichier de bibliothèque. L'exigence de développement minimum est de.NET Framework 4.0 (Visual Studio 2010)\n";
	 */
	}
}
