using System;
using System.ComponentModel;

namespace PeriodicBackupService.Models
{
	public interface IProcessModel
	{
		string Name { get; set; }
		string Status { get; set; }
		string NextBackup { get; }

		bool IsBackingUp { get; }
		DateTime NextBackupTime { get; set; }
		string LastBackupStatus { get; }
		DateTime LastBackupStatusTime { get; set; }
		string SourcePath { get; }
		string TargetPath { get; }

		void ForceAction();

		void Toggle();
	}
}