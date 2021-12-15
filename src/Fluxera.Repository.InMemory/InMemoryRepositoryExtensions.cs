﻿namespace Fluxera.Repository.InMemory{	using System.Linq;	using Fluxera.Entity;	using Fluxera.Repository.Query;	internal static class InMemoryRepositoryExtensions	{		internal static IQueryable<TAggregateRoot> ApplyOptions<TAggregateRoot>(			this IQueryable<TAggregateRoot> queryable, IQueryOptions<TAggregateRoot>? options)			where TAggregateRoot : AggregateRoot<TAggregateRoot>		{			if(options is null)			{				return queryable;			}			if(options.HasPagingOptions)			{				options.TryGetPagingOptions(out IPagingOptions<TAggregateRoot>? pagingOptions);				queryable = queryable.Skip(pagingOptions.Skip).Take(pagingOptions.PageSize);			}			if(options.HasSkipTakeOptions)			{				options.TryGetSkipTakeOptions(out ISkipTakeOptions<TAggregateRoot>? skipTakeOptions);				if(skipTakeOptions.Skip.HasValue)				{					queryable = queryable.Skip(skipTakeOptions.Skip.Value);				}				if(skipTakeOptions.Take.HasValue)				{					queryable = queryable.Take(skipTakeOptions.Take.Value);				}			}			if(options.HasOrderByOptions)			{				options.TryGetOrderByOptions(out IOrderByOptions<TAggregateRoot>? orderByOptions);				IOrderByExpression<TAggregateRoot> orderBy = orderByOptions.OrderByExpression;				IOrderedQueryable<TAggregateRoot> orderedQueryable = orderBy.IsDescending					? queryable.OrderByDescending(orderBy.SortExpression)					: queryable.OrderBy(orderBy.SortExpression);				if(orderByOptions.ThenByExpressions != null)				{					foreach(IOrderByExpression<TAggregateRoot> thenBy in orderByOptions.ThenByExpressions)					{						orderedQueryable = thenBy.IsDescending							? orderedQueryable.ThenBy(thenBy.SortExpression)							: orderedQueryable.ThenByDescending(thenBy.SortExpression);					}				}				queryable = orderedQueryable;			}			return queryable;		}	}}