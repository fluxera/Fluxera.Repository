namespace Fluxera.Repository.Validation
{
	using Fluxera.Entity;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IValidationStrategyFactory
	{
		/// <summary>
		///     Creates the validation strategy to use for the repository and <see cref="TAggregateRoot" />.
		/// </summary>
		/// <typeparam name="TAggregateRoot"></typeparam>
		/// <returns></returns>
		IValidationStrategy<TAggregateRoot> CreateStrategy<TAggregateRoot>()
			where TAggregateRoot : AggregateRoot<TAggregateRoot>;
	}
}
