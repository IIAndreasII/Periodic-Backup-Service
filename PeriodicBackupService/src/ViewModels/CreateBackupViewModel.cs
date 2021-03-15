using System.IO;
using System.Windows.Input;

namespace PeriodicBackupService.ViewModels
{
	public class CreateBackupViewModel : ViewModelBase, IPageViewModel
	{
		public string Name => "Create backup";

		private ICommand createBackupCommand;


		public CreateBackupViewModel()
		{
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
						/* TODO: Send stuff to model */
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