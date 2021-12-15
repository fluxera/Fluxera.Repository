namespace Fluxera.Repository.MongoDB
{
	internal sealed class MongoPersistenceSettings
	{
		public string ConnectionString { get; set; }

		public string Database { get; set; }

		public bool UseSsl { get; set; }
	}
}
