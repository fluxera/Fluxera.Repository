namespace ODataApplication.Controllers
{
	using Fluxera.Extensions.Common;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.OData.Query;
	using Microsoft.AspNetCore.OData.Routing.Controllers;

	public class PeopleController : ODataController
	{
		private readonly IGuidGenerator guidGenerator;

		public PeopleController(IGuidGenerator guidGenerator)
		{
			this.guidGenerator = guidGenerator;
		}

		[HttpGet]
		[EnableQuery]
		public IEnumerable<Person> Get()
		{
			return new Person[]
			{
				new Person
				{
					ID = this.guidGenerator.Create().ToString("N"),
					Name = "Tester-12"
				},
				new Person
				{
					ID = this.guidGenerator.Create().ToString("N"),
					Name = "Tester-26"
				},
				new Person
				{
					ID = this.guidGenerator.Create().ToString("N"),
					Name = "Tester-32"
				},
			};
		}
	}
}
