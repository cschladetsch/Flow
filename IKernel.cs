// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow
{
    /// When the Kernel is Stepped, it updates its Time property, Steps the Root node, then calls Post on the
    /// Root node.
    public interface IKernel
        : IGenerator
    {
        EDebugLevel DebugLevel { get; set; }
        ILogger Log { get; set; }

        bool Break { get; }
        INode Root { get; set; }
        IFactory Factory { get; }
        ITimeFrame Time { get; }

        void Update(float deltaSeconds);
        void Wait(TimeSpan duration);
        void BreakFlow();
    }
}
