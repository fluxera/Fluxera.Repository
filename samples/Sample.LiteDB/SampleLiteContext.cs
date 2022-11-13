namespace Sample.LiteDB
{
	using Fluxera.Repository.LiteDB;

	public class SampleLiteContext : LiteContext
	{
		/// <inheritdoc />
		protected override void ConfigureOptions(LiteContextOptions options)
		{
			options.Database = "sample.lite.db";
		}
	}
}
