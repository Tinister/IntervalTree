using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS.Enumeration
{
	/// <summary>Class for enumerating all intersecting elements in a <see cref="IntervalTree{TElement,TEndpoint}"/> for a
	/// specified interval.</summary>
	/// <remarks>Walks the tree in "preorder" traversal so that finding the first intersecting interval can finish the
	/// traversal right away.</remarks>
	/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
	internal class IntersectingEnumerator<TElement, TEndpoint> : TreeEnumerator<TElement, TEndpoint>
		where TElement : IInterval<TEndpoint>
	{
		/// <summary>Set to true to also get adjacent intervals.</summary>
		private readonly bool alsoAdjacent;

		/// <summary>Keeping track of the nodes as we traverse.  If this field is null, the enumeration is finished.</summary>
		private readonly Stack<IntervalNode<TElement, TEndpoint>> stack = new Stack<IntervalNode<TElement, TEndpoint>>();

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
			Interval = new Interval<TEndpoint>(interval, Tree.Comparer);
			this.alsoAdjacent = alsoAdjacent;
		}

		/// <summary>Initializes a new instance of the <see cref="IntersectingEnumerator{TElement,TEndpoint}"/> class as a copy of
		/// the specified enumerator. Copied enumerators are always in the <see cref="IEnumerator"/> state.</summary>
		/// <param name="copy"><see cref="IntersectingEnumerator{TElement,TEndpoint}"/> to copy.</param>
		internal IntersectingEnumerator(IntersectingEnumerator<TElement, TEndpoint> copy)
			: base(copy)
		{
			Interval = copy.Interval;
			alsoAdjacent = copy.alsoAdjacent;
		}

		/// <summary>Gets the interval to find all intersecting elements for.</summary>
		internal IInterval<TEndpoint> Interval { get; }

		/// <summary>Call copy ctor and return as an <see cref="IEnumerator{TElement}"/> instance.</summary>
		/// <returns>Copied instance.</returns>
		protected override IEnumerator<TElement> CallCopyCtor()
		{
			return new IntersectingEnumerator<TElement, TEndpoint>(this);
		}

		/// <summary>Advances the enumerator to the next element of the collection.</summary>
		/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the
		/// end of the collection.</returns>
		/// <exception cref="InvalidOperationException">The collection was modified after the enumerator was created.</exception>
		protected override bool MoveNextCore()
		{
			while (true)
			{
				if (CurrentNode == Sentinel) // only true when "above" the root
					CurrentNode = Tree.IRoot;
				else if (CanGoLeft())
					CurrentNode = CurrentNode.Left;
				else
				{
					if (stack.Count == 0)
						return false;

					CurrentNode = stack.Pop();
				}

				if (CanGoRight())
					stack.Push(CurrentNode.Right);

				if (StopHere())
					return true;
			}
		}

		/// <summary>Determine whether the enumerator can continue down the left side of the subtree at
		/// <see cref="TreeEnumerator{TElement,TEndpoint}.CurrentNode"/>.</summary>
		/// <returns>true to continue down the left subtree, false to skip the left subtree.</returns>
		private bool CanGoLeft()
		{
			if (CurrentNode.Left == Sentinel)
				return false;
			// if the left subtree's max is smaller than the interval we're looking at, we can ignore the whole left subtree
			if (alsoAdjacent)
				return Tree.Comparer.Compare(CurrentNode.Left.Max, Interval.Start) >= 0;
			return Tree.Comparer.Compare(CurrentNode.Left.Max, Interval.Start) > 0;
		}

		/// <summary>Determine whether the enumerator can continue down the right side of the subtree at
		/// <see cref="TreeEnumerator{TElement,TEndpoint}.CurrentNode"/>.</summary>
		/// <returns>true to continue down the right subtree, false to skip the right subtree.</returns>
		private bool CanGoRight()
		{
			if (CurrentNode.Right == Sentinel)
				return false;
			// if *this* node's starting endpoint is past the end of the interval we're looking at, we can ignore the whole right subtree
			if (alsoAdjacent)
				return Tree.Comparer.Compare(CurrentNode.Interval.Start, Interval.End) <= 0;
			return Tree.Comparer.Compare(CurrentNode.Interval.Start, Interval.End) < 0;
		}

		/// <summary>Determine whether to stop at current value for <see cref="TreeEnumerator{TElement,TEndpoint}.CurrentNode"/>.</summary>
		/// <returns>true to stop at this node and return it to the enumerator's caller.</returns>
		private bool StopHere()
		{
			if (alsoAdjacent)
				return CurrentNode.Interval.IntersectsOrIsAdjacentTo(Interval, Tree.Comparer);
			return CurrentNode.Interval.Intersects(Interval, Tree.Comparer);
		}
	}
}
