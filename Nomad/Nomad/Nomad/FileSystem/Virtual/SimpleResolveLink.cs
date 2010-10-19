namespace Nomad.FileSystem.Virtual
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class SimpleResolveLink : IResolveLink2, IResolveLink
    {
        private Keys FHotkey;
        private IVirtualItem FTarget;
        private string FTargetPath;

        public string Description { get; set; }

        public bool HasTarget
        {
            get
            {
                return ((this.FTarget != null) || !string.IsNullOrEmpty(this.FTargetPath));
            }
        }

        public Keys Hotkey
        {
            get
            {
                return this.FHotkey;
            }
            set
            {
                this.FHotkey = value;
            }
        }

        public IVirtualItem Target
        {
            get
            {
                if (!((this.FTarget != null) || string.IsNullOrEmpty(this.FTargetPath)))
                {
                    this.FTarget = VirtualItem.FromFullName(this.FTargetPath, VirtualItemType.Unknown);
                }
                return this.FTarget;
            }
            set
            {
                this.FTarget = value;
            }
        }

        public string TargetPath
        {
            get
            {
                if ((this.FTargetPath == null) && (this.FTarget != null))
                {
                    this.FTargetPath = this.FTarget.FullName;
                }
                return this.FTargetPath;
            }
            set
            {
                this.FTargetPath = value;
            }
        }
    }
}

