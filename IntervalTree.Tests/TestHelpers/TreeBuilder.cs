using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS.TestHelpers
{
	/// <summary>Helper class for building a tree.</summary>
	/// <remarks>
	/// Only manipulates the
	/// <see cref="IntervalNode{TElement,TEndpoint}.Parent"/>, <see cref="IntervalNode{TElement,TEndpoint}.Left"/> and <see cref="IntervalNode{TElement,TEndpoint}.Right"/>
	/// fields directly.  Useful for wiring up those fields while making it look like an actual tree in the source code. =)
	/// Does no validation.
	/// </remarks>
	/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
	internal sealed class TreeBuilder<TElement, TEndpoint>
		where TElement : IInterval<TEndpoint>
		where TEndpoint : IComparable<TEndpoint>
	{
		private readonly IntervalTree<TElement, TEndpoint> tree;
		private readonly Dictionary<int, IntervalNode<TElement, TEndpoint>> needsParent = new Dictionary<int, IntervalNode<TElement, TEndpoint>>();
		private readonly Dictionary<int, IntervalNode<TElement, TEndpoint>> needsLeftChild = new Dictionary<int, IntervalNode<TElement, TEndpoint>>();
		private int depth;

		public TreeBuilder(IntervalTree<TElement, TEndpoint> tree)
		{
			this.tree = tree;
		}

#pragma warning disable SA1300 // Element must begin with upper-case letter
		/// <summary>
		/// Gets this <see cref="TreeBuilder{TElement,TEndpoint}"/> instance but with the tracked depth incremented.
		/// Represents a "space" in the tree.
		/// </summary>
		public TreeBuilder<TElement, TEndpoint> ____
		{
			get
			{
				depth++;
				return this;
			}
		}
#pragma warning restore SA1300 // Element must begin with upper-case letter

		/// <summary>
		/// Set the specified node at the specified depth (where depth is the number of <see cref="____"/> invocations prior to this call).
		/// Will wire up the node based on previous calls to <see cref="N"/></summary>
		/// <param name="node">Node to wire up at the specified depth.</param>
		public void N(IntervalNode<TElement, TEndpoint> node)
		{
			IntervalNode<TElement, TEndpoint> temp;

			// find parent
			if (depth == 0)
			{
				tree.Root = node;
			}
			else if (needsLeftChild.TryGetValue(depth - 1, out temp))
			{
				node.Parent = temp;
				temp.Left = node;
				needsLeftChild.Remove(depth - 1);
			}
			else
			{
				needsParent[depth] = node;
			}

			// find right
			if (needsParent.TryGetValue(depth + 1, out temp))
			{
				node.Right = temp;
				temp.Parent = node;
				needsParent.Remove(depth + 1);
				needsLeftChild.Remove(depth + 1); // doesn't have a left child
			}

			// mark as needing left (will come at next time this method is called).
			needsLeftChild[depth] = node;
			// reset
			depth = 0;
		}
	}
}
