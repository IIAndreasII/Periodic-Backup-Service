using GUI.Services;
using GUI.Views;
using System.Windows;
using PeriodicBackupService.GUI.Core.Services;
using PeriodicBackupService.GUI.Core.ViewModels;

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
			var viewModel = new MainWindowViewModel(new ChooseDirectoryService(),
					new ConfigureBackupProcessWindowService(), new SaveFileService(), new ChooseFileService(), new InfoMessageBoxService(), new ConfirmationService());
			var window = new MainWindow
			{
				DataContext = viewModel,
			};
			//window.Closed += viewModel.OnWindowClosing();
			window.Show();

		}
	}
}