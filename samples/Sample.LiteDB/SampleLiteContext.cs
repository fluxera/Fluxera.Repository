namespace Sample.LiteDB
{
	using Fluxera.Repository.LiteDB;

	public class SampleLiteContext : LiteContext
	{
		/// <inheritdoc />
		public SampleLiteContext()
		{
		}

		/// <inheritdoc />
		protected override void ConfigureOptions(LiteContextOptions contextOptions)
		{
			base.ConfigureOptions(contextOptions);
		}
	}
}
