namespace Fluxera.Repository.OData
{
	internal sealed class ODataPersistenceSettings
	{
		/// <summary>
		///     The base URL of the data feed.
		/// </summary>
		public string ServiceRoot { get; set; }

		/// <summary>
		///     Flag, if the repository should use batching for bulk inserts and updates.
		/// </summary>
		public bool UseBatching { get; set; }
	}
}
