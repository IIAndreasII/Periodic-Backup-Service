using Microsoft.Win32;

namespace PeriodicBackupService.GUI.Core.Services
{
	public class ChooseFileService : IIOService
	{
		public string GetPath(string defaultPath = "")
		{
			OpenFileDialog ofd = new OpenFileDialog
			{
				Multiselect = false, Filter = "Backup Process (*.bp)|*.bp", Title = "Load config..."
			};
			ofd.ShowDialog();

			return ofd.FileName;
		}
	}
}