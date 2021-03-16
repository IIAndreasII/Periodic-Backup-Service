namespace PeriodicBackupService.Models
{
	public interface IProcessModel
	{
		string Name { get; set; }
		string Status { get; set; }

		string LastBackupStatus { get; set; }

		void Toggle();
	}
}