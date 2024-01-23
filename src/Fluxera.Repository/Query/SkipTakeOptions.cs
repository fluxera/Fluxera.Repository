namespace Fluxera.Repository.Query
{
	using System;
	using System.Linq;
	using Fluxera.Utilities.Extensions;

	internal sealed class SkipTakeOptions<T> : ISkipTakeOptions<T>
		where T : class
	{
		private readonly QueryOptionsImpl<T> queryOptions;

		private int? skip;
		private int? take;

		private Func<IQueryable<T>, IQueryable<T>> applyAdditionalQueryable;

		public SkipTakeOptions(QueryOptionsImpl<T> queryOptions, int? skip = null, int? take = null)
		{
			this.queryOptions = queryOptions;
			this.skip = skip;
			this.take = take;
		}

		public int? SkipAmount => this.skip;

		public int? TakeAmount => this.take;

		/// <inheritdoc />
		public ISkipTakeOptions<T> Skip(int skipAmount)
		{
			this.skip = skipAmount;

			return this;
		}

		/// <inheritdoc />
		public ISkipTakeOptions<T> Take(int takeAmount)
		{
			this.take = takeAmount;

			return this;
		}

		/// <inheritdoc />
		public IQueryOptions<T> Build(Func<IQueryable<T>, IQueryable<T>> applyFunc)
		{
			this.applyAdditionalQueryable = applyFunc;

			return this.queryOptions;
		}

		/// <inheritdoc />
		IQueryable<T> ISkipTakeOptions<T>.ApplyTo(IQueryable<T> queryable)
		{
			if(this.skip is > 0)
			{
				queryable = queryable.Skip(this.skip.Value);
			}

			if(this.take is > 0)
			{
				queryable = queryable.Take(this.take.Value);
			}

			queryable = this.applyAdditionalQueryable?.Invoke(queryable) ?? queryable;

			return queryable;
		}

		/// <summary>
		///     Used in compiling a unique key for a query.
		/// </summary>
		/// <returns>Unique key for a query.</returns>
		public override string ToString()
		{
			string skipString = this.skip.HasValue ? this.skip.ToString() : "none";
			string takeString = this.take.HasValue ? this.take.ToString() : "none";

			return "(Skip: {0}, Take: {1})".FormatInvariantWith(skipString, takeString);
		}
	}
}
