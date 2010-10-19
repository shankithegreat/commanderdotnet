namespace Nomad.FileSystem.Virtual.Filter
{
    using Nomad;
    using Nomad.Commons.IO;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;

    internal class RestoreSelectionFilter : IVirtualItemFilter, IEquatable<IVirtualItemFilter>
    {
        private Dictionary<string, int> FileSystemFiles;
        private bool FullPath;
        private Dictionary<IVirtualItem, int> VirtualItems;

        public RestoreSelectionFilter()
        {
            if (Clipboard.ContainsData("Virtual Items"))
            {
                this.VirtualItems = new Dictionary<IVirtualItem, int>();
                foreach (IVirtualItem item in VirtualClipboardItem.GetClipboardItems())
                {
                    this.VirtualItems.Add(item, 0);
                }
            }
            else if (Clipboard.ContainsFileDropList())
            {
                this.FileSystemFiles = new Dictionary<string, int>();
                foreach (string str in Clipboard.GetFileDropList())
                {
                    this.FileSystemFiles.Add(PathHelper.ExcludeTrailingDirectorySeparator(str), 0);
                }
                this.FullPath = true;
            }
            else if (Clipboard.ContainsText())
            {
                this.FullPath = true;
                this.FileSystemFiles = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                using (TextReader reader = new StringReader(Clipboard.GetText()))
                {
                    string str2;
                    while ((str2 = reader.ReadLine()) != null)
                    {
                        try
                        {
                            if (!(this.FullPath && !(Path.GetDirectoryName(str2) == string.Empty)))
                            {
                                this.FullPath = false;
                            }
                            this.FileSystemFiles.Add(str2, 0);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        public static bool ClipboardContainsSelection()
        {
            return ((Clipboard.ContainsData("Virtual Items") || Clipboard.ContainsFileDropList()) || Clipboard.ContainsText());
        }

        public bool Equals(IVirtualItemFilter other)
        {
            return false;
        }

        public bool IsMatch(IVirtualItem Item)
        {
            if (this.VirtualItems != null)
            {
                return this.VirtualItems.ContainsKey(Item);
            }
            if (this.FileSystemFiles != null)
            {
                if (this.FullPath)
                {
                    return this.FileSystemFiles.ContainsKey(PathHelper.ExcludeTrailingDirectorySeparator(Item.FullName));
                }
                return this.FileSystemFiles.ContainsKey(Item.Name);
            }
            return false;
        }
    }
}

