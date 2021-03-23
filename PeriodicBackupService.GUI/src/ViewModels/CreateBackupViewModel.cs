using GUI.Models;
using GUI.Services;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GUI.ViewModels
{
	public class CreateBackupViewModel : IOViewModelBase, IPageViewModel
	{
		#region Fields

		public string Name => "Create backup";

		private ICommand createBackupCommand;

		private readonly BackupDirectoryManagerModel backupManager;
		private readonly IMessageBoxService messageBoxService;

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
				return createBackupCommand ?? (createBackupCommand = new RelayCommand(p =>
					{
						IsBackingUp = true;
						Task.Run(() =>
						{
							backupManager.SourcePath = SourcePath;
							backupManager.TargetPath = TargetPath;
							backupManager.UseCompression = UseCompression;
							string message = backupManager.CreateBackup(BackupName)
								? "Backup successful!"
								: "Backup failed!";
							IsBackingUp = false;
							messageBoxService.ShowMessage(message);
						});
					},
					p => ValidateProperties() && !isBackingUp));
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