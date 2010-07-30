using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Shell;

namespace Shell
{
    public class FileSystemWatcher : NativeWindow
    {
        private uint notifyId;
        private bool enabled;
        private bool includeSubdirectories;
        private string directory = string.Empty;


        public FileSystemWatcher()
        {
            CreateHandle(new CreateParams());
        }

        ~FileSystemWatcher()
        {
            UnRegisterShellNotify();
            GC.SuppressFinalize(this);
        }


        [DefaultValue(false)]
        public bool EnableRaisingEvents
        {
            get { return this.enabled; }
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
            get { return this.includeSubdirectories; }
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
            get { return this.directory; }
            set
            {
                value = value ?? string.Empty;
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


        public event FileSystemEventHandler Changed;


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WM.SH_NOTIFY)
            {
                SHNOTIFYSTRUCT shNotify = (SHNOTIFYSTRUCT)Marshal.PtrToStructure(m.WParam, typeof(SHNOTIFYSTRUCT));

                switch ((SHCNE)m.LParam)
                {
                    case SHCNE.MKDIR:
                    case SHCNE.UPDATEDIR:
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
            SHChangeNotifyEntry entry = new SHChangeNotifyEntry();
            entry.pIdl = ShellHelper.GetPathPIDL(this.Path);
            entry.Recursively = this.IncludeSubdirectories;

            this.notifyId = ShellApi.SHChangeNotifyRegister(this.Handle, SHCNRF.NewDelivery | SHCNRF.InterruptLevel | SHCNRF.ShellLevel, SHCNE.ALLEVENTS, WM.SH_NOTIFY, 1, new SHChangeNotifyEntry[] { entry });
        }

        private void UnRegisterShellNotify()
        {
            if (this.notifyId > 0)
            {
                ShellApi.SHChangeNotifyDeregister(this.notifyId);
            }
        }
    }

    public delegate void FileSystemEventHandler();
}