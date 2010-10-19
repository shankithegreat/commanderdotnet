namespace Nomad.Commons
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Class)]
    public class VersionAttribute : Attribute
    {
        public VersionAttribute(System.Version version)
        {
            if (version == null)
            {
                throw new ArgumentNullException();
            }
            this.Version = (System.Version) version.Clone();
        }

        public VersionAttribute(int major, int minor)
        {
            this.Version = new System.Version(major, minor);
        }

        public VersionAttribute(int major, int minor, int build)
        {
            this.Version = new System.Version(major, minor, build);
        }

        public VersionAttribute(int major, int minor, int build, int revision)
        {
            this.Version = new System.Version(major, minor, build, revision);
        }

        public System.Version Version { get; private set; }
    }
}

