namespace CongestionTax
{
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

	public class PolicyExemptions
	{
		[JsonProperty(PropertyName = "daysOfWeek", ItemConverterType = typeof(StringEnumConverter))]
		public List<DayOfWeek>? DaysOfWeek { get; set; }

		public List<int>? MonthsOfYear { get; set; }

		public bool PublicHoliday { get; set; }

		public bool DayBeforePublicHoliday { get; set; }

		[JsonProperty(PropertyName = "vehicleCategories", ItemConverterType = typeof(StringEnumConverter))]
		public List<VehicleCategoryEnum>? VehicleCategories { get; set; }

		public bool EmergencyVehicle { get; set; }

		public bool DiplomatVehicle { get; set; }

		public bool MilitaryVehicle { get; set; }

		public bool ForeignVehicle { get; set; }
	}
}
