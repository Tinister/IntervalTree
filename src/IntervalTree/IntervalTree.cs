using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS
{
	/// <summary>Provides static methods for creating interval trees.</summary>
	public static class IntervalTree
	{
		/// <summary>Creates a new interval tree of typle <typeparamref name="TElement"/> which is an interval of type
		/// <typeparamref name="TElement"/>.</summary>
		/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
		/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
		/// <returns>A new interval tree.</returns>
		public static IIntervalTree<TElement> Create<TElement, TEndpoint>()
			where TElement : IInterval<TEndpoint>
			where TEndpoint : IComparable<TEndpoint>
		{
			return new IntervalTree<TElement, TEndpoint>();
		}
	}
}
