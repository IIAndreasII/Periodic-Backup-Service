using NUnit.Framework;
using PeriodicBackupService.Core;

// Use the Moq library?

// TODO: Implement tests for backup manager

namespace PeriodicBackupService.Test
{
	public class Tests
	{
		private const string SOURCE_DIR = "";
		private const string TARGET_DIR = "";

		private readonly IBackupManager backupManager;

		[SetUp]
		public void Setup()
		{
		}

		[TearDown]
		public void TearDown()
		{
		}

		[Test]
		public void TestBackupNoCompression()
		{
			UseImplementationBackupManager(false);

			Assert.Pass();
		}

		[Test]
		public void TestBackupCompression()
		{
			UseImplementationBackupManager(true);

			Assert.Pass();
		}

		private void UseImplementationBackupManager(bool useCompression)
		{
			//backupManager = new BackupDirectoryManager(SOURCE_DIR, TARGET_DIR, useCompression);
		}
	}
}