using System.Text.Json;
using System.Text.Json.Serialization;
using CongestionTax;
using CongestionTax.Services;

var builder = WebApplication.CreateBuilder(args);

// loading the policy files from the work dir
Dictionary<string, Policy> policyMap = new ();
string policyFilesDir;
if (Environment.GetEnvironmentVariable("IsLocalDebug") == "true")
{
	policyFilesDir = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName ?? throw new Exception("Could not obtain working dir.");
}
else
{
	policyFilesDir = Directory.GetCurrentDirectory();
}

string searchSuffix = ".policy.json";
string[] cityPolicyPathnames = Directory.GetFiles(policyFilesDir, $"*{searchSuffix}");
foreach (var p in cityPolicyPathnames)
{
	string fileName = Path.GetFileName(p);
	string cityName = fileName[..^searchSuffix.Length].Trim();

	CongestionTax.Policy? policy = Newtonsoft.Json.JsonConvert.DeserializeObject<CongestionTax.Policy>(File.ReadAllText(p));
	if (policy == null)
	{
		throw new Exception($"ERROR: failed to deserialize policy file ${p}");
	}

	policyMap.Add(cityName, policy);
}

// dependency injection setup
builder.Services.AddSingleton<IPolicyService>(new PolicyService(policyMap));
builder.Services.AddSingleton<IPublicHolidayService, PublicHolidayService>();
builder.Services.AddSingleton<ICongestionTaxCalculator, CongestionTaxCalculator>();

builder.Services.AddControllers().AddJsonOptions(o =>
{
	o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
	o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
