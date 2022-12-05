namespace Fluxera.Repository.LiteDB
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using Fluxera.Repository.Query;
	using global::LiteDB.Queryable;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class LiteIncludeApplier : IIncludeApplier
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
