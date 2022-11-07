namespace Fluxera.Repository.EntityFrameworkCore
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;

	[UsedImplicitly]
	internal sealed class EntityFrameworkCoreUnitOfWork : IUnitOfWork
	{
		private readonly DbContextProvider dbContextProvider;

		private DbContext dbContext;
		private bool isInitialized;

		public EntityFrameworkCoreUnitOfWork(DbContextProvider dbContextProvider)
		{
			Guard.Against.Null(dbContextProvider);

			this.dbContextProvider = dbContextProvider;
		}

		/// <inheritdoc />
		public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			this.EnsureInitialized();

			if(this.dbContext.ChangeTracker.HasChanges())
			{
				await this.dbContext.SaveChangesAsync(cancellationToken);
			}
		}

		/// <inheritdoc />
		public void DiscardChanges()
		{
			this.EnsureInitialized();

			if(this.dbContext.ChangeTracker.HasChanges())
			{
				this.dbContext.ChangeTracker.Clear();
			}
		}

		/// <inheritdoc />
		void IUnitOfWork.Initialize(RepositoryName repositoryName)
		{
			this.dbContext = this.dbContextProvider.GetContextFor(repositoryName);
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
