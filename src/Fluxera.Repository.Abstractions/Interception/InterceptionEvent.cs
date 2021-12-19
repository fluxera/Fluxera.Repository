namespace Fluxera.Repository.Interception
{
	using JetBrains.Annotations;

	/// <summary>
	///     An event that is used in the interception feature.
	/// </summary>
	[PublicAPI]
	public sealed class InterceptionEvent
	{
		/// <summary>
		///     Creates a new instance of the <see cref="InterceptionEvent" /> type.
		/// </summary>
		public InterceptionEvent()
		{
			this.CancelOperation = false;
			this.ThrowOnCancellation = true;
			this.CancellationMessage = "The current operation was cancelled by an interceptor.";
		}

		/// <summary>
		///     A flag, indicating if the current operation should be cancelled.
		/// </summary>
		public bool CancelOperation { get; set; }

		/// <summary>
		///     Gets or sets an cancellation message that will be used as exception message
		///     when <see cref="ThrowOnCancellation" /> is <c>true</c> or as log message otherwise.
		/// </summary>
		public string CancellationMessage { get; set; }

		/// <summary>
		///     A flag, indicating if the repository should throw an exception when
		///     <see cref="CancelOperation" /> is <c>true</c>. If it is <c>false</c>
		///     the repository cancels silently and just logs the <see cref="CancellationMessage" /> .
		/// </summary>
		public bool ThrowOnCancellation { get; set; }
	}
}
