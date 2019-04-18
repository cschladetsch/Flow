<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

using System;

namespace Flow
{
<<<<<<< HEAD
=======
    public enum EDebugLevel
    {
        None,
        Low,
        Medium,
        High,
        Verbose,
    }

>>>>>>> 2156678... Updated to .Net4.5
    /// <inheritdoc />
    /// <summary>
    /// A Kernel contains a top-entryType root Node, and a local TimeFrame.
    /// <para>
<<<<<<< HEAD
    /// When the Kernel is Stepped, it updates its Time property, Steps the Root node, then calls Post on the
    /// Root node.
    /// </para>
    /// </summary>
    public interface IKernel 
        : IGenerator
=======
    /// When the Kernel is Stepped, it updates its Time property, Steps the top-entryType Root node, then calls Post on the
    /// top-entryType Root node.
    /// </para>
    /// </summary>
    public interface IKernel : IGenerator
>>>>>>> 2156678... Updated to .Net4.5
    {
        EDebugLevel DebugLevel { get; set; }
        ILogger Log { get; set; }

<<<<<<< HEAD
        bool Break { get; }
        INode Root { get; set; }
        IFactory Factory { get; }
        ITimeFrame Time { get; }

        void Update(float deltaSeconds);
        void Wait(TimeSpan duration);
        void BreakFlow();
=======
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
>>>>>>> 2156678... Updated to .Net4.5
    }
}
