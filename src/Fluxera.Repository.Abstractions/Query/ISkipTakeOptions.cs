namespace Fluxera.Repository.Query
{
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for skip/take options configuration.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public interface ISkipTakeOptions<T> : IQueryOptions<T> where T : class
	{
		/// <summary>
		///     Gets the amount of items to skip.
		/// </summary>
		int? SkipAmount { get; }

		/// <summary>
		///     Gets the amount of items to take.
		/// </summary>
		int? TakeAmount { get; }

		/// <summary>
		///     Configures the amount of items to skip.
		/// </summary>
		/// <param name="skipAmount"></param>
		/// <returns></returns>
		ISkipTakeOptions<T> Skip(int skipAmount);

		/// <summary>
		///     Configures the amount of items to take.
		/// </summary>
		/// <param name="takeAmount"></param>
		/// <returns></returns>
		ISkipTakeOptions<T> Take(int takeAmount);
	}
}
