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
		[Test]
		public void CannotInstantiateIntervalWithEndBeforeStart()
		{
			Assert.Throws<ArgumentException>(() => new Interval<int>(2, 1));
		}

		[Test]
		public void CanInstantiateIntervalWithStartAndEndEqual()
		{
			Assert.DoesNotThrow(() => new Interval<int>(3, 3));
		}

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
	}
}
