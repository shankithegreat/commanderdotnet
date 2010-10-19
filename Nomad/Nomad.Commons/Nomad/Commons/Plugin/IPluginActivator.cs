namespace Nomad.Commons.Plugin
{
    using System;

    public interface IPluginActivator
    {
        T Create<T>(string objectUri);
    }
}

