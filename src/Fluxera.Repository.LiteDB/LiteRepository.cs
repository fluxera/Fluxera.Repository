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
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using Fluxera.StronglyTypedId;
	using global::LiteDB.Async;

	internal sealed class LiteRepository<TAggregateRoot, TKey> : RepositoryBase<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly ILiteCollectionAsync<TAggregateRoot> collection;
		private readonly LiteContext context;
		private readonly SequentialGuidGenerator sequentialGuidGenerator;

		public LiteRepository(
			LiteContextProvider contextProvider,
			IRepositoryRegistry repositoryRegistry,
			SequentialGuidGenerator sequentialGuidGenerator)
		{
			Guard.Against.Null(contextProvider);
			Guard.Against.Null(repositoryRegistry);

			this.sequentialGuidGenerator = Guard.Against.Null(sequentialGuidGenerator);

			RepositoryName repositoryName = repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();
			this.context = contextProvider.GetContextFor(repositoryName);
			this.collection = this.context.GetCollection<TAggregateRoot>();
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
			Task PerformAddAsync()
			{
				item.ID = this.GenerateKey();

				return this.collection.InsertAsync(item).Then(cancellationToken);
			}

			await this.context
				.AddCommandAsync(PerformAddAsync)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> itemList = items.ToList();

			Task PerformAddRangeAsync()
			{
				foreach(TAggregateRoot item in itemList)
				{
					item.ID = this.GenerateKey();
				}

				return this.collection.InsertBulkAsync(itemList).Then(cancellationToken);
			}

			await this.context
				.AddCommandAsync(PerformAddRangeAsync)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			Task PerformRemoveRangeAsync()
			{
				return this.collection.DeleteManyAsync(specification.Predicate).Then(cancellationToken);
			}

			await this.context
				.AddCommandAsync(PerformRemoveRangeAsync)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> itemsList = items.ToList();

			Task PerformRemoveRangeAsync()
			{
				IList<ISpecification<TAggregateRoot>> specifications = new List<ISpecification<TAggregateRoot>>();

				foreach(TAggregateRoot item in itemsList)
				{
					specifications.Add(this.CreatePrimaryKeySpecification(item.ID));
				}

				ManyOrSpecification<TAggregateRoot> specification = new ManyOrSpecification<TAggregateRoot>(specifications);
				return this.collection.DeleteManyAsync(specification.Predicate).Then(cancellationToken);
			}

			await this.context
				.AddCommandAsync(PerformRemoveRangeAsync)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			Task PerformUpdateAsync()
			{
				return this.collection.UpdateAsync(item).Then(cancellationToken);
			}

			await this.context
				.AddCommandAsync(PerformUpdateAsync)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> itemsList = items.ToList();

			Task PerformUpdateRangeAsync()
			{
				return this.collection.UpdateAsync(itemsList).Then(cancellationToken);
			}

			await this.context
				.AddCommandAsync(PerformUpdateRangeAsync)
				.ConfigureAwait(false);
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

		/// <inheritdoc />
		protected override async Task<int> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<int> values = await this.FindManyAsync(specification, selector, QueryOptions<TAggregateRoot>.Empty(), cancellationToken);
			return values.Sum();
		}

		/// <inheritdoc />
		protected override async Task<int> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> values = await this.collection
				.Query()
				.Where(specification.Predicate)
				.ToListAsync()
				.ConfigureAwait(false);

			return values.AsQueryable().Sum(selector).GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<long> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<long> values = await this.FindManyAsync(specification, selector, QueryOptions<TAggregateRoot>.Empty(), cancellationToken);
			return values.Sum();
		}

		/// <inheritdoc />
		protected override async Task<long> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> values = await this.collection
				.Query()
				.Where(specification.Predicate)
				.ToListAsync()
				.ConfigureAwait(false);

			return values.AsQueryable().Sum(selector).GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<decimal> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<decimal> values = await this.FindManyAsync(specification, selector, QueryOptions<TAggregateRoot>.Empty(), cancellationToken);
			return values.Sum();
		}

		/// <inheritdoc />
		protected override async Task<decimal> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> values = await this.collection
				.Query()
				.Where(specification.Predicate)
				.ToListAsync()
				.ConfigureAwait(false);

			return values.AsQueryable().Sum(selector).GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<float> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<float> values = await this.FindManyAsync(specification, selector, QueryOptions<TAggregateRoot>.Empty(), cancellationToken);
			return values.Sum();
		}

		/// <inheritdoc />
		protected override async Task<float> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> values = await this.collection
				.Query()
				.Where(specification.Predicate)
				.ToListAsync()
				.ConfigureAwait(false);

			return values.AsQueryable().Sum(selector).GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<double> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<double> values = await this.FindManyAsync(specification, selector, QueryOptions<TAggregateRoot>.Empty(), cancellationToken);
			return values.Sum();
		}

		/// <inheritdoc />
		protected override async Task<double> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> values = await this.collection
				.Query()
				.Where(specification.Predicate)
				.ToListAsync()
				.ConfigureAwait(false);

			return values.AsQueryable().Sum(selector).GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<int> values = await this.FindManyAsync(specification, selector, QueryOptions<TAggregateRoot>.Empty(), cancellationToken);
			return values.Average();
		}

		/// <inheritdoc />
		protected override async Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> values = await this.collection
				.Query()
				.Where(specification.Predicate)
				.ToListAsync()
				.ConfigureAwait(false);

			return values.AsQueryable().Average(selector).GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<long> values = await this.FindManyAsync(specification, selector, QueryOptions<TAggregateRoot>.Empty(), cancellationToken);
			return values.Average();
		}

		/// <inheritdoc />
		protected override async Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> values = await this.collection
				.Query()
				.Where(specification.Predicate)
				.ToListAsync()
				.ConfigureAwait(false);

			return values.AsQueryable().Average(selector).GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<decimal> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<decimal> values = await this.FindManyAsync(specification, selector, QueryOptions<TAggregateRoot>.Empty(), cancellationToken);
			return values.Average();
		}

		/// <inheritdoc />
		protected override async Task<decimal> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> values = await this.collection
				.Query()
				.Where(specification.Predicate)
				.ToListAsync()
				.ConfigureAwait(false);

			return values.AsQueryable().Average(selector).GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<float> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<float> values = await this.FindManyAsync(specification, selector, QueryOptions<TAggregateRoot>.Empty(), cancellationToken);
			return values.Average();
		}

		/// <inheritdoc />
		protected override async Task<float> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> values = await this.collection
				.Query()
				.Where(specification.Predicate)
				.ToListAsync()
				.ConfigureAwait(false);

			return values.AsQueryable().Average(selector).GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			IReadOnlyCollection<double> values = await this.FindManyAsync(specification, selector, QueryOptions<TAggregateRoot>.Empty(), cancellationToken);
			return values.Average();
		}

		/// <inheritdoc />
		protected override async Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> values = await this.collection
				.Query()
				.Where(specification.Predicate)
				.ToListAsync()
				.ConfigureAwait(false);

			return values.AsQueryable().Average(selector).GetValueOrDefault();
		}

		private TKey GenerateKey()
		{
			Type keyType = typeof(TKey);

			if(keyType == typeof(string))
			{
				Guid key = this.sequentialGuidGenerator.Generate();
				return (TKey)Convert.ChangeType(key.ToString("D"), keyType);
			}

			if(keyType == typeof(Guid))
			{
				Guid key = this.sequentialGuidGenerator.Generate();
				return (TKey)Convert.ChangeType(key, keyType);
			}

			if(keyType.IsStronglyTypedId())
			{
				object keyValue;

				Type valueType = keyType.GetStronglyTypedIdValueType();
				if(valueType == typeof(Guid))
				{
					Guid key = this.sequentialGuidGenerator.Generate();
					keyValue = key;
				}
				else if(valueType == typeof(string))
				{
					Guid key = this.sequentialGuidGenerator.Generate();
					keyValue = key.ToString("D");
				}
				else
				{
					throw new InvalidOperationException("A key could not be generated. The LiteDB repository only supports guid or string as type for strongly-typed keys.");
				}

				object instance = Activator.CreateInstance(keyType, new object[] { keyValue });
				return (TKey)instance;
			}

			throw new InvalidOperationException("A key could not be generated. The LiteDB repository only supports guid or string as type for keys.");
		}
	}
}
