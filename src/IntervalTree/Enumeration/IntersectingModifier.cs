using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS.Enumeration
{
	/// <summary>Modifies a tree traversal based on looking for intersecting intervals.</summary>
	/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
	internal class IntersectingModifier<TElement, TEndpoint> : ITraversalModifier<TElement, TEndpoint>
		where TElement : IInterval<TEndpoint>
	{
		/// <summary>The interval to find all intersecting elements for.</summary>
		private readonly IInterval<TEndpoint> interval;

		/// <summary>The comparer to use to compare endpoints.</summary>
		private readonly IComparer<TEndpoint> comparer;

		/// <summary>Set to true to also get adjacent intervals.</summary>
		private readonly bool alsoAdjacent;

		/// <summary>Initializes a new instance of the <see cref="IntersectingModifier{TElement, TEndpoint}"/> class.</summary>
		/// <param name="interval">The interval to find all intersecting elements for.</param>
		/// <param name="comparer">The comparer to use to compare endpoints.</param>
		/// <param name="alsoAdjacent">Set to true to also get adjacent intervals.</param>
		public IntersectingModifier(IInterval<TEndpoint> interval, IComparer<TEndpoint> comparer, bool alsoAdjacent)
		{
			this.interval = interval;
			this.alsoAdjacent = alsoAdjacent;
			this.comparer = comparer;
		}

		/// <summary>Determines if the tree should traverse down the left subtree of the specified node.</summary>
		/// <remarks>The <see cref="TreeEnumerator{TElement,TEndpoint}"/> instance should determine that the node and the node's
		/// left child are not the <see cref="IntervalNode{TElement,TEndpoint}.Sentinel"/> before calling into this class.</remarks>
		/// <param name="node">Node in the tree currently at during traversal.</param>
		/// <returns><c>true</c> if we can traverse down the left subtree; <c>false</c> otherwise.</returns>
		public bool CanGoLeft(IntervalNode<TElement, TEndpoint> node)
		{
			if (node == null)
				throw new ArgumentNullException(nameof(node));
			int compareValue = comparer.Compare(node.Left.Max, interval.Start);
			return alsoAdjacent ? compareValue >= 0 : compareValue > 0;
		}

		/// <summary>Determines if the tree should traverse down the right subtree of the specified node.</summary>
		/// <remarks>The <see cref="TreeEnumerator{TElement,TEndpoint}"/> instance should determine that the node and the node's
		/// right child are not the <see cref="IntervalNode{TElement,TEndpoint}.Sentinel"/> before calling into this class.</remarks>
		/// <param name="node">Node in the tree currently at during traversal.</param>
		/// <returns><c>true</c> if we can traverse down the right subtree; <c>false</c> otherwise.</returns>
		public bool CanGoRight(IntervalNode<TElement, TEndpoint> node)
		{
			if (node == null)
				throw new ArgumentNullException(nameof(node));
			int compareValue = comparer.Compare(node.Interval.Start, interval.End);
			return alsoAdjacent ? compareValue <= 0 : compareValue < 0;
		}

		/// <summary>Determines if we can yield the specified node as part of the traversal.</summary>
		/// <remarks>The <see cref="TreeEnumerator{TElement,TEndpoint}"/> instance should determine that the node is not the
		/// <see cref="IntervalNode{TElement,TEndpoint}.Sentinel"/> before calling into this class.</remarks>
		/// <param name="node">Node in the tree currently at during traversal.</param>
		/// <returns><c>true</c> if we can traverse down the right subtree; <c>false</c> otherwise.</returns>
		public bool CanYield(IntervalNode<TElement, TEndpoint> node)
		{
			if (node == null)
				throw new ArgumentNullException(nameof(node));
			return alsoAdjacent
				? node.Interval.IntersectsOrIsAdjacentTo(interval, comparer)
				: node.Interval.Intersects(interval, comparer);
		}
	}
}
