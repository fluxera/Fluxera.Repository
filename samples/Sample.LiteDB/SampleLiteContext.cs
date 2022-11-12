namespace Sample.LiteDB
{
	using Fluxera.Repository;
	using Fluxera.Repository.LiteDB;

	public class SampleLiteContext : LiteContext
	{
		/// <inheritdoc />
		public SampleLiteContext(
			string repositoryName,
			DatabaseProvider databaseProvider,
			IRepositoryRegistry repositoryRegistry)
			: base(repositoryName, databaseProvider, repositoryRegistry)
		{
		}
	}
}
