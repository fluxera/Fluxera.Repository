﻿namespace Fluxera.Repository.Query
{
	using System.Linq;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a paging and/or sorting criteria builder.
	/// </summary>
	/// <typeparam name="T">The type of the aggregate root.</typeparam>
	[PublicAPI]
	public interface IQueryOptions<T> where T : class
	{
		/// <summary>
		///     Checks if the instance is empty options.
		/// </summary>
		/// <returns></returns>
		bool IsEmpty { get; }

		/// <summary>
		///     Applies the query options to the given <see cref="IQueryable{T}" />.
		/// </summary>
		/// <param name="queryable"></param>
		/// <returns></returns>
		IQueryable<T> ApplyTo(IQueryable<T> queryable);
	}
}
