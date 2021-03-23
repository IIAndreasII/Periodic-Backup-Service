using GUI.Services;
using GUI.ViewModels;
using GUI.Views;
using System.Windows;

namespace GUI
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			MainWindow window = new MainWindow { DataContext = new MainWindowViewModel(new ChooseDirectoryService()) };
			window.Show();
		}
	}
}