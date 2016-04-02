using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using IntervalTree_ = IntervalTreeNS.IntervalTree<IntervalTreeNS.Interval<int>, int>;
using IntervalNode_ = IntervalTreeNS.IntervalNode<IntervalTreeNS.Interval<int>, int>;
using TreeHelper_ = IntervalTreeNS.TestHelpers.TreeHelper<IntervalTreeNS.Interval<int>, int>;

namespace IntervalTreeNS
{
	[TestFixture]
	public class IntervalTreeTests
	{
		[Test]
		public void LeftRotateTwoNodes()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ nodeA = new IntervalNode_ { Name = "A" };
			IntervalNode_ nodeB = new IntervalNode_ { Name = "B" };

			TreeHelper_ b = new TreeHelper_(TreeHelper_.Build, tree);
			b.____.N(nodeB);
			b.N(nodeA);

			tree.LeftRotate(nodeA);

			TreeHelper_ a = new TreeHelper_(TreeHelper_.Assert, tree);
			a.N(nodeB);
			a.____.N(nodeA);
		}

		[Test]
		public void RightRotateTwoNodes()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ nodeA = new IntervalNode_ { Name = "A" };
			IntervalNode_ nodeB = new IntervalNode_ { Name = "B" };

			TreeHelper_ b = new TreeHelper_(TreeHelper_.Build, tree);
			b.N(nodeA);
			b.____.N(nodeB);

			tree.RightRotate(nodeA);

			TreeHelper_ a = new TreeHelper_(TreeHelper_.Assert, tree);
			a.____.N(nodeA);
			a.N(nodeB);
		}

		[Test]
		public void LeftRotateMidTree()
		{
			IntervalTree_ tree = new IntervalTree_();
			// rotating nodes
			IntervalNode_ nodeA = new IntervalNode_ { Name = "A" };
			IntervalNode_ nodeB = new IntervalNode_ { Name = "B" };
			// boundaries
			IntervalNode_ nodeW = new IntervalNode_ { Name = "W" };
			IntervalNode_ nodeX = new IntervalNode_ { Name = "X" };
			IntervalNode_ nodeY = new IntervalNode_ { Name = "Y" };
			IntervalNode_ nodeZ = new IntervalNode_ { Name = "Z" };

			TreeHelper_ b = new TreeHelper_(TreeHelper_.Build, tree);
			b.N(nodeW);
			b.____.____.____.N(nodeZ);
			b.____.____.N(nodeB);
			b.____.____.____.N(nodeY);
			b.____.N(nodeA);
			b.____.____.N(nodeX);

			tree.LeftRotate(nodeA);

			TreeHelper_ a = new TreeHelper_(TreeHelper_.Assert, tree);
			a.N(nodeW);
			a.____.____.N(nodeZ);
			a.____.N(nodeB);
			a.____.____.____.N(nodeY);
			a.____.____.N(nodeA);
			a.____.____.____.N(nodeX);
		}

		[Test]
		public void RightRotateMidTree()
		{
			IntervalTree_ tree = new IntervalTree_();
			// rotating nodes
			IntervalNode_ nodeA = new IntervalNode_ { Name = "A" };
			IntervalNode_ nodeB = new IntervalNode_ { Name = "B" };
			// boundaries
			IntervalNode_ nodeW = new IntervalNode_ { Name = "W" };
			IntervalNode_ nodeX = new IntervalNode_ { Name = "X" };
			IntervalNode_ nodeY = new IntervalNode_ { Name = "Y" };
			IntervalNode_ nodeZ = new IntervalNode_ { Name = "Z" };

			TreeHelper_ b = new TreeHelper_(TreeHelper_.Build, tree);
			b.____.____.N(nodeX);
			b.____.N(nodeA);
			b.____.____.____.N(nodeZ);
			b.____.____.N(nodeB);
			b.____.____.____.N(nodeY);
			b.N(nodeW);

			tree.RightRotate(nodeA);

			TreeHelper_ a = new TreeHelper_(TreeHelper_.Assert, tree);
			a.____.____.____.N(nodeX);
			a.____.____.N(nodeA);
			a.____.____.____.N(nodeZ);
			a.____.N(nodeB);
			a.____.____.N(nodeY);
			a.N(nodeW);
		}

		[Test]
		public void LeftRotateWithNoRightChildDoesNotRotate()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ nodeA = new IntervalNode_ { Name = "A" };
			IntervalNode_ nodeB = new IntervalNode_ { Name = "B" };

			TreeHelper_ b = new TreeHelper_(TreeHelper_.Build, tree);
			b.N(nodeA);
			b.____.N(nodeB);

			tree.LeftRotate(nodeA);

			TreeHelper_ a = new TreeHelper_(TreeHelper_.Assert, tree);
			a.N(nodeA);
			a.____.N(nodeB);
		}

		[Test]
		public void RightRotateWithNoLeftChildDoesNotRotate()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ nodeA = new IntervalNode_ { Name = "A" };
			IntervalNode_ nodeB = new IntervalNode_ { Name = "B" };

			TreeHelper_ b = new TreeHelper_(TreeHelper_.Build, tree);
			b.____.N(nodeB);
			b.N(nodeA);

			tree.RightRotate(nodeA);

			TreeHelper_ a = new TreeHelper_(TreeHelper_.Assert, tree);
			a.____.N(nodeB);
			a.N(nodeA);
		}
	}
}
