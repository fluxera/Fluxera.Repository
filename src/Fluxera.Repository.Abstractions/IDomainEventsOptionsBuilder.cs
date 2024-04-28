namespace Fluxera.Repository
{
	using Fluxera.Repository.DomainEvents;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a domain events options builder.
	/// </summary>
	[PublicAPI]
	public interface IDomainEventsOptionsBuilder
	{
		/// <summary>
		///     Adds a domain events reducer.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		IDomainEventsOptionsBuilder AddDomainEventsReducer<T>() where T : class, IDomainEventsReducer;
	}
}
