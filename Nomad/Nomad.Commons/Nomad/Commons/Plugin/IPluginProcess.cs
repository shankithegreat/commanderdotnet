namespace Nomad.Commons.Plugin
{
    using System;

    public interface IPluginProcess
    {
        bool Shutdown();
        bool Start();

        bool IsAlive { get; }

        bool KeepAlive { get; set; }
    }
}

