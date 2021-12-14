namespace Fluxera.Repository.Query{	using System;	using System.Linq.Expressions;	using Fluxera.Utilities.Extensions;	public sealed class OrderByExpression<T> : IOrderByExpression<T>		where T : class	{		public OrderByExpression(Expression<Func<T, object>> sortExpression, bool isDescending)		{			this.SortExpression = sortExpression;			this.IsDescending = isDescending;		}		public Expression<Func<T, object>> SortExpression { get; }		public bool IsDescending { get; }		public override string ToString()		{			return "(Expression: {0}, Descending: {1})".FormatInvariantWith(this.SortExpression, this.IsDescending);		}	}}