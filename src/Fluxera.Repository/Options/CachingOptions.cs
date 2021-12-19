namespace Fluxera.Repository.Options
{
	using JetBrains.Annotations;

	/// <summary>
	///     Provides the caching options.
	/// </summary>
	[PublicAPI]
	public sealed class CachingOptions
	{
		/// <summary>
		///     Creates a new instance of the <see cref="CachingOptions" /> type.
		/// </summary>
		/// <param name="repositoryName"></param>
		public CachingOptions(RepositoryName repositoryName)
		{
			this.RepositoryName = repositoryName;
		}

		// TODO: Enable/Disable for specific aggregates.

		/// <summary>
		///     Gets the repository name.
		/// </summary>
		public RepositoryName RepositoryName { get; }

		/// <summary>
		///     Flag, if the caching is enabled.
		/// </summary>
		public bool IsEnabled { get; set; }
	}
}
