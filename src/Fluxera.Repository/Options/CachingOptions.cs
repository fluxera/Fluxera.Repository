namespace Fluxera.Repository.Options
{
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class CachingOptions
	{
		public CachingOptions(string repositoryName)
		{
			this.RepositoryName = repositoryName;
		}

		// TODO: Enable/Disable for specific aggregates.

		public string RepositoryName { get; }

		public bool Enabled { get; set; }
	}
}
