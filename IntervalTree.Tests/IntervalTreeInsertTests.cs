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
	public class IntervalTreeInsertTests
	{
		[Test]
		public void RootIsSetIfThereIsntOne()
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

			TreeHelper_ a = new TreeHelper_(TreeHelper_.Assert, tree);
			a.N(root);
			a.____.N(node);
		}

		[Test]
		public void InsertOnTheRight()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_ root = new IntervalNode_(new Interval<int>(512, 512)) { Name = "X" };
			tree.Root = root;

			IntervalNode_ node = new IntervalNode_(new Interval<int>(515, 515)) { Name = "N" };
			tree.Insert(node);

			TreeHelper_ a = new TreeHelper_(TreeHelper_.Assert, tree);
			a.____.N(node);
			a.N(root);
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

			TreeHelper_ a = new TreeHelper_(TreeHelper_.Assert, tree);
			a.____.N(right);
			a.N(root);
			a.____.N(left);
			a.____.____.N(node);
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

			TreeHelper_ a = new TreeHelper_(TreeHelper_.Assert, tree);
			a.____.N(right);
			a.N(root);
			a.____.____.N(node);
			a.____.N(left);
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

			TreeHelper_ a = new TreeHelper_(TreeHelper_.Assert, tree);
			a.____.N(right);
			a.____.____.N(node);
			a.N(root);
			a.____.N(left);
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

			TreeHelper_ a = new TreeHelper_(TreeHelper_.Assert, tree);
			a.____.____.N(node);
			a.____.N(right);
			a.N(root);
			a.____.N(left);
		}
	}
}
