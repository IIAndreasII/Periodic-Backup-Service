using System.Windows;

namespace GUI.Services
{
	internal class InfoMessageBoxService : IMessageBoxService
	{
		public void ShowMessage(string text, string caption = "Info")
		{
			MessageBox.Show(text, "Info", MessageBoxButton.OK);
		}
	}
}