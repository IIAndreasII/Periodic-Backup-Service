using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Models
{
	public interface IProcessModel
	{
		string Name { get; set; }
		string Status { get; set; }

		string LastBackupStatus { get; set; }

		void Toggle();
	}
}