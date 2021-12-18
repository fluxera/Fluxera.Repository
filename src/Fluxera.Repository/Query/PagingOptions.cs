namespace Fluxera.Repository.Query
{
	using System.Linq;
	using Fluxera.Utilities.Extensions;

	internal sealed class PagingOptions<T> : IPagingOptions<T>
		where T : class
	{
		public PagingOptions() : this(1, 25)
		{
		}

		public PagingOptions(int pageNumber, int pageSize)
		{
			this.PageNumber = pageNumber;
			this.PageSize = pageSize;
		}

		/// <inheritdoc />
		public int TotalItemCount { get; private set; }

		/// <inheritdoc />
		public int PageNumber { get; private set; }

		/// <inheritdoc />
		public int PageSize { get; private set; }

		/// <inheritdoc />
		public int Skip => (this.PageNumber - 1) * this.PageSize;

		/// <inheritdoc />
		public int Take => this.PageSize;

		/// <inheritdoc />
		public IQueryable<T> ApplyTo(IQueryable<T> queryable)
		{
			this.TotalItemCount = queryable.Count();

			if((this.Skip > 0) || (this.Take > 0))
			{
				return queryable.Skip(this.Skip).Take(this.Take);
			}

			return queryable;
		}

		public IPagingOptions<T> Number(int pageNumber)
		{
			this.PageNumber = pageNumber;

			return this;
		}

		public IPagingOptions<T> Size(int pageSize)
		{
			this.PageSize = pageSize;

			return this;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return "(PageSize: {0}, PageNumber: {1})".FormatInvariantWith(this.PageSize, this.PageNumber);
		}
	}
}
