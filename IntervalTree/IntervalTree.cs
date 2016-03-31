using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS
{
	/// <summary>Represents a strongly-typed collection of objects that can be represented by intervals.</summary>
	/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
	public class IntervalTree<TElement, TEndpoint>
		where TElement : IInterval<TEndpoint>
		where TEndpoint : IComparable<TEndpoint>
	{
		/// <summary>A single sentinel object to use as boundaries of the tree.</summary>
		/// <remarks>Same reference as <see cref="IntervalNode{TElement,TEndpoint}.Sentinel"/> to prevent excessive typing.</remarks>
		internal static readonly IntervalNode<TElement, TEndpoint> Sentinel = IntervalNode<TElement, TEndpoint>.Sentinel;

		/// <summary>Gets or sets the root of the tree.</summary>
		internal IntervalNode<TElement, TEndpoint> Root { get; set; } = Sentinel;

		/// <summary>Performs a left rotation operation on the specified node.</summary>
		/// <param name="node">Node to perform a left rotation on.</param>
		internal void LeftRotate(IntervalNode<TElement, TEndpoint> node)
		{
			if (node == Sentinel || node.Right == Sentinel)
				return; // don't rotate the sentinel up the tree
			IntervalNode<TElement, TEndpoint> temp = node.Right;

			node.Right = temp.Left;
			if (temp.Left != Sentinel)
				temp.Left.Parent = node;

			temp.Parent = node.Parent;
			if (node.Parent == Sentinel)
				Root = temp;
			else if (node == node.Parent.Left)
				node.Parent.Left = temp;
			else
				node.Parent.Right = temp;

			temp.Left = node;
			node.Parent = temp;
		}

		/// <summary>Performs a right rotation operation on the specified node.</summary>
		/// <param name="node">Node to perform a right rotation on.</param>
		internal void RightRotate(IntervalNode<TElement, TEndpoint> node)
		{
			if (node == Sentinel || node.Left == Sentinel)
				return; // don't rotate the sentinel up the tree
			IntervalNode<TElement, TEndpoint> temp = node.Left;

			node.Left = temp.Right;
			if (temp.Right != Sentinel)
				temp.Right.Parent = node;

			temp.Parent = node.Parent;
			if (node.Parent == Sentinel)
				Root = temp;
			else if (node == node.Parent.Left)
				node.Parent.Left = temp;
			else
				node.Parent.Right = temp;

			temp.Right = node;
			node.Parent = temp;
		}
	}
}
