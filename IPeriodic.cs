<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

using System;

namespace Flow
{
    /// <inheritdoc />
    /// <summary>
    /// Periodic instances regularly fire their Elapsed event.
<<<<<<< HEAD
    /// <para>
    /// NOTE the timer will fire at most once per Kernel Step
    /// </para>
=======
    /// NOTE the timer will fire at most once per Kernel Step
>>>>>>> 2156678... Updated to .Net4.5
    /// </summary>
    public interface IPeriodic : IGenerator
    {
        // Periodically fires when the timer has elapsed. Fired at most once per Kernel Step
        event TransientHandler Elapsed;

        // The time that this periodic timer was started.
        DateTime TimeStarted { get; }

        // Successive Elapsed events will have not less than this TimeSpan between being fired.
        TimeSpan Interval { get; }

        TimeSpan TimeRemaining { get; }
    }
}
