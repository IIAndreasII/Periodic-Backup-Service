using System;

namespace PeriodicBackupService.GUI.Core.Models
{
	public interface IProcessModel
	{
		string Name { get; set; }

		string Status { get; set; }

		string NextBackup { get; }

		string SourcePath { get; }

		string TargetPath { get; }

		public int MaxNbrBackups { get; }

		public double Interval { get; }

		public bool UseCompression { get; }

		public string IntervalUnit { get; }

		bool IsBackingUp { get; }

		DateTime NextBackupTime { get; set; }

		string LastBackupStatus { get; }

		DateTime LastBackupStatusTime { get; set; }

		void ForceAction();

		void Toggle();
	}
}