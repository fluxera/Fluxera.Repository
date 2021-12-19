namespace Fluxera.Repository.Options
{
	using JetBrains.Annotations;

	/// <summary>
	///     Provides the options for the interception feature.
	/// </summary>
	[PublicAPI]
	public sealed class InterceptionOptions
	{
		/// <summary>
		///     Creates a new instance of the <see cref="InterceptionOptions" /> type.
		/// </summary>
		/// <param name="repositoryName"></param>
		public InterceptionOptions(RepositoryName repositoryName)
		{
			this.RepositoryName = repositoryName;
		}

		/// <summary>
		///     Gets the repository name.
		/// </summary>
		public RepositoryName RepositoryName { get; }

		/// <summary>
		///     Flag, if the interception is enabled.
		/// </summary>
		public bool IsEnabled { get; set; }
	}
}
