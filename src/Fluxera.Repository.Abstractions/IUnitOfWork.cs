namespace Fluxera.Repository
{
	using System.Threading;
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for implementing store-specific unit-of-work implementations.
	/// </summary>
	[PublicAPI]
	public interface IUnitOfWork
	{
		/// <summary>
		///     Commits changes to the underlying store.
		/// </summary>
		/// <returns></returns>
		Task SaveChangesAsync(CancellationToken cancellationToken = default);

		/// <summary>
		///     Discards all changes.
		/// </summary>
		void DiscardChanges();

		/// <summary>
		///     Initializes the unit-of-work for the given repository name.
		/// </summary>
		/// <param name="repositoryName"></param>
		protected internal void Initialize(RepositoryName repositoryName);
	}
}
