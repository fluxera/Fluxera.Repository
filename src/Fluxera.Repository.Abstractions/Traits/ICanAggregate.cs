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
	///     Based on the Interface Segregation Principle (ISP), the <see cref="ICanAggregate{TEntity,TKey}" /> interface
	///     exposes only the "Aggregate" methods of the repository.
	/// </summary>
	/// <remarks>
	///     <see href="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/" />
	/// </remarks>
	/// <typeparam name="TEntity">Generic repository aggregate root type.</typeparam>
	/// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public interface ICanAggregate<TEntity, in TKey> : IProvideRepositoryName<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		/// <summary>
		///     Forms the sum of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<int> SumAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<int> SumAsync(Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<long> SumAsync(Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<long> SumAsync(Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<decimal> SumAsync(Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<float> SumAsync(Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<float> SumAsync(Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> SumAsync(Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> SumAsync(Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<int> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, int>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<int> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, int?>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<long> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, long>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<long> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, long?>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<decimal> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<decimal> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<float> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, float>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<float> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, float?>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, double>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, double?>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<int> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, int>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<int> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, int?>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<long> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, long>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<long> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, long?>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<decimal> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<decimal> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<float> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, float>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<float> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, float?>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, double>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the sum of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> SumAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, double?>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> AverageAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> AverageAsync(Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> AverageAsync(Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> AverageAsync(Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<decimal> AverageAsync(Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<decimal> AverageAsync(Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<float> AverageAsync(Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<float> AverageAsync(Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> AverageAsync(Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all available items.
		/// </summary>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> AverageAsync(Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, int>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, int?>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, long>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, long?>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<decimal> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<decimal> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<float> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, float>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<float> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, float?>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, double>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> AverageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, double?>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, int>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, int?>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, long>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, long?>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<decimal> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<decimal> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<float> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, float>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<float> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, float?>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, double>> selector,
			CancellationToken cancellationToken = default);

		/// <summary>
		///     Forms the average of the selected value for all items that satisfy the specification.
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="selector"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<double> AverageAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, double?>> selector,
			CancellationToken cancellationToken = default);

		//Task<TResult> MinAsync<TResult>(Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken = default);
		//Task<TResult> MinAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken = default);

		//Task<TResult> MaxAsync<TResult>(Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken = default);
		//Task<TResult> MaxAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all available items.
		///// </summary>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<double> AverageAsync(Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all available items.
		///// </summary>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<double> AverageAsync(Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all available items.
		///// </summary>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<double> AverageAsync(Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all available items.
		///// </summary>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<double> AverageAsync(Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all available items.
		///// </summary>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<decimal> AverageAsync(Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all available items.
		///// </summary>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<decimal> AverageAsync(Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all available items.
		///// </summary>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<float> AverageAsync(Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all available items.
		///// </summary>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<float> AverageAsync(Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all available items.
		///// </summary>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<double> AverageAsync(Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all available items.
		///// </summary>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<double> AverageAsync(Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the predicate.
		///// </summary>
		///// <param name="predicate"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<double> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, int>> selector,
		//	CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the predicate.
		///// </summary>
		///// <param name="predicate"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<double> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, int?>> selector,
		//	CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the predicate.
		///// </summary>
		///// <param name="predicate"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<double> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, long>> selector,
		//	CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the predicate.
		///// </summary>
		///// <param name="predicate"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<double> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, long?>> selector,
		//	CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the predicate.
		///// </summary>
		///// <param name="predicate"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<decimal> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, decimal>> selector,
		//	CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the predicate.
		///// </summary>
		///// <param name="predicate"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<decimal> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, decimal?>> selector,
		//	CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the predicate.
		///// </summary>
		///// <param name="predicate"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<float> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, float>> selector,
		//	CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the predicate.
		///// </summary>
		///// <param name="predicate"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<float> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, float?>> selector,
		//	CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the predicate.
		///// </summary>
		///// <param name="predicate"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<double> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, double>> selector,
		//	CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the predicate.
		///// </summary>
		///// <param name="predicate"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<double> AverageAsync(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, double?>> selector,
		//	CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the specification.
		///// </summary>
		///// <param name="specification"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int>> selector,
		//	CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the specification.
		///// </summary>
		///// <param name="specification"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int?>> selector,
		//	CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the specification.
		///// </summary>
		///// <param name="specification"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long>> selector,
		//	CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the specification.
		///// </summary>
		///// <param name="specification"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long?>> selector,
		//	CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the specification.
		///// </summary>
		///// <param name="specification"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<decimal> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal>> selector,
		//	CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the specification.
		///// </summary>
		///// <param name="specification"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<decimal> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal?>> selector,
		//	CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the specification.
		///// </summary>
		///// <param name="specification"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<float> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float>> selector,
		//	CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the specification.
		///// </summary>
		///// <param name="specification"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<float> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float?>> selector,
		//	CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the specification.
		///// </summary>
		///// <param name="specification"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double>> selector,
		//	CancellationToken cancellationToken = default);

		///// <summary>
		/////     Forms the average of the selected value for all items that satisfy the specification.
		///// </summary>
		///// <param name="specification"></param>
		///// <param name="selector"></param>
		///// <param name="cancellationToken"></param>
		///// <returns></returns>
		//Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double?>> selector,
		//	CancellationToken cancellationToken = default);
	}
}
