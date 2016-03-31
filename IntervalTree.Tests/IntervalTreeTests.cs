using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using IntervalTree_ = IntervalTreeNS.IntervalTree<IntervalTreeNS.Interval<int>, int>;
using IntervalNode_ = IntervalTreeNS.IntervalNode<IntervalTreeNS.Interval<int>, int>;

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

			tree.Root = nodeA;
			nodeA.Right = nodeB;
			nodeB.Parent = nodeA;

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

			tree.Root = nodeA;
			nodeA.Left = nodeB;
			nodeB.Parent = nodeA;

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

			tree.Root = nodeW;
			nodeW.Left = nodeA;
			nodeA.Parent = nodeW;

			nodeA.Left = nodeX;
			nodeA.Right = nodeB;
			nodeX.Parent = nodeA;
			nodeB.Parent = nodeA;

			nodeB.Left = nodeY;
			nodeB.Right = nodeZ;
			nodeY.Parent = nodeB;
			nodeZ.Parent = nodeB;

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

			tree.Root = nodeW;
			nodeW.Left = nodeA;
			nodeA.Parent = nodeW;

			nodeA.Left = nodeB;
			nodeA.Right = nodeX;
			nodeX.Parent = nodeA;
			nodeB.Parent = nodeA;

			nodeB.Left = nodeY;
			nodeB.Right = nodeZ;
			nodeY.Parent = nodeB;
			nodeZ.Parent = nodeB;

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

			tree.Root = nodeA;
			nodeA.Left = nodeB;
			nodeB.Parent = nodeA;

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

			tree.Root = nodeA;
			nodeA.Right = nodeB;
			nodeB.Parent = nodeA;

			tree.RightRotate(nodeA);

			Assert.AreSame(nodeA, tree.Root);
			Assert.AreSame(nodeB, nodeA.Right);
			Assert.AreSame(nodeA, nodeB.Parent);
		}
	}
}
