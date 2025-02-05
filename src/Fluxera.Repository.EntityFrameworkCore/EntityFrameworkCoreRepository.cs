namespace Fluxera.Repository.EntityFrameworkCore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Extensions;
	using Fluxera.Repository.Options;
	using Fluxera.Repository.Specifications;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.ChangeTracking;

	internal sealed class EntityFrameworkCoreRepository<TEntity, TKey> : LinqRepositoryBase<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly EntityFrameworkCoreContext context;
		private readonly DbSet<TEntity> dbSet;
		private readonly RepositoryOptions options;

		public EntityFrameworkCoreRepository(
			EntityFrameworkCoreContextProvider contextProvider,
			IRepositoryRegistry repositoryRegistry)
			: base(repositoryRegistry)
		{
			Guard.Against.Null(contextProvider, nameof(contextProvider));
			Guard.Against.Null(repositoryRegistry, nameof(repositoryRegistry));

			RepositoryName repositoryName = repositoryRegistry.GetRepositoryNameFor<TEntity>();
			this.options = repositoryRegistry.GetRepositoryOptionsFor(repositoryName);

			this.context = contextProvider.GetContextFor(repositoryName);
			this.dbSet = this.context.Set<TEntity>();
		}

		private static string Name => "Fluxera.Repository.EntityFrameworkCoreRepository";

		/// <inheritdoc />
		protected override IQueryable<TEntity> Queryable => this.dbSet;

		/// <inheritdoc />
		public override string ToString()
		{
			return Name;
		}

		/// <inheritdoc />
		protected override async Task AddAsync(TEntity item, CancellationToken cancellationToken)
		{
			this.PrepareItem(item, EntityState.Added);
			await this.dbSet.AddAsync(item, cancellationToken).ConfigureAwait(false);

			if(!this.options.IsUnitOfWorkEnabled)
			{
				await this.context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		protected override async Task AddRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			IList<TEntity> itemList = items.ToList();

			foreach(TEntity item in itemList)
			{
				this.PrepareItem(item, EntityState.Added);
			}

			await this.dbSet.AddRangeAsync(itemList, cancellationToken).ConfigureAwait(false);

			if(!this.options.IsUnitOfWorkEnabled)
			{
				await this.context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		protected override async Task RemoveRangeAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			IList<TEntity> items = await this.dbSet
				.Where(specification.Predicate)
				.ToListAsync(cancellationToken)
				.ConfigureAwait(false);

			this.dbSet.RemoveRange(items);

			if(!this.options.IsUnitOfWorkEnabled)
			{
				await this.context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		protected override async Task RemoveRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			this.dbSet.RemoveRange(items);

			if(!this.options.IsUnitOfWorkEnabled)
			{
				await this.context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		protected override async Task UpdateAsync(TEntity item, CancellationToken cancellationToken)
		{
			await this.PerformUpdateAsync(item).ConfigureAwait(false);

			if(!this.options.IsUnitOfWorkEnabled)
			{
				await this.context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		protected override async Task UpdateRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			foreach(TEntity item in items)
			{
				await this.PerformUpdateAsync(item).ConfigureAwait(false);
			}

			if(!this.options.IsUnitOfWorkEnabled)
			{
				await this.context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		protected override async Task<TEntity> FirstOrDefaultAsync(IQueryable<TEntity> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<TResult> FirstOrDefaultAsync<TResult>(IQueryable<TResult> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.FirstOrDefaultAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<TEntity>> ToListAsync(IQueryable<TEntity> queryable,
			CancellationToken cancellationToken)
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
		protected override async Task<long> LongCountAsync(IQueryable<TEntity> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.LongCountAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<int> SumAsync(IQueryable<int> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<int> SumAsync(IQueryable<int?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<long> SumAsync(IQueryable<long> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<long> SumAsync(IQueryable<long?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<decimal> SumAsync(IQueryable<decimal> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<decimal> SumAsync(IQueryable<decimal?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<float> SumAsync(IQueryable<float> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<float> SumAsync(IQueryable<float?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<double> SumAsync(IQueryable<double> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<double> SumAsync(IQueryable<double?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.SumAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<double> AverageAsync(IQueryable<int> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<double> AverageAsync(IQueryable<int?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<double> AverageAsync(IQueryable<long> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<double> AverageAsync(IQueryable<long?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<decimal> AverageAsync(IQueryable<decimal> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<decimal> AverageAsync(IQueryable<decimal?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<float> AverageAsync(IQueryable<float> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<float> AverageAsync(IQueryable<float?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
		}

		/// <inheritdoc />
		protected override async Task<double> AverageAsync(IQueryable<double> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task<double> AverageAsync(IQueryable<double?> queryable, CancellationToken cancellationToken)
		{
			return await queryable
				.AverageAsync(cancellationToken)
				.ConfigureAwait(false)
				.GetValueOrDefault();
		}

		private async Task PerformUpdateAsync(TEntity item)
		{
			EntityEntry<TEntity> entry = this.context.Entry(item);

			try
			{
				if(entry.State == EntityState.Detached)
				{
					TKey key = item.ID;

					// Check to see if this item is already attached. If it is then we need to copy the values to the
					// attached value instead of changing the State to modified since it will throw a duplicate key
					// exception specifically: "An object with the same key already exists in the ObjectStateManager.
					// The ObjectStateManager cannot track multiple objects with the same key."
					TEntity attachedEntity = await this.dbSet.FindAsync(key).ConfigureAwait(false);
					if(attachedEntity is not null)
					{
						this.context.Entry(attachedEntity).CurrentValues.SetValues(item);

						this.PrepareItem(attachedEntity, EntityState.Modified);

						// Return in this case to prevent the default behavior.
						return;
					}
				}
			}
			catch
			{
				// Ignored; try the default behavior.
			}

			// Default behavior.
			entry.State = EntityState.Modified;
			this.PrepareItem(item, EntityState.Modified);
		}

		private void PrepareItem(TEntity item, EntityState entityState)
		{
			foreach(PropertyInfo propertyInfo in typeof(TEntity).GetRuntimeProperties())
			{
				if(propertyInfo.PropertyType.IsEntity())
				{
					object value = propertyInfo.GetValue(item);
					if(value is not null)
					{
						this.context.Entry(value).State = EntityState.Modified;
					}
				}
			}

			this.context.Entry(item).State = entityState;
		}
	}
}
