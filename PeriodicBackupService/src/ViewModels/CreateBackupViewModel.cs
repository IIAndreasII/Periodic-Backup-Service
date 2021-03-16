using System.IO;
using System.Windows.Input;

namespace PeriodicBackupService.ViewModels
{
	public class CreateBackupViewModel : ViewModelBase, IPageViewModel
	{
		public string Name => "Create backup";

		private readonly IBackupManager backupManager;

		public CreateBackupViewModel(IBackupManager backupManager)
		{
			this.backupManager = backupManager;
		}

		public string SourcePath { get; set; }

		public string TargetPath { get; set; }

		public bool UseCompression { get; set; }

		public ICommand CreateBackupCommand
		{
			get
			{
				return new RelayCommand(p =>
					{
						backupManager.SourceDirectory = SourcePath;
						backupManager.TargetDirectory = TargetPath;
						backupManager.UseCompression = UseCompression;
						backupManager.CreateBackup();
					},
					p => ValidateProperties());
			}
		}

		private bool ValidateProperties()
		{
			return Directory.Exists(SourcePath) && Directory.Exists(TargetPath);
		}
	}
}