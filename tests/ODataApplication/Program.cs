using Fluxera.Extensions.Common;
using Fluxera.Repository.UnitTests.Core.PersonAggregate;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddGuidGenerator();

builder.Services.AddControllers()
	.AddOData(options =>
	{
		options.Select().Filter().OrderBy();
		options.AddRouteComponents(GetModel());
	});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

IEdmModel GetModel()
{
	ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
	modelBuilder.EntitySet<Person>("People");

	EntityTypeConfiguration<Person> entityType = modelBuilder.EntityType<Person>();
	entityType.HasKey(x => x.ID);
	entityType.Ignore(x => x.DomainEvents);
	entityType.Ignore(x => x.IsTransient);

	IEdmModel model = modelBuilder.GetEdmModel();

	return model;
}

;
