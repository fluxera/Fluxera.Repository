namespace Fluxera.Repository.UnitTests.Interception
{
	using System;
	using System.Threading.Tasks;
	using Fluxera.Repository.Interception;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;

	public class PersonInterceptor : InterceptorBase<Person, Guid>
	{
		private readonly InterceptorCounter interceptorCounter;

		public PersonInterceptor(InterceptorCounter interceptorCounter)
		{
			this.interceptorCounter = interceptorCounter;
		}

		/// <inheritdoc />
		public override Task BeforeAddAsync(Person item, InterceptionEvent e)
		{
			this.interceptorCounter.BeforeAddCalled++;

			return base.BeforeAddAsync(item, e);
		}

		///// <inheritdoc />
		//public override Task AfterAddAsync(Person item)
		//{
		//	this.interceptorCounter.AfterAddCalled++;

		//	return base.AfterAddAsync(item);
		//}

		/// <inheritdoc />
		public override Task BeforeUpdateAsync(Person item, InterceptionEvent e)
		{
			this.interceptorCounter.BeforeUpdateCalled++;

			return base.BeforeUpdateAsync(item, e);
		}

		///// <inheritdoc />
		//public override Task AfterUpdateAsync(Person item)
		//{
		//	this.interceptorCounter.AfterUpdateCalled++;

		//	return base.AfterUpdateAsync(item);
		//}

		/// <inheritdoc />
		public override Task BeforeRemoveAsync(Person item, InterceptionEvent e)
		{
			this.interceptorCounter.BeforeRemoveCalled++;

			return base.BeforeRemoveAsync(item, e);
		}

		///// <inheritdoc />
		//public override Task AfterRemoveAsync(Person item)
		//{
		//	this.interceptorCounter.AfterRemoveCalled++;

		//	return base.AfterRemoveAsync(item);
		//}
	}
}
