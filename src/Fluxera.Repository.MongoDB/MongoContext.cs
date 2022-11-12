namespace Fluxera.Repository.MongoDB
{
	using System;
	using System.Collections.Concurrent;
	using System.Linq;
	using System.Security.Authentication;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Guards;
	using Fluxera.Repository.Options;
	using Fluxera.Utilities;
	using Fluxera.Utilities.Extensions;
	using global::MongoDB.Driver;
	using global::MongoDB.Driver.Core.Clusters;
	using global::MongoDB.Driver.Core.Extensions.DiagnosticSources;
	using JetBrains.Annotations;

	/// <summary>
	///     A base class for context implementations for the MongoDB repository.
	/// </summary>
	[PublicAPI]
	public abstract class MongoContext : Disposable
	{
		private readonly RepositoryName repositoryName;
		private readonly IRepositoryRegistry repositoryRegistry;

		private IMongoClient client;
		private ConcurrentQueue<Func<Task>> commands;
		private IMongoDatabase database;

		/// <summary>
		///     Initializes a new instance of the <see cref="MongoContext" /> type.
		/// </summary>
		/// <param name="repositoryName"></param>
		/// <param name="repositoryRegistry"></param>
		protected MongoContext(
			string repositoryName,
			IRepositoryRegistry repositoryRegistry)
		{
			this.repositoryName = (RepositoryName)repositoryName;
			this.repositoryRegistry = repositoryRegistry;

			// Command will be stored and later processed on saving changes.
			this.commands = new ConcurrentQueue<Func<Task>>();

			this.Configure();
		}

		/// <summary>
		///     Gets the session for this context.
		/// </summary>
		public IClientSessionHandle Session { get; private set; }

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

				ClusterType clusterType = this.client.Cluster.Description.Type;
				if(clusterType == ClusterType.ReplicaSet)
				{
					using(this.Session = await this.client.StartSessionAsync(cancellationToken: cancellationToken))
					{
						this.Session.StartTransaction();

						await this.ExecuteCommands(cancellationToken);

						await this.Session.CommitTransactionAsync(cancellationToken);
					}
				}
				else
				{
					await this.ExecuteCommands(cancellationToken);
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
		public IMongoCollection<T> GetCollection<T>()
		{
			string collectionName = typeof(T).Name.Pluralize();
			return this.database.GetCollection<T>(collectionName);
		}

		/// <inheritdoc />
		protected override void DisposeManaged()
		{
			this.ClearCommands();
			this.Session?.Dispose();
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
			if(this.client is null)
			{
				RepositoryOptions options = this.repositoryRegistry.GetRepositoryOptionsFor(this.repositoryName);

				MongoPersistenceSettings persistenceSettings = new MongoPersistenceSettings
				{
					ConnectionString = (string)options.Settings.GetOrDefault("Mongo.ConnectionString"),
					Database = (string)options.Settings.GetOrDefault("Mongo.Database")
				};

				object settingsUseSsl = options.Settings.GetOrDefault("Mongo.UseSsl");
				persistenceSettings.UseSsl = (bool)(settingsUseSsl ?? false);

				string connectionString = persistenceSettings.ConnectionString;
				string databaseName = persistenceSettings.Database;

				// TODO
				//// If a custom database name provider is available use this to resolve the database name dynamically.
				//if (databaseNameProvider != null)
				//{
				//	databaseName = databaseNameProvider.GetDatabaseName(typeof(TAggregateRoot));
				//}

				Guard.Against.NullOrWhiteSpace(connectionString, nameof(connectionString));
				Guard.Against.NullOrWhiteSpace(databaseName, nameof(databaseName));

				MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

				object captureCommandText = options.Settings.GetOrDefault("Mongo.CaptureCommandText");
				InstrumentationOptions instrumentationOptions = new InstrumentationOptions
				{
					CaptureCommandText = (bool)(captureCommandText ?? true)
				};
				settings.ClusterConfigurator = clusterBuilder => clusterBuilder.Subscribe(new DiagnosticsActivityEventSubscriber(instrumentationOptions));

				if(persistenceSettings.UseSsl)
				{
					settings.SslSettings = new SslSettings
					{
						EnabledSslProtocols = SslProtocols.Tls12
					};
				}

				this.client = new MongoClient(settings);
				this.database = this.client.GetDatabase(databaseName);
			}
		}
	}
}
