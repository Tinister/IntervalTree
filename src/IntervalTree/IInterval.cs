using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace IntervalTreeNS
{
	/// <summary>Represents an interval with a specified endpoint type.</summary>
	/// <typeparam name="TEndpoint">Type of the interval endpoints.</typeparam>
	public interface IInterval<out TEndpoint>
		where TEndpoint : IComparable<TEndpoint>
	{
		/// <summary>Gets the inclusive starting point of the interval.</summary>
		/// <value>The inclusive starting point of the interval.</value>
		TEndpoint Start { get; }

		/// <summary>Gets the inclusive ending point of the interval.</summary>
		/// <value>The inclusive ending point of the interval.</value>
#if SUPPRESS_MESSAGE
		[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "End",
			Justification = "Not sure which .NET languages reserves 'End'.")]
#endif
		TEndpoint End { get; }
	}

	/// <summary>Provides extension methods for common interval functions.</summary>
	public static class IntervalExtensions
	{
		/// <summary>Determines if the two intervals intersect.</summary>
		/// <typeparam name="TEndpoint">Type of the interval endpoints.</typeparam>
		/// <param name="self">First interval to compare.</param>
		/// <param name="other">Second interval to compare.</param>
		/// <returns><c>true</c> if they intersect, <c>false</c> otherwise.</returns>
		public static bool Intersects<TEndpoint>(this IInterval<TEndpoint> self, IInterval<TEndpoint> other)
			where TEndpoint : IComparable<TEndpoint>
		{
			return new Interval<TEndpoint>(self).Intersects(other);
		}

		/// <summary>Determines if two intervals are adjacent.</summary>
		/// <typeparam name="TEndpoint">Type of the interval endpoints.</typeparam>
		/// <param name="self">First interval to compare.</param>
		/// <param name="other">Second interval to compare.</param>
		/// <returns><c>true</c> if they are adjacent, <c>false</c> otherwise.</returns>
		public static bool IsAdjacentTo<TEndpoint>(this IInterval<TEndpoint> self, IInterval<TEndpoint> other)
			where TEndpoint : IComparable<TEndpoint>
		{
			return new Interval<TEndpoint>(self).IsAdjacentTo(other);
		}

		/// <summary>Determines if two intervals intersect or are adjacent.</summary>
		/// <typeparam name="TEndpoint">Type of the interval endpoints.</typeparam>
		/// <param name="self">First interval to compare.</param>
		/// <param name="other">Second interval to compare.</param>
		/// <returns><c>true</c> if they intersect or are adjacent, <c>false</c> otherwise.</returns>
		public static bool IntersectsOrIsAdjacentTo<TEndpoint>(this IInterval<TEndpoint> self, IInterval<TEndpoint> other)
			where TEndpoint : IComparable<TEndpoint>
		{
			return new Interval<TEndpoint>(self).IntersectsOrIsAdjacentTo(other);
		}

		/// <summary>Determines if the two intervals are equivalent.</summary>
		/// <typeparam name="TEndpoint">Type of the interval endpoints.</typeparam>
		/// <param name="self">First interval to compare.</param>
		/// <param name="other">Second interval to compare.</param>
		/// <returns><c>true</c> if they are equivalent, <c>false</c> otherwise.</returns>
		public static bool IsEquivalentTo<TEndpoint>(this IInterval<TEndpoint> self, IInterval<TEndpoint> other)
			where TEndpoint : IComparable<TEndpoint>
		{
			return new Interval<TEndpoint>(self).IsEquivalentTo(other);
		}

		/// <summary>Determines if the one interval wholly contains another interval.</summary>
		/// <typeparam name="TEndpoint">Type of the interval endpoints.</typeparam>
		/// <param name="self">First interval to compare.</param>
		/// <param name="other">Second interval to compare.</param>
		/// <returns><c>true</c> if <paramref name="other"/> is wholly contained within <paramref name="self"/>, <c>false</c>
		/// otherwise.</returns>
		public static bool Contains<TEndpoint>(this IInterval<TEndpoint> self, IInterval<TEndpoint> other)
			where TEndpoint : IComparable<TEndpoint>
		{
			return new Interval<TEndpoint>(self).Contains(other);
		}

		/// <summary>Determines if the specified interval contains the specified value.</summary>
		/// <typeparam name="TEndpoint">Type of the interval endpoints.</typeparam>
		/// <param name="self">Interval to compare.</param>
		/// <param name="value">Value to compare.</param>
		/// <returns><c>true</c> if <paramref name="value"/> is contained within <paramref name="self"/>, <c>false</c> otherwise.</returns>
		public static bool Contains<TEndpoint>(this IInterval<TEndpoint> self, TEndpoint value)
			where TEndpoint : IComparable<TEndpoint>
		{
			return new Interval<TEndpoint>(self).Contains(value);
		}
	}
}
