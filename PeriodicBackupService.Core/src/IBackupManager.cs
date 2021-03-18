namespace PeriodicBackupService.Core
{
	public interface IBackupManager
	{
		bool CreateBackup();

		bool CreateBackup(string name);

		string SourcePath { get; set; }

		string TargetPath { get; set; }

		bool UseCompression { get; set; }
	}
}