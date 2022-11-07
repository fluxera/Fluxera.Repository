namespace Fluxera.Repository.OData
{
	using System.Threading;
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class ODataUnitOfWork : IUnitOfWork
	{
		/// <inheritdoc />
		public Task SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public void DiscardChanges()
		{
		}

		/// <inheritdoc />
		void IUnitOfWork.Initialize(RepositoryName repositoryName)
		{
		}
	}
}
