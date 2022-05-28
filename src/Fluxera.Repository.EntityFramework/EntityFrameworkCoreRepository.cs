﻿namespace Fluxera.Repository.EntityFrameworkCore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Specifications;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.ChangeTracking;

	internal sealed class EntityFrameworkCoreRepository<TAggregateRoot, TKey> : LinqRepositoryBase<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly DbContext dbContext;
		private readonly DbSet<TAggregateRoot> dbSet;

		public EntityFrameworkCoreRepository(IDbContextFactory dbContextFactory)
		{
			Guard.Against.Null(dbContextFactory, nameof(dbContextFactory));

			this.dbContext = dbContextFactory.CreateDbContext<TAggregateRoot, TKey>();
			this.dbSet = this.dbContext.Set<TAggregateRoot>();
		}

		private static string Name => "Fluxera.Repository.EntityFrameworkCoreRepository";

		/// <inheritdoc />
		protected override IQueryable<TAggregateRoot> Queryable => this.dbSet.AsQueryable();

		/// <inheritdoc />
		public override string ToString()
		{
			return Name;
		}

		/// <inheritdoc />
		protected override async Task AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.dbSet.AddAsync(item, cancellationToken).ConfigureAwait(false);
			await this.dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.dbSet.AddRangeAsync(items, cancellationToken).ConfigureAwait(false);
			await this.dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> items = await this.dbSet
				.Where(specification.Predicate)
				.ToListAsync(cancellationToken)
				.ConfigureAwait(false);

			this.dbSet.RemoveRange(items);
			await this.dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			this.dbSet.RemoveRange(items);
			await this.dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.PerformUpdateAsync(item).ConfigureAwait(false);
			await this.dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				await this.PerformUpdateAsync(item).ConfigureAwait(false);
			}

			await this.dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<TAggregateRoot> FirstOrDefaultAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken)
		{
			return (await queryable
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false))!;
		}

		/// <inheritdoc />
		protected override async Task<TResult> FirstOrDefaultAsync<TResult>(IQueryable<TResult> queryable, CancellationToken cancellationToken)
		{
			return (await queryable
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false))!;
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<TAggregateRoot>> ToListAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.ToListAsync(cancellationToken)
				.ConfigureAwait(false)
				.AsReadOnly();
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<TResult>> ToListAsync<TResult>(IQueryable<TResult> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.ToListAsync(cancellationToken)
				.ConfigureAwait(false)
				.AsReadOnly();
		}

		/// <inheritdoc />
		protected override async Task<long> LongCountAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.LongCountAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		private async Task PerformUpdateAsync(TAggregateRoot item)
		{
			EntityEntry<TAggregateRoot> entry = this.dbContext.Entry(item);

			try
			{
				if(entry.State == EntityState.Detached)
				{
					TKey key = item.ID!;

					// Check to see if this item is already attached. If it is then we need to copy the values to the
					// attached value instead of changing the State to modified since it will throw a duplicate key
					// exception specifically: "An object with the same key already exists in the ObjectStateManager.
					// The ObjectStateManager cannot track multiple objects with the same key."
					TAggregateRoot attachedEntity = await this.dbSet.FindAsync(key).ConfigureAwait(false);
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
		}
	}
}