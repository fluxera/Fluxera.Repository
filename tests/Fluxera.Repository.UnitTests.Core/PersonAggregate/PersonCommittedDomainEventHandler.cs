namespace Fluxera.Repository.UnitTests.Core.PersonAggregate
{
	using System.Threading.Tasks;
	using Fluxera.Entity.DomainEvents;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

	[UsedImplicitly]
	public class PersonCommittedDomainEventHandler : ICommittedDomainEventHandler<PersonDomainEvent>
	{
		private readonly ILogger<PersonCommittedDomainEventHandler> logger;

		public PersonCommittedDomainEventHandler(ILogger<PersonCommittedDomainEventHandler> logger)
		{
			this.logger = logger;
		}

		/// <inheritdoc />
		public Task HandleAsync(PersonDomainEvent domainEvent)
		{
			this.logger.LogInformation("PersonCommittedDomainEventHandler called.");
			return Task.CompletedTask;
		}
	}
}
