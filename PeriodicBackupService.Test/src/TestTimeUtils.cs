using NUnit.Framework;
using PeriodicBackupService.Core;

namespace PeriodicBackupService.Test
{
	[TestFixture]
	class TestTimeUtils
	{
		[Test]
		public void TestHoursToMillis()
		{
			Assert.That(TimeUtils.HoursToMillis(1).Equals(60 * 60000));
			Assert.That(TimeUtils.HoursToMillis(1.5).Equals(90 * 60000));
		}

		[Test]
		public void TestMinutesToMillis()
		{
			Assert.That(TimeUtils.MinutesToMillis(1).Equals(60000));
			Assert.That(TimeUtils.MinutesToMillis(1.5).Equals(90000));
		}

		[Test]
		public void TestMillisToMinutes()
		{
			Assert.That(TimeUtils.MillisToMinutes(60000).Equals(1));
			Assert.That(TimeUtils.MillisToMinutes(90000).Equals(1.5));
		}
	}
}
