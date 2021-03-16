using System.Windows;

namespace PeriodicBackupService.Services
{
	internal class InfoMessageBoxService : IMessageBoxService
	{
		public void ShowMessage(string text, string caption = "Info")
		{
			MessageBox.Show(text, caption, MessageBoxButton.OK);
		}
	}
}