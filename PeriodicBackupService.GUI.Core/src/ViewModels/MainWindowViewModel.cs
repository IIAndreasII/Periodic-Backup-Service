using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using PeriodicBackupService.GUI.Core.Base;
using PeriodicBackupService.GUI.Core.IO;
using PeriodicBackupService.GUI.Core.Models;
using PeriodicBackupService.GUI.Core.Models.Factories;
using PeriodicBackupService.GUI.Core.Services;

namespace PeriodicBackupService.GUI.Core.ViewModels
{
	public class MainWindowViewModel : ModelBase
	{
		#region Fields

		private ICommand changePageCommand;

		private ICommand loadConfigCommand;
		private ICommand saveConfigCommand;
		private ICommand saveConfigAsCommand;

		private ICommand exitApplicationCommand;

		private readonly IIOService saveConfigService;
		private readonly IIOService chooseFileService;
		private readonly IOManager ioManager;

		private IPageViewModel currentPageViewModel;
		private List<IPageViewModel> pageViewModels;

		private readonly List<string> recentFilePaths;

		private readonly BackupProcessesViewModel backupProcessesViewModel;

		private string currentConfigPath;

		#endregion

		#region Constructors

		public MainWindowViewModel(IIOService ioService, IWindowService windowService, IIOService saveConfigService,
			IIOService chooseFileService, IMessageBoxService messageBoxService, IMessageBoxService confirmationService)
		{
			PageViewModels.Add(new CreateBackupViewModel(new BackupDirectoryManagerModel(), messageBoxService, confirmationService,
				ioService));
			backupProcessesViewModel =
				new BackupProcessesViewModel(new BackupProcessModelFactory(), windowService, ioService);
			PageViewModels.Add(backupProcessesViewModel);
			this.saveConfigService = saveConfigService;
			this.chooseFileService = chooseFileService;
			ioManager = new IOManager();
			recentFilePaths = new List<string>();
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

		public List<IPageViewModel> PageViewModels => pageViewModels ??= new List<IPageViewModel>();

		#endregion

		#region Commands

		public ICommand ChangePageCommand => changePageCommand ??=
					new RelayCommand(p => ChangeViewModel((IPageViewModel)p), p => p is IPageViewModel);

		public ICommand SaveConfigCommand => saveConfigCommand ??= new RelayCommand(SaveConfig);

		public ICommand SaveConfigAsCommand => saveConfigAsCommand ??= new RelayCommand(SaveConfigAs);

		public ICommand LoadConfigCommand => loadConfigCommand ??= new RelayCommand(LoadConfig);

		public ICommand ExitApplicationCommand => exitApplicationCommand ??= new RelayCommand(ExitApplication);

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

		private void SaveConfig(object commandParameter)
		{
			if (!File.Exists(currentConfigPath))
			{
				SaveConfigAs(commandParameter);
			}
			else
			{
				SaveProcessList(currentConfigPath);
			}
		}

		private void SaveConfigAs(object commandParameter)
		{
			var filepath = currentConfigPath = saveConfigService.GetPath();
			if (!string.IsNullOrWhiteSpace(filepath))
			{
				SaveProcessList(filepath);
			}
			AddRecentFilePath(filepath);
		}

		private void LoadConfig(object commandParameter)
		{
			var filepath = currentConfigPath = chooseFileService.GetPath();

			if (string.IsNullOrEmpty(filepath))
			{
				return;
			}

			var list = ioManager.Deserialized(filepath, new BackupProcessModelFactory());

			var newList = new ObservableCollection<IProcessModel>();

			foreach (var it in list)
			{
				newList.Add(it);
			}

			backupProcessesViewModel.ProcessModels = newList;

			CurrentPageViewModel = backupProcessesViewModel;
		}

		private void SaveProcessList(string filepath)
		{
			var list = backupProcessesViewModel.ProcessModels.ToList();
			ioManager.SetItems(list);
			ioManager.Write(filepath);
			AddRecentFilePath(filepath);
		}

		private void AddRecentFilePath(string filepath)
		{
			recentFilePaths.Add(filepath);
			while (recentFilePaths.Count > 5)
			{
				recentFilePaths.RemoveAt(0);
			}
		}

		private static void ExitApplication(object commandParameter)
		{
			Environment.Exit(0);
		}

		public void OnWindowClosing(object sender, CancelEventArgs e)
		{
			ioManager.Write("recent.txt", recentFilePaths.ToString());
		}

		#endregion
	}
}