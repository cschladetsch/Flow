<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow.Impl
{
    internal class Break
        : Generator
        , IBreak
=======
namespace Flow.Impl
{
    internal class Break : Generator, IBreak
>>>>>>> 2156678... Updated to .Net4.5
    {
        public override void Step()
        {
            Kernel.BreakFlow();
        }
    }
}