namespace CongestionTax
{
	using System.Text.RegularExpressions;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;

	public class RateDeserializer : JsonConverter
	{
		private static readonly Regex RegexTime = new (@"(.*):(.*)");

		public override bool CanConvert(Type objectType)
		{
			throw new NotImplementedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
		{
			JToken jToken = JToken.Load(reader);

			int rate = jToken.Value<int>("rate");
			string from = jToken.Value<string>("from") ?? string.Empty;
			string to = jToken.Value<string>("to") ?? string.Empty;

			Match? matchesFrom = RegexTime.Match(from);
			Match? matchesTo = RegexTime.Match(to);

			int fromHour = int.Parse(matchesFrom.Groups[1].Value);
			int fromMinute = int.Parse(matchesFrom.Groups[2].Value);
			int toHour = int.Parse(matchesTo.Groups[1].Value);
			int toMinute = int.Parse(matchesTo.Groups[2].Value);

			int fromMinuteOfDay = (fromHour * 60) + fromMinute;
			int toMinuteOfDay = (toHour * 60) + toMinute;
			if (toMinuteOfDay == 0)
			{
				toMinuteOfDay = 24 * 60;
			}

			return new PolicyRate(new PolicyRatePeriod(fromMinuteOfDay, toMinuteOfDay), rate);
		}

		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
