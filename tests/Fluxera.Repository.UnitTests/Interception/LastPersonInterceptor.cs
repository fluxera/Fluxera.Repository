namespace Fluxera.Repository.UnitTests.Interception
{
	using System.Threading.Tasks;
	using Fluxera.Repository.Interception;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;

	public class LastPersonInterceptor : PersonInterceptor
	{
		private readonly InterceptorCounter interceptorCounter;

		/// <inheritdoc />
		public LastPersonInterceptor(InterceptorCounter interceptorCounter)
			: base(interceptorCounter)
		{
			this.interceptorCounter = interceptorCounter;
		}

		/// <inheritdoc />
		public override int Order => 2;

		/// <inheritdoc />
		public override Task BeforeAddAsync(Person item, InterceptionEvent e)
		{
			this.interceptorCounter.BeforeAddCall.Add("Last");

			return base.BeforeAddAsync(item, e);
		}
	}
}
