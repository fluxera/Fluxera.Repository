﻿namespace Fluxera.Repository
{
	using Fluxera.Guards;
	using Fluxera.ValueObject;
	using JetBrains.Annotations;

	/// <summary>
	///     A value-object for the name of repository. This exists to prevent
	///     mix-ups between other string values and the name of a repository.
	/// </summary>
	[PublicAPI]
	public sealed class RepositoryName : ValueObject<RepositoryName>
	{
		/// <summary>
		///     Creates a new instance of the <see cref="RepositoryName" /> type.
		/// </summary>
		/// <param name="name"></param>
		public RepositoryName(string name)
		{
			Guard.Against.NullOrWhiteSpace(name, nameof(name));

			this.Name = name;
		}

		/// <summary>
		///     Gets the name of the repository.
		/// </summary>
		public string Name { get; }

		/// <summary>
		///     Converts the given <see cref="RepositoryName" /> to a <see cref="string" />.
		/// </summary>
		/// <param name="repositoryName"></param>
		public static explicit operator string(RepositoryName repositoryName)
		{
			return repositoryName.Name;
		}

		/// <summary>
		///     Converts the given <see cref="string" /> to a <see cref="RepositoryName" />.
		/// </summary>
		/// <param name="repositoryName"></param>
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
