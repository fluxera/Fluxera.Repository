namespace Fluxera.Repository.MongoDB
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Security.Authentication;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Options;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Traits;
	using Fluxera.Utilities.Extensions;
	using global::MongoDB.Driver;
	using Microsoft.Extensions.Logging;

	// TODO: Transactions; https://gist.github.com/codepope/1366893d703a0be57953545619e87eea
	internal sealed class MongoRepository<TAggregateRoot, TKey> : IRepository<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		private readonly MongoClient client;
		private readonly IMongoCollection<TAggregateRoot> collection;
		private readonly IMongoDatabase database;
		private readonly ILogger logger;

		public MongoRepository(
			ILoggerFactory loggerFactory,
			IRepositoryRegistry repositoryRegistry,
			IDatabaseNameProvider? databaseNameProvider = null)
		{
			this.logger = loggerFactory.CreateLogger(Name);

			RepositoryName repositoryName = repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();
			RepositoryOptions options = repositoryRegistry.GetRepositoryOptionsFor(repositoryName);

			MongoPersistenceSettings persistenceSettings = new MongoPersistenceSettings
			{
				ConnectionString = (string)options.SettingsValues.GetOrDefault("Mongo.ConnectionString"),
				Database = (string)options.SettingsValues.GetOrDefault("Mongo.Database")
			};

			object settingsUseSsl = options.SettingsValues.GetOrDefault("Mongo.UseSsl");
			persistenceSettings.UseSsl = (bool)(settingsUseSsl ?? false);

			string connectionString = persistenceSettings.ConnectionString;
			string databaseName = persistenceSettings.Database;
			string collectionName = typeof(TAggregateRoot).Name.Pluralize();

			// If a custom database name provider is available use this to resolve the database name dynamically.
			if(databaseNameProvider != null)
			{
				databaseName = databaseNameProvider.GetDatabaseName(typeof(TAggregateRoot));
			}

			Guard.Against.NullOrWhiteSpace(connectionString, nameof(connectionString));
			Guard.Against.NullOrWhiteSpace(databaseName, nameof(databaseName));
			Guard.Against.NullOrWhiteSpace(collectionName, nameof(collectionName));

			MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

			if(persistenceSettings.UseSsl)
			{
				settings.SslSettings = new SslSettings
				{
					EnabledSslProtocols = SslProtocols.Tls12,
				};
			}

			this.client = new MongoClient(settings);
			this.database = this.client.GetDatabase(databaseName);
			this.collection = this.database.GetCollection<TAggregateRoot>(collectionName);
		}

		private static string Name => "Fluxera.Repository.MongoRepository";

		private bool IsDisposed { get; set; }

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.collection
				.InsertOneAsync(item, cancellationToken: cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.collection
				.InsertManyAsync(items, cancellationToken: cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.collection
				.ReplaceOneAsync(x => x.ID == item.ID, item, cancellationToken: cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IList<WriteModel<TAggregateRoot>> updates = new List<WriteModel<TAggregateRoot>>();
			foreach(TAggregateRoot item in items)
			{
				Expression<Func<TAggregateRoot, bool>> predicate = x => x.ID == item.ID;
				updates.Add(new ReplaceOneModel<TAggregateRoot>(predicate, item));
			}

			await this.collection.BulkWriteAsync(updates, new BulkWriteOptions
			{
				IsOrdered = false,
			}, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.collection
				.DeleteOneAsync(x => x.ID == item.ID, cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			await this.collection
				.DeleteOneAsync(x => x.ID == id, cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			await this.collection
				.DeleteManyAsync(predicate, cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> itemsList = items.ToList();

			IList<WriteModel<TAggregateRoot>> deletes = new List<WriteModel<TAggregateRoot>>();
			foreach(TAggregateRoot item in itemsList)
			{
				Expression<Func<TAggregateRoot, bool>> predicate = x => x.ID == item.ID;
				deletes.Add(new DeleteOneModel<TAggregateRoot>(predicate));
			}

			await this.collection.BulkWriteAsync(deletes, new BulkWriteOptions
				{
					IsOrdered = false
				}, cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot, TKey>.GetAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.collection
				.Find(x => x.ID == id)
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			return await this.collection
				.Find(x => x.ID == id)
				.Project(selector)
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot, TKey>.ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.collection
				.Find(x => x.ID == id)
				.AnyAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.collection
				.Find(predicate)
				.ApplyOptions(queryOptions)
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.collection
				.Find(predicate)
				.ApplyOptions(queryOptions)
				.Project(selector)
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot, TKey>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.collection
				.Find(predicate)
				.AnyAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.collection
				.Find(predicate)
				.ApplyOptions(queryOptions)
				.ToListAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.collection
				.Find(predicate)
				.ApplyOptions(queryOptions)
				.Project(selector)
				.ToListAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(CancellationToken cancellationToken)
		{
			return await this.collection
				.CountDocumentsAsync(x => true, cancellationToken: cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.collection
				.CountDocumentsAsync(predicate, cancellationToken: cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			this.IsDisposed = true;
		}

		/// <inheritdoc />
		bool IReadOnlyRepository<TAggregateRoot, TKey>.IsDisposed => this.IsDisposed;

		/// <inheritdoc />
		ValueTask IAsyncDisposable.DisposeAsync()
		{
			try
			{
				this.Dispose();
				return default;
			}
			catch(Exception exception)
			{
				return new ValueTask(Task.FromException(exception));
			}
		}
	}
}
