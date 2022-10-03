namespace CongestionTax.Services
{
	public class CongestionTaxCalculator : ICongestionTaxCalculator
	{
		private readonly IPublicHolidayService publicHolidayService;

		public CongestionTaxCalculator(IPublicHolidayService publicHolidayService)
		{
			this.publicHolidayService = publicHolidayService;
		}

		public TaxCharge GetTax(Policy policy, string city, Vehicle vehicle, List<DateTime> dates)
		{
			PolicyExemptions? exemptions = policy.Exemptions;
			if (
				((exemptions?.EmergencyVehicle ?? false) && vehicle.IsEmergency) ||
				((exemptions?.DiplomatVehicle ?? false) && vehicle.IsDiplomat) ||
				((exemptions?.MilitaryVehicle ?? false) && vehicle.IsMilitary) ||
				((exemptions?.ForeignVehicle ?? false) && vehicle.IsForeign) ||
				(exemptions?.VehicleCategories?.Contains(vehicle.Category) ?? false))
			{
				return new TaxCharge() { Total = 0 };
			}

			dates.Sort();

			List<TaxSubcharge> subcharges = new ();
			int startingIndex = 0;
			// itarete through dates collecting daily subtotals
			while (true)
			{
				TaxSubcharge? taxSubcharge = this.GetDailySubtotal(policy, city, startingIndex, dates, out int? nextStartingIndex);
				if (taxSubcharge != null && taxSubcharge.Subtotal != 0)
				{
					subcharges.Add(taxSubcharge);
				}

				if (nextStartingIndex == null)
				{
					break;
				}

				startingIndex = nextStartingIndex.Value;
			}

			return new TaxCharge() { Total = subcharges.Sum(s => s.Subtotal), Subcharges = subcharges };
		}

		protected TaxSubcharge? GetDailySubtotal(Policy policy, string city, int startingIndex, List<DateTime> dateTimes, out int? nextStartingIndex)
		{
			DateTime? currentDate = null;
			int subtotal = 0;
			int singleChargeWindowCharge = 0;
			DateTime? endOfSingleChargeWindow = null;

			int i;
			for (i = startingIndex; i < dateTimes.Count; i++)
			{
				DateTime dateTime = dateTimes[i];
				DateTime date = dateTimes[i].Date;
				if (i == startingIndex)
				{
					currentDate = date;
				}

				int toll = this.GetTollFee(policy, city, dateTime);
				if (date != currentDate && date >= endOfSingleChargeWindow)
				{
					break;
				}

				if (toll == 0)
				{
					continue;
				}

				if (dateTime >= endOfSingleChargeWindow)
				{
					subtotal += singleChargeWindowCharge;
					endOfSingleChargeWindow = null;
				}

				if (endOfSingleChargeWindow == null)
				{
					endOfSingleChargeWindow = dateTime.AddMinutes(policy.SingleChargeWindowMins ?? 0);
					singleChargeWindowCharge = 0;
				}

				if (toll > singleChargeWindowCharge)
				{
					singleChargeWindowCharge = toll;
				}
			}

			// the trailing single-charge window
			if (singleChargeWindowCharge != 0)
			{
				subtotal += singleChargeWindowCharge;
			}

			int maximumDailyCharge = policy?.MaximumDailyCharge ?? int.MaxValue;
			if (subtotal > maximumDailyCharge)
			{
				subtotal = maximumDailyCharge;
			}

			nextStartingIndex = i == dateTimes.Count ? null : i;

			if (currentDate == null)
			{
				return null;
			}

			return new TaxSubcharge() { Date = currentDate.Value, Subtotal = subtotal };
		}

		protected int GetTollFee(Policy policy, string city, DateTime timestamp)
		{
			PolicyExemptions? exemptions = policy.Exemptions;
			if ((exemptions?.DaysOfWeek?.Contains(timestamp.DayOfWeek) ?? false) ||
				(exemptions?.MonthsOfYear?.Contains(timestamp.Month) ?? false) ||
				((exemptions?.PublicHoliday ?? false) && this.publicHolidayService.IsPublicHoliday(city, timestamp)) ||
				((exemptions?.DayBeforePublicHoliday ?? false) && this.publicHolidayService.IsDayBeforePublicHoliday(city, timestamp)))
			{
				return 0;
			}

			int minuteOfDay = (timestamp.Hour * 60) + timestamp.Minute;
			foreach (PolicyRate policyRate in policy.Rates)
			{
				if (minuteOfDay >= policyRate.Period.FromMinuteOfDay && minuteOfDay < policyRate.Period.ToMinuteOfDay)
				{
					return policyRate.Amount;
				}
			}

			return 0;
		}
	}
}
