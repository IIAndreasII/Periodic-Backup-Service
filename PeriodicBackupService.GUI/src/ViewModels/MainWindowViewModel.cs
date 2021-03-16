using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GUI.Services;
using PeriodicBackupService.Models;
using PeriodicBackupService.Models.Factories;

namespace GUI.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		private ICommand changePageCommand;

		private IPageViewModel currentPageViewModel;
		private List<IPageViewModel> pageViewModels;


		public MainWindowViewModel(IIOService ioService)
		{
			PageViewModels.Add(new CreateBackupViewModel(new BackupDirectoryManagerModel(), new InfoMessageBoxService(),
				ioService));
			PageViewModels.Add(new BackupProcessesViewModel(new BackupProcessModelFactory(),
				new ConfigureBackupProcessWindowService(), ioService));
		}


		public ICommand ChangePageCommand
		{
			get
			{
				return changePageCommand ?? (changePageCommand =
					new RelayCommand(p => ChangeViewModel((IPageViewModel) p), p => p is IPageViewModel));
			}
		}

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

		private void ChangeViewModel(IPageViewModel viewModel)
		{
			if (!PageViewModels.Contains(viewModel))
			{
				PageViewModels.Add(viewModel);
			}

			CurrentPageViewModel = PageViewModels.FirstOrDefault(vm => vm == viewModel);
		}
	}
}