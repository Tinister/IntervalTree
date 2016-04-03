using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntervalTreeNS.TestHelpers;
using NUnit.Framework;
using IntervalTree_ = IntervalTreeNS.IntervalTree<IntervalTreeNS.Interval<int>, int>;
using IntervalNode_ = IntervalTreeNS.IntervalNode<IntervalTreeNS.Interval<int>, int>;
using TreeHelper_ = IntervalTreeNS.TestHelpers.TreeHelper<IntervalTreeNS.Interval<int>, int>;

namespace IntervalTreeNS
{
	[TestFixture]
	public class IntervalTreeInsertTests
	{
		[Test]
		public void RootIsSetIfThereIsNotOne()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ node = new IntervalNode_();

			tree.Insert(node);
			Assert.AreSame(node, tree.Root);
		}

		[Test]
		public void InsertOnTheLeft()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ root = new IntervalNode_(new Interval<int>(512, 512)) { Name = "X" };
			tree.Root = root;

			IntervalNode_ node = new IntervalNode_(new Interval<int>(510, 510)) { Name = "N" };
			tree.Insert(node);

			TreeHelper_ h = new TreeHelper_(TreeHelper_.Assert, tree);
			h.B(root);
			h.____.B(node);
		}

		[Test]
		public void InsertOnTheRight()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ root = new IntervalNode_(new Interval<int>(512, 512)) { Name = "X" };
			tree.Root = root;

			IntervalNode_ node = new IntervalNode_(new Interval<int>(515, 515)) { Name = "N" };
			tree.Insert(node);

			TreeHelper_ h = new TreeHelper_(TreeHelper_.Assert, tree);
			h.____.B(node);
			h.B(root);
		}

		[Test]
		public void InsertFirstGrandChild()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ root = new IntervalNode_(new Interval<int>(512, 512)) { Name = "X" };
			IntervalNode_ left = new IntervalNode_(new Interval<int>(256, 256)) { Name = "L" };
			IntervalNode_ right = new IntervalNode_(new Interval<int>(768, 768)) { Name = "R" };
			tree.Insert(root);
			tree.Insert(left);
			tree.Insert(right);

			IntervalNode_ node = new IntervalNode_(new Interval<int>(250, 250)) { Name = "N" };
			tree.Insert(node);

			TreeHelper_ h = new TreeHelper_(TreeHelper_.Assert, tree);
			h.____.B(right);
			h.B(root);
			h.____.B(left);
			h.____.____.B(node);
		}

		[Test]
		public void InsertSecondGrandChild()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ root = new IntervalNode_(new Interval<int>(512, 512)) { Name = "X" };
			IntervalNode_ left = new IntervalNode_(new Interval<int>(256, 256)) { Name = "L" };
			IntervalNode_ right = new IntervalNode_(new Interval<int>(768, 768)) { Name = "R" };
			tree.Insert(root);
			tree.Insert(left);
			tree.Insert(right);

			IntervalNode_ node = new IntervalNode_(new Interval<int>(510, 510)) { Name = "N" };
			tree.Insert(node);

			TreeHelper_ h = new TreeHelper_(TreeHelper_.Assert, tree);
			h.____.B(right);
			h.B(root);
			h.____.____.B(node);
			h.____.B(left);
		}

		[Test]
		public void InsertThirdGrandChild()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ root = new IntervalNode_(new Interval<int>(512, 512)) { Name = "X" };
			IntervalNode_ left = new IntervalNode_(new Interval<int>(256, 256)) { Name = "L" };
			IntervalNode_ right = new IntervalNode_(new Interval<int>(768, 768)) { Name = "R" };
			tree.Insert(root);
			tree.Insert(left);
			tree.Insert(right);

			IntervalNode_ node = new IntervalNode_(new Interval<int>(515, 515)) { Name = "N" };
			tree.Insert(node);

			TreeHelper_ h = new TreeHelper_(TreeHelper_.Assert, tree);
			h.____.B(right);
			h.____.____.B(node);
			h.B(root);
			h.____.B(left);
		}

		[Test]
		public void InsertFourthGrandChild()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ root = new IntervalNode_(new Interval<int>(512, 512)) { Name = "X" };
			IntervalNode_ left = new IntervalNode_(new Interval<int>(256, 256)) { Name = "L" };
			IntervalNode_ right = new IntervalNode_(new Interval<int>(768, 768)) { Name = "R" };
			tree.Insert(root);
			tree.Insert(left);
			tree.Insert(right);

			IntervalNode_ node = new IntervalNode_(new Interval<int>(770, 770)) { Name = "N" };
			tree.Insert(node);

			TreeHelper_ h = new TreeHelper_(TreeHelper_.Assert, tree);
			h.____.____.B(node);
			h.____.B(right);
			h.B(root);
			h.____.B(left);
		}

		[Test]
		public void InsertUpdatesMaxAllTheWayUp()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ root = new IntervalNode_(new Interval<int>(512, 512)) { Name = "X" };
			IntervalNode_ left = new IntervalNode_(new Interval<int>(256, 256)) { Name = "L" };
			IntervalNode_ right = new IntervalNode_(new Interval<int>(768, 768)) { Name = "R" };
			tree.Insert(root);
			tree.Insert(left);
			tree.Insert(right);

			IntervalNode_ node = new IntervalNode_(new Interval<int>(250, 1000)) { Name = "N" };
			tree.Insert(node);

			Assert.AreEqual(1000, node.Max);
			Assert.AreEqual(1000, left.Max);
			Assert.AreEqual(1000, root.Max);
			Assert.AreEqual(768, right.Max);
		}

		[Test]
		public void InsertUpdatesMaxAsFarAsItNeeds()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ root = new IntervalNode_(new Interval<int>(512, 512)) { Name = "X" };
			IntervalNode_ left = new IntervalNode_(new Interval<int>(256, 256)) { Name = "L" };
			IntervalNode_ right = new IntervalNode_(new Interval<int>(768, 1500)) { Name = "R" };
			tree.Insert(root);
			tree.Insert(left);
			tree.Insert(right);

			IntervalNode_ node = new IntervalNode_(new Interval<int>(250, 1000)) { Name = "N" };
			tree.Insert(node);

			Assert.AreEqual(1000, node.Max);
			Assert.AreEqual(1000, left.Max);
			Assert.AreEqual(1500, root.Max);
			Assert.AreEqual(1500, right.Max);
		}

		[Test]
		public void InsertFixesupSimpleLeft()
		{
			TreeHelper_ h;
			IntervalTree_ tree = new IntervalTree_();
			// in tree
			IntervalNode_ root = new IntervalNode_ { Name = "R" };
			IntervalNode_ nodeA = new IntervalNode_ { Name = "A" };
			IntervalNode_ nodeB = new IntervalNode_ { Name = "B" };
			IntervalNode_ nodeC = new IntervalNode_ { Name = "C" };
			// newly inserted
			IntervalNode_ node = new IntervalNode_ { Name = "N" };

			h = new TreeHelper_(TreeHelper_.Build, tree);
			h.B(root);
			h.____.____.R(nodeC);
			h.____.B(nodeB);
			h.____.____.R(nodeA);
			h.____.____.____.R(node);

			tree.InsertFixup(node);

			h = new TreeHelper_(TreeHelper_.Assert, tree);
			h.B(root);
			h.____.____.B(nodeC);
			h.____.R(nodeB);
			h.____.____.B(nodeA);
			h.____.____.____.R(node);
		}

		[Test]
		public void InsertFixesupSimpleRight()
		{
			// ReSharper disable once JoinDeclarationAndInitializer
			TreeHelper_ h;
			IntervalTree_ tree = new IntervalTree_();
			// in tree
			IntervalNode_ root = new IntervalNode_ { Name = "R" };
			IntervalNode_ nodeA = new IntervalNode_ { Name = "A" };
			IntervalNode_ nodeB = new IntervalNode_ { Name = "B" };
			IntervalNode_ nodeC = new IntervalNode_ { Name = "C" };
			// newly inserted
			IntervalNode_ node = new IntervalNode_ { Name = "N" };

			h = new TreeHelper_(TreeHelper_.Build, tree);
			h.____.____.____.R(node);
			h.____.____.R(nodeC);
			h.____.B(nodeB);
			h.____.____.R(nodeA);
			h.B(root);

			tree.InsertFixup(node);

			h = new TreeHelper_(TreeHelper_.Assert, tree);
			h.____.____.____.R(node);
			h.____.____.B(nodeC);
			h.____.R(nodeB);
			h.____.____.B(nodeA);
			h.B(root);
		}

		[Test]
		public void InsertFixupDoesRotationLeft()
		{
			// ReSharper disable once JoinDeclarationAndInitializer
			TreeHelper_ h;
			IntervalTree_ tree = new IntervalTree_();
			// in tree
			IntervalNode_ root = new IntervalNode_ { Name = "R" };
			IntervalNode_ nodeA = new IntervalNode_ { Name = "A" };
			IntervalNode_ nodeB = new IntervalNode_ { Name = "B" };
			// newly inserted
			IntervalNode_ node = new IntervalNode_ { Name = "N" };

			h = new TreeHelper_(TreeHelper_.Build, tree);
			h.____.B(nodeB);
			h.B(root);
			h.____.____.R(node);
			h.____.R(nodeA);

			tree.InsertFixup(node);

			h = new TreeHelper_(TreeHelper_.Assert, tree);
			h.____.____.B(nodeB);
			h.____.R(root);
			h.B(node);
			h.____.R(nodeA);
		}

		[Test]
		public void InsertFixupDoesRotationRight()
		{
			// ReSharper disable once JoinDeclarationAndInitializer
			TreeHelper_ h;
			IntervalTree_ tree = new IntervalTree_();
			// in tree
			IntervalNode_ root = new IntervalNode_ { Name = "R" };
			IntervalNode_ nodeA = new IntervalNode_ { Name = "A" };
			IntervalNode_ nodeB = new IntervalNode_ { Name = "B" };
			// newly inserted
			IntervalNode_ node = new IntervalNode_ { Name = "N" };

			h = new TreeHelper_(TreeHelper_.Build, tree);
			h.____.R(nodeB);
			h.____.____.R(node);
			h.B(root);
			h.____.B(nodeA);

			tree.InsertFixup(node);

			h = new TreeHelper_(TreeHelper_.Assert, tree);
			h.____.R(nodeB);
			h.B(node);
			h.____.R(root);
			h.____.____.B(nodeA);
		}

		[Test]
		public void InsertFixupBigExample()
		{
			// ReSharper disable once JoinDeclarationAndInitializer
			TreeHelper_ h;
			IntervalTree_ tree = new IntervalTree_();
			// in tree
			IntervalNode_[] nodes = Enumerable.Range(0, 8).Select(i => new IntervalNode_ { Name = i.ToString() }).ToArray();
			// newly inserted
			IntervalNode_ node = new IntervalNode_ { Name = "N" };

			h = new TreeHelper_(TreeHelper_.Build, tree);
			h.____.____.R(nodes[7]);
			h.____.B(nodes[6]);
			h.B(nodes[5]);
			h.____.____.____.R(nodes[4]);
			h.____.____.B(nodes[3]);
			h.____.____.____.R(nodes[2]);
			h.____.____.____.____.R(node);
			h.____.R(nodes[1]);
			h.____.____.B(nodes[0]);

			tree.InsertFixup(node);

			h = new TreeHelper_(TreeHelper_.Assert, tree);
			h.____.____.____.R(nodes[7]);
			h.____.____.B(nodes[6]);
			h.____.R(nodes[5]);
			h.____.____.B(nodes[4]);
			h.B(nodes[3]);
			h.____.____.B(nodes[2]);
			h.____.____.____.R(node);
			h.____.R(nodes[1]);
			h.____.____.B(nodes[0]);
		}
	}
}
