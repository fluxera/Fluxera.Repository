namespace Sample.MongoDB
{
	using Fluxera.Repository;
	using Fluxera.Repository.MongoDB;

	public class SampleMongoContext : MongoContext
	{
		/// <inheritdoc />
		public SampleMongoContext(
			string repositoryName,
			IRepositoryRegistry repositoryRegistry)
			: base(repositoryName, repositoryRegistry)
		{
		}
	}
}
