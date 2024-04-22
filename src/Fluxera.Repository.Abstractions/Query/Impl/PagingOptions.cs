namespace Fluxera.Repository.Query.Impl
{
	using System;
	using System.Linq;
	using Fluxera.Utilities.Extensions;

	internal sealed class PagingOptions<T> : IPagingOptions<T> where T : class
	{
		private readonly QueryOptionsImpl<T> queryOptions;

		private Func<IQueryable<T>, IQueryable<T>> applyAdditionalQueryable;

		public PagingOptions(QueryOptionsImpl<T> queryOptions, int pageNumber = 1, int pageSize = 25)
		{
			this.queryOptions = queryOptions;

			this.PageNumberAmount = pageNumber;
			this.PageSizeAmount = pageSize;
		}

		public int SkipAmount => (this.PageNumberAmount - 1) * this.PageSizeAmount;

		public int TakeAmount => this.PageSizeAmount;

		public int TotalItemCount { get; private set; }

		public int PageNumberAmount { get; private set; }

		public int PageSizeAmount { get; private set; }

		/// <inheritdoc />
		public IPagingOptions<T> PageNumber(int pageNumberAmount)
		{
			this.PageNumberAmount = pageNumberAmount;

			return this;
		}

		/// <inheritdoc />
		public IPagingOptions<T> PageSize(int pageSizeAmount)
		{
			this.PageSizeAmount = pageSizeAmount;

			return this;
		}

		/// <inheritdoc />
		public IQueryOptions<T> Build(Func<IQueryable<T>, IQueryable<T>> applyFunc)
		{
			this.applyAdditionalQueryable = applyFunc;

			return this.queryOptions;
		}

		/// <inheritdoc />
		IQueryable<T> IPagingOptions<T>.ApplyTo(IQueryable<T> queryable)
		{
			this.TotalItemCount = queryable.Count();

			if(this.SkipAmount > 0 || this.TakeAmount > 0)
			{
				queryable = queryable.Skip(this.SkipAmount);
				queryable = queryable.Take(this.TakeAmount);
			}

			queryable = this.applyAdditionalQueryable?.Invoke(queryable) ?? queryable;

			return queryable;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return "(PageSize: {0}, PageNumber: {1})".FormatInvariantWith(this.PageSizeAmount, this.PageNumberAmount);
		}
	}
}
