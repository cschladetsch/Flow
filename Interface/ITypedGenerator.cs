// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	/// <summary>
	/// An instance of ITypedGenerator is-an IGenerator that has a typed Value property that is the result of the last successful Step.
	/// </summary>
	public interface ITypedGenerator<TR> : IGenerator
	{
		/// <summary>
		/// Gets the value as a result of the last Step() call.
		/// </summary>
		/// <value>
		/// The value of the last step.
		/// </value>
		TR Value { get; }
	}
}