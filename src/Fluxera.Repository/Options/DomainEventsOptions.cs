namespace Fluxera.Repository.Options
{
	using System;
	using System.Collections.Generic;
	using JetBrains.Annotations;

	/// <summary>
	///     Provides the options for the domain events feature.
	/// </summary>
	[PublicAPI]
	public sealed class DomainEventsOptions
	{
		private bool isAutomaticCrudDomainEventsEnabled;

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

		/// <summary>
		///     Flag, indicating if the automatic adding of CRUD domain events is enabled.
		/// </summary>
		/// <remarks>
		///     The default is disabled. If the domain events feature is disabled altogether
		///     the automatic CRUD domain events creates in also disabled.
		/// </remarks>
		public bool IsAutomaticCrudDomainEventsEnabled
		{
			get => this.IsEnabled && this.isAutomaticCrudDomainEventsEnabled;
			set => this.isAutomaticCrudDomainEventsEnabled = value;
		}

		/// <summary>
		///     Gets the event handler types.
		/// </summary>
		public IList<Type> DomainEventHandlerTypes { get; } = new List<Type>();
	}
}
