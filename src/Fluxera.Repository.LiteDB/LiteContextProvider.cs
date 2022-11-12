namespace Fluxera.Repository.LiteDB
{
	using System;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class LiteContextProvider : ContextProviderBase<LiteContext>
	{
		/// <inheritdoc />
		public LiteContextProvider(
			IServiceProvider serviceProvider,
			IRepositoryRegistry repositoryRegistry)
			: base("Lite.Context", serviceProvider, repositoryRegistry)
		{
		}
	}

	//public sealed class LiteContextOptions<TContext> : LiteContextOptions
	//{
	//}

	//public abstract class LiteContextOptions
	//{
	//	public Type ContextType { get; set; }
	//}

	//public sealed class LiteContextOptionsBuilder
	//{
	//}
}
