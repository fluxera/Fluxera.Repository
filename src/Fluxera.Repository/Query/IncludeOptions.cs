namespace Fluxera.Repository.Query
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;

	internal sealed class IncludeOptions<T> : IIncludeOptions<T> where T : class
	{
		private readonly QueryOptionsImpl<T> queryOptions;

		private readonly IIncludeApplier includeApplier;

		private readonly IList<Expression<Func<T, object>>> includeExpressions = new List<Expression<Func<T, object>>>();

		private Func<IQueryable<T>, IQueryable<T>> applyAdditionalQueryable;

		public IncludeOptions(QueryOptionsImpl<T> queryOptions, Expression<Func<T, object>> includeExpression, IIncludeApplier includeApplier)
		{
			this.queryOptions = queryOptions;
			this.includeApplier = includeApplier;

			this.includeExpressions.Add(includeExpression);
		}

		/// <inheritdoc />
		public IIncludeOptions<T> Include(Expression<Func<T, object>> includeExpression)
		{
			this.includeExpressions.Add(includeExpression);

			return this;
		}

		/// <inheritdoc />
		public ISortingOptions<T> OrderBy<TValue>(Expression<Func<T, TValue>> sortExpression)
		{
			this.queryOptions.SortingOptions ??= new SortingOptions<T>(this.queryOptions, new SortExpression<T,TValue>(sortExpression, false));
			return this.queryOptions.SortingOptions;
		}

		/// <inheritdoc />
		public ISortingOptions<T> OrderByDescending<TValue>(Expression<Func<T, TValue>> sortExpression)
		{
			this.queryOptions.SortingOptions ??= new SortingOptions<T>(this.queryOptions, new SortExpression<T, TValue>(sortExpression, true));
			return this.queryOptions.SortingOptions;
		}

		/// <inheritdoc />
		public ISkipTakeOptions<T> Skip(int skip)
		{
			this.queryOptions.SkipTakeOptions ??= new SkipTakeOptions<T>(this.queryOptions, skip);
			return this.queryOptions.SkipTakeOptions;
		}

		/// <inheritdoc />
		public ISkipTakeOptions<T> Take(int take)
		{
			this.queryOptions.SkipTakeOptions ??= new SkipTakeOptions<T>(this.queryOptions, take: take);
			return this.queryOptions.SkipTakeOptions;
		}

		/// <inheritdoc />
		public ISkipTakeOptions<T> SkipTake(int skip, int take)
		{
			this.queryOptions.SkipTakeOptions ??= new SkipTakeOptions<T>(this.queryOptions, skip, take);
			return this.queryOptions.SkipTakeOptions;
		}

		/// <inheritdoc />
		public IPagingOptions<T> Paging(int pageNumber, int pageSize)
		{
			this.queryOptions.PagingOptions ??= new PagingOptions<T>(this.queryOptions, pageNumber, pageSize);
			return this.queryOptions.PagingOptions;
		}

		/// <inheritdoc />
		public IPagingOptions<T> Paging()
		{
			this.queryOptions.PagingOptions ??= new PagingOptions<T>(this.queryOptions);
			return this.queryOptions.PagingOptions;
		}

		/// <inheritdoc />
		public IQueryOptions<T> Build(Func<IQueryable<T>, IQueryable<T>> applyFunc)
		{
			this.applyAdditionalQueryable = applyFunc;

			return this.queryOptions;
		}

		/// <inheritdoc />
		IQueryable<T> IIncludeOptions<T>.ApplyTo(IQueryable<T> queryable)
		{
			queryable = this.includeApplier?.ApplyTo(queryable, this.includeExpressions) ?? queryable;

			queryable = this.applyAdditionalQueryable?.Invoke(queryable) ?? queryable;

			return queryable;
		}
	}
}
