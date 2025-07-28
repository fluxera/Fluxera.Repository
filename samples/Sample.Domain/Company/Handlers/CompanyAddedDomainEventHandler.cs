namespace Sample.Domain.Company.Handlers
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.DomainEvents;
	using JetBrains.Annotations;
	using Sample.Domain.Company.DomainEvents;

	[UsedImplicitly]
	public sealed class CompanyAddedDomainEventHandler : IDomainEventHandler<CompanyAdded>
	{
		/// <inheritdoc />
		public ValueTask HandleAsync(CompanyAdded domainEvent, CancellationToken cancellationToken = new CancellationToken())
		{
			Console.WriteLine($"HANDLE COMPANY ADDED: ID = {domainEvent.ID}");

			return ValueTask.CompletedTask;
		}
	}
}
