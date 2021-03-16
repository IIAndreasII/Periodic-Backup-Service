using System;
using System.ComponentModel;
using System.Diagnostics;

namespace PeriodicBackupService.ViewModels
{
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		public string DisplayName { get; protected set; }
		public bool ThrowOnInvalidPropertyName { get; protected set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected ViewModelBase(string displayName, bool throwOnInvalidProperty)
		{
			DisplayName = displayName;
			ThrowOnInvalidPropertyName = throwOnInvalidProperty;
		}

		protected ViewModelBase()
		{
			ThrowOnInvalidPropertyName = true;
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			VerifyPropertyName(propertyName);
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler == null)
			{
				return;
			}

			PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
			handler(this, e);
		}

		[Conditional("DEBUG")]
		[DebuggerStepThrough]
		public void VerifyPropertyName(string propertyName)
		{
			// Verify that the property name matches a real, 
			// public, instance property on this object. 
			if (TypeDescriptor.GetProperties(this)[propertyName] != null)
			{
				return;
			}

			string msg = "Invalid property name: " + propertyName;
			if (ThrowOnInvalidPropertyName)
			{
				throw new Exception(msg);
			}

			Debug.Fail(msg);
		}
	}
}