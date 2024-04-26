namespace Sample.Domain.Company.Handlers
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity.DomainEvents;
	using JetBrains.Annotations;
	using Sample.Domain.Company.DomainEvents;

	[UsedImplicitly]
	public sealed class CompanyAddedDomainEventHandler : IDomainEventHandler<CompanyAdded>
	{
		/// <inheritdoc />
		public Task HandleAsync(CompanyAdded domainEvent, CancellationToken cancellationToken)
		{
			Console.WriteLine($"HANDLE COMPANY ADDED: ID = {domainEvent.ID}");

			return Task.CompletedTask;
		}
	}
}
