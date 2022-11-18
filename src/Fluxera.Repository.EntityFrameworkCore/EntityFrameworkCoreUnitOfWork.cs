namespace Fluxera.Repository.EntityFrameworkCore
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class EntityFrameworkCoreUnitOfWork : IUnitOfWork
	{
		private readonly EntityFrameworkCoreContextProvider contextProvider;

		private EntityFrameworkCoreContext context;
		private bool isInitialized;

		public EntityFrameworkCoreUnitOfWork(EntityFrameworkCoreContextProvider contextProvider)
		{
			Guard.Against.Null(contextProvider);

			this.contextProvider = contextProvider;
		}

		/// <inheritdoc />
		public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			this.EnsureInitialized();

			if(this.context.HasChanges())
			{
				await this.context.SaveChangesAsync(cancellationToken);
			}
		}

		/// <inheritdoc />
		public void DiscardChanges()
		{
			this.EnsureInitialized();

			if(this.context.HasChanges())
			{
				this.context.DiscardChanges();
			}
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
