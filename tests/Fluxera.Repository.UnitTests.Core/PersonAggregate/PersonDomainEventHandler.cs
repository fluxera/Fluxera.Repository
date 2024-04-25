namespace Fluxera.Repository.UnitTests.Core.PersonAggregate
{
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity.DomainEvents;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

	[UsedImplicitly]
	public class PersonDomainEventHandler : IDomainEventHandler<PersonDomainEvent>
	{
		private readonly ILogger<PersonDomainEventHandler> logger;

		public PersonDomainEventHandler(ILogger<PersonDomainEventHandler> logger)
		{
			this.logger = logger;
		}

		/// <inheritdoc />
		public Task HandleAsync(PersonDomainEvent domainEvent, CancellationToken cancellationToken)
		{
			this.logger.LogInformation("PersonDomainEventHandler called.");
			return Task.CompletedTask;
		}
	}
}
