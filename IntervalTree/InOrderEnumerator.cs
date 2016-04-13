using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace IntervalTreeNS
{
	/// <summary>Class for enumerating an <see cref="IntervalTree{TElement,TEndpoint}"/>.</summary>
	/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
	internal class InOrderEnumerator<TElement, TEndpoint> : IEnumerable<TElement>, IEnumerator<TElement>
		where TElement : IInterval<TEndpoint>
		where TEndpoint : IComparable<TEndpoint>
	{
		/// <summary>A single sentinel object to use as boundaries of the tree.</summary>
		/// <remarks>Same reference as <see cref="IntervalNode{TElement,TEndpoint}.Sentinel"/> to prevent excessive typing.</remarks>
		internal static readonly IntervalNode<TElement, TEndpoint> Sentinel = IntervalNode<TElement, TEndpoint>.Sentinel;

		/// <summary>The <see cref="Thread.ManagedThreadId"/> that constructed the object.</summary>
		private readonly int initialThreadId;

		/// <summary>Tree enumerating.</summary>
		private readonly IntervalTree<TElement, TEndpoint> tree;

		/// <summary>Version of the tree when we started enumerating.</summary>
		/// <remarks>When the version has been set this object has transitioned into its IEnumerator state.</remarks>
		private int? version;

		/// <summary>Set to true when the enumeration has started.</summary>
		private bool enumerationStarted;

		/// <summary>Keeping track of the nodes as we traverse.  If this field is null, the enumeration is finished.</summary>
		private Stack<IntervalNode<TElement, TEndpoint>> stack;

		/// <summary>Node we moved to.</summary>
		private IntervalNode<TElement, TEndpoint> currentNode = Sentinel;

		/// <summary>Initializes a new instance of the <see cref="InOrderEnumerator{TElement,TEndpoint}"/> class. </summary>
		/// <param name="tree">Tree enumerating.</param>
		/// <param name="asEnumerator">Set to true to instantiate this object in its <see cref="IEnumerator"/> state. Otherwise it
		/// will be in its <see cref="IEnumerable"/> state.</param>
		internal InOrderEnumerator(IntervalTree<TElement, TEndpoint> tree, bool asEnumerator = false)
		{
			initialThreadId = Thread.CurrentThread.ManagedThreadId;
			this.tree = tree;
			if (asEnumerator)
			{
				version = tree.Version;
				stack = new Stack<IntervalNode<TElement, TEndpoint>>();
			}
		}

		/// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
		/// <returns>The element in the collection at the current position of the enumerator.</returns>
		public TElement Current => currentNode == Sentinel ? default(TElement) : currentNode.Element;

		/// <summary>Gets the current element in the collection.</summary>
		/// <returns>The current element in the collection.</returns>
		object IEnumerator.Current => Current;

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
		/// <exception cref="InvalidOperationException">The collection was modified after the enumerator was created.</exception>
		void IEnumerator.Reset() => Reset();

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		void IDisposable.Dispose() => Dispose();

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>An <see cref="IEnumerator{TElement}"/> that can be used to iterate through the collection.</returns>
		public IEnumerator<TElement> GetEnumerator()
		{
			if (!version.HasValue && initialThreadId == Thread.CurrentThread.ManagedThreadId)
			{
				version = tree.Version;
				stack = new Stack<IntervalNode<TElement, TEndpoint>>();
				return this; // no extra object instantiation
			}
			return CloneAsEnumerator(tree);
		}

		/// <summary>Advances the enumerator to the next element of the collection.</summary>
		/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the
		/// end of the collection.</returns>
		/// <exception cref="InvalidOperationException">The collection was modified after the enumerator was created. </exception>
		public bool MoveNext()
		{
			if (!version.HasValue)
				return false;
			if (tree.Version != version)
				throw new InvalidOperationException("Interval tree was modified.");
			if (stack == null)
				return false;
			if (!enumerationStarted)
			{
				FillStack(tree.IRoot);
				enumerationStarted = true;
			}

			while (true)
			{
				if (stack.Count == 0)
				{
					currentNode = Sentinel;
					stack = null; // setting to null allows us to free memory a smidgen more aggressively
					return false;
				}
				currentNode = stack.Pop();

				if (currentNode.Right != Sentinel && GoRight(currentNode))
					FillStack(currentNode.Right);

				if (StopHere(currentNode))
					return true;
			}
		}

		/// <summary>Fills the stack with the node and the left child of the node (repeat).</summary>
		/// <param name="node">Node to fill the stack with.</param>
		protected void FillStack(IntervalNode<TElement, TEndpoint> node)
		{
			if (node == Sentinel)
				return;
			while (true)
			{
				stack.Push(node); // always push the argument node

				if (node.Left != Sentinel && GoLeft(node))
					node = node.Left;
				else
					break;
			}
		}

		/// <summary>Clone the current instance as an <see cref="IEnumerator{TElement}"/> instance.</summary>
		/// <param name="tree">Tree enumerating.</param>
		/// <returns>This instance cloned.</returns>
		// ReSharper disable once ParameterHidesMember
		protected virtual IEnumerator<TElement> CloneAsEnumerator(IntervalTree<TElement, TEndpoint> tree)
		{
			// it's cumpulsory that derived types override this =/
			return new InOrderEnumerator<TElement, TEndpoint>(tree, true);
		}

		/// <summary>Determine whether the enumerator continue down the left side of the specified subtree.</summary>
		/// <param name="node">Node at the top of the subtree before the enumerator is at before it decides to go left.</param>
		/// <returns>true to continue down the left subtree, false to skip the left subtree.</returns>
		protected virtual bool GoLeft(IntervalNode<TElement, TEndpoint> node) => true;

		/// <summary>Determine whether the enumerator continue down the right side of the specified subtree.</summary>
		/// <param name="node">Node at the top of the subtree before the enumerator is at before it decides to go right.</param>
		/// <returns>true to continue down the right subtree, false to skip the right subtree.</returns>
		protected virtual bool GoRight(IntervalNode<TElement, TEndpoint> node) => true;

		/// <summary>Determine whether to stop at the specified node. If true, will set to <see cref="Current"/> and return.</summary>
		/// <param name="node">Node the enumerator is currently stopped at.</param>
		/// <returns>true to stop at this node and return it to the enumerator's caller.</returns>
		protected virtual bool StopHere(IntervalNode<TElement, TEndpoint> node) => true;

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		protected void Dispose()
		{
			if (version.HasValue) // don't dispose if it hasn't started enumerating
			{
				currentNode = Sentinel;
				stack = null;
			}
		}

		/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
		/// <exception cref="InvalidOperationException">The collection was modified after the enumerator was created. </exception>
		protected static void Reset()
		{
			throw new NotSupportedException();
		}
	}
}
