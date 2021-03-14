namespace PeriodicBackupService.Core
{
	public interface IBackupManager
	{
		bool CreateBackup();

		bool CreateBackup(string name);

		string SourceDirectory { get; set; }

		string TargetDirectory { get; set; }

		bool UseCompression { get; set; }
	}
}