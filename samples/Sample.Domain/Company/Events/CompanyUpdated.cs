namespace Sample.Domain.Company.Events
{
	using Fluxera.Repository.DomainEvents;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class CompanyUpdated : ItemUpdated<Company, CompanyId>
	{
		/// <inheritdoc />
		public CompanyUpdated(Company beforeUpdateItem, Company afterUpdateItem)
			: base(beforeUpdateItem, afterUpdateItem)
		{
		}
	}
}
