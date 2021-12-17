namespace Fluxera.Repository.EntityFramework
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Traits;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.ChangeTracking;
	using Microsoft.Extensions.Logging;

	internal sealed class EntityFrameworkRepository<TAggregateRoot> : IRepository<TAggregateRoot>
		where TAggregateRoot : AggregateRoot<TAggregateRoot>
	{
		private readonly DbContext dbContext;
		private readonly DbSet<TAggregateRoot> dbSet;
		private readonly ILogger logger;

		public EntityFrameworkRepository(ILoggerFactory loggerFactory, IDbContextFactory dbContextFactory)
		{
			Guard.Against.Null(loggerFactory, nameof(loggerFactory));
			Guard.Against.Null(dbContextFactory, nameof(dbContextFactory));

			this.logger = loggerFactory.CreateLogger(Name);
			this.dbContext = dbContextFactory.CreateDbContext<TAggregateRoot>();
			this.dbSet = this.dbContext.Set<TAggregateRoot>();
		}

		private static string Name => "Fluxera.Repository.EntityFrameworkRepository";

		private bool IsDisposed { get; set; }

		async Task ICanAdd<TAggregateRoot>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.dbSet.AddAsync(item, cancellationToken).ConfigureAwait(false);
			await this.dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot>.AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.dbSet.AddRangeAsync(items, cancellationToken).ConfigureAwait(false);
			await this.dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.UpdateAsync(item, cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				await this.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			this.dbSet.Remove(item);
			await this.dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(string id, CancellationToken cancellationToken)
		{
			TAggregateRoot? item = await this.dbSet
				.FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
				.ConfigureAwait(false);

			if(item is not null)
			{
				this.dbSet.Remove(item);
			}

			await this.dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> items = await this.dbSet
				.Where(predicate)
				.ToListAsync(cancellationToken)
				.ConfigureAwait(false);

			this.dbSet.RemoveRange(items);
			await this.dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			this.dbSet.RemoveRange(items);
			await this.dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot>.GetAsync(string id, CancellationToken cancellationToken)
		{
			TAggregateRoot? item = await this.dbSet
				.FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
				.ConfigureAwait(false);

			return item!;
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot>.GetAsync<TResult>(string id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			TResult? result = await this.dbSet
				.Where(x => x.ID == id)
				.Select(selector)
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false);

			return result!;
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(CancellationToken cancellationToken)
		{
			return await this.dbSet
				.LongCountAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.dbSet
				.LongCountAsync(predicate, cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot>.ExistsAsync(string id, CancellationToken cancellationToken)
		{
			return await this.dbSet
				.LongCountAsync(x => x.ID == id, cancellationToken)
				.ConfigureAwait(false) > 0;
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.dbSet
				.LongCountAsync(predicate, cancellationToken)
				.ConfigureAwait(false) > 0;
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.dbSet
				.Where(predicate)
				.ApplyOptions(queryOptions)
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.dbSet
				.Where(predicate)
				.ApplyOptions(queryOptions)
				.Select(selector)
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.dbSet
				.Where(predicate)
				.ApplyOptions(queryOptions)
				.ToListAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.dbSet
				.Where(predicate)
				.ApplyOptions(queryOptions)
				.Select(selector)
				.ToListAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		void IDisposable.Dispose()
		{
			if(!this.IsDisposed)
			{
				this.dbContext.Dispose();
			}

			this.IsDisposed = true;
		}

		/// <inheritdoc />
		bool IReadOnlyRepository<TAggregateRoot>.IsDisposed => this.IsDisposed;

		/// <inheritdoc />
		async ValueTask IAsyncDisposable.DisposeAsync()
		{
			if(!this.IsDisposed)
			{
				await this.dbContext.DisposeAsync();
			}

			this.IsDisposed = true;
		}

		private async Task UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			EntityEntry<TAggregateRoot> entry = this.dbContext.Entry(item);

			try
			{
				if(entry.State == EntityState.Detached)
				{
					string key = item.ID;

					// Check to see if this item is already attached. If it is then we need to copy the values to the
					// attached value instead of changing the State to modified since it will throw a duplicate key
					// exception specifically: "An object with the same key already exists in the ObjectStateManager.
					// The ObjectStateManager cannot track multiple objects with the same key."
					TAggregateRoot? attachedEntity = await this.dbSet.FindAsync(key).ConfigureAwait(false);
					if(attachedEntity is not null)
					{
						this.dbContext.Entry(attachedEntity).CurrentValues.SetValues(item);
					}
				}
			}
			catch
			{
				// Ignore and try the default behavior.
				entry.State = EntityState.Modified;
			}
			finally
			{
				await this.dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return Name;
		}
	}
}
