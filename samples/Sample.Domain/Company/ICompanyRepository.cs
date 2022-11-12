namespace Sample.Domain.Company
{
	using Fluxera.Repository;

	public interface ICompanyRepository : IRepository<Company, CompanyId>
	{
	}
}
