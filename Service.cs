namespace Flow
{

	/// <summary>
	/// Service. Not implemented.
	/// </summary>
    internal abstract class Service : Generator<bool>, IService
    {
        public EState State { get; private set; }

        public event ServiceStateChangeHandler StateChanged;

        public override bool Step()
        {
            throw new System.NotImplementedException();
        }

        public void Start()
        {
            if (!Exists)
                return;

            if (InState(EState.Started))
                return;

            Stop();

            ChangeState(EState.Starting);
        }

        private void ChangeState(EState state)
        {
            if (state == State)
                return;

            if (StateChanged != null)
                StateChanged(this, State, state);

            State = state;
        }

        private bool InState(EState state)
        {
            return State == state;
        }

        public void Stop()
        {
            if (!Exists)
                return;


        }
    }
}