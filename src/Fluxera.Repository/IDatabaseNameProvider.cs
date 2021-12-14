namespace Fluxera.Repository
{
	using System;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IDatabaseNameProvider
	{
		string GetDatabaseName(Type aggregateRootType);
	}
}
