// ReSharper disable StaticMemberInGenericType

namespace Fluxera.Repository.Decorators
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.CompilerServices;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Options;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using Fluxera.Repository.Traits;
	using Fluxera.Utilities.Extensions;

	/// <summary>
	///     A repository decorator that provides events for diagnostic traces using System.Diagnostics.
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	public sealed class DiagnosticRepositoryDecorator<TEntity, TKey> : IRepository<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		private static readonly AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
		private static readonly string activitySourceName = assemblyName.Name;
		private static readonly Version version = assemblyName.Version;

		private readonly IRepository<TEntity, TKey> innerRepository;
		private readonly RepositoryOptions repositoryOptions;

		/// <summary>
		///     Creates a new instance of the <see cref="DiagnosticRepositoryDecorator{TAggregateRoot,TKey}" /> type.
		/// </summary>
		/// <param name="innerRepository"></param>
		/// <param name="repositoryRegistry"></param>
		public DiagnosticRepositoryDecorator(
			IRepository<TEntity, TKey> innerRepository,
			IRepositoryRegistry repositoryRegistry)
		{
			this.innerRepository = Guard.Against.Null(innerRepository);
			Guard.Against.Null(repositoryRegistry);

			RepositoryName repositoryName = repositoryRegistry.GetRepositoryNameFor<TEntity>();
			this.repositoryOptions = repositoryRegistry.GetRepositoryOptionsFor(repositoryName);
		}

		private static ActivitySource ActivitySource { get; } = new ActivitySource(activitySourceName, version.ToString());

		private static string ActivityName { get; } = $"{activitySourceName}.Diagnostic";

		private static string AggregateRootName { get; } = typeof(TEntity).Name;

		/// <inheritdoc />
		async Task ICanAdd<TEntity, TKey>.AddAsync(TEntity item, CancellationToken cancellationToken)
		{
			await this.RunDiagnosticAsync(async () => await this.innerRepository.AddAsync(item, cancellationToken));
		}

		/// <inheritdoc />
		async Task ICanAdd<TEntity, TKey>.AddRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			await this.RunDiagnosticAsync(async () => await this.innerRepository.AddRangeAsync(items, cancellationToken));
		}

		/// <inheritdoc />
		async Task ICanUpdate<TEntity, TKey>.UpdateAsync(TEntity item, CancellationToken cancellationToken)
		{
			await this.RunDiagnosticAsync(async () => await this.innerRepository.UpdateAsync(item, cancellationToken));
		}

		/// <inheritdoc />
		async Task ICanUpdate<TEntity, TKey>.UpdateRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			await this.RunDiagnosticAsync(async () => await this.innerRepository.UpdateRangeAsync(items, cancellationToken));
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveAsync(TEntity item, CancellationToken cancellationToken)
		{
			await this.RunDiagnosticAsync(async () => await this.innerRepository.RemoveAsync(item, cancellationToken));
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			await this.RunDiagnosticAsync(async () => await this.innerRepository.RemoveAsync(id, cancellationToken));
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			await this.RunDiagnosticAsync(async () => await this.innerRepository.RemoveRangeAsync(predicate, cancellationToken));
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			await this.RunDiagnosticAsync(async () => await this.innerRepository.RemoveRangeAsync(specification, cancellationToken));
		}

		/// <inheritdoc />
		async Task ICanRemove<TEntity, TKey>.RemoveRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			await this.RunDiagnosticAsync(async () => await this.innerRepository.RemoveRangeAsync(items, cancellationToken));
		}

		/// <inheritdoc />
		async Task<TEntity> ICanGet<TEntity, TKey>.GetAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.GetAsync(id, cancellationToken));
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TEntity, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TEntity, TResult>> selector,
			CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.GetAsync(id, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TEntity, TKey>.ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.ExistsAsync(id, cancellationToken));
		}

		/// <inheritdoc />
		async Task<TEntity> ICanFind<TEntity, TKey>.FindOneAsync(Expression<Func<TEntity, bool>> predicate,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.FindOneAsync(predicate, queryOptions, cancellationToken));
		}

		/// <inheritdoc />
		async Task<TEntity> ICanFind<TEntity, TKey>.FindOneAsync(ISpecification<TEntity> specification,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.FindOneAsync(specification, queryOptions, cancellationToken));
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TEntity, TKey>.FindOneAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.FindOneAsync(predicate, selector, queryOptions, cancellationToken));
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TEntity, TKey>.FindOneAsync<TResult>(ISpecification<TEntity> specification,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.FindOneAsync(specification, selector, queryOptions, cancellationToken));
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TEntity, TKey>.ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.ExistsAsync(predicate, cancellationToken));
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TEntity, TKey>.ExistsAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.ExistsAsync(specification, cancellationToken));
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TEntity>> ICanFind<TEntity, TKey>.FindManyAsync(Expression<Func<TEntity, bool>> predicate,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.FindManyAsync(predicate, queryOptions, cancellationToken));
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TEntity>> ICanFind<TEntity, TKey>.FindManyAsync(ISpecification<TEntity> specification,
			IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.FindManyAsync(specification, queryOptions, cancellationToken));
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TEntity, TKey>.FindManyAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.FindManyAsync(predicate, selector, queryOptions, cancellationToken));
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TEntity, TKey>.FindManyAsync<TResult>(ISpecification<TEntity> specification,
			Expression<Func<TEntity, TResult>> selector, IQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () =>
				await this.innerRepository.FindManyAsync(specification, selector, queryOptions, cancellationToken));
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.CountAsync(cancellationToken));
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.CountAsync(predicate, cancellationToken));
		}

		/// <inheritdoc />
		async Task<long> ICanGet<TEntity, TKey>.CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.CountAsync(specification, cancellationToken));
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<int> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.SumAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.SumAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, int?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, long>> selector,
			CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, long?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, decimal>> selector,
			CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, decimal?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, float>> selector,
			CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, float?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, double>> selector,
			CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, double?>> selector,
			CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(Expression<Func<TEntity, bool>> predicate,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(predicate, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<decimal> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<float> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<double> ICanAggregate<TEntity, TKey>.AverageAsync(ISpecification<TEntity> specification,
			Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.AverageAsync(specification, selector, cancellationToken));
		}

		/// <inheritdoc />
		void IDisposable.Dispose()
		{
			if(!this.innerRepository.IsDisposed)
			{
				this.innerRepository.Dispose();
			}
		}

		/// <inheritdoc />
		bool IDisposableRepository.IsDisposed => this.innerRepository.IsDisposed;

		/// <inheritdoc />
		async ValueTask IAsyncDisposable.DisposeAsync()
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

		private async Task<TResult> RunDiagnosticAsync<TResult>(Func<Task<TResult>> func, [CallerMemberName] string callerMemberName = "")
		{
			string commandName = callerMemberName.RemovePostFix("Async") ?? callerMemberName;
			string storageName = this.ToString().RemovePreFix("Fluxera.Repository.") ?? this.ToString();

			Activity activity = StartActivity(commandName, storageName, this.repositoryOptions);
			try
			{
				TResult result = await func.Invoke();
				HandleSuccess(activity);
				return result;
			}
			catch(Exception ex)
			{
				HandleFailure(activity, ex);
				throw;
			}
			finally
			{
				StopActivity(activity);
			}
		}

		private async Task RunDiagnosticAsync(Func<Task> func, [CallerMemberName] string callerMemberName = "")
		{
			string commandName = callerMemberName.RemovePostFix("Async") ?? callerMemberName;
			string storageName = this.ToString().RemovePreFix("Fluxera.Repository.") ?? this.ToString();

			Activity activity = StartActivity(commandName, storageName, this.repositoryOptions);
			try
			{
				await func.Invoke();
				HandleSuccess(activity);
			}
			catch(Exception ex)
			{
				HandleFailure(activity, ex);
				throw;
			}
			finally
			{
				StopActivity(activity);
			}
		}

		private static Activity StartActivity(string commandName, string storageName, RepositoryOptions repositoryOptions)
		{
			Activity activity = ActivitySource.StartActivity(ActivityName);
			if(activity is not null)
			{
				activity.DisplayName = $"{commandName} for {AggregateRootName}";

				activity.AddTag("db.system", "repository");
				activity.AddTag("db.name", repositoryOptions.RepositoryName);
				activity.AddTag("db.operation", commandName);

				activity.AddTag("db.repository.storage", storageName);
				activity.AddTag("db.repository.name", repositoryOptions.RepositoryName);
				activity.AddTag("db.repository.operation", commandName);
				activity.AddTag("db.repository.aggregate", AggregateRootName);

				activity.AddTag("db.repository.options.validation.enabled", repositoryOptions.ValidationOptions.IsEnabled);
				activity.AddTag("db.repository.options.domainevents.enabled", repositoryOptions.DomainEventsOptions.IsEnabled);
				activity.AddTag("db.repository.options.caching.enabled", repositoryOptions.CachingOptions.IsEnabled);
				activity.AddTag("db.repository.options.interception.enabled", repositoryOptions.InterceptionOptions.IsEnabled);
				activity.AddTag("db.repository.options.unitofwork.enabled", repositoryOptions.IsUnitOfWorkEnabled);
				activity.AddTag("db.repository.options.context", repositoryOptions.RepositoryContextType.FullName);
			}

			return activity;
		}

		private static void StopActivity(Activity activity)
		{
			activity?.Stop();
		}

		private static void HandleSuccess(Activity activity)
		{
			activity?.AddTag("otel.status_code", "Ok");
		}

		private static void HandleFailure(Activity activity, Exception exception)
		{
			if(activity?.IsAllDataRequested == true)
			{
				activity.AddTag("otel.status_code", "Error");
				activity.AddTag("otel.status_description", exception.Message);
				activity.AddTag("error.type", exception.GetType().FullName);
				activity.AddTag("error.msg", exception.Message);
				activity.AddTag("error.stack", exception.StackTrace);
			}
		}
	}
}
