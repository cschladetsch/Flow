// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow
{
    public delegate void TriggerHandler(ITrigger trigger, ITransient reason);

    public interface ITrigger : IGroup
    {
        event TriggerHandler Tripped;

        ITransient Reason { get; }
    }
}
