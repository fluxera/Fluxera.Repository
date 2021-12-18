namespace Fluxera.Repository.Query{	using System.Linq;	using Fluxera.Utilities.Extensions;	internal sealed class SkipTakeOptions<T> : ISkipTakeOptions<T>		where T : class	{		private int? skip;		private int? take;		public SkipTakeOptions(int? skip = null, int? take = null)		{			this.skip = skip;			this.take = take;		}		/// <inheritdoc />		public int? SkipNumber => this.skip;		/// <inheritdoc />		public int? TakeNumber => this.take;		/// <inheritdoc />		public ISkipTakeOptions<T> Skip(int skipNumber)		{			this.skip = skipNumber;			return this;		}		/// <inheritdoc />		public ISkipTakeOptions<T> Take(int takeNumber)		{			this.take = takeNumber;			return this;		}		/// <inheritdoc />		public IQueryable<T> ApplyTo(IQueryable<T> queryable)		{			if(this.skip is > 0)			{				queryable = queryable.Skip(this.skip.Value);			}			if(this.take is > 0)			{				queryable = queryable.Take(this.take.Value);			}			return queryable;		}		/// <inheritdoc />		public bool TryGetPagingOptions(out IPagingOptions<T>? pagingOptions)		{			pagingOptions = null;			return false;		}		/// <inheritdoc />		public bool TryGetSkipTakeOptions(out ISkipTakeOptions<T>? skipTakeOptions)		{			skipTakeOptions = this;			return true;		}		/// <inheritdoc />		public bool TryGetSortingOptions(out ISortingOptions<T>? sortingOptions)		{			sortingOptions = null;			return false;		}		/// <summary>		///     Used in compiling a unique key for a query.		/// </summary>		/// <returns>Unique key for a query.</returns>		public override string ToString()		{			string skipString = this.skip.HasValue ? this.skip.ToString() : "none";			string takeString = this.take.HasValue ? this.take.ToString() : "none";			return "(Skip: {0}, Take: {1})".FormatInvariantWith(skipString, takeString);		}	}}