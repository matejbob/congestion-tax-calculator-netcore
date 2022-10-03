namespace CongestionTax
{
	public class TaxCharge
	{
		public int Total { get; set; }

		public List<TaxSubcharge>? Subcharges { get; set; }
	}
}
