namespace Fluxera.Repository
{
	using System;
	using JetBrains.Annotations;

	/// <summary>
	///     Contract for a repository that can get disposed.
	/// </summary>
	[PublicAPI]
	public interface IDisposableRepository : IDisposable, IAsyncDisposable
	{
		/// <summary>
		///     Checks it this repository instance was disposed.
		/// </summary>
		bool IsDisposed { get; }
	}
}
