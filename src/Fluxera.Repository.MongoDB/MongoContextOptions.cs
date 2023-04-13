namespace Fluxera.Repository.MongoDB
{
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using System;
	using MadEyeMatt.MongoDB.DbContext;

	/// <summary>
	///     Provides the options for the MongoDB repository implementation.
	/// </summary>
	[PublicAPI]
	public sealed class MongoContextOptions
	{
		/// <summary>
		///     Gets the configured db context type.
		/// </summary>
		public Type DbContextType { get; private set; }

		/// <summary>
		///     Configures the db context to use for the repository context.
		/// </summary>
		/// <typeparam name="TContext"></typeparam>
		public void UseDbContext<TContext>() where TContext : MongoDbContext
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
			Guard.Against.False(dbContextType.IsAssignableTo(typeof(MongoDbContext)),
				message: $"The db context type must inherit from '{nameof(MongoDbContext)}'.");

			this.DbContextType = dbContextType;
		}
	}
}
