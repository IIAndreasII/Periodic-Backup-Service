using System;
using System.Diagnostics;
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

		private Timer timer;
		private Stopwatch stopwatch;

		private readonly double interval;
		private readonly IBackupManager backupManager;

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
			stopwatch = Stopwatch.StartNew();
		}

		public void Dispose()
		{
			timer?.Dispose();
		}

		public void Toggle()
		{
			timer.Enabled = !timer.Enabled;
			if (timer.Enabled)
			{
				NextBackupTime = DateTime.Now.AddMilliseconds(interval - stopwatch.ElapsedMilliseconds)
					.ToLongTimeString();
				stopwatch.Start();
			}
			else
			{
				NextBackupTime = Constants.EMPTY_TIME;
				stopwatch.Stop();
			}

			Status = timer.Enabled ? Constants.RUNNING : Constants.SUSPENDED;
		}

		private void DoBackup()
		{
			LastBackupStatus =
				$"{(backupManager.CreateBackup() ? Constants.OK : Constants.NOK)} - {DateTime.Now.ToLongTimeString()}";
			NextBackupTime = DateTime.Now.AddMilliseconds(interval).ToLongTimeString();
			stopwatch = Stopwatch.StartNew();
		}
	}
}