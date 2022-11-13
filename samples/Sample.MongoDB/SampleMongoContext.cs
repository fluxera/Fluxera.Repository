namespace Sample.MongoDB
{
	using Fluxera.Repository.MongoDB;

	public class SampleMongoContext : MongoContext
	{
		/// <inheritdoc />
		protected override void ConfigureOptions(MongoContextOptions options)
		{
			options.ConnectionString = "mongodb://localhost:27017";
			options.Database = "sample";
		}
	}
}
