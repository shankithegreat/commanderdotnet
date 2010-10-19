namespace Nomad.Commons
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public static class ExceptionHelper
    {
        public static void TraceException(this TraceSource trace, TraceEventType eventType, Exception e)
        {
            trace.TraceEvent(eventType, 0, ErrorReport.ExceptionToString(e));
        }
    }
}

