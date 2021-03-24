namespace PeriodicBackupService.GUI.Core.Services
{
	public interface IMessageBoxService
	{
		void ShowMessage(string text, string caption = "Message");
	}
}