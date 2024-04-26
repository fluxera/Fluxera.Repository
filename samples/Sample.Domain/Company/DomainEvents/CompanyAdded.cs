﻿namespace Sample.Domain.Company.DomainEvents
{
	using Fluxera.Entity.DomainEvents;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class CompanyAdded : IDomainEvent
	{
		public CompanyAdded(CompanyId id)
		{
			this.ID = id;
		}

		public CompanyId ID { get; }
	}
}
