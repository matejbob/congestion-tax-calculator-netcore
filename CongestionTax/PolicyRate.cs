namespace CongestionTax
{
	public class PolicyRate
	{
		public PolicyRate(PolicyRatePeriod period, int amount)
		{
			this.Period = period;
			this.Amount = amount;
		}

		public PolicyRatePeriod Period { get; set; }

		public int Amount { get; set; }
	}
}