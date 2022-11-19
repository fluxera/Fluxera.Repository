namespace Sample.Domain.Company.Handlers
{
	using System;
	using System.Threading.Tasks;
	using Fluxera.Entity.DomainEvents;
	using JetBrains.Annotations;
	using Sample.Domain.Company.Events;

	[UsedImplicitly]
	public sealed class CompanyAddedHandler : IDomainEventHandler<CompanyAdded>
	{
		/// <inheritdoc />
		public Task HandleAsync(CompanyAdded domainEvent)
		{
			Console.WriteLine("HANDLE COMPANY ADDED");

			return Task.CompletedTask;
		}
	}
}
