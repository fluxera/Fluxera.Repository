namespace Sample.Domain.Company
{
	using Fluxera.ValueObject;

	public class Address : ValueObject<Address>
	{
		public string Street { get; set; }

		public string Number { get; set; }

		public string City { get; set; }
	}
}
