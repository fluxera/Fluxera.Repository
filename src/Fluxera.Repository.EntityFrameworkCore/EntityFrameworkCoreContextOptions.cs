namespace Fluxera.Repository.EntityFrameworkCore
{
	using System;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;

	/// <summary>
	///     Provides the options for the EF Core repository implementation.
	/// </summary>
	[PublicAPI]
	public sealed class EntityFrameworkCoreContextOptions
	{
		/// <summary>
		///     Gets the configured db context type.
		/// </summary>
		public Type DbContextType { get; private set; }

		/// <summary>
		///     Configures the db context to use for the repository context.
		/// </summary>
		/// <typeparam name="TContext"></typeparam>
		public void UseDbContext<TContext>() where TContext : DbContext
		{
			this.UseDbContext(typeof(TContext));
		}

		/// <summary>
		///     Configures the db context to use for the repository context.
		/// </summary>
		/// <param name="dbContextType"></param>
		public void UseDbContext(Type dbContextType)
		{
			Guard.Against.Null(dbContextType);
			Guard.Against.False(dbContextType.IsAssignableTo(typeof(DbContext)),
				message: $"The db context type must inherit from '{nameof(DbContext)}'.");

			this.DbContextType = dbContextType;
		}
	}
}
