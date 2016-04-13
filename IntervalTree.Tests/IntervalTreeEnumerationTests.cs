using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using IntervalNode_ = IntervalTreeNS.IntervalNode<IntervalTreeNS.Interval<int>, int>;
using IntervalTree_ = IntervalTreeNS.IntervalTree<IntervalTreeNS.Interval<int>, int>;
using TreeHelper_ = IntervalTreeNS.TestHelpers.TreeHelper<IntervalTreeNS.Interval<int>, int>;

namespace IntervalTreeNS
{
	[TestFixture]
	public class IntervalTreeEnumerationTests
	{
		[Test]
		public void CompleteTreeEnumeration()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_[] nodes = Enumerable.Range(0, 15)
				.Select(i => new IntervalNode_(new Interval<int>(i, i)) { Name = i.ToString() }).ToArray();

			TreeHelper_ h = new TreeHelper_(TreeHelper_.Build, tree);
			h.____.____.____.B(nodes[14]);
			h.____.____.B(nodes[13]);
			h.____.____.____.B(nodes[12]);
			h.____.B(nodes[11]);
			h.____.____.____.B(nodes[10]);
			h.____.____.B(nodes[9]);
			h.____.____.____.B(nodes[8]);
			h.B(nodes[7]);
			h.____.____.____.B(nodes[6]);
			h.____.____.B(nodes[5]);
			h.____.____.____.B(nodes[4]);
			h.____.B(nodes[3]);
			h.____.____.____.B(nodes[2]);
			h.____.____.B(nodes[1]);
			h.____.____.____.B(nodes[0]);

			Interval<int>[] intervals = tree.ToArray();

			Assert.Greater(intervals.Length, 0);
			for (int i = 0; i < intervals.Length; i++)
				Assert.AreEqual(i, intervals[i].Start, $"Intervals at index {i} differ.");
		}

		[Test]
		public void LeftImbalancedEnumeration()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_[] nodes = Enumerable.Range(0, 15)
				.Select(i => new IntervalNode_(new Interval<int>(i, i)) { Name = i.ToString() }).ToArray();

			TreeHelper_ h = new TreeHelper_(TreeHelper_.Build, tree);
			h.____.____.B(nodes[14]);
			h.____.____.____.B(nodes[13]);
			h.____.____.____.____.B(nodes[12]);
			h.____.B(nodes[11]);
			h.____.____.B(nodes[10]);
			h.____.____.____.B(nodes[9]);
			h.____.____.____.____.B(nodes[8]);
			h.B(nodes[7]);
			h.____.____.B(nodes[6]);
			h.____.____.____.B(nodes[5]);
			h.____.____.____.____.B(nodes[4]);
			h.____.B(nodes[3]);
			h.____.____.B(nodes[2]);
			h.____.____.____.B(nodes[1]);
			h.____.____.____.____.B(nodes[0]);

			Interval<int>[] intervals = tree.ToArray();

