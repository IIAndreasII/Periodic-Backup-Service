using System;
using System.Timers;
using ValheimServerBackupper.src;

namespace ValheimServerBackupper
{
	class App
	{
		// Constants for default directories
		public const string SOURCE_DIR = @"C:\Users\andeb\AppData\LocalLow\IronGate\Valheim\worlds";
		public const string TARGET_DIR = @"C:\Users\andeb\Desktop\ValheimWorldBackups";

		private readonly ArgumentParser parser;
		private readonly BackupManager backupManager;
		private Timer timer;


		public App(ArgumentParser parser)
		{
			this.parser = parser;
			backupManager = new BackupManager(parser.GetArgument("s"), parser.GetArgument("t"), parser.GetBoolArgument("c"));
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
			double duration = TimeUtils.HoursToMillis(1);
			string temp = parser.GetArgument("d");
			if (temp != string.Empty)
			{
				duration = TimeUtils.MinutesToMillis(int.Parse(temp));
			}
			Console.WriteLine($"Files will be backed up every {TimeUtils.MillisToMinutes((int)duration)} minutes\n");
			timer = new Timer(duration);
			timer.Elapsed += OnTimedEvent;
			timer.Start();
		}
	}
}
