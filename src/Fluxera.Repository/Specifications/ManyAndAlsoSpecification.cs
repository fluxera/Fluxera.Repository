namespace Fluxera.Repository.Specifications
{
	using System.Collections.Generic;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class ManyAndAlsoSpecification<T> : ManyCompositeSpecification<T> where T : class
	{
		public ManyAndAlsoSpecification(IEnumerable<ISpecification<T>> specifications)
			: base(specifications, (s1, s2) => s1.AndAlso(s2))
		{
		}
	}
}
