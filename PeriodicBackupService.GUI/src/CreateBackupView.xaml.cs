using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace GUI
{
	/// <summary>
	/// Interaction logic for CreateBackupView.xaml
	/// </summary>
	public partial class CreateBackupView : System.Windows.Controls.UserControl
	{
		public CreateBackupView()
		{
			InitializeComponent();
		}


		private static void ChoosePath(System.Windows.Controls.TextBox box)
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();

			if (dialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			box.Text = dialog.SelectedPath;
			box.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty)?.UpdateSource();
		}

		// Source directory
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			ChoosePath(SourceDirTextBox);
		}

		// Target directory
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			ChoosePath(TargetDirTextBox);
		}
	}
}