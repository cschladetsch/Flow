<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

namespace Flow
{
<<<<<<< HEAD
    /// <inheritdoc />
=======
>>>>>>> 2156678... Updated to .Net4.5
    /// <summary>
    /// A Barrier is a Group that Completes itself when all added Transients have been Removed from it.
    /// </summary>
    public interface IBarrier : IGroup
    {
<<<<<<< HEAD
=======
        IBarrier ForEach<T>(Action<T> act) where T : class;
>>>>>>> 2156678... Updated to .Net4.5
    }
}
