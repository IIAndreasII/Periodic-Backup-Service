using System;
using System.IO;
using System.IO.Compression;

namespace PeriodicBackupService
{
	public class BackupDirectoryManager : IBackupManager
	{
		public const string SOURCE_DIR = @"C:\Users\andeb\AppData\LocalLow\IronGate\Valheim\worlds";
		public const string TARGET_DIR = @"C:\Users\andeb\Desktop\ValheimWorldBackups";


		public int MaxNumberOfBackups { get; set; }
		public string SourceDirectory { get; set; }
		public string TargetDirectory { get; set; }
		public bool UseCompression { get; set; }

		/// <summary>
		/// Should only be used 
		/// </summary>
		public BackupDirectoryManager()
		{
			MaxNumberOfBackups = 0;
		}

		public BackupDirectoryManager(string sourceDirectory, string targetDirectory, int maxNbrBackups = 0,
			bool compress = true)
		{
			SourceDirectory = string.IsNullOrEmpty(sourceDirectory) ? SOURCE_DIR : sourceDirectory;
			TargetDirectory = string.IsNullOrEmpty(targetDirectory) ? TARGET_DIR : targetDirectory;
			MaxNumberOfBackups = maxNbrBackups;
			UseCompression = compress;
		}

		public bool CreateBackup()
		{
			return BackupDirectory(SourceDirectory, TargetDirectory, MaxNumberOfBackups, UseCompression);
		}

		public bool CreateBackup(string name)
		{
			return BackupDirectory(SourceDirectory, TargetDirectory, MaxNumberOfBackups, UseCompression, name);
		}

		private static bool BackupDirectory(string sourceDir, string targetDir, int maxNbrBackups, bool useCompression,
			string name = "")
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

			if (string.IsNullOrEmpty(name))
			{
				// Create directory with current date
				var timeString =
					DateTime.Now.ToString("u").Replace('-', '_')
						.Replace(':', '-'); // Make Windows accept directory name
				name = timeString.Remove(timeString.Length - 1, 1);
			}

			var backupDir = Path.Combine(targetDir, name);
			if (!Directory.Exists(backupDir))
			{
				Console.WriteLine("\nCreating backup directory...");
				Directory.CreateDirectory(backupDir);
			}

			foreach (var file in Directory.GetFiles(sourceDir))
			{
				var destFile = Path.Combine(backupDir, Path.GetFileName(file));
				Console.WriteLine($"\nCopying \"{file}\" to {backupDir}");
				File.Copy(file, destFile, true);
			}

			if (useCompression)
			{
				CompressDirectory(backupDir);
			}

			CleanBackupDirectory(targetDir, maxNbrBackups);

			return true;
		}

		private static void CompressDirectory(string directory, bool deleteUncompressed = true)
		{
			try
			{
				var archiveName = directory + ".zip";
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
			}
		}

		private static void CleanBackupDirectory(string targetDir, int maxNbrBackups)
		{
			if (maxNbrBackups <= 0)
			{
				return;
			}

			var fileInfos = new DirectoryInfo(targetDir).GetFiles();
			if (fileInfos.Length <= maxNbrBackups)
			{
				return;
			}

			Console.WriteLine("Cleaning up backup directory...\n");
			try
			{
				Array.Sort(fileInfos, (left, right) => left.CreationTime.CompareTo(right.CreationTime));
				for (var i = fileInfos.Length - maxNbrBackups; i > 0; i--)
				{
					Console.WriteLine($"Deleting \"{fileInfos[i - 1].Name}\"\n");
					fileInfos[i - 1].Delete();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Console.WriteLine("\nCleaning up backup directory failed!\n");
			}

			Console.WriteLine("Cleaning up backup directory succeeded!\n");
		}
	}
}