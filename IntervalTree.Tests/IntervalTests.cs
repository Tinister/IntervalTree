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
	}
}
