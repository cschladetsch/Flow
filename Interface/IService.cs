namespace Flow
{
	/// <summary>
	/// Service state change handler.
	/// </summary>
    public delegate void ServiceStateChangeHandler(IService service, EState oldState, EState newState);

	/// <summary>
	/// Not implemented.
	/// </summary>
    public interface IService : IGenerator
    {
		/// <summary>
		/// Gets the state.
		/// </summary>
		/// <value>
		/// The state.
		/// </value>
        EState State { get; }

		/// <summary>
		/// Occurs when state changed.
		/// </summary>
        event ServiceStateChangeHandler StateChanged;

		/// <summary>
		/// Start this instance.
		/// </summary>
        void Start();
        
		/// <summary>
		/// Stop this instance.
		/// </summary>
        void Stop();
    }
}