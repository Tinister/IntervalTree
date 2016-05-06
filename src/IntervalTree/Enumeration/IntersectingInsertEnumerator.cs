using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS.Enumeration
{
	/// <summary>Class for enumerating all intersecting elements in a <see cref="IntervalTree{TElement,TEndpoint}"/> for a
	/// specified interval.  Additionally keeps track of the insertion point the interval would be inserted under.</summary>
	/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
	internal class IntersectingInsertEnumerator<TElement, TEndpoint> : IntersectingEnumerator<TElement, TEndpoint>
		where TElement : IInterval<TEndpoint>
	{
		/// <summary>The node to insert <see cref="IntersectingEnumerator{TElement,TEndpoint}.Interval"/> into.</summary>
		private IntervalNode<TElement, TEndpoint> insertionPoint;

		/// <summary>Initializes a new instance of the <see cref="IntersectingInsertEnumerator{TElement,TEndpoint}"/> class.</summary>
		/// <param name="tree">Tree enumerating.</param>
		/// <param name="interval">Interval to find all intersecting elements for.</param>
		/// <param name="alsoAdjacent">Set to true to also get adjacent intervals.</param>
		internal IntersectingInsertEnumerator(
			IntervalTree<TElement, TEndpoint> tree, IInterval<TEndpoint> interval, bool alsoAdjacent = false)
			: base(tree, interval, alsoAdjacent, true) // always in ienumerator state
		{
			InsertionPoint = tree.IRoot;
		}

		/// <summary>Gets the node to insert <see cref="IntersectingEnumerator{TElement,TEndpoint}.Interval"/> into.</summary>
		public IntervalNode<TElement, TEndpoint> InsertionPoint
		{
			get { return insertionPoint; }
			private set
			{
				insertionPoint = value;
				if (insertionPoint != Sentinel && Tree.Comparer.Compare(Interval.End, insertionPoint.Max) > 0)
					insertionPoint.Max = Interval.End;
			}
		}

		/// <summary>Call copy ctor and return as an <see cref="IEnumerator{TElement}"/> instance.</summary>
		/// <returns>Copied instance.</returns>
		protected override IEnumerator<TElement> CallCopyCtor()
		{
			return this; // *shouldn't* be calling GetEnumerator again
		}

		/// <summary>Determine whether the enumerator continue down the left side of the specified subtree.</summary>
		/// <param name="node">Node at the top of the subtree before the enumerator is at before it decides to go left.</param>
		/// <returns>true to continue down the left subtree, false to skip the left subtree.</returns>
		protected override bool GoLeft(IntervalNode<TElement, TEndpoint> node)
		{
			if (node == null)
				throw new ArgumentNullException(nameof(node));

			if (node.Parent == InsertionPoint &&
				Tree.Comparer.Compare(Interval.Start, InsertionPoint.Interval.Start) < 0)
				InsertionPoint = node;

			return base.GoLeft(node);
		}

		/// <summary>Determine whether the enumerator continue down the right side of the specified subtree.</summary>
		/// <param name="node">Node at the top of the subtree before the enumerator is at before it decides to go right.</param>
		/// <returns>true to continue down the right subtree, false to skip the right subtree.</returns>
		protected override bool GoRight(IntervalNode<TElement, TEndpoint> node)
		{
			if (node == null)
				throw new ArgumentNullException(nameof(node));

			if (node.Parent == InsertionPoint &&
				Tree.Comparer.Compare(Interval.Start, InsertionPoint.Interval.Start) >= 0)
				InsertionPoint = node;

			return base.GoRight(node);
		}
	}
}
