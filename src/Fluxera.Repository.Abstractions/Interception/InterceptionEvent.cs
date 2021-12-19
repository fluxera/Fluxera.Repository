namespace Fluxera.Repository.Interception
{
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class InterceptionEvent
	{
		public InterceptionEvent()
		{
			this.CancelOperation = false;
			this.ThrowOnCancellation = true;
		}

		public bool CancelOperation { get; set; }

		public string CancellationMessage { get; set; }

		public bool ThrowOnCancellation { get; set; }
	}
}
