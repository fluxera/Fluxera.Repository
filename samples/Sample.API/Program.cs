namespace Sample.API
{
	using Fluxera.StronglyTypedId.SystemTextJson;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Http;
	using Microsoft.Extensions.DependencyInjection;
	using Sample.Domain.Company;

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

			builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			//builder.Services.AddEntityFrameworkCore();
			//builder.Services.AddLiteDB();
			builder.Services.AddMongoDB();

			builder.Services.AddTransient<ICompanyRepository, CompanyRepository>();

			WebApplication app = builder.Build();

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
