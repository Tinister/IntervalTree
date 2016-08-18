using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS.Enumeration
{
	/// <summary>Class for enumerating an <see cref="IntervalTree{TElement,TEndpoint}"/> with an pre-order tree walk.</summary>
	/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
	internal sealed class PreOrderEnumerator<TElement, TEndpoint> : TreeEnumerator<TElement, TEndpoint>
		where TElement : IInterval<TEndpoint>
	{
		/// <summary>Keeping track of the nodes as we traverse.</summary>
		private Stack<IntervalNode<TElement, TEndpoint>> stack;

		/// <summary>Number of nodes at the top of the stack that are unverified.</summary>
		private int unverifiedLength;

		/// <summary>Initializes a new instance of the <see cref="PreOrderEnumerator{TElement,TEndpoint}"/> class.</summary>
		/// <param name="tree">Tree enumerating.</param>
		/// <param name="modifier"><see cref="ITraversalModifier{TElement,TEndpoint}"/> instance to modify this traversal.</param>
		/// <param name="asEnumerator">Set to true to instantiate this object in its <see cref="IEnumerator"/> state. Otherwise it
		/// will be in its <see cref="IEnumerable"/> state.</param>
		internal PreOrderEnumerator(
			IntervalTree<TElement, TEndpoint> tree, ITraversalModifier<TElement, TEndpoint> modifier, bool asEnumerator = false)
			: base(tree, modifier, asEnumerator)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="PreOrderEnumerator{TElement,TEndpoint}"/> class as a copy of the
		/// specified enumerator. Copied enumerators are always in the <see cref="IEnumerator"/> state.</summary>
		/// <param name="copy"><see cref="PreOrderEnumerator{TElement,TEndpoint}"/> to copy.</param>
		private PreOrderEnumerator(PreOrderEnumerator<TElement, TEndpoint> copy)
			: base(copy)
		{
		}

		/// <summary>Advances the enumerator to the next element of the collection.</summary>
		/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the
		/// end of the collection.</returns>
		/// <exception cref="InvalidOperationException">The collection was modified after the enumerator was created.</exception>
		protected override bool MoveNextCore()
		{
			if (stack == null)
				stack = new Stack<IntervalNode<TElement, TEndpoint>>();
			while (true)
			{
				if (CurrentNode == Sentinel) // only true when "above" the root
				{
					unverifiedLength = 0;
					CurrentNode = Tree.IRoot;
				}
				else if (CanGoLeft())
				{
					CurrentNode = CurrentNode.Left;
				}
				else if (stack.Count > 0 && stack.Peek() == CurrentNode.Right)
				{
					if (unverifiedLength > 0)
						unverifiedLength--;
					CurrentNode = stack.Pop();
				}
				else
				{
					for (; unverifiedLength > 0; unverifiedLength--)
						stack.Pop();
					if (stack.Count == 0)
						return false;

					CurrentNode = stack.Pop();
				}

				if (CanGoRight())
				{
					unverifiedLength++;
					stack.Push(CurrentNode.Right);
				}

				if (Modifier.CanYield(CurrentNode))
				{
					unverifiedLength = 0;
					return true;
				}
			}
		}

		/// <summary>Call copy ctor and return as an <see cref="IEnumerator{TElement}"/> instance.</summary>
		/// <returns>Copied instance.</returns>
		protected override IEnumerator<TElement> CallCopyCtor()
		{
			return new PreOrderEnumerator<TElement, TEndpoint>(this);
		}

		/// <summary>Determines whether we can go down the left subtree from the current node.</summary>
		/// <returns><c>true</c> if we can go left from the current node.</returns>
		private bool CanGoLeft() => CurrentNode.Left != Sentinel && Modifier.CanGoLeft(CurrentNode);

		/// <summary>Determines whether we can go down the right subtree from the current node.</summary>
		/// <returns><c>true</c> if we can go right from the current node.</returns>
		private bool CanGoRight() => CurrentNode.Right != Sentinel && Modifier.CanGoRight(CurrentNode);
	}
}
