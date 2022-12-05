namespace Fluxera.Repository.Query
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     A <see cref="IIncludeApplier" /> implementation that applies nothing
	///     to the <see cref="IQueryable" /> and just write a log entry.
	/// </summary>
	[PublicAPI]
	public class NoopIncludeApplier : IIncludeApplier
	{
		private readonly string storageName;
		private readonly ILogger logger;

		/// <summary>
		///     Initializes a new instance of the <see cref="NoopIncludeApplier" /> type.
		/// </summary>
		/// <param name="loggerFactory"></param>
		/// <param name="storageName"></param>
		public NoopIncludeApplier(ILoggerFactory loggerFactory, string storageName)
		{
			this.storageName = Guard.Against.NullOrWhiteSpace(storageName);
			this.logger = loggerFactory.CreateLogger(LoggerNames.Repository);
		}

		/// <inheritdoc />
		public IQueryable<T> ApplyTo<T>(IQueryable<T> queryable, IEnumerable<Expression<Func<T, object>>> includeExpressions) where T : class
		{
			this.logger.LogStorageNotSupportsIncludeQueryOption(this.storageName);

			return queryable;
		}
	}
}
