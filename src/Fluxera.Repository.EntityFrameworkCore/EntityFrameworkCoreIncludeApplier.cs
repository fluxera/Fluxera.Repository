namespace Fluxera.Repository.EntityFrameworkCore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using Fluxera.Repository.Query;
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;

	[UsedImplicitly]
	internal sealed class EntityFrameworkCoreIncludeApplier : IIncludeApplier
	{
		/// <inheritdoc />
		public IQueryable<T> ApplyTo<T>(IQueryable<T> queryable, IEnumerable<Expression<Func<T, object>>> includeExpressions) where T : class
		{
			foreach(Expression<Func<T, object>> includeExpression in includeExpressions)
			{
				queryable = queryable.Include(includeExpression);
			}

			return queryable;
		}
	}
}
