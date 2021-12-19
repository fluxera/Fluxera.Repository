namespace Fluxera.Repository.Specifications
{
	using System.Collections.Generic;
	using JetBrains.Annotations;

	/// <summary>
	///     A specification that combines all given specifications using the OrElse operator.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public sealed class ManyOrElseSpecification<T> : ManyCompositeSpecification<T> where T : class
	{
		/// <summary>
		///     Creates a new instance of the <see cref="ManyOrElseSpecification{T}" /> type.
		/// </summary>
		/// <param name="specifications"></param>
		public ManyOrElseSpecification(IEnumerable<ISpecification<T>> specifications)
			: base(specifications, (s1, s2) => s1.OrElse(s2))
		{
		}
	}
}
