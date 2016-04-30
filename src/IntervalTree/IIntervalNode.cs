using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS
{
	/// <summary>Represents a node in an <see cref="IntervalTree{TElement,TEndpoint}"/>.</summary>
	/// <typeparam name="TElement">The type of element in the node.</typeparam>
	// ReSharper disable once TypeParameterCanBeVariant
	public interface IIntervalNode<TElement>
	{
		/// <summary>Gets the element the node represents.</summary>
		TElement Element { get; }

		/// <summary>Gets the tree this node is included in.  Will be null if not in a tree.</summary>
		IIntervalTree<TElement> Tree { get; }

		/// <summary>Gets the parent node to this node.</summary>
		IIntervalNode<TElement> Parent { get; }

		/// <summary>Gets the left child node to this node.</summary>
		IIntervalNode<TElement> Left { get; }

		/// <summary>Gets the right child node to this node.</summary>
		IIntervalNode<TElement> Right { get; }
	}
}
