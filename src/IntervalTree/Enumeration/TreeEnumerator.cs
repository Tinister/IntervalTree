using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace IntervalTreeNS.Enumeration
{
	/// <summary>Class for enumerating an <see cref="IntervalTree{TElement,TEndpoint}"/>.</summary>
	/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
	internal abstract class TreeEnumerator<TElement, TEndpoint> : IEnumerable<TElement>, IEnumerator<TElement>
		where TElement : IInterval<TEndpoint>
	{
		/// <summary>A single sentinel object to use as boundaries of the tree.</summary>
		/// <remarks>Same reference as <see cref="IntervalNode{TElement,TEndpoint}.Sentinel"/> to prevent excessive typing.</remarks>
		internal static readonly IntervalNode<TElement, TEndpoint> Sentinel = IntervalNode<TElement, TEndpoint>.Sentinel;

		/// <summary>The <see cref="Thread.ManagedThreadId"/> that constructed the object.</summary>
		private readonly int initialThreadId;

		/// <summary>Version of the tree when we started enumerating.</summary>
		/// <remarks>When the version has been set this object has transitioned into its IEnumerator state.</remarks>
		private int? version;

		/// <summary>Set to true when the enumeration has started.</summary>
		private EnumeratorState state;

		/// <summary>Initializes a new instance of the <see cref="TreeEnumerator{TElement, TEndpoint}"/> class.</summary>
		/// <param name="tree">Tree enumerating.</param>
		/// <param name="asEnumerator">Set to true to instantiate this object in its <see cref="IEnumerator"/> state. Otherwise it
		/// will be in its <see cref="IEnumerable"/> state.</param>
		protected TreeEnumerator(IntervalTree<TElement, TEndpoint> tree, bool asEnumerator = false)
		{
			initialThreadId = Thread.CurrentThread.ManagedThreadId;
			Tree = tree;
			state = EnumeratorState.NotAnEnumerator;
			if (asEnumerator)
			{
				version = tree.Version;
				state = EnumeratorState.Enumerating;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="TreeEnumerator{TElement, TEndpoint}"/> class as a copy of the
		/// specified enumerator. Copied enumerators are always in the <see cref="IEnumerator"/> state.</summary>
		/// <param name="copy"><see cref="TreeEnumerator{TElement,TEndpoint}"/> to copy.</param>
		protected TreeEnumerator(TreeEnumerator<TElement, TEndpoint> copy)
		{
			initialThreadId = Thread.CurrentThread.ManagedThreadId;
			Tree = copy.Tree;
			version = Tree.Version;
			state = EnumeratorState.Enumerating;
		}

		/// <summary>Different states an enumerator can be in.</summary>
		private enum EnumeratorState
		{
			/// <summary>Returned as an <see cref="IEnumerable"/> before <see cref="IEnumerable.GetEnumerator"/> is called.</summary>
			NotAnEnumerator = -1,

			/// <summary>Currently enumerating.</summary>
			Enumerating = 0,

			/// <summary>Finished enumerating.</summary>
			Finished = 1
		}

		/// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
		/// <returns>The element in the collection at the current position of the enumerator.</returns>
		public TElement Current => CurrentNode == Sentinel ? default(TElement) : CurrentNode.Element;

		/// <summary>Gets the current element in the collection.</summary>
		/// <returns>The current element in the collection.</returns>
		object IEnumerator.Current => Current;

		/// <summary>Gets the tree enumerating.</summary>
		protected IntervalTree<TElement, TEndpoint> Tree { get; }

		/// <summary>Gets or sets the node at the current position of the enumerator.</summary>
		protected IntervalNode<TElement, TEndpoint> CurrentNode { get; set; } = Sentinel;

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
			if (state == EnumeratorState.NotAnEnumerator && initialThreadId == Thread.CurrentThread.ManagedThreadId)
			{
				version = Tree.Version;
				state = EnumeratorState.Enumerating;
				return this; // no extra object instantiation
			}
			return CallCopyCtor();
		}

		/// <summary>Advances the enumerator to the next element of the collection.</summary>
		/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the
		/// end of the collection.</returns>
		/// <exception cref="InvalidOperationException">The collection was modified after the enumerator was created.</exception>
		public bool MoveNext()
		{
			if (state == EnumeratorState.NotAnEnumerator)
				return false;
			if (Tree.Version != version)
				throw new InvalidOperationException("Interval tree was modified.");
			if (state == EnumeratorState.Finished)
				return false;

			bool result = MoveNextCore();

			if (result == false)
			{
				state = EnumeratorState.Finished;
				CurrentNode = Sentinel;
			}
			return result;
		}

		/// <summary>Call copy ctor and return as an <see cref="IEnumerator{TElement}"/> instance.</summary>
		/// <returns>Copied instance.</returns>
		protected abstract IEnumerator<TElement> CallCopyCtor();

		/// <summary>Advances the enumerator to the next element of the collection.</summary>
		/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the
		/// end of the collection.</returns>
		/// <exception cref="InvalidOperationException">The collection was modified after the enumerator was created.</exception>
		protected abstract bool MoveNextCore();

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		protected virtual void Dispose()
		{
			if (state != EnumeratorState.NotAnEnumerator) // don't dispose if it hasn't started enumerating
			{
				state = EnumeratorState.Finished;
				CurrentNode = Sentinel;
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
