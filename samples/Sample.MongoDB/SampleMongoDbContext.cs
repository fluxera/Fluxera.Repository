namespace Sample.MongoDB
{
	using Fluxera.Utilities.Extensions;
	using MadEyeMatt.MongoDB.DbContext;

	public class SampleMongoDbContext : MongoDbContext
	{
		/// <inheritdoc />
		protected override void OnConfiguring(MongoDbContextOptionsBuilder builder)
		{
			if(!builder.IsConfigured)
			{
				builder.UseDatabase("mongodb://localhost:27017", "sample");
			}
		}

		/// <inheritdoc />
		public override string GetCollectionName<TDocument>()
		{
			return typeof(TDocument).Name.Pluralize();
		}
	}
}
