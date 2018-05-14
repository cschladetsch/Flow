namespace Flow.Impl
{
    internal class Break : Generator, IBreak
    {
        public override void Step()
        {
            Kernel.BreakFlow();
        }
    }
}