namespace Fluxera.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Specifications;

	/// <summary>
	///     A base class for specialized repository service implementations.
	/// </summary>
	/// <example>
	///     public interface IPersonRepository : ICanAdd&lt;Person, Guid&gt;
	///     {
	///     }
	///     <br />
	///     public class PersonRepository : Repository&lt;Person, Guid&gt;, IPersonRepository
	///     {
	///     public PersonRepository(IRepository&lt;Person, Guid&gt; innerRepository)
	///     : base(innerRepository)
	///     {
	///     }
	///     }
	/// </example>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	public abstract class Repository<TEntity, TKey> : ReadOnlyRepository<TEntity, TKey>, IRepository<TEntity, TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly IRepository<TEntity, TKey> innerRepository;

		/// <summary>
		///     Creates a new instance of the <see cref="Repository{TAggregateRoot,TKey}" /> type.
		/// </summary>
		/// <param name="innerRepository"></param>
		protected Repository(IRepository<TEntity, TKey> innerRepository)
			: base(innerRepository)
		{
			Guard.Against.Null(innerRepository, nameof(innerRepository));

			this.innerRepository = innerRepository;
		}

		/// <inheritdoc />
		public async Task AddAsync(TEntity item, CancellationToken cancellationToken)
		{
			await this.innerRepository.AddAsync(item, cancellationToken);
		}

		/// <inheritdoc />
		public async Task AddRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			await this.innerRepository.AddRangeAsync(items, cancellationToken);
		}

		/// <inheritdoc />
		public async Task UpdateAsync(TEntity item, CancellationToken cancellationToken)
		{
			await this.innerRepository.UpdateAsync(item, cancellationToken);
		}

		/// <inheritdoc />
		public async Task UpdateRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			await this.innerRepository.UpdateRangeAsync(items, cancellationToken);
		}

		/// <inheritdoc />
		public async Task RemoveAsync(TEntity item, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveAsync(item, cancellationToken);
		}

		/// <inheritdoc />
		public async Task RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveAsync(id, cancellationToken);
		}

		/// <inheritdoc />
		public async Task RemoveRangeAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveRangeAsync(predicate, cancellationToken);
		}

		/// <inheritdoc />
		public async Task RemoveRangeAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveRangeAsync(specification, cancellationToken);
		}

		/// <inheritdoc />
		public async Task RemoveRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveRangeAsync(items, cancellationToken);
		}
	}
}
