namespace Sample.Domain.Company
{
	using FluentValidation;

	public class AddressValidator : AbstractValidator<Address>
	{
		public AddressValidator()
		{
			this.RuleFor(x => x.Street).NotEmpty();

			this.RuleFor(x => x.Number).NotEmpty();

			this.RuleFor(x => x.City).NotEmpty();
		}
	}
}
