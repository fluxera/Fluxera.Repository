namespace Fluxera.Repository.Options
{
	using System;
	using System.Collections.Generic;
	using JetBrains.Annotations;

	/// <summary>
	///     Provides the options for a repository.
	/// </summary>
	[PublicAPI]
	public sealed class RepositoryOptions
	{
		/// <summary>
		///     Creates a new instance of the <see cref="RepositoryOptions" /> type.
		/// </summary>
		/// <param name="repositoryName"></param>
		public RepositoryOptions(RepositoryName repositoryName)
		{
			this.RepositoryName = repositoryName;
			this.ValidationOptions = new ValidationOptions(repositoryName);
			this.DomainEventsOptions = new DomainEventsOptions(repositoryName);
			this.CachingOptions = new CachingOptions(repositoryName);
			this.InterceptionOptions = new InterceptionOptions(repositoryName);
		}

		/// <summary>
		///     Gets the repository name.
		/// </summary>
		public RepositoryName RepositoryName { get; }

		/// <summary>
		///     Gets the aggregate root types this repository is used for.
		/// </summary>
		public IList<Type> AggregateRootTypes { get; } = new List<Type>();

		/// <summary>
		///     Gets the validation options.
		/// </summary>
		public ValidationOptions ValidationOptions { get; }

		/// <summary>
		///     Gets the domain events options.
		/// </summary>
		public DomainEventsOptions DomainEventsOptions { get; }

		/// <summary>
		///     Gets the caching options.
		/// </summary>
		public CachingOptions CachingOptions { get; }

		/// <summary>
		///     Gets the interception options.
		/// </summary>
		public InterceptionOptions InterceptionOptions { get; }

		/// <summary>
		///     Flag, indicating if the unit-of-work usage is enabled. The default is <c>false</c>.
		/// </summary>
		public bool IsUnitOfWorkEnabled { get; internal set; }

		/// <summary>
		///     Gets the type of the repository context.
		/// </summary>
		public Type RepositoryContextType { get; internal set; }
	}
}
