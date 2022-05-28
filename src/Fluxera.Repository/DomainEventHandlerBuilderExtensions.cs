namespace Fluxera.Repository
{
	using System;
	using System.Collections.Generic;
	using Fluxera.Entity.DomainEvents;
	using JetBrains.Annotations;

	[PublicAPI]
	internal static class DomainEventHandlerBuilderExtensions
	{
		public static void AddDomainEventHandlers(this DomainEventHandlerBuilder builder, Func<IReadOnlyCollection<Type>> func)
		{
			IReadOnlyCollection<Type> types = func.Invoke();
			builder.AddDomainEventHandlers(types);
		}
	}
}
