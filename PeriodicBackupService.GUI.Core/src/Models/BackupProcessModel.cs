using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using GUI;
using PeriodicBackupService.Core;
using PeriodicBackupService.GUI.Core.Base;

namespace PeriodicBackupService.GUI.Core.Models
{
	public class BackupProcessModel : ModelBase, IProcessModel, IDisposable
	{
		#region Fields

		private string name;
		private string status = Constants.RUNNING;
		private string lastBackupStatus = Constants.OK;
		private bool isBackingUp;

		private DateTime nextBackupTime;

		private Timer timer;
		private Stopwatch stopwatch;

		private readonly double interval;
		private readonly IBackupManager backupManager;

		#endregion

		#region Constructors

		public BackupProcessModel(string name, string sourcePath, string targetPath, int maxNbrBackups, double interval,
			string intervalUnit,
			bool useCompression, bool backupOnInit = true)
		{
			Name = name;
			SourcePath = sourcePath;
			TargetPath = targetPath;
			MaxNbrBackups = maxNbrBackups;
			Interval = interval;
			IntervalUnit = intervalUnit;
			UseCompression = useCompression;
			backupManager = new BackupDirectoryManager(sourcePath, targetPath, maxNbrBackups, useCompression);
			this.interval = interval == 0 ? TimeUtils.HoursToMillis(1) : interval;
			SetUpTimer();
			if (backupOnInit)
			{
				Task.Run(DoBackup);
			}
		}

		#endregion

		#region Properties

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

		public string IntervalUnit { get; }

		public bool IsBackingUp
		{
			get => isBackingUp;
			private set
			{
				isBackingUp = value;
				OnPropertyChanged(nameof(IsBackingUp));
			}
		}

		public DateTime NextBackupTime
		{
			get => nextBackupTime;
			set
			{
				nextBackupTime = value;
				OnPropertyChanged(nameof(NextBackup));
				OnPropertyChanged(nameof(NextBackupTime));
			}
		}

		public DateTime LastBackupStatusTime { get; set; }
		public string SourcePath { get; }
		public string TargetPath { get; }

		public string NextBackup => NextBackupTime.ToLongTimeString();

		public int MaxNbrBackups { get; private set; }

		public double Interval { get; }

		public bool UseCompression { get; }

		#endregion

		#region IDispose Members

		public void Dispose()
		{
			timer?.Dispose();
		}

		#endregion

		#region IProcessModel Members

		public void ForceAction()
		{
			DoBackup();
		}

		public void Toggle()
		{
			timer.Enabled = !timer.Enabled;
			if (timer.Enabled)
			{
				NextBackupTime = DateTime.Now.AddMilliseconds(interval - stopwatch.ElapsedMilliseconds);
				stopwatch.Start();
			}
			else
			{
				stopwatch.Stop();
			}

			Status = timer.Enabled ? Constants.RUNNING : Constants.SUSPENDED;
		}

		#endregion

		#region Helper Methods

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

		private void DoBackup()
		{
			IsBackingUp = true;
			LastBackupStatusTime = DateTime.Now;
			var backupResult = backupManager.CreateBackup();
			IsBackingUp = false;
			LastBackupStatus =
				$"{(backupResult ? Constants.OK : Constants.NOK)} - {DateTime.Now.ToLongTimeString()}";
			NextBackupTime = DateTime.Now.AddMilliseconds(interval);
			stopwatch = Stopwatch.StartNew();
		}

		#endregion
	}
}