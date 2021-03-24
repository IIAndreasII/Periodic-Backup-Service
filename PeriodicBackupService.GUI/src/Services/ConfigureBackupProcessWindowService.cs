using GUI.src.Views;
using PeriodicBackupService.GUI.Core.Services;
using System.Windows;

namespace GUI.Services
{
	public class ConfigureBackupProcessWindowService : IWindowService
	{
		#region Fields

		private Window window;

		#endregion

		#region IWindowService Members

		public void OpenWindow(object context)
		{
			window = new ConfigureBackupProcessView {DataContext = context};
			window.ShowDialog();
		}

		public void CloseWindow()
		{
			window?.Close();
		}

		#endregion
	}
}