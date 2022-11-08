namespace Sample.EntityFrameworkCore.Controllers
{
	using Microsoft.AspNetCore.Mvc;

	[ApiController]
	[Route("[controller]")]
	public class CompaniesController : ControllerBase
	{
		[HttpPost("/tenant-one/company")]
		public async Task<IActionResult> CreateCompany1([FromBody] CreateCompanyRequest request)
		{
			return this.Ok();
		}

		[HttpPost("/tenant-two/company")]
		public async Task<IActionResult> CreateCompany2([FromBody] CreateCompanyRequest request)
		{
			return this.Ok();
		}
	}
}
