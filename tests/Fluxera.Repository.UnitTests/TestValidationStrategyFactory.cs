namespace Fluxera.Repository.UnitTests
{
	using System;
	using Fluxera.Entity;
	using Fluxera.Extensions.Validation;
	using Fluxera.Repository.Validation;
	using Microsoft.Extensions.DependencyInjection;

	public class TestValidationStrategyFactory : IValidationStrategyFactory
	{
		private readonly IServiceProvider serviceProvider;

		public TestValidationStrategyFactory(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public IValidationStrategy<TEntity, TKey> CreateStrategy<TEntity, TKey>()
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			IValidationService validationService = this.serviceProvider.GetRequiredService<IValidationService>();

			return new TestValidationStrategy<TEntity, TKey>(validationService);
		}
	}
}
