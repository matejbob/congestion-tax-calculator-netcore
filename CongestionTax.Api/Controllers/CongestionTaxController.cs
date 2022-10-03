namespace CongestionTax.Api.Controllers
{
	using System.Net;
	using CongestionTax.Services;
	using Microsoft.AspNetCore.Mvc;

	[ApiController]
	[Route("api/congestion-tax")]
	public class CongestionTaxController : ControllerBase
	{
		private readonly IPolicyService policyService;
		private readonly ICongestionTaxCalculator congestionTaxCalculator;

		public CongestionTaxController(IPolicyService policyService, ICongestionTaxCalculator congestionTaxCalculator)
		{
			this.policyService = policyService;
			this.congestionTaxCalculator = congestionTaxCalculator;
		}

		[HttpPost]
		[Route("calculate")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TaxCharge))]
		[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
		public IActionResult Calculate(
			[FromBody] CongestionTaxCalulateRequest request)
		{
			Policy? cityPolicy = this.policyService.GetPolicy(request.City.ToLower());
			if (cityPolicy == null)
			{
				return this.BadRequest($"No policy found for {request.City}.");
			}

			TaxCharge taxCharge = this.congestionTaxCalculator.GetTax(cityPolicy, request.City, request.Vehicle, request.Dates);

			return this.Ok(taxCharge);
		}
	}
}
