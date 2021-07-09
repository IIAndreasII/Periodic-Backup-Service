using NUnit.Framework;
using PeriodicBackupService.Core;
using System.IO;

namespace PeriodicBackupService.Test
{
	[TestFixture]
	public class Tests
	{
		private const string SOURCE_DIR = "TestSource";
		private const string TEST_BACKUP_NAME = "TestName";
		private const string TEST_FILE_NAME = "test.txt";

		private readonly string TARGET_DIR = Directory.GetCurrentDirectory();
		private readonly string TEST_FILEPATH = Path.Join(SOURCE_DIR, TEST_FILE_NAME);

		private IBackupManager backupManager;

		[OneTimeSetUp]
		public void Setup()
		{
			Directory.CreateDirectory(SOURCE_DIR);
			var file = File.CreateText(TEST_FILEPATH);
			file.Close();
			backupManager = new BackupDirectoryManager(SOURCE_DIR, TARGET_DIR);
		}

		[OneTimeTearDown]
		public void TearDown()
		{
			Directory.Delete(SOURCE_DIR, true);
			var createdDir = Path.Join(Directory.GetCurrentDirectory(), TEST_BACKUP_NAME);
			
			if (Directory.Exists(createdDir))
			{
				Directory.Delete(createdDir, true);
			}

			var createdFile = createdDir + ".zip";
			if (File.Exists(createdFile))
			{
				File.Delete(createdFile);
			}
		}

		[Test]
		public void TestBackupNoCompression()
		{
			backupManager.UseCompression = false;
			backupManager.CreateBackup(TEST_BACKUP_NAME);

			var createdBackupDir = Path.Join(Directory.GetCurrentDirectory(), TEST_BACKUP_NAME);
			var createdFilePath = Path.Join(createdBackupDir, TEST_FILE_NAME);

			bool directoryExists = Directory.Exists(Path.Join(Directory.GetCurrentDirectory(), TEST_BACKUP_NAME));
			bool testFileExists = File.Exists(createdFilePath);

			Assert.IsTrue(directoryExists);
			Assert.IsTrue(testFileExists);
		}

		[Test]
		public void TestBackupCompression()
		{
			backupManager.UseCompression = true;
			backupManager.CreateBackup(TEST_BACKUP_NAME);

			var createdFilePath = Path.Join(Directory.GetCurrentDirectory(), TEST_BACKUP_NAME) + ".zip";
			bool testFileExists = File.Exists(createdFilePath);

			Assert.IsTrue(testFileExists);
		}
	}
}
