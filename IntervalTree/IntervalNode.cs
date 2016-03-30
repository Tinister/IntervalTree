using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace IntervalTreeNS
{
	/// <summary>Represents the node in an <see cref="IntervalTree{TElement,TEndpoint}"/>.</summary>
	/// <typeparam name="TElement">The type of elements in the node.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
	internal sealed class IntervalNode<TElement, TEndpoint>
		where TElement : IInterval<TEndpoint>
		where TEndpoint : IComparable<TEndpoint>
	{
		/// <summary>A single sentinel object to use as a null object.</summary>
		internal static readonly IntervalNode<TElement, TEndpoint> Sentinel;

#if DEBUG
		/// <summary>Last id given to nodes of this type.</summary>
		// ReSharper disable once StaticMemberInGenericType
		private static int lastId = -1;
		/// <summary>Id of this node.</summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Can read this id during debug.")]
		private readonly int id = Interlocked.Increment(ref lastId);
#endif

		static IntervalNode()
		{
			Sentinel = new IntervalNode<TElement, TEndpoint>();
			// need to reset the sentinel's properties, as they're otherwise null
			Sentinel.Parent = Sentinel;
			Sentinel.Left = Sentinel;
			Sentinel.Right = Sentinel;
		}

		/// <summary>Initializes a new instance of the <see cref="IntervalNode{TElement, TEndpoint}"/> class.</summary>
		/// <param name="element">The element the node represents.</param>
		public IntervalNode(TElement element)
		{
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

		/// <summary>Gets the interval the node represents.</summary>
		public IInterval<TEndpoint> Interval { get; }

		/// <summary>Gets the color of this node.</summary>
		internal NodeColor Color { get; private set; } = NodeColor.Black;

		/// <summary>Gets or sets the parent node to this node.</summary>
		internal IntervalNode<TElement, TEndpoint> Parent { get; set; } = Sentinel;

		/// <summary>Gets or sets the left child node to this node.</summary>
		internal IntervalNode<TElement, TEndpoint> Left { get; set; } = Sentinel;

		/// <summary>Gets or sets the right child node to this node.</summary>
		internal IntervalNode<TElement, TEndpoint> Right { get; set; } = Sentinel;

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
