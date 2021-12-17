namespace Fluxera.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Traits;

	public abstract class Repository<TAggregateRoot, TKey> : ReadOnlyRepository<TAggregateRoot, TKey>, IRepository<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		private readonly IRepository<TAggregateRoot, TKey> innerRepository;

		protected Repository(IRepository<TAggregateRoot, TKey> innerRepository)
			: base(innerRepository)
		{
			Guard.Against.Null(innerRepository, nameof(innerRepository));

			this.innerRepository = innerRepository;
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.innerRepository.AddAsync(item, cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.innerRepository.AddAsync(items, cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.innerRepository.UpdateAsync(item, cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.innerRepository.UpdateAsync(items, cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveAsync(item, cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveAsync(id, cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveAsync(predicate, cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveAsync(items, cancellationToken);
		}
	}
}
