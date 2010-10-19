namespace Nomad.Commons.Drawing
{
    using Nomad.Commons;
    using System;

    public class IconLocation
    {
        public readonly string IconFileName;
        public readonly int IconIndex;

        public IconLocation(string iconFileName, int iconIndex)
        {
            if (iconFileName == null)
            {
                throw new ArgumentNullException();
            }
            this.IconFileName = Unquote(iconFileName);
            this.IconIndex = iconIndex;
        }

        private IconLocation(string iconFileName, int iconIndex, int dummy)
        {
            this.IconFileName = iconFileName;
            this.IconIndex = iconIndex;
        }

        public override bool Equals(object obj)
        {
            IconLocation location = obj as IconLocation;
            return (((location != null) && (this.IconIndex == location.IconIndex)) && string.Equals(this.IconFileName, location.IconFileName, StringComparison.OrdinalIgnoreCase));
        }

        public static bool Equals(IconLocation x, IconLocation y)
        {
            return ((((x != null) && (y != null)) && x.Equals(y)) || ((x == null) && (y == null)));
        }

        public override int GetHashCode()
        {
            return (this.IconIndex.GetHashCode() ^ this.IconFileName.GetHashCode());
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", this.IconFileName, this.IconIndex);
        }

        public static IconLocation TryParse(string iconLocation)
        {
            if (iconLocation == null)
            {
                return null;
            }
            string[] strArray = iconLocation.Split(new char[] { ',' });
            string str = Unquote(strArray[0]);
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            int result = 0;
            if (strArray.Length > 1)
            {
                int.TryParse(strArray[1], out result);
            }
            return new IconLocation(str, result, 0);
        }

        private static string Unquote(string str)
        {
            if (((str.Length > 1) && str.StartsWith('"')) && str.EndsWith('"'))
            {
                return str.Substring(1, str.Length - 2);
            }
            return str;
        }
    }
}

