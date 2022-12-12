namespace Sample.API.Controllers
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Fluxera.Repository;
	using Fluxera.Repository.Query;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Options;
	using Sample.Domain.Company;

	[ApiController]
	[Route("[controller]")]
	public class CompaniesController : ControllerBase
	{
		private readonly ICompanyRepository repository;
		private readonly IUnitOfWork unitOfWork;
		private readonly SampleOptions options;

		public CompaniesController(
			IOptions<SampleOptions> options,
			ICompanyRepository repository,
			IUnitOfWorkFactory unitOfWorkFactory)
		{
			this.options = options.Value;
			this.repository = repository;
			this.unitOfWork = unitOfWorkFactory.CreateUnitOfWork(repository.RepositoryName);
		}

		[HttpPost]
		public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest request)
		{
			await this.repository.AddAsync(new Company
			{
				Name = request.Name,
				LegalType = LegalType.LimitedLiabilityCompany
			});

			if(this.options.EnableUnitOfWork)
			{
				await this.unitOfWork.SaveChangesAsync();
			}

			return this.Ok();
		}

		[HttpGet]
		public async Task<IActionResult> GetCompanies([FromServices] QueryOptionsBuilder<Company> queryOptionsBuilder)
		{
			IQueryOptions<Company> queryOptions = queryOptionsBuilder
				.Include(x => x.Partners)
				.OrderBy(x => x.Name)
				.ThenByDescending(x => x.LegalType)
				.Build(queryable => queryable.AsNoTracking());

			IReadOnlyCollection<Company> companies = await this.repository.FindManyAsync(x => true, queryOptions);

			return this.Ok(companies.Select(x => new
			{
				x.ID,
				x.Name,
				LegalType = x.LegalType.ShortName
			}));
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteCompanies()
		{
			await this.repository.RemoveRangeAsync(x => true);

			if(this.options.EnableUnitOfWork)
			{
				await this.unitOfWork.SaveChangesAsync();
			}

			return this.NoContent();
		}
	}
}
