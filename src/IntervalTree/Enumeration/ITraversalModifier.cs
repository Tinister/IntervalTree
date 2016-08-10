using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS.Enumeration
{
	/// <summary>Object that's attached to a <see cref="TreeEnumerator{TElement,TEndpoint}"/> that modifies how the tree is
	/// traversed.</summary>
	/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
	internal interface ITraversalModifier<TElement, TEndpoint>
		where TElement : IInterval<TEndpoint>
	{
		/// <summary>Determines if the tree should traverse down the left subtree of the specified node.</summary>
		/// <remarks>The <see cref="TreeEnumerator{TElement,TEndpoint}"/> instance should determine that the node and the node's
		/// left child are not the <see cref="IntervalNode{TElement,TEndpoint}.Sentinel"/> before calling into this class.</remarks>
		/// <param name="node">Node in the tree currently at during traversal.</param>
		/// <returns><c>true</c> if we can traverse down the left subtree; <c>false</c> otherwise.</returns>
		bool CanGoLeft(IntervalNode<TElement, TEndpoint> node);

		/// <summary>Determines if the tree should traverse down the right subtree of the specified node.</summary>
		/// <remarks>The <see cref="TreeEnumerator{TElement,TEndpoint}"/> instance should determine that the node and the node's
		/// right child are not the <see cref="IntervalNode{TElement,TEndpoint}.Sentinel"/> before calling into this class.</remarks>
		/// <param name="node">Node in the tree currently at during traversal.</param>
		/// <returns><c>true</c> if we can traverse down the right subtree; <c>false</c> otherwise.</returns>
		bool CanGoRight(IntervalNode<TElement, TEndpoint> node);

		/// <summary>Determines if we can yield the specified node as part of the traversal.</summary>
		/// <remarks>The <see cref="TreeEnumerator{TElement,TEndpoint}"/> instance should determine that the node is not the
		/// <see cref="IntervalNode{TElement,TEndpoint}.Sentinel"/> before calling into this class.</remarks>
		/// <param name="node">Node in the tree currently at during traversal.</param>
		/// <returns><c>true</c> if we can traverse down the right subtree; <c>false</c> otherwise.</returns>
		bool CanYield(IntervalNode<TElement, TEndpoint> node);
	}

	/// <summary>Implementation of <see cref="ITraversalModifier{TElement,TEndpoint}"/> that doesn't modify anything at all.</summary>
	/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
	internal sealed class VoidModifier<TElement, TEndpoint> : ITraversalModifier<TElement, TEndpoint>
		where TElement : IInterval<TEndpoint>
	{
		/// <summary>Return that the traversal can always go left.</summary>
		/// <param name="node">Node in the tree currently at during traversal.</param>
		/// <returns><c>true</c></returns>
		public bool CanGoLeft(IntervalNode<TElement, TEndpoint> node) => true;

		/// <summary>Return that the traversal can always go right.</summary>
		/// <param name="node">Node in the tree currently at during traversal.</param>
		/// <returns><c>true</c></returns>
		public bool CanGoRight(IntervalNode<TElement, TEndpoint> node) => true;

		/// <summary>Return that the traversal can always yield.</summary>
		/// <param name="node">Node in the tree currently at during traversal.</param>
		/// <returns><c>true</c></returns>
		public bool CanYield(IntervalNode<TElement, TEndpoint> node) => true;
	}
}
