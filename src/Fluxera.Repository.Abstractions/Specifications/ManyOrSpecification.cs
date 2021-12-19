namespace Fluxera.Repository.Specifications
{
	using System.Collections.Generic;
	using JetBrains.Annotations;

	/// <summary>
	///     A specification that combines all given specifications using the Or operator.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public sealed class ManyOrSpecification<T> : ManyCompositeSpecification<T> where T : class
	{
		/// <summary>
		///     Creates a new instance of the <see cref="ManyOrSpecification{T}" /> type.
		/// </summary>
		/// <param name="specifications"></param>
		public ManyOrSpecification(IEnumerable<ISpecification<T>> specifications)
			: base(specifications, (s1, s2) => s1.Or(s2))
		{
		}
	}
}
