using System;
using System.ComponentModel;
using System.Diagnostics;

namespace PeriodicBackupService.Models
{
	public abstract class ModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public bool ThrowOnInvalidPropertyName { get; protected set; }

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

			var msg = "Invalid property name: " + propertyName;
			if (ThrowOnInvalidPropertyName)
			{
				throw new Exception(msg);
			}

			Debug.Fail(msg);
		}
	}
}