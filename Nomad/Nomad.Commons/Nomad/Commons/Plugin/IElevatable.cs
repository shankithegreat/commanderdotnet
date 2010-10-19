namespace Nomad.Commons.Plugin
{
    using System;

    public interface IElevatable
    {
        bool Elevate(IPluginProcess process);

        bool CanElevate { get; }
    }
}

