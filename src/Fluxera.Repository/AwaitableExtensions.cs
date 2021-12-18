namespace Fluxera.Repository
{
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using System.Threading.Tasks;
	using Fluxera.Guards;

	public static class AwaitableExtensions
	{
		public static async Task<IReadOnlyCollection<T>> AsReadOnly<T>(this ConfiguredTaskAwaitable<List<T>> awaitable)
		{
			Guard.Against.Default(awaitable, nameof(awaitable));

			List<T> list = await awaitable ?? new List<T>();

			return list.AsReadOnly();
		}
	}
}
