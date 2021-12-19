namespace Fluxera.Repository.Query
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract to hold information about the sort expression and direction.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public interface ISortExpression<T> where T : class
	{
		/// <summary>
		///     Gets the sort expression.
		/// </summary>
		Expression<Func<T, object>> Expression { get; }

		/// <summary>
		///     Gets the sort direction.
		/// </summary>
		bool IsDescending { get; }

		/// <summary>
		///     Applies the sort expression and direction to the given <see cref="IQueryable{T}" />.
		/// </summary>
		/// <param name="queryable"></param>
		/// <returns></returns>
		IOrderedQueryable<T> ApplyTo(IQueryable<T> queryable);

		/// <summary>
		///     Applies the sort expression and direction to the given <see cref="IOrderedQueryable{T}" />.
		/// </summary>
		/// <param name="queryable"></param>
		/// <returns></returns>
		IQueryable<T> ApplyTo(IOrderedQueryable<T> queryable);
	}
}
