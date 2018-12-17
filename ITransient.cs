<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

namespace Flow
{
    public delegate void TransientHandler(ITransient sender);
    public delegate void TransientHandlerReason(ITransient sender, ITransient reason);

    /// <summary>
    /// A Transient object notifies observers when it has been Completed. When a Transient is Completed,
    /// it has no more work to do and its internal state will not change without external influence.
    /// flow-control.
    /// </summary>
    public interface ITransient : INamed
    {
        event TransientHandler Completed;

        bool Active { get; }
        IKernel Kernel { get; /*internal*/ set; }

        ITransient Named(string name);
        void Complete();
    }
}
