// Define some important constants and the activity source

using System.Collections.Generic;
using Fluxera.Repository;
using Fluxera.Repository.MongoDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using WebApplication1;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure important OpenTelemetry settings, the console exporter, and automatic instrumentation
builder.Services.AddOpenTelemetryTracing(providerBuilder =>
{
	providerBuilder
		.AddConsoleExporter()
		.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("WebApplication1", "1.0.0"))
		.AddHttpClientInstrumentation()
		.AddAspNetCoreInstrumentation()
		.AddMongoDBInstrumentation()
		.AddRepositoryInstrumentation();
});

builder.Services.AddRepository(rb =>
{
	//rb.AddInMemoryRepository("PersonRepository", rob =>
	//{
	//	rob.UseFor<Person>();
	//});
	rb.AddMongoRepository("PersonRepository", rob =>
	{
		rob.UseFor<Person>();
	});
});

builder.Services.AddTransient<IPersonRepository, PersonRepository>();

WebApplication app = builder.Build();

app.MapGet("/", async (IPersonRepository repository) =>
{
	Person person = new Person
	{
		Name = "Tester",
		Age = 42
	};
	await repository.AddAsync(person);

	IReadOnlyCollection<Person> result = await repository.FindManyAsync(x => x.Name == "Tester");

	return result;
});

app.Run();
