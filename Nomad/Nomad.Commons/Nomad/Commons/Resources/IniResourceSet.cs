namespace Nomad.Commons.Resources
{
    using Nomad.Commons.IO;
    using System;
    using System.Collections;
    using System.Resources;

    public class IniResourceSet : ResourceSet
    {
        public IniResourceSet(Ini iniSource, string iniSection)
        {
            base.Reader = new IniResourceReader(iniSource, iniSection);
            this.Initialize();
        }

        public IniResourceSet(Ini iniSource, Type resourceSource)
        {
            base.Reader = new IniResourceReader(iniSource, resourceSource);
            this.Initialize();
        }

        public IniResourceSet(string iniPath, string iniSection)
        {
            base.Reader = new IniResourceReader(iniPath, iniSection);
            this.Initialize();
        }

        public IniResourceSet(string iniPath, Type resourceSource)
        {
            base.Reader = new IniResourceReader(iniPath, resourceSource);
            this.Initialize();
        }

        public override Type GetDefaultReader()
        {
            return typeof(IniResourceReader);
        }

        public override Type GetDefaultWriter()
        {
            throw new NotImplementedException();
        }

        private void Initialize()
        {
            base.Table = new Hashtable();
            this.ReadResources();
        }
    }
}

