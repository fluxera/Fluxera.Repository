namespace Fluxera.Repository.LiteDB
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Options;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using Fluxera.Utilities.Extensions;
	using global::LiteDB.Async;
	using Microsoft.Extensions.Logging;

	internal sealed class LiteRepository<TAggregateRoot, TKey> : RepositoryBase<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		private readonly ILiteCollectionAsync<TAggregateRoot> collection;
		private readonly IDatabaseProvider databaseProvider;
		private readonly ILogger logger;
		private readonly RepositoryName repositoryName;

		public LiteRepository(
			ILoggerFactory loggerFactory,
			IRepositoryRegistry repositoryRegistry,
			IDatabaseProvider databaseProvider,
			IDatabaseNameProvider? databaseNameProvider = null)
		{
			Guard.Against.Null(loggerFactory, nameof(loggerFactory));
			Guard.Against.Null(repositoryRegistry, nameof(repositoryRegistry));
			Guard.Against.Null(databaseProvider, nameof(databaseProvider));

			this.logger = loggerFactory.CreateLogger(Name);
			this.databaseProvider = databaseProvider;

			this.repositoryName = repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();
			RepositoryOptions options = repositoryRegistry.GetRepositoryOptionsFor(this.repositoryName);

			LitePersistenceSettings persistenceSettings = new LitePersistenceSettings
			{
				Database = (string)options.SettingsValues.GetOrDefault("Lite.Database"),
			};

			string databaseName = persistenceSettings.Database;
			string collectionName = typeof(TAggregateRoot).Name.Pluralize().ToLower();

			// If a custom database name provider is available use this to resolve the database name dynamically.
			if(databaseNameProvider != null)
			{
				databaseName = databaseNameProvider.GetDatabaseName(typeof(TAggregateRoot));
			}

			Guard.Against.NullOrEmpty(databaseName, nameof(databaseName));
			Guard.Against.NullOrEmpty(collectionName, nameof(collectionName));

			LiteDatabaseAsync database = this.databaseProvider.GetDatabase(this.repositoryName, databaseName!);
			this.collection = database.GetCollection<TAggregateRoot>(collectionName);
		}

		private static string Name => "Fluxera.Repository.LiteRepository";

		/// <inheritdoc />
		protected override async Task AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.collection.InsertAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.collection.InsertBulkAsync(items).ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			await this.collection.DeleteManyAsync(specification.Predicate).ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IList<ISpecification<TAggregateRoot>> specifications = new List<ISpecification<TAggregateRoot>>();

			IList<TAggregateRoot> itemsList = items.ToList();
			foreach(TAggregateRoot item in itemsList)
			{
				specifications.Add(this.CreatePrimaryKeySpecification(item.ID));
			}

			ManyOrSpecification<TAggregateRoot> specification = new ManyOrSpecification<TAggregateRoot>(specifications);
			await this.RemoveRangeAsync(specification, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.collection.UpdateAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.collection.UpdateAsync(items).ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<TAggregateRoot> FindOneAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.collection
				.Query()
				.Where(specification.Predicate)
				.Apply(queryOptions)
				.FirstOrDefaultAsync()
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<TResult> FindOneAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.collection
				.Query()
				.Where(specification.Predicate)
				.Apply(queryOptions)
				.Select(selector)
				.FirstOrDefaultAsync()
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<TAggregateRoot>> FindManyAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.collection
				.Query()
				.Where(specification.Predicate)
				.Apply(queryOptions)
				.ToListAsync()
				.ConfigureAwait(false)
				.AsReadOnly();
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.collection
				.Query()
				.Where(specification.Predicate)
				.Apply(queryOptions)
				.Select(selector)
				.ToListAsync()
				.ConfigureAwait(false)
				.AsReadOnly();
		}

		/// <inheritdoc />
		protected override async Task<long> LongCountAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			return await this.collection.LongCountAsync(specification.Predicate);
		}

		/// <inheritdoc />
		protected override void DisposeManaged()
		{
			this.databaseProvider.Dispose(this.repositoryName);
		}
	}
}
