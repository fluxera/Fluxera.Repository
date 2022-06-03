namespace Fluxera.Repository.InMemory
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.LiteDB;
	using Fluxera.Repository.Specifications;
	using Fluxera.StronglyTypedId;

	internal sealed class InMemoryRepository<TAggregateRoot, TKey> : LinqRepositoryBase<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private static readonly ConcurrentDictionary<TKey, TAggregateRoot> Store = new ConcurrentDictionary<TKey, TAggregateRoot>();

		private readonly SequentialGuidGenerator sequentialGuidGenerator;

		public InMemoryRepository(SequentialGuidGenerator sequentialGuidGenerator)
		{
			this.sequentialGuidGenerator = Guard.Against.Null(sequentialGuidGenerator);
		}

		private static string Name => "Fluxera.Repository.InMemoryRepository";

		/// <inheritdoc />
		protected override IQueryable<TAggregateRoot> Queryable => Store.Values.AsQueryable();

		/// <inheritdoc />
		public override string ToString()
		{
			return Name;
		}

		/// <inheritdoc />
		protected override Task<long> LongCountAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.LongCount());
		}

		/// <inheritdoc />
		protected override Task<int> SumAsync(IQueryable<int> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Sum());
		}

		/// <inheritdoc />
		protected override Task<int> SumAsync(IQueryable<int?> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Sum().GetValueOrDefault());
		}

		/// <inheritdoc />
		protected override Task<long> SumAsync(IQueryable<long> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Sum());
		}

		/// <inheritdoc />
		protected override Task<long> SumAsync(IQueryable<long?> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Sum().GetValueOrDefault());
		}

		/// <inheritdoc />
		protected override Task<decimal> SumAsync(IQueryable<decimal> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Sum());
		}

		/// <inheritdoc />
		protected override Task<decimal> SumAsync(IQueryable<decimal?> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Sum().GetValueOrDefault());
		}

		/// <inheritdoc />
		protected override Task<float> SumAsync(IQueryable<float> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Sum());
		}

		/// <inheritdoc />
		protected override Task<float> SumAsync(IQueryable<float?> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Sum().GetValueOrDefault());
		}

		/// <inheritdoc />
		protected override Task<double> SumAsync(IQueryable<double> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Sum());
		}

		/// <inheritdoc />
		protected override Task<double> SumAsync(IQueryable<double?> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Sum().GetValueOrDefault());
		}

		/// <inheritdoc />
		protected override Task<double> AverageAsync(IQueryable<int> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Average());
		}

		/// <inheritdoc />
		protected override Task<double> AverageAsync(IQueryable<int?> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Average().GetValueOrDefault());
		}

		/// <inheritdoc />
		protected override Task<double> AverageAsync(IQueryable<long> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Average());
		}

		/// <inheritdoc />
		protected override Task<double> AverageAsync(IQueryable<long?> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Average().GetValueOrDefault());
		}

		/// <inheritdoc />
		protected override Task<decimal> AverageAsync(IQueryable<decimal> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Average());
		}

		/// <inheritdoc />
		protected override Task<decimal> AverageAsync(IQueryable<decimal?> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Average().GetValueOrDefault());
		}

		/// <inheritdoc />
		protected override Task<float> AverageAsync(IQueryable<float> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Average());
		}

		/// <inheritdoc />
		protected override Task<float> AverageAsync(IQueryable<float?> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Average().GetValueOrDefault());
		}

		/// <inheritdoc />
		protected override Task<double> AverageAsync(IQueryable<double> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Average());
		}

		/// <inheritdoc />
		protected override Task<double> AverageAsync(IQueryable<double?> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.Average().GetValueOrDefault());
		}

		/// <inheritdoc />
		protected override Task<IReadOnlyCollection<TAggregateRoot>> ToListAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult<IReadOnlyCollection<TAggregateRoot>>(queryable.ToList());
		}

		/// <inheritdoc />
		protected override Task<IReadOnlyCollection<TResult>> ToListAsync<TResult>(IQueryable<TResult> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult<IReadOnlyCollection<TResult>>(queryable.ToList());
		}

		/// <inheritdoc />
		protected override Task<TAggregateRoot> FirstOrDefaultAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.FirstOrDefault());
		}

		/// <inheritdoc />
		protected override Task<TResult> FirstOrDefaultAsync<TResult>(IQueryable<TResult> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.FirstOrDefault());
		}

		/// <inheritdoc />
		protected override Task AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			item.ID = this.GenerateKey();
			Store.TryAdd(item.ID, item);
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		protected override Task AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				item.ID = this.GenerateKey();
				Store.TryAdd(item.ID, item);
			}

			return Task.CompletedTask;
		}

		/// <inheritdoc />
		protected override Task RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			IQueryable<TAggregateRoot> items = this.Queryable.Where(specification.Predicate);
			foreach(TAggregateRoot item in items)
			{
				Store.TryRemove(item.ID, out _);
			}

			return Task.CompletedTask;
		}

		/// <inheritdoc />
		protected override Task RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				Store.TryRemove(item.ID, out _);
			}

			return Task.CompletedTask;
		}

		/// <inheritdoc />
		protected override Task UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			Store[item.ID] = item;
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		protected override Task UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				Store[item.ID] = item;
			}

			return Task.CompletedTask;
		}

		/// <summary>
		///     Creates an <see cref="Expression" /> in the form of <c>x => x.ID.Equals(id)</c> for the given ID value.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected override Expression<Func<TAggregateRoot, bool>> CreatePrimaryKeyPredicate(TKey id)
		{
			Expression<Func<TAggregateRoot, bool>> predicate = x => x.ID.Equals(id);
			return predicate;
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

			if(keyType == typeof(int))
			{
				TKey pkValue = Store.Keys.LastOrDefault();

				int nextInt = Convert.ToInt32(pkValue) + 1;
				return (TKey)Convert.ChangeType(nextInt, keyType);
			}

			if(keyType == typeof(long))
			{
				TKey pkValue = Store.Keys.LastOrDefault();

				int nextInt = Convert.ToInt32(pkValue) + 1;
				return (TKey)Convert.ChangeType(nextInt, keyType);
			}

			if(keyType.IsStronglyTypedId())
			{
				Type valueType = keyType.GetValueType();
				object value = this.GenerateKey(valueType);
				object key = Activator.CreateInstance(typeof(TKey), new object[] { value });
				return (TKey)key;
			}

			throw new InvalidOperationException("A key could not be generated. The in-memory repository only supports guid, string, int and long for keys.");
		}

		private object GenerateKey(Type keyType)
		{
			if(keyType == typeof(string))
			{
				Guid key = this.sequentialGuidGenerator.Generate();
				return key.ToString("D");
			}

			if(keyType == typeof(Guid))
			{
				return this.sequentialGuidGenerator.Generate();
			}

			if(keyType == typeof(int))
			{
				IStronglyTypedId<TKey, int> pkValue = Store.Keys.LastOrDefault() as IStronglyTypedId<TKey, int>;

				int nextInt = (pkValue?.Value ?? 0) + 1;
				return nextInt;
			}

			if(keyType == typeof(long))
			{
				IStronglyTypedId<TKey, long> pkValue = Store.Keys.LastOrDefault() as IStronglyTypedId<TKey, long>;

				long nextInt = (pkValue?.Value ?? 0) + 1;
				return nextInt;
			}

			throw new InvalidOperationException("A key could not be generated. The in-memory repository only supports guid, string, int and long for keys.");
		}
	}
}
