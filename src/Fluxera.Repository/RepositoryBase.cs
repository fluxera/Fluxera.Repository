namespace Fluxera.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using Fluxera.Repository.Traits;
	using Fluxera.Utilities;
	using JetBrains.Annotations;

	[PublicAPI]
	public abstract class RepositoryBase<TAggregateRoot, TKey> : Disposable, IRepository<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		/// <inheritdoc />
		bool IDisposableRepository.IsDisposed => base.IsDisposed;

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.AddAsync(item, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.AddRangeAsync(items, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.RemoveRangeAsync(this.CreatePrimaryKeySpecification(item.ID), cancellationToken).ConfigureAwait(false);
			item.ID = default(TKey);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			await this.RemoveRangeAsync(this.CreatePrimaryKeySpecification(id), cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			await this.RemoveRangeAsync(new Specification<TAggregateRoot>(predicate), cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			await this.RemoveRangeAsync(specification, cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.RemoveRangeAsync(items, cancellationToken);
			foreach(TAggregateRoot item in items)
			{
				item.ID = default(TKey);
			}
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.UpdateRangeAsync(items, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindOneAsync(new Specification<TAggregateRoot>(predicate), queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindOneAsync(specification, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindOneAsync(new Specification<TAggregateRoot>(predicate), selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.FindOneAsync(specification, selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions = null, CancellationToken cancellationToken = default)
		{
			return await this.FindManyAsync(new Specification<TAggregateRoot>(predicate), queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot>? queryOptions = null, CancellationToken cancellationToken = default)
		{
			return await this.FindManyAsync(specification, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions = null, CancellationToken cancellationToken = default)
		{
			return await this.FindManyAsync(new Specification<TAggregateRoot>(predicate), selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions = null, CancellationToken cancellationToken = default)
		{
			return await this.FindManyAsync(specification, selector, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot, TKey>.GetAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.FindOneAsync(this.CreatePrimaryKeySpecification(id), null, cancellationToken);
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			return await this.FindOneAsync(this.CreatePrimaryKeySpecification(id), selector, null, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(CancellationToken cancellationToken)
		{
			return await this.LongCountAsync(Specification<TAggregateRoot>.All, cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.LongCountAsync(new Specification<TAggregateRoot>(predicate), cancellationToken);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			return await this.LongCountAsync(specification, cancellationToken);
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot, TKey>.ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.LongCountAsync(this.CreatePrimaryKeySpecification(id), cancellationToken) > 0;
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot, TKey>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.LongCountAsync(new Specification<TAggregateRoot>(predicate), cancellationToken) > 0;
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot, TKey>.ExistsAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			return await this.LongCountAsync(specification, cancellationToken) > 0;
		}

		protected abstract Task AddAsync(TAggregateRoot item, CancellationToken cancellationToken);

		protected abstract Task AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken);

		protected abstract Task RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken);

		protected abstract Task RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken);

		protected abstract Task UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken);

		protected abstract Task UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken);

		protected abstract Task<TAggregateRoot> FindOneAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken);

		protected abstract Task<TResult> FindOneAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken);

		protected abstract Task<IReadOnlyCollection<TAggregateRoot>> FindManyAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken);

		protected abstract Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken);

		protected abstract Task<long> LongCountAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken);

		protected Expression<Func<TAggregateRoot, bool>> CreatePrimaryKeyPredicate(TKey id)
		{
			PropertyInfo primaryKeyProperty = this.GetPrimaryKeyProperty();

			ParameterExpression parameter = Expression.Parameter(typeof(TAggregateRoot), "x");
			Expression<Func<TAggregateRoot, bool>> predicate = Expression.Lambda<Func<TAggregateRoot, bool>>(
				Expression.Equal(
					Expression.PropertyOrField(parameter, primaryKeyProperty.Name),
					Expression.Constant(id)
				),
				parameter);

			return predicate;
		}

		protected ISpecification<TAggregateRoot> CreatePrimaryKeySpecification(TKey id)
		{
			Expression<Func<TAggregateRoot, bool>> predicate = this.CreatePrimaryKeyPredicate(id);
			ISpecification<TAggregateRoot> specification = new Specification<TAggregateRoot>(predicate);
			return specification;
		}

		private PropertyInfo GetPrimaryKeyProperty()
		{
			Type type = typeof(TAggregateRoot);
			Type keyType = typeof(TKey);

			Tuple<Type, Type> key = Tuple.Create(type, keyType);

			// Check the cache for already existing property info instance.
			if(PropertyInfoCache.PrimaryKeyDict.ContainsKey(key))
			{
				return PropertyInfoCache.PrimaryKeyDict[key];
			}

			string keyPropertyName = nameof(AggregateRoot<TAggregateRoot, TKey>.ID);
			PropertyInfo propertyInfo = type.GetTypeInfo().GetDeclaredProperty(keyPropertyName);
			while((propertyInfo == null) && (type.GetTypeInfo().BaseType != null))
			{
				type = type.GetTypeInfo().BaseType;
				propertyInfo = type.GetTypeInfo().GetDeclaredProperty(keyPropertyName);
			}

			if(propertyInfo == null)
			{
				throw new InvalidOperationException($"No property '{keyPropertyName}' found for type '{typeof(TAggregateRoot)}'.");
			}

			if(propertyInfo.PropertyType != keyType)
			{
				throw new InvalidOperationException($"No property '{keyPropertyName}' found for type '{typeof(TAggregateRoot)}' that has the type {typeof(TKey)}.");
			}

			PropertyInfoCache.PrimaryKeyDict[key] = propertyInfo;
			return propertyInfo;
		}
	}
}
