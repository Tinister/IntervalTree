using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS
{
	/// <summary>A simple implementation of the <see cref="IInterval{TEndpoint}"/> interface.</summary>
	/// <typeparam name="TEndpoint">Type of the interval endpoints.</typeparam>
	public struct Interval<TEndpoint> : IInterval<TEndpoint>
		where TEndpoint : IComparable<TEndpoint>
	{
		/// <summary>Initializes a new instance of the <see cref="Interval{TEndpoint}"/> struct with the specified endpoints.</summary>
		/// <param name="start">The inclusive starting point of the interval.</param>
		/// <param name="end">The inclusive ending point of the interval.</param>
		public Interval(TEndpoint start, TEndpoint end)
		{
			if (Comparer<TEndpoint>.Default.Compare(start, end) > 0)
				throw new ArgumentException("Interval end must occur after the interval start.");

			Start = start;
			End = end;
		}

		/// <summary>Initializes a new instance of the <see cref="Interval{TEndpoint}"/> struct as a copy of the specified
		/// interval.</summary>
		/// <param name="interval">Interval to copy.</param>
		public Interval(IInterval<TEndpoint> interval)
		{
			if (interval == null)
				throw new ArgumentNullException(nameof(interval));
			if (Comparer<TEndpoint>.Default.Compare(interval.Start, interval.End) > 0)
				throw new ArgumentException("Interval end must occur after the interval start.");

			Start = interval.Start;
			End = interval.End;
		}

		/// <summary>Gets the inclusive starting point of the interval.</summary>
		/// <value>The inclusive starting point of the interval.</value>
		public TEndpoint Start { get; }

		/// <summary>Gets the inclusive ending point of the interval.</summary>
		/// <value>The inclusive ending point of the interval.</value>
		public TEndpoint End { get; }

		/// <summary>Determines if the specified interval intersects this interval.</summary>
		/// <param name="other">Interval to compare with this one.</param>
		/// <returns><c>true</c> if they intersect, <c>false</c> otherwise.</returns>
		public bool Intersects(IInterval<TEndpoint> other)
		{
			if (other == null)
				throw new ArgumentNullException(nameof(other));
			if (Comparer<TEndpoint>.Default.Compare(other.Start, other.End) > 0)
				throw new ArgumentException("Interval end must occur after the interval start.");

			return Comparer<TEndpoint>.Default.Compare(other.Start, End) < 0
				&& Comparer<TEndpoint>.Default.Compare(Start, other.End) < 0;
		}

		/// <summary>Determines if the specified interval is adjacent this interval.</summary>
		/// <param name="other">Interval to compare with this one.</param>
		/// <returns><c>true</c> if they are adjacent, <c>false</c> otherwise.</returns>
		public bool IsAdjacentTo(IInterval<TEndpoint> other)
		{
			if (other == null)
				throw new ArgumentNullException(nameof(other));
			if (Comparer<TEndpoint>.Default.Compare(other.Start, other.End) > 0)
				throw new ArgumentException("Interval end must occur after the interval start.");

			return Comparer<TEndpoint>.Default.Compare(End, other.Start) == 0
				|| Comparer<TEndpoint>.Default.Compare(other.End, Start) == 0;
		}

		/// <summary>Determines if the specified interval intersects or is adjacent to this interval.</summary>
		/// <param name="other">Interval to compare with this one.</param>
		/// <returns><c>true</c> if they intersect or are adjacent, <c>false</c> otherwise.</returns>
		public bool IntersectsOrIsAdjacentTo(IInterval<TEndpoint> other)
		{
			if (other == null)
				throw new ArgumentNullException(nameof(other));
			if (Comparer<TEndpoint>.Default.Compare(other.Start, other.End) > 0)
				throw new ArgumentException("Interval end must occur after the interval start.");

			return Comparer<TEndpoint>.Default.Compare(other.Start, End) <= 0
				&& Comparer<TEndpoint>.Default.Compare(Start, other.End) <= 0;
		}

		/// <summary>Determines if the specified interval is equivalent to this interval.</summary>
		/// <param name="other">Interval to compare with this one.</param>
		/// <returns><c>true</c> if they are equivalent, <c>false</c> otherwise.</returns>
		public bool IsEquivalentTo(IInterval<TEndpoint> other)
		{
			if (other == null)
				throw new ArgumentNullException(nameof(other));
			if (Comparer<TEndpoint>.Default.Compare(other.Start, other.End) > 0)
				throw new ArgumentException("Interval end must occur after the interval start.");

			return Comparer<TEndpoint>.Default.Compare(Start, other.Start) == 0
				|| Comparer<TEndpoint>.Default.Compare(End, other.End) == 0;
		}

		/// <summary>Determines if this interval wholly contains the specified interval.</summary>
		/// <param name="other">Interval to compare with this one.</param>
		/// <returns><c>true</c> if <paramref name="other"/> is wholly contained within this one, <c>false</c> otherwise.</returns>
		public bool Contains(IInterval<TEndpoint> other)
		{
			if (other == null)
				throw new ArgumentNullException(nameof(other));
			if (Comparer<TEndpoint>.Default.Compare(other.Start, other.End) > 0)
				throw new ArgumentException("Interval end must occur after the interval start.");

			return Comparer<TEndpoint>.Default.Compare(Start, other.Start) <= 0
				&& Comparer<TEndpoint>.Default.Compare(End, other.End) >= 0;
		}

		/// <summary>Determines if this interval contains the specified value.</summary>
		/// <param name="value">Value to compare.</param>
		/// <returns><c>true</c> if this interval contains the <paramref name="value"/>, <c>false</c> otherwise.</returns>
		public bool Contains(TEndpoint value)
		{
			return Comparer<TEndpoint>.Default.Compare(Start, value) <= 0
				&& Comparer<TEndpoint>.Default.Compare(End, value) >= 0;
		}
	}
}
