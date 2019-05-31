// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow
{
    public delegate void GeneratorHandler(IGenerator generator);

    public interface ISteppable
    {
        void Step();
    }

    /// <inheritdoc cref="ITransient" />
    /// <summary>
    /// A Generator does some work every time its Step method is called, unless it is Suspended or Completed.
    /// <para>All Generators are Resumed when they are first created by a Factory</para>
    /// </summary>
    public interface IGenerator 
        : ITransient
        , ISteppable
    {
        event GeneratorHandler Resumed;
        event GeneratorHandler Stepped;
        event GeneratorHandler Suspended;

        bool Running { get; }
        int StepNumber { get; }
        object Value { get; }

        void Resume();
        void Pre();
        void Post();
        void Suspend();

        new IGenerator Named(string name);
        IGenerator SuspendAfter(ITransient other);
        IGenerator SuspendAfter(TimeSpan span);
        IGenerator ResumeAfter(Func<bool> pred);
        IGenerator ResumeAfter(ITransient other);
        IGenerator ResumeAfter(TimeSpan span);
    }

    public delegate void GeneratorHandler<in T>(IGenerator<T> generator);

    public interface IGenerator<out T> : IGenerator
    {
        new T Value { get; }
    }
}
