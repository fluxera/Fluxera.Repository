namespace Fluxera.Repository.Options
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Fluxera.Guards;
	using Fluxera.Repository.Interception;
	using Microsoft.Extensions.DependencyInjection;

	internal sealed class InterceptionOptionsBuilder : IInterceptionOptionsBuilder
	{
		private readonly IServiceCollection services;

		public InterceptionOptionsBuilder(IServiceCollection services)
		{
			this.services = services;
		}

		/// <inheritdoc />
		public IInterceptionOptionsBuilder AddInterceptors(IEnumerable<Assembly>? assemblies)
		{
			assemblies ??= Enumerable.Empty<Assembly>();

			foreach(Assembly assembly in assemblies)
			{
				this.AddInterceptors(assembly);
			}

			return this;
		}

		/// <inheritdoc />
		public IInterceptionOptionsBuilder AddInterceptors(Assembly assembly)
		{
			Guard.Against.Null(assembly, nameof(assembly));

			foreach(Type type in assembly.GetTypes())
			{
				bool isInterceptor = type.GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == typeof(IInterceptor<,>)));
				if(isInterceptor && !type.IsAbstract && !type.IsInterface)
				{
					foreach(Type interceptorServiceType in type.GetInterfaces().Where(x => x.IsGenericType && (x.GetGenericTypeDefinition() == typeof(IInterceptor<,>))))
					{
						if(this.services.All(x => x.ImplementationType != type))
						{
							this.services.AddTransient(interceptorServiceType, type);
						}
					}
				}
			}

			return this;
		}
	}
}
