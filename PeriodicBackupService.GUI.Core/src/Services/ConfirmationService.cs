using System.Windows;

namespace PeriodicBackupService.GUI.Core.Services
{
	class ConfirmationService : IMessageBoxService
	{
		public MessageBoxResult Show(string text, string caption)
		{
			return MessageBox.Show(text, caption, MessageBoxButton.YesNo);
		}
	}
}
