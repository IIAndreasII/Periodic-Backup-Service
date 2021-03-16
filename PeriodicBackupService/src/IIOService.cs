using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeriodicBackupService
{
	public interface IIOService
	{
		string ChooseFileDialog(string defaultPath = "");
	}
}