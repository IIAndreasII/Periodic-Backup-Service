using System.Windows;

namespace PeriodicBackupService.GUI.Core.Services
{
	public class InfoMessageBoxService : IMessageBoxService
	{
		#region IMessageBoxService Members

		public MessageBoxResult Show(string text, string caption)
		{
			return MessageBox.Show(text, "Info", MessageBoxButton.OK);
		}

		#endregion
	}
}