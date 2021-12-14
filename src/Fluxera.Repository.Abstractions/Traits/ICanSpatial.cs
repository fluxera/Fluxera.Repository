//namespace Fluxera.Repository.Traits
//{
//	using System;
//	using System.Collections.Generic;
//	using System.Linq.Expressions;
//	using System.Threading;
//	using System.Threading.Tasks;
//	using JetBrains.Annotations;

//	/// <summary>
//	///     Based on the Interface Segregation Principle (ISP), the
//	///     ICanAdd interface exposes only the "Add" methods of the
//	///     Repository.
//	///     <see href="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/" />
//	/// </summary>
//	/// <typeparam name="TAggregateRoot">Generic repository entity root type.</typeparam>
//	[PublicAPI]
//	public interface ICanSpatial<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot<TAggregateRoot>
//	{
//		/// <summary>
//		///     Finds items near the given location.
//		/// </summary>
//		/// <param name="predicate">The predicate.</param>
//		/// <param name="locationSelector">The location property.</param>
//		/// <param name="location">The location.</param>
//		/// <param name="minDistance">The minimum distance.</param>
//		/// <param name="maxDistance">The maximum distance.</param>
//		/// <param name="queryOptions">The query options.</param>
//		/// <param name="cancellationToken">The cancellation token.</param>
//		/// <returns></returns>
//		Task<IReadOnlyList<TAggregateRoot>> FindManyNearAsync(
//			Expression<Func<TAggregateRoot, bool>> predicate,
//			Expression<Func<TAggregateRoot, GeoPoint>> locationSelector,
//			GeoPoint location,
//			double? minDistance = null,
//			double? maxDistance = null,
//			IQueryOptions<TAggregateRoot> queryOptions = null,
//			CancellationToken cancellationToken = default);
//	}
//}


