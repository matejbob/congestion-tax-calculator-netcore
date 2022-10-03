namespace CongestionTax.Services
{
	public interface IPublicHolidayService
	{
		bool IsPublicHoliday(string city, DateTime date);

		bool IsDayBeforePublicHoliday(string city, DateTime date);
	}
}
