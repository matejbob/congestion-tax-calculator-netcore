namespace CongestionTax.Tests
{
	using System;
	using System.Collections.Generic;
	using CongestionTax.Services;
	using FluentAssertions;
	using Xunit;

	public class CongestionTaxCalculatorTests
	{
		private readonly Policy testPolicy;
		private readonly CongestionTaxCalculator congestionTaxCalculator;
		private readonly List<DateTime> dates;

		public CongestionTaxCalculatorTests()
		{
			this.testPolicy = new Policy()
			{
				MaximumDailyCharge = 60,
				SingleChargeWindowMins = 60,
				Exemptions = new PolicyExemptions()
				{
					DaysOfWeek = new List<DayOfWeek>() { DayOfWeek.Saturday, DayOfWeek.Sunday },
					MonthsOfYear = new List<int>() { 7 },
					PublicHoliday = true,
					DayBeforePublicHoliday = true,
					VehicleCategories = new List<VehicleCategoryEnum>() { VehicleCategoryEnum.Bus, VehicleCategoryEnum.Motorcycle },
					EmergencyVehicle = true,
					DiplomatVehicle = true,
					MilitaryVehicle = true,
					ForeignVehicle = true,
				},
				Rates = new List<PolicyRate>()
				{
					new PolicyRate(new PolicyRatePeriod(0, 30), 5), // 00:00 - 00:30 ~ 5 DKK
					new PolicyRate(new PolicyRatePeriod(30, 720), 15), // 00:30 - 12:00 ~ 15 DKK
					new PolicyRate(new PolicyRatePeriod(720, 1410), 10), // 12:00 - 23:30 ~ 10 DKK
					new PolicyRate(new PolicyRatePeriod(1410, 1440), 5), // 23:30 - 00:00 ~ 5 DKK
				},
			};

			this.congestionTaxCalculator = new CongestionTaxCalculator(new PublicHolidayService());

			this.dates = new List<DateTime>()
			{
				new DateTime(2013, 3, 1, 0, 15, 0), // 2013-03-01 00:15:00
				new DateTime(2013, 3, 1, 0, 45, 0), // 2013-03-01 00:45:00, merged with previous => +15 (15)
				new DateTime(2013, 3, 1, 23, 45, 0), // 2013-03-01 23:45
				new DateTime(2013, 3, 2, 00, 15, 0), // 2013-03-01 23:45, merged with prevous => +5 (20)
				new DateTime(2013, 12, 22, 0, 15, 0), // 2013-12-22 00:15 SUNDAY => +0 (20)
				new DateTime(2013, 12, 23, 0, 15, 0), // 2013-12-23 00:15 DAY BEFORE PUBLIC HOLIDAY => +0 (20)
				new DateTime(2013, 12, 24, 0, 15, 0), // 2013-12-23 00:15 PUBLIC HOLIDAY => +0 (20)
			};
		}

		[Fact]
		public void Emergency_Vehicle_Has_Zero_Tax()
		{
			TaxCharge taxCharge = this.congestionTaxCalculator.GetTax(
				this.testPolicy,
				"Test city",
				new Vehicle() { Category = VehicleCategoryEnum.Car, IsEmergency = true },
				this.dates);

			taxCharge.Total.Should().Be(0);
		}

		[Fact]
		public void Exempt_VehicleCategory_Has_Zero_Tax()
		{
			TaxCharge taxCharge = this.congestionTaxCalculator.GetTax(
				this.testPolicy,
				"Test city",
				new Vehicle() { Category = VehicleCategoryEnum.Bus },
				this.dates);

			taxCharge.Total.Should().Be(0);
		}

		[Fact]
		public void Eligible_Vehicle_Has_CorrectTax()
		{
			TaxCharge taxCharge = this.congestionTaxCalculator.GetTax(
				this.testPolicy,
				"Test city",
				new Vehicle() { Category = VehicleCategoryEnum.Car },
				this.dates);

			taxCharge.Total.Should().Be(20);
		}

		//TODO:
	}
}
