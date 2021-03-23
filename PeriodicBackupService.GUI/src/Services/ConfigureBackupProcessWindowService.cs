using GUI.src.Views;
using System.Windows;

namespace GUI.Services
{
	internal class ConfigureBackupProcessWindowService : IWindowService
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