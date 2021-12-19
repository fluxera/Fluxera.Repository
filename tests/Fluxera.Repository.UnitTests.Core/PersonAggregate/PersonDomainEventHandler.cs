namespace Fluxera.Repository.UnitTests.Core.PersonAggregate
{
	using System.Threading.Tasks;
	using Fluxera.Entity.DomainEvents;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

	[UsedImplicitly]
	public class PersonDomainEventHandler : IDomainEventHandler<PersonDomainEvent>
	{
		private readonly ILogger<PersonCommittedDomainEventHandler> logger;

		public PersonDomainEventHandler(ILogger<PersonCommittedDomainEventHandler> logger)
		{
			this.logger = logger;
		}

		/// <inheritdoc />
		public async Task HandleAsync(PersonDomainEvent domainEvent)
		{
			this.logger.LogInformation("PersonCommittedDomainEventHandler called.");
		}
	}
}
