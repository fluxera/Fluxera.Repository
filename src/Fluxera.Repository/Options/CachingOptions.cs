namespace Fluxera.Repository.Options
{
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class CachingOptions
	{
		public CachingOptions(RepositoryName repositoryName)
		{
			this.RepositoryName = repositoryName;
		}

		// TODO: Enable/Disable for specific aggregates.

		public RepositoryName RepositoryName { get; }

		public bool Enabled { get; set; }
	}
}
