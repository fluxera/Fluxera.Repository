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
		///     Configures the options to use for this context instance over it's lifetime.
		/// </summary>
		protected abstract void ConfigureOptions(LiteContextOptions options);

		internal void Configure(RepositoryName repositoryName, DatabaseProvider databaseProvider)
		{
			if(!this.isConfigured)
			{
				LiteContextOptions options = new LiteContextOptions();

				this.ConfigureOptions(options);

				string databaseName = options.Database;

				Guard.Against.NullOrWhiteSpace(databaseName);

				this.database = databaseProvider.GetDatabase(repositoryName, databaseName);

				this.isConfigured = true;
			}
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
	}
}
