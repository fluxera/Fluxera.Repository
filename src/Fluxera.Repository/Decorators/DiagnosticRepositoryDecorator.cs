namespace Fluxera.Repository.Decorators
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.CompilerServices;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Repository.Options;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Specifications;
	using Fluxera.Repository.Traits;
	using Fluxera.Utilities.Extensions;

	/// <summary>
	///     A repository decorator that provides events for diagnostic traces using System.Diagnostics.
	/// </summary>
	/// <typeparam name="TAggregateRoot"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	public sealed class DiagnosticRepositoryDecorator<TAggregateRoot, TKey> : IRepository<TAggregateRoot, TKey>
		where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
	{
		private static readonly AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
		private static readonly string activitySourceName = assemblyName.Name;
		private static readonly Version version = assemblyName.Version;

		private readonly IRepository<TAggregateRoot, TKey> innerRepository;
		private readonly RepositoryOptions repositoryOptions;

		/// <summary>
		///     Creates a new instance of the <see cref="DiagnosticRepositoryDecorator{TAggregateRoot,TKey}" /> type.
		/// </summary>
		/// <param name="innerRepository"></param>
		/// <param name="repositoryRegistry"></param>
		public DiagnosticRepositoryDecorator(
			IRepository<TAggregateRoot, TKey> innerRepository,
			IRepositoryRegistry repositoryRegistry)
		{
			Guard.Against.Null(innerRepository, nameof(innerRepository));
			Guard.Against.Null(repositoryRegistry, nameof(repositoryRegistry));

			this.innerRepository = innerRepository;

			RepositoryName repositoryName = repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();
			this.repositoryOptions = repositoryRegistry.GetRepositoryOptionsFor(repositoryName);
		}

		private static ActivitySource ActivitySource { get; } = new ActivitySource(activitySourceName, version.ToString());

		private static string ActivityName { get; } = $"{activitySourceName}.Diagnostic";

		private static string AggregateRootName { get; } = typeof(TAggregateRoot).Name;

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.RunDiagnosticAsync(async () => await this.innerRepository.AddAsync(item, cancellationToken));
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot, TKey>.AddRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.RunDiagnosticAsync(async () => await this.innerRepository.AddRangeAsync(items, cancellationToken));
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.RunDiagnosticAsync(async () => await this.innerRepository.UpdateAsync(item, cancellationToken));
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot, TKey>.UpdateRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.RunDiagnosticAsync(async () => await this.innerRepository.UpdateRangeAsync(items, cancellationToken));
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.RunDiagnosticAsync(async () => await this.innerRepository.RemoveAsync(item, cancellationToken));
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveAsync(TKey id, CancellationToken cancellationToken)
		{
			await this.RunDiagnosticAsync(async () => await this.innerRepository.RemoveAsync(id, cancellationToken));
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveRangeAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			await this.RunDiagnosticAsync(async () => await this.innerRepository.RemoveRangeAsync(predicate, cancellationToken));
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveRangeAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			await this.RunDiagnosticAsync(async () => await this.innerRepository.RemoveRangeAsync(specification, cancellationToken));
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot, TKey>.RemoveRangeAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.RunDiagnosticAsync(async () => await this.innerRepository.RemoveRangeAsync(items, cancellationToken));
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot, TKey>.GetAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.GetAsync(id, cancellationToken));
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot, TKey>.GetAsync<TResult>(TKey id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.GetAsync(id, selector, cancellationToken));
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot, TKey>.ExistsAsync(TKey id, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.ExistsAsync(id, cancellationToken));
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.FindOneAsync(predicate, queryOptions, cancellationToken));
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot, TKey>.FindOneAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.FindOneAsync(specification, queryOptions, cancellationToken));
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.FindOneAsync(predicate, selector, queryOptions, cancellationToken));
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot, TKey>.FindOneAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.FindOneAsync(specification, selector, queryOptions, cancellationToken));
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot, TKey>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.ExistsAsync(predicate, cancellationToken));
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot, TKey>.ExistsAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.ExistsAsync(specification, cancellationToken));
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.FindManyAsync(predicate, queryOptions, cancellationToken));
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot, TKey>.FindManyAsync(ISpecification<TAggregateRoot> specification, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.FindManyAsync(specification, queryOptions, cancellationToken));
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.FindManyAsync(predicate, selector, queryOptions, cancellationToken));
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot, TKey>.FindManyAsync<TResult>(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot> queryOptions, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.FindManyAsync(specification, selector, queryOptions, cancellationToken));
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.CountAsync(cancellationToken));
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot, TKey>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.CountAsync(predicate, cancellationToken));
		}

		/// <inheritdoc />
		public async Task<long> CountAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken = default)
		{
			return await this.RunDiagnosticAsync(async () => await this.innerRepository.CountAsync(specification, cancellationToken));
		}

		/// <inheritdoc />
		void IDisposable.Dispose()
		{
			if(!this.innerRepository.IsDisposed)
			{
				this.innerRepository.Dispose();
			}
		}

		/// <inheritdoc />
		bool IDisposableRepository.IsDisposed => this.innerRepository.IsDisposed;

		/// <inheritdoc />
		async ValueTask IAsyncDisposable.DisposeAsync()
		{
			if(!this.innerRepository.IsDisposed)
			{
				await this.innerRepository.DisposeAsync();
			}
		}

		private async Task<TResult> RunDiagnosticAsync<TResult>(Func<Task<TResult>> func, [CallerMemberName] string callerMemberName = "")
		{
			string commandName = callerMemberName.RemovePostFix("Async") ?? callerMemberName;
			string storageName = this.ToString().RemovePreFix("Fluxera.Repository.") ?? this.ToString();

			Activity activity = StartActivity(commandName, storageName, this.repositoryOptions);
			try
			{
				TResult result = await func.Invoke();
				HandleSuccess(activity);
				return result;
			}
			catch(Exception ex)
			{
				HandleFailure(activity, ex);
				throw;
			}
			finally
			{
				StopActivity(activity);
			}
		}

		private async Task RunDiagnosticAsync(Func<Task> func, [CallerMemberName] string callerMemberName = "")
		{
			string commandName = callerMemberName.RemovePostFix("Async") ?? callerMemberName;
			string storageName = this.ToString().RemovePreFix("Fluxera.Repository.") ?? this.ToString();

			Activity activity = StartActivity(commandName, storageName, this.repositoryOptions);
			try
			{
				await func.Invoke();
				HandleSuccess(activity);
			}
			catch(Exception ex)
			{
				HandleFailure(activity, ex);
				throw;
			}
			finally
			{
				StopActivity(activity);
			}
		}

		private static Activity StartActivity(string commandName, string storageName, RepositoryOptions repositoryOptions)
		{
			Activity activity = ActivitySource.StartActivity(ActivityName);
			if(activity != null)
			{
				activity.DisplayName = $"{commandName} for {AggregateRootName}";

				activity.AddTag("db.system", "repository");
				activity.AddTag("db.name", repositoryOptions.RepositoryName);
				activity.AddTag("db.operation", commandName);

				activity.AddTag("db.repository.storage", storageName);
				activity.AddTag("db.repository.name", repositoryOptions.RepositoryName);
				activity.AddTag("db.repository.operation", commandName);
				activity.AddTag("db.repository.aggregate", AggregateRootName);
				activity.AddTag("db.repository.options.caching.enabled", repositoryOptions.CachingOptions.IsEnabled);
				activity.AddTag("db.repository.options.events.enabled", repositoryOptions.DomainEventsOptions.IsEnabled);
				activity.AddTag("db.repository.options.interception.enabled", repositoryOptions.InterceptionOptions.IsEnabled);
				activity.AddTag("db.repository.options.validation.enabled", repositoryOptions.ValidationOptions.IsEnabled);
			}

			return activity;
		}

		private static void StopActivity(Activity activity)
		{
			activity?.Stop();
		}

		private static void HandleSuccess(Activity activity)
		{
			activity?.AddTag("otel.status_code", "Ok");
		}

		private static void HandleFailure(Activity activity, Exception exception)
		{
			if(activity?.IsAllDataRequested == true)
			{
				activity.AddTag("otel.status_code", "Error");
				activity.AddTag("otel.status_description", exception.Message);
				activity.AddTag("error.type", exception.GetType().FullName);
				activity.AddTag("error.msg", exception.Message);
				activity.AddTag("error.stack", exception.StackTrace);
			}
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return this.innerRepository.ToString();
		}
	}
}
