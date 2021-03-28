using Microsoft.Win32;

namespace PeriodicBackupService.GUI.Core.Services
{
	public class SaveFileService : IIOService
	{
		public string GetPath(string defaultPath = "")
		{
			SaveFileDialog sfd = new SaveFileDialog
			{
				DefaultExt = ".bp", OverwritePrompt = true, AddExtension = true, Title = "Save config...",
				Filter = "Backup Process (*.bp)|*.bp"
			};
			sfd.ShowDialog();
			return sfd.FileName;
		}
	}
}