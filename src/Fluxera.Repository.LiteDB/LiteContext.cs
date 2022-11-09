namespace Fluxera.Repository.LiteDB
{
	using System;
	using System.Collections.Concurrent;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Guards;
	using Fluxera.Repository.Options;
	using Fluxera.Utilities;
	using Fluxera.Utilities.Extensions;
	using global::LiteDB.Async;
	using JetBrains.Annotations;

	[PublicAPI]
	public abstract class LiteContext : Disposable
	{
		private readonly DatabaseProvider databaseProvider;
		private readonly RepositoryName repositoryName;
		private readonly IRepositoryRegistry repositoryRegistry;

		private ConcurrentQueue<Func<Task>> commands;
		private LiteDatabaseAsync database;

		protected LiteContext(
			string repositoryName,
			DatabaseProvider databaseProvider,
			IRepositoryRegistry repositoryRegistry)
		{
			this.repositoryName = (RepositoryName)repositoryName;
			this.databaseProvider = databaseProvider;
			this.repositoryRegistry = repositoryRegistry;

			// Command will be stored and later processed on saving changes.
			this.commands = new ConcurrentQueue<Func<Task>>();

			this.Configure();
		}

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
			try
			{
				if(!this.commands.Any())
				{
					return;
				}

				using(ILiteDatabaseAsync transaction = await this.database.BeginTransactionAsync())
				{
					await this.ExecuteCommands(cancellationToken);

					await transaction.CommitAsync();
				}
			}
			finally
			{
				this.ClearCommands();
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

		/// <summary>
		///     Gets a collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public ILiteCollectionAsync<T> GetCollection<T>()
		{
			string collectionName = typeof(T).Name.Pluralize();
			return this.database.GetCollection<T>(collectionName);
		}

		/// <inheritdoc />
		protected override void DisposeManaged()
		{
			this.ClearCommands();
		}

		/// <summary>
		///     Removes all added commands.
		/// </summary>
		private void ClearCommands()
		{
			this.commands?.Clear();
		}

		private async Task ExecuteCommands(CancellationToken cancellationToken)
		{
			foreach(Func<Task> command in this.commands)
			{
				cancellationToken.ThrowIfCancellationRequested();

				await command.Invoke().ConfigureAwait(false);
			}
		}

		private void Configure()
		{
			RepositoryOptions options = this.repositoryRegistry.GetRepositoryOptionsFor(this.repositoryName);

			LitePersistenceSettings persistenceSettings = new LitePersistenceSettings
			{
				Database = (string)options.Settings.GetOrDefault("Lite.Database")
			};

			string databaseName = persistenceSettings.Database;

			//// If a custom database name provider is available use this to resolve the database name dynamically.
			//if(databaseNameProvider != null)
			//{
			//	databaseName = databaseNameProvider.GetDatabaseName(typeof(TAggregateRoot));
			//}

			Guard.Against.NullOrWhiteSpace(databaseName, nameof(databaseName));

			this.database = this.databaseProvider.GetDatabase(this.repositoryName, databaseName);
		}
	}
}
