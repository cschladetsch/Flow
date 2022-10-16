// (C) 2012 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow
{
    public delegate void TransientHandler(ITransient sender);
    public delegate void TransientHandlerReason(ITransient sender, ITransient reason);

    /// <inheritdoc cref="INamed" />
    /// <inheritdoc cref="IDisposable" />
    /// <summary>
    /// An ITransient object has-a IKernel and notifies observers when it
    /// has been Disposed.
    /// </summary>
    public interface ITransient
        : INamed
    {
        event TransientHandler Completed;

        /// <summary>
        /// True if the transient has not been Completed.
        /// </summary>
        bool Active { get; }

        void Complete();

        /// <summary>
        /// The kernel that made this transient.
        /// </summary>
        IKernel Kernel { get; /* TODO internal: */ set; }

        ITransient Named(string name);
        ITransient AddTo(IGroup group);

        /// <summary>
        /// Resume the given process after this one completes.
        /// </summary>
        /// <param name="next"></param>
        //ITransient Then(IGenerator next);
        //ITransient Then(Action action);
        //ITransient Then(Action<ITransient> action);
    }
}

