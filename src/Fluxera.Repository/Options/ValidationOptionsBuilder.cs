namespace Fluxera.Repository.Options
{
	using System.Collections.Generic;
	using System.Reflection;
	using FluentValidation;
	using Microsoft.Extensions.DependencyInjection;

	internal sealed class ValidationOptionsBuilder : IValidationOptionsBuilder
	{
		private readonly IServiceCollection services;

		public ValidationOptionsBuilder(RepositoryOptions repositoryOptions, IServiceCollection services)
		{
			this.RepositoryName = (string)repositoryOptions.RepositoryName;
			this.services = services;
		}

		public string RepositoryName { get; }

		/// <inheritdoc />
		public IValidationOptionsBuilder AddValidatorsFromAssemblies(IEnumerable<Assembly> assemblies)
		{
			this.services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);
			return this;
		}

		/// <inheritdoc />
		public IValidationOptionsBuilder AddValidatorsFromAssembly(Assembly assembly)
		{
			this.services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);
			return this;
		}
	}
}
