﻿using System.IO;
using System.Windows.Input;
using GUI.Services;
using PeriodicBackupService.Models;

namespace GUI.ViewModels
{
	public class CreateBackupViewModel : IOViewModelBase, IPageViewModel
	{
		public string Name => "Create backup";

		#region Fields

		private ICommand createBackupCommand;

		private readonly BackupDirectoryManagerModel backupManager;
		private readonly IMessageBoxService messageBoxService;

		private string backUpName;

		#endregion

		public CreateBackupViewModel(BackupDirectoryManagerModel backupManager, IMessageBoxService messageBoxService,
			IIOService ioService) : base(ioService)
		{
			this.backupManager = backupManager;
			this.messageBoxService = messageBoxService;
			BackupName = string.Empty;
		}

		#region Properties / Commands

		public string BackupName
		{
			get => backUpName;
			set
			{
				backUpName = value;
				OnPropertyChanged(nameof(BackupName));
			}
		}

		public ICommand CreateBackupCommand
		{
			get
			{
				return createBackupCommand ?? (createBackupCommand = new RelayCommand(p =>
					{
						backupManager.SourcePath = SourcePath;
						backupManager.TargetPath = TargetPath;
						backupManager.UseCompression = UseCompression;
						string message = backupManager.CreateBackup(BackupName)
							? "Backup successful!"
							: "Backup failed!";
						messageBoxService.ShowMessage(message);
					},
					p => ValidateProperties()));
			}
		}

		#endregion

		#region Helper Methods

		private bool ValidateProperties()
		{
			return ValidatePaths() && ValidateBackupName();
		}

		private bool ValidatePaths()
		{
			return Directory.Exists(SourcePath) && Directory.Exists(TargetPath);
		}

		private bool ValidateBackupName()
		{
			return
				BackupName == string.Empty ||
				!string.IsNullOrWhiteSpace(BackupName) &&
				BackupName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 &&
				!File.Exists(Path.Combine(SourcePath, BackupName));
		}

		#endregion
	}
}