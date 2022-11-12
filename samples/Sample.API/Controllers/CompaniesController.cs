namespace Sample.API.Controllers
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Fluxera.Repository;
	using Microsoft.AspNetCore.Mvc;
	using Sample.Domain.Company;

	[ApiController]
	[Route("[controller]")]
	public class CompaniesController : ControllerBase
	{
		private readonly ICompanyRepository repository;
		private readonly IUnitOfWork unitOfWork;

		public CompaniesController(ICompanyRepository repository, IUnitOfWorkFactory unitOfWorkFactory)
		{
			this.repository = repository;
			this.unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		}

		[HttpPost]
		public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest request)
		{
			await this.repository.AddAsync(new Company
			{
				Name = request.Name,
				LegalType = LegalType.LimitedLiabilityCompany
			});
			await this.unitOfWork.SaveChangesAsync();

			return this.Ok();
		}

		[HttpGet]
		public async Task<IActionResult> GetCompanies()
		{
			IReadOnlyCollection<Company> companies = await this.repository.FindManyAsync(x => true);

			return this.Ok(companies.Select(x => new
			{
				x.ID,
				x.Name,
				LegalType = x.LegalType.ShortName
			}));
		}
	}
}
