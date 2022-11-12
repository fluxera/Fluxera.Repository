namespace Sample.Domain.Company
{
	using Fluxera.StronglyTypedId;

	public class CompanyId : StronglyTypedId<CompanyId, string>
	{
		/// <inheritdoc />
		public CompanyId(string value) : base(value)
		{
		}
	}
}
