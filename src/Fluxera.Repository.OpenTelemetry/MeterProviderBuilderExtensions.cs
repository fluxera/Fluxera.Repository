// ReSharper disable once CheckNamespace

namespace OpenTelemetry.Trace
{
	using JetBrains.Annotations;
	using OpenTelemetry.Metrics;

	/// <summary>
	///     Provides extension methods to enable the meter instrumentation for the Repository.
	/// </summary>
	[PublicAPI]
	public static class MeterProviderBuilderExtensions
	{
		/// <summary>
		///     Adds the meter for the repository to OpenTelemetry.
		/// </summary>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static MeterProviderBuilder AddRepositoryInstrumentation(this MeterProviderBuilder builder)
		{
			return builder.AddMeter("Fluxera.Repository");
		}
	}
}
