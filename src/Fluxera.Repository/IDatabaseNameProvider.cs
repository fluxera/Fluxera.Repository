namespace Fluxera.Repository
{
	using System;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract that provides the name of a database for a given aggregate root type.
	/// </summary>
	[PublicAPI]
	public interface IDatabaseNameProvider
	{
		/// <summary>
		///     Acquires the database name for the given type.
		/// </summary>
		/// <param name="aggregateRootType"></param>
		/// <returns></returns>
		string GetDatabaseName(Type aggregateRootType);
	}
}
