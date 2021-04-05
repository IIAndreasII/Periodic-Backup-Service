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

		public CreateBackupViewModel(BackupDirectoryManagerModel backupManager, IMessageBoxService messageBoxService, IMessageBoxService confirmationService,
			IIOService ioService) : base(ioService)
		{
			this.backupManager = backupManager;
			this.messageBoxService = messageBoxService;
			this.confirmationService = confirmationService;
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
							if (!ValidateAvailability())
							{
								switch (confirmationService.Show("A backup with the same name already exists. Overwrite it?", "Warning"))
								{
									case MessageBoxResult.Yes:
										DoBackup();
										break;
									case MessageBoxResult.No:
										IsBackingUp = false;
										break;
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
				BackupName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
		}

		private bool ValidateAvailability()
		{
			return UseCompression
				? !File.Exists(Path.Combine(TargetPath, string.Join(string.Empty, BackupName, ".zip")))
				: !Directory.Exists(Path.Combine(TargetPath, BackupName));
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