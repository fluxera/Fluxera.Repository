namespace Fluxera.Repository.Query
{
	using System.Linq;
	using Fluxera.Utilities.Extensions;

	internal sealed class PagingOptions<T> : IPagingOptions<T> where T : class
	{
		private readonly QueryOptionsImpl<T> queryOptions;

		public PagingOptions(QueryOptionsImpl<T> queryOptions, int pageNumber = 1, int pageSize = 25)
		{
			this.queryOptions = queryOptions;

			this.PageNumberAmount = pageNumber;
			this.PageSizeAmount = pageSize;
		}

		/// <inheritdoc />
		public int TotalItemCount { get; private set; }

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
		public int PageNumberAmount { get; private set; }

		/// <inheritdoc />
		public int PageSizeAmount { get; private set; }

		/// <inheritdoc />
		public int SkipAmount => (this.PageNumberAmount - 1) * this.PageSizeAmount;

		/// <inheritdoc />
		public int TakeAmount => this.PageSizeAmount;

		/// <inheritdoc />
		public IQueryable<T> ApplyTo(IQueryable<T> queryable)
		{
			this.TotalItemCount = queryable.Count();

			if(this.SkipAmount > 0 || this.TakeAmount > 0)
			{
				return queryable.Skip(this.SkipAmount).Take(this.TakeAmount);
			}

			return queryable;
		}

		/// <inheritdoc />
		public bool IsEmpty()
		{
			return this.queryOptions.IsEmpty();
		}

		/// <inheritdoc />
		public bool TryGetPagingOptions(out IPagingOptions<T> options)
		{
			return this.queryOptions.TryGetPagingOptions(out options);
		}

		/// <inheritdoc />
		public bool TryGetSkipTakeOptions(out ISkipTakeOptions<T> options)
		{
			return this.queryOptions.TryGetSkipTakeOptions(out options);
		}

		/// <inheritdoc />
		public bool TryGetSortingOptions(out ISortingOptions<T> options)
		{
			return this.queryOptions.TryGetSortingOptions(out options);
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return "(PageSize: {0}, PageNumber: {1})".FormatInvariantWith(this.PageSizeAmount, this.PageNumberAmount);
		}
	}
}
