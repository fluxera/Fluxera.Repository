namespace Fluxera.Repository.Query
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using Fluxera.Utilities.Extensions;

	internal sealed class QueryOptionsImpl<T> : IQueryOptions<T>, ISortingOptions<T>, ISkipTakeOptions<T>, IPagingOptions<T>
		where T : class
	{
		internal ISortingOptions<T> SortingOptions { get; set; }

		internal ISkipTakeOptions<T> SkipTakeOptions { get; set; }

		internal IPagingOptions<T> PagingOptions { get; set; }

		/// <inheritdoc />
		IPagingOptions<T> IPagingOptions<T>.PageNumber(int pageNumberAmount)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		IPagingOptions<T> IPagingOptions<T>.PageSize(int pageSizeAmount)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		IQueryOptions<T> IPagingOptions<T>.Build(Func<IQueryable<T>, IQueryable<T>> applyFunc)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		IQueryable<T> IPagingOptions<T>.ApplyTo(IQueryable<T> queryable)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		int IPagingOptions<T>.TotalItemCount => throw new NotSupportedException();

		/// <inheritdoc />
		int IPagingOptions<T>.PageNumberAmount => throw new NotSupportedException();

		/// <inheritdoc />
		int IPagingOptions<T>.PageSizeAmount => throw new NotSupportedException();

		/// <inheritdoc />
		IQueryable<T> IQueryOptions<T>.ApplyTo(IQueryable<T> queryable)
		{
			if(this.SortingOptions is not null)
			{
				queryable = this.SortingOptions.ApplyTo(queryable);
			}

			if(this.SkipTakeOptions is not null)
			{
				queryable = this.SkipTakeOptions.ApplyTo(queryable);
			}

			if(this.PagingOptions is not null)
			{
				queryable = this.PagingOptions.ApplyTo(queryable);
			}

			return queryable;
		}

		/// <inheritdoc />
		bool IQueryOptions<T>.IsEmpty => false;

		/// <inheritdoc />
		ISkipTakeOptions<T> ISkipTakeOptions<T>.Take(int takeAmount)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		int? ISkipTakeOptions<T>.SkipAmount => throw new NotSupportedException();

		/// <inheritdoc />
		int? ISkipTakeOptions<T>.TakeAmount => throw new NotSupportedException();

		/// <inheritdoc />
		ISkipTakeOptions<T> ISkipTakeOptions<T>.Skip(int skipAmount)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		IQueryOptions<T> ISkipTakeOptions<T>.Build(Func<IQueryable<T>, IQueryable<T>> applyFunc)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		IQueryable<T> ISkipTakeOptions<T>.ApplyTo(IQueryable<T> queryable)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		ISortExpression<T> ISortingOptions<T>.PrimaryExpression => throw new NotSupportedException();

		/// <inheritdoc />
		IList<ISortExpression<T>> ISortingOptions<T>.SecondaryExpressions => throw new NotSupportedException();

		/// <inheritdoc />
		ISortingOptions<T> ISortingOptions<T>.ThenBy<TValue>(Expression<Func<T, TValue>> sortExpression)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		ISortingOptions<T> ISortingOptions<T>.ThenByDescending<TValue>(Expression<Func<T, TValue>> sortExpression)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		ISkipTakeOptions<T> ISortingOptions<T>.Skip(int skip)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		ISkipTakeOptions<T> ISortingOptions<T>.Take(int take)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		ISkipTakeOptions<T> ISortingOptions<T>.SkipTake(int skip, int take)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		IPagingOptions<T> ISortingOptions<T>.Paging(int pageNumber, int pageSize)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		IPagingOptions<T> ISortingOptions<T>.Paging()
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		IQueryOptions<T> ISortingOptions<T>.Build(Func<IQueryable<T>, IQueryable<T>> applyFunc)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		IQueryable<T> ISortingOptions<T>.ApplyTo(IQueryable<T> queryable)
		{
			throw new NotSupportedException();
		}

		internal ISortingOptions<T> OrderBy<TValue>(Expression<Func<T, TValue>> sortExpression)
		{
			this.SortingOptions ??= new SortingOptions<T>(this, new SortExpression<T, TValue>(sortExpression, false));
			return this.SortingOptions;
		}

		internal ISortingOptions<T> OrderByDescending<TValue>(Expression<Func<T, TValue>> sortExpression)
		{
			this.SortingOptions ??= new SortingOptions<T>(this, new SortExpression<T, TValue>(sortExpression, true));
			return this.SortingOptions;
		}

		internal ISkipTakeOptions<T> Skip(int skipAmount)
		{
			this.SkipTakeOptions ??= new SkipTakeOptions<T>(this, skipAmount);
			return this.SkipTakeOptions;
		}

		internal ISkipTakeOptions<T> Take(int takeAmount)
		{
			this.SkipTakeOptions ??= new SkipTakeOptions<T>(this, take: takeAmount);
			return this.SkipTakeOptions;
		}

		internal ISkipTakeOptions<T> SkipTake(int skipAmount, int takeAmount)
		{
			this.SkipTakeOptions ??= new SkipTakeOptions<T>(this, skipAmount, takeAmount);
			return this.SkipTakeOptions;
		}

		internal IPagingOptions<T> Paging(int pageNumber, int pageSize)
		{
			this.PagingOptions ??= new PagingOptions<T>(this, pageNumber, pageSize);
			return this.PagingOptions;
		}

		internal IPagingOptions<T> Paging()
		{
			this.PagingOptions ??= new PagingOptions<T>(this);
			return this.PagingOptions;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			if(this.SortingOptions is not null)
			{
				return this.SortingOptions.ToString();
			}

			const string sortingOptionsString = "none";
			string pagingOptionsString = this.PagingOptions != null ? this.PagingOptions.ToString() : "none";
			string skipTakeOptionsString = this.SkipTakeOptions != null ? this.SkipTakeOptions.ToString() : "none";

			return "QueryOptions<{0}>(Sorting: {1}, Paging: {2}, SkipTake: {3})"
				.FormatInvariantWith(typeof(T).Name, sortingOptionsString, pagingOptionsString, skipTakeOptionsString);
		}
	}
}
