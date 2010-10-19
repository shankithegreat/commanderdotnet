namespace Nomad.Commons
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.InteropServices;

    public static class VersionHelper
    {
        public static bool TryParse(string value, VersionStyles styles, out Version result)
        {
            if ((styles & ~VersionStyles.Any) > VersionStyles.None)
            {
                throw new ArgumentException();
            }
            result = null;
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            List<int> list = new List<int>(4);
            foreach (string str in StringHelper.SplitString(value, new char[] { '.' }))
            {
                int num;
                if (int.TryParse(str, NumberStyles.None, CultureInfo.InvariantCulture, out num) && (list.Count < 5))
                {
                    list.Add(num);
                }
                else
                {
                    return false;
                }
            }
            switch (list.Count)
            {
                case 2:
                    if ((styles & VersionStyles.AllowMajorMinor) > VersionStyles.None)
                    {
                        result = new Version(list[0], list[1]);
                    }
                    break;

                case 3:
                    if ((styles & VersionStyles.AllowMajorMinorBuild) > VersionStyles.None)
                    {
                        result = new Version(list[0], list[1], list[2]);
                    }
                    break;

                case 4:
                    if ((styles & VersionStyles.AllowMajorMinorBuildRevision) > VersionStyles.None)
                    {
                        result = new Version(list[0], list[1], list[2], list[3]);
                    }
                    break;
            }
            return (result != null);
        }
    }
}

