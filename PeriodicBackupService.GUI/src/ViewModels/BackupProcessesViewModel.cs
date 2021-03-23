using GUI.Models;
using GUI.Models.Factories;
using GUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GUI.ViewModels
{
	public class BackupProcessesViewModel : IOViewModelBase, IPageViewModel
	{
		#region Fields

		public string Name => "Backup processes";

		private int selectedIndex;

		private int nameSortingFactor = -1;
		private int nextTimeSortingFactor = -1;
		private int lastBackupSortingFactor = -1;
		private int statusSortingFactor = -1;

		private bool isAddProcess = true;

		private string intervalUnit;
		private string interval;
		private string processName;
		private string maxNbrBackups;
		private string toggleButtonText;

		private ICommand addProcessCommand;
		private ICommand editProcessCommand;

		private ICommand toggleProcessCommand;
		private ICommand terminateProcessCommand;
		private ICommand forceBackupCommand;
		private ICommand selectionChangedCommand;

		private ICommand confirmConfigurationCommand;
		private ICommand cancelConfigurationCommand;

		private ICommand sortNameCommand;
		private ICommand sortNextBackupCommand;
		private ICommand sortLastBackupCommand;
		private ICommand sortStatusCommand;

		private readonly IProcessFactory processModelFactory;
		private readonly IWindowService windowService;

		private ObservableCollection<IProcessModel> processModels;

		#endregion

		#region Constructors

		public BackupProcessesViewModel(IProcessFactory processModelFactory, IWindowService windowService,
			IIOService ioService) : base(ioService)
		{
			this.processModelFactory = processModelFactory;
			this.windowService = windowService;
			IntervalUnit = ComboBoxContent[0];
			SelectedIndex = -1;
		}

		#endregion

		#region Properties

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

		public bool IsAddProcess
		{
			get => isAddProcess;
			set
			{
				isAddProcess = value;
				OnPropertyChanged(nameof(IsAddProcess));
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

		#endregion

		#region Commands

		public ICommand AddProcessCommand
		{
			get
			{
				return addProcessCommand ?? (addProcessCommand = new RelayCommand(p =>
				{
					IsAddProcess = true;
					ClearFields();
					windowService.OpenWindow(this);
				}));
			}
		}

		public ICommand EditProcessCommand
		{
			get
			{
				return editProcessCommand ?? (editProcessCommand =
					new RelayCommand(p =>
					{
						IsAddProcess = false;
						SourcePath = ProcessModels[SelectedIndex].SourcePath;
						TargetPath = ProcessModels[SelectedIndex].TargetPath;
						ProcessName = ProcessModels[SelectedIndex].Name;

						windowService.OpenWindow(this);
					}, p => SelectedIndex > -1));
			}
		}

		public ICommand ToggleProcessCommand
		{
			get
			{
				return toggleProcessCommand ?? (toggleProcessCommand = new RelayCommand(p =>
					{
						ProcessModels[SelectedIndex].Toggle();
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

		public ICommand ForceBackupCommand
		{
			get
			{
				return forceBackupCommand ?? (forceBackupCommand =
					new RelayCommand(p => { Task.Run(() => ProcessModels[SelectedIndex].ForceAction()); },
						p => SelectedIndex > -1 && !ProcessModels[SelectedIndex].IsBackingUp));
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
						windowService.CloseWindow();

						IProcessModel process = processModelFactory.Create(ProcessName, SourcePath, TargetPath,
							MaxNbrBackups,
							Interval, intervalUnit, UseCompression.ToString(), true.ToString());

						if (!isAddProcess)
						{
							int tempIndex = SelectedIndex;
							ProcessModels.RemoveAt(tempIndex);
							ProcessModels.Insert(tempIndex > -1 ? tempIndex : 0, process);
						}
						else
						{
							ProcessModels.Add(process);
						}

						OnPropertyChanged(nameof(ProcessModels));
					},
					p => ValidateParams()));
			}
		}

		public ICommand CancelConfigurationCommand
		{
			get
			{
				return cancelConfigurationCommand ?? (cancelConfigurationCommand = new RelayCommand(p =>
				{
					windowService.CloseWindow();
				}));
			}
		}

		public ICommand SortNameCommand
		{
			get
			{
				return sortNameCommand ?? (sortNameCommand = new RelayCommand(p =>
				{
					nameSortingFactor = -nameSortingFactor;
					SortProcessModels((left, right) =>
						nameSortingFactor * string.Compare(left.Name, right.Name, StringComparison.Ordinal));
				}));
			}
		}

		public ICommand SortNextBackupCommand
		{
			get
			{
				return sortNextBackupCommand ?? (sortNextBackupCommand = new RelayCommand(p =>
				{
					nextTimeSortingFactor = -nextTimeSortingFactor;
					SortProcessModels((left, right) =>
						nextTimeSortingFactor * DateTime.Compare(left.NextBackupTime, right.NextBackupTime));
				}));
			}
		}

		public ICommand SortLastBackupCommand
		{
			get
			{
				return sortLastBackupCommand ?? (sortLastBackupCommand = new RelayCommand(p =>
				{
					lastBackupSortingFactor = -lastBackupSortingFactor;
					SortProcessModels((left, right) =>
					{
						if (left.LastBackupStatus.Contains(Constants.NOK))
						{
							return -1;
						}

						if (right.LastBackupStatus.Contains(Constants.NOK))
						{
							return 1;
						}

						return lastBackupSortingFactor *
						       DateTime.Compare(left.LastBackupStatusTime, right.LastBackupStatusTime);
					});
				}));
			}
		}

		public ICommand SortStatusCommand
		{
			get
			{
				return sortStatusCommand ?? (sortStatusCommand = new RelayCommand(p =>
				{
					statusSortingFactor = -statusSortingFactor;
					SortProcessModels((left, right) => statusSortingFactor *
					                                   string.Compare(left.Status, right.Status,
						                                   StringComparison.Ordinal));
				}));
			}
		}

		#endregion

		#region Helper Methods

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

		private void ClearFields()
		{
			SourcePath = string.Empty;
			TargetPath = string.Empty;
			Interval = string.Empty;
			IntervalUnit = ComboBoxContent[0];
			MaxNbrBackups = string.Empty;
			UseCompression = true;
			ProcessName = string.Empty;
		}

		private void SortProcessModels(Comparison<IProcessModel> comparison)
		{
			var tempList = ProcessModels.ToList();

			tempList.Sort(comparison);

			ProcessModels.Clear();
			ProcessModels.AddRange(tempList);
			OnPropertyChanged(nameof(ProcessModels));
		}

		#endregion
	}
}