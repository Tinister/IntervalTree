using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS
{
	/// <summary>Represents a node in an <see cref="IntervalTree{TElement,TEndpoint}"/>.</summary>
	/// <typeparam name="TElement">The type of element in the node.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval the element represents.</typeparam>
	internal sealed class IntervalNode<TElement, TEndpoint> : IIntervalNode<TElement>
		where TElement : IInterval<TEndpoint>
		where TEndpoint : IComparable<TEndpoint>
	{
		/// <summary>A single sentinel object to use as a null object.</summary>
		internal static readonly IntervalNode<TElement, TEndpoint> Sentinel;

		static IntervalNode()
		{
			Sentinel = new IntervalNode<TElement, TEndpoint>();
			// need to reset the sentinel's properties, as they're otherwise null
			Sentinel.Parent = Sentinel;
			Sentinel.Left = Sentinel;
			Sentinel.Right = Sentinel;
#if DEBUG
			Sentinel.Name = "NIL";
#endif
		}

		/// <summary>Initializes a new instance of the <see cref="IntervalNode{TElement, TEndpoint}"/> class.</summary>
		/// <param name="element">The element the node represents.</param>
		public IntervalNode(TElement element)
		{
			if (element == null)
				throw new ArgumentNullException(nameof(element));
			Element = element;
			Interval = new Interval<TEndpoint>(element); // create a copy of the interval in case TElement changes...
			Max = Interval.End;
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

		/// <summary>Gets the tree this node is included in.  Will be null if not in a tree.</summary>
		public IIntervalTree<TElement> Tree => ITree;

		/// <summary>Gets the interval the node represents.</summary>
		public IInterval<TEndpoint> Interval { get; }

		/// <summary>Gets the parent node to this node.</summary>
		IIntervalNode<TElement> IIntervalNode<TElement>.Parent => Parent == Sentinel ? null : Parent;

		/// <summary>Gets the left child node to this node.</summary>
		IIntervalNode<TElement> IIntervalNode<TElement>.Left => Left == Sentinel ? null : Left;

		/// <summary>Gets the right child node to this node.</summary>
		IIntervalNode<TElement> IIntervalNode<TElement>.Right => Right == Sentinel ? null : Right;

#if DEBUG
		/// <summary>Gets or sets the name of the node for debug.</summary>
		internal string Name { get; set; }
#endif

		/// <summary>Gets or sets the tree this node belongs to.</summary>
		// ReSharper disable once InconsistentNaming
		internal IntervalTree<TElement, TEndpoint> ITree { get; set; }

		/// <summary>Gets or sets the color of this node.</summary>
		internal NodeColor Color { get; set; } = NodeColor.Black;

		/// <summary>Gets or sets the parent node to this node.</summary>
		internal IntervalNode<TElement, TEndpoint> Parent { get; set; } = Sentinel;

		/// <summary>Gets or sets the left child node to this node.</summary>
		internal IntervalNode<TElement, TEndpoint> Left { get; set; } = Sentinel;

		/// <summary>Gets or sets the right child node to this node.</summary>
		internal IntervalNode<TElement, TEndpoint> Right { get; set; } = Sentinel;

		/// <summary>Gets or sets the maximum value of any endpoint in the subtree.</summary>
		internal TEndpoint Max { get; set; }

		/// <summary>Updates <see cref="Max"/> by checking the actual maximum between itself and its direct children.</summary>
		internal void UpdateMax()
		{
			TEndpoint max = Interval.End;
			if (Left != Sentinel && Comparer<TEndpoint>.Default.Compare(Left.Max, max) > 0)
				max = Left.Max;
			if (Right != Sentinel && Comparer<TEndpoint>.Default.Compare(Right.Max, max) > 0)
				max = Right.Max;
			Max = max;
		}
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
