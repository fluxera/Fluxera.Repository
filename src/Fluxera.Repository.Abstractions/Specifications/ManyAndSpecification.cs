namespace Fluxera.Repository.Specifications
{
	using System.Collections.Generic;
	using JetBrains.Annotations;

	/// <summary>
	///     A specification that combines all given specifications using the And operator.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public sealed class ManyAndSpecification<T> : ManyCompositeSpecification<T> where T : class
	{
		/// <summary>
		///     Creates a new instance of the <see cref="ManyAndSpecification{T}" /> type.
		/// </summary>
		/// <param name="specifications"></param>
		public ManyAndSpecification(IEnumerable<ISpecification<T>> specifications)
			: base(specifications, (s1, s2) => s1.And(s2))
		{
		}
	}
}
