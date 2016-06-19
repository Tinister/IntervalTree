using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS.Enumeration
{
	/// <summary>Class for enumerating an <see cref="IntervalTree{TElement,TEndpoint}"/>.</summary>
	/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
	internal sealed class InOrderEnumerator<TElement, TEndpoint> : TreeEnumerator<TElement, TEndpoint>
		where TElement : IInterval<TEndpoint>
	{
		/// <summary>Keeping track of the nodes as we traverse.  If this field is null, the enumeration is finished.</summary>
		private Stack<IntervalNode<TElement, TEndpoint>> stack;

		/// <summary>Initializes a new instance of the <see cref="InOrderEnumerator{TElement,TEndpoint}"/> class.</summary>
		/// <param name="tree">Tree enumerating.</param>
		/// <param name="asEnumerator">Set to true to instantiate this object in its <see cref="IEnumerator"/> state. Otherwise it
		/// will be in its <see cref="IEnumerable"/> state.</param>
		internal InOrderEnumerator(IntervalTree<TElement, TEndpoint> tree, bool asEnumerator = false)
			: base(tree, asEnumerator)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="InOrderEnumerator{TElement,TEndpoint}"/> class as a copy of the
		/// specified enumerator. Copied enumerators are always in the <see cref="IEnumerator"/> state.</summary>
		/// <param name="copy"><see cref="InOrderEnumerator{TElement,TEndpoint}"/> to copy.</param>
		private InOrderEnumerator(InOrderEnumerator<TElement, TEndpoint> copy)
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
			{
				stack = new Stack<IntervalNode<TElement, TEndpoint>>();
				FillStack(Tree.IRoot); // testing stack == null to determine "before first element" state
			}
			while (true)
			{
				if (stack.Count == 0)
					return false;

				CurrentNode = stack.Pop();

				if (CurrentNode.Right != Sentinel)
					FillStack(CurrentNode.Right);

				return true;
			}
		}

		/// <summary>Call copy ctor and return as an <see cref="IEnumerator{TElement}"/> instance.</summary>
		/// <returns>Copied instance.</returns>
		protected override IEnumerator<TElement> CallCopyCtor()
		{
			return new InOrderEnumerator<TElement, TEndpoint>(this);
		}

		/// <summary>Fills the stack with the node and the left child of the node (repeat).</summary>
		/// <param name="node">Node to fill the stack with.</param>
		private void FillStack(IntervalNode<TElement, TEndpoint> node)
		{
			if (node == Sentinel)
				return;
			while (true)
			{
				stack.Push(node); // always push the argument node

				if (node.Left != Sentinel)
					node = node.Left;
				else
					break;
			}
		}
	}
}
