using System.Windows;

namespace GUI.Services
{
	internal class InfoMessageBoxService : IMessageBoxService
	{
		#region IMessageBoxService Members

		public void ShowMessage(string text, string caption = "Message")
		{
			MessageBox.Show(text, "Info", MessageBoxButton.OK);
		}

		#endregion
	}
}