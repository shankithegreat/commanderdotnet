namespace Nomad.FileSystem.Virtual
{
    using Nomad.Commons.IO;
    using Nomad.Properties;
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class VirtualFolderBranch : VirtualSearchFolder
    {
        public VirtualFolderBranch(IVirtualFolder Folder) : base(Folder, new VirtualItemAttributeFilter(0, FileAttributes.Directory), SearchFolderOptions.DetectChanges | SearchFolderOptions.ProcessSubfolders)
        {
        }

        protected VirtualFolderBranch(SerializationInfo info, StreamingContext context) : base((IVirtualFolder) info.GetValue("Folder", typeof(IVirtualFolder)), new VirtualItemAttributeFilter(0, FileAttributes.Directory), SearchFolderOptions.DetectChanges | SearchFolderOptions.ProcessSubfolders)
        {
        }

        public override bool Equals(IVirtualItem other)
        {
            VirtualFolderBranch branch = other as VirtualFolderBranch;
            return ((this == other) || ((branch != null) && base.Folder.Equals(branch.Folder)));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Folder", base.Folder);
        }

        public override bool IsChild(IVirtualItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return item.FullName.StartsWith(PathHelper.IncludeTrailingDirectorySeparator(base.Folder.FullName), StringComparison.OrdinalIgnoreCase);
        }

        public override string Name
        {
            get
            {
                return Resources.sFolderBranch;
            }
        }
    }
}

