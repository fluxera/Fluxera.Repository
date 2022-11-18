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
	using Fluxera.Repository.Options;
	using Fluxera.Repository.Specifications;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.ChangeTracking;

	internal sealed class EntityFrameworkCoreRepository<TAggregateRoot, TKey> : LinqRepositoryBase<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly EntityFrameworkCoreContext context;
		private readonly DbSet<TAggregateRoot> dbSet;
		private readonly RepositoryOptions options;

		public EntityFrameworkCoreRepository(
			EntityFrameworkCoreContextProvider contextProvider,
			IRepositoryRegistry repositoryRegistry)
		{
			Guard.Against.Null(contextProvider, nameof(contextProvider));
			Guard.Against.Null(repositoryRegistry, nameof(repositoryRegistry));

			RepositoryName repositoryName = repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();
			this.options = repositoryRegistry.GetRepositoryOptionsFor(repositoryName);

			this.context = contextProvider.GetContextFor(repositoryName);
			this.dbSet = this.context.Set<TAggregateRoot>();
		}

		private static string Name => "Fluxera.Repository.EntityFrameworkCoreRepository";

		/// <inheritdoc />
		protected override IQueryable<TAggregateRoot> Queryable => this.dbSet;

		/// <inheritdoc />
		public override string ToString()
		{
			return Name;
		}

		/// <inheritdoc />
		protected override async Task AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			this.PrepareItem(item, EntityState.Added);
			await this.dbSet.AddAsync(item, cancellationToken).ConfigureAwait(false);

			if(!this.options.IsUnitOfWorkEnabled)
			{
				await this.context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		protected override async Task AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> itemList = items.ToList();

			foreach(TAggregateRoot item in itemList)
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
		protected override async Task RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			IList<TAggregateRoot> items = await this.dbSet
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
		protected override async Task RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			this.dbSet.RemoveRange(items);

			if(!this.options.IsUnitOfWorkEnabled)
			{
				await this.context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		protected override async Task UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.PerformUpdateAsync(item).ConfigureAwait(false);

			if(!this.options.IsUnitOfWorkEnabled)
			{
				await this.context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		protected override async Task UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			foreach(TAggregateRoot item in items)
			{
				await this.PerformUpdateAsync(item).ConfigureAwait(false);
			}

			if(!this.options.IsUnitOfWorkEnabled)
			{
				await this.context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			}
		}

		/// <inheritdoc />
		protected override async Task<TAggregateRoot> FirstOrDefaultAsync(IQueryable<TAggregateRoot> queryable, CancellationToken cancellationToken)
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

		private async Task PerformUpdateAsync(TAggregateRoot item)
		{
			EntityEntry<TAggregateRoot> entry = this.context.Entry(item);

			try
			{
				if(entry.State == EntityState.Detached)
				{
					TKey key = item.ID;

					// Check to see if this item is already attached. If it is then we need to copy the values to the
					// attached value instead of changing the State to modified since it will throw a duplicate key
					// exception specifically: "An object with the same key already exists in the ObjectStateManager.
					// The ObjectStateManager cannot track multiple objects with the same key."
					TAggregateRoot attachedEntity = await this.dbSet.FindAsync(key).ConfigureAwait(false);
					if(attachedEntity is not null)
					{
						this.context.Entry(attachedEntity).CurrentValues.SetValues(item);
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
				this.PrepareItem(item, EntityState.Modified);
			}
		}

		private void PrepareItem(TAggregateRoot item, EntityState entityState)
		{
			foreach(PropertyInfo propertyInfo in typeof(TAggregateRoot).GetProperties())
			{
				if(propertyInfo.PropertyType.IsAggregateRoot())
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
