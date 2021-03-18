using PeriodicBackupService.Core;

namespace PeriodicBackupService.Models
{
	public class BackupDirectoryManagerModel : IBackupManager
	{
		private readonly IBackupManager backupManager;

		public BackupDirectoryManagerModel()
		{
			backupManager = new BackupDirectoryManager();
		}

		public bool CreateBackup()
		{
			return backupManager.CreateBackup();
		}

		public bool CreateBackup(string name)
		{
			return backupManager.CreateBackup(name);
		}

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
	}
}