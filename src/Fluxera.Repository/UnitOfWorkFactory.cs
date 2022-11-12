namespace Fluxera.Repository
{
	using System;
	using Fluxera.Extensions.DependencyInjection;

	internal sealed class UnitOfWorkFactory : IUnitOfWorkFactory
	{
		private readonly IServiceProvider serviceProvider;

		public UnitOfWorkFactory(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public IUnitOfWork CreateUnitOfWork(string repositoryName)
		{
			IUnitOfWork unitOfWork = this.serviceProvider.GetRequiredNamedService<IUnitOfWork>(repositoryName);
			unitOfWork.Initialize((RepositoryName)repositoryName);

			return unitOfWork;
		}
	}
}
