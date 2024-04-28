namespace Fluxera.Repository.UnitTests.Interception
{
	using System;
	using System.Threading.Tasks;
	using Fluxera.Repository.Interception;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;

	public class CountingPersonInterceptor : InterceptorBase<Person, Guid>
	{
		private readonly InterceptorCounter interceptorCounter;

		public CountingPersonInterceptor(InterceptorCounter interceptorCounter)
		{
			this.interceptorCounter = interceptorCounter;
		}

		/// <inheritdoc />
		public override Task BeforeAddAsync(Person item, InterceptionEvent e)
		{
			this.interceptorCounter.BeforeAddCalled++;

			return base.BeforeAddAsync(item, e);
		}

		/// <inheritdoc />
		public override Task BeforeUpdateAsync(Person item, InterceptionEvent e)
		{
			this.interceptorCounter.BeforeUpdateCalled++;

			return base.BeforeUpdateAsync(item, e);
		}
		/// <inheritdoc />
		public override Task BeforeRemoveAsync(Person item, InterceptionEvent e)
		{
			this.interceptorCounter.BeforeRemoveCalled++;

			return base.BeforeRemoveAsync(item, e);
		}
	}
}
