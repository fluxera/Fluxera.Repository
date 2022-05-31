namespace Fluxera.Repository.InMemory
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Specifications;
	using Fluxera.StronglyTypedId;

	internal sealed class InMemoryRepository<TAggregateRoot, TKey> : LinqRepositoryBase<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private static readonly ConcurrentDictionary<TKey, TAggregateRoot> Store = new ConcurrentDictionary<TKey, TAggregateRoot>();

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
			return Task.FromResult(queryable.FirstOrDefault()!);
		}

		/// <inheritdoc />
		protected override Task<TResult> FirstOrDefaultAsync<TResult>(IQueryable<TResult> queryable, CancellationToken cancellationToken)
		{
			return Task.FromResult(queryable.FirstOrDefault()!);
		}

		/// <inheritdoc />
		protected override Task AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			item.ID = GenerateKey();
			Store.TryAdd(item.ID, item);
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		protected override Task AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				item.ID = GenerateKey();
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

		private static TKey GenerateKey()
		{
			Type keyType = typeof(TKey);

			if(keyType.IsStronglyTypedId())
			{
				keyType = keyType.GetValueType();
				object value = GenerateKey(keyType);
				object key = Activator.CreateInstance(typeof(TKey), BindingFlags.Public | BindingFlags.Instance, null, new object[] { value }, null);
				return (TKey)key;
			}

			if(keyType == typeof(string))
			{
				return (TKey)Convert.ChangeType(Guid.NewGuid().ToString("N"), keyType);
			}

			if(keyType == typeof(Guid))
			{
				return (TKey)Convert.ChangeType(Guid.NewGuid(), keyType);
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

			throw new InvalidOperationException("A key could not be generated. The in-memory repository only supports guid, string, int and long for keys.");
		}

		private static object GenerateKey(Type keyType)
		{
			if(keyType == typeof(string))
			{
				return Guid.NewGuid().ToString("N");
			}

			if(keyType == typeof(Guid))
			{
				return Guid.NewGuid();
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
