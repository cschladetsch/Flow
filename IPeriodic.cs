// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using System;

namespace Flow
{
    /// <inheritdoc />
    /// <summary>
    /// Periodic instances regularly fire their Elapsed event.
    /// NOTE the timer will fire at most once per Kernel Step
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
