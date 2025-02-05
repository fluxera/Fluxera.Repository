// ReSharper disable PossibleMultipleEnumeration

namespace Fluxera.Repository.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository;
	using JetBrains.Annotations;
	using static Fluxera.Guards.ExceptionHelpers;

	[PublicAPI]
	internal static class GuardExtensions
	{
		public static void NotTransient<TEntity, TKey>(this IGuard guard, TEntity input, [InvokerParameterName][CallerArgumentExpression(nameof(input))] string parameterName = null, string message = null)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			Guard.Against.Null(input);

			if(!input.IsTransient)
			{
				throw CreateArgumentException(parameterName, message);
			}
		}

		public static void NotTransient<TEntity, TKey>(this IGuard guard, IEnumerable<TEntity> input, [InvokerParameterName][CallerArgumentExpression(nameof(input))] string parameterName = null, string message = null)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			Guard.Against.Null(input);

			foreach(TEntity item in input)
			{
				Guard.Against.NotTransient<TEntity, TKey>(item, parameterName, message);
			}
		}

		public static void Transient<TEntity, TKey>(this IGuard guard, TEntity input, [InvokerParameterName][CallerArgumentExpression(nameof(input))] string parameterName = null, string message = null)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			Guard.Against.Null(input);

			if(input.IsTransient)
			{
				throw CreateArgumentException(parameterName, message);
			}
		}

		public static void Transient<TEntity, TKey>(this IGuard guard, IEnumerable<TEntity> input, [InvokerParameterName][CallerArgumentExpression(nameof(input))] string parameterName = null, string message = null)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			Guard.Against.Null(input);

			foreach(TEntity item in input)
			{
				Guard.Against.Transient<TEntity, TKey>(item, parameterName, message);
			}
		}

		public static void Disposed<TEntity, TKey>(this IGuard guard, IRepository<TEntity, TKey> input)
			where TEntity : Entity<TEntity, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			Guard.Against.Null(input);

			if(input.IsDisposed)
			{
				throw new ObjectDisposedException(input.ToString(), "The repository instance was already disposed.");
			}
		}
	}
}
