namespace Fluxera.Repository.Options
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Fluxera.Guards;
	using Fluxera.Repository.Interception;
	using Fluxera.Utilities.Extensions;
	using Microsoft.Extensions.DependencyInjection;

	internal sealed class InterceptionOptionsBuilder : IInterceptionOptionsBuilder
	{
		private readonly IServiceCollection services;

		public InterceptionOptionsBuilder(IServiceCollection services)
		{
			this.services = services;
		}

		/// <inheritdoc />
		public IInterceptionOptionsBuilder AddInterceptorsFromAssemblies(IEnumerable<Assembly> assemblies)
		{
			assemblies ??= Enumerable.Empty<Assembly>();

			foreach(Assembly assembly in assemblies)
			{
				this.AddInterceptorsFromAssembly(assembly);
			}

			return this;
		}

		/// <inheritdoc />
		public IInterceptionOptionsBuilder AddInterceptorsFromAssembly(Assembly assembly)
		{
			Guard.Against.Null(assembly, nameof(assembly));

			IEnumerable<Type> types = assembly
				.GetTypes()
				.Where(x => x.Implements<IInterceptor>())
				.Where(x => !x.Implements<IDecoratingInterceptor>()); // Do not add the interceptor decorator.

			foreach(Type type in types)
			{
				this.AddInterceptor(type);
			}

			return this;
		}

		private IInterceptionOptionsBuilder AddInterceptor(Type interceptorType)
		{
			bool isInterceptor = interceptorType.GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == typeof(IInterceptor<,>)));
			if(isInterceptor && !interceptorType.IsAbstract && !interceptorType.IsInterface)
			{
				foreach(Type interceptorServiceType in interceptorType.GetInterfaces().Where(x => x.IsGenericType && (x.GetGenericTypeDefinition() == typeof(IInterceptor<,>))))
				{
					if(this.services.All(x => x.ImplementationType != interceptorType))
					{
						this.services.AddTransient(interceptorServiceType, interceptorType);
					}
				}
			}

			return this;
		}
	}
}
