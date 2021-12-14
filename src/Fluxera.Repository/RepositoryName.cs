namespace Fluxera.Repository
{
	using Fluxera.Guards;
	using Fluxera.ValueObject;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class RepositoryName : ValueObject<RepositoryName>
	{
		public RepositoryName(string name)
		{
			Guard.Against.NullOrWhiteSpace(name, nameof(name));

			this.Name = name;
		}

		public string Name { get; }

		public static explicit operator string(RepositoryName repositoryName)
		{
			return repositoryName.Name;
		}

		public static explicit operator RepositoryName(string repositoryName)
		{
			return new RepositoryName(repositoryName);
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return this.Name;
		}
	}
}
