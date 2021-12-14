namespace Fluxera.Repository
{
	using System;
	using System.Collections.Generic;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using static Guards.ExceptionHelpers;

	[PublicAPI]
	public static class GuardExtensions
	{
		public static void NotTransient<TAggregateRoot>(this IGuard guard, TAggregateRoot? input, [InvokerParameterName] string parameterName, string? message = null)
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			Guard.Against.Null(input, nameof(input));

			if(!input.IsTransient)
			{
				throw CreateArgumentException(parameterName, message);
			}
		}

		public static void NotTransient<TAggregateRoot>(this IGuard guard, IEnumerable<TAggregateRoot>? input, [InvokerParameterName] string parameterName, string? message = null)
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			Guard.Against.Null(input, nameof(input));

			foreach(TAggregateRoot item in input)
			{
				Guard.Against.NotTransient(item, parameterName, message);
			}
		}

		public static void Transient<TAggregateRoot>(this IGuard guard, TAggregateRoot? input, [InvokerParameterName] string parameterName, string? message = null)
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			Guard.Against.Null(input, nameof(input));

			if(input.IsTransient)
			{
				throw CreateArgumentException(parameterName, message);
			}
		}

		public static void Transient<TAggregateRoot>(this IGuard guard, IEnumerable<TAggregateRoot> input, [InvokerParameterName] string parameterName, string? message = null)
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			Guard.Against.Null(input, nameof(input));

			foreach(TAggregateRoot item in input)
			{
				Guard.Against.Transient(item, parameterName, message);
			}
		}

		public static void Disposed<TAggregateRoot>(this IGuard guard, IRepository<TAggregateRoot>? input)
			where TAggregateRoot : AggregateRoot<TAggregateRoot>
		{
			Guard.Against.Null(input, nameof(input));

			if(input.IsDisposed)
			{
				throw new ObjectDisposedException(input.ToString(), "The repository instance was already disposed.");
			}
		}
	}
}
