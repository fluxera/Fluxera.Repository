namespace Fluxera.Repository.Query
{
	using System;
	using System.Linq.Expressions;

	public interface IOrderByExpression<T> where T : class
	{
		Expression<Func<T, object>> SortExpression { get; }

		bool IsDescending { get; }
	}
}
