// ReSharper disable PossibleMultipleEnumeration

namespace Fluxera.Repository
{
	using System;
	using System.Collections.Generic;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using static Fluxera.Guards.ExceptionHelpers;

	[PublicAPI]
	internal static class GuardExtensions
	{
		public static void NotTransient<TAggregateRoot, TKey>(this IGuard guard, TAggregateRoot? input, [InvokerParameterName] string parameterName, string? message = null)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			Guard.Against.Null(input, nameof(input));

			if(!input!.IsTransient)
			{
				throw CreateArgumentException(parameterName, message);
			}
		}

		public static void NotTransient<TAggregateRoot, TKey>(this IGuard guard, IEnumerable<TAggregateRoot>? input, [InvokerParameterName] string parameterName, string? message = null)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			Guard.Against.Null(input, nameof(input));

			foreach(TAggregateRoot item in input!)
			{
				Guard.Against.NotTransient<TAggregateRoot, TKey>(item, parameterName, message);
			}
		}

		public static void Transient<TAggregateRoot, TKey>(this IGuard guard, TAggregateRoot? input, [InvokerParameterName] string parameterName, string? message = null)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			Guard.Against.Null(input, nameof(input));

			if(input!.IsTransient)
			{
				throw CreateArgumentException(parameterName, message);
			}
		}

		public static void Transient<TAggregateRoot, TKey>(this IGuard guard, IEnumerable<TAggregateRoot> input, [InvokerParameterName] string parameterName, string? message = null)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			Guard.Against.Null(input, nameof(input));

			foreach(TAggregateRoot item in input)
			{
				Guard.Against.Transient<TAggregateRoot, TKey>(item, parameterName, message);
			}
		}

		public static void Disposed<TAggregateRoot, TKey>(this IGuard guard, IRepository<TAggregateRoot, TKey>? input)
			where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		{
			Guard.Against.Null(input, nameof(input));

			if(input!.IsDisposed)
			{
				throw new ObjectDisposedException(input.ToString(), "The repository instance was already disposed.");
			}
		}
	}
}
