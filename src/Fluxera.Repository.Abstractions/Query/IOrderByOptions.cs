namespace Fluxera.Repository.Query
{
	using System;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IOrderByOptions<T> where T : class
	{
		IOrderByExpression<T> OrderByExpression { get; }

		IOrderByExpression<T>[]? ThenByExpressions { get; }
		IThenByOptions<T> OrderBy(Expression<Func<T, object>> orderByExpression);

		IThenByOptions<T> OrderByDescending(Expression<Func<T, object>> orderByExpression);
	}
}
