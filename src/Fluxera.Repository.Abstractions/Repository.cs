﻿namespace Fluxera.Repository
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
	/// <typeparam name="TAggregateRoot"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	public abstract class Repository<TAggregateRoot, TKey> : ReadOnlyRepository<TAggregateRoot, TKey>, IRepository<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly IRepository<TAggregateRoot, TKey> innerRepository;

		/// <summary>
		///     Creates a new instance of the <see cref="Repository{TAggregateRoot,TKey}" /> type.
		/// </summary>
		/// <param name="innerRepository"></param>
		protected Repository(IRepository<TAggregateRoot, TKey> innerRepository)
			: base(innerRepository)
		{
			Guard.Against.Null(innerRepository, nameof(innerRepository));

			this.innerRepository = innerRepository;
		}

		/// <inheritdoc />
		public async Task AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.innerRepository.AddAsync(item, cancellationToken);
		}

		/// <inheritdoc />
		public async Task AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.innerRepository.AddRangeAsync(items, cancellationToken);
		}

		/// <inheritdoc />
		public async Task UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.innerRepository.UpdateAsync(item, cancellationToken);
		}

		/// <inheritdoc />
		public async Task UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.innerRepository.UpdateRangeAsync(items, cancellationToken);
		}

		/// <inheritdoc />
		public async Task RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveAsync(item, cancellationToken);
		}

		/// <inheritdoc />
		public async Task RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveAsync(id, cancellationToken);
		}

		/// <inheritdoc />
		public async Task RemoveRangeAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveRangeAsync(predicate, cancellationToken);
		}

		/// <inheritdoc />
		public async Task RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveRangeAsync(specification, cancellationToken);
		}

		/// <inheritdoc />
		public async Task RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveRangeAsync(items, cancellationToken);
		}
	}
}
