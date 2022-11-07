namespace Fluxera.Repository
{
	using System;
	using System.Collections.Concurrent;
	using Fluxera.Guards;
	using Fluxera.Repository.Options;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     Base class for storage specific context provider implementations.
	/// </summary>
	/// <typeparam name="TContextBase"></typeparam>
	[PublicAPI]
	public abstract class ContextProviderBase<TContextBase>
	{
		private readonly string contextTypeSettingsKey;
		private readonly ConcurrentDictionary<RepositoryName, Type> dbContextMap = new ConcurrentDictionary<RepositoryName, Type>();

		private readonly IRepositoryRegistry repositoryRegistry;
		private readonly IServiceProvider serviceProvider;

		/// <summary>
		///     Initializes a new instance of the <see cref="ContextProviderBase{T}" /> type.
		/// </summary>
		/// <param name="contextTypeSettingsKey"></param>
		/// <param name="serviceProvider"></param>
		/// <param name="repositoryRegistry"></param>
		protected ContextProviderBase(
			string contextTypeSettingsKey,
			IServiceProvider serviceProvider,
			IRepositoryRegistry repositoryRegistry)
		{
			this.contextTypeSettingsKey = contextTypeSettingsKey;
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
			TContextBase dbContext = this.dbContextMap.TryGetValue(repositoryName, out Type dbContextType)
				? this.GetContext(dbContextType)
				: this.RegisterContextType(repositoryName);

			return dbContext;
		}

		private TContextBase RegisterContextType(RepositoryName repositoryName)
		{
			RepositoryOptions options = this.repositoryRegistry.GetRepositoryOptionsFor(repositoryName);
			Type dbContextType = options.Settings.GetOrDefault(this.contextTypeSettingsKey) as Type;

			Guard.Against.Null(dbContextType, nameof(dbContextType));

			if(!this.dbContextMap.TryAdd(repositoryName, dbContextType))
			{
				throw new InvalidOperationException($"Could not add MongoContext type for repository '{repositoryName}'.");
			}

			return this.GetContext(dbContextType);
		}

		private TContextBase GetContext(Type dbContextType)
		{
			TContextBase dbContext = (TContextBase)this.serviceProvider.GetRequiredService(dbContextType);
			return dbContext;
		}
	}
}
