namespace Sample.API
{
	using Fluxera.Repository;
	using Fluxera.Repository.EntityFrameworkCore;
	using Fluxera.StronglyTypedId.SystemTextJson;
	using Sample.Domain.Company;
	using Sample.EntityFrameworkCore;

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

			builder.Services.AddDbContext<SampleContext>((serviceProvider, optionsBuilder) =>
			{
				//optionsBuilder.UseSqlite("Filename=sample.db");
			});

			builder.Services.AddRepository(repositoryBuilder =>
			{
				repositoryBuilder.AddEntityFrameworkRepository<SampleContext>(repositoryOptionsBuilder =>
				{
					repositoryOptionsBuilder.UseFor<Company>();
				});
			});

			builder.Services.AddTransient<ICompanyRepository, CompanyRepository>();

			WebApplication app = builder.Build();

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
