namespace CongestionTax.Services
{
	public interface ICongestionTaxCalculator
	{
		TaxCharge GetTax(Policy policy, string city, Vehicle vehicle, List<DateTime> dates);
	}
}
