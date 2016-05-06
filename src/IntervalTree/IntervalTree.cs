using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS
{
	/// <summary>Provides static methods for creating interval trees.</summary>
	public static class IntervalTree
	{
		/// <summary>Creates a new interval tree of type <typeparamref name="TElement"/> which is an interval of type
		/// <typeparamref name="TElement"/>.</summary>
		/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
		/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
		/// <returns>A new interval tree.</returns>
		public static IIntervalTree<TElement> Create<TElement, TEndpoint>()
			where TElement : IInterval<TEndpoint>
			where TEndpoint : IComparable<TEndpoint>
		{
			return new IntervalTree<TElement, TEndpoint>(Comparer<TEndpoint>.Default);
		}

		/// <summary>Creates a new interval tree of type <typeparamref name="TElement"/> which is an interval of type
		/// <typeparamref name="TElement"/>.</summary>
		/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
		/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
		/// <param name="comparer">An <see cref="IComparer{T}"/> to compare endpoints.</param>
		/// <returns>A new interval tree.</returns>
		public static IIntervalTree<TElement> Create<TElement, TEndpoint>(IComparer<TEndpoint> comparer)
			where TElement : IInterval<TEndpoint>
		{
			return new IntervalTree<TElement, TEndpoint>(comparer);
		}
	}
}
