using System.Windows.Forms;

namespace PeriodicBackupService.Services
{
	public class ChooseDirectoryService : IIOService
	{
		public string GetPath(string defaultPath = "")
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			return fbd.ShowDialog() == DialogResult.OK ? fbd.SelectedPath : string.Empty;
		}
	}
}