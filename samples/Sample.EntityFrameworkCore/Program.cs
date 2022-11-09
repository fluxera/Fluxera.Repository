namespace Sample.EntityFrameworkCore
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();

			builder.Services.AddDbContext<SampleContext>();

			WebApplication app = builder.Build();

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
