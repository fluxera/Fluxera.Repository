﻿namespace Fluxera.Repository.Query{	using System;	using System.Linq.Expressions;	using JetBrains.Annotations;	[PublicAPI]	public interface IQueryOptions<T> where T : class	{		bool HasPagingOptions { get; }		bool HasSkipTakeOptions { get; }		bool HasOrderByOptions { get; }		bool IsEmpty { get; }		IOrderByOptions<T> Paging(int pageNumber, int pageSize);		IOrderByOptions<T> SkipTake(int skipAmount, int takeAmount);		IOrderByOptions<T> Skip(int skipAmount);		IOrderByOptions<T> Take(int takeAmount);		IThenByOptions<T> OrderBy(Expression<Func<T, object>> sortExpression);		IThenByOptions<T> OrderByDescending(Expression<Func<T, object>> sortExpression);		bool TryGetPagingOptions(out IPagingOptions<T>? pagingOptions);		bool TryGetSkipTakeOptions(out ISkipTakeOptions<T>? skipTakeOptions);		bool TryGetOrderByOptions(out IOrderByOptions<T>? orderByOptions);	}}