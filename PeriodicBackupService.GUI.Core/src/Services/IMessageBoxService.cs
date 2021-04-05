using System.Windows;

namespace PeriodicBackupService.GUI.Core.Services
{
	public interface IMessageBoxService
	{
		MessageBoxResult Show(string text, string caption = "Message");
	}
}