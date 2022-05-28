namespace Fluxera.Repository.EntityFrameworkCore
{
	using System;
	using Fluxera.Entity;
	using Microsoft.EntityFrameworkCore;

	internal interface IDbContextFactory
	{
		DbContext CreateDbContext<TAggregateRoot, TKey>()
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>;
	}
}
