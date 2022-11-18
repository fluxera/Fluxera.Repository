namespace Sample.Domain.Company.Events
{
	using Fluxera.Repository.DomainEvents;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class CompanyRemoved : ItemRemoved<Company, CompanyId>
	{
		/// <inheritdoc />
		public CompanyRemoved(CompanyId id, Company item)
			: base(id, item)
		{
		}
	}
}
