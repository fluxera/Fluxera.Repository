namespace Fluxera.Repository.UnitTests.Core.PersonAggregate
{
	using FluentValidation;

	internal class PersonValidator : AbstractValidator<Person>
	{
		public PersonValidator()
		{
			this.RuleFor(x => x.Name).NotEmpty();
		}
	}
}
