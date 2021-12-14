namespace Fluxera.Repository.Options
{
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class InterceptionOptions
	{
		public InterceptionOptions(string repositoryName)
		{
			this.RepositoryName = repositoryName;
		}

		public string RepositoryName { get; }

		public bool IsEnabled { get; set; }
	}
}
