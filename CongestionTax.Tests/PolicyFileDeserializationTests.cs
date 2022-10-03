namespace CongestionTax.Tests
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using FluentAssertions;
	using Newtonsoft.Json;
	using Xunit;

	public class PolicyFileDeserializationTests
	{
		private readonly Policy? testPolicy;

		public PolicyFileDeserializationTests()
		{
			this.testPolicy = JsonConvert.DeserializeObject<Policy>(File.ReadAllText(@"test.policy.json"));
		}

		[Fact]
		public void Policy_NotNull()
		{
			Assert.NotNull(this.testPolicy);
		}

		[Fact]
		public void MaxDailyCharge_Correct()
		{
			Assert.Equal(60, this.testPolicy?.MaximumDailyCharge);
		}

		[Fact]
		public void SingleChargeWindowMins_Correct()
		{
			Assert.Equal(60, this.testPolicy?.SingleChargeWindowMins);
		}

		[Fact]
		public void ExemptionsDaysOfWeek_Correct()
		{
			List<DayOfWeek>? exemptDaysOfWeek = this.testPolicy?.Exemptions?.DaysOfWeek;

			exemptDaysOfWeek.Should().NotBeNull().And.Equal(DayOfWeek.Saturday, DayOfWeek.Sunday);
		}

		[Fact]
		public void ExemptionsMonthsOfYear_Correct()
		{
			List<int>? exemptMonthsOfYear = this.testPolicy?.Exemptions?.MonthsOfYear;

			exemptMonthsOfYear.Should().NotBeNull().And.Equal(7);
		}

		[Fact]
		public void ExemptionsPublicHoliday_True()
		{
			bool exemptPublicHoliday = this.testPolicy?.Exemptions?.PublicHoliday ?? false;

			Assert.True(exemptPublicHoliday);
		}

		[Fact]
		public void ExemptionsDayBeforePublicHoliday_False()
		{
			bool exemptDayBeforePublicHoliday = this.testPolicy?.Exemptions?.DayBeforePublicHoliday ?? false;

			Assert.False(exemptDayBeforePublicHoliday);
		}

		[Fact]
		public void ExemptionsVehicleCategories_Correct()
		{
			List<VehicleCategoryEnum>? exemptVehicleCategories = this.testPolicy?.Exemptions?.VehicleCategories;

			exemptVehicleCategories.Should().NotBeNull().And.Equal(VehicleCategoryEnum.Bus, VehicleCategoryEnum.Motorcycle);
		}

		[Fact]
		public void ExemptionsEmergencyVehicle_True()
		{
			bool exemptEmergencyVehicle = this.testPolicy?.Exemptions?.EmergencyVehicle ?? false;

			Assert.True(exemptEmergencyVehicle);
		}

		[Fact]
		public void ExemptionsDiplomaticVehicle_True()
		{
			bool exemptDiplomaticVehicle = this.testPolicy?.Exemptions?.DiplomatVehicle ?? false;

			Assert.True(exemptDiplomaticVehicle);
		}

		[Fact]
		public void ExemptionsMilitaryVehicle_True()
		{
			bool exemptMilitaryVehicle = this.testPolicy?.Exemptions?.MilitaryVehicle ?? false;

			Assert.True(exemptMilitaryVehicle);
		}

		[Fact]
		public void ExemptionsForeignVehicle_True()
		{
			bool exemptForeignVehicle = this.testPolicy?.Exemptions?.ForeignVehicle ?? false;

			Assert.True(exemptForeignVehicle);
		}

		[Fact]
		public void Rates_Correct()
		{
			List<PolicyRate>? rates = this.testPolicy?.Rates;

			rates.Should().NotBeNull().And.Satisfy(
				r1 => r1.Period.FromMinuteOfDay == 360 && r1.Period.ToMinuteOfDay == 720 && r1.Amount == 1,
				r2 => r2.Period.FromMinuteOfDay == 720 && r2.Period.ToMinuteOfDay == 1410 && r2.Amount == 2,
				r3 => r3.Period.FromMinuteOfDay == 1410 && r3.Period.ToMinuteOfDay == 1440 && r3.Amount == 3);
		}
	}
}