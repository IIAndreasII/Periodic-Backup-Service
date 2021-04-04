using System.Windows;

namespace PeriodicBackupService.GUI.Core.Services
{
	internal class InfoMessageBoxService : IMessageBoxService
	{
		#region IMessageBoxService Members

		public MessageBoxResult Show(string text, string caption = "Message")
		{
			return MessageBox.Show(text, "Info", MessageBoxButton.OK);
		}

		#endregion
	}
}