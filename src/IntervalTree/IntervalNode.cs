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
	{
		/// <summary>A single sentinel object to use as a null object.</summary>
		internal static readonly IntervalNode<TElement, TEndpoint> Sentinel;

		static IntervalNode()
		{
			Sentinel = new IntervalNode<TElement, TEndpoint>(null);
			// need to reset the sentinel's properties, as they're otherwise null
			Sentinel.Parent = Sentinel;
			Sentinel.Left = Sentinel;
			Sentinel.Right = Sentinel;
#if DEBUG
			Sentinel.Name = "NIL";
#endif
		}

		/// <summary>Initializes a new instance of the <see cref="IntervalNode{TElement, TEndpoint}"/> class.</summary>
		/// <param name="tree">The tree this node will belong to.</param>
		/// <param name="element">The element the node represents.</param>
		public IntervalNode(IntervalTree<TElement, TEndpoint> tree, TElement element)
		{
			if (tree == null)
				throw new ArgumentNullException(nameof(tree));
			if (element == null)
				throw new ArgumentNullException(nameof(element));
			Element = element;
			Interval = new Interval<TEndpoint>(element, tree.Comparer);
			// create a copy of the interval in case TElement changes...
			ITree = tree;
			Max = Interval.End;
		}

		/// <summary>Initializes a new instance of the <see cref="IntervalNode{TElement, TEndpoint}"/> class to represents an empty
		/// node.</summary>
		/// <param name="tree">The tree this node will belong to.</param>
		internal IntervalNode(IntervalTree<TElement, TEndpoint> tree)
		{
			Element = default(TElement);
			Interval = default(Interval<TEndpoint>);
			ITree = tree;
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

		/// <summary>Gets the tree this node belongs to.</summary>
		// ReSharper disable once InconsistentNaming
		internal IntervalTree<TElement, TEndpoint> ITree { get; }

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
			if (ITree == null)
				throw new InvalidOperationException("Cannot use this operation on node not part of a tree.");

			TEndpoint max = Interval.End;
			if (Left != Sentinel && ITree.Comparer.Compare(Left.Max, max) > 0)
				max = Left.Max;
			if (Right != Sentinel && ITree.Comparer.Compare(Right.Max, max) > 0)
				max = Right.Max;
			Max = max;
		}
	}
}
