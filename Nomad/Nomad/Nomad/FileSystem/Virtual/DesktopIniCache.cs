namespace Nomad.FileSystem.Virtual
{
    using Nomad;
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using Nomad.FileSystem.LocalFile;
    using System;
    using System.IO;

    public static class DesktopIniCache
    {
        private static PersistentIni CatalogIni;
        private static Ini.IniSection CatalogSection;
        private static string SectionDesktopIni = "DesktopIni";

        private static void CatalogIniNeeded()
        {
            if (CatalogIni == null)
            {
                CatalogIni = new PersistentIni(Path.Combine(SettingsManager.SpecialFolders.DesktopIni, "catalog.ini"));
                CatalogIni.Read();
            }
            if (CatalogSection == null)
            {
                CatalogSection = CatalogIni[SectionDesktopIni];
                if (CatalogSection == null)
                {
                    CatalogSection = CatalogIni.AddSection(SectionDesktopIni);
                }
            }
        }

        public static bool Contains(string folder)
        {
            CatalogIniNeeded();
            return CatalogSection.Contains(folder);
        }

        public static DesktopIni Get(string folder)
        {
            CatalogIniNeeded();
            string str = CatalogSection[folder];
            if (string.IsNullOrEmpty(str))
            {
                str = StringHelper.GuidToCompactString(Guid.NewGuid());
            }
            return new CachedDesktopIni(GetDesktopIniPath(str), folder, str);
        }

        private static string GetDesktopIniPath(string key)
        {
            return Path.Combine(SettingsManager.SpecialFolders.DesktopIni, Path.ChangeExtension(key, ".ini"));
        }

        public static string GetPath(string folder)
        {
            CatalogIniNeeded();
            string str = CatalogSection[folder];
            if (!string.IsNullOrEmpty(str))
            {
                return GetDesktopIniPath(str);
            }
            return null;
        }

        public static void Remove(string folder)
        {
            if (CatalogSection != null)
            {
                string str = CatalogSection[folder];
                CatalogSection.Remove(folder);
                if (string.IsNullOrEmpty(str))
                {
                    string desktopIniPath = GetDesktopIniPath(str);
                    if (File.Exists(desktopIniPath))
                    {
                        File.Delete(desktopIniPath);
                        LocalFileSystemCreator.RaiseFileChangedEvent(WatcherChangeTypes.Deleted, desktopIniPath);
                    }
                }
                UpdateCatalogIni();
            }
        }

        private static void UpdateCatalogIni()
        {
            WatcherChangeTypes created = WatcherChangeTypes.Created;
            if (File.Exists(CatalogIni.FileName))
            {
                created = WatcherChangeTypes.Changed;
            }
            if (!CatalogIni.HasValues)
            {
                File.Delete(CatalogIni.FileName);
                created = WatcherChangeTypes.Deleted;
            }
            else
            {
                CatalogIni.Write();
            }
            LocalFileSystemCreator.RaiseFileChangedEvent(created, CatalogIni.FileName);
        }

        private class CachedDesktopIni : DesktopIni
        {
            private string Folder;
            private string Key;

            public CachedDesktopIni(string iniPath, string folder, string key) : base(iniPath)
            {
                this.Folder = folder;
                this.Key = key;
            }

            public override void Write()
            {
                Directory.CreateDirectory(SettingsManager.SpecialFolders.DesktopIni);
                base.Write();
                if (!base.HasValues)
                {
                    DesktopIniCache.CatalogSection.Remove(this.Folder);
                }
                else
                {
                    DesktopIniCache.CatalogSection[this.Folder] = this.Key;
                }
                DesktopIniCache.UpdateCatalogIni();
            }

            public override CustomizeFolderParts UpdatableParts
            {
                get
                {
                    return (CustomizeFolderParts.ListColumnCount | CustomizeFolderParts.Colors | CustomizeFolderParts.ThumbnailSize | CustomizeFolderParts.View | CustomizeFolderParts.Sort | CustomizeFolderParts.Filter | CustomizeFolderParts.Columns);
                }
            }
        }
    }
}

