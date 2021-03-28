using System.Collections.Generic;

namespace PeriodicBackupService.GUI.Core.IO
{
	public interface IIOManager<T>
	{
		void AddItem(T item);
		void AddItems(IList<T> items);
		void SetItems(IList<T> items);
		void Write(string filepath);
		string Read(string filepath);
		IList<T> Deserialized(string filepath);
	}
}