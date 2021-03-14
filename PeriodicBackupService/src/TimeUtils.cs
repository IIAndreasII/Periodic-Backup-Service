namespace PeriodicBackupService
{
	public class TimeUtils
	{
		public static double HoursToMillis(int hours)
		{
			return hours * 60 * 60 * 1000;
		}

		public static double MinutesToMillis(int minutes)
		{
			return minutes * 60000;
		}

		public static int MillisToMinutes(int millis)
		{
			return millis / 60000;
		}
	}
}