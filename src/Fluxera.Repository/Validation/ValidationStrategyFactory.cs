namespace Fluxera.Repository.Validation
{
	using System;
	using Fluxera.Entity;
	using Fluxera.Extensions.Validation;
	using Fluxera.Guards;
	using Fluxera.Repository.Options;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class ValidationStrategyFactory : IValidationStrategyFactory
	{
		private readonly IRepositoryRegistry repositoryRegistry;
		private readonly IValidationService validationService;

		public ValidationStrategyFactory(IRepositoryRegistry repositoryRegistry, IValidationService validationService)
		{
			Guard.Against.Null(repositoryRegistry, nameof(repositoryRegistry));

			this.repositoryRegistry = repositoryRegistry;
			this.validationService = validationService;
		}

		/// <inheritdoc />
		public IValidationStrategy<TEntity, TKey> CreateStrategy<TEntity, TKey>()
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			RepositoryName repositoryName = this.repositoryRegistry.GetRepositoryNameFor<TEntity>();
			RepositoryOptions repositoryOptions = this.repositoryRegistry.GetRepositoryOptionsFor(repositoryName);

			if(repositoryOptions.ValidationOptions.IsEnabled)
			{
				return new StandardValidationStrategy<TEntity, TKey>(this.validationService);
			}

			return new NoValidationStrategy<TEntity, TKey>();
		}
	}
}
