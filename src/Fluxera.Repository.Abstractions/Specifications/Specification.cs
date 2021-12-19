namespace Fluxera.Repository.Specifications
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using Fluxera.Guards;
	using Fluxera.Linq.Expressions;
	using JetBrains.Annotations;

	/// <summary>
	///     Represents an expression-based specification.
	/// </summary>
	/// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
	[PublicAPI]
	public class Specification<T> : ISpecification<T> where T : class
	{
		/// <summary>
		///     A specification that defines that any item satisfies it.
		/// </summary>
		public static ISpecification<T> All = new Specification<T>(x => true);

		/// <summary>
		///     A specification that defines that no item satisfies it.
		/// </summary>
		public static ISpecification<T> None = new Specification<T>(x => false);

		/// <summary>
		///     Creates a new instance of the <see cref="Specification{T}" /> class.
		/// </summary>
		public Specification()
		{
			this.Predicate = null!;
		}

		/// <summary>
		///     Creates a new instance of the <see cref="Specification{T}" /> class.
		/// </summary>
		/// <param name="predicate"></param>
		public Specification(Expression<Func<T, bool>> predicate)
		{
			Guard.Against.Null(predicate, nameof(predicate));

			this.Predicate = predicate;
		}

		/// <summary>
		///     Creates a new instance of the <see cref="Specification{T}" /> class.
		/// </summary>
		/// <param name="specification"></param>
		public Specification(ISpecification<T> specification)
		{
			Guard.Against.Null(specification, nameof(specification));

			this.Predicate = specification.Predicate;
		}

		/// <inheritdoc />
		public bool IsSatisfiedBy(T item)
		{
			Guard.Against.Null(item, nameof(item));

			return (this.Predicate == null!) || new T[] { item }.AsQueryable().Any(this.Predicate);
		}

		/// <inheritdoc />
		public IQueryable<T> ApplyTo(IQueryable<T> queryable)
		{
			Guard.Against.Null(queryable, nameof(queryable));

			return this.Predicate == null!
				? queryable
				: queryable.Where(this.Predicate);
		}

		/// <inheritdoc />
		public ISpecification<T> And(ISpecification<T> specification)
		{
			Guard.Against.Null(specification, nameof(specification));

			return new AndSpecification<T>(this, specification);
		}

		/// <inheritdoc />
		public ISpecification<T> And(Expression<Func<T, bool>> predicate)
		{
			Guard.Against.Null(predicate, nameof(predicate));

			return this.And(new Specification<T>(predicate));
		}

		/// <inheritdoc />
		public ISpecification<T> AndAlso(ISpecification<T> specification)
		{
			Guard.Against.Null(specification, nameof(specification));

			return new AndAlsoSpecification<T>(this, specification);
		}

		/// <inheritdoc />
		public ISpecification<T> AndAlso(Expression<Func<T, bool>> predicate)
		{
			Guard.Against.Null(predicate, nameof(predicate));

			return this.AndAlso(new Specification<T>(predicate));
		}

		/// <inheritdoc />
		public ISpecification<T> Or(ISpecification<T> specification)
		{
			Guard.Against.Null(specification, nameof(specification));

			return new OrSpecification<T>(this, specification);
		}

		/// <inheritdoc />
		public ISpecification<T> Or(Expression<Func<T, bool>> predicate)
		{
			Guard.Against.Null(predicate, nameof(predicate));

			return this.Or(new Specification<T>(predicate));
		}

		/// <inheritdoc />
		public ISpecification<T> OrElse(ISpecification<T> specification)
		{
			Guard.Against.Null(specification, nameof(specification));

			return new OrElseSpecification<T>(this, specification);
		}

		/// <inheritdoc />
		public ISpecification<T> OrElse(Expression<Func<T, bool>> predicate)
		{
			Guard.Against.Null(predicate, nameof(predicate));

			return this.OrElse(new Specification<T>(predicate));
		}

		/// <inheritdoc />
		public ISpecification<T> Not()
		{
			return new NotSpecification<T>(this);
		}

		/// <inheritdoc />
		public ISpecification<T> AndNot(ISpecification<T> specification)
		{
			Guard.Against.Null(specification, nameof(specification));

			return new AndNotSpecification<T>(this, specification);
		}

		/// <inheritdoc />
		public ISpecification<T> AndNot(Expression<Func<T, bool>> predicate)
		{
			Guard.Against.Null(predicate, nameof(predicate));

			return this.AndNot(new Specification<T>(predicate));
		}

		/// <inheritdoc />
		public ISpecification<T> OrNot(ISpecification<T> specification)
		{
			Guard.Against.Null(specification, nameof(specification));

			return new OrNotSpecification<T>(this, specification);
		}

		/// <inheritdoc />
		public ISpecification<T> OrNot(Expression<Func<T, bool>> predicate)
		{
			Guard.Against.Null(predicate, nameof(predicate));

			return this.OrNot(new Specification<T>(predicate));
		}

		/// <inheritdoc />
		public virtual Expression<Func<T, bool>> Predicate { get; private set; }

		/// <inheritdoc />
		public override string ToString()
		{
			return this.Predicate.ToExpressionString() ?? base.ToString();
		}
	}
}
