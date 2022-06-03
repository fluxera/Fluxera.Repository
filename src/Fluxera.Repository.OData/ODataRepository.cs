namespace Fluxera.Repository.OData
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Net;
	using System.Net.Http;
	using System.Reflection;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Options;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using Fluxera.Utilities.Extensions;
	using Simple.OData.Client;

	/// <summary>
	///     http://odata.github.io/odata.net/#04-01-basic-crud-operations
	/// </summary>
	internal class ODataRepository<TAggregateRoot, TKey> : RepositoryBase<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly ODataClient client;
		private readonly ODataPersistenceSettings persistenceSettings;

		static ODataRepository()
		{
			V4Adapter.Reference();
		}

		public ODataRepository(IRepositoryRegistry repositoryRegistry, IHttpClientFactory httpClientFactory)
		{
			RepositoryName repositoryName = repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();
			RepositoryOptions options = repositoryRegistry.GetRepositoryOptionsFor(repositoryName);

			this.persistenceSettings = new ODataPersistenceSettings
			{
				ServiceRoot = (string)options.Settings.GetOrDefault("OData.ServiceRoot"),
				UseBatching = (bool)options.Settings.GetOrDefault("OData.UseBatching"),
			};

			HttpClient httpClient = httpClientFactory.CreateClient(Microsoft.Extensions.Options.Options.DefaultName);
			ODataClientSettings settings = new ODataClientSettings(httpClient)
			{
				BaseUri = new Uri(this.persistenceSettings.ServiceRoot),
				IgnoreUnmappedProperties = true,
				RenewHttpConnection = false
			};

			this.client = new ODataClient(settings);
		}

		private static string Name => "Fluxera.Repository.ODataRepository";

		/// <inheritdoc />
		public override string ToString()
		{
			return Name;
		}

		/// <inheritdoc />
		protected override async Task AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			TAggregateRoot result = await this.client
				.For<TAggregateRoot>()
				.Set(item)
				.InsertEntryAsync(cancellationToken)
				.ConfigureAwait(false);

			item.ID = result.ID;
		}

		/// <inheritdoc />
		protected override async Task AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
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
		protected override async Task RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			await this.client
				.For<TAggregateRoot>()
				.Filter(specification.Predicate)
				.DeleteEntryAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
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
		protected override async Task UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.client
				.For<TAggregateRoot>()
				.Key(item.ID)
				.Set(item)
				.UpdateEntryAsync(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override async Task UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
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
		protected override async Task<TAggregateRoot> FindOneAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				return await this.client
					.For<TAggregateRoot>()
					.Filter(specification.Predicate)
					.Apply(queryOptions)
					.FindEntryAsync(cancellationToken)
					.ConfigureAwait(false);
			}
			catch(WebRequestException ex) when(ex.Code == HttpStatusCode.NotFound)
			{
				return null;
			}
		}

		/// <inheritdoc />
		protected override async Task<TResult> FindOneAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				Expression<Func<TAggregateRoot, object>> objectSelector = ConvertSelector(selector);
				TAggregateRoot item = await this.client
					.For<TAggregateRoot>()
					.Filter(specification.Predicate)
					.Apply(queryOptions)
					.Select(objectSelector)
					.FindEntryAsync(cancellationToken)
					.ConfigureAwait(false);

				// HACK: Implement property selection on server side.
				return selector.Compile().Invoke(item);
			}
			catch(WebRequestException ex) when(ex.Code == HttpStatusCode.NotFound)
			{
				return default;
			}
		}

		/// <inheritdoc />
		protected override async Task<IReadOnlyCollection<TAggregateRoot>> FindManyAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				IEnumerable<TAggregateRoot> items = await this.client
					.For<TAggregateRoot>()
					.Filter(specification.Predicate)
					.Apply(queryOptions)
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
		protected override async Task<IReadOnlyCollection<TResult>> FindManyAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			try
			{
				Expression<Func<TAggregateRoot, object>> objectSelector = ConvertSelector(selector);
				IEnumerable<TAggregateRoot> items = await this.client
					.For<TAggregateRoot>()
					.Filter(specification.Predicate)
					.Apply(queryOptions)
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
		protected override async Task<long> LongCountAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			return await this.client
				.For<TAggregateRoot>()
				.Filter(specification.Predicate)
				.Count()
				.FindScalarAsync<long>(cancellationToken)
				.ConfigureAwait(false);
		}

		/// <inheritdoc />
		protected override Task<int> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The sum aggregate is not supported.");
		}

		/// <inheritdoc />
		protected override Task<int> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The sum aggregate is not supported.");
		}

		/// <inheritdoc />
		protected override Task<long> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The sum aggregate is not supported.");
		}

		/// <inheritdoc />
		protected override Task<long> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The sum aggregate is not supported.");
		}

		/// <inheritdoc />
		protected override Task<decimal> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The sum aggregate is not supported.");
		}

		/// <inheritdoc />
		protected override Task<decimal> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The sum aggregate is not supported.");
		}

		/// <inheritdoc />
		protected override Task<float> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The sum aggregate is not supported.");
		}

		/// <inheritdoc />
		protected override Task<float> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The sum aggregate is not supported.");
		}

		/// <inheritdoc />
		protected override Task<double> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The sum aggregate is not supported.");
		}

		/// <inheritdoc />
		protected override Task<double> SumAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The sum aggregate is not supported.");
		}

		/// <inheritdoc />
		protected override Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The average aggregate is not supported.");
		}

		/// <inheritdoc />
		protected override Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, int?>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The average aggregate is not supported.");
		}

		/// <inheritdoc />
		protected override Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The average aggregate is not supported.");
		}

		/// <inheritdoc />
		protected override Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, long?>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The average aggregate is not supported.");
		}

		/// <inheritdoc />
		protected override Task<decimal> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The average aggregate is not supported.");
		}

		/// <inheritdoc />
		protected override Task<decimal> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, decimal?>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The average aggregate is not supported.");
		}

		/// <inheritdoc />
		protected override Task<float> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The average aggregate is not supported.");
		}

		/// <inheritdoc />
		protected override Task<float> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, float?>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The average aggregate is not supported.");
		}

		/// <inheritdoc />
		protected override Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The average aggregate is not supported.");
		}

		/// <inheritdoc />
		protected override Task<double> AverageAsync(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, double?>> selector, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The average aggregate is not supported.");
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
			Guard.Against.Null(method, nameof(method));

			LambdaExpression lambda = method as LambdaExpression;
			MemberExpression memberExpr = null;

			if(lambda != null && lambda.Body.NodeType == ExpressionType.Convert)
			{
				memberExpr = ((UnaryExpression)lambda.Body).Operand as MemberExpression;
			}
			else if(lambda != null && lambda.Body.NodeType == ExpressionType.MemberAccess)
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

					if(entryCount % batchSize == 0 || entryCount == itemsList.Count)
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
