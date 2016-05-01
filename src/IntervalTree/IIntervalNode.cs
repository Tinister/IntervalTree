using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS
{
	/// <summary>Represents a node within an <see cref="IIntervalTree{TElement}"/>.</summary>
	/// <typeparam name="TElement">The type of element in the node.</typeparam>
	// ReSharper disable once TypeParameterCanBeVariant
	public interface IIntervalNode<TElement>
	{
		/// <summary>Gets the element the node represents.</summary>
		/// <value>The element the node represents.</value>
		TElement Element { get; }

		/// <summary>Gets the tree this node is included in.  Will be null if removed from its tree.</summary>
		/// <value>The tree this node is included in.</value>
		IIntervalTree<TElement> Tree { get; }

		/// <summary>Gets the parent node to this node.</summary>
		/// <value>The parent node to this node.</value>
		IIntervalNode<TElement> Parent { get; }

		/// <summary>Gets the left child node to this node.</summary>
		/// <value>The left child node to this node.</value>
		IIntervalNode<TElement> Left { get; }

		/// <summary>Gets the right child node to this node.</summary>
		/// <value>The right child node to this node.</value>
		IIntervalNode<TElement> Right { get; }
	}
}
