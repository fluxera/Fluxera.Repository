namespace Sample.Domain.Company.DomainEvents
{
	using Fluxera.Entity.DomainEvents;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class CompanyRemoved : IDomainEvent
	{
		public CompanyRemoved(CompanyId id)
		{
			this.ID = id;
		}

		public CompanyId ID { get; }
	}
}
