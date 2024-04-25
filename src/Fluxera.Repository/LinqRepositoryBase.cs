namespace Fluxera.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Extensions;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using JetBrains.Annotations;

	/// <summary>
	///     A base class for helping in implementing storage specific repositories for storage that
	///     support <see cref="IQueryable{T}" /> and async extension methods for it.
	/// </summary>
	/// <typeparam name="TAggregateRoot"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	[PublicAPI]
	public abstract class LinqRepositoryBase<TAggregateRoot, TKey> : RepositoryBase<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		/// <inheritdoc />
		protected LinqRepositoryBase(IRepositoryRegistry repositoryRegistry)
			: base(repositoryRegistry)
		{
		}

		/// <summary>
		///     Gets the underlying <see cref="IQueryable{T}" />
		/// </summary>
		protected abstract IQueryable<TAggregateRoot> Queryable { get; }

		/// <inheritdoc />
		protected sealed override async Task<TAggregateRoot> FindOneAsync(ISpecification<TAggregateRoot> specification,
			IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			IQueryable<TAggregateRoot> queryable = this.Queryable
				.Apply(specification)
				.Apply(queryOptions);

			return await this.FirstOrDefaultAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<TResult> FindOneAsync<TResult>(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			IQueryable<TResult> queryable = this.Queryable
				.Apply(specification)
				.Apply(queryOptions)
				.Select(selector);

			return await this.FirstOrDefaultAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<IReadOnlyCollection<TAggregateRoot>> FindManyAsync(ISpecification<TAggregateRoot> specification,
			IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			IQueryable<TAggregateRoot> queryable = this.Queryable
				.Apply(specification)
				.Apply(queryOptions);

			return await this.ToListAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
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

		/// <inheritdoc />
		protected sealed override async Task<int> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int>> selector,
			CancellationToken cancellationToken)
		{
			IQueryable<int> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.SumAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<int> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int?>> selector,
			CancellationToken cancellationToken)
		{
			IQueryable<int?> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.SumAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<long> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long>> selector,
			CancellationToken cancellationToken)
		{
			IQueryable<long> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.SumAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<long> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long?>> selector,
			CancellationToken cancellationToken)
		{
			IQueryable<long?> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.SumAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<decimal> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal>> selector,
			CancellationToken cancellationToken)
		{
			IQueryable<decimal> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.SumAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<decimal> SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			IQueryable<decimal?> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.SumAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<float> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float>> selector,
			CancellationToken cancellationToken)
		{
			IQueryable<float> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.SumAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<float> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float?>> selector,
			CancellationToken cancellationToken)
		{
			IQueryable<float?> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.SumAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<double> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double>> selector,
			CancellationToken cancellationToken)
		{
			IQueryable<double> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.SumAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<double> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double?>> selector,
			CancellationToken cancellationToken)
		{
			IQueryable<double?> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.SumAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int>> selector,
			CancellationToken cancellationToken)
		{
			IQueryable<int> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.AverageAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int?>> selector,
			CancellationToken cancellationToken)
		{
			IQueryable<int?> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.AverageAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long>> selector,
			CancellationToken cancellationToken)
		{
			IQueryable<long> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.AverageAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<double> AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			IQueryable<long?> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.AverageAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<decimal> AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			IQueryable<decimal> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.AverageAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<decimal> AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			IQueryable<decimal?> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.AverageAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<float> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float>> selector,
			CancellationToken cancellationToken)
		{
			IQueryable<float> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.AverageAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<float> AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			IQueryable<float?> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.AverageAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<double> AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			IQueryable<double> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.AverageAsync(queryable, cancellationToken);
		}

		/// <inheritdoc />
		protected sealed override async Task<double> AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			IQueryable<double?> queryable = this.Queryable
				.Apply(specification)
				.Select(selector);

			return await this.AverageAsync(queryable, cancellationToken);
		}

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a FirstOrDefaultAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<TAggregateRoot> FirstOrDefaultAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a FirstOrDefaultAsync type of extension method.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<TResult> FirstOrDefaultAsync<TResult>(IQueryable<TResult> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a ToListAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<IReadOnlyCollection<TAggregateRoot>> ToListAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a ToListAsync type of extension method.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<IReadOnlyCollection<TResult>> ToListAsync<TResult>(IQueryable<TResult> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a LongCountAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<long> LongCountAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a SumAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<int> SumAsync(IQueryable<int> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a SumAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<int> SumAsync(IQueryable<int?> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a SumAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<long> SumAsync(IQueryable<long> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a SumAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<long> SumAsync(IQueryable<long?> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a SumAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<decimal> SumAsync(IQueryable<decimal> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a SumAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<decimal> SumAsync(IQueryable<decimal?> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a SumAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<float> SumAsync(IQueryable<float> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a SumAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<float> SumAsync(IQueryable<float?> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a SumAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> SumAsync(IQueryable<double> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a SumAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> SumAsync(IQueryable<double?> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a AverageAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> AverageAsync(IQueryable<int> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a AverageAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> AverageAsync(IQueryable<int?> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a AverageAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> AverageAsync(IQueryable<long> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a AverageAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> AverageAsync(IQueryable<long?> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a AverageAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<decimal> AverageAsync(IQueryable<decimal> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a AverageAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<decimal> AverageAsync(IQueryable<decimal?> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a AverageAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<float> AverageAsync(IQueryable<float> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a AverageAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<float> AverageAsync(IQueryable<float?> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a AverageAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> AverageAsync(IQueryable<double> queryable, CancellationToken cancellationToken);

		/// <summary>
		///     Executes the <see cref="IQueryable{T}" /> using a AverageAsync type of extension method.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected abstract Task<double> AverageAsync(IQueryable<double?> queryable, CancellationToken cancellationToken);
	}
}
