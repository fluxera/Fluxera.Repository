namespace Fluxera.Repository.InMemory
{
	using System.Collections.Concurrent;

	internal sealed class InMemoryStorage<TKey, TAggregateRoot>
	{
		private readonly ConcurrentDictionary<string, ConcurrentDictionary<TKey, TAggregateRoot>> stores
			= new ConcurrentDictionary<string, ConcurrentDictionary<TKey, TAggregateRoot>>();

		public ConcurrentDictionary<TKey, TAggregateRoot> GetStore(string databaseName)
		{
			return this.stores.GetOrAdd(databaseName,
				_ => new ConcurrentDictionary<TKey, TAggregateRoot>());
		}
	}
}
