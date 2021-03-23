using PeriodicBackupService.Core;
using System;
using System.Timers;

namespace GUI.ConsoleApp
{
	internal class App
	{
		private readonly ArgumentParser parser;
		private readonly IBackupManager backupManager;
		private Timer timer;

		public App(ArgumentParser parser)
		{
			this.parser = parser;
			backupManager = new BackupDirectoryManager(parser.GetArgument("s"), parser.GetArgument("t"),
				string.IsNullOrEmpty(parser.GetArgument("b")) ? 0 : int.Parse(parser.GetArgument("b")),
				parser.GetBoolArgument("c"));
			SetUpTimer();

			// Create one backup immediately
			backupManager.CreateBackup();
		}

		public void OnTimedEvent(object sender, EventArgs args)
		{
			backupManager.CreateBackup();
		}

		private void SetUpTimer()
		{
			var duration = TimeUtils.HoursToMillis(1);
			var arg = parser.GetArgument("d");
			if (arg != string.Empty)
			{
				duration = TimeUtils.MinutesToMillis(int.Parse(arg));
			}

			Console.WriteLine($"Files will be backed up every {TimeUtils.MillisToMinutes((int) duration)} minutes\n");
			timer = new Timer(duration);
			timer.Elapsed += OnTimedEvent;
			timer.Start();
		}
	}
}