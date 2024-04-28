namespace Sample.Domain.Company.DomainEvents
{
	using Fluxera.DomainEvents.Abstractions;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class CompanyUpdated : IDomainEvent
	{
		public CompanyUpdated(CompanyId id)
		{
			this.ID = id;
		}

		public CompanyId ID { get; }
	}
}
