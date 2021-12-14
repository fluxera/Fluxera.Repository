namespace Fluxera.Repository.Storage.EntityFramework
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Repository.Query;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.ChangeTracking;
	using Microsoft.Extensions.Logging;

	internal sealed class EntityFrameworkRepository<TAggregateRoot> : RepositoryBase<TAggregateRoot>
		where TAggregateRoot : AggregateRoot<TAggregateRoot>
	{
		private readonly IDbContextFactory dbContextFactory;

		private DbContext dbContext;
		private DbSet<TAggregateRoot> dbSet;

		public EntityFrameworkRepository(ILoggerFactory loggerFactory, IDbContextFactory dbContextFactory)
			: base(loggerFactory)
		{
			this.dbContextFactory = dbContextFactory;
		}

		/// <inheritdoc />
		protected override string Name => "Fluxera.Repository.EntityFrameworkRepository";

		/// <inheritdoc />
		protected override async Task OnAddAsync(TAggregateRoot item, CancellationToken cancellationToken = default)
		{
			await this.dbSet.AddAsync(item, cancellationToken).ConfigureAwait(false);
			await this.dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task OnAddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken = default)
		{
			await this.dbSet.AddRangeAsync(items, cancellationToken).ConfigureAwait(false);
			await this.dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task OnUpdateAsync(TAggregateRoot item, CancellationToken cancellationToken = default)
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
					TAggregateRoot? attachedEntity = await this.dbContext.Set<TAggregateRoot>().FindAsync(key).ConfigureAwait(false);
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
		protected override async Task OnUpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken = default)
		{
			foreach(TAggregateRoot item in items)
			{
				await this.OnUpdateAsync(item, cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		protected override async Task OnRemoveAsync(TAggregateRoot item, CancellationToken cancellationToken = default)
		{
			this.dbSet.Remove(item);
			await this.dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task OnRemoveAsync(string id, CancellationToken cancellationToken = default)
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
		protected override async Task OnRemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken = default)
		{
			IList<TAggregateRoot> items = await this.dbSet
				.Where(predicate)
				.ToListAsync(cancellationToken)
				.ConfigureAwait(false);

			this.dbSet.RemoveRange(items);
			await this.dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task OnRemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken = default)
		{
			this.dbSet.RemoveRange(items);
			await this.dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<TAggregateRoot> OnGetAsync(string id, CancellationToken cancellationToken = default)
		{
			TAggregateRoot? item = await this.dbSet
				.FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
				.ConfigureAwait(false);

			return item!;
		}

		/// <inheritdoc />
		protected override async Task<TResult> OnGetAsync<TResult>(string id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken = default)
		{
			TResult? result = await this.dbSet
				.Where(x => x.ID == id)
				.Select(selector)
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false);

			return result!;
		}

		/// <inheritdoc />
		protected override async Task<long> OnCountAsync(CancellationToken cancellationToken)
		{
			return await this.dbSet
				.LongCountAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<long> OnCountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken = default)
		{
			return await this.dbSet
				.LongCountAsync(predicate, cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken)
		{
			return await this.dbSet
				.LongCountAsync(x => x.ID == id, cancellationToken)
				.ConfigureAwait(false) > 0;
		}

		/// <inheritdoc />
		protected override async Task<bool> ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.dbSet
				.LongCountAsync(predicate, cancellationToken)
				.ConfigureAwait(false) > 0;
		}

		/// <inheritdoc />
		protected override async Task<TAggregateRoot> OnFindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken = default)
		{
			return await this.dbSet
				.Where(predicate)
				.ApplyOptions(queryOptions)
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<TResult> OnFindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken = default)
		{
			return await this.dbSet
				.Where(predicate)
				.ApplyOptions(queryOptions)
				.Select(selector)
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<TAggregateRoot>> OnFindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken = default)
		{
			return await this.dbSet
				.Where(predicate)
				.ApplyOptions(queryOptions)
				.ToListAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<TResult>> OnFindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken = default)
		{
			return await this.dbSet
				.Where(predicate)
				.ApplyOptions(queryOptions)
				.Select(selector)
				.ToListAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override void OnDispose()
		{
			this.dbContext.Dispose();
		}
	}
}
