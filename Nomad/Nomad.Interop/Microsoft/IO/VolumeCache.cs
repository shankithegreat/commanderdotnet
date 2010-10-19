namespace Microsoft.IO
{
    using Microsoft;
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public static class VolumeCache
    {
        private static Dictionary<string, WeakReference> VolumeMap = new Dictionary<string, WeakReference>(StringComparer.OrdinalIgnoreCase);

        public static VolumeInfo FromPath(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException();
            }
            if (OS.IsWin2k)
            {
                WeakReference reference;
                StringBuilder lpszVolumePathName = new StringBuilder(0x400);
                if (!Windows.GetVolumePathName(path, lpszVolumePathName, lpszVolumePathName.Capacity))
                {
                    return null;
                }
                string key = lpszVolumePathName.ToString();
                if (VolumeMap.TryGetValue(key, out reference) && reference.IsAlive)
                {
                    return (VolumeInfo) reference.Target;
                }
                VolumeInfo target = new VolumeInfo(key);
                reference = new WeakReference(target);
                VolumeMap[key] = reference;
                if (key.Length == 3)
                {
                    VolumeMap[target.Name] = reference;
                }
                return target;
            }
            return Get(Path.GetPathRoot(path));
        }

        public static VolumeInfo Get(string driveName)
        {
            WeakReference reference;
            if (driveName == null)
            {
                throw new ArgumentNullException();
            }
            if (VolumeMap.TryGetValue(driveName, out reference) && reference.IsAlive)
            {
                return (VolumeInfo) reference.Target;
            }
            VolumeInfo target = new VolumeInfo(driveName);
            reference = new WeakReference(target);
            VolumeMap[driveName] = reference;
            if (OS.IsWin2k && (driveName.Length == 3))
            {
                VolumeMap[target.Name] = reference;
            }
            return target;
        }
    }
}

