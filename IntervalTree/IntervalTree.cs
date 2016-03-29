using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS
{
	/// <summary>Represents a strongly-typed collection of objects that can be represented by intervals.</summary>
	/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
	public class IntervalTree<TElement, TEndpoint>
		where TElement : IInterval<TEndpoint>
		where TEndpoint : IComparable<TEndpoint>
	{
		/// <summary>A single sentinel object to use as boundaries of the tree.</summary>
		internal static readonly IntervalNode<TElement, TEndpoint> Sentinel = new IntervalNode<TElement, TEndpoint>();

		/// <summary>Gets the root of the tree.</summary>
		internal IntervalNode<TElement, TEndpoint> Root { get; private set; } = Sentinel;
	}
}
