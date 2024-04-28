namespace Fluxera.Repository.Options
{
	using Fluxera.Repository.DomainEvents;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	[PublicAPI]
	internal sealed class DomainEventsOptionsBuilder : IDomainEventsOptionsBuilder
	{
		private readonly IServiceCollection services;

		public DomainEventsOptionsBuilder(RepositoryOptions _, IServiceCollection services)
		{
			this.services = services;
		}

		/// <inheritdoc />
		public IDomainEventsOptionsBuilder AddDomainEventsReducer<T>() where T : class, IDomainEventsReducer
		{
			this.services.AddTransient<IDomainEventsReducer, T>();

			return this;
		}
	}
}
