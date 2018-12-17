using System;

namespace Flow.Impl.Detail
{
    /// <summary>
    /// Perform an arbitrary action at every step.
    /// </summary>
<<<<<<< HEAD
    internal class EveryTime
        : Generator
=======
    internal class EveryTime : Generator
>>>>>>> 2156678... Updated to .Net4.5
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
<<<<<<< HEAD
    internal class OneTime
        : EveryTime
=======
    internal class OneTime : EveryTime
>>>>>>> 2156678... Updated to .Net4.5
    {
        public OneTime(Action act) : base(act) { }

        public override void Step()
        {
            base.Step();
            Complete();
        }
    }
}
