namespace Sample.Domain.Company.DomainEvents
{
	using System.Collections.Generic;
	using Fluxera.DomainEvents.Abstractions;
	using Fluxera.Repository.DomainEvents;
	using JetBrains.Annotations;
#if NET6_0
	using Fluxera.Utilities.Extensions;
#endif

	[PublicAPI]
	public sealed class SampleDomainEventsReducer : IDomainEventsReducer
	{
		/// <inheritdoc />
		public IReadOnlyCollection<IDomainEvent> Reduce(IReadOnlyCollection<IDomainEvent> domainEvents)
		{
			IList<IDomainEvent> reducedDomainEvents = new List<IDomainEvent>();

			foreach(IDomainEvent domainEvent in domainEvents)
			{
				if(domainEvent.DisplayName != "CompanyRemoved")
				{
					reducedDomainEvents.Add(domainEvent);
				}
			}

			return reducedDomainEvents.AsReadOnly();
		}
	}
}
