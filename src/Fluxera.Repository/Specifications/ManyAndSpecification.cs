namespace Fluxera.Repository.Specifications
{
	using System.Collections.Generic;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class ManyAndSpecification<T> : ManyCompositeSpecification<T> where T : class
	{
		public ManyAndSpecification(IEnumerable<ISpecification<T>> specifications)
			: base(specifications, (s1, s2) => s1.And(s2))
		{
		}
	}
}
