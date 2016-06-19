using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using NUnit.Framework;
using IntervalNode_ = IntervalTreeNS.IntervalNode<IntervalTreeNS.Interval<int>, int>;
using IntervalTree_ = IntervalTreeNS.IntervalTree<IntervalTreeNS.Interval<int>, int>;
using TreeHelper_ = IntervalTreeNS.TestHelpers.TreeHelper<IntervalTreeNS.Interval<int>, int>;

namespace IntervalTreeNS.Enumeration
{
	[TestFixture]
	public class IntervalTreeEnumerationTests
	{
		[Test]
		public void CompleteTreeEnumeration()
		{
			IntervalTree_ tree = new IntervalTree_(Comparer<int>.Default);
			IntervalNode_[] nodes = Enumerable.Range(0, 15)
				.Select(i => new IntervalNode_(tree, new Interval<int>(i, i)) { Name = i.ToString() }).ToArray();

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
			IntervalTree_ tree = new IntervalTree_(Comparer<int>.Default);
			IntervalNode_[] nodes = Enumerable.Range(0, 15)
				.Select(i => new IntervalNode_(tree, new Interval<int>(i, i)) { Name = i.ToString() }).ToArray();

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
			IntervalTree_ tree = new IntervalTree_(Comparer<int>.Default);
			IntervalNode_[] nodes = Enumerable.Range(0, 15)
				.Select(i => new IntervalNode_(tree, new Interval<int>(i, i)) { Name = i.ToString() }).ToArray();

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
			IntervalTree_ tree = new IntervalTree_(Comparer<int>.Default);

			TreeHelper_ h = new TreeHelper_(TreeHelper_.Build, tree);
			h.____.____.B(new IntervalNode_(tree, new Interval<int>(7, 7)));
			h.____.B(new IntervalNode_(tree, new Interval<int>(6, 6)));
			h.____.____.B(new IntervalNode_(tree, new Interval<int>(5, 5)));
			h.B(new IntervalNode_(tree, new Interval<int>(4, 4)));
			h.____.____.B(new IntervalNode_(tree, new Interval<int>(3, 3)));
			h.____.B(new IntervalNode_(tree, new Interval<int>(2, 2)));
			h.____.____.B(new IntervalNode_(tree, new Interval<int>(1, 1)));

			int[] expectedIntervalStarts = { 1, 2, 3, 4, 5, 6, 7 };

			Interval<int>[] intervals = tree.FindAllIntersecting(new Interval<int>(0, 8)).ToArray();

			Assert.AreEqual(7, intervals.Length);
			Assert.That(intervals.Select(i => i.Start), Is.EquivalentTo(expectedIntervalStarts));
		}

		[Test]
		public void FindAllIntersectingNotAllNodes()
		{
			IntervalTree_ tree = new IntervalTree_(Comparer<int>.Default);

			TreeHelper_ h = new TreeHelper_(TreeHelper_.Build, tree);
			h.____.____.B(new IntervalNode_(tree, new Interval<int>(7, 7)));
			h.____.B(new IntervalNode_(tree, new Interval<int>(6, 6)));
			h.____.____.B(new IntervalNode_(tree, new Interval<int>(5, 5)));
			h.B(new IntervalNode_(tree, new Interval<int>(4, 4)));
			h.____.____.B(new IntervalNode_(tree, new Interval<int>(3, 3)));
			h.____.B(new IntervalNode_(tree, new Interval<int>(2, 2)));
			h.____.____.B(new IntervalNode_(tree, new Interval<int>(1, 1)));

			int[] expectedIntervalStarts = { 3, 4, 5 };

			Interval<int>[] intervals = tree.FindAllIntersecting(new Interval<int>(2, 6)).ToArray();

			Assert.AreEqual(3, intervals.Length);
			Assert.That(intervals.Select(i => i.Start), Is.EquivalentTo(expectedIntervalStarts));
		}

		[Test]
		public void FindAllIntersectingWithAdjacent()
		{
			IntervalTree_ tree = new IntervalTree_(Comparer<int>.Default);

			TreeHelper_ h = new TreeHelper_(TreeHelper_.Build, tree);
			h.____.____.B(new IntervalNode_(tree, new Interval<int>(7, 7)));
			h.____.B(new IntervalNode_(tree, new Interval<int>(6, 6)));
			h.____.____.B(new IntervalNode_(tree, new Interval<int>(5, 5)));
			h.B(new IntervalNode_(tree, new Interval<int>(4, 4)));
			h.____.____.B(new IntervalNode_(tree, new Interval<int>(3, 3)));
			h.____.B(new IntervalNode_(tree, new Interval<int>(2, 2)));
			h.____.____.B(new IntervalNode_(tree, new Interval<int>(1, 1)));

			int[] expectedIntervalStarts = { 2, 3, 4, 5, 6 };

			Interval<int>[] intervals = tree.FindAllIntersecting(new Interval<int>(2, 6), true).ToArray();

			Assert.AreEqual(5, intervals.Length);
			Assert.That(intervals.Select(i => i.Start), Is.EquivalentTo(expectedIntervalStarts));
		}
	}
}
