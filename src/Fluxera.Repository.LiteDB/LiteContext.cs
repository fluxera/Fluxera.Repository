namespace Fluxera.Repository.LiteDB
{
	using System;
	using System.Collections.Concurrent;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Guards;
	using Fluxera.Utilities;
	using Fluxera.Utilities.Extensions;
	using global::LiteDB.Async;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     A base class for context implementations for the LiteDB repository.
	/// </summary>
	[PublicAPI]
	public abstract class LiteContext : Disposable
	{
		private ConcurrentQueue<Func<Task>> commands;
		private LiteDatabaseAsync database;

		private bool isConfigured;

		/// <summary>
		///     Initializes a new instance of the <see cref="LiteContext" /> type.
		/// </summary>
		protected LiteContext()
		{
			// Command will be stored and later processed on saving changes.
			this.commands = new ConcurrentQueue<Func<Task>>();
		}

		private IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		///     Gets the name of the repository this context belong to.
		/// </summary>
		protected RepositoryName RepositoryName { get; private set; }

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
		///     Saves the changes made by the registered commands inside a transaction.
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
				using(ILiteDatabaseAsync transaction = await this.database.BeginTransactionAsync())
				{
					await this.ExecuteCommands(cancellationToken);
					await this.DispatchDomainEventsAsync();

					await transaction.CommitAsync();
				}
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
		protected abstract void ConfigureOptions(LiteContextOptions options);

		internal void Configure(RepositoryName repositoryName, IServiceProvider serviceProvider)
		{
			if(!this.isConfigured)
			{
				LiteContextOptions options = new LiteContextOptions(repositoryName);

				this.ConfigureOptions(options);

				string databaseName = options.Database;

				Guard.Against.NullOrWhiteSpace(databaseName);

				DatabaseProvider databaseProvider = serviceProvider.GetRequiredService<DatabaseProvider>();
				this.database = databaseProvider.GetDatabase(repositoryName, databaseName);

				this.RepositoryName = repositoryName;
				this.ServiceProvider = serviceProvider;

				this.isConfigured = true;
			}
		}

		/// <summary>
		///     Gets a collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		internal ILiteCollectionAsync<T> GetCollection<T>()
		{
			string collectionName = typeof(T).Name.Pluralize();
			return this.database.GetCollection<T>(collectionName);
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
