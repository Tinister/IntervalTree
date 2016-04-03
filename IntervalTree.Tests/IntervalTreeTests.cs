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
			// ReSharper disable once JoinDeclarationAndInitializer
			TreeHelper_ h;
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ nodeA = new IntervalNode_ { Name = "A" };
			IntervalNode_ nodeB = new IntervalNode_ { Name = "B" };

			h = new TreeHelper_(TreeHelper_.Build, tree);
			h.____.B(nodeB);
			h.B(nodeA);

			tree.LeftRotate(nodeA);

			h = new TreeHelper_(TreeHelper_.Assert, tree);
			h.B(nodeB);
			h.____.B(nodeA);
		}

		[Test]
		public void RightRotateTwoNodes()
		{
			// ReSharper disable once JoinDeclarationAndInitializer
			TreeHelper_ h;
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ nodeA = new IntervalNode_ { Name = "A" };
			IntervalNode_ nodeB = new IntervalNode_ { Name = "B" };

			h = new TreeHelper_(TreeHelper_.Build, tree);
			h.B(nodeA);
			h.____.B(nodeB);

			tree.RightRotate(nodeA);

			h = new TreeHelper_(TreeHelper_.Assert, tree);
			h.____.B(nodeA);
			h.B(nodeB);
		}

		[Test]
		public void LeftRotateMidTree()
		{
			// ReSharper disable once JoinDeclarationAndInitializer
			TreeHelper_ h;
			IntervalTree_ tree = new IntervalTree_();
			// rotating nodes
			IntervalNode_ nodeA = new IntervalNode_ { Name = "A" };
			IntervalNode_ nodeB = new IntervalNode_ { Name = "B" };
			// boundaries
			IntervalNode_ nodeW = new IntervalNode_ { Name = "W" };
			IntervalNode_ nodeX = new IntervalNode_ { Name = "X" };
			IntervalNode_ nodeY = new IntervalNode_ { Name = "Y" };
			IntervalNode_ nodeZ = new IntervalNode_ { Name = "Z" };

			h = new TreeHelper_(TreeHelper_.Build, tree);
			h.B(nodeW);
			h.____.____.____.B(nodeZ);
			h.____.____.B(nodeB);
			h.____.____.____.B(nodeY);
			h.____.B(nodeA);
			h.____.____.B(nodeX);

			tree.LeftRotate(nodeA);

			h = new TreeHelper_(TreeHelper_.Assert, tree);
			h.B(nodeW);
			h.____.____.B(nodeZ);
			h.____.B(nodeB);
			h.____.____.____.B(nodeY);
			h.____.____.B(nodeA);
			h.____.____.____.B(nodeX);
		}

		[Test]
		public void RightRotateMidTree()
		{
			// ReSharper disable once JoinDeclarationAndInitializer
			TreeHelper_ h;
			IntervalTree_ tree = new IntervalTree_();
			// rotating nodes
			IntervalNode_ nodeA = new IntervalNode_ { Name = "A" };
			IntervalNode_ nodeB = new IntervalNode_ { Name = "B" };
			// boundaries
			IntervalNode_ nodeW = new IntervalNode_ { Name = "W" };
			IntervalNode_ nodeX = new IntervalNode_ { Name = "X" };
			IntervalNode_ nodeY = new IntervalNode_ { Name = "Y" };
			IntervalNode_ nodeZ = new IntervalNode_ { Name = "Z" };

			h = new TreeHelper_(TreeHelper_.Build, tree);
			h.____.____.B(nodeX);
			h.____.B(nodeA);
			h.____.____.____.B(nodeZ);
			h.____.____.B(nodeB);
			h.____.____.____.B(nodeY);
			h.B(nodeW);

			tree.RightRotate(nodeA);

			h = new TreeHelper_(TreeHelper_.Assert, tree);
			h.____.____.____.B(nodeX);
			h.____.____.B(nodeA);
			h.____.____.____.B(nodeZ);
			h.____.B(nodeB);
			h.____.____.B(nodeY);
			h.B(nodeW);
		}

		[Test]
		public void LeftRotateWithNoRightChildDoesNotRotate()
		{
			// ReSharper disable once JoinDeclarationAndInitializer
			TreeHelper_ h;
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ nodeA = new IntervalNode_ { Name = "A" };
			IntervalNode_ nodeB = new IntervalNode_ { Name = "B" };

			h = new TreeHelper_(TreeHelper_.Build, tree);
			h.B(nodeA);
			h.____.B(nodeB);

			tree.LeftRotate(nodeA);

			h = new TreeHelper_(TreeHelper_.Assert, tree);
			h.B(nodeA);
			h.____.B(nodeB);
		}

		[Test]
		public void RightRotateWithNoLeftChildDoesNotRotate()
		{
			// ReSharper disable once JoinDeclarationAndInitializer
			TreeHelper_ h;
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ nodeA = new IntervalNode_ { Name = "A" };
			IntervalNode_ nodeB = new IntervalNode_ { Name = "B" };

			h = new TreeHelper_(TreeHelper_.Build, tree);
			h.____.B(nodeB);
			h.B(nodeA);

			tree.RightRotate(nodeA);

			h = new TreeHelper_(TreeHelper_.Assert, tree);
			h.____.B(nodeB);
			h.B(nodeA);
		}
	}
}
