namespace Fluxera.Repository.UnitTests.Core.CompanyAggregate
{
	using System.Runtime.CompilerServices;
	using Fluxera.Enumeration;

	public sealed class LegalType : Enumeration<LegalType>
	{
		public static readonly LegalType Corporation = new LegalType(0, "Corp.");
		public static readonly LegalType LimitedLiabilityCompany = new LegalType(1, "LLC");

		/// <inheritdoc />
		private LegalType(int value, string shortName, [CallerMemberName] string name = null)
			: base(value, name)
		{
			this.ShortName = shortName;
		}

		public string ShortName { get; }
	}
}
