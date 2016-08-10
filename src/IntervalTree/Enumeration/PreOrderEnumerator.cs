using System;
using System.Collections;
using System.Collections.Generic;

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

		/// <summary>Initializes a new instance of the <see cref="PreOrderEnumerator{TElement,TEndpoint}"/> class.</summary>
		/// <param name="tree">Tree enumerating.</param>
		/// <param name="modifier"><see cref="ITraversalModifier{TElement,TEndpoint}"/> instance to modify this traversal.</param>
		/// <param name="asEnumerator">Set to true to instantiate this object in its <see cref="IEnumerator"/> state. Otherwise it
		/// will be in its <see cref="IEnumerable"/> state.</param>
		internal PreOrderEnumerator(IntervalTree<TElement, TEndpoint> tree, ITraversalModifier<TElement, TEndpoint> modifier, bool asEnumerator = false)
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
					CurrentNode = Tree.IRoot;
				else if (CurrentNode.Left != Sentinel && Modifier.CanGoLeft(CurrentNode))
					CurrentNode = CurrentNode.Left;
				else
				{
					if (stack.Count == 0)
						return false;

					CurrentNode = stack.Pop();
				}

				if (CurrentNode.Right != Sentinel && Modifier.CanGoRight(CurrentNode))
					stack.Push(CurrentNode.Right);

				if (Modifier.CanYield(CurrentNode))
					return true;
			}
		}

		/// <summary>Call copy ctor and return as an <see cref="IEnumerator{TElement}"/> instance.</summary>
		/// <returns>Copied instance.</returns>
		protected override IEnumerator<TElement> CallCopyCtor()
		{
			return new PreOrderEnumerator<TElement, TEndpoint>(this);
		}
	}
}