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
	using Fluxera.Repository.Options;
	using Fluxera.Repository.Specifications;
	using Fluxera.StronglyTypedId;

	internal sealed class InMemoryRepository<TAggregateRoot, TKey> : LinqRepositoryBase<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private static readonly InMemoryStorage<TKey, TAggregateRoot> Storage = new InMemoryStorage<TKey, TAggregateRoot>();

		private readonly InMemoryContext context;
		private readonly RepositoryOptions options;

		private readonly SequentialGuidGenerator sequentialGuidGenerator;

		public InMemoryRepository(
			InMemoryContextProvider contextProvider,
			IRepositoryRegistry repositoryRegistry,
			SequentialGuidGenerator sequentialGuidGenerator)
		{
			Guard.Against.Null(contextProvider);
			Guard.Against.Null(repositoryRegistry);
			this.sequentialGuidGenerator = Guard.Against.Null(sequentialGuidGenerator);

			RepositoryName repositoryName = repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();
			this.options = repositoryRegistry.GetRepositoryOptionsFor(repositoryName);

			this.context = contextProvider.GetContextFor(repositoryName);
		}

		private static string Name => "Fluxera.Repository.InMemoryRepository";

		/// <inheritdoc />
		protected override IQueryable<TAggregateRoot> Queryable => this.Store.Values.AsQueryable();

		private ConcurrentDictionary<TKey, TAggregateRoot> Store => Storage.GetStore(this.context.Database);

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
				this.Store.TryAdd(item.ID, item);

				return Task.CompletedTask;
			}

			if(this.options.IsUnitOfWorkEnabled)
			{
				await this.context
					.AddCommandAsync(PerformAddAsync)
					.ConfigureAwait(false);
			}
			else
			{
				await PerformAddAsync().ConfigureAwait(false);
			}
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
					this.Store.TryAdd(item.ID, item);
				}

				return Task.CompletedTask;
			}

			if(this.options.IsUnitOfWorkEnabled)
			{
				await this.context
					.AddCommandAsync(PerformAddRangeAsync)
					.ConfigureAwait(false);
			}
			else
			{
				await PerformAddRangeAsync().ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		protected override async Task RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			Task PerformRemoveRangeAsync()
			{
				IQueryable<TAggregateRoot> items = this.Queryable.Where(specification.Predicate);
				foreach(TAggregateRoot item in items)
				{
					this.Store.TryRemove(item.ID, out _);
				}

				return Task.CompletedTask;
			}

			if(this.options.IsUnitOfWorkEnabled)
			{
				await this.context
					.AddCommandAsync(PerformRemoveRangeAsync)
					.ConfigureAwait(false);
			}
			else
			{
				await PerformRemoveRangeAsync().ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		protected override async Task RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> itemList = items.ToList();

			Task PerformRemoveRangeAsync()
			{
				foreach(TAggregateRoot item in itemList)
				{
					this.Store.TryRemove(item.ID, out _);
				}

				return Task.CompletedTask;
			}

			if(this.options.IsUnitOfWorkEnabled)
			{
				await this.context
					.AddCommandAsync(PerformRemoveRangeAsync)
					.ConfigureAwait(false);
			}
			else
			{
				await PerformRemoveRangeAsync().ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		protected override async Task UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			Task PerformUpdateAsync()
			{
				this.Store[item.ID] = item;

				return Task.CompletedTask;
			}

			if(this.options.IsUnitOfWorkEnabled)
			{
				await this.context
					.AddCommandAsync(PerformUpdateAsync)
					.ConfigureAwait(false);
			}
			else
			{
				await PerformUpdateAsync().ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		protected override async Task UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> itemList = items.ToList();

			Task PerformUpdateRangeAsync()
			{
				foreach(TAggregateRoot item in itemList)
				{
					this.Store[item.ID] = item;
				}

				return Task.CompletedTask;
			}

			if(this.options.IsUnitOfWorkEnabled)
			{
				await this.context
					.AddCommandAsync(PerformUpdateRangeAsync)
					.ConfigureAwait(false);
			}
			else
			{
				await PerformUpdateRangeAsync().ConfigureAwait(false);
			}
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

		/// <summary>
		///     Creates an <see cref="Expression" /> in the form of <c>x => x.ID.Equals(id)</c> for the given ID value.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected override Expression<Func<TAggregateRoot, bool>> CreatePrimaryKeyPredicate(TKey id)
		{
			if(typeof(TKey).IsStronglyTypedId())
			{
				Expression<Func<TAggregateRoot, bool>> predicate = x => x.ID.Equals(id);
				return predicate;
			}

			return base.CreatePrimaryKeyPredicate(id);
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
				TKey pkValue = this.Store.Keys.LastOrDefault();

				int nextInt = Convert.ToInt32(pkValue) + 1;
				return (TKey)Convert.ChangeType(nextInt, keyType);
			}

			if(keyType == typeof(long))
			{
				TKey pkValue = this.Store.Keys.LastOrDefault();

				int nextInt = Convert.ToInt32(pkValue) + 1;
				return (TKey)Convert.ChangeType(nextInt, keyType);
			}

			if(keyType.IsStronglyTypedId())
			{
				Type valueType = keyType.GetStronglyTypedIdValueType();
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
				IStronglyTypedId<TKey, int> pkValue = this.Store.LastOrDefault().Key as IStronglyTypedId<TKey, int>;

				int nextInt = (pkValue?.Value ?? 0) + 1;
				return nextInt;
			}

			if(keyType == typeof(long))
			{
				IStronglyTypedId<TKey, long> pkValue = this.Store.LastOrDefault().Key as IStronglyTypedId<TKey, long>;

				long nextInt = (pkValue?.Value ?? 0) + 1;
				return nextInt;
			}

			throw new InvalidOperationException("A key could not be generated. The in-memory repository only supports guid, string, int and long for keys.");
		}
	}
}
