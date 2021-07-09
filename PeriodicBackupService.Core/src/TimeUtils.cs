namespace PeriodicBackupService.Core
{
	public class TimeUtils
	{
		public static double HoursToMillis(double hours)
		{
			return hours * 60 * 60 * 1000;
		}

		public static double MinutesToMillis(double minutes)
		{
			return minutes * 60000;
		}

		public static double MillisToMinutes(double millis)
		{
			return millis / 60000;
		}
	}
}