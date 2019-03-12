// define this for some extra diagnostics
#define LOG_TRACE_VERBOSE

// ReSharper disable InconsistentNaming

namespace Dekuple
{
    /// <summary>
    /// Global Dekuple parameters.
    /// </summary>
    public static class Parameters
    {
#if LOG_TRACE_VERBOSE
        public static bool DefaultShowTraceStack = false;
        public static bool DefaultShowTraceSource = true;
        public static int DefaultLogVerbosity = 100;
#else
        public static bool DefaultShowTraceStack = false;
        public static bool DefaultShowTraceSource = true;
        public static int DefaultLogVerbosity = 4;
#endif
    }
}
