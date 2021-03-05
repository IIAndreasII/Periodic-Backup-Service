using System;
using System.IO;
using System.Timers;

namespace ValheimServerBackupper
{
	class App
	{
		// Constants for default directories
		public const string TARGET_DIR = @"C:\Users\andeb\Desktop\ValheimWorldBackups";
		public const string SOURCE_DIR = @"C:\Users\andeb\AppData\LocalLow\IronGate\Valheim\worlds";

		private static ArgumentParser parser;

		private static Timer timer;

		public static void Init(ArgumentParser argParser)
		{
			Console.WriteLine("Starting world backup service...\n");
			parser = argParser;
			SetUpTimer();
			Run(); // Run once, then every one hour
		}

		private static void SetUpTimer()
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

		private static void Run()
		{
			ParseArgs(out string source, out string target, out bool overwrite);

			if (BackupFiles(source, target, overwrite))
			{
				Console.WriteLine("\nFiles backed up successfully!\n");
			}
			else
			{
				Console.WriteLine($"\nError backing up files: Could not find directory: {source}\n\nExiting...");
			}
		}

		private static void OnTimedEvent(object sender, EventArgs args)
		{
			Run();
		}

		private static void ParseArgs(out string source, out string target, out bool overwrite)
		{
			source = SOURCE_DIR;
			target = TARGET_DIR;
			overwrite = true;

			string temp = parser.GetArgument("s");
			if (!string.IsNullOrEmpty(temp))
			{
				source = temp;
			}

			temp = parser.GetArgument("t");
			if (!string.IsNullOrEmpty(temp))
			{
				target = temp;
			}

			temp = parser.GetArgument("o");
			if (!string.IsNullOrEmpty(temp))
			{
				overwrite = bool.Parse(temp);
			}
		}

		private static bool BackupFiles(string sourceDir, string targetDir, bool overwrite)
		{
			Console.WriteLine("Backing up files...");

			if (!Directory.Exists(sourceDir))
			{
				return false;
			}

			if (!Directory.Exists(targetDir))
			{
				Console.WriteLine("\nTarget directory not found\nCreating directory...");
				Directory.CreateDirectory(targetDir);
			}

			string timeString = DateTime.Now.ToString("u").Replace('-', '_').Replace(':', '-');
			string backupDir = Path.Combine(targetDir, timeString.Remove(timeString.Length - 1, 1));

			if (!Directory.Exists(backupDir))
			{
				Console.WriteLine("\nCreating sub directory...");
				Directory.CreateDirectory(backupDir);
			}

			foreach (var file in Directory.GetFiles(sourceDir))
			{
				string destFile = Path.Combine(backupDir, Path.GetFileName(file));
				Console.WriteLine($"\nCopying \"{file}\" to {backupDir}");
				File.Copy(file, destFile, overwrite);
			}

			return true;
		}
	}
}
