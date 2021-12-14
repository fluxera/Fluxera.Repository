namespace Fluxera.Repository
{
	using System.Collections.Generic;
	using Fluxera.Entity;
	using Fluxera.Extensions.Validation;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IValidatorProvider
	{
		IEnumerable<IValidator> GetValidatorsFor<TAggregateRoot>()
			where TAggregateRoot : AggregateRoot<TAggregateRoot>;
	}
}
