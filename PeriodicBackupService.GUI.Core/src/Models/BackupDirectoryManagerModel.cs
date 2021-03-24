using PeriodicBackupService.Core;

namespace PeriodicBackupService.GUI.Core.Models
{
	public class BackupDirectoryManagerModel : IBackupManager
	{
		#region Fields

		private readonly IBackupManager backupManager;

		#endregion

		#region Constructors

		public BackupDirectoryManagerModel()
		{
			backupManager = new BackupDirectoryManager();
		}

		#endregion

		#region Properties

		public string SourcePath
		{
			get => backupManager.SourcePath;
			set => backupManager.SourcePath = value;
		}

		public string TargetPath
		{
			get => backupManager.TargetPath;
			set => backupManager.TargetPath = value;
		}

		public bool UseCompression
		{
			get => backupManager.UseCompression;
			set => backupManager.UseCompression = value;
		}

		#endregion

		#region Member Methods

		public bool CreateBackup()
		{
			return backupManager.CreateBackup();
		}

		public bool CreateBackup(string name)
		{
			return backupManager.CreateBackup(name);
		}

		#endregion
	}
}