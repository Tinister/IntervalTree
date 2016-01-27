using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace IntervalTreeNS
{
	/// <summary>Represents an interval with the specified endpoint type.</summary>
	/// <typeparam name="TEndpoint">Type of the interval endpoints.</typeparam>
	public interface IInterval<out TEndpoint>
		where TEndpoint : IComparable<TEndpoint>
	{
		/// <summary>Gets the inclusive starting point of the interval.</summary>
		TEndpoint Start { get; }

		/// <summary>Gets the inclusive ending point of the interval.</summary>
		[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "End",
			Justification = "Not sure which .NET languages reserves 'End'.")]
		TEndpoint End { get; }
	}
}
