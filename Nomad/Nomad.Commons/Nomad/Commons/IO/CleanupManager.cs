namespace Nomad.Commons.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class CleanupManager
    {
        private static Dictionary<string, bool> FileSystemCleanup = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

        public static void Add(string fileName)
        {
            if (Directory.Exists(fileName))
            {
                Add(PathHelper.ExcludeTrailingDirectorySeparator(fileName), true);
            }
            else if (File.Exists(fileName))
            {
                Add(fileName, false);
            }
        }

        private static void Add(string fileName, bool directory)
        {
            lock (FileSystemCleanup)
            {
                FileSystemCleanup[fileName] = directory;
            }
        }

        public static void AddDirectory(string directoryName)
        {
            if (directoryName == null)
            {
                throw new ArgumentNullException();
            }
            if (directoryName == string.Empty)
            {
                throw new ArgumentException();
            }
            Add(PathHelper.ExcludeTrailingDirectorySeparator(directoryName), true);
        }

        public static void AddFile(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException();
            }
            if (fileName == string.Empty)
            {
                throw new ArgumentException();
            }
            Add(fileName, false);
        }

        public static void Cleanup()
        {
            lock (FileSystemCleanup)
            {
                if (FileSystemCleanup.Count != 0)
                {
                    List<string> list = new List<string>();
                    foreach (KeyValuePair<string, bool> pair in FileSystemCleanup)
                    {
                        try
                        {
                            if (pair.Value)
                            {
                                Directory.Delete(pair.Key, true);
                            }
                            else
                            {
                                File.Delete(pair.Key);
                            }
                            list.Add(pair.Key);
                        }
                        catch (IOException)
                        {
                        }
                    }
                    foreach (string str in list)
                    {
                        FileSystemCleanup.Remove(str);
                    }
                }
            }
        }

        public static bool Remove(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException();
            }
            lock (FileSystemCleanup)
            {
                return FileSystemCleanup.Remove(PathHelper.ExcludeTrailingDirectorySeparator(fileName));
            }
        }
    }
}

