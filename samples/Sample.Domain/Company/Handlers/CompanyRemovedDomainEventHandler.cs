namespace Sample.Domain.Company.Handlers
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.DomainEvents.MediatR;
	using JetBrains.Annotations;
	using Sample.Domain.Company.DomainEvents;

	[UsedImplicitly]
	public sealed class CompanyRemovedDomainEventHandler : IDomainEventHandler<CompanyRemoved>
	{
		/// <inheritdoc />
		public Task HandleAsync(CompanyRemoved domainEvent, CancellationToken cancellationToken)
		{
			Console.WriteLine($"HANDLE COMPANY REMOVED: ID = {domainEvent.ID}");

			return Task.CompletedTask;
		}
	}
}
