namespace Fluxera.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using JetBrains.Annotations;

	[PublicAPI]
	public abstract class LinqRepositoryBase<TAggregateRoot, TKey> : RepositoryBase<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		protected abstract IQueryable<TAggregateRoot> Queryable { get; }

		/// <inheritdoc />
		protected sealed override async Task<TAggregateRoot> FindOneAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			IQueryable<TAggregateRoot> queryable = this.Queryable
				.Apply(specification)
				.Apply(queryOptions);

			return await this.FirstOrDefaultAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<TResult> FindOneAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			IQueryable<TResult> queryable = this.Queryable
				.Apply(specification)
				.Apply(queryOptions)
				.Select(selector);

			return await this.FirstOrDefaultAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<IReadOnlyCollection<TAggregateRoot>> FindManyAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			IQueryable<TAggregateRoot> queryable = this.Queryable
				.Apply(specification)
				.Apply(queryOptions);

			return await this.ToListAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			IQueryable<TResult> queryable = this.Queryable
				.Apply(specification)
				.Apply(queryOptions)
				.Select(selector);

			return await this.ToListAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<long> LongCountAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			IQueryable<TAggregateRoot> queryable = this.Queryable
				.Apply(specification);

			return await this.LongCountAsync(queryable, cancellationToken);
		}

		protected abstract Task<TAggregateRoot> FirstOrDefaultAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken);

		protected abstract Task<TResult> FirstOrDefaultAsync<TResult>(IQueryable<TResult> queryable, CancellationToken cancellationToken);

		protected abstract Task<IReadOnlyCollection<TAggregateRoot>> ToListAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken);

		protected abstract Task<IReadOnlyCollection<TResult>> ToListAsync<TResult>(IQueryable<TResult> queryable, CancellationToken cancellationToken);

		protected abstract Task<long> LongCountAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken);
	}
}
