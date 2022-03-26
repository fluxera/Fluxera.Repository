namespace Fluxera.Repository.EntityFrameworkCore
{
	using Fluxera.Entity;
	using Microsoft.EntityFrameworkCore;

	internal interface IDbContextFactory
	{
		DbContext CreateDbContext<TAggregateRoot, TKey>() where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>;
	}
}
