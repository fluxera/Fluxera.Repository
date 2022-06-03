namespace Fluxera.Repository.Traits
{
	using System;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Specifications;
	using JetBrains.Annotations;

	/// <summary>
	///     Based on the Interface Segregation Principle (ISP), the <see cref="ICanAggregate{TAggregateRoot,TKey}" /> interface
	///     exposes only the "Aggregate" methods of the repository.
	/// </summary>
	/// <remarks>
	///     <see href="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/" />
	/// </remarks>
	/// <typeparam name="TAggregateRoot">Generic repository aggregate root type.</typeparam>
	/// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public interface ICanAggregate<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		/// <summary>
		///     Gets the count of existing items of the underlying store.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The item count.</returns>
		Task<long> CountAsync(CancellationToken cancellationToken = default);

		/// <summary>
		///     Gets the count of items of the underlying store that match the given predicate.
		/// </summary>
		/// <param name="predicate">The predicate to match.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The item count.</returns>
		Task<long> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken = default);

		/// <summary>
		///     Gets the count of items of the underlying store that match the given specification.
		/// </summary>
		/// <param name="specification">The specification to match.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The item count.</returns>
		Task<long> CountAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken = default);

		//Task<int> SumAsync(Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken = default);
		//Task<int> SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken = default);
		//Task<int> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken = default);

		//Task<long> SumAsync(Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken = default);
		//Task<long> SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken = default);

		//Task<decimal> SumAsync(Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken = default);
		//Task<decimal> SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken = default);

		//Task<float> SumAsync(Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken = default);
		//Task<float> SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken = default);

		//Task<double> SumAsync(Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken = default);
		//Task<double> SumAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken = default);

		//Task<double> AverageAsync(Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken = default);
		//Task<double> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken = default);

		//Task<double> AverageAsync(Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken = default);
		//Task<double> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken = default);

		//Task<decimal> AverageAsync(Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken = default);
		//Task<decimal> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken = default);

		//Task<float> AverageAsync(Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken = default);
		//Task<float> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken = default);

		//Task<double> AverageAsync(Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken = default);
		//Task<double> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken = default);

		//TResult MinAsync<TResult>(Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken = default);
		//TResult MinAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken = default);

		//TResult MaxAsync<TResult>(Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken = default);
		//TResult MaxAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken = default);
	}
}
