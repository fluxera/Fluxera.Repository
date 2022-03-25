namespace Fluxera.Repository.UnitTests.Interception
{
	using System.Threading.Tasks;
	using Fluxera.Repository.Interception;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;

	public class MiddlePersonInterceptor : PersonInterceptor
	{
		private readonly InterceptorCounter interceptorCounter;

		/// <inheritdoc />
		public MiddlePersonInterceptor(InterceptorCounter interceptorCounter)
			: base(interceptorCounter)
		{
			this.interceptorCounter = interceptorCounter;
		}

		/// <inheritdoc />
		public override int Order => 1;

		/// <inheritdoc />
		public override Task BeforeAddAsync(Person item, InterceptionEvent e)
		{
			this.interceptorCounter.BeforeAddCall.Add("Middle");

			return base.BeforeAddAsync(item, e);
		}
	}
}
