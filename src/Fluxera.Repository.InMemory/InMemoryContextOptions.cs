namespace Fluxera.Repository.InMemory
{
	using JetBrains.Annotations;

	/// <summary>
	///     Provides the options for the in-memory repository implementation.
	/// </summary>
	[PublicAPI]
	public sealed class InMemoryContextOptions
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="InMemoryContextOptions" /> type.
		/// </summary>
		/// <param name="repositoryName"></param>
		public InMemoryContextOptions(RepositoryName repositoryName)
		{
			this.RepositoryName = repositoryName;
		}

		/// <summary>
		///     Gets the name of the repository this options belong to.
		/// </summary>
		public RepositoryName RepositoryName { get; }

		/// <summary>
		///     Gets or sets the name of the database to use.
		/// </summary>
		public string Database { get; set; } = string.Empty;
	}
}
