namespace Fluxera.Repository.MongoDB
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Extensions;
	using Fluxera.Repository.Options;
	using Fluxera.Repository.Specifications;
	using global::MongoDB.Driver;
	using global::MongoDB.Driver.Linq;

	internal sealed class MongoRepository<TEntity, TKey> : LinqRepositoryBase<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly IMongoCollection<TEntity> collection;
		private readonly MongoContext context;
		private readonly RepositoryOptions options;

		public MongoRepository(
			MongoContextProvider contextProvider,
			IRepositoryRegistry repositoryRegistry)
			: base(repositoryRegistry)
		{
			Guard.Against.Null(contextProvider);
			Guard.Against.Null(repositoryRegistry);

			RepositoryName repositoryName = repositoryRegistry.GetRepositoryNameFor<TEntity>();
			this.options = repositoryRegistry.GetRepositoryOptionsFor(repositoryName);

			this.context = contextProvider.GetContextFor(repositoryName);
			this.collection = this.context.GetCollection<TEntity>();
		}

		private static string Name => "Fluxera.Repository.MongoRepository";

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
			// 1. UnitOfWork deaktiviert
			// 2. UnitOfWork aktiviert.
			//    a) Server supports transactions.
			//    b) Server doesn't support transactions.

			Task PerformAddAsync()
			{
				Task result = this.context.Session is not null
					? this.collection.InsertOneAsync(this.context.Session, item, cancellationToken: cancellationToken)
					: this.collection.InsertOneAsync(item, cancellationToken: cancellationToken);

				return result.Then(cancellationToken);
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
		protected override async Task AddRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			IList<TEntity> itemsList = items.ToList();

			Task PerformAddRangeAsync()
			{
				Task result = this.context.Session is not null
					? this.collection.InsertManyAsync(this.context.Session, itemsList, cancellationToken: cancellationToken)
					: this.collection.InsertManyAsync(itemsList, cancellationToken: cancellationToken);

				return result.Then(cancellationToken);
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
				Task result = this.context.Session is not null
					? this.collection.DeleteManyAsync(this.context.Session, specification.Predicate, cancellationToken: cancellationToken)
					: this.collection.DeleteManyAsync(specification.Predicate, cancellationToken);

				return result.Then(cancellationToken);
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

			IList<WriteModel<TEntity>> deletes = new List<WriteModel<TEntity>>();
			foreach(TEntity item in itemsList)
			{
				Expression<Func<TEntity, bool>> predicate = this.CreatePrimaryKeyPredicate(item.ID);
				deletes.Add(new DeleteOneModel<TEntity>(predicate));
			}

			Task PerformRemoveRangeAsync()
			{
				Task result = this.context.Session is not null
					? this.collection.BulkWriteAsync(this.context.Session, deletes, new BulkWriteOptions { IsOrdered = false }, cancellationToken)
					: this.collection.BulkWriteAsync(deletes, new BulkWriteOptions { IsOrdered = false }, cancellationToken);

				return result.Then(cancellationToken);
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
				Task result = this.context.Session is not null
					? this.collection.ReplaceOneAsync(this.context.Session, this.CreatePrimaryKeyPredicate(item.ID), item, cancellationToken: cancellationToken)
					: this.collection.ReplaceOneAsync(this.CreatePrimaryKeyPredicate(item.ID), item, cancellationToken: cancellationToken);

				return result.Then(cancellationToken);
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
			IList<WriteModel<TEntity>> updates = new List<WriteModel<TEntity>>();
			foreach(TEntity item in items)
			{
				Expression<Func<TEntity, bool>> predicate = this.CreatePrimaryKeyPredicate(item.ID);
				updates.Add(new ReplaceOneModel<TEntity>(predicate, item));
			}

			Task PerformUpdateRangeAsync()
			{
				Task result = this.context.Session is not null
					? this.collection.BulkWriteAsync(this.context.Session, updates, new BulkWriteOptions { IsOrdered = false }, cancellationToken)
					: this.collection.BulkWriteAsync(updates, new BulkWriteOptions { IsOrdered = false }, cancellationToken);

				return result.Then(cancellationToken);
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

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<TEntity>> ToListAsync(IQueryable<TEntity> queryable,
			CancellationToken cancellationToken)
		{
			return await queryable
				.ToListAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<TResult>> ToListAsync<TResult>(IQueryable<TResult> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.ToListAsync(cancellationToken)
				.ConfigureAwait(false);
		}
	}
}
