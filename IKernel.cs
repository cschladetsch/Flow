// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;
using Flow.Logger;

namespace Flow
{
    public enum EDebugLevel
    {
        None,
        Low,
        Medium,
        High,
        Verbose,
    }

    /// <inheritdoc />
    /// <summary>
    /// A Kernel contains a top-entryType root Node, and a local TimeFrame.
    /// <para>
    /// When the Kernel is Stepped, it updates its Time property, Steps the top-entryType Root node, then calls Post on the
    /// top-entryType Root node.
    /// </para>
    /// </summary>
    public interface IKernel : IGenerator
    {
        EDebugLevel DebugLevel { get; set; }
        ILogger Log { get; set; }

        // true to break out of loops
        bool Break { get; }

        INode Root { get; set; }
        IFactory Factory { get; }
        ITimeFrame Time { get; }
        void Update(float deltaSeconds);
        void Wait(TimeSpan end);
        void WaitSteps(int numSteps);
        void BreakFlow();
        void StepTime();
        void ContinueFlow();
    }
}
