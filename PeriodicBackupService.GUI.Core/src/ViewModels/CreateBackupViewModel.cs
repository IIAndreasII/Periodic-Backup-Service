using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PeriodicBackupService.GUI.Core.Models;
using PeriodicBackupService.GUI.Core.Services;

namespace PeriodicBackupService.GUI.Core.ViewModels
{
	public class CreateBackupViewModel : IOViewModelBase, IPageViewModel
	{
		#region Fields

		public string Name => "Create backup";

		private ICommand createBackupCommand;

		private readonly BackupDirectoryManagerModel backupManager;
		private readonly IMessageBoxService messageBoxService;
		private readonly IMessageBoxService confirmationService; 

		private string backUpName;
		private bool isBackingUp;

		#endregion

		#region Constructors

		public CreateBackupViewModel(BackupDirectoryManagerModel backupManager, IMessageBoxService messageBoxService,
			IIOService ioService) : base(ioService)
		{
			this.backupManager = backupManager;
			this.messageBoxService = messageBoxService;
			BackupName = string.Empty;
		}

		#endregion

		#region Properties

		public bool IsBackingUp
		{
			get => isBackingUp;
			set
			{
				isBackingUp = value;
				OnPropertyChanged(nameof(IsBackingUp));
			}
		}

		public string BackupName
		{
			get => backUpName;
			set
			{
				backUpName = value;
				OnPropertyChanged(nameof(BackupName));
			}
		}

		#endregion

		#region Commands

		public ICommand CreateBackupCommand
		{
			get
			{
				return createBackupCommand ??= new RelayCommand(p =>
					{
						IsBackingUp = true;
						Task.Run(() =>
						{
							if (!ValidateBackupName())
							{
								switch (confirmationService.Show("Backup with the same already exists. Overwrite it?", "Hej"))
								{
									case MessageBoxResult.Yes:
										DoBackup();
										break;
									case MessageBoxResult.No:
									case MessageBoxResult.None:
									case MessageBoxResult.OK:
									case MessageBoxResult.Cancel:
										break;
									default:
										throw new ArgumentOutOfRangeException();
								}
							}
							else
							{
								DoBackup();
							}
						});
					},
					p => ValidateProperties() && !isBackingUp);
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

		private void DoBackup()
		{
			backupManager.SourcePath = SourcePath;
			backupManager.TargetPath = TargetPath;
			backupManager.UseCompression = UseCompression;

			var message = backupManager.CreateBackup(BackupName)
				? "Backup successful!"
				: "Backup failed!";
			IsBackingUp = false;
			messageBoxService.Show(message);
		}

		#endregion
	}
}