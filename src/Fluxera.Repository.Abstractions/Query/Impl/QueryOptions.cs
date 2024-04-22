namespace Fluxera.Repository.Query.Impl
{
	using JetBrains.Annotations;

	/// <summary>
	///     A static helper class to create entry points for building <see cref="IQueryOptions{T}" />.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public static class QueryOptions<T> where T : class
	{
		/// <summary>
		///     Creates an empty options instance.
		/// </summary>
		/// <returns></returns>
		public static IQueryOptions<T> Empty()
		{
			return new EmptyQueryOptions<T>();
		}
	}
}
