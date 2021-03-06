﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS.TestHelpers
{
	/// <summary>Helper class for building or verifying a tree.</summary>
	/// <remarks>Only manipulates the
	/// <see cref="IntervalNode{TElement,TEndpoint}.Parent"/>, <see cref="IntervalNode{TElement,TEndpoint}.Left"/> and
	/// <see cref="IntervalNode{TElement,TEndpoint}.Right"/>
	/// fields directly.  Useful for wiring up those fields while making it look like an actual tree in the source code. =)</remarks>
	/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
	/// <typeparam name="TEndpoint">The type of the endpoints of the interval each element represents.</typeparam>
	internal sealed class TreeHelper<TElement, TEndpoint>
		where TElement : IInterval<TEndpoint>
		where TEndpoint : IComparable<TEndpoint>
	{
		private readonly ITreeActor actor;

		private readonly IntervalTree<TElement, TEndpoint> tree;

		private readonly Dictionary<int, IntervalNode<TElement, TEndpoint>> needsParent =
			new Dictionary<int, IntervalNode<TElement, TEndpoint>>();

		private readonly Dictionary<int, IntervalNode<TElement, TEndpoint>> needsLeftChild =
			new Dictionary<int, IntervalNode<TElement, TEndpoint>>();

		private int depth;

		private int maxDepth;

		public TreeHelper(ITreeActor actor, IntervalTree<TElement, TEndpoint> tree)
		{
			this.actor = actor;
			this.tree = tree;
		}

		/// <summary>Interface for the actions that can occur.</summary>
		internal interface ITreeActor
		{
			void DoRoot(IntervalTree<TElement, TEndpoint> tree, IntervalNode<TElement, TEndpoint> node);

			void DoLeftChild(IntervalNode<TElement, TEndpoint> parent, IntervalNode<TElement, TEndpoint> left);

			void DoRightChild(IntervalNode<TElement, TEndpoint> parent, IntervalNode<TElement, TEndpoint> right);

			void OnBlack(IntervalNode<TElement, TEndpoint> node);

			void OnRed(IntervalNode<TElement, TEndpoint> node);
		}

		/// <summary>Gets a tree actor that builds a tree.</summary>
		public static ITreeActor Build => new BuildActor();

		/// <summary>Gets a tree actor that asserts a tree was wired up as specified.</summary>
		public static ITreeActor Assert => new AssertActor();

#pragma warning disable SA1300 // Element must begin with upper-case letter
		/// <summary>Gets this <see cref="TreeHelper{TElement,TEndpoint}"/> instance but with the tracked depth incremented.
		/// Represents a "space" in the tree.</summary>
		public TreeHelper<TElement, TEndpoint> ____
		{
			get
			{
				depth++;
				maxDepth = Math.Max(depth, maxDepth);
				return this;
			}
		}
#pragma warning restore SA1300 // Element must begin with upper-case letter

		/// <summary>Set the specified node at the specified depth (where depth is the number of <see cref="____"/> invocations
		/// prior to this call). Will wire up the node based on previous calls to <see cref="B"/> or <see cref="R"/>.  Also sets
		/// the color to black.</summary>
		/// <param name="node">Node to wire up at the specified depth and set to black.</param>
		public void B(IntervalNode<TElement, TEndpoint> node)
		{
			actor.OnBlack(node);
			N(node);
		}

		/// <summary>Set the specified node at the specified depth (where depth is the number of <see cref="____"/> invocations
		/// prior to this call). Will wire up the node based on previous calls to <see cref="B"/> or <see cref="R"/>.  Also sets
		/// the color to black.</summary>
		/// <param name="node">Node to wire up at the specified depth and set to red.</param>
		public void R(IntervalNode<TElement, TEndpoint> node)
		{
			actor.OnRed(node);
			N(node);
		}

		/// <summary>Helper method for <see cref="B"/> and <see cref="R"/> to set the node at the specified depth.</summary>
		/// <param name="node">Node to wire up at the specified depth.</param>
		private void N(IntervalNode<TElement, TEndpoint> node)
		{
			IntervalNode<TElement, TEndpoint> temp;
			if (node.ITree != null && node.ITree != tree)
				throw new ArgumentException("Using wrong node for this tree.");

			// find parent
			if (depth == 0)
				actor.DoRoot(tree, node);
			else if (needsLeftChild.TryGetValue(depth - 1, out temp))
			{
				actor.DoLeftChild(temp, node);
				needsLeftChild.Remove(depth - 1);
			}
			else
				needsParent[depth] = node;

			// find right
			if (needsParent.TryGetValue(depth + 1, out temp))
			{
				actor.DoRightChild(node, temp);
				needsParent.Remove(depth + 1);
				for (int i = maxDepth; i > depth; i--)
					needsLeftChild.Remove(i); // any previous nodes of any further depth don't have left children
			}

			// mark as needing left (will come at next time this method is called).
			needsLeftChild[depth] = node;
			// reset
			depth = 0;
		}

		/// <summary>Tree actor that builds a tree.</summary>
		private class BuildActor : ITreeActor
		{
			public void DoRoot(IntervalTree<TElement, TEndpoint> tree, IntervalNode<TElement, TEndpoint> node)
			{
				if (tree == null || node == null)
					return;
				tree.IRoot = node;
			}

			public void DoLeftChild(IntervalNode<TElement, TEndpoint> parent, IntervalNode<TElement, TEndpoint> left)
			{
				if (parent == null || left == null)
					return;
				parent.Left = left;
				left.Parent = parent;
				while (parent != IntervalNode<TElement, TEndpoint>.Sentinel)
				{
					parent.UpdateMax();
					parent = parent.Parent;
				}
			}

			public void DoRightChild(IntervalNode<TElement, TEndpoint> parent, IntervalNode<TElement, TEndpoint> right)
			{
				if (parent == null || right == null)
					return;
				parent.Right = right;
				right.Parent = parent;
				while (parent != IntervalNode<TElement, TEndpoint>.Sentinel)
				{
					parent.UpdateMax();
					parent = parent.Parent;
				}
			}

			public void OnBlack(IntervalNode<TElement, TEndpoint> node)
			{
				if (node == null)
					return;
				node.Color = NodeColor.Black;
			}

			public void OnRed(IntervalNode<TElement, TEndpoint> node)
			{
				if (node == null)
					return;
				node.Color = NodeColor.Red;
			}
		}

		/// <summary>Tree actor that asserts a tree was wired up as specified.</summary>
		private class AssertActor : ITreeActor
		{
			public void DoRoot(IntervalTree<TElement, TEndpoint> tree, IntervalNode<TElement, TEndpoint> node)
			{
				if (tree == null)
					throw new ArgumentNullException(nameof(tree));
				if (node == null)
					throw new ArgumentNullException(nameof(node));
				NUnit.Framework.Assert.AreSame(tree.IRoot, node, $"Expected '{node.Name}' to be the root of the tree.");
			}

			public void DoLeftChild(IntervalNode<TElement, TEndpoint> parent, IntervalNode<TElement, TEndpoint> left)
			{
				if (parent == null)
					throw new ArgumentNullException(nameof(parent));
				if (left == null)
					throw new ArgumentNullException(nameof(left));
				NUnit.Framework.Assert.AreSame(
					parent.Left, left, $"Expected '{left.Name}' to be the left child of '{parent.Name}'.");
				NUnit.Framework.Assert.AreSame(left.Parent, parent, $"Expected '{parent.Name}' to be the parent of '{left.Name}'.");
			}

			public void DoRightChild(IntervalNode<TElement, TEndpoint> parent, IntervalNode<TElement, TEndpoint> right)
			{
				if (parent == null)
					throw new ArgumentNullException(nameof(parent));
				if (right == null)
					throw new ArgumentNullException(nameof(right));
				NUnit.Framework.Assert.AreSame(
					parent.Right, right, $"Expected '{right.Name}' to be the left child of '{parent.Name}'.");
				NUnit.Framework.Assert.AreSame(
					right.Parent, parent, $"Expected '{parent.Name}' to be the parent of '{right.Name}'.");
			}

			public void OnBlack(IntervalNode<TElement, TEndpoint> node)
			{
				if (node == null)
					throw new ArgumentNullException(nameof(node));
				NUnit.Framework.Assert.AreEqual(NodeColor.Black, node.Color, $"Expected '{node.Name}' to be black.");
			}

			public void OnRed(IntervalNode<TElement, TEndpoint> node)
			{
				if (node == null)
					throw new ArgumentNullException(nameof(node));
				NUnit.Framework.Assert.AreEqual(NodeColor.Red, node.Color, $"Expected '{node.Name}' to be red.");
			}
		}
	}
}
