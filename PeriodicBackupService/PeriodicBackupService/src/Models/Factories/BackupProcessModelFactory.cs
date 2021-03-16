using PeriodicBackupService.Core;

namespace PeriodicBackupService.Models.Factories
{
	public class BackupProcessModelFactory : IProcessFactory
	{
		public IProcessModel Create(params object[] data)
		{
			int maxNbrBackups = int.Parse(ParseStringOrDefault(data[3], "0"));

			string intervalUnit = ParseStringOrDefault(data[5], Constants.HOURS);
			string intervalString = ParseStringOrDefault(data[4], "0");
			double interval = intervalUnit == Constants.HOURS
				? TimeUtils.HoursToMillis(int.Parse(intervalString))
				: TimeUtils.MinutesToMillis(int.Parse(intervalString));

			return new BackupProcessModel(data[0] as string, data[1] as string, data[2] as string, maxNbrBackups,
				interval, bool.Parse(ParseStringOrDefault(data[6], true.ToString())));
		}

		private static string ParseStringOrDefault(object o, string defaultValue = "")
		{
			return string.IsNullOrWhiteSpace(o as string) ? defaultValue : (string) o;
		}
	}
}