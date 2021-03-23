using GUI.Base;
using GUI.Models;
using GUI.Models.Factories;
using GUI.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace GUI.ViewModels
{
	public class MainWindowViewModel : ModelBase
	{
		#region Fields

		private ICommand changePageCommand;

		private IPageViewModel currentPageViewModel;
		private List<IPageViewModel> pageViewModels;

		#endregion

		#region Constructors

		public MainWindowViewModel(IIOService ioService)
		{
			PageViewModels.Add(new CreateBackupViewModel(new BackupDirectoryManagerModel(), new InfoMessageBoxService(),
				ioService));
			PageViewModels.Add(new BackupProcessesViewModel(new BackupProcessModelFactory(),
				new ConfigureBackupProcessWindowService(), ioService));
		}

		#endregion

		#region Properties

		public IPageViewModel CurrentPageViewModel
		{
			get => currentPageViewModel;
			set
			{
				if (currentPageViewModel == value)
				{
					return;
				}

				currentPageViewModel = value;
				OnPropertyChanged(nameof(CurrentPageViewModel));
			}
		}

		public List<IPageViewModel> PageViewModels => pageViewModels ?? (pageViewModels = new List<IPageViewModel>());

		#endregion

		#region Commands

		public ICommand ChangePageCommand
		{
			get
			{
				return changePageCommand ?? (changePageCommand =
					new RelayCommand(p => ChangeViewModel((IPageViewModel) p), p => p is IPageViewModel));
			}
		}

		#endregion

		#region Helper Methods

		private void ChangeViewModel(IPageViewModel viewModel)
		{
			if (!PageViewModels.Contains(viewModel))
			{
				PageViewModels.Add(viewModel);
			}

			CurrentPageViewModel = PageViewModels.FirstOrDefault(vm => vm == viewModel);
		}

		#endregion
	}
}