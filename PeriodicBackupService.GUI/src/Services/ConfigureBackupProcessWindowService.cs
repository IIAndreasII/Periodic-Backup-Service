using System.Windows;
using GUI.src.Views;

namespace GUI.Services
{
	class ConfigureBackupProcessWindowService : IWindowService
	{
		private Window window;

		public void OpenWindow(object context)
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