using System;
using System.Diagnostics;
using System.Windows.Input;

namespace PeriodicBackupService.GUI.Core.ViewModels
{
	public class RelayCommand : ICommand
	{
		#region Fields

		private readonly Action<object> execute;
		private readonly Predicate<object> canExecute;

		#endregion

		#region Constructors

		public RelayCommand(Action<object> execute) : this(execute, null)
		{
		}

		public RelayCommand(Action<object> execute, Predicate<object> canExecute)
		{
			this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
			this.canExecute = canExecute;
		}

		#endregion

		#region ICommand Members

		[DebuggerStepThrough]
		public bool CanExecute(object parameter)
		{
			return canExecute?.Invoke(parameter) ?? true;
		}

		public event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		public void Execute(object parameter)
		{
			execute(parameter);
		}

		#endregion
	}
}