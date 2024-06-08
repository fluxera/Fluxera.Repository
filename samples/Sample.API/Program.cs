namespace Sample.API
{
	using Fluxera.StronglyTypedId.SystemTextJson;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Http;
	using Microsoft.Extensions.DependencyInjection;
	using Sample.Domain.Company;
	using System.Reflection;

	public static class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers().AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.UseStronglyTypedId();
			});

			builder.Services.AddMediatR(options =>
			{
				options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
			});

			builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			//builder.Services.AddEntityFrameworkCore(true);
			builder.Services.AddMongoDB(true);
			//builder.Services.AddLiteDB(true);
			//builder.Services.AddInMemory(true);

			//builder.Services.AddEntityFrameworkCore(false);
			//builder.Services.AddMongoDB(false);
			//builder.Services.AddLiteDB(false);
			//builder.Services.AddInMemory(false);

			builder.Services.AddTransient<ICompanyRepository, CompanyRepository>();

			WebApplication app = builder.Build();

			//using(IServiceScope scope = app.Services.CreateScope())
			//{
			//	SampleDbContext context = scope.ServiceProvider.GetService<SampleDbContext>();
			//	context?.Database.EnsureCreated();
			//	context?.Database.Migrate();
			//}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();

			//File.Delete(SampleLiteContext.DatabaseFile);
		}
	}
}
