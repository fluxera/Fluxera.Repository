namespace Sample.Domain.Company
{
	using System.ComponentModel.DataAnnotations;
	using Fluxera.ValueObject;

	public class Address : ValueObject<Address>
	{
		[Required]
		public string Street { get; set; }

		[Required]
		public string Number { get; set; }

		[Required]
		public string City { get; set; }
	}
}
