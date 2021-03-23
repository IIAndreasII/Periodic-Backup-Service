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
				// Create directory with current date and time
				string timeString =
					DateTime.Now.ToString("u").Replace('-', '_')
						.Replace(':', '-'); // Make Windows accept directory name
				name = timeString.Remove(timeString.Length - 1, 1);
			}

			string backupDir = Path.Combine(targetDir, name);


			if (!useCompression)
			{
				if (!Directory.Exists(backupDir))
				{
					Console.WriteLine("\nCreating backup directory...");
					Directory.CreateDirectory(backupDir);
				}

				foreach (string dirPath in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
				{
					Console.WriteLine($"\nCreating subdirectories...");
					Directory.CreateDirectory(dirPath.Replace(sourceDir, backupDir));
				}

				foreach (string newPath in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
				{
					Console.WriteLine($"\nCopying \"{newPath}\" to {backupDir}");
					File.Copy(newPath, newPath.Replace(sourceDir, backupDir), true);
				}
			}
			else
			{
				CompressDirectory(sourceDir, backupDir);
			}

			if (maxNbrBackups > 0)
			{
				CleanBackupDirectory(targetDir, maxNbrBackups);
			}

			return true;
		}

		private static void CompressDirectory(string sourceDir, string targetDir, bool overwrite = true)
		{
			try
			{
				string archiveName = targetDir + ".zip";

				Console.WriteLine($"\nCompressing directory \"{sourceDir}\" into \"{archiveName}\"");
				if (overwrite)
				{
					File.Delete(archiveName);
				}

				ZipFile.CreateFromDirectory(sourceDir, archiveName);

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
			var fileSystemInfos = new DirectoryInfo(targetDir).GetFileSystemInfos();

			if (fileSystemInfos.Length <= maxNbrBackups)
			{
				return;
			}

			Console.WriteLine("Cleaning up backup directory...\n");
			try
			{
				Array.Sort(fileSystemInfos, (left, right) => left.CreationTime.CompareTo(right.CreationTime));

				for (int i = fileSystemInfos.Length - maxNbrBackups; i > 0; i--)
				{
					Console.WriteLine($"Deleting \"{fileSystemInfos[i - 1].FullName}\"\n");

					if (Directory.Exists(fileSystemInfos[i - 1].FullName))
					{
						Directory.Delete(fileSystemInfos[i - 1].FullName, true);
					}
					else
					{
						fileSystemInfos[i - 1].Delete();
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e}\nCleaning up backup directory failed!\n");
			}

			Console.WriteLine("Cleaning up backup directory succeeded!\n");
		}
	}
}