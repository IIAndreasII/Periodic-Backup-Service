using Microsoft.Win32;

namespace PeriodicBackupService.GUI.Core.Services
{
	public class ChooseFileService : IIOService
	{
		#region IIOService Members

		public string GetPath(string defaultPath = "")
		{
			var ofd = new OpenFileDialog
			{
				Multiselect = false,
				Filter = "Backup Process (*.bp)|*.bp",
				Title = "Load config..."
			};
			ofd.ShowDialog();

			return ofd.FileName;
		}

		#endregion
	}
}