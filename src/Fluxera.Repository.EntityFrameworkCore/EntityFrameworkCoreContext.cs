namespace Fluxera.Repository.EntityFrameworkCore
{
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;

	/// <summary>
	///     A base class for context implementations for the EFCore repository.
	/// </summary>
	[PublicAPI]
	public abstract class EntityFrameworkCoreContext : DbContext
	{
		private bool isConfigured;

		/// <summary>
		///     Initializes a new instance of the <see cref="EntityFrameworkCoreContext" /> class. The
		///     <see cref="DbContext.OnConfiguring(DbContextOptionsBuilder)" />
		///     method will be called to configure the database (and other options) to be used for this context.
		/// </summary>
		/// <remarks>
		///     See <see href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and initialization</see>
		///     for more information and examples.
		/// </remarks>
		protected EntityFrameworkCoreContext()
		{
			// For DbContext compatibility.
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="DbContext" /> class using the specified options.
		///     The <see cref="DbContext.OnConfiguring(DbContextOptionsBuilder)" /> method will still be called to allow further
		///     configuration of the options.
		/// </summary>
		/// <remarks>
		///     See <see href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and initialization</see>
		///     and
		///     <see href="https://aka.ms/efcore-docs-dbcontext-options">Using DbContextOptions</see> for more information and
		///     examples.
		/// </remarks>
		/// <param name="options">The options for this context.</param>
		// ReSharper disable once PublicConstructorInAbstractClass
		public EntityFrameworkCoreContext(DbContextOptions options)
			: base(options)
		{
			// For DbContext compatibility.
		}

		/// <summary>
		///     Gets the name of the repository this context belong to.
		/// </summary>
		protected RepositoryName RepositoryName { get; private set; }

		internal void Configure(RepositoryName repositoryName)
		{
			if(!this.isConfigured)
			{
				this.RepositoryName = repositoryName;

				this.isConfigured = true;
			}
		}
	}
}
