namespace Nomad.FileSystem.Virtual
{
    using Microsoft.Shell;
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows.Forms;

    public class ResolveShellLink : IResolveLink, ISerializable
    {
        private const string EntryDescription = "Description";
        private const string EntryHotkey = "Hotkey";
        private const string EntryTarget = "Target";
        private const string EntryTargetPath = "TargetPath";
        private string FDescription;
        private Keys FHotkey;
        private IVirtualItem FTarget;
        private string FTargetPath;

        internal ResolveShellLink(ShellLink link, IVirtualItem target)
        {
            this.FTarget = target;
            this.Initialize(link);
        }

        internal ResolveShellLink(ShellLink link, string targetPath)
        {
            this.FTargetPath = targetPath;
            this.Initialize(link);
        }

        protected ResolveShellLink(SerializationInfo info, StreamingContext context)
        {
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializationEntry current = enumerator.Current;
                string name = current.Name;
                if (name != null)
                {
                    if (!(name == "Target"))
                    {
                        if (name == "TargetPath")
                        {
                            goto Label_0074;
                        }
                        if (name == "Description")
                        {
                            goto Label_0088;
                        }
                        if (name == "Hotkey")
                        {
                            goto Label_009C;
                        }
                    }
                    else
                    {
                        this.FTarget = (IVirtualItem) current.Value;
                    }
                }
                continue;
            Label_0074:
                this.FTargetPath = (string) current.Value;
                continue;
            Label_0088:
                this.FDescription = (string) current.Value;
                continue;
            Label_009C:
                this.FHotkey = (Keys) current.Value;
            }
        }

        public static IResolveLink Create(ShellLink link)
        {
            if (link == null)
            {
                throw new ArgumentNullException();
            }
            string targetPath = GetTargetPath(link);
            return (string.IsNullOrEmpty(targetPath) ? null : ((IResolveLink) new ResolveShellLink(link, targetPath)));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (!string.IsNullOrEmpty(this.FTargetPath))
            {
                info.AddValue("TargetPath", this.FTargetPath);
            }
            if (this.FTarget != null)
            {
                info.AddValue("Target", this.FTarget);
            }
            if (!string.IsNullOrEmpty(this.FDescription))
            {
                info.AddValue("Description", this.FDescription);
            }
            if (this.FHotkey != Keys.None)
            {
                info.AddValue("Hotkey", this.FHotkey);
            }
        }

        private static string GetTargetPath(ShellLink link)
        {
            string path = link.Path;
            if (string.IsNullOrEmpty(path))
            {
                IntPtr idList = link.IdList;
                if (!(idList != IntPtr.Zero))
                {
                    return path;
                }
                IShellFolder desktopFolder = ShellItem.GetDesktopFolder();
                try
                {
                    path = desktopFolder.GetDisplayNameOf(idList, SHGNO.SHGDN_FORPARSING);
                }
                catch (ArgumentException)
                {
                }
                finally
                {
                    Marshal.ReleaseComObject(desktopFolder);
                }
            }
            return path;
        }

        private void Initialize(ShellLink link)
        {
            this.FHotkey = link.Hotkey;
            this.FDescription = link.Description;
        }

        public Keys Hotkey
        {
            get
            {
                return this.FHotkey;
            }
        }

        public IVirtualItem Target
        {
            get
            {
                if (!((this.FTarget != null) || string.IsNullOrEmpty(this.FTargetPath)))
                {
                    this.FTarget = VirtualItem.FromFullName(this.FTargetPath, VirtualItemType.Unknown, null);
                }
                return this.FTarget;
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
        }
    }
}

