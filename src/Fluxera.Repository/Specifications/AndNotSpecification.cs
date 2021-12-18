namespace Fluxera.Repository.Specifications
{
	using System;
	using System.Linq.Expressions;
	using Fluxera.Linq.Expressions;

	/// <summary>
	///     Represents the combined specification which indicates that the first specification
	///     can be satisfied by the given object whereas the second one cannot.
	/// </summary>
	/// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
	internal sealed class AndNotSpecification<T> : CompositeSpecification<T>
		where T : class
	{
		/// <summary>
		///     Constructs a new instance of <see cref="AndNotSpecification{T}" /> class.
		/// </summary>
		/// <param name="left">The first specification.</param>
		/// <param name="right">The second specification.</param>
		public AndNotSpecification(ISpecification<T> left, ISpecification<T> right)
			: base(left, right)
		{
		}

		/// <inheritdoc />
		public override Expression<Func<T, bool>> Predicate
			=> this.Left.Predicate.AndNot(this.Right.Predicate);
	}
}
