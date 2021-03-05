namespace ValheimServerBackupper
{
	class TimeUtils
	{
		public static double HoursToMillis(int hours)
		{
			return hours * 60 * MinutesToMillis(60);
		}

		public static double MinutesToMillis(int minutes)
		{
			return minutes * 1000;
		}

		public static int MillisToMinutes(int millis)
		{
			return millis / 1000;
		}
	}
}
