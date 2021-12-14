namespace Fluxera.Repository.UnitTests
{
	using System;
	using System.Collections.Generic;

	public static class ParamsUtil
	{
		public static IEnumerable<T> AsEnumerable<T>(params T[] items)
		{
			items ??= Array.Empty<T>();
			return items;
		}
	}
}
