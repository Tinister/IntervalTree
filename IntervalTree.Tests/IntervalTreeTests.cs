using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using IntervalTree_ = IntervalTreeNS.IntervalTree<IntervalTreeNS.Interval<int>, int>;
using IntervalNode_ = IntervalTreeNS.IntervalNode<IntervalTreeNS.Interval<int>, int>;
using TreeBuilder_ = IntervalTreeNS.TestHelpers.TreeBuilder<IntervalTreeNS.Interval<int>, int>;

namespace IntervalTreeNS
{
	[TestFixture]
	public class IntervalTreeTests
	{
		[Test]
		public void LeftRotateTwoNodes()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ nodeA = new IntervalNode_();
			IntervalNode_ nodeB = new IntervalNode_();

			TreeBuilder_ b = new TreeBuilder_(tree);
			b.____.N(nodeB);
			b.N(nodeA);

			tree.LeftRotate(nodeA);

			Assert.AreSame(nodeB, tree.Root);
			Assert.AreSame(nodeA, nodeB.Left);
			Assert.AreSame(nodeB, nodeA.Parent);
		}

		[Test]
		public void RightRotateTwoNodes()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ nodeA = new IntervalNode_();
			IntervalNode_ nodeB = new IntervalNode_();

			TreeBuilder_ b = new TreeBuilder_(tree);
			b.N(nodeA);
			b.____.N(nodeB);

			tree.RightRotate(nodeA);

			Assert.AreSame(nodeB, tree.Root);
			Assert.AreSame(nodeA, nodeB.Right);
			Assert.AreSame(nodeB, nodeA.Parent);
		}

		[Test]
		public void LeftRotateMidTree()
		{
			IntervalTree_ tree = new IntervalTree_();
			// rotating nodes
			IntervalNode_ nodeA = new IntervalNode_();
			IntervalNode_ nodeB = new IntervalNode_();
			// boundaries
			IntervalNode_ nodeW = new IntervalNode_();
			IntervalNode_ nodeX = new IntervalNode_();
			IntervalNode_ nodeY = new IntervalNode_();
			IntervalNode_ nodeZ = new IntervalNode_();

			TreeBuilder_ b = new TreeBuilder_(tree);
			b.N(nodeW);
			b.____.____.____.N(nodeZ);
			b.____.____.N(nodeB);
			b.____.____.____.N(nodeY);
			b.____.N(nodeA);
			b.____.____.N(nodeX);

			tree.LeftRotate(nodeA);

			Assert.AreSame(nodeW, tree.Root);
			Assert.AreSame(nodeB, nodeW.Left);
			Assert.AreSame(nodeW, nodeB.Parent);

			Assert.AreSame(nodeA, nodeB.Left);
			Assert.AreSame(nodeZ, nodeB.Right);
			Assert.AreSame(nodeB, nodeA.Parent);
			Assert.AreSame(nodeB, nodeZ.Parent);

			Assert.AreSame(nodeX, nodeA.Left);
			Assert.AreSame(nodeY, nodeA.Right);
			Assert.AreSame(nodeA, nodeX.Parent);
			Assert.AreSame(nodeA, nodeY.Parent);
		}

		[Test]
		public void RightRotateMidTree()
		{
			IntervalTree_ tree = new IntervalTree_();
			// rotating nodes
			IntervalNode_ nodeA = new IntervalNode_();
			IntervalNode_ nodeB = new IntervalNode_();
			// boundaries
			IntervalNode_ nodeW = new IntervalNode_();
			IntervalNode_ nodeX = new IntervalNode_();
			IntervalNode_ nodeY = new IntervalNode_();
			IntervalNode_ nodeZ = new IntervalNode_();

			TreeBuilder_ b = new TreeBuilder_(tree);
			b.N(nodeW);
			b.____.____.N(nodeX);
			b.____.N(nodeA);
			b.____.____.____.N(nodeZ);
			b.____.____.N(nodeB);
			b.____.____.____.N(nodeY);

			tree.RightRotate(nodeA);

			Assert.AreSame(nodeW, tree.Root);
			Assert.AreSame(nodeB, nodeW.Left);
			Assert.AreSame(nodeW, nodeB.Parent);

			Assert.AreSame(nodeY, nodeB.Left);
			Assert.AreSame(nodeA, nodeB.Right);
			Assert.AreSame(nodeB, nodeA.Parent);
			Assert.AreSame(nodeB, nodeY.Parent);

			Assert.AreSame(nodeZ, nodeA.Left);
			Assert.AreSame(nodeX, nodeA.Right);
			Assert.AreSame(nodeA, nodeZ.Parent);
			Assert.AreSame(nodeA, nodeX.Parent);
		}

		[Test]
		public void LeftRotateWithNoRightChildDoesNotRotate()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ nodeA = new IntervalNode_();
			IntervalNode_ nodeB = new IntervalNode_();

			TreeBuilder_ b = new TreeBuilder_(tree);
			b.N(nodeA);
			b.____.N(nodeB);

			tree.LeftRotate(nodeA);

			Assert.AreSame(nodeA, tree.Root);
			Assert.AreSame(nodeB, nodeA.Left);
			Assert.AreSame(nodeA, nodeB.Parent);
		}

		[Test]
		public void RightRotateWithNoLeftChildDoesNotRotate()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ nodeA = new IntervalNode_();
			IntervalNode_ nodeB = new IntervalNode_();

			TreeBuilder_ b = new TreeBuilder_(tree);
			b.____.N(nodeB);
			b.N(nodeA);

			tree.RightRotate(nodeA);

			Assert.AreSame(nodeA, tree.Root);
			Assert.AreSame(nodeB, nodeA.Right);
			Assert.AreSame(nodeA, nodeB.Parent);
		}
	}
}
