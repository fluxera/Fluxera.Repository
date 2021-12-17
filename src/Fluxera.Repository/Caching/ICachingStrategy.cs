namespace Fluxera.Repository.Caching
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface ICachingStrategy<TAggregateRoot, in TKey> where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		Task AddAsync(TAggregateRoot item);

		Task AddAsync(IEnumerable<TAggregateRoot> items);

		Task UpdateAsync(TAggregateRoot item);

		Task UpdateAsync(IEnumerable<TAggregateRoot> items);

		Task RemoveAsync(TKey id);

		Task RemoveAsync(IEnumerable<TKey> ids);

		Task<TAggregateRoot> GetAsync(TKey id, Func<Task<TAggregateRoot>> setter);

		Task<TResult> GetAsync<TResult>(TKey id, Expression<Func<TAggregateRoot, TResult>> selector, Func<Task<TResult>> setter);

		Task<long> CountAsync(Func<Task<long>> setter);

		Task<long> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, Func<Task<long>> setter);

		Task<TAggregateRoot> FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, Func<Task<TAggregateRoot>> setter);

		Task<TResult> FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, Func<Task<TResult>> setter);

		Task<IReadOnlyCollection<TAggregateRoot>> FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, Func<Task<IReadOnlyCollection<TAggregateRoot>>> setter);

		Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, Func<Task<IReadOnlyCollection<TResult>>> setter);

		Task<bool> ExistsAsync(TKey id, Func<Task<bool>> setter);

		Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, Func<Task<bool>> setter);
	}
}
