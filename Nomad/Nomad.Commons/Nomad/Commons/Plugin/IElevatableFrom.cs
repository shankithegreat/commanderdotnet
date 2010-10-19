namespace Nomad.Commons.Plugin
{
    using System;

    public interface IElevatableFrom : IElevatable
    {
        bool ElevateFrom(IElevatable other);
    }
}

