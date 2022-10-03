namespace CongestionTax
{
	using Newtonsoft.Json;

	public class Policy
	{
		[JsonProperty(PropertyName = "rates", ItemConverterType = typeof(RateDeserializer))]
		public List<PolicyRate> Rates { get; set; } = new List<PolicyRate>();

		public int? MaximumDailyCharge { get; set; }

		public int? SingleChargeWindowMins { get; set; }

		public PolicyExemptions? Exemptions { get; set; }
	}
}
