namespace Fluxera.Repository.Specifications
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	[PublicAPI]
	public abstract class ManyCompositeSpecification<T> : Specification<T> where T : class
	{
		private readonly IEnumerable<ISpecification<T>> specifications;

		protected ManyCompositeSpecification(
			IEnumerable<ISpecification<T>> specifications,
			Func<ISpecification<T>, ISpecification<T>, ISpecification<T>> converter)
		{
			this.Converter = converter;
			this.specifications = Guard.Against.NullOrEmpty(specifications, nameof(specifications));
		}

		public Func<ISpecification<T>, ISpecification<T>, ISpecification<T>> Converter { get; }

		/// <inheritdoc />
		public override Expression<Func<T, bool>> Predicate
		{
			get
			{
				ISpecification<T>[] specificationArray = this.specifications.ToArray();
				ISpecification<T> specification = specificationArray[0];

				foreach(ISpecification<T> spec in specificationArray[1..])
				{
					specification = specification.Or(spec);
				}

				return specification.Predicate;
			}
		}
	}
}
