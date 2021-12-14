namespace Fluxera.Repository
{
	using System;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IDisposableRepository : IDisposable
	{
		public bool IsDisposed { get; }
	}
}
