using System.Windows;

namespace PeriodicBackupService.GUI.Core.Services
{
	public class ConfirmationService : IMessageBoxService
	{
		#region IMessageBoxService Members

		public MessageBoxResult Show(string text, string caption)
		{
			return MessageBox.Show(text, caption, MessageBoxButton.YesNo);
		}

		#endregion
	}
}
