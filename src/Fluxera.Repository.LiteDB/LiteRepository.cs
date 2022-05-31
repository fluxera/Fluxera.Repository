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
	using Fluxera.StronglyTypedId;
	using Fluxera.Utilities.Extensions;
	using global::LiteDB.Async;

	internal sealed class LiteRepository<TAggregateRoot, TKey> : RepositoryBase<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly ILiteCollectionAsync<TAggregateRoot> collection;
		private readonly SequentialGuidGenerator sequentialGuidGenerator;

		public LiteRepository(
			IRepositoryRegistry repositoryRegistry,
			IDatabaseProvider databaseProvider,
			SequentialGuidGenerator sequentialGuidGenerator,
			IDatabaseNameProvider databaseNameProvider = null)
		{
			Guard.Against.Null(repositoryRegistry);
			Guard.Against.Null(databaseProvider);

			this.sequentialGuidGenerator = Guard.Against.Null(sequentialGuidGenerator);

			RepositoryName repositoryName = repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();
			RepositoryOptions options = repositoryRegistry.GetRepositoryOptionsFor(repositoryName);

			LitePersistenceSettings persistenceSettings = new LitePersistenceSettings
			{
				Database = (string)options.Settings.GetOrDefault("Lite.Database")
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

			LiteDatabaseAsync database = databaseProvider.GetDatabase(repositoryName, databaseName);
			this.collection = database.GetCollection<TAggregateRoot>(collectionName);
		}

		private static string Name => "Fluxera.Repository.LiteRepository";

		/// <inheritdoc />
		public override string ToString()
		{
			return Name;
		}

		/// <inheritdoc />
		protected override async Task AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			TKey key = this.GenerateKey();
			item.ID = key;

			await this.collection.InsertAsync(item).ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> itemList = items.ToList();

			foreach(TAggregateRoot item in itemList)
			{
				TKey key = this.GenerateKey();
				item.ID = key;
			}

			await this.collection.InsertBulkAsync(itemList).ConfigureAwait(false);
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
				specifications.Add(this.CreatePrimaryKeySpecification(item.ID!));
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
		protected override async Task<TAggregateRoot> FindOneAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.collection
				.Query()
				.Where(specification.Predicate)
				.Apply(queryOptions)
				.FirstOrDefaultAsync()
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<TResult> FindOneAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
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
		protected override async Task<IReadOnlyCollection<TAggregateRoot>> FindManyAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
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
		protected override async Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
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

		private TKey GenerateKey()
		{
			Guid key = this.sequentialGuidGenerator.Generate();

			Type keyType = typeof(TKey);
			if(keyType == typeof(Guid))
			{
				object guidKey = key;
				return (TKey)guidKey;
			}

			if(keyType == typeof(string))
			{
				object stringKey = key.ToString("D");
				return (TKey)stringKey;
			}

			if(keyType.IsStronglyTypedId())
			{
				object keyValue;

				Type valueType = keyType.GetValueType();
				if(valueType == typeof(Guid))
				{
					keyValue = key;
				}
				else if(valueType == typeof(string))
				{
					keyValue = key.ToString("D");
				}
				else
				{
					throw new InvalidOperationException("The LiteDB repository only supports guid or string as type for strongly-typed keys.");
				}

				object instance = Activator.CreateInstance(keyType, new object[] { keyValue });
				return (TKey)instance;
			}

			throw new InvalidOperationException("The LiteDB repository only supports guid or string as type for keys.");
		}
	}
}
