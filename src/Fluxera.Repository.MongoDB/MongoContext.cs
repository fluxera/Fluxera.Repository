namespace Fluxera.Repository.MongoDB
{
	using System;
	using System.Collections.Concurrent;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Guards;
	using Fluxera.Repository.DomainEvents;
	using Fluxera.Repository.Options;
	using Fluxera.Utilities;
	using global::MongoDB.Driver;
	using global::MongoDB.Driver.Core.Clusters;
	using JetBrains.Annotations;
	using MadEyeMatt.MongoDB.DbContext;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     A base class for context implementations for the MongoDB repository.
	/// </summary>
	[PublicAPI]
	public abstract class MongoContext : Disposable, IDisposable
	{
		private static readonly ClientSessionOptions sessionOptions = new ClientSessionOptions
		{
			DefaultTransactionOptions = new TransactionOptions(ReadConcern.Majority, ReadPreference.Primary, WriteConcern.WMajority)
		};

		private MongoDbContext context;
		private ConcurrentQueue<Func<Task>> commands;

		private bool isConfigured;

		/// <summary>
		///     Initializes a new instance of the <see cref="MongoContext" /> type.
		/// </summary>
		protected MongoContext()
		{
			// Command will be stored and later processed on saving changes.
			this.commands = new ConcurrentQueue<Func<Task>>();
		}

		private IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		///     Gets the current session.
		/// </summary>
		public IClientSessionHandle Session { get; private set; }

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
				await this.ExecuteCommandsAsync(cancellationToken);
				await this.DispatchDomainEventsAsync();

				// Commit the transaction if a session with a transaction exists.
				if(this.context.Session is not null && this.context.Session.IsInTransaction)
				{
					await this.CommitTransactionAsync(cancellationToken);
				}
			}
			catch
			{
				// Abort the transaction on error, if a session with a transaction exists
				if(this.context.Session is not null && this.context.Session.IsInTransaction)
				{
					await this.AbortTransactionAsync(cancellationToken);
				}
			}
			finally
			{
				this.ClearCommands();
				this.ClearDomainEvents();

				// Start a new transaction, if a session without transaction exists.
				if(this.context.Session is not null && !this.context.Session.IsInTransaction)
				{
					await this.BeginTransactionAsync(cancellationToken);
				}
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
		///     Starts a client session.
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<IClientSessionHandle> StartSessionAsync(CancellationToken cancellationToken = default)
		{
			this.Session = await this.context.StartSessionAsync(sessionOptions, cancellationToken);
			return this.Session;
		}

		/// <summary>
		///     Starts a transaction.
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
		{
			IClientSessionHandle session = await this.StartSessionAsync(cancellationToken).ConfigureAwait(false);

			if(!session.IsInTransaction)
			{
				session.StartTransaction();
			}
		}

		/// <summary>
		///     Commits the transaction.
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
		{
			if(this.context.Session is null)
			{
				throw new InvalidOperationException("The session hasn't been created.");
			}

			if(!this.context.Session.IsInTransaction)
			{
				throw new InvalidOperationException("The session isn't in an active transaction.");
			}

			return this.context.Session.CommitTransactionAsync(cancellationToken);
		}

		/// <summary>
		///     Aborts the transaction.
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public Task AbortTransactionAsync(CancellationToken cancellationToken = default)
		{
			if(this.context.Session is null)
			{
				throw new InvalidOperationException("The session hasn't been created.");
			}

			if(!this.context.Session.IsInTransaction)
			{
				throw new InvalidOperationException("The session isn't in an active transaction.");
			}

			return this.context.Session.AbortTransactionAsync(cancellationToken);
		}

		/// <summary>
		///     Gets a collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IMongoCollection<T> GetCollection<T>()
		{
			return this.context.GetCollection<T>();
		}

		/// <inheritdoc />
		protected override void DisposeManaged()
		{
			this.ClearCommands();
			this.context?.Dispose();
		}

		/// <summary>
		///     Configures the options to use for this context instance over its lifetime.
		/// </summary>
		protected abstract void ConfigureOptions(MongoContextOptions options);

		internal void Configure(RepositoryName repositoryName, IServiceProvider serviceProvider)
		{
			if(!this.isConfigured)
			{
				MongoContextOptions options = new MongoContextOptions();

				this.ConfigureOptions(options);

				this.context = (MongoDbContext)serviceProvider.GetRequiredService(options.DbContextType);

				// Start a transaction, if UoW is configured and the cluster is a replica set..
				IRepositoryRegistry repositoryRegistry = serviceProvider.GetRequiredService<IRepositoryRegistry>();
				RepositoryOptions repositoryOptions = repositoryRegistry.GetRepositoryOptionsFor(repositoryName);

				if(repositoryOptions.IsUnitOfWorkEnabled)
				{
					if(this.context.Client.Cluster.Description.Type == ClusterType.ReplicaSet)
					{
						this.StartSession();
						this.BeginTransaction();
					}
					else
					{
						ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
						ILogger logger = loggerFactory.CreateLogger(typeof(MongoContext));
						logger.LogUnitOfWorkEnabledWithoutReplicaSet(repositoryName.Name);
					}
				}

				this.RepositoryName = repositoryName;
				this.ServiceProvider = serviceProvider;

				this.isConfigured = true;
			}
		}

		private IClientSessionHandle StartSession()
		{
			return AsyncHelper.RunSync(() => this.StartSessionAsync());
		}

		private void BeginTransaction()
		{
			AsyncHelper.RunSync(() => this.BeginTransactionAsync());
		}

		private void ClearCommands()
		{
			this.commands?.Clear();
		}

		private void ClearDomainEvents()
		{
			IOutboxDomainEventDispatcher outboxDispatcher = this.ServiceProvider.GetRequiredService<IOutboxDomainEventDispatcher>();
			outboxDispatcher.Clear();
		}

		private async Task ExecuteCommandsAsync(CancellationToken cancellationToken)
		{
			foreach(Func<Task> command in this.commands)
			{
				cancellationToken.ThrowIfCancellationRequested();

				await command.Invoke().ConfigureAwait(false);
			}
		}

		private async Task DispatchDomainEventsAsync()
		{
			IOutboxDomainEventDispatcher outboxDispatcher = this.ServiceProvider.GetRequiredService<IOutboxDomainEventDispatcher>();
			await outboxDispatcher.FlushAsync();
		}
	}
}
