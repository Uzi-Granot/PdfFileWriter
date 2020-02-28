using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestPdfFileWriter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

		private void OnArticleExample(object sender, RoutedEventArgs e)
		{
			ArticleExample Example = new ArticleExample();
			Example.Test(false, "ArticleExample.pdf");
		}

		private void OnTest(object sender, RoutedEventArgs e)
		{
		GlyphTypeface GTF = new GlyphTypeface();

		}
    }
}
