namespace Fluxera.Repository.LiteDB
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Extensions;
	using Fluxera.Repository.Options;
	using Fluxera.Repository.Specifications;
	using Fluxera.StronglyTypedId;
	using global::LiteDB.Async;
	using global::LiteDB.Queryable;

	internal sealed class LiteRepository<TEntity, TKey> : LinqRepositoryBase<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly ILiteCollectionAsync<TEntity> collection;
		private readonly LiteContext context;
		private readonly RepositoryOptions options;
		private readonly SequentialGuidGenerator sequentialGuidGenerator;

		public LiteRepository(
			LiteContextProvider contextProvider,
			IRepositoryRegistry repositoryRegistry,
			SequentialGuidGenerator sequentialGuidGenerator)
			: base(repositoryRegistry)
		{
			Guard.Against.Null(contextProvider);
			Guard.Against.Null(repositoryRegistry);
			this.sequentialGuidGenerator = Guard.Against.Null(sequentialGuidGenerator);

			RepositoryName repositoryName = repositoryRegistry.GetRepositoryNameFor<TEntity>();
			this.options = repositoryRegistry.GetRepositoryOptionsFor(repositoryName);

			this.context = contextProvider.GetContextFor(repositoryName);
			this.collection = this.context.GetCollection<TEntity>();
		}

		private static string Name => "Fluxera.Repository.LiteRepository";

		/// <inheritdoc />
		protected override IQueryable<TEntity> Queryable => this.collection.AsQueryable();

		/// <inheritdoc />
		public override string ToString()
		{
			return Name;
		}

		/// <inheritdoc />
		protected override async Task AddAsync(TEntity item, CancellationToken cancellationToken)
		{
			Task PerformAddAsync()
			{
				item.ID = this.GenerateKey();

				return this.collection.InsertAsync(item).Then(cancellationToken);
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
		/// //
		protected override async Task AddRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			IList<TEntity> itemList = items.ToList();

			Task PerformAddRangeAsync()
			{
				foreach(TEntity item in itemList)
				{
					item.ID = this.GenerateKey();
				}

				return this.collection.InsertBulkAsync(itemList).Then(cancellationToken);
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
		protected override async Task RemoveRangeAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			Task PerformRemoveRangeAsync()
			{
				return this.collection.DeleteManyAsync(specification.Predicate).Then(cancellationToken);
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
		protected override async Task RemoveRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			IList<TEntity> itemsList = items.ToList();

			Task PerformRemoveRangeAsync()
			{
				IList<ISpecification<TEntity>> specifications = new List<ISpecification<TEntity>>();

				foreach(TEntity item in itemsList)
				{
					specifications.Add(this.CreatePrimaryKeySpecification(item.ID));
				}

				ManyOrSpecification<TEntity> specification = new ManyOrSpecification<TEntity>(specifications);
				return this.collection.DeleteManyAsync(specification.Predicate).Then(cancellationToken);
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
		protected override async Task UpdateAsync(TEntity item, CancellationToken cancellationToken)
		{
			Task PerformUpdateAsync()
			{
				return this.collection.UpdateAsync(item).Then(cancellationToken);
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
		protected override async Task UpdateRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			IList<TEntity> itemsList = items.ToList();

			Task PerformUpdateRangeAsync()
			{
				return this.collection.UpdateAsync(itemsList).Then(cancellationToken);
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
		protected override async Task<TEntity> FirstOrDefaultAsync(IQueryable<TEntity> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<TResult> FirstOrDefaultAsync<TResult>(IQueryable<TResult> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<TEntity>> ToListAsync(IQueryable<TEntity> queryable,
			CancellationToken cancellationToken)
		{
			return await queryable
				.ToListAsync(cancellationToken)
				.ConfigureAwait(false)
				.AsReadOnly();
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<TResult>> ToListAsync<TResult>(IQueryable<TResult> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.ToListAsync(cancellationToken)
				.ConfigureAwait(false)
				.AsReadOnly();
		}

		/// <inheritdoc />
		protected override async Task<long> LongCountAsync(IQueryable<TEntity> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.LongCountAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<int> SumAsync(IQueryable<int> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<int> SumAsync(IQueryable<int?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<long> SumAsync(IQueryable<long> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<long> SumAsync(IQueryable<long?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<decimal> SumAsync(IQueryable<decimal> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<decimal> SumAsync(IQueryable<decimal?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<float> SumAsync(IQueryable<float> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<float> SumAsync(IQueryable<float?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<double> SumAsync(IQueryable<double> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<double> SumAsync(IQueryable<double?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<double> AverageAsync(IQueryable<int> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<double> AverageAsync(IQueryable<int?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<double> AverageAsync(IQueryable<long> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<double> AverageAsync(IQueryable<long?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<decimal> AverageAsync(IQueryable<decimal> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<decimal> AverageAsync(IQueryable<decimal?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<float> AverageAsync(IQueryable<float> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<float> AverageAsync(IQueryable<float?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<double> AverageAsync(IQueryable<double> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<double> AverageAsync(IQueryable<double?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
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
					throw new InvalidOperationException(
						"A key could not be generated. The LiteDB repository only supports guid or string as type for strongly-typed keys.");
				}

				object instance = Activator.CreateInstance(keyType, [keyValue]);
				return (TKey)instance;
			}

			throw new InvalidOperationException("A key could not be generated. The LiteDB repository only supports guid or string as type for keys.");
		}
	}
}
