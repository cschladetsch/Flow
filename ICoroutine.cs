// (C) 2012 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow
{
    public interface ICoroutine
        : IGenerator
    {
    }

    public interface ICoroutine<out T>
        : IGenerator<T>
    {
    }
}
