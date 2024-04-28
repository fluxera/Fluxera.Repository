namespace Fluxera.Repository.UnitTests.Interception
{
	using System.Collections.Generic;

	public class InterceptorCounter
	{
		public int BeforeAddCalled { get; set; }

		public int BeforeUpdateCalled { get; set; }

		public int BeforeRemoveCalled { get; set; }

		public IList<string> BeforeAddCall { get; } = new List<string>();
	}
}
