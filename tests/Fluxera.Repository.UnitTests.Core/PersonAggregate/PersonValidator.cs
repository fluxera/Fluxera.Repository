namespace Fluxera.Repository.UnitTests.Core.PersonAggregate
{
	using FluentValidation;

	public class PersonValidator : AbstractValidator<Person>
	{
		public PersonValidator()
		{
			this.RuleFor(x => x.Name).NotEmpty();
		}
	}
}
