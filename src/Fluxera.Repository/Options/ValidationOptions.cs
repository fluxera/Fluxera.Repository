namespace Fluxera.Repository.Options
{
	using System;
	using System.Collections.Generic;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class ValidationOptions
	{
		public ValidationOptions(string repositoryName)
		{
			this.RepositoryName = repositoryName;
		}

		public string RepositoryName { get; }

		//public IList<Type> ValidatorFactoryTypes { get; } = new List<Type>();

		public bool IsEnabled { get; set; }
	}
}
