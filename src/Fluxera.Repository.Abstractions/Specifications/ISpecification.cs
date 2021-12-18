namespace Fluxera.Repository.Specifications
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	/// <summary>
	///     A contact for specifications.
	/// </summary>
	/// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
	[PublicAPI]
	public interface ISpecification<T> where T : class
	{
		/// <summary>
		///     Gets the expression which represents the specification.
		/// </summary>
		Expression<Func<T, bool>> Predicate { get; }

		/// <summary>
		///     Returns a <see cref="bool" /> value which indicates whether the specification
		///     is satisfied by the given object.
		/// </summary>
		/// <param name="item">The object to which the specification is applied.</param>
		/// <returns><c>true</c> if the specification is satisfied; <c>false</c> otherwise.</returns>
		bool IsSatisfiedBy(T item);

		/// <summary>
		///     Applies the underlying expression predicate of this specification
		///     to the given <see cref="IQueryable{T}" />.
		/// </summary>
		/// <param name="queryable">The queryable to apply to.</param>
		/// <returns>The modified queryable.</returns>
		IQueryable<T> ApplyTo(IQueryable<T> queryable);

		ISpecification<T> And(ISpecification<T> specification);

		ISpecification<T> And(Expression<Func<T, bool>> predicate);

		ISpecification<T> AndAlso(ISpecification<T> specification);

		ISpecification<T> AndAlso(Expression<Func<T, bool>> predicate);

		ISpecification<T> Or(ISpecification<T> specification);

		ISpecification<T> Or(Expression<Func<T, bool>> predicate);

		ISpecification<T> OrElse(ISpecification<T> specification);

		ISpecification<T> OrElse(Expression<Func<T, bool>> predicate);

		ISpecification<T> Not();

		ISpecification<T> AndNot(ISpecification<T> specification);

		ISpecification<T> AndNot(Expression<Func<T, bool>> predicate);

		ISpecification<T> OrNot(ISpecification<T> specification);

		ISpecification<T> OrNot(Expression<Func<T, bool>> predicate);
	}
}