			Assert.Greater(intervals.Length, 0);
			for (int i = 0; i < intervals.Length; i++)
				Assert.AreEqual(i, intervals[i].Start, $"Intervals at index {i} differ.");
		}

		[Test]
		public void RightImbalancedEnumeration()
		{
			IntervalTree_ tree = new IntervalTree_();
			IntervalNode_[] nodes = Enumerable.Range(0, 15)
				.Select(i => new IntervalNode_(new Interval<int>(i, i)) { Name = i.ToString() }).ToArray();

			TreeHelper_ h = new TreeHelper_(TreeHelper_.Build, tree);
			h.____.____.____.____.B(nodes[14]);
			h.____.____.____.B(nodes[13]);
			h.____.____.B(nodes[12]);
			h.____.B(nodes[11]);
			h.____.____.____.____.B(nodes[10]);
			h.____.____.____.B(nodes[9]);
			h.____.____.B(nodes[8]);
			h.B(nodes[7]);
			h.____.____.____.____.B(nodes[6]);
			h.____.____.____.B(nodes[5]);
			h.____.____.B(nodes[4]);
			h.____.B(nodes[3]);
			h.____.____.____.____.B(nodes[2]);
			h.____.____.____.B(nodes[1]);
			h.____.____.B(nodes[0]);

			Interval<int>[] intervals = tree.ToArray();

			Assert.Greater(intervals.Length, 0);
			for (int i = 0; i < intervals.Length; i++)
				Assert.AreEqual(i, intervals[i].Start, $"Intervals at index {i} differ.");
		}

		[Test]
		public void FindAllIntersectingGetsAllNodes()
		{
			IntervalTree_ tree = new IntervalTree_();

			TreeHelper_ h = new TreeHelper_(TreeHelper_.Build, tree);
			h.____.____.B(new IntervalNode_(new Interval<int>(7, 7)));
			h.____.B(new IntervalNode_(new Interval<int>(6, 6)));
			h.____.____.B(new IntervalNode_(new Interval<int>(5, 5)));
			h.B(new IntervalNode_(new Interval<int>(4, 4)));
			h.____.____.B(new IntervalNode_(new Interval<int>(3, 3)));
			h.____.B(new IntervalNode_(new Interval<int>(2, 2)));
			h.____.____.B(new IntervalNode_(new Interval<int>(1, 1)));

			Interval<int>[] intervals = tree.FindAllIntersecting(new Interval<int>(0, 8)).ToArray();

			Assert.AreEqual(7, intervals.Length);
			for (int i = 1; i <= 7; i++)
				Assert.AreEqual(i, intervals[i - 1].Start, $"Intervals at index {i - 1} differ.");
		}

		[Test]
		public void FindAllIntersectingNotAllNodes()
		{
			IntervalTree_ tree = new IntervalTree_();

			TreeHelper_ h = new TreeHelper_(TreeHelper_.Build, tree);
			h.____.____.B(new IntervalNode_(new Interval<int>(7, 7)));
			h.____.B(new IntervalNode_(new Interval<int>(6, 6)));
			h.____.____.B(new IntervalNode_(new Interval<int>(5, 5)));
			h.B(new IntervalNode_(new Interval<int>(4, 4)));
			h.____.____.B(new IntervalNode_(new Interval<int>(3, 3)));
			h.____.B(new IntervalNode_(new Interval<int>(2, 2)));
			h.____.____.B(new IntervalNode_(new Interval<int>(1, 1)));

			Interval<int>[] intervals = tree.FindAllIntersecting(new Interval<int>(2, 6)).ToArray();

			Assert.AreEqual(3, intervals.Length);
			for (int i = 3; i <= 5; i++)
				Assert.AreEqual(i, intervals[i - 3].Start, $"Intervals at index {i - 3} differ.");
		}

		[Test]
		public void FindAllIntersectingWithAdjacent()
		{
			IntervalTree_ tree = new IntervalTree_();

			TreeHelper_ h = new TreeHelper_(TreeHelper_.Build, tree);
			h.____.____.B(new IntervalNode_(new Interval<int>(7, 7)));
			h.____.B(new IntervalNode_(new Interval<int>(6, 6)));
			h.____.____.B(new IntervalNode_(new Interval<int>(5, 5)));
			h.B(new IntervalNode_(new Interval<int>(4, 4)));
			h.____.____.B(new IntervalNode_(new Interval<int>(3, 3)));
			h.____.B(new IntervalNode_(new Interval<int>(2, 2)));
			h.____.____.B(new IntervalNode_(new Interval<int>(1, 1)));

			Interval<int>[] intervals = tree.FindAllIntersecting(new Interval<int>(2, 6), true).ToArray();

			Assert.AreEqual(5, intervals.Length);
			for (int i = 2; i <= 6; i++)
				Assert.AreEqual(i, intervals[i - 2].Start, $"Intervals at index {i - 2} differ.");
		}
	}
}
