namespace Fluxera.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;

	/// <summary>
	///     A base class for specialized repository service implementations.
	/// </summary>
	/// <remarks>
	///     This class contains the read operations only.
	/// </remarks>
	/// <example>
	///     public interface IPersonRepository : ICanFind&lt;Person, Guid&gt;
	///     {
	///     }
	///     <br />
	///     public class PersonRepository : ReadOnlyRepository&lt;Person, Guid&gt;, IPersonRepository
	///     {
	///     public PersonRepository(IRepository&lt;Person, Guid&gt; innerRepository)
	///     : base(innerRepository)
	///     {
	///     }
	///     }
	/// </example>
	/// <typeparam name="TAggregateRoot"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	public abstract class ReadOnlyRepository<TAggregateRoot, TKey> : IReadOnlyRepository<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		private readonly IReadOnlyRepository<TAggregateRoot, TKey> innerRepository;

		/// <summary>
		///     Creates a new instance of the <see cref="ReadOnlyRepository{TAggregateRoot,TKey}" /> type.
		/// </summary>
		/// <param name="innerRepository"></param>
		protected ReadOnlyRepository(IReadOnlyRepository<TAggregateRoot, TKey> innerRepository)
		{
			Guard.Against.Null(innerRepository, nameof(innerRepository));

			this.innerRepository = innerRepository;
		}

		/// <inheritdoc />
		public async Task<TAggregateRoot> GetAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.innerRepository.GetAsync(id, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<TResult> GetAsync<TResult>(TKey id, Expression<Func<TAggregateRoot, TResult>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.GetAsync(id, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.innerRepository.ExistsAsync(id, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<TAggregateRoot> FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindOneAsync(predicate, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<TAggregateRoot> FindOneAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions = null,
			CancellationToken cancellationToken = default)
		{
			return await this.innerRepository.FindOneAsync(specification, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<TResult> FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindOneAsync(predicate, selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<TResult> FindOneAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector,
			IQueryOptions<TAggregateRoot> queryOptions = null, CancellationToken cancellationToken = default)
		{
			return await this.innerRepository.FindOneAsync(specification, selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.innerRepository.ExistsAsync(predicate, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<bool> ExistsAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken = default)
		{
			return await this.innerRepository.ExistsAsync(specification, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<TAggregateRoot>> FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindManyAsync(predicate, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<TAggregateRoot>> FindManyAsync(ISpecification<TAggregateRoot> specification,
			IQueryOptions<TAggregateRoot> queryOptions = null, CancellationToken cancellationToken = default)
		{
			return await this.innerRepository.FindManyAsync(specification, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindManyAsync(predicate, selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions = null,
			CancellationToken cancellationToken = default)
		{
			return await this.innerRepository.FindManyAsync(specification, selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<long> CountAsync(CancellationToken cancellationToken)
		{
			return await this.innerRepository.CountAsync(cancellationToken);
		}

		/// <inheritdoc />
		public async Task<long> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.innerRepository.CountAsync(predicate, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<long> CountAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			return await this.innerRepository.CountAsync(specification, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<int> SumAsync(Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<int> SumAsync(Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<long> SumAsync(Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<long> SumAsync(Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<decimal> SumAsync(Expression<Func<TAggregateRoot, decimal>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<decimal> SumAsync(Expression<Func<TAggregateRoot, decimal?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<float> SumAsync(Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<float> SumAsync(Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> SumAsync(Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> SumAsync(Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<int> SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<int> SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<long> SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<long> SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<decimal> SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<decimal> SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<float> SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<float> SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> SumAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<int> SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<int> SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<long> SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<long> SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<decimal> SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<decimal> SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<float> SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<float> SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> SumAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.SumAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TAggregateRoot, int?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TAggregateRoot, long>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TAggregateRoot, long?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<decimal> AverageAsync(Expression<Func<TAggregateRoot, decimal>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<decimal> AverageAsync(Expression<Func<TAggregateRoot, decimal?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<float> AverageAsync(Expression<Func<TAggregateRoot, float>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<float> AverageAsync(Expression<Func<TAggregateRoot, float?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TAggregateRoot, double>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TAggregateRoot, double?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<decimal> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<decimal> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<float> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<float> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate,
			Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(predicate, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<decimal> AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<decimal> AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<float> AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<float> AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<double> AverageAsync(ISpecification<TAggregateRoot> specification,
			Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.AverageAsync(specification, selector, cancellationToken);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			if(!this.innerRepository.IsDisposed)
			{
				this.innerRepository.Dispose();
			}
		}

		/// <inheritdoc />
		public bool IsDisposed => this.innerRepository.IsDisposed;

		/// <inheritdoc />
		public async ValueTask DisposeAsync()
		{
			if(!this.innerRepository.IsDisposed)
			{
				await this.innerRepository.DisposeAsync();
			}
		}

		/// <inheritdoc />
		public RepositoryName RepositoryName => this.innerRepository.RepositoryName;

		/// <inheritdoc />
		public override string ToString()
		{
			return this.innerRepository.ToString();
		}
	}
}
