namespace Nomad.Commons.Plugin
{
    using System;

    public interface IPluginController
    {
        void Shutdown();

        bool KeepAlive { get; set; }
    }
}

