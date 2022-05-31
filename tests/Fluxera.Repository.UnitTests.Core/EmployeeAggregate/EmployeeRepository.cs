namespace Fluxera.Repository.UnitTests.Core.EmployeeAggregate
{
	public class EmployeeRepository : Repository<Employee, EmployeeId>, IEmployeeRepository
	{
		/// <inheritdoc />
		public EmployeeRepository(IRepository<Employee, EmployeeId> innerRepository)
			: base(innerRepository)
		{
		}
	}
}
