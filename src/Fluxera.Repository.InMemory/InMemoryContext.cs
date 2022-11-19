namespace Fluxera.Repository.InMemory
{
	using System;
	using System.Collections.Concurrent;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Guards;
	using Fluxera.Utilities;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     A base class for context implementations for the in-memory repository.
	/// </summary>
	[PublicAPI]
	public abstract class InMemoryContext : Disposable
	{
		private ConcurrentQueue<Func<Task>> commands;

		private bool isConfigured;

		/// <summary>
		///     Initializes a new instance of the <see cref="InMemoryContext" /> type.
		/// </summary>
		protected InMemoryContext()
		{
			// Command will be stored and later processed on saving changes.
			this.commands = new ConcurrentQueue<Func<Task>>();
		}

		private IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		///     Gets the name of the repository this context belong to.
		/// </summary>
		protected RepositoryName RepositoryName { get; private set; }

		internal string Database { get; set; } = string.Empty;

		/// <summary>
		///     Adds a command for execution.
		/// </summary>
		/// <param name="command"></param>
		public Task AddCommandAsync(Func<Task> command)
		{
			Guard.Against.Null(command);

			this.commands ??= new ConcurrentQueue<Func<Task>>();
			this.commands.Enqueue(command);

			return Task.CompletedTask;
		}

		/// <summary>
		///     Saves the changes made by the registered commands.
		/// </summary>
		/// <returns></returns>
		public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			if(!this.commands.Any())
			{
				return;
			}

			try
			{
				await this.ExecuteCommands(cancellationToken);
				await this.DispatchDomainEventsAsync();
			}
			finally
			{
				this.ClearCommands();
				this.ClearDomainEvents();
			}
		}

		/// <summary>
		///     Checks if any commands were added for execution.
		/// </summary>
		/// <returns></returns>
		public bool HasChanges()
		{
			return this.commands.Any();
		}

		/// <summary>
		///     Discards all changes.
		/// </summary>
		public void DiscardChanges()
		{
			this.ClearCommands();
		}

		/// <inheritdoc />
		protected override void DisposeManaged()
		{
			this.ClearCommands();
		}

		/// <summary>
		///     Configures the options to use for this context instance over it's lifetime.
		/// </summary>
		protected abstract void ConfigureOptions(InMemoryContextOptions options);

		internal void Configure(RepositoryName repositoryName, IServiceProvider serviceProvider)
		{
			if(!this.isConfigured)
			{
				InMemoryContextOptions contextOptions = new InMemoryContextOptions(repositoryName);

				this.ConfigureOptions(contextOptions);

				this.Database = contextOptions.Database;

				this.RepositoryName = repositoryName;
				this.ServiceProvider = serviceProvider;

				this.isConfigured = true;
			}
		}

		private void ClearCommands()
		{
			this.commands?.Clear();
		}

		private void ClearDomainEvents()
		{
			OutboxDomainEventDispatcher outboxDispatcher = this.ServiceProvider.GetRequiredService<OutboxDomainEventDispatcher>();
			outboxDispatcher.Clear();
		}

		private async Task ExecuteCommands(CancellationToken cancellationToken)
		{
			foreach(Func<Task> command in this.commands)
			{
				cancellationToken.ThrowIfCancellationRequested();

				await command.Invoke().ConfigureAwait(false);
			}
		}

		private async Task DispatchDomainEventsAsync()
		{
			OutboxDomainEventDispatcher outboxDispatcher = this.ServiceProvider.GetRequiredService<OutboxDomainEventDispatcher>();
			await outboxDispatcher.FlushAsync();
		}
	}
}
