namespace Fluxera.Repository.Options
{
	using JetBrains.Annotations;

	[PublicAPI]
	internal sealed class CachingOptionsBuilder : ICachingOptionsBuilder
	{
		public CachingOptionsBuilder(RepositoryOptions repositoryOptions)
		{
			this.CachingOptions = repositoryOptions.CachingOptions;
		}

		public CachingOptions CachingOptions { get; }
	}
}
