namespace Sample.Domain.Company.Events
{
	using Fluxera.Repository.DomainEvents;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class CompanyAdded : ItemAdded<Company, CompanyId>
	{
		/// <inheritdoc />
		public CompanyAdded(Company item)
			: base(item)
		{
		}
	}
}
