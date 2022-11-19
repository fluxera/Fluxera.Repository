namespace Sample.LiteDB
{
	using Fluxera.Repository.LiteDB;
	using Fluxera.Utilities;

	public class SampleLiteContext : LiteContext
	{
		public static readonly string DatabaseFile;

		static SampleLiteContext()
		{
			DatabaseFile = TempPathHelper.CreateTempFile("db");
		}

		/// <inheritdoc />
		protected override void ConfigureOptions(LiteContextOptions options)
		{
			options.Database = DatabaseFile;
		}
	}
}
