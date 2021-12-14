namespace Fluxera.Repository.Query
{
	using JetBrains.Annotations;

	[PublicAPI]
	public interface ISkipTakeOptions<T> where T : class
	{
		int? Skip { get; }

		int? Take { get; }
	}
}
