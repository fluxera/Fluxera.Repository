namespace Sample.InMemory
{
	using Fluxera.Repository.InMemory;

	public class SampleInMemoryContext : InMemoryContext
	{
		/// <inheritdoc />
		protected override void ConfigureOptions(InMemoryContextOptions options)
		{
		}
	}
}
