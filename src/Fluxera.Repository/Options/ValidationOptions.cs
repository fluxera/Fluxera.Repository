namespace Fluxera.Repository.Options
{
	using JetBrains.Annotations;

	/// <summary>
	///     Provides the validation options.
	/// </summary>
	[PublicAPI]
	public sealed class ValidationOptions
	{
		/// <summary>
		///     Creates a new instance of the <see cref="ValidationOptions" /> type.
		/// </summary>
		/// <param name="repositoryName"></param>
		public ValidationOptions(RepositoryName repositoryName)
		{
			this.RepositoryName = repositoryName;
		}

		/// <summary>
		///     Gets the repository name.
		/// </summary>
		public RepositoryName RepositoryName { get; }

		/// <summary>
		///     Flag, if the validation is enabled.
		/// </summary>
		public bool IsEnabled { get; set; }
	}
}
