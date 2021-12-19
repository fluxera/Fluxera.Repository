namespace Fluxera.Repository
{
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using System.Threading.Tasks;
	using Fluxera.Guards;

	/// <summary>
	///     Contains extension methods for <see cref="ConfiguredTaskAwaitable" /> type.
	/// </summary>
	public static class AwaitableExtensions
	{
		/// <summary>
		///     Simple helper that just awaits the <see cref="List{T}" /> result of the <see cref="ConfiguredTaskAwaitable" />
		///     and returns the data as <see cref="IReadOnlyCollection{T}" />. This is a helper to create a fluent structure
		///     when calling several methods on a query builder of a storage.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="awaitable"></param>
		/// <returns></returns>
		public static async Task<IReadOnlyCollection<T>> AsReadOnly<T>(this ConfiguredTaskAwaitable<List<T>> awaitable)
		{
			Guard.Against.Default(awaitable, nameof(awaitable));

			List<T> list = await awaitable ?? new List<T>();

			return list.AsReadOnly();
		}
	}
}
