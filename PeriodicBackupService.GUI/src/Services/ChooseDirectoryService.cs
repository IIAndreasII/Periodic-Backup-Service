using System.Windows.Forms;

namespace GUI.Services
{
	internal class ChooseDirectoryService : IIOService
	{
		#region IIOService Members

		public string GetPath(string defaultPath = "")
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			return fbd.ShowDialog() == DialogResult.OK ? fbd.SelectedPath : string.Empty;
		}

		#endregion
	}
}