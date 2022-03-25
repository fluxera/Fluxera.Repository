namespace Fluxera.Repository.UnitTests.Core.CompanyAggregate
{
	using System;

	public class CompanyRepository : Repository<Company, Guid>, ICompanyRepository
	{
		/// <inheritdoc />
		public CompanyRepository(IRepository<Company, Guid> innerRepository)
			: base(innerRepository)
		{
		}
	}
}
