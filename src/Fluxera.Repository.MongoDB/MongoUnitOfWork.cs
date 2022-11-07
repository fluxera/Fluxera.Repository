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
		private readonly MongoContextProvider mongoContextProvider;
		private bool isInitialized;

		private MongoContext mongoContext;

		public MongoUnitOfWork(MongoContextProvider mongoContextProvider)
		{
			Guard.Against.Null(mongoContextProvider);

			this.mongoContextProvider = mongoContextProvider;
		}

		/// <inheritdoc />
		public Task SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			this.EnsureInitialized();

			return this.mongoContext.SaveChangesAsync(cancellationToken);
		}

		/// <inheritdoc />
		public void DiscardChanges()
		{
			this.EnsureInitialized();

			this.mongoContext.DiscardChanges();
		}

		/// <inheritdoc />
		void IUnitOfWork.Initialize(RepositoryName repositoryName)
		{
			this.mongoContext = this.mongoContextProvider.GetContextFor(repositoryName);
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
