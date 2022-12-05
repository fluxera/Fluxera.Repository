namespace Fluxera.Repository.Query
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a service that applies the include expressions of a
	///     query options instance to a queryable using the storage-specific API.
	/// </summary>
	[PublicAPI]
	public interface IIncludeApplier
	{
		/// <summary>
		///     Applies the include expressions to the given <see cref="IQueryable{T}" />.
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="includeExpressions"></param>
		/// <returns></returns>
		IQueryable<T> ApplyTo<T>(IQueryable<T> queryable, IEnumerable<Expression<Func<T, object>>> includeExpressions) where T : class;
	}
}
