namespace Fluxera.Repository.LiteDB
{
	using JetBrains.Annotations;

	/// <summary>
	///     Provides the options for the LiteDB repository implementation.
	/// </summary>
	[PublicAPI]
	public sealed class LiteContextOptions
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="LiteContextOptions" /> type.
		/// </summary>
		/// <param name="repositoryName"></param>
		public LiteContextOptions(RepositoryName repositoryName)
		{
			this.RepositoryName = repositoryName;
		}

		/// <summary>
		///     Gets the name of the repository this options belong to.
		/// </summary>
		public RepositoryName RepositoryName { get; }

		/// <summary>
		///     Gets or sets the filename of the database to use.
		/// </summary>
		public string Database { get; set; }
	}
}
