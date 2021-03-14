using System;
using System.IO;
using System.IO.Compression;

namespace PeriodicBackupService
{
	class BackupManager
	{
		public const string SOURCE_DIR = @"C:\Users\andeb\AppData\LocalLow\IronGate\Valheim\worlds";
		public const string TARGET_DIR = @"C:\Users\andeb\Desktop\ValheimWorldBackups";

		private readonly string sourceDirectory = SOURCE_DIR;
		private readonly string targetDirectory = TARGET_DIR;
		private readonly bool compress;

		public BackupManager(string sourceDirectory, string targetDirectory, bool compress)
		{
			this.sourceDirectory = string.IsNullOrEmpty(sourceDirectory) ? SOURCE_DIR : sourceDirectory;
			this.targetDirectory = string.IsNullOrEmpty(targetDirectory) ? TARGET_DIR : targetDirectory;
			this.compress = compress;	
		}

		public bool CreateBackup()
		{
			return BackupDirectory(sourceDirectory, targetDirectory, compress);
		}

		private bool BackupDirectory(string sourceDir, string targetDir, bool compress)
		{
			Console.WriteLine("Backing up files...");

			if (!Directory.Exists(sourceDir))
			{
				Console.WriteLine($"\nError backing up files: Could not find directory {sourceDir}\n\nExiting...");
				return false;
			}

			if (!Directory.Exists(targetDir))
			{
				Console.WriteLine("\nTarget directory not found. Creating directory...");
				Directory.CreateDirectory(targetDir);
			}

			// Create directory with current date
			string timeString = DateTime.Now.ToString("u").Replace('-', '_').Replace(':', '-'); // Make Windows accept directory name
			string backupDir = Path.Combine(targetDir, timeString.Remove(timeString.Length - 1, 1));
			if (!Directory.Exists(backupDir))
			{
				Console.WriteLine("\nCreating backup directory...");
				Directory.CreateDirectory(backupDir);
			}

			foreach (var file in Directory.GetFiles(sourceDir))
			{
				string destFile = Path.Combine(backupDir, Path.GetFileName(file));
				Console.WriteLine($"\nCopying \"{file}\" to {backupDir}");
				File.Copy(file, destFile, true);
			}

			if (compress)
			{
				CompressDirectory(backupDir);
			}

			return true;
		}

		private void CompressDirectory(string directory, bool deleteUncompressed = true)
		{
			try
			{
				string archiveName = directory + ".zip";
				Console.WriteLine($"\nCompressing directory \"{directory}\" into \"{archiveName}\"");
				ZipFile.CreateFromDirectory(directory, archiveName);

				if (deleteUncompressed)
				{
					Directory.Delete(directory, true);
				}

				Console.WriteLine("\nCompression successful!");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine("\nCompression failed!");
				return;
			}
		}
	}
}
