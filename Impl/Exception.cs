<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

using System;

namespace Flow.Impl
{
<<<<<<< HEAD
    public class FutureNotSetException
        : Exception
=======
    public class FutureNotSetException : Exception
>>>>>>> 2156678... Updated to .Net4.5
    {
        public FutureNotSetException() : base("Future value not arrived yet")
        {
        }
    }

<<<<<<< HEAD
    public class FutureAlreadySetException
        : Exception
=======
    public class FutureAlreadySetException : Exception
>>>>>>> 2156678... Updated to .Net4.5
    {
        public FutureAlreadySetException() : base("Future already set")
        {
        }
    }

<<<<<<< HEAD
    public class ReentrancyException
        : Exception
=======
    public class ReentrancyException : Exception
>>>>>>> 2156678... Updated to .Net4.5
    {
        public ReentrancyException() : base("Method is not re-entrant")
        {
        }
    }
<<<<<<< HEAD
=======

    public class EventStream<A0>
    {
        //public EventStream(Action<
    }
>>>>>>> 2156678... Updated to .Net4.5
}
