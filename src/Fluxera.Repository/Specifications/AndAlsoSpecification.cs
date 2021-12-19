namespace Fluxera.Repository.Specifications
{
	using System;
	using System.Linq.Expressions;
	using Fluxera.Linq.Expressions;

	/// <summary>
	/// </summary>
	/// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
	internal sealed class AndAlsoSpecification<T> : CompositeSpecification<T> where T : class
	{
		/// <summary>
		///     Constructs a new instance of <see cref="AndSpecification{T}" /> class.
		/// </summary>
		/// <param name="left">The left specification.</param>
		/// <param name="right">The right specification.</param>
		public AndAlsoSpecification(ISpecification<T> left, ISpecification<T> right)
			: base(left, right)
		{
		}

		/// <inheritdoc />
		public override Expression<Func<T, bool>> Predicate
			=> this.Left.Predicate.AndAlso(this.Right.Predicate);
	}
}
