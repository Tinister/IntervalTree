using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntervalTreeNS
{
	/// <summary>A simple implementation of the <see cref="IInterval{TEndpoint}"/> interface.</summary>
	/// <typeparam name="TEndpoint">Type of the interval endpoints.</typeparam>
	public struct Interval<TEndpoint> : IInterval<TEndpoint>
	{
		/// <summary>Initializes a new instance of the <see cref="Interval{TEndpoint}"/> struct with the specified endpoints.</summary>
		/// <param name="start">The inclusive starting point of the interval.</param>
		/// <param name="end">The inclusive ending point of the interval.</param>
		/// <param name="comparer">An <see cref="IComparer{T}"/> to compare endpoints.  If <c>null</c> will use
		/// <see cref="Comparer{T}.Default"/> instead.</param>
		public Interval(TEndpoint start, TEndpoint end, IComparer<TEndpoint> comparer = null)
		{
			if (comparer == null)
				comparer = Comparer<TEndpoint>.Default;
			if (comparer.Compare(start, end) > 0)
				throw new ArgumentException("Interval end must occur after the interval start.");

			Start = start;
			End = end;
		}

		/// <summary>Initializes a new instance of the <see cref="Interval{TEndpoint}"/> struct as a copy of the specified
		/// interval.</summary>
		/// <param name="interval">Interval to copy.</param>
		/// <param name="comparer">An <see cref="IComparer{T}"/> to compare endpoints.  If <c>null</c> will use
		/// <see cref="Comparer{T}.Default"/> instead.</param>
		public Interval(IInterval<TEndpoint> interval, IComparer<TEndpoint> comparer = null)
		{
			if (interval == null)
				throw new ArgumentNullException(nameof(interval));
			if (comparer == null)
				comparer = Comparer<TEndpoint>.Default;
			if (comparer.Compare(interval.Start, interval.End) > 0)
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
	}
}
