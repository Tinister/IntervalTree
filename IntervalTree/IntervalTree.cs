using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS
{
	/// <summary>Represents a strongly-typed collection of objects that can be represented by intervals.</summary>
	/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "'Tree' is already a sufficient suffix.")]
	public class IntervalTree<TElement, TEndpoint> : IEnumerable<TElement>
		where TElement : IInterval<TEndpoint>
		where TEndpoint : IComparable<TEndpoint>
	{
		/// <summary>A single sentinel object to use as boundaries of the tree.</summary>
		/// <remarks>Same reference as <see cref="IntervalNode{TElement,TEndpoint}.Sentinel"/> to prevent excessive typing.</remarks>
		internal static readonly IntervalNode<TElement, TEndpoint> Sentinel = IntervalNode<TElement, TEndpoint>.Sentinel;

		/// <summary>Gets the root node of the tree.</summary>
		public IIntervalNode<TElement> Root => IRoot;

		/// <summary>Gets or sets the root node of the tree.</summary>
		// ReSharper disable once InconsistentNaming
		internal IntervalNode<TElement, TEndpoint> IRoot { get; set; } = Sentinel;

		/// <summary>Gets the current version of the tree.</summary>
		internal int Version { get; private set; }

		/// <summary>Adds the specified item to the interval tree.</summary>
		/// <param name="item">Item to add.</param>
		/// <returns>The node added to the tree that contains this item.</returns>
		public IIntervalNode<TElement> Add(TElement item)
		{
			Version++;
			IntervalNode<TElement, TEndpoint> node = new IntervalNode<TElement, TEndpoint>(item) { Tree = this };
			Insert(node);
			node.Color = NodeColor.Red;
			InsertFixup(node);
			return node;
		}

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>A <see cref="IEnumerator{TElement}"/> that can be used to iterate through the collection.</returns>
		public IEnumerator<TElement> GetEnumerator() => new IntervalTreeEnumerator<TElement, TEndpoint>(this);

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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
				IRoot = temp;
			else if (node == node.Parent.Left)
				node.Parent.Left = temp;
			else
				node.Parent.Right = temp;

			temp.Left = node;
			node.Parent = temp;

			node.UpdateMax();
			temp.UpdateMax();
			// don't need to continue up the tree as its subtree didn't change
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
				IRoot = temp;
			else if (node == node.Parent.Left)
				node.Parent.Left = temp;
			else
				node.Parent.Right = temp;

			temp.Right = node;
			node.Parent = temp;

			node.UpdateMax();
			temp.UpdateMax();
			// don't need to continue up the tree as its subtree didn't change
		}

		/// <summary>Inserts the node into the tree.</summary>
		/// <param name="node">Node to insert.</param>
		internal void Insert(IntervalNode<TElement, TEndpoint> node)
		{
			IntervalNode<TElement, TEndpoint> leaf = Sentinel;

			// find the proper leaf node
			IntervalNode<TElement, TEndpoint> curr = IRoot;
			while (curr != Sentinel)
			{
				leaf = curr;
				if (Comparer<TEndpoint>.Default.Compare(node.Max, curr.Max) > 0) // update the max on our way down
					curr.Max = node.Max;
				curr = Comparer<TEndpoint>.Default.Compare(node.Interval.Start, curr.Interval.Start) < 0 ? curr.Left : curr.Right;
			}

			node.Parent = leaf;
			if (leaf == Sentinel)
				IRoot = node;
			else if (Comparer<TEndpoint>.Default.Compare(node.Interval.Start, leaf.Interval.Start) < 0)
				leaf.Left = node;
			else
				leaf.Right = node;
		}

		/// <summary>Fixes up the tree based on red/black violations.</summary>
		/// <param name="node">Red node to look at.</param>
		internal void InsertFixup(IntervalNode<TElement, TEndpoint> node)
		{
			while (node.Parent.Color == NodeColor.Red)
			{
				if (node.Parent == node.Parent.Parent.Left)
				{
					IntervalNode<TElement, TEndpoint> uncle = node.Parent.Parent.Right;
					if (uncle.Color == NodeColor.Red)
					{
						node.Parent.Color = NodeColor.Black;
						uncle.Color = NodeColor.Black;
						node.Parent.Parent.Color = NodeColor.Red;
						node = node.Parent.Parent;
					}
					else
					{
						if (node == node.Parent.Right)
						{
							node = node.Parent;
							LeftRotate(node);
						}
						node.Parent.Color = NodeColor.Black;
						node.Parent.Parent.Color = NodeColor.Red;
						RightRotate(node.Parent.Parent);
					}
				}
				else // if (node.Parent == node.Parent.Parent.Right)
				{
					IntervalNode<TElement, TEndpoint> uncle = node.Parent.Parent.Left;
					if (uncle.Color == NodeColor.Red)
					{
						node.Parent.Color = NodeColor.Black;
						uncle.Color = NodeColor.Black;
						node.Parent.Parent.Color = NodeColor.Red;
						node = node.Parent.Parent;
					}
					else
					{
						if (node == node.Parent.Left)
						{
							node = node.Parent;
							RightRotate(node);
						}
						node.Parent.Color = NodeColor.Black;
						node.Parent.Parent.Color = NodeColor.Red;
						LeftRotate(node.Parent.Parent);
					}
				}
			}
			IRoot.Color = NodeColor.Black;
		}
	}
}
