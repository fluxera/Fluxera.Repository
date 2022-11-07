namespace Fluxera.Repository
{
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a factory that creates <see cref="IUnitOfWork" /> instances for a repository.
	/// </summary>
	[PublicAPI]
	public interface IUnitOfWorkFactory
	{
		/// <summary>
		///     Creates a <see cref="IUnitOfWork" /> for the given repository.
		/// </summary>
		/// <param name="repositoryName"></param>
		/// <returns></returns>
		IUnitOfWork CreateFor(RepositoryName repositoryName);
	}
}
