using System;

namespace Shell
{
    public static class ShellGuids
    {
        public static Guid Desktop = new Guid("{00021400-0000-0000-C000-000000000046}");

        public static Guid IShellFolder = new Guid("{000214E6-0000-0000-C000-000000000046}");
        public static Guid IShellItem = new Guid("{43826d1e-e718-42ee-bc55-a1e261c37bfe}");
        public static Guid IContextMenu = new Guid("{000214e4-0000-0000-c000-000000000046}");
        public static Guid IContextMenu2 = new Guid("{000214f4-0000-0000-c000-000000000046}");
        public static Guid IContextMenu3 = new Guid("{bcfce0a0-ec17-11d0-8d10-00a0c90f2719}");

        public static Guid IDropTarget = new Guid("{00000122-0000-0000-C000-000000000046}");
        public static Guid IDataObject = new Guid("{0000010e-0000-0000-C000-000000000046}");

        public static Guid IQueryInfo = new Guid("{00021500-0000-0000-C000-000000000046}");
        public static Guid IPersistFile = new Guid("{0000010b-0000-0000-C000-000000000046}");

        public static Guid DragDropHelper = new Guid("{4657278A-411B-11d2-839A-00C04FD918D0}");
        public static Guid NewMenu = new Guid("{D969A300-E7FF-11d0-A93B-00A0C90F2719}");
        public static Guid IDragSourceHelper = new Guid("{DE5BF786-477A-11d2-839D-00C04FD918D0}");
        public static Guid IDropTargetHelper = new Guid("{4657278B-411B-11d2-839A-00C04FD918D0}");

        public static Guid IShellExtInit = new Guid("{000214e8-0000-0000-c000-000000000046}");
        public static Guid IStream = new Guid("{0000000c-0000-0000-c000-000000000046}");
        public static Guid IStorage = new Guid("{0000000B-0000-0000-C000-000000000046}");

        public static Guid ShellFolderObject = new Guid("{3981e224-f559-11d3-8e3a-00c04f6837d5}");
    }
}