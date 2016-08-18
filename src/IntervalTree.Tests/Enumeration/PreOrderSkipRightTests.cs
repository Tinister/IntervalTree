using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using IntervalNode_ = IntervalTreeNS.IntervalNode<IntervalTreeNS.Interval<int>, int>;
using IntervalTree_ = IntervalTreeNS.IntervalTree<IntervalTreeNS.Interval<int>, int>;
using ITraversalModifier_ = IntervalTreeNS.Enumeration.ITraversalModifier<IntervalTreeNS.Interval<int>, int>;
using PreOrderEnumerator_ = IntervalTreeNS.Enumeration.PreOrderEnumerator<IntervalTreeNS.Interval<int>, int>;
using TreeHelper_ = IntervalTreeNS.TestHelpers.TreeHelper<IntervalTreeNS.Interval<int>, int>;

namespace IntervalTreeNS.Enumeration
{
	[TestFixture]
	public class PreOrderSkipRightTests
	{
		[Test]
		public void MinimumVisitWhenNoElementsYielded()
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

			List<IntervalNode_> nodesVisited = new List<IntervalNode_>();
			List<IntervalNode_> nodesExpectToVisit = new List<IntervalNode_> { nodes[7], nodes[3], nodes[1], nodes[0] };

			Mock<ITraversalModifier_> mockModifier = new Mock<ITraversalModifier_>();
			mockModifier.Setup(x => x.CanGoLeft(It.IsAny<IntervalNode_>())).Returns(true);
			mockModifier.Setup(x => x.CanGoRight(It.IsAny<IntervalNode_>())).Returns(true);
			mockModifier.Setup(x => x.CanYield(It.IsAny<IntervalNode_>())).Returns(false)
				.Callback((IntervalNode_ node) => { nodesVisited.Add(node); });

			PreOrderEnumerator_ enumerator = new PreOrderEnumerator_(tree, mockModifier.Object);
			CollectionAssert.IsEmpty(enumerator.ToList());

			CollectionAssert.AreEqual(nodesExpectToVisit, nodesVisited);
		}

		[Test]
		public void RightSubChildrenVisitedWhenAppropriate()
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

			List<IntervalNode_> nodesVisited = new List<IntervalNode_>();
			List<IntervalNode_> nodesExpectToVisit = new List<IntervalNode_>
			{
				nodes[7],
				nodes[3],
				nodes[1],
				nodes[0],
				nodes[5],
				nodes[4],
				nodes[11],
				nodes[9],
				nodes[8]
			};
			List<Interval<int>> expected = new List<Interval<int>> { nodes[3].Element };

			Mock<ITraversalModifier_> mockModifier = new Mock<ITraversalModifier_>();
			mockModifier.Setup(x => x.CanGoLeft(It.IsAny<IntervalNode_>())).Returns(true);
			mockModifier.Setup(x => x.CanGoRight(It.IsAny<IntervalNode_>())).Returns(true);
			mockModifier.Setup(x => x.CanYield(It.IsAny<IntervalNode_>()))
				.Returns((IntervalNode_ node) => node.Name == "3")
				.Callback((IntervalNode_ node) => { nodesVisited.Add(node); });

			PreOrderEnumerator_ enumerator = new PreOrderEnumerator_(tree, mockModifier.Object);
			CollectionAssert.AreEqual(expected, enumerator.ToList());

			Console.WriteLine(string.Join(", ", nodesVisited.Select(x => x.Name)));

			CollectionAssert.AreEqual(nodesExpectToVisit, nodesVisited);
		}

		[Test]
		public void DescendingDownOnlyChildRightTreeDoesntImbalanceStack()
		{
			IntervalTree_ tree = new IntervalTree_(Comparer<int>.Default);
			IntervalNode_[] nodes = Enumerable.Range(0, 15)
				.Select(i => new IntervalNode_(tree, new Interval<int>(i, i)) { Name = i.ToString() }).ToArray();

			TreeHelper_ h = new TreeHelper_(TreeHelper_.Build, tree);
			h.____.____.____.B(nodes[14]);
			h.____.____.B(nodes[13]);
			h.____.____.____.B(nodes[12]);
			h.____.B(nodes[11]);
			h.B(nodes[7]);
			h.____.____.____.____.B(nodes[6]);
			h.____.____.____.B(nodes[5]);
			h.____.____.____.____.B(nodes[4]);
			h.____.____.B(nodes[3]);
			h.____.B(nodes[2]);

			List<IntervalNode_> nodesVisited = new List<IntervalNode_>();
			List<IntervalNode_> nodesExpectToVisit = new List<IntervalNode_>
			{
				nodes[7],
				nodes[2],
				nodes[3],
				nodes[5],
				nodes[4],
				nodes[11],
				nodes[13],
				nodes[12]
			};
			List<Interval<int>> expected = new List<Interval<int>> { nodes[3].Element };

			Mock<ITraversalModifier_> mockModifier = new Mock<ITraversalModifier_>();
			mockModifier.Setup(x => x.CanGoLeft(It.IsAny<IntervalNode_>())).Returns(true);
			mockModifier.Setup(x => x.CanGoRight(It.IsAny<IntervalNode_>())).Returns(true);
			mockModifier.Setup(x => x.CanYield(It.IsAny<IntervalNode_>()))
				.Returns((IntervalNode_ node) => node.Name == "3")
				.Callback((IntervalNode_ node) => { nodesVisited.Add(node); });

			PreOrderEnumerator_ enumerator = new PreOrderEnumerator_(tree, mockModifier.Object);
			CollectionAssert.AreEqual(expected, enumerator.ToList());

			Console.WriteLine(string.Join(", ", nodesVisited.Select(x => x.Name)));

			CollectionAssert.AreEqual(nodesExpectToVisit, nodesVisited);
		}
	}
}
