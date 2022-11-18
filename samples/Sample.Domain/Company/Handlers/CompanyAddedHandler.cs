namespace Sample.Domain.Company.Handlers
{
	using System;
	using System.Threading.Tasks;
	using Fluxera.Entity.DomainEvents;
	using JetBrains.Annotations;
	using Sample.Domain.Company.Events;

	[UsedImplicitly]
	public sealed class CompanyAddedHandler : ICommittedDomainEventHandler<CompanyAdded>
	{
		/// <inheritdoc />
		public Task HandleAsync(CompanyAdded domainEvent)
		{
			Console.WriteLine("HANDLE COMMITTED");

			return Task.CompletedTask;
		}
	}
}
