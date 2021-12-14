namespace Fluxera.Repository.Decorators
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Extensions.Validation;
	using Fluxera.Guards;
	using Fluxera.Repository.Options;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Traits;

	public sealed class ValidationRepositoryDecorator<TAggregateRoot> : IRepository<TAggregateRoot>
		where TAggregateRoot : AggregateRoot<TAggregateRoot>
	{
		private readonly IRepository<TAggregateRoot> innerRepository;

		private readonly RepositoryOptions repositoryOptions;
		private readonly IEnumerable<IValidator> validators;

		public ValidationRepositoryDecorator(IRepository<TAggregateRoot> innerRepository, IRepositoryRegistry repositoryRegistry, IServiceProvider serviceProvider)
		{
			Guard.Against.Null(innerRepository, nameof(innerRepository));
			Guard.Against.Null(repositoryRegistry, nameof(repositoryRegistry));
			Guard.Against.Null(serviceProvider, nameof(serviceProvider));

			this.innerRepository = innerRepository;

			// Get the repository options.
			RepositoryName? repositoryName = repositoryRegistry.GetRepositoryNameFor<TAggregateRoot>();
			this.repositoryOptions = repositoryRegistry.GetRepositoryOptionsFor(repositoryName);

			// Initialize validators.
			ICollection<IValidator> validatorCollection = new List<IValidator>();
			if(this.repositoryOptions.ValidationOptions.IsEnabled)
			{
				IEnumerable<IValidatorFactory> validatorFactories = serviceProvider.GetNamedServices<IValidatorFactory>((string)repositoryName);
				foreach(IValidatorFactory validatorFactory in validatorFactories)
				{
					IValidator validator = validatorFactory.CreateValidator();
					validatorCollection.Add(validator);
				}
			}

			this.validators = validatorCollection;
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot>.AddAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.ValidateAsync(item);

			await this.innerRepository.AddAsync(item, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanAdd<TAggregateRoot>.AddAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.ValidateAsync(items);

			await this.innerRepository.AddAsync(items, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.ValidateAsync(item);

			await this.innerRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanUpdate<TAggregateRoot>.UpdateAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.ValidateAsync(items);

			await this.innerRepository.UpdateAsync(items, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(TAggregateRoot item, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveAsync(item, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(string id, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveAsync(id, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveAsync(predicate, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task ICanRemove<TAggregateRoot>.RemoveAsync(IEnumerable<TAggregateRoot> items, CancellationToken cancellationToken)
		{
			await this.innerRepository.RemoveAsync(items, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanGet<TAggregateRoot>.GetAsync(string id, CancellationToken cancellationToken)
		{
			return await this.innerRepository.GetAsync(id, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanGet<TAggregateRoot>.GetAsync<TResult>(string id, Expression<Func<TAggregateRoot, TResult>> selector, CancellationToken cancellationToken)
		{
			return await this.innerRepository.GetAsync(id, selector, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanGet<TAggregateRoot>.ExistsAsync(string id, CancellationToken cancellationToken)
		{
			return await this.innerRepository.ExistsAsync(id, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TAggregateRoot> ICanFind<TAggregateRoot>.FindOneAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindOneAsync(predicate, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<TResult> ICanFind<TAggregateRoot>.FindOneAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindOneAsync(predicate, selector, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<bool> ICanFind<TAggregateRoot>.ExistsAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.innerRepository.ExistsAsync(predicate, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TAggregateRoot>> ICanFind<TAggregateRoot>.FindManyAsync(Expression<Func<TAggregateRoot, bool>> predicate, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindManyAsync(predicate, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<IReadOnlyCollection<TResult>> ICanFind<TAggregateRoot>.FindManyAsync<TResult>(Expression<Func<TAggregateRoot, bool>> predicate, Expression<Func<TAggregateRoot, TResult>> selector, IQueryOptions<TAggregateRoot>? queryOptions, CancellationToken cancellationToken)
		{
			return await this.innerRepository.FindManyAsync(predicate, selector, queryOptions, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(CancellationToken cancellationToken = default)
		{
			return await this.innerRepository.CountAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc />
		async Task<long> ICanAggregate<TAggregateRoot>.CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
		{
			return await this.innerRepository.CountAsync(predicate, cancellationToken).ConfigureAwait(false);
		}

		private async Task ValidateAsync(TAggregateRoot item)
		{
			if (repositoryOptions.ValidationOptions.IsEnabled)
			{
				ValidationResult validationResult = await this.validators.ValidateAsync(item);
				if (!validationResult.IsValid)
				{
					throw Errors.ItemNotValid(validationResult.ValidationErrors);
				}
			}
		}

		private async Task ValidateAsync(IEnumerable<TAggregateRoot> items)
		{
			if (repositoryOptions.ValidationOptions.IsEnabled)
			{
				foreach(TAggregateRoot item in items)
				{
					ValidationResult validationResult = await this.validators.ValidateAsync(item);
					if (!validationResult.IsValid)
					{
						throw Errors.ItemNotValid(validationResult.ValidationErrors);
					}	
				}
			}
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return this.innerRepository.ToString();
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
		bool IReadOnlyRepository<TAggregateRoot>.IsDisposed => this.innerRepository.IsDisposed;
	}
}
