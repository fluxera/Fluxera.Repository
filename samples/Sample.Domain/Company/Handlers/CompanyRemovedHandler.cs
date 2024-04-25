namespace Sample.Domain.Company.Handlers
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity.DomainEvents;
	using JetBrains.Annotations;
	using Sample.Domain.Company.Events;

	[UsedImplicitly]
	public sealed class CompanyRemovedHandler : IDomainEventHandler<CompanyRemoved>
	{
		/// <inheritdoc />
		public Task HandleAsync(CompanyRemoved domainEvent, CancellationToken cancellationToken)
		{
			Console.WriteLine($"HANDLE COMPANY REMOVED: ID = {domainEvent.ID}");

			return Task.CompletedTask;
		}
	}
}
