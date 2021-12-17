namespace Fluxera.Repository.LiteDB
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Options;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Traits;
	using Fluxera.Utilities.Extensions;
	using global::LiteDB.Async;
	using Microsoft.Extensions.Logging;

	internal sealed class LiteRepository<TAggregateRoot> : IRepository<TAggregateRoot>
		where TAggregateRoot : AggregateRoot<TAggregateRoot>
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

		private bool IsDisposed { get; set; }

		async Task ICanAdd<TAggregateRoot>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.collection.InsertAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot>.AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.collection.InsertBulkAsync(items).ConfigureAwait(false);
		}

		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.collection.UpdateAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.collection.UpdateAsync(items).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.collection.DeleteAsync(item.ID).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(string id, CancellationToken cancellationToken)
		{
			await this.collection.DeleteAsync(id).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			await this.collection.DeleteManyAsync(predicate).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				await this.collection.DeleteAsync(item.ID).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot>.GetAsync(string id, CancellationToken cancellationToken)
		{
			return await this.collection.FindByIdAsync(id);
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot>.GetAsync<TResult>(string id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			return await this.collection
				.Query()
				.Where(x => x.ID == id)
				.Select(selector)
				.FirstOrDefaultAsync();
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot>.ExistsAsync(string id, CancellationToken cancellationToken = default)
		{
			return await this.collection
				.Query()
				.Where(x => x.ID == id)
				.ExistsAsync();
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(CancellationToken cancellationToken)
		{
			return await this.collection.LongCountAsync();
		}

		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.collection.LongCountAsync(predicate);
		}

		async Task<TAggregateRoot> ICanFind<TAggregateRoot>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.collection
				.Query()
				.Where(predicate)
				.ApplyOptions(queryOptions)
				.FirstOrDefaultAsync();
		}

		async Task<TResult> ICanFind<TAggregateRoot>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.collection
				.Query()
				.Where(predicate)
				.ApplyOptions(queryOptions)
				.Select(selector)
				.FirstOrDefaultAsync();
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.collection
				.Query()
				.Where(predicate)
				.ExistsAsync();
		}

		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> result = await this.collection
				.Query()
				.ApplyOptions(queryOptions)
				.ToListAsync();

			return result.AsReadOnly();
		}

		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			IList<TResult> result = await this.collection
				.Query()
				.Where(predicate)
				.ApplyOptions(queryOptions)
				.Select(selector)
				.ToListAsync();

			return result.AsReadOnly();
		}

		/// <inheritdoc />
		public void Dispose()
		{
			if(!this.IsDisposed)
			{
				this.databaseProvider.Dispose(this.repositoryName);
			}

			this.IsDisposed = true;
		}

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

		/// <inheritdoc />
		bool IReadOnlyRepository<TAggregateRoot>.IsDisposed => this.IsDisposed;
	}
}
