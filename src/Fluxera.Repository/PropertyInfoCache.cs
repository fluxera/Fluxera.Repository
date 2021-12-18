namespace Fluxera.Repository
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Reflection;

	internal static class PropertyInfoCache
	{
		internal static readonly IDictionary<Tuple<Type, Type>, PropertyInfo> PrimaryKeyDict = new ConcurrentDictionary<Tuple<Type, Type>, PropertyInfo>();
	}
}
