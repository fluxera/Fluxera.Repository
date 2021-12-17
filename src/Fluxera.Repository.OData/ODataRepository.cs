namespace Fluxera.Repository.OData
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Net;
	using System.Reflection;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Options;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Traits;
	using Fluxera.Utilities.Extensions;
	using Microsoft.Extensions.Logging;
	using Simple.OData.Client;

	/// <summary>
	///     http://odata.github.io/odata.net/#04-01-basic-crud-operations
	/// </summary>
	internal class ODataRepository<TAggregateRoot> : IRepository<TAggregateRoot>
		where TAggregateRoot : AggregateRoot<TAggregateRoot>
	{
		private readonly ODataClient client;
		private readonly ILogger logger;
		private readonly ODataPersistenceSettings persistenceSettings;

		static ODataRepository()
		{
			V4Adapter.Reference();
		}

		public ODataRepository(ILoggerFactory loggerFactory, IRepositoryRegistry repositoryRegistry)
		{
			this.logger = loggerFactory.CreateLogger(Name);

			RepositoryName repositoryName = repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();
			RepositoryOptions options = repositoryRegistry.GetRepositoryOptionsFor(repositoryName);

			this.persistenceSettings = new ODataPersistenceSettings
			{
				ServiceRoot = (string)options.SettingsValues.GetOrDefault("OData.ServiceRoot"),
				UseBatching = (bool)options.SettingsValues.GetOrDefault("OData.UseBatching"),
			};

			// TODO: Would be nice, if the authentication would work just as in the HttpClient module.
			ODataClientSettings settings = new ODataClientSettings
			{
				BaseUri = new Uri(this.persistenceSettings.ServiceRoot),
				IgnoreUnmappedProperties = true,
				RenewHttpConnection = false,
				BeforeRequest = request =>
				{
					// TODO: Authentication
				},
				OnTrace = (message, parameters) =>
				{
					if((parameters != null) && (parameters.Length > 0))
					{
						this.logger.LogTrace(string.Format(message, parameters));
					}
					else
					{
						this.logger.LogTrace(message);
					}
				},
			};

			this.client = new ODataClient(settings);
		}

		private static string Name => "Fluxera.Repository.ODataRepository";

		private bool IsDisposed { get; set; }

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			TAggregateRoot result = await this.client
				.For<TAggregateRoot>()
				.Set(item)
				.InsertEntryAsync(cancellationToken)
				.ConfigureAwait(false);

			item.ID = result.ID;
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot>.AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.ExecuteBatchAsync(items, async (batchClient, item) =>
			{
				TAggregateRoot result = await batchClient
					.For<TAggregateRoot>()
					.Set(item)
					.InsertEntryAsync(cancellationToken)
					.ConfigureAwait(false);

				item.ID = result.ID;
			}, cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.client
				.For<TAggregateRoot>()
				.Key(item.ID)
				.Set(item)
				.UpdateEntryAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.ExecuteBatchAsync(items, async (batchClient, item) =>
			{
				await batchClient
					.For<TAggregateRoot>()
					.Key(item.ID)
					.Set(item)
					.UpdateEntryAsync(cancellationToken)
					.ConfigureAwait(false);
			}, cancellationToken);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.client
				.For<TAggregateRoot>()
				.Key(item.ID)
				.DeleteEntryAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(string id, CancellationToken cancellationToken)
		{
			await this.client
				.For<TAggregateRoot>()
				.Key(id)
				.DeleteEntryAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			await this.client
				.For<TAggregateRoot>()
				.Filter(predicate)
				.DeleteEntryAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.ExecuteBatchAsync(items, async (batchClient, item) =>
			{
				await batchClient
					.For<TAggregateRoot>()
					.Key(item.ID)
					.DeleteEntryAsync(cancellationToken)
					.ConfigureAwait(false);
			}, cancellationToken);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot>.GetAsync(string id, CancellationToken cancellationToken)
		{
			try
			{
				return await this.client
					.For<TAggregateRoot>()
					.Key(id)
					.FindEntryAsync(cancellationToken)
					.ConfigureAwait(false);
			}
			catch(WebRequestException ex) when(ex.Code == HttpStatusCode.NotFound)
			{
				return null!;
			}
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot>.GetAsync<TResult>(string id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			try
			{
				Expression<Func<TAggregateRoot, object>> objectSelector = ConvertSelector(selector);
				TAggregateRoot item = await this.client
					.For<TAggregateRoot>()
					.Key(id)
					.Select(objectSelector)
					.FindEntryAsync(cancellationToken)
					.ConfigureAwait(false);

				// HACK: Implement property selection on server side.
				return selector.Compile().Invoke(item);
			}
			catch(WebRequestException ex) when(ex.Code == HttpStatusCode.NotFound)
			{
				return default!;
			}
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot>.ExistsAsync(string id, CancellationToken cancellationToken)
		{
			return await this.client
				.For<TAggregateRoot>()
				.Key(id)
				.Count()
				.FindScalarAsync<long>(cancellationToken)
				.ConfigureAwait(false) > 0;
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				return await this.client
					.For<TAggregateRoot>()
					.Filter(predicate)
					.ApplyOptions(queryOptions)
					.FindEntryAsync(cancellationToken)
					.ConfigureAwait(false);
			}
			catch(WebRequestException ex) when(ex.Code == HttpStatusCode.NotFound)
			{
				return null!;
			}
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				Expression<Func<TAggregateRoot, object>> objectSelector = ConvertSelector(selector);
				TAggregateRoot item = await this.client
					.For<TAggregateRoot>()
					.Filter(predicate)
					.ApplyOptions(queryOptions)
					.Select(objectSelector)
					.FindEntryAsync(cancellationToken)
					.ConfigureAwait(false);

				// HACK: Implement property selection on server side.
				return selector.Compile().Invoke(item);
			}
			catch(WebRequestException ex) when(ex.Code == HttpStatusCode.NotFound)
			{
				return default!;
			}
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.client
				.For<TAggregateRoot>()
				.Filter(predicate)
				.Count()
				.FindScalarAsync<long>(cancellationToken)
				.ConfigureAwait(false) > 0;
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				IEnumerable<TAggregateRoot> items = await this.client
					.For<TAggregateRoot>()
					.Filter(predicate)
					.ApplyOptions(queryOptions)
					.FindEntriesAsync(cancellationToken)
					.ConfigureAwait(false);

				return items.AsReadOnly();
			}
			catch(WebRequestException ex) when(ex.Code == HttpStatusCode.NotFound)
			{
				return Enumerable.Empty<TAggregateRoot>().AsReadOnly();
			}
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				Expression<Func<TAggregateRoot, object>> objectSelector = ConvertSelector(selector);
				IEnumerable<TAggregateRoot> items = await this.client
					.For<TAggregateRoot>()
					.Filter(predicate)
					.ApplyOptions(queryOptions)
					.Select(objectSelector)
					.FindEntriesAsync(cancellationToken)
					.ConfigureAwait(false);

				Func<TAggregateRoot, TResult> selectorFunc = selector.Compile();

				IList<TResult> result = new List<TResult>();
				foreach(TAggregateRoot item in items)
				{
					// HACK: Implement property selection on server side.
					result.Add(selectorFunc.Invoke(item));
				}

				return result.AsReadOnly();
			}
			catch(WebRequestException ex) when(ex.Code == HttpStatusCode.NotFound)
			{
				return Enumerable.Empty<TResult>().AsReadOnly();
			}
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(CancellationToken cancellationToken)
		{
			return await this.client
				.For<TAggregateRoot>()
				.Count()
				.FindScalarAsync<long>(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.client
				.For<TAggregateRoot>()
				.Filter(predicate)
				.Count()
				.FindScalarAsync<long>(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			this.IsDisposed = true;
		}

		/// <inheritdoc />
		bool IReadOnlyRepository<TAggregateRoot>.IsDisposed => this.IsDisposed;

		/// <inheritdoc />
		ValueTask IAsyncDisposable.DisposeAsync()
		{
			try
			{
				this.Dispose();
				return default;
			}
			catch(Exception exception)
			{
				return new ValueTask(Task.FromException(exception));
			}
		}

		private static Expression<Func<TAggregateRoot, object>> ConvertSelector<TResult>(
			Expression<Func<TAggregateRoot, TResult>> selector)
		{
			MemberExpression memberExpression = GetMemberInfo(selector);
			ParameterExpression param = Expression.Parameter(typeof(TAggregateRoot), "x");
			MemberExpression field = Expression.PropertyOrField(param, memberExpression.Member.Name);

			Expression body = field;
			if(field.Type.GetTypeInfo().IsValueType)
			{
				body = Expression.Convert(field, typeof(object));
			}

			Expression<Func<TAggregateRoot, object>> orderExpression =
				Expression.Lambda<Func<TAggregateRoot, object>>(body, param);
			return (Expression<Func<TAggregateRoot, object>>)Unquote(orderExpression);
		}

		private static MemberExpression GetMemberInfo(Expression method)
		{
			LambdaExpression lambda = method as LambdaExpression;
			Guard.Against.Null(method, nameof(method));

			MemberExpression memberExpr = null;

			if((lambda != null) && (lambda.Body.NodeType == ExpressionType.Convert))
			{
				memberExpr = ((UnaryExpression)lambda.Body).Operand as MemberExpression;
			}
			else if((lambda != null) && (lambda.Body.NodeType == ExpressionType.MemberAccess))
			{
				memberExpr = lambda.Body as MemberExpression;
			}

			Guard.Against.Null(memberExpr, nameof(method));

			return memberExpr;
		}

		private static Expression Unquote(Expression quote)
		{
			if(quote.NodeType == ExpressionType.Quote)
			{
				return Unquote(((UnaryExpression)quote).Operand);
			}

			return quote;
		}

		private async Task ExecuteBatchAsync(IEnumerable<TAggregateRoot> items, Func<IODataClient, TAggregateRoot, Task> converter, CancellationToken cancellationToken)
		{
			if(this.persistenceSettings.UseBatching)
			{
				ODataBatch batch = new ODataBatch(this.client);

				IList<TAggregateRoot> itemsList = items.ToList();

				// Insert in batches of max. 50 requests.
				int batchSize = 25;

				int entryCount = 0;
				foreach(TAggregateRoot item in itemsList)
				{
					entryCount++;
					batch += async batchClient =>
					{
						await converter.Invoke(batchClient, item);
					};

					if(((entryCount % batchSize) == 0) || (entryCount == itemsList.Count))
					{
						await batch.ExecuteAsync(cancellationToken).ConfigureAwait(false);
						batch = new ODataBatch(this.client);
					}
				}
			}
			else
			{
				foreach(TAggregateRoot item in items)
				{
					await converter.Invoke(this.client, item);
				}
			}
		}
	}
}
