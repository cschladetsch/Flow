using System;

namespace Flow.Impl.Detail
{
    /// <summary>
    /// Perform an arbitrary action at every step.
    /// </summary>
    internal class EveryTime : Generator
    {
        public EveryTime(Action act)
        {
            _act = act;
        }

        public override void Step()
        {
            if (!Active)
                return;

            _act();
        }

        readonly Action _act;
    }

    /// <summary>
    /// Perform an arbitrary action at first step, then Complete.
    /// </summary>
    internal class OneTime : EveryTime
    {
        public OneTime(Action act) : base(act) { }

        public override void Step()
        {
            base.Step();
            Complete();
        }
    }
}
