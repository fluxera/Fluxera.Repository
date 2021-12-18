namespace Fluxera.Repository.Specifications
{
	using System.Collections.Generic;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class ManyOrSpecification<T> : ManyCompositeSpecification<T> where T : class
	{
		public ManyOrSpecification(IEnumerable<ISpecification<T>> specifications)
			: base(specifications, (s1, s2) => s1.Or(s2))
		{
		}
	}
}
