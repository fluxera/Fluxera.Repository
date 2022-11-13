namespace Fluxera.Repository.MongoDB
{
	using System;
	using System.Collections.Concurrent;
	using System.Linq;
	using System.Security.Authentication;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Guards;
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
		private static readonly ConcurrentDictionary<string, IMongoClient> Clients = new ConcurrentDictionary<string, IMongoClient>();

		private IMongoClient client;
		private ConcurrentQueue<Func<Task>> commands;
		private IMongoDatabase database;

		private bool isConfigured;

		/// <summary>
		///     Initializes a new instance of the <see cref="MongoContext" /> type.
		/// </summary>
		protected MongoContext()
		{
			// Command will be stored and later processed on saving changes.
			this.commands = new ConcurrentQueue<Func<Task>>();
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
		///     Configures the options to use for this context instance over it's lifetime.
		/// </summary>
		protected abstract void ConfigureOptions(MongoContextOptions options);

		internal void Configure(RepositoryName repositoryName)
		{
			if(!this.isConfigured)
			{
				MongoContextOptions options = new MongoContextOptions();

				this.ConfigureOptions(options);

				string connectionString = options.ConnectionString;
				string databaseName = options.Database;

				Guard.Against.NullOrWhiteSpace(connectionString);
				Guard.Against.NullOrWhiteSpace(databaseName);

				// Create the client instance and cache it for the connection string. It is recommended to only have
				// a single client instance.
				// See: https://mongodb.github.io/mongo-csharp-driver/2.18/reference/driver/connecting/
				if(!Clients.ContainsKey(connectionString))
				{
					MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

					InstrumentationOptions instrumentationOptions = new InstrumentationOptions
					{
						CaptureCommandText = options.CaptureCommandText
					};
					settings.ClusterConfigurator = clusterBuilder => clusterBuilder.Subscribe(new DiagnosticsActivityEventSubscriber(instrumentationOptions));

					if(options.UseSsl)
					{
						settings.SslSettings = new SslSettings
						{
							EnabledSslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13
						};
					}

					IMongoClient mongoClient = new MongoClient(settings);
					if(!Clients.TryAdd(connectionString, mongoClient))
					{
						throw new InvalidOperationException("The MongoDB client could not be added to the client cache.");
					}
				}

				this.client = Clients[connectionString];
				this.database = this.client.GetDatabase(databaseName);


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
