// (C) 2012 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

namespace Flow
{
    /// <inheritdoc />
    /// <summary>
    /// When an IKernel is Stepped, it updates its Time property,
    /// and steps its Root and Detail nodes.
    /// </summary>
    public interface IKernel
        : IGenerator
    {
        EDebugLevel DebugLevel { get; set; }
        ILogger Log { get; set; }

        /// <summary>
        /// If true, the Kernel will stop mid-senten
        /// </summary>
        bool Break { get; }

        /// <summary>
        /// The root of the process tree.
        /// </summary>
        INode Root { get; set; }

        /// <summary>
        /// Used for incidentals that must be added to Kernels.
        /// </summary>
        INode Detail { get; set; }

        IFactory Factory { get; }
        ITimeFrame Time { get; }

        void Update(float deltaSeconds);
        void Wait(System.TimeSpan duration);
        void BreakFlow();
    }
}
