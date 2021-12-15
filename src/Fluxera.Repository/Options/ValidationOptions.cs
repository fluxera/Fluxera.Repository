namespace Fluxera.Repository.Options
{
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class ValidationOptions
	{
		public ValidationOptions(RepositoryName repositoryName)
		{
			this.RepositoryName = repositoryName;
		}

		public RepositoryName RepositoryName { get; }

		//public IList<Type> ValidatorFactoryTypes { get; } = new List<Type>();

		public bool IsEnabled { get; set; }
	}
}
