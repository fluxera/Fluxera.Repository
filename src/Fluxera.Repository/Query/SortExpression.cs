namespace Fluxera.Repository.Query
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;

	/// <summary>
	///     A container hold information about the sort expression and direction.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	[PublicAPI]
	public sealed class SortExpression<T, TValue> : ISortExpression<T> where T : class
	{
		/// <summary>
		///     Creates a new instance of the <see cref="SortExpression{T, TValue}" /> type.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="isDescending"></param>
		public SortExpression(Expression<Func<T, TValue>> expression, bool isDescending)
		{
			this.Expression = expression;
			this.IsDescending = isDescending;
		}

		/// <summary>
		///     The sort property expression.
		/// </summary>
		public Expression<Func<T, TValue>> Expression { get; }

		/// <inheritdoc />
		public LambdaExpression LambdaExpression => this.Expression;

		/// <summary>
		///     Flag, if the sort order is descending.
		/// </summary>
		public bool IsDescending { get; }

		/// <inheritdoc />
		public IOrderedQueryable<T> ApplyTo(IQueryable<T> queryable)
		{
			queryable = this.IsDescending
				? queryable.OrderByDescending(this.Expression)
				: queryable.OrderBy(this.Expression);

			return (IOrderedQueryable<T>)queryable;
		}

		/// <inheritdoc />
		public IQueryable<T> ApplyTo(IOrderedQueryable<T> queryable)
		{
			queryable = this.IsDescending
				? queryable.ThenByDescending(this.Expression)
				: queryable.ThenBy(this.Expression);

			return queryable;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return "(Expression: {0}, Descending: {1})"
				.FormatInvariantWith(this.Expression, this.IsDescending);
		}
	}
}
