﻿namespace Fluxera.Repository.Options
{
	using JetBrains.Annotations;

	/// <summary>
	///     Provides the options for the domain events feature.
	/// </summary>
	[PublicAPI]
	public sealed class DomainEventsOptions
	{
		/// <summary>
		///     Creates a new instance of the <see cref="DomainEventsOptions" /> type.
		/// </summary>
		/// <param name="repositoryName"></param>
		public DomainEventsOptions(RepositoryName repositoryName)
		{
			this.RepositoryName = repositoryName;
		}

		/// <summary>
		///     Gets the repository name.
		/// </summary>
		public RepositoryName RepositoryName { get; }

		/// <summary>
		///     Flag, if the domain events are enabled.
		/// </summary>
		public bool IsEnabled { get; set; }
	}
}
