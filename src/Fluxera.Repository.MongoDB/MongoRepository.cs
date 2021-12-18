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
	using Fluxera.Repository.Specifications;
	using Fluxera.Utilities.Extensions;
	using global::MongoDB.Driver;
	using global::MongoDB.Driver.Linq;
	using Microsoft.Extensions.Logging;

	// TODO: Transactions; https://gist.github.com/codepope/1366893d703a0be57953545619e87eea
	internal sealed class MongoRepository<TAggregateRoot, TKey> : LinqRepositoryBase<TAggregateRoot, TKey>
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

		/// <inheritdoc />
		protected override IQueryable<TAggregateRoot> Queryable => this.collection.AsQueryable();

		/// <inheritdoc />
		protected override async Task AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.collection
				.InsertOneAsync(item, cancellationToken: cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.collection
				.InsertManyAsync(items, cancellationToken: cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			await this.collection
				.DeleteManyAsync(specification.Predicate, cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> itemsList = items.ToList();

			IList<WriteModel<TAggregateRoot>> deletes = new List<WriteModel<TAggregateRoot>>();
			foreach(TAggregateRoot item in itemsList)
			{
				Expression<Func<TAggregateRoot, bool>> predicate = this.CreatePrimaryKeyPredicate(item.ID);
				deletes.Add(new DeleteOneModel<TAggregateRoot>(predicate));
			}

			await this.collection
				.BulkWriteAsync(deletes, new BulkWriteOptions { IsOrdered = false }, cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.collection
				.ReplaceOneAsync(this.CreatePrimaryKeyPredicate(item.ID), item, cancellationToken: cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IList<WriteModel<TAggregateRoot>> updates = new List<WriteModel<TAggregateRoot>>();
			foreach(TAggregateRoot item in items)
			{
				Expression<Func<TAggregateRoot, bool>> predicate = this.CreatePrimaryKeyPredicate(item.ID);
				updates.Add(new ReplaceOneModel<TAggregateRoot>(predicate, item));
			}

			await this.collection
				.BulkWriteAsync(updates, new BulkWriteOptions { IsOrdered = false }, cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<TAggregateRoot> FirstOrDefaultAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.ToMongoQueryable()
				.FirstOrDefaultAsync(cancellationToken);
		}

		/// <inheritdoc />
		protected override async Task<TResult> FirstOrDefaultAsync<TResult>(IQueryable<TResult> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.ToMongoQueryable()
				.FirstOrDefaultAsync(cancellationToken);
		}

		/// <inheritdoc />
		protected override async Task<long> LongCountAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.ToMongoQueryable()
				.LongCountAsync(cancellationToken);
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<TAggregateRoot>> ToListAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.ToMongoQueryable()
				.ToListAsync(cancellationToken);
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<TResult>> ToListAsync<TResult>(IQueryable<TResult> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.ToMongoQueryable()
				.ToListAsync(cancellationToken);
		}
	}
}
