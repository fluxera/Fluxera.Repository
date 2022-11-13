namespace Fluxera.Repository.InMemory
{
	using System;
	using System.Collections.Concurrent;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Guards;
	using Fluxera.Utilities;
	using JetBrains.Annotations;

	/// <summary>
	///     A base class for context implementations for the in-memory repository.
	/// </summary>
	[PublicAPI]
	public abstract class InMemoryContext : Disposable
	{
		private ConcurrentQueue<Func<Task>> commands;

		/// <summary>
		///     Initializes a new instance of the <see cref="InMemoryContext" /> type.
		/// </summary>
		protected InMemoryContext()
		{
			// Command will be stored and later processed on saving changes.
			this.commands = new ConcurrentQueue<Func<Task>>();
		}

		/// <summary>
		///     Adds a command for execution.
		/// </summary>
		/// <param name="command"></param>
		public Task AddCommandAsync(Func<Task> command)
		{
			Guard.Against.Null(command);

			this.commands ??= new ConcurrentQueue<Func<Task>>();
			this.commands.Enqueue(command);

			return Task.CompletedTask;
		}

		/// <summary>
		///     Saves the changes made by the registered commands.
		/// </summary>
		/// <returns></returns>
		public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				if(!this.commands.Any())
				{
					return;
				}

				await this.ExecuteCommands(cancellationToken);
			}
			finally
			{
				this.ClearCommands();
			}
		}

		/// <summary>
		///     Checks if any commands were added for execution.
		/// </summary>
		/// <returns></returns>
		public bool HasChanges()
		{
			return this.commands.Any();
		}

		/// <summary>
		///     Discards all changes.
		/// </summary>
		public void DiscardChanges()
		{
			this.ClearCommands();
		}

		/// <inheritdoc />
		protected override void DisposeManaged()
		{
			this.ClearCommands();
		}

		/// <summary>
		///     Configures the options to use for this context instance over it's lifetime.
		/// </summary>
		/// <param name="contextOptions">The options instance configured with the default settings.</param>
		protected virtual void ConfigureOptions(InMemoryContextOptions contextOptions)
		{
		}

		internal void Configure(RepositoryName repositoryName, IServiceProvider serviceProvider)
		{
			InMemoryContextOptions contextOptions = new InMemoryContextOptions
			{
				Database = string.Empty
			};

			this.ConfigureOptions(contextOptions);
		}

		/// <summary>
		///     Removes all added commands.
		/// </summary>
		private void ClearCommands()
		{
			this.commands?.Clear();
		}

		private async Task ExecuteCommands(CancellationToken cancellationToken)
		{
			foreach(Func<Task> command in this.commands)
			{
				cancellationToken.ThrowIfCancellationRequested();

				await command.Invoke().ConfigureAwait(false);
			}
		}
	}
}
