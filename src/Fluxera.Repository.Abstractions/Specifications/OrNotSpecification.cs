namespace Fluxera.Repository.Specifications
{
	using System;
	using System.Linq.Expressions;
	using Fluxera.Linq.Expressions;

	internal sealed class OrNotSpecification<T> : CompositeSpecification<T>
		where T : class
	{
		/// <summary>
		///     Constructs a new instance of <see cref="OrNotSpecification{T}" /> class.
		/// </summary>
		/// <param name="left">The first specification.</param>
		/// <param name="right">The second specification.</param>
		public OrNotSpecification(ISpecification<T> left, ISpecification<T> right)
			: base(left, right)
		{
		}

		/// <inheritdoc />
		public override Expression<Func<T, bool>> Predicate
			=> this.Left.Predicate.OrNot(this.Right.Predicate);
	}
}
