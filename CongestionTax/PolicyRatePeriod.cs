namespace CongestionTax
{
	public class PolicyRatePeriod
	{
		public PolicyRatePeriod(int fromMinuteOfDay, int toMinuteOfDay)
		{
			this.FromMinuteOfDay = fromMinuteOfDay;
			this.ToMinuteOfDay = toMinuteOfDay;
		}

		public int FromMinuteOfDay { get; set; }

		public int ToMinuteOfDay { get; set; }
	}
}
