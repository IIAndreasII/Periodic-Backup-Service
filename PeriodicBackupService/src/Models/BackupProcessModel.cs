using System;
using System.Timers;
using PeriodicBackupService.Core;

namespace PeriodicBackupService.Models
{
	public class BackupProcessModel : ModelBase, IProcessModel, IDisposable
	{
		private string name;
		private string status = Constants.RUNNING;
		private string lastBackupStatus = Constants.OK;
		private string nextBackupTime;

		private readonly double interval;

		public string Name
		{
			get => name;
			set
			{
				name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		public string Status
		{
			get => status;
			set
			{
				status = value;
				OnPropertyChanged(nameof(Status));
			}
		}

		public string LastBackupStatus
		{
			get => lastBackupStatus;
			set
			{
				lastBackupStatus = value;
				OnPropertyChanged(nameof(LastBackupStatus));
			}
		}

		public string NextBackupTime
		{
			get => nextBackupTime;
			set
			{
				nextBackupTime = value;
				OnPropertyChanged(nameof(NextBackupTime));
			}
		}

		private readonly IBackupManager backupManager;

		private Timer timer;

		public BackupProcessModel(string name, string sourceDir, string targetDir, int maxNbrBackups, double interval,
			bool useCompression)
		{
			Name = name;
			backupManager = new BackupDirectoryManager(sourceDir, targetDir, maxNbrBackups, useCompression);
			this.interval = interval == 0 ? TimeUtils.HoursToMillis(1) : interval;
			SetUpTimer();
			DoBackup();
		}

		private void OnTimedEvent(object sender, EventArgs args)
		{
			DoBackup();
		}

		private void SetUpTimer()
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

		private void DoBackup()
		{
			LastBackupStatus =
				$"{(backupManager.CreateBackup() ? Constants.OK : Constants.NOK)} - {DateTime.Now.ToLongTimeString()}";
			NextBackupTime = DateTime.Now.AddMilliseconds(interval).ToLongTimeString();
		}
	}
}