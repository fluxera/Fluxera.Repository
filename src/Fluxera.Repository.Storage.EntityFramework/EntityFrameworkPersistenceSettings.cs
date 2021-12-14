﻿namespace Fluxera.Repository.Storage.EntityFramework
{
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class EntityFrameworkPersistenceSettings
	{
		public string ConnectionString { get; set; }

		public bool LogSQL { get; set; }
	}
}
