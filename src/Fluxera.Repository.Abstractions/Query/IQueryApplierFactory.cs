namespace Fluxera.Repository.Query
{
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for a factory that creates query applier instances for a repository.
	/// </summary>
	[PublicAPI]
	public interface IQueryApplierFactory
	{
		/// <summary>
		///     Creates a <see cref="IIncludeApplier" /> for the given repository name.
		/// </summary>
		/// <param name="repositoryName"></param>
		/// <returns></returns>
		IIncludeApplier CreateIncludeApplier(string repositoryName);

		/// <summary>
		///     Creates a <see cref="IIncludeApplier" /> for the given repository name.
		/// </summary>
		/// <param name="repositoryName"></param>
		/// <returns></returns>
		IIncludeApplier CreateIncludeApplier(RepositoryName repositoryName)
		{
			return this.CreateIncludeApplier(repositoryName.Name);
		}
	}
}
