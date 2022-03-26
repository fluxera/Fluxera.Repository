namespace Fluxera.Repository.UnitTests.Core.PersonAggregate
{
	using Fluxera.ValueObject;

	public class Address : ValueObject<Address>
	{
		public string Street { get; set; }

		public string Number { get; set; }

		public string City { get; set; }

		public string PostCode { get; set; }
	}
}
