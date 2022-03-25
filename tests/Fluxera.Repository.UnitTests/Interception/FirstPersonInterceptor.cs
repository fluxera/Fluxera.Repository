namespace Fluxera.Repository.UnitTests.Interception
{
	using System.Threading.Tasks;
	using Fluxera.Repository.Interception;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;

	public class FirstPersonInterceptor : PersonInterceptor
	{
		private readonly InterceptorCounter interceptorCounter;

		/// <inheritdoc />
		public FirstPersonInterceptor(InterceptorCounter interceptorCounter)
			: base(interceptorCounter)
		{
			this.interceptorCounter = interceptorCounter;
		}

		/// <inheritdoc />
		public override int Order => 0;

		/// <inheritdoc />
		public override Task BeforeAddAsync(Person item, InterceptionEvent e)
		{
			this.interceptorCounter.BeforeAddCall.Add("First");

			return base.BeforeAddAsync(item, e);
		}
	}
}
