namespace Fluxera.Repository.Options
{
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class InterceptionOptions
	{
		public InterceptionOptions(RepositoryName repositoryName)
		{
			this.RepositoryName = repositoryName;
		}

		public RepositoryName RepositoryName { get; }

		public bool IsEnabled { get; set; }
	}
}
