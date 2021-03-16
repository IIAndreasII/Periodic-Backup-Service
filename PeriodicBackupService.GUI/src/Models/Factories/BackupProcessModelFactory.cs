using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeriodicBackupService;

namespace GUI.Models.Factories
{
	class BackupProcessModelFactory : IProcessFactory
	{
		public IProcessModel Create(params object[] data)
		{
			var maxNbrBackups = int.Parse(data[3] as string ?? "0");
			var interval = data[5] as string == Constants.HOURS
				? TimeUtils.HoursToMillis(int.Parse(data[4] as string ?? "0"))
				: TimeUtils.MinutesToMillis(int.Parse(data[4] as string ?? "0"));

			return new BackupProcessModel(data[0] as string, data[1] as string, data[2] as string, maxNbrBackups,
				interval, bool.Parse(data[6] as string ?? "True"));
		}
	}
}