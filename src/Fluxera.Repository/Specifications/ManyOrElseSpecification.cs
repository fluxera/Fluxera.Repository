namespace Fluxera.Repository.Specifications
{
	using System.Collections.Generic;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class ManyOrElseSpecification<T> : ManyCompositeSpecification<T> where T : class
	{
		public ManyOrElseSpecification(IEnumerable<ISpecification<T>> specifications)
			: base(specifications, (s1, s2) => s1.OrElse(s2))
		{
		}
	}
}
