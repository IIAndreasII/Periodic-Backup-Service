using System;
using System.Timers;

namespace PeriodicBackupService.Models
{
	public class BackupProcessModel
	{
		public string Name { get; private set; }

		public string Status { get; private set; }

		private readonly IBackupManager backupManager;

		private Timer timer;

		public BackupProcessModel(string name, string sourceDir, string targetDir, int maxNbrBackups, int interval,
			bool useCompression)
		{
			Name = name;
			backupManager = new BackupManager(sourceDir, targetDir, maxNbrBackups, useCompression);
			SetUpTimer(interval);
		}

		public void Terminate()
		{
			Status = "Terminated";
			timer.Stop();
		}

		public void Start()
		{
			Status = "OK";
			timer.Start();
		}

		private void OnTimedEvent(object sender, EventArgs args)
		{
			Status = backupManager.CreateBackup() ? "OK" : "NOK";
		}

		private void SetUpTimer(double interval)
		{
			timer = new Timer(interval);
			timer.Elapsed += OnTimedEvent;
			timer.Start();
		}
	}
}