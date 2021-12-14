namespace Fluxera.Repository.Query
{
	using System;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IThenByOptions<T> where T : class
	{
		IThenByOptions<T> ThenBy(Expression<Func<T, object>> sortExpression);

		IThenByOptions<T> ThenByDescending(Expression<Func<T, object>> sortExpression);
	}
}
