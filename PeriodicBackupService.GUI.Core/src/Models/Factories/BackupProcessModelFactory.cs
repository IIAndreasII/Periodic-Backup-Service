using GUI;
using PeriodicBackupService.Core;

namespace PeriodicBackupService.GUI.Core.Models.Factories
{
	public class BackupProcessModelFactory : IProcessFactory
	{
		#region IProcessFactory Members

		public IProcessModel Create(params object[] data)
		{
			var maxNbrBackups = int.Parse(ParseStringOrDefault(data[3], "0"));

			var intervalUnit = ParseStringOrDefault(data[5], Constants.HOURS);
			var intervalString = ParseStringOrDefault(data[4], "0");

			var intervalMillis = int.Parse(intervalString);

			double interval;

			if (data.Length > 8)
			{
				interval = intervalMillis;
			}
			else
			{
				interval = intervalUnit == Constants.HOURS
					? TimeUtils.HoursToMillis(intervalMillis)
					: TimeUtils.MinutesToMillis(intervalMillis);
			}

			return new BackupProcessModel(data[0] as string, data[1] as string, data[2] as string, maxNbrBackups,
				interval, intervalUnit, bool.Parse(ParseStringOrDefault(data[6], true.ToString())),
				bool.Parse(ParseStringOrDefault(data[7], true.ToString())));
		}

		#endregion

		#region Helper Methods

		private static string ParseStringOrDefault(object o, string defaultValue = "")
		{
			return string.IsNullOrWhiteSpace(o as string) ? defaultValue : (string) o;
		}

		#endregion
	}
}