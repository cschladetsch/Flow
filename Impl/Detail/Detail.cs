using System;

namespace Flow.Impl.Detail
{
    class EveryTime : Generator
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

    class OneTime : EveryTime
    {
        public OneTime(Action act) : base(act) { }

        public override void Step()
        {
            base.Step();
            Complete();
        }
    }
}
