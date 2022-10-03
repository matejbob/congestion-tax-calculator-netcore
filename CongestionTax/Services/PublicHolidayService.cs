namespace CongestionTax.Services
{
	public class PublicHolidayService : IPublicHolidayService
	{
		private readonly List<DateTime> publicHolidays = new ()
		{
			new DateTime(2013, 1, 1),
			new DateTime(2013, 3, 28),
			new DateTime(2013, 3, 29),
			new DateTime(2013, 4, 1),
			new DateTime(2013, 4, 30),
			new DateTime(2013, 5, 1),
			new DateTime(2013, 5, 8),
			new DateTime(2013, 5, 9),
			new DateTime(2013, 6, 5),
			new DateTime(2013, 6, 6),
			new DateTime(2013, 6, 21),
			new DateTime(2013, 11, 1),
			new DateTime(2013, 12, 24),
			new DateTime(2013, 12, 25),
			new DateTime(2013, 12, 26),
			new DateTime(2013, 12, 31),
		};

		public bool IsPublicHoliday(string city, DateTime dateTime)
		{
			// let's pretend that public holidays are city independent
			DateTime date = dateTime.Date;

			foreach (DateTime p in this.publicHolidays)
			{
				if (p == date)
				{
					return true;
				}
			}

			return false;
		}

		public bool IsDayBeforePublicHoliday(string city, DateTime dateTime)
		{
			// let's pretend that public holidays are city independent
			DateTime dateNextDay = dateTime.Date.AddDays(1);

			foreach (DateTime p in this.publicHolidays)
			{
				if (p == dateNextDay)
				{
					return true;
				}
			}

			return false;
		}
	}
}
