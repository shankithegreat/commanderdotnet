namespace Nomad.FileSystem.Virtual
{
    using Nomad.FileSystem.Property;
    using System;

    public interface IChangeVirtualItem : IPersistVirtualItem, IVirtualItem, ISimpleItem, IEquatable<IVirtualItem>, ISetVirtualProperty, IGetVirtualProperty
    {
        bool CanMoveTo(IVirtualFolder dest);
        void Delete(bool SendToBin);
        IVirtualItem MoveTo(IVirtualFolder dest);

        bool CanSendToBin { get; }

        string Name { get; set; }
    }
}

