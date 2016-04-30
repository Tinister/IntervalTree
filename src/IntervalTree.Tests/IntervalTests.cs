using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace IntervalTreeNS
{
	[TestFixture]
	public class IntervalTests
	{
		[TestCase(1, 3, 2, 4, true)]
		[TestCase(1, 3, 1, 4, true)]
		[TestCase(1, 3, 0, 3, true)]
		[TestCase(1, 3, 3, 5, false)]
		[TestCase(1, 3, 4, 6, false)]
		public void IntervalIntersection(int s1, int e1, int s2, int e2, bool expected)
		{
			Interval<int> a = new Interval<int>(s1, e1);
			Interval<int> b = new Interval<int>(s2, e2);

			if (expected)
			{
				Assert.True(a.Intersects(b));
				Assert.True(b.Intersects(a));
			}
			else
			{
				Assert.False(a.Intersects(b));
				Assert.False(b.Intersects(a));
			}
		}

		[TestCase(1, 3, 2, 4, false)]
		[TestCase(1, 3, 1, 4, false)]
		[TestCase(1, 3, 0, 3, false)]
		[TestCase(1, 3, 3, 5, true)]
		[TestCase(1, 3, 4, 6, false)]
		public void IntervalAdjacency(int s1, int e1, int s2, int e2, bool expected)
		{
			Interval<int> a = new Interval<int>(s1, e1);
			Interval<int> b = new Interval<int>(s2, e2);

			if (expected)
			{
				Assert.True(a.IsAdjacentTo(b));
				Assert.True(b.IsAdjacentTo(a));
			}
			else
			{
				Assert.False(a.IsAdjacentTo(b));
				Assert.False(b.IsAdjacentTo(a));
			}
		}

		[TestCase(1, 3, 2, 4, true)]
		[TestCase(1, 3, 1, 4, true)]
		[TestCase(1, 3, 0, 3, true)]
		[TestCase(1, 3, 3, 5, true)]
		[TestCase(1, 3, 4, 6, false)]
		public void IntervalIntersectionOrAdjacency(int s1, int e1, int s2, int e2, bool expected)
		{
			Interval<int> a = new Interval<int>(s1, e1);
			Interval<int> b = new Interval<int>(s2, e2);

			if (expected)
			{
				Assert.True(a.IntersectsOrIsAdjacentTo(b));
				Assert.True(b.IntersectsOrIsAdjacentTo(a));
			}
			else
			{
				Assert.False(a.IntersectsOrIsAdjacentTo(b));
				Assert.False(b.IntersectsOrIsAdjacentTo(a));
			}
		}

		[TestCase(1, 3, 2, 4, false)]
		[TestCase(1, 3, 4, 6, false)]
		[TestCase(1, 3, 1, 3, true)]
		public void IntervalEquivalency(int s1, int e1, int s2, int e2, bool expected)
		{
			Interval<int> a = new Interval<int>(s1, e1);
			Interval<int> b = new Interval<int>(s2, e2);

			if (expected)
			{
				Assert.True(a.IsEquivalentTo(b));
				Assert.True(b.IsEquivalentTo(a));
			}
			else
			{
				Assert.False(a.IsEquivalentTo(b));
				Assert.False(b.IsEquivalentTo(a));
			}
		}

		[TestCase(1, 5, 2, 4, true)]
		[TestCase(1, 5, 2, 5, true)]
		[TestCase(1, 5, 1, 4, true)]
		[TestCase(1, 3, 1, 4, false)]
		[TestCase(1, 3, 0, 3, false)]
		public void IntervalContainment(int s1, int e1, int s2, int e2, bool expected)
		{
			Interval<int> a = new Interval<int>(s1, e1);
			Interval<int> b = new Interval<int>(s2, e2);

			if (expected)
			{
				Assert.True(a.Contains(b));
				Assert.False(b.Contains(a));
			}
			else
				Assert.False(a.Contains(b));
		}

		[TestCase(1, 3, 2, true)]
		[TestCase(1, 3, 1, true)]
		[TestCase(1, 3, 3, true)]
		[TestCase(1, 3, 0, false)]
		[TestCase(1, 3, 4, false)]
		public void IntervalContainment(int s1, int e1, int v, bool expected)
		{
			Interval<int> a = new Interval<int>(s1, e1);

			if (expected)
				Assert.True(a.Contains(v));
			else
				Assert.False(a.Contains(v));
		}

		[Test]
		public void CanInstantiateIntervalWithStartAndEndEqual()
		{
			Assert.DoesNotThrow(() => new Interval<int>(3, 3));
		}

		[Test]
		public void CannotInstantiateIntervalWithEndBeforeStart()
		{
			Assert.Throws<ArgumentException>(() => new Interval<int>(2, 1));
		}
	}
}
