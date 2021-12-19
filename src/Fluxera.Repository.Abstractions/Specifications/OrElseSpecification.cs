namespace Fluxera.Repository.Specifications
{
	using System;
	using System.Linq.Expressions;
	using Fluxera.Linq.Expressions;

	/// <summary>
	/// </summary>
	/// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
	internal sealed class OrElseSpecification<T> : CompositeSpecification<T> where T : class
	{
		/// <summary>
		///     Initializes a new instance of <see cref="OrSpecification{T}" /> class.
		/// </summary>
		/// <param name="left">The left specification.</param>
		/// <param name="right">The right specification.</param>
		public OrElseSpecification(ISpecification<T> left, ISpecification<T> right)
			: base(left, right)
		{
		}

		/// <inheritdoc />
		public override Expression<Func<T, bool>> Predicate
			=> this.Left.Predicate.OrElse(this.Right.Predicate);
	}
}
