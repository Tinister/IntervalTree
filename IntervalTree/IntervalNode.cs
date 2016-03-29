using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS
{
	/// <summary>Represents the node in an <see cref="IntervalTree{TElement,TEndpoint}"/>.</summary>
	/// <typeparam name="TElement">The type of elements in the node.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
	internal sealed class IntervalNode<TElement, TEndpoint>
		where TElement : IInterval<TEndpoint>
		where TEndpoint : IComparable<TEndpoint>
	{
		/// <summary>Initializes a new instance of the <see cref="IntervalNode{TElement, TEndpoint}"/> class.</summary>
		/// <param name="element">The element the node represents.</param>
		public IntervalNode(TElement element)
		{
			Element = element;
			Interval = new Interval<TEndpoint>(element); // create a copy of the interval in case TElement changes...
		}

		/// <summary>Initializes a new instance of the <see cref="IntervalNode{TElement, TEndpoint}"/> class to represents an empty
		/// node.</summary>
		internal IntervalNode()
		{
			Element = default(TElement);
			Interval = new Interval<TEndpoint>(default(TEndpoint), default(TEndpoint));
		}

		/// <summary>Gets the element the node represents.</summary>
		public TElement Element { get; }

		/// <summary>Gets the interval the node represents.</summary>
		public IInterval<TEndpoint> Interval { get; }

		/// <summary>Gets the color of this node.</summary>
		internal NodeColor Color { get; private set; } = NodeColor.Black;

		/// <summary>Gets the parent node to this node.</summary>
		internal IntervalNode<TElement, TEndpoint> Parent { get; private set; } = IntervalTree<TElement, TEndpoint>.Sentinel;

		/// <summary>Gets the left child node to this node.</summary>
		internal IntervalNode<TElement, TEndpoint> Left { get; private set; } = IntervalTree<TElement, TEndpoint>.Sentinel;

		/// <summary>Gets the right child node to this node.</summary>
		internal IntervalNode<TElement, TEndpoint> Right { get; private set; } = IntervalTree<TElement, TEndpoint>.Sentinel;

		/// <summary>Gets the maximum value of any endpoint in the subtree.</summary>
		internal TEndpoint Max { get; private set; } = default(TEndpoint);
	}

	/// <summary>Defines the valid colors of a <see cref="IntervalNode{TElement,TEndpoint}"/>.</summary>
	internal enum NodeColor
	{
		/// <summary>Represents the color black.</summary>
		Black = 0,

		/// <summary>Represents the color red.</summary>
		Red
	}
}
