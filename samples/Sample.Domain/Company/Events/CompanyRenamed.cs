namespace Sample.Domain.Company.Events
{
	using Fluxera.Entity.DomainEvents;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class CompanyRenamed : IDomainEvent
	{
	}
}
