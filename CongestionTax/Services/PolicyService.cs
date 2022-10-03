namespace CongestionTax.Services
{
	public class PolicyService : IPolicyService
	{
		private readonly Dictionary<string, Policy> policyMap;

		public PolicyService(Dictionary<string, Policy> policyMap)
		{
			this.policyMap = policyMap;
		}

		public Policy? GetPolicy(string city)
		{
			if (this.policyMap.TryGetValue(city, out var policy))
			{
				return policy;
			}

			return null;
		}
	}
}
