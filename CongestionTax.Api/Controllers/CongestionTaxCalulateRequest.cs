namespace CongestionTax.Api.Controllers
{
	public class CongestionTaxCalulateRequest
	{
		public CongestionTaxCalulateRequest(string city, Vehicle vehicle, List<DateTime> dates)
		{
			this.City = city;
			this.Vehicle = vehicle;
			this.Dates = dates;
		}

		public string City { get; set; }

		public Vehicle Vehicle { get; set; }

		public List<DateTime> Dates { get; set; }
	}
}
