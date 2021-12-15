namespace Fluxera.Repository.EntityFramework
{
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class EntityFrameworkPersistenceSettings
	{
		public string ConnectionString { get; set; }

		public bool LogSQL { get; set; }
	}
}
