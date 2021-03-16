using System;
using System.Timers;
using PeriodicBackupService;

namespace GUI.Models
{
	public class BackupProcessModel : IProcessModel, IDisposable
	{
		public string Name { get; set; }

		public string Status { get; set; } = Constants.RUNNING;

		public string LastBackupStatus { get; set; } = Constants.OK;

		private readonly IBackupManager backupManager;

		private Timer timer;

		public BackupProcessModel(string name, string sourceDir, string targetDir, int maxNbrBackups, double interval,
			bool useCompression)
		{
			Name = name;
			backupManager = new BackupDirectoryManager(sourceDir, targetDir, maxNbrBackups, useCompression);
			SetUpTimer(interval);
		}

		private void OnTimedEvent(object sender, EventArgs args)
		{
			backupManager.CreateBackup();
		}

		private void SetUpTimer(double interval)
		{
			timer = new Timer(interval);
			timer.Elapsed += OnTimedEvent;
			timer.Start();
		}

		public void Dispose()
		{
			timer?.Dispose();
		}

		public void Toggle()
		{
			timer.Enabled = !timer.Enabled;
			Status = timer.Enabled ? Constants.RUNNING : Constants.SUSPENDED;
		}
	}
}