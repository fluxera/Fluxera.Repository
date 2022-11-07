namespace Fluxera.Repository.InMemory
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class InMemoryUnitOfWork : IUnitOfWork
	{
		private readonly InMemoryContextProvider inMemoryContextProvider;
		private InMemoryContext inMemoryContext;

		private bool isInitialized;

		public InMemoryUnitOfWork(InMemoryContextProvider inMemoryContextProvider)
		{
			Guard.Against.Null(inMemoryContextProvider);

			this.inMemoryContextProvider = inMemoryContextProvider;
		}

		/// <inheritdoc />
		public Task SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			this.EnsureInitialized();

			return this.inMemoryContext.SaveChangesAsync(cancellationToken);
		}

		/// <inheritdoc />
		public void DiscardChanges()
		{
			this.EnsureInitialized();

			this.inMemoryContext.DiscardChanges();
		}

		/// <inheritdoc />
		void IUnitOfWork.Initialize(RepositoryName repositoryName)
		{
			this.inMemoryContext = this.inMemoryContextProvider.GetContextFor(repositoryName);
			this.isInitialized = true;
		}

		private void EnsureInitialized()
		{
			if(!this.isInitialized)
			{
				throw new InvalidOperationException("The unit-of-work instance was not initialized.");
			}
		}
	}
}
