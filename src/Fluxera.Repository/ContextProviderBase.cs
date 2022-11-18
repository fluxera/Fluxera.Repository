namespace Fluxera.Repository
{
	using System;
	using System.Collections.Concurrent;
	using Fluxera.Repository.Options;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     Base class for storage specific context provider implementations.
	/// </summary>
	/// <typeparam name="TContextBase"></typeparam>
	[PublicAPI]
	public abstract class ContextProviderBase<TContextBase>
	{
		private readonly ConcurrentDictionary<RepositoryName, Type> contextMap = new ConcurrentDictionary<RepositoryName, Type>();

		private readonly IRepositoryRegistry repositoryRegistry;
		private readonly IServiceProvider serviceProvider;

		/// <summary>
		///     Initializes a new instance of the <see cref="ContextProviderBase{T}" /> type.
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <param name="repositoryRegistry"></param>
		protected ContextProviderBase(
			IServiceProvider serviceProvider,
			IRepositoryRegistry repositoryRegistry)
		{
			this.serviceProvider = serviceProvider;
			this.repositoryRegistry = repositoryRegistry;
		}

		/// <summary>
		///     Gets a storage specific context provider instance.
		/// </summary>
		/// <param name="repositoryName"></param>
		/// <returns></returns>
		public TContextBase GetContextFor(RepositoryName repositoryName)
		{
			TContextBase context = this.contextMap.TryGetValue(repositoryName, out Type contextType)
				? this.GetContext(contextType)
				: this.RegisterContextType(repositoryName);

			this.PerformConfigureContext(context, repositoryName);

			return context;
		}

		private TContextBase RegisterContextType(RepositoryName repositoryName)
		{
			RepositoryOptions options = this.repositoryRegistry.GetRepositoryOptionsFor(repositoryName);

			if(!this.contextMap.TryAdd(repositoryName, options.RepositoryContextType))
			{
				throw new InvalidOperationException($"Could not add context type for repository '{repositoryName}'.");
			}

			return this.GetContext(options.RepositoryContextType);
		}

		private TContextBase GetContext(Type contextType)
		{
			TContextBase context = (TContextBase)this.serviceProvider.GetRequiredService(contextType);
			return context;
		}

		/// <summary>
		///     Performs initialization of the context before usage.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="repositoryName"></param>
		protected abstract void PerformConfigureContext(TContextBase context, RepositoryName repositoryName);
	}
}
