using System.Windows.Forms;

namespace PeriodicBackupService.GUI.Core.Services
{
	public class ChooseDirectoryService : IIOService
	{
		#region IIOService Members

		public string GetPath(string defaultPath = "")
		{
			var fbd = new FolderBrowserDialog();
			return fbd.ShowDialog() == DialogResult.OK ? fbd.SelectedPath : string.Empty;
		}

		#endregion
	}
}