namespace Fluxera.Repository.Extensions
{
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using System.Threading.Tasks;

	/// <summary>
	///     Contains extension methods for <see cref="ConfiguredTaskAwaitable" /> type.
	/// </summary>
	public static class AwaitableExtensions
	{
		/// <summary>
		///     Simple helper that just awaits the list result of the <see cref="ConfiguredTaskAwaitable" />
		///     and returns the data as <see cref="IReadOnlyCollection{T}" />. This is a helper to create a fluent structure
		///     when calling several methods on a query builder of a storage.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="awaitable"></param>
		/// <returns></returns>
		public static async Task<IReadOnlyCollection<T>> AsReadOnly<T>(this ConfiguredTaskAwaitable<List<T>> awaitable)
		{
			List<T> list = await awaitable ?? new List<T>();
			return list.AsReadOnly();
		}

		/// <summary>
		///     Simple helper that just awaits the result of the <see cref="ConfiguredTaskAwaitable" />
		///     and returns the data or the default value if the result was <c>null</c>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="awaitable"></param>
		/// <returns></returns>
		public static async Task<T> GetValueOrDefault<T>(this ConfiguredTaskAwaitable<T?> awaitable)
			where T : struct
		{
			T? value = await awaitable;
			return value.GetValueOrDefault();
		}
	}
}
