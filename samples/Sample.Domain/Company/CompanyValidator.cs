namespace Sample.Domain.Company
{
	using FluentValidation;

	public class CompanyValidator : AbstractValidator<Company>
	{
		public CompanyValidator()
		{
			this.RuleFor(x => x.Name).NotEmpty();

			this.RuleFor(x => x.LegalType).NotEmpty();
		}
	}
}
