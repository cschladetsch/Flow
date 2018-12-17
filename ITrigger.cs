<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

namespace Flow
{
    public delegate void TriggerHandler(ITrigger trigger, ITransient reason);

    public interface ITrigger : IGroup
    {
        event TriggerHandler Tripped;

        ITransient Reason { get; }
    }
}
