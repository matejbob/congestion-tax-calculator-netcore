namespace CongestionTax.Services
{
	public interface IPolicyService
	{
		Policy? GetPolicy(string city);
	}
}
