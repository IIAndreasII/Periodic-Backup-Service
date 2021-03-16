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

		public string SourceDirectory
		{
			get => backupManager.SourceDirectory;
			set => backupManager.SourceDirectory = value;
		}

		public string TargetDirectory
		{
			get => backupManager.TargetDirectory;
			set => backupManager.TargetDirectory = value;
		}

		public bool UseCompression
		{
			get => backupManager.UseCompression;
			set => backupManager.UseCompression = value;
		}
	}
}