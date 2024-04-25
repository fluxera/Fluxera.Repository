namespace Fluxera.Repository.EntityFrameworkCore
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Repository.DomainEvents;
	using Fluxera.Utilities;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.ChangeTracking;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     A base class for context implementations for the EFCore repository.
	/// </summary>
	[PublicAPI]
	public abstract class EntityFrameworkCoreContext : Disposable
	{
		private DbContext context;

		private bool isConfigured;

		/// <summary>
		///     Initializes a new instance of the <see cref="EntityFrameworkCoreContext" /> type.
		/// </summary>
		protected EntityFrameworkCoreContext()
		{
		}

		private IServiceProvider ServiceProvider { get; set; }

		/// <summary>
		///     Gets the name of the repository this context belong to.
		/// </summary>
		protected RepositoryName RepositoryName { get; private set; }

		/// <summary>
		///     Saves the changes inside a transaction.
		/// </summary>
		/// <returns></returns>
		public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				await this.DispatchDomainEventsAsync();

				await this.context.SaveChangesAsync(cancellationToken);
			}
			finally
			{
				this.ClearDomainEvents();
			}
		}

		/// <summary>
		///     Checks if any commands were added for execution.
		/// </summary>
		/// <returns></returns>
		public bool HasChanges()
		{
			return this.context.ChangeTracker.HasChanges();
		}

		/// <summary>
		///     Discards all changes.
		/// </summary>
		public void DiscardChanges()
		{
			this.context.ChangeTracker.Clear();
		}

		/// <inheritdoc />
		protected override void DisposeManaged()
		{
		}

		/// <summary>
		///     Configures the options to use for this context instance over its lifetime.
		/// </summary>
		protected abstract void ConfigureOptions(EntityFrameworkCoreContextOptions options);

		internal void Configure(RepositoryName repositoryName, IServiceProvider serviceProvider)
		{
			if(!this.isConfigured)
			{
				EntityFrameworkCoreContextOptions options = new EntityFrameworkCoreContextOptions();

				this.ConfigureOptions(options);

				this.context = (DbContext)serviceProvider.GetRequiredService(options.DbContextType);

				this.RepositoryName = repositoryName;
				this.ServiceProvider = serviceProvider;

				this.isConfigured = true;
			}
		}

		/// <summary>
		///     Creates a <see cref="DbSet{TEntity}" /> that can be used to query and save instances of
		///     <typeparamref name="TEntity" />.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <returns></returns>
		internal DbSet<TEntity> Set<TEntity>() where TEntity : class
		{
			return this.context.Set<TEntity>();
		}

		/// <summary>
		///     Gets a <see cref="EntityEntry{TEntity}" /> for the given entity. The entry provides
		///     access to change tracking information and operations for the entity.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		internal EntityEntry<TEntity> Entry<TEntity>(TEntity item) where TEntity : class
		{
			return this.context.Entry(item);
		}

		private void ClearDomainEvents()
		{
			IOutboxDomainEventDispatcher outboxDispatcher = this.ServiceProvider.GetRequiredService<IOutboxDomainEventDispatcher>();
			outboxDispatcher.Clear();
		}

		private async Task DispatchDomainEventsAsync()
		{
			IOutboxDomainEventDispatcher outboxDispatcher = this.ServiceProvider.GetRequiredService<IOutboxDomainEventDispatcher>();
			await outboxDispatcher.FlushAsync();
		}
	}
}
