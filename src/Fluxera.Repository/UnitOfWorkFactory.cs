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
		public IUnitOfWork CreateFor(RepositoryName repositoryName)
		{
			IUnitOfWork unitOfWork = this.serviceProvider.GetRequiredNamedService<IUnitOfWork>(repositoryName.Name);
			unitOfWork.Initialize(repositoryName);

			return unitOfWork;
		}
	}
}
