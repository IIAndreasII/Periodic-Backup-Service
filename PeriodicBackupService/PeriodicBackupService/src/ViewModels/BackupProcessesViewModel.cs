using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using PeriodicBackupService.Models;
using PeriodicBackupService.Models.Factories;
using PeriodicBackupService.Services;

namespace PeriodicBackupService.ViewModels
{
	public class BackupProcessesViewModel : IOViewModelBase, IPageViewModel
	{
		public string Name => "Backup processes";

		private int selectedIndex;

		private string intervalUnit;
		private string interval;
		private string processName;
		private string maxNbrBackups;
		private string toggleButtonText;

		private ICommand addProcessCommand;
		private ICommand toggleProcessCommand;
		private ICommand terminateProcessCommand;
		private ICommand selectionChangedCommand;

		private ICommand confirmConfigurationCommand;

		private readonly IProcessFactory processModelFactory;
		private readonly IWindowService windowService;

		private ObservableCollection<IProcessModel> processModels;

		public BackupProcessesViewModel(IProcessFactory processModelFactory, IWindowService windowService,
			IIOService ioService) : base(ioService)
		{
			this.processModelFactory = processModelFactory;
			this.windowService = windowService;
			IntervalUnit = ComboBoxContent[0];
			SelectedIndex = -1;
		}

		public string ToggleButtonText
		{
			get => toggleButtonText;
			set
			{
				toggleButtonText = value;
				OnPropertyChanged(nameof(ToggleButtonText));
			}
		}

		public int SelectedIndex
		{
			get => selectedIndex;
			set
			{
				selectedIndex = value;
				OnPropertyChanged(nameof(SelectedIndex));
				OnPropertyChanged(nameof(ShowProcessButtons));
			}
		}

		public string IntervalUnit
		{
			get => intervalUnit;
			set
			{
				intervalUnit = value;
				OnPropertyChanged(nameof(IntervalUnit));
			}
		}

		public string Interval
		{
			get => interval;
			set
			{
				interval = value;
				OnPropertyChanged(nameof(Interval));
			}
		}

		public string ProcessName
		{
			get => processName;
			set
			{
				processName = value;
				OnPropertyChanged(nameof(ProcessName));
			}
		}

		public string MaxNbrBackups
		{
			get => maxNbrBackups;
			set
			{
				maxNbrBackups = value;
				OnPropertyChanged(nameof(MaxNbrBackups));
			}
		}

		public bool ShowProcessButtons => SelectedIndex > -1;

		public List<string> ComboBoxContent { get; } = new List<string> {Constants.MINUTES, Constants.HOURS};

		public ObservableCollection<IProcessModel> ProcessModels
		{
			get => processModels ?? (processModels = new ObservableCollection<IProcessModel>());
			set
			{
				processModels = value;
				OnPropertyChanged(nameof(ProcessModels));
			}
		}

		public ICommand AddProcessCommand
		{
			get
			{
				return addProcessCommand ?? (addProcessCommand = new RelayCommand(p =>
				{
					windowService.OpenWindow(this);
				}));
			}
		}

		public ICommand ToggleProcessCommand
		{
			get
			{
				return toggleProcessCommand ?? (toggleProcessCommand = new RelayCommand(p =>
					{
						IProcessModel temp = ProcessModels[SelectedIndex];
						int index = ProcessModels.IndexOf(temp);
						ProcessModels.RemoveAt(SelectedIndex);
						temp.Toggle();
						SelectedIndex = index;
						ProcessModels.Insert(SelectedIndex, temp);
						UpdateToggleButtonText();
						OnPropertyChanged(nameof(ProcessModels));
					},
					p => SelectedIndex > -1));
			}
		}

		public ICommand TerminateProcessCommand
		{
			get
			{
				return terminateProcessCommand ?? (terminateProcessCommand = new RelayCommand(p =>
					{
						ProcessModels.RemoveAt(SelectedIndex);
						SelectedIndex = -1;
						OnPropertyChanged(nameof(ProcessModels));
					},
					p => SelectedIndex < ProcessModels.Count && SelectedIndex > -1));
			}
		}

		public ICommand SelectionChangedCommand
		{
			get
			{
				return selectionChangedCommand ?? (selectionChangedCommand =
					new RelayCommand(p => UpdateToggleButtonText(), p => SelectedIndex > -1));
			}
		}

		public ICommand ConfirmConfigurationCommand
		{
			get
			{
				return confirmConfigurationCommand ?? (confirmConfigurationCommand = new RelayCommand(p =>
					{
						// TODO: Create new model with factory and add to observable list

						ProcessModels.Add(
							processModelFactory.Create(ProcessName, SourcePath, TargetPath, MaxNbrBackups,
								Interval, intervalUnit, UseCompression.ToString()));

						windowService.CloseWindow();
						OnPropertyChanged(nameof(ProcessModels));
					},
					p => ValidateParams()));
			}
		}

		private bool ValidateParams()
		{
			return
				Directory.Exists(SourcePath) &&
				Directory.Exists(TargetPath) &&
				!string.IsNullOrWhiteSpace(ProcessName) &&
				(string.IsNullOrWhiteSpace(Interval) || int.TryParse(Interval, out _)) &&
				(string.IsNullOrWhiteSpace(MaxNbrBackups) || int.TryParse(MaxNbrBackups, out _));
		}

		private void UpdateToggleButtonText()
		{
			ToggleButtonText = ProcessModels[SelectedIndex].Status == Constants.RUNNING
				? Constants.SUSPEND
				: Constants.RESUME;
		}
	}
}