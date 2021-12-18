namespace Fluxera.Repository.Query
{
	using JetBrains.Annotations;

	[PublicAPI]
	public interface ISkipTakeOptions<T> : IQueryOptions<T> where T : class
	{
		ISkipTakeOptions<T> Skip(int skipNumber);

		ISkipTakeOptions<T> Take(int takeNumber);
	}
}
