using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS
{
	/// <summary>Class for enumerating all intersecting elements in a <see cref="IntervalTree{TElement,TEndpoint}"/> for a
	/// specified interval.</summary>
	/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
	internal class IntersectingEnumerator<TElement, TEndpoint> : InOrderEnumerator<TElement, TEndpoint>
		where TElement : IInterval<TEndpoint>
		where TEndpoint : IComparable<TEndpoint>
	{
		/// <summary>Set to true to also get adjacent intervals.</summary>
		private readonly bool alsoAdjacent;

		/// <summary>Initializes a new instance of the <see cref="IntersectingEnumerator{TElement,TEndpoint}"/> class.</summary>
		/// <param name="tree">Tree enumerating.</param>
		/// <param name="interval">Interval to find all intersecting elements for.</param>
		/// <param name="alsoAdjacent">Set to true to also get adjacent intervals.</param>
		/// <param name="asEnumerator">Set to true to instantiate this object in its <see cref="IEnumerator"/> state. Otherwise it
		/// will be in its <see cref="IEnumerable"/> state.</param>
		internal IntersectingEnumerator(
			IntervalTree<TElement, TEndpoint> tree, IInterval<TEndpoint> interval, bool alsoAdjacent = false,
			bool asEnumerator = false)
			: base(tree, asEnumerator)
		{
			Interval = new Interval<TEndpoint>(interval);
			this.alsoAdjacent = alsoAdjacent;
		}

		/// <summary>Initializes a new instance of the <see cref="IntersectingEnumerator{TElement,TEndpoint}"/> class as a copy of
		/// the specified enumerator. Copied enumerators are always in the <see cref="IEnumerator"/> state.</summary>
		/// <param name="copy"><see cref="InOrderEnumerator{TElement,TEndpoint}"/> to copy.</param>
		protected IntersectingEnumerator(IntersectingEnumerator<TElement, TEndpoint> copy)
			: base(copy)
		{
			Interval = copy.Interval;
			alsoAdjacent = copy.alsoAdjacent;
		}

		/// <summary>Gets the interval to find all intersecting elements for.</summary>
		protected IInterval<TEndpoint> Interval { get; }

		/// <summary>Call copy ctor and return as an <see cref="IEnumerator{TElement}"/> instance.</summary>
		/// <returns>Copied instance.</returns>
		protected override IEnumerator<TElement> CallCopyCtor()
		{
			return new IntersectingEnumerator<TElement, TEndpoint>(this);
		}

		/// <summary>Determine whether the enumerator continue down the left side of the specified subtree.</summary>
		/// <param name="node">Node at the top of the subtree before the enumerator is at before it decides to go left.</param>
		/// <returns>true to continue down the left subtree, false to skip the left subtree.</returns>
		protected override bool GoLeft(IntervalNode<TElement, TEndpoint> node)
		{
			if (node == null || node == Sentinel)
				throw new ArgumentNullException(nameof(node));
			// if the left subtree's max is smaller than the interval we're looking at, we can ignore the whole left subtree
			if (alsoAdjacent)
				return Comparer.Default.Compare(node.Left.Max, Interval.Start) >= 0;
			return Comparer.Default.Compare(node.Left.Max, Interval.Start) > 0;
		}

		/// <summary>Determine whether the enumerator continue down the right side of the specified subtree.</summary>
		/// <param name="node">Node at the top of the subtree before the enumerator is at before it decides to go right.</param>
		/// <returns>true to continue down the right subtree, false to skip the right subtree.</returns>
		protected override bool GoRight(IntervalNode<TElement, TEndpoint> node)
		{
			if (node == null || node == Sentinel)
				throw new ArgumentNullException(nameof(node));
			// if *this* node's starting endpoint is past the end of the interval we're looking at, we can ignore the whole right subtree
			if (alsoAdjacent)
				return Comparer.Default.Compare(node.Interval.Start, Interval.End) <= 0;
			return Comparer.Default.Compare(node.Interval.Start, Interval.End) < 0;
		}

		/// <summary>Determine whether to stop at the specified node. If true, will set to
		/// <see cref="InOrderEnumerator{TElement,TEndpoint}.Current"/> and return.</summary>
		/// <param name="node">Node the enumerator is currently stopped at.</param>
		/// <returns>true to stop at this node and return it to the enumerator's caller.</returns>
		protected override bool StopHere(IntervalNode<TElement, TEndpoint> node)
		{
			if (node == null || node == Sentinel)
				throw new ArgumentNullException(nameof(node));
			if (alsoAdjacent)
				return node.Interval.IntersectsOrIsAdjacentTo(Interval);
			return node.Interval.Intersects(Interval);
		}
	}
}
