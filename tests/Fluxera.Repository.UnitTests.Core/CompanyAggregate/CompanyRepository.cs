namespace Fluxera.Repository.UnitTests.Core.CompanyAggregate
{
	public class CompanyRepository : Repository<Company, string>, ICompanyRepository
	{
		/// <inheritdoc />
		public CompanyRepository(IRepository<Company, string> innerRepository)
			: base(innerRepository)
		{
		}
	}
}
