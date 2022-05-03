// ReSharper disable once CheckNamespace

namespace OpenTelemetry.Trace
{
	using JetBrains.Annotations;

	/// <summary>
	///     Provides extension methods to enable the trace instrumentation for the Repository.
	/// </summary>
	[PublicAPI]
	public static class TracerProviderBuilderExtensions
	{
		/// <summary>
		///     Adds the diagnostic source for the repository to OpenTelemetry.
		/// </summary>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static TracerProviderBuilder AddRepositoryInstrumentation(this TracerProviderBuilder builder)
		{
			return builder.AddSource("Fluxera.Repository");
		}
	}
}
