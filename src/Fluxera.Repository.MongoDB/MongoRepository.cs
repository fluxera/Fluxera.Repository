namespace Fluxera.Repository.MongoDB
{
	using System;
	using System.Collections.Generic;
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
	internal sealed class MongoRepository<TAggregateRoot> : IRepository<TAggregateRoot>
		where TAggregateRoot : AggregateRoot<TAggregateRoot>
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
		async Task ICanAdd<TAggregateRoot>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.collection
				.InsertOneAsync(item, cancellationToken: cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot>.AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.collection
				.InsertManyAsync(items, cancellationToken: cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.collection
				.ReplaceOneAsync(item.ID, item, cancellationToken: cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IList<WriteModel<TAggregateRoot>> updates = new List<WriteModel<TAggregateRoot>>();
			foreach(TAggregateRoot item in items)
			{
				updates.Add(new ReplaceOneModel<TAggregateRoot>(item.ID, item));
			}

			await this.collection.BulkWriteAsync(updates, new BulkWriteOptions { IsOrdered = false }, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.collection
				.DeleteOneAsync(item.ID, cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(string id, CancellationToken cancellationToken)
		{
			await this.collection
				.DeleteOneAsync(id, cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			await this.collection
				.DeleteOneAsync(predicate, cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IList<WriteModel<TAggregateRoot>> deletes = new List<WriteModel<TAggregateRoot>>();
			foreach(TAggregateRoot item in items)
			{
				deletes.Add(new DeleteOneModel<TAggregateRoot>(item.ID));
			}

			await this.collection.BulkWriteAsync(deletes, new BulkWriteOptions { IsOrdered = false }, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot>.GetAsync(string id, CancellationToken cancellationToken)
		{
			return await this.collection
				.Find(id)
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot>.GetAsync<TResult>(string id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			return await this.collection
				.Find(id)
				.Project(selector)
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot>.ExistsAsync(string id, CancellationToken cancellationToken)
		{
			return await this.collection
				.Find(id)
				.AnyAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.collection
				.Find(predicate)
				.ApplyOptions(queryOptions)
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.collection
				.Find(predicate)
				.ApplyOptions(queryOptions)
				.Project(selector)
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.collection
				.Find(predicate)
				.AnyAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.collection
				.Find(predicate)
				.ApplyOptions(queryOptions)
				.ToListAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.collection
				.Find(predicate)
				.ApplyOptions(queryOptions)
				.Project(selector)
				.ToListAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(CancellationToken cancellationToken)
		{
			return await this.collection
				.CountDocumentsAsync(x => true, cancellationToken: cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.collection
				.CountDocumentsAsync(predicate, cancellationToken: cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		void IDisposable.Dispose()
		{
			this.IsDisposed = true;
		}

		/// <inheritdoc />
		bool IReadOnlyRepository<TAggregateRoot>.IsDisposed => this.IsDisposed;
	}
}
