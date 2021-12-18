namespace Fluxera.Repository.Query
{
	using System.Linq;
	using JetBrains.Annotations;

	[PublicAPI]
	public static class QueryOptions
	{
		public static IQueryOptions<T> CreateFor<T>() where T : class
		{
			return new QueryOptions<T>();
		}
	}

	internal sealed class QueryOptions<T> : IQueryOptions<T>
		where T : class
	{
		//private IPagingOptions<T>? PagingOptions { get; set; }

		//private ISkipTakeOptions<T>? SkipTakeOptions { get; set; }

		//private IOrderByOptions<T>? OrderByOptions { get; set; }

		//public IOrderByOptions<T> Paging(int pageNumber, int pageSize)
		//{
		//	this.PagingOptions = new PagingOptions<T>(pageNumber, pageSize);

		//	return this.OrderByOptions ??= new OrderByOptions<T>();
		//}

		//public IOrderByOptions<T> SkipTake(int skipAmount, int takeAmount)
		//{
		//	this.SkipTakeOptions ??= new SkipTakeOptions<T>(skipAmount, takeAmount);
		//	return this.OrderByOptions ??= new OrderByOptions<T>();
		//}

		//public IOrderByOptions<T> Skip(int skipAmount)
		//{
		//	this.SkipTakeOptions ??= new SkipTakeOptions<T>(skipAmount);
		//	return this.OrderByOptions ??= new OrderByOptions<T>();
		//}

		//public IOrderByOptions<T> Take(int takeAmount)
		//{
		//	this.SkipTakeOptions ??= new SkipTakeOptions<T>(take: takeAmount);
		//	return this.OrderByOptions ??= new OrderByOptions<T>();
		//}

		//public IThenByOptions<T> OrderBy(Expression<Func<T, object>> sortExpression)
		//{
		//	this.OrderByOptions ??= new OrderByOptions<T>();
		//	return this.OrderByOptions.OrderBy(sortExpression);
		//}

		//public IThenByOptions<T> OrderByDescending(Expression<Func<T, object>> sortExpression)
		//{
		//	this.OrderByOptions ??= new OrderByOptions<T>();
		//	return this.OrderByOptions.OrderByDescending(sortExpression);
		//}

		///// <inheritdoc />
		//public bool HasPagingOptions => this.PagingOptions != null;

		///// <inheritdoc />
		//public bool HasSkipTakeOptions => this.SkipTakeOptions != null;

		///// <inheritdoc />
		//public bool HasOrderByOptions => this.OrderByOptions != null;

		///// <inheritdoc />
		//public bool TryGetPagingOptions(out IPagingOptions<T>? pagingOptions)
		//{
		//	if(!this.HasPagingOptions)
		//	{
		//		pagingOptions = null;
		//		return false;
		//	}

		//	pagingOptions = this.PagingOptions;
		//	return true;
		//}

		///// <inheritdoc />
		//public bool TryGetSkipTakeOptions(out ISkipTakeOptions<T>? skipTakeOptions)
		//{
		//	if(!this.HasSkipTakeOptions)
		//	{
		//		skipTakeOptions = null;
		//		return false;
		//	}

		//	skipTakeOptions = this.SkipTakeOptions;
		//	return true;
		//}

		///// <inheritdoc />
		//public bool TryGetOrderByOptions(out IOrderByOptions<T>? orderByOptions)
		//{
		//	if(!this.HasOrderByOptions)
		//	{
		//		orderByOptions = null;
		//		return false;
		//	}

		//	orderByOptions = this.OrderByOptions;
		//	return true;
		//}

		/// <inheritdoc />
		public bool IsEmpty => false; //(this.PagingOptions == null) && (this.SkipTakeOptions == null) && (this.OrderByOptions == null);

		/// <inheritdoc />
		public IQueryable<T> ApplyTo(IQueryable<T> queryable)
		{
			return queryable;
		}

		///// <summary>
		/////     Used in compiling a unique key for a query.
		///// </summary>
		///// <returns>Unique key for a query.</returns>
		//public override string ToString()
		//{
		//	if(this.IsEmpty)
		//	{
		//		return "QueryOptions<{0}>.Empty".FormatInvariantWith(typeof(T).Name);
		//	}

		//	string pagingOptionsString = this.PagingOptions?.ToString() ?? "none";
		//	string skipTakeOptionsString = this.SkipTakeOptions?.ToString() ?? "none";
		//	string sortByOptionsString = this.OrderByOptions?.ToString() ?? "none";

		//	return "QueryOptions<{0}>(Paging: {1}, SkipTake: {2}, Sorting: {3})".FormatInvariantWith(
		//		typeof(T).Name, pagingOptionsString, skipTakeOptionsString, sortByOptionsString);
		//}
	}
}
