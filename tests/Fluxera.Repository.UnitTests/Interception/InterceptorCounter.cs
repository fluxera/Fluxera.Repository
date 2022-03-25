namespace Fluxera.Repository.UnitTests.Interception
{
	using System.Collections.Generic;

	public class InterceptorCounter
	{
		public int AfterAddCalled { get; set; }

		public int BeforeAddCalled { get; set; }

		public int BeforeUpdateCalled { get; set; }

		public int AfterUpdateCalled { get; set; }

		public int BeforeRemoveCalled { get; set; }

		public int AfterRemoveCalled { get; set; }

		public IList<string> BeforeAddCall { get; } = new List<string>();
	}
}
