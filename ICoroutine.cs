<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

namespace Flow
{
    public interface ICoroutine : IGenerator
    {
    }

    public interface ICoroutine<out T> : IGenerator<T>
    {
    }
}
