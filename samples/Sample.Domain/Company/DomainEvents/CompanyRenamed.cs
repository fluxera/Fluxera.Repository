namespace Sample.Domain.Company.DomainEvents
{
	using Fluxera.DomainEvents.Abstractions;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class CompanyRenamed : IDomainEvent
	{
	}
}
