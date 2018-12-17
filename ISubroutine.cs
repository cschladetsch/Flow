// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow
{
    /// Subroutine is a Generator, implemented as a direct method call.
    public interface ISubroutine : IGenerator
    {
    }

    public interface ISubroutine<out T> : IGenerator<T>
    {
    }
}

