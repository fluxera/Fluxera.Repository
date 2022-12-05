namespace Fluxera.Repository.Query
{
	using System.Linq;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for skip/take options configuration.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public interface ISkipTakeOptions<T> where T : class
	{
		/// <summary>
		///     Configures the amount of items to skip.
		/// </summary>
		/// <param name="skipAmount"></param>
		/// <returns></returns>
		ISkipTakeOptions<T> Skip(int skipAmount);

		/// <summary>
		///     Configures the amount of items to take.
		/// </summary>
		/// <param name="takeAmount"></param>
		/// <returns></returns>
		ISkipTakeOptions<T> Take(int takeAmount);

		/// <summary>
		///     Builds a query options instance from this options.
		/// </summary>
		/// <returns></returns>
		IQueryOptions<T> Build();

		/// <summary>
		///     Applies the query options to the given <see cref="IQueryable" />.
		/// </summary>
		/// <param name="queryable"></param>
		/// <returns></returns>
		internal IQueryable<T> ApplyTo(IQueryable<T> queryable);
	}
}
