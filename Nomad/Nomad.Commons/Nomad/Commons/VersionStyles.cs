namespace Nomad.Commons
{
    using System;

    [Flags]
    public enum VersionStyles
    {
        AllowMajorMinor = 2,
        AllowMajorMinorBuild = 4,
        AllowMajorMinorBuildRevision = 8,
        Any = 14,
        None = 0
    }
}

