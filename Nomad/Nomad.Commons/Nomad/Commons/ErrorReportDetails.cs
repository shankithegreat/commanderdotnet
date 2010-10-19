namespace Nomad.Commons
{
    using System;

    [Flags]
    public enum ErrorReportDetails
    {
        All = 7,
        Assemblies = 4,
        Data = 1,
        Environment = 2
    }
}

