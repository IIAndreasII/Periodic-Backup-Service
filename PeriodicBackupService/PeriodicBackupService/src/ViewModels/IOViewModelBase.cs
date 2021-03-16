using System.Windows.Input;
using PeriodicBackupService.Services;

namespace PeriodicBackupService.ViewModels
{
	public abstract class IOViewModelBase : ViewModelBase
	{
		private ICommand chooseSourcePathCommand;
		private ICommand chooseTargetPathCommand;

		private readonly IIOService ioService;

		private string sourcePath;
		private string targetPath;
		private bool useCompression;

		protected IOViewModelBase(IIOService ioService)
		{
			this.ioService = ioService;
			UseCompression = true;
		}

		public string SourcePath
		{
			get => sourcePath;
			set
			{
				sourcePath = value;
				OnPropertyChanged(nameof(SourcePath));
			}
		}

		public string TargetPath
		{
			get => targetPath;
			set
			{
				targetPath = value;
				OnPropertyChanged(nameof(TargetPath));
			}
		}

		public bool UseCompression
		{
			get => useCompression;
			set
			{
				useCompression = value;
				OnPropertyChanged(nameof(UseCompression));
			}
		}

		public ICommand ChooseSourcePathCommand
		{
			get
			{
				return chooseSourcePathCommand ?? (chooseSourcePathCommand = new RelayCommand(p =>
				{
					SourcePath = ioService.GetPath();
				}));
			}
		}

		public ICommand ChooseTargetPathCommand
		{
			get
			{
				return chooseTargetPathCommand ?? (chooseTargetPathCommand = new RelayCommand(p =>
				{
					TargetPath = ioService.GetPath();
				}));
			}
		}
	}
}