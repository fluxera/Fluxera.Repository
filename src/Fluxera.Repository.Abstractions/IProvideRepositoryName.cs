namespace Fluxera.Repository
{
	using System;
	using Fluxera.Entity;
	using JetBrains.Annotations;

	/// <summary>
	///     Contract for a repository or trait that provides the repository name.
	/// </summary>
	/// <typeparam name="TEntity">The entity type.</typeparam>
	/// <typeparam name="TKey">The type of the ID.</typeparam>
	[PublicAPI]
	public interface IProvideRepositoryName<in TEntity, in TKey>
		where TEntity : Entity<TEntity, TKey>
		where TKey : notnull, IComparable<TKey>, IEquatable<TKey>
	{
		/// <summary>
		///     Gets the repository name.
		/// </summary>
		RepositoryName RepositoryName { get; }
	}
}
