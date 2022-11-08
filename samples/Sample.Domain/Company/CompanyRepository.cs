namespace Sample.Domain.Company
{
	using Fluxera.Repository;

	public class CompanyRepository : Repository<Company, CompanyId>, ICompanyRepository
	{
		/// <inheritdoc />
		public CompanyRepository(IRepository<Company, CompanyId> innerRepository)
			: base(innerRepository)
		{
		}
	}
}
