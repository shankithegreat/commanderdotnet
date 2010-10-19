namespace Nomad.FileSystem.Property
{
    using System;

    public interface IUpdateVirtualProperty : ISetVirtualProperty, IGetVirtualProperty
    {
        void BeginUpdate();
        void EndUpdate();
    }
}

