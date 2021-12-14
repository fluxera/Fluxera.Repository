namespace Fluxera.Repository.Storage.EntityFramework
{
	using Fluxera.Entity;
	using Microsoft.EntityFrameworkCore;

	internal interface IDbContextFactory
	{
		DbContext CreateDbContext<TAggregateRoot>() where TAggregateRoot : AggregateRoot<TAggregateRoot>;
	}
}
