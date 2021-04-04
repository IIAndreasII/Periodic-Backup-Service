using System;
using System.ComponentModel;
using System.Diagnostics;

namespace PeriodicBackupService.GUI.Core.Base
{
	public abstract class ModelBase : INotifyPropertyChanged
	{
		#region Fields

		public bool ThrowOnInvalidPropertyName { get; protected set; }

		#endregion

		#region Event Handlers

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region INotifyPropertyChanged Members

		protected virtual void OnPropertyChanged(string propertyName)
		{
			VerifyPropertyName(propertyName);
			var handler = PropertyChanged;
			if (handler == null)
			{
				return;
			}

			var e = new PropertyChangedEventArgs(propertyName);
			handler(this, e);
		}

		#endregion

		#region Helper Methods

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

		#endregion
	}
}