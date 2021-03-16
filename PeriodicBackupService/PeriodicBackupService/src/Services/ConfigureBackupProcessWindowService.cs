using System.Windows;

namespace PeriodicBackupService.Services
{
	class ConfigureBackupProcessWindowService : IWindowService
	{
		private Window window;

		public void OpenWindow(Window window, object context)
		{
			window = new ConfigureBackupProcessView {DataContext = context};
			window.ShowDialog();
		}

		public void CloseWindow()
		{
			window?.Close();
		}
	}
}