namespace Fluxera.Repository.MongoDB
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class MongoUnitOfWork : IUnitOfWork
	{
		private readonly MongoContextProvider contextProvider;
		private MongoContext context;

		private bool isInitialized;

		public MongoUnitOfWork(MongoContextProvider contextProvider)
		{
			Guard.Against.Null(contextProvider);

			this.contextProvider = contextProvider;
		}

		/// <inheritdoc />
		public Task SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			this.EnsureInitialized();

			return this.context.SaveChangesAsync(cancellationToken);
		}

		/// <inheritdoc />
		public void DiscardChanges()
		{
			this.EnsureInitialized();

			this.context.DiscardChanges();
		}

		/// <inheritdoc />
		void IUnitOfWork.Initialize(RepositoryName repositoryName)
		{
			this.context = this.contextProvider.GetContextFor(repositoryName);

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
