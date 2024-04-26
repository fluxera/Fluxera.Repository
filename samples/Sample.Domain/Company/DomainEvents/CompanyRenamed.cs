namespace Sample.Domain.Company.DomainEvents
{
	using Fluxera.Entity.DomainEvents;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class CompanyRenamed : IDomainEvent
	{
	}
}
