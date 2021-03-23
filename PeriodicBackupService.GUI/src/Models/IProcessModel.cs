using System;

namespace GUI.Models
{
	public interface IProcessModel
	{
		string Name { get; set; }

		string Status { get; set; }

		string NextBackup { get; }

		string SourcePath { get; }

		string TargetPath { get; }

		bool IsBackingUp { get; }

		DateTime NextBackupTime { get; set; }

		string LastBackupStatus { get; }

		DateTime LastBackupStatusTime { get; set; }

		void ForceAction();

		void Toggle();
	}
}