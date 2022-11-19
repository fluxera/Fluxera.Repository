namespace Fluxera.Repository.EntityFrameworkCore
{
	using System;
	using System.Collections.Concurrent;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Guards;
	using Fluxera.Repository.Options;
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
		private ConcurrentQueue<Func<Task>> commands;
		private DbContext context;

		private bool isConfigured;

		/// <summary>
		///     Initializes a new instance of the <see cref="EntityFrameworkCoreContext" /> type.
		/// </summary>
		protected EntityFrameworkCoreContext()
		{
			// Command will be stored and later processed on saving changes.
			this.commands = new ConcurrentQueue<Func<Task>>();
		}

		private IServiceProvider ServiceProvider { get; set; }

		private RepositoryOptions RepositoryOptions { get; set; }

		/// <summary>
		///     Gets the name of the repository this context belong to.
		/// </summary>
		protected RepositoryName RepositoryName { get; private set; }

		/// <summary>
		///     Adds a command for execution.
		/// </summary>
		/// <param name="command"></param>
		public Task AddCommandAsync(Func<Task> command)
		{
			Guard.Against.Null(command);

			this.commands ??= new ConcurrentQueue<Func<Task>>();
			this.commands.Enqueue(command);

			return Task.CompletedTask;
		}

		/// <summary>
		///     Saves the changes inside a transaction.
		/// </summary>
		/// <returns></returns>
		public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			//if(!this.commands.Any())
			//{
			//	return;
			//}

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
		///     Checks if any new, deleted, or changed entities are being tracked
		///     such that these changes will be sent to the database if <see cref="DbContext.SaveChanges()" />
		///     or <see cref="DbContext.SaveChangesAsync(CancellationToken)" /> is called.
		/// </summary>
		/// <returns></returns>
		public bool HasChanges()
		{
			return this.context.ChangeTracker.HasChanges();
		}

		/// <summary>
		///     Stops tracking all currently tracked entities.
		/// </summary>
		public void DiscardChanges()
		{
			this.context.ChangeTracker.Clear();
		}

		/// <summary>
		///     Configures the options to use for this context instance over it's lifetime.
		/// </summary>
		protected abstract void ConfigureOptions(EntityFrameworkCoreContextOptions options);

		internal void Configure(RepositoryName repositoryName, IServiceProvider serviceProvider)
		{
			if(!this.isConfigured)
			{
				EntityFrameworkCoreContextOptions options = new EntityFrameworkCoreContextOptions();

				this.ConfigureOptions(options);

				this.context = (DbContext)serviceProvider.GetRequiredService(options.DbContextType);

				IRepositoryRegistry repositoryRegistry = serviceProvider.GetRequiredService<IRepositoryRegistry>();
				RepositoryOptions repositoryOptions = repositoryRegistry.GetRepositoryOptionsFor(repositoryName);
				this.RepositoryOptions = repositoryOptions;

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
		///     Gets an <see cref="EntityEntry{TEntity}" /> for the given entity. The entry provides
		///     access to change tracking information and operations for the entity.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		internal EntityEntry<TEntity> Entry<TEntity>(TEntity item) where TEntity : class
		{
			return this.context.Entry(item);
		}

		/// <inheritdoc />
		protected override void DisposeManaged()
		{
			this.ClearCommands();
		}

		private void ClearCommands()
		{
			this.commands?.Clear();
		}

		private async Task ExecuteCommands(CancellationToken cancellationToken)
		{
			foreach(Func<Task> command in this.commands)
			{
				cancellationToken.ThrowIfCancellationRequested();

				await command.Invoke().ConfigureAwait(false);
			}
		}

		private void ClearDomainEvents()
		{
			OutboxDomainEventDispatcher outboxDispatcher = this.ServiceProvider.GetRequiredService<OutboxDomainEventDispatcher>();
			outboxDispatcher.Clear();
		}

		private async Task DispatchDomainEventsAsync()
		{
			OutboxDomainEventDispatcher outboxDispatcher = this.ServiceProvider.GetRequiredService<OutboxDomainEventDispatcher>();
			await outboxDispatcher.FlushAsync();
		}
	}
}
