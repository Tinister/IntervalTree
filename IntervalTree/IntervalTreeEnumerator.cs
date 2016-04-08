using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS
{
	/// <summary>Class for enumerating an <see cref="IntervalTree{TElement,TEndpoint}"/>.</summary>
	/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
	internal sealed class IntervalTreeEnumerator<TElement, TEndpoint> : IEnumerator<TElement>
		where TElement : IInterval<TEndpoint>
		where TEndpoint : IComparable<TEndpoint>
	{
		/// <summary>A single sentinel object to use as boundaries of the tree.</summary>
		/// <remarks>Same reference as <see cref="IntervalNode{TElement,TEndpoint}.Sentinel"/> to prevent excessive typing.</remarks>
		internal static readonly IntervalNode<TElement, TEndpoint> Sentinel = IntervalNode<TElement, TEndpoint>.Sentinel;

		/// <summary>Tree enumerating.</summary>
		private readonly IntervalTree<TElement, TEndpoint> tree;

		/// <summary>Version of the tree when we started enumerating.</summary>
		private readonly int version;

		/// <summary>Keeping track of the nodes as we traverse.  If this field is null, the enumeration is finished.</summary>
		private Stack<IntervalNode<TElement, TEndpoint>> stack = new Stack<IntervalNode<TElement, TEndpoint>>();

		/// <summary>Node we moved to.</summary>
		private IntervalNode<TElement, TEndpoint> currentNode = Sentinel;

		/// <summary>Initializes a new instance of the <see cref="IntervalTreeEnumerator{TElement, TEndpoint}"/> class. </summary>
		/// <param name="tree">Tree enumerating.</param>
		internal IntervalTreeEnumerator(IntervalTree<TElement, TEndpoint> tree)
		{
			this.tree = tree;
			version = tree.Version;
			FillStack(tree.IRoot);
		}

		/// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
		/// <returns>The element in the collection at the current position of the enumerator.</returns>
		public TElement Current => currentNode == Sentinel ? default(TElement) : currentNode.Element;

		/// <summary>Gets the current element in the collection.</summary>
		/// <returns>The current element in the collection.</returns>
		object IEnumerator.Current => Current;

		/// <summary>Advances the enumerator to the next element of the collection.</summary>
		/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the
		/// end of the collection.</returns>
		/// <exception cref="InvalidOperationException">The collection was modified after the enumerator was created. </exception>
		public bool MoveNext()
		{
			if (tree.Version != version)
				throw new InvalidOperationException("Interval tree was modified.");
			if (stack == null)
				return false;

			if (stack.Count == 0)
			{
				currentNode = Sentinel;
				stack = null; // setting to null allows us to free memory a smidgen more aggressively
				return false;
			}
			currentNode = stack.Pop();
			FillStack(currentNode.Right);
			return true;
		}

		/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
		/// <exception cref="InvalidOperationException">The collection was modified after the enumerator was created. </exception>
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		void IDisposable.Dispose()
		{
			currentNode = Sentinel;
			stack = null;
		}

		/// <summary>Fills the stack with the node and the left child of the node (repeat).</summary>
		/// <param name="node">Node to fill the stack with.</param>
		private void FillStack(IntervalNode<TElement, TEndpoint> node)
		{
			while (node != Sentinel)
			{
				stack.Push(node);
				node = node.Left;
			}
		}
	}
}
