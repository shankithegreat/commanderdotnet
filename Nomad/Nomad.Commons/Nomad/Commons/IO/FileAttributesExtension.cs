namespace Nomad.Commons.IO
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    public static class FileAttributesExtension
    {
        public static bool CheckAttribute(this FileAttributes attributes, FileAttributes attribute)
        {
            return ((attributes & attribute) == attribute);
        }
    }
}

