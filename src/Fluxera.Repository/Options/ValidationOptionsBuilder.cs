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
		public IValidationOptionsBuilder AddValidators(IEnumerable<Assembly> assemblies)
		{
			this.services.AddValidatorsFromAssemblies(assemblies);
			return this;
		}

		/// <inheritdoc />
		public IValidationOptionsBuilder AddValidators(Assembly assembly)
		{
			this.services.AddValidatorsFromAssembly(assembly);
			return this;
		}
	}
}
