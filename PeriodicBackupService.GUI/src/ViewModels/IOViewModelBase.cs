using GUI.Base;
using GUI.Services;
using System.Windows.Input;

namespace GUI.ViewModels
{
	public abstract class IOViewModelBase : ModelBase
	{
		#region Fields

		private ICommand chooseSourcePathCommand;
		private ICommand chooseTargetPathCommand;

		private readonly IIOService ioService;

		private string sourcePath;
		private string targetPath;
		private bool useCompression;

		#endregion

		#region Constructors

		protected IOViewModelBase(IIOService ioService)
		{
			this.ioService = ioService;
			UseCompression = true;
		}

		#endregion

		#region Properties

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

		#endregion

		#region Commands

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

		#endregion
	}
}