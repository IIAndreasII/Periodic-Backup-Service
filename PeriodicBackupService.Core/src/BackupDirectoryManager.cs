using System;
using System.IO;
using System.IO.Compression;

namespace PeriodicBackupService.Core
{
	public class BackupDirectoryManager : IBackupManager
	{
		public int MaxNumberOfBackups { get; set; }
		public string SourcePath { get; set; }
		public string TargetPath { get; set; }
		public bool UseCompression { get; set; }

		public BackupDirectoryManager()
		{
			MaxNumberOfBackups = 0;
		}

		public BackupDirectoryManager(string sourcePath, string targetPath, int maxNbrBackups = 0,
			bool compress = true)
		{
			if (string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(targetPath))
			{
				throw new ArgumentException("Paths must not me null or empty!");
			}

			SourcePath = sourcePath;
			TargetPath = targetPath;

			MaxNumberOfBackups = maxNbrBackups;
			UseCompression = compress;
		}

		public bool CreateBackup()
		{
			return BackupDirectory(SourcePath, TargetPath, MaxNumberOfBackups, UseCompression);
		}

		public bool CreateBackup(string name)
		{
			return BackupDirectory(SourcePath, TargetPath, MaxNumberOfBackups, UseCompression, name);
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
				string timeString =
					DateTime.Now.ToString("u").Replace('-', '_')
						.Replace(':', '-'); // Make Windows accept directory name
				name = timeString.Remove(timeString.Length - 1, 1);
			}

			string backupDir = Path.Combine(targetDir, name);
			if (!Directory.Exists(backupDir))
			{
				Console.WriteLine("\nCreating backup directory...");
				Directory.CreateDirectory(backupDir);
			}

			foreach (string file in Directory.GetFiles(sourceDir))
			{
				string destFile = Path.Combine(backupDir, Path.GetFileName(file));
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
				for (int i = fileInfos.Length - maxNbrBackups; i > 0; i--)
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