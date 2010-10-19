namespace Nomad.FileSystem.Property.Providers
{
    using Microsoft.COM;
    using System;
    using System.Runtime.InteropServices;

    [ClassInterface(ClassInterfaceType.AutoDispatch), ComVisible(true)]
    public class DummyClientSite : IOleClientSite
    {
        internal DummyClientSite()
        {
        }

        public void GetContainer(out IOleContainer container)
        {
            throw new NotSupportedException();
        }

        public int GetMoniker(OLEGETMONIKER dwAssign, OLEWHICHMK dwWhichMoniker, out object moniker)
        {
            throw new NotSupportedException();
        }

        [DispId(-5512)]
        public virtual int IDispatch_Invoke_Handler()
        {
            return 0x60000780;
        }

        public void OnShowWindow(bool fShow)
        {
            throw new NotSupportedException();
        }

        public void RequestNewObjectLayout()
        {
            throw new NotSupportedException();
        }

        public int SaveObject()
        {
            throw new NotSupportedException();
        }

        public void ShowObject()
        {
            throw new NotSupportedException();
        }
    }
}

