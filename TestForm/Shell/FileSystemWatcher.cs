using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ShellDll;

namespace Commander.Shell
{
    public class FileSystemWatcher : NativeWindow
    {
        private uint notifyId;
        private bool enabled = false;
        private bool includeSubdirectories = false;
        private string directory = "";

        public FileSystemWatcher()
        {
            CreateHandle(new CreateParams());
        }

        ~FileSystemWatcher()
        {
            UnRegisterShellNotify();
            GC.SuppressFinalize(this);
        }

        public event FileSystemEventHandler Changed;


        [DefaultValue(false)]
        public bool EnableRaisingEvents
        {
            get
            {
                return this.enabled;
            }
            set
            {
                if (this.enabled != value)
                {
                    this.enabled = value;
                    if (this.enabled)
                    {
                        RegisterShellNotify();
                    }
                    else
                    {
                        UnRegisterShellNotify();
                    }
                }
            }
        }

        [DefaultValue(false)]
        public bool IncludeSubdirectories
        {
            get
            {
                return this.includeSubdirectories;
            }
            set
            {
                if (this.includeSubdirectories != value)
                {
                    this.includeSubdirectories = value;
                    OnUpdate();
                }
            }
        }

        [Editor("System.Diagnostics.Design.FSWPathEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), DefaultValue(""), RecommendedAsConfigurable(true), IODescription("FSW_Path"), TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public string Path
        {
            get
            {
                return this.directory;
            }
            set
            {
                value = (value == null) ? string.Empty : value;
                if (string.Compare(this.directory, value, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    if (!Directory.Exists(value))
                    {
                        throw new ArgumentException(string.Format("Invalid directory name: \"{0}\"", value));
                    }
                    this.directory = value;
                    OnUpdate();
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)ShellAPI.WM.SH_NOTIFY)
            {
                ShellAPI.SHNOTIFYSTRUCT shNotify = (ShellAPI.SHNOTIFYSTRUCT)Marshal.PtrToStructure(m.WParam, typeof(ShellAPI.SHNOTIFYSTRUCT));

                switch ((ShellAPI.SHCNE)m.LParam)
                {
                    case ShellAPI.SHCNE.MKDIR:
                    case ShellAPI.SHCNE.UPDATEDIR:
                        {
                            OnChanged();
                            break;
                        }
                }
            }

            base.WndProc(ref m);
        }

        protected virtual void OnUpdate()
        {
            if (this.enabled)
            {
                RegisterShellNotify();
            }
        }

        protected virtual void OnChanged()
        {
            if (Changed != null)
            {
                Changed();
            }
        }

        private void RegisterShellNotify()
        {
            ShellAPI.SHChangeNotifyEntry entry = new ShellAPI.SHChangeNotifyEntry();
            entry.pIdl = ShellFolder.GetPathPIDL(this.Path); ;
            entry.Recursively = this.IncludeSubdirectories;

            this.notifyId = ShellAPI.SHChangeNotifyRegister(
                                                            this.Handle,
                                                            ShellAPI.SHCNRF.NewDelivery | ShellAPI.SHCNRF.InterruptLevel | ShellAPI.SHCNRF.ShellLevel,
                                                            ShellAPI.SHCNE.ALLEVENTS,
                                                            ShellAPI.WM.SH_NOTIFY,
                                                            1,
                                                            new ShellAPI.SHChangeNotifyEntry[] { entry });
        }

        private void UnRegisterShellNotify()
        {
            if (this.notifyId > 0)
            {
                ShellAPI.SHChangeNotifyDeregister(this.notifyId);
            }
        }
    }

    public delegate void FileSystemEventHandler();
}
