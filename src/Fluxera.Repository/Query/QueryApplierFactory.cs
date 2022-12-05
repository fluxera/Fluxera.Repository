namespace Fluxera.Repository.Query
{
	using System;
	using Fluxera.Extensions.DependencyInjection;

	internal sealed class QueryApplierFactory : IQueryApplierFactory
	{
		private readonly IServiceProvider serviceProvider;

		public QueryApplierFactory(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public IIncludeApplier CreateIncludeApplier(string repositoryName)
		{
			IIncludeApplier includeApplier = this.serviceProvider.GetNamedService<IIncludeApplier>(repositoryName);
			return includeApplier;
		}
	}
}
