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
		public IInterceptionOptionsBuilder AddInterceptors(IEnumerable<Assembly> assemblies)
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
				this.AddInterceptor(type);
			}

			return this;
		}

		/// <inheritdoc />
		public IInterceptionOptionsBuilder AddInterceptor(Type interceptorType)
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

		/// <inheritdoc />
		public IInterceptionOptionsBuilder AddInterceptor<TInterceptor>()
			where TInterceptor : IInterceptor
		{
			return this.AddInterceptor(typeof(TInterceptor));
		}
	}
}
