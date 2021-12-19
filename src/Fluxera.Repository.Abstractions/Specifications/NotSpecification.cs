namespace Fluxera.Repository.Specifications
{
	using Fluxera.Linq.Expressions;
	using JetBrains.Annotations;

	/// <summary>
	///     Represents the specification which indicates the semantics opposite to the given specification.
	/// </summary>
	/// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
	[PublicAPI]
	internal sealed class NotSpecification<T> : Specification<T> where T : class
	{
		/// <summary>
		///     Initializes a new instance of <see cref="NotSpecification{T}" /> class.
		/// </summary>
		/// <param name="specification">The specification to be reversed.</param>
		public NotSpecification(ISpecification<T> specification)
			: base(specification.Predicate.Not())
		{
		}
	}
}
