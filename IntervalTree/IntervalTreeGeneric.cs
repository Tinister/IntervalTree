using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using IntervalTreeNS.Enumeration;

namespace IntervalTreeNS
{
	/// <summary>Represents a strongly-typed collection of objects that can be represented by intervals.</summary>
	/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
		Justification = "'Tree' is already a sufficient suffix.")]
#pragma warning disable SA1649 // File name must match first type name
	internal class IntervalTree<TElement, TEndpoint> : IIntervalTree<TElement>
#pragma warning restore SA1649 // File name must match first type name
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
			Insert(node, IRoot);
			node.Color = NodeColor.Red;
			InsertFixup(node);
			return node;
		}

		/// <summary>Adds the specified item to the interval tree and also gets all intersecting elements at the time of addition.</summary>
		/// <param name="item">Item to add.</param>
		/// <param name="intersectingCollection">Collection to add intersecting elements to.  Will call
		/// <see cref="ICollection{TElement}.Add"/> for each element.</param>
		/// <param name="alsoAdjacent">Set to true to also get all adjacent elements.</param>
		/// <returns>The node added to the tree that contains this item.</returns>
		public IIntervalNode<TElement> Add(
			TElement item, ICollection<TElement> intersectingCollection, bool alsoAdjacent = false)
		{
			if (intersectingCollection == null)
				throw new ArgumentNullException(nameof(intersectingCollection));
			if (intersectingCollection.IsReadOnly)
				throw new ArgumentException($"{nameof(intersectingCollection)} must not be readonly.");

			IntersectingInsertEnumerator<TElement, TEndpoint> enumerator =
				new IntersectingInsertEnumerator<TElement, TEndpoint>(this, item, alsoAdjacent);
			using (enumerator)
			{
				while (enumerator.MoveNext())
					intersectingCollection.Add(enumerator.Current);
			}

			Version++;
			IntervalNode<TElement, TEndpoint> node = new IntervalNode<TElement, TEndpoint>(item) { Tree = this };
			Insert(node, enumerator.InsertionPoint);
			node.Color = NodeColor.Red;
			InsertFixup(node);
			return node;
		}

		/// <summary>Returns an enumerator that iterates through all elements that intersect the specified interval.</summary>
		/// <param name="item">Interval to use to find all intersecting elements.</param>
		/// <param name="alsoAdjacent">Set to true to also get all adjacent elements.</param>
		/// <returns>An enumerator for all intersecting elements.</returns>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "Just some oddness with the IEnumerable/IEnumerator duality.")]
		public IEnumerable<TElement> FindAllIntersecting(TElement item, bool alsoAdjacent = false)
		{
			return new IntersectingEnumerator<TElement, TEndpoint>(this, item, alsoAdjacent);
		}

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>A <see cref="IEnumerator{TElement}"/> that can be used to iterate through the collection.</returns>
		public IEnumerator<TElement> GetEnumerator() => new InOrderEnumerator<TElement, TEndpoint>(this, true);

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
			// don't need to continue doing `UpdateMax` up the tree as parent's subtree didn't change
		}

		/// <summary>Inserts the node into the tree.</summary>
		/// <param name="node">Node to insert.</param>
		/// <param name="insertionPoint">Node representing the subtree to insert <paramref name="node"/> into.</param>
		/// <remarks><paramref name="insertionPoint"/>'s purpose is not to allow inserting nodes at arbitrary subtrees. Rather, if
		/// coupling insert operation with something else, and that something else can figure out where the node should be
		/// inserted, then this method need not recompute it.</remarks>
		internal void Insert(IntervalNode<TElement, TEndpoint> node, IntervalNode<TElement, TEndpoint> insertionPoint)
		{
			// find the proper insertion point
			IntervalNode<TElement, TEndpoint> curr = insertionPoint == Sentinel ? IRoot : insertionPoint;
			while (curr != Sentinel)
			{
				insertionPoint = curr;
				if (Comparer<TEndpoint>.Default.Compare(node.Max, curr.Max) > 0) // update the max on our way down
					curr.Max = node.Max;
				curr = Comparer<TEndpoint>.Default.Compare(node.Interval.Start, curr.Interval.Start) < 0 ? curr.Left : curr.Right;
			}

			node.Parent = insertionPoint;
			if (insertionPoint == Sentinel)
				IRoot = node;
			else if (Comparer<TEndpoint>.Default.Compare(node.Interval.Start, insertionPoint.Interval.Start) < 0)
				insertionPoint.Left = node;
			else
				insertionPoint.Right = node;
		}

		/// <summary>Fixes up the tree based on red/black violations.</summary>
		/// <param name="node">Red node to look at.</param>
		internal void InsertFixup(IntervalNode<TElement, TEndpoint> node)
		{
			while (node.Parent.Color == NodeColor.Red)
			{
				// set up some variables for symmetric algorithm
				IntervalNode<TElement, TEndpoint> uncle;
				bool isUncleSideChild;
				Action<IntervalNode<TElement, TEndpoint>> rotateAwayFromUncle;
				Action<IntervalNode<TElement, TEndpoint>> rotateTowardsUncle;
				if (node.Parent == node.Parent.Parent.Left)
				{
					uncle = node.Parent.Parent.Right;
					isUncleSideChild = node == node.Parent.Right;
					rotateAwayFromUncle = LeftRotate;
					rotateTowardsUncle = RightRotate;
				}
				else // if (node.Parent == node.Parent.Parent.Right)
				{
					uncle = node.Parent.Parent.Left;
					isUncleSideChild = node == node.Parent.Left;
					rotateAwayFromUncle = RightRotate;
					rotateTowardsUncle = LeftRotate;
				}

				// algorithm start
				if (uncle.Color == NodeColor.Red)
					// if uncle is red we can just recolor nodes and then check again at the grandparent
				{
					node.Parent.Color = NodeColor.Black;
					uncle.Color = NodeColor.Black;
					node.Parent.Parent.Color = NodeColor.Red;
					node = node.Parent.Parent;
				}
				else // otherwise we will need to do some rotations to rebalance the tree
				{
					if (isUncleSideChild)
					{
						node = node.Parent;
						rotateAwayFromUncle(node);
					}
					node.Parent.Color = NodeColor.Black;
					node.Parent.Parent.Color = NodeColor.Red;
					rotateTowardsUncle(node.Parent.Parent);
				}
			}
			IRoot.Color = NodeColor.Black;
		}
	}
}
