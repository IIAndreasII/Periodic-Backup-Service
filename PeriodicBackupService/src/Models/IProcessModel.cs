using System;

namespace PeriodicBackupService.Models
{
	public interface IProcessModel
	{
		string Name { get; set; }
		string Status { get; set; }
		string NextBackup { get; }
		DateTime NextBackupTime { get; set; }
		string LastBackupStatus { get; set; }
		DateTime LastBackupStatusTime { get; set; }

		void Toggle();
	}
}