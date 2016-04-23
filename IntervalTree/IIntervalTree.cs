using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IntervalTreeNS
{
	/// <summary>Represents a strongly-typed collection of objects that can be represented by intervals.</summary>
	/// <typeparam name="TElement">The type of elements in the tree.</typeparam>
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
		Justification = "'Tree' is already a sufficient suffix.")]
	public interface IIntervalTree<TElement> : IEnumerable<TElement>
	{
		/// <summary>Gets the root node of the tree.</summary>
		IIntervalNode<TElement> Root { get; }

		/// <summary>Adds the specified item to the interval tree.</summary>
		/// <param name="item">Item to add.</param>
		/// <returns>The node added to the tree that contains this item.</returns>
		IIntervalNode<TElement> Add(TElement item);

		/// <summary>Adds the specified item to the interval tree and also gets all intersecting elements at the time of addition.</summary>
		/// <param name="item">Item to add.</param>
		/// <param name="intersectingCollection">Collection to add intersecting elements to.  Will call
		/// <see cref="ICollection{TElement}.Add"/> for each element.</param>
		/// <param name="alsoAdjacent">Set to true to also get all adjacent elements.</param>
		/// <returns>The node added to the tree that contains this item.</returns>
		IIntervalNode<TElement> Add(TElement item, ICollection<TElement> intersectingCollection, bool alsoAdjacent = false);

		/// <summary>Returns an enumerator that iterates through all elements that intersect the specified interval.</summary>
		/// <param name="item">Interval to use to find all intersecting elements.</param>
		/// <param name="alsoAdjacent">Set to true to also get all adjacent elements.</param>
		/// <returns>An enumerator for all intersecting elements.</returns>
		IEnumerable<TElement> FindAllIntersecting(TElement item, bool alsoAdjacent = false);
	}
}