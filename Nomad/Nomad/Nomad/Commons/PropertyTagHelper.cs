namespace Nomad.Commons
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.Text;

    public static class PropertyTagHelper
    {
        public static Type GetPropertyType(PropertyTag tag)
        {
            switch (tag)
            {
                case PropertyTag.ImageWidth:
                case PropertyTag.ImageHeight:
                    return typeof(int);

                case PropertyTag.ImageDescription:
                case PropertyTag.EquipMake:
                case PropertyTag.EquipModel:
                case PropertyTag.SoftwareUsed:
                case PropertyTag.ExifUserComment:
                    return typeof(string);

                case PropertyTag.Orientation:
                    return typeof(RotateFlipType);

                case PropertyTag.ExifLightSource:
                    return typeof(LightSource);

                case PropertyTag.ExifDTOrig:
                case PropertyTag.ExifDTDigitized:
                    return typeof(DateTime);

                case PropertyTag.ExifISOSpeed:
                    return typeof(short);
            }
            return typeof(object);
        }

        public static object GetPropertyValue(PropertyItem item)
        {
            if (item != null)
            {
                int num;
                switch (item.Type)
                {
                    case 1:
                    case 7:
                        if (item.Value.Length != 1)
                        {
                            return item.Value;
                        }
                        return item.Value[0];

                    case 2:
                    {
                        string s = Encoding.ASCII.GetString(item.Value, 0, item.Len - 1);
                        switch (item.Id)
                        {
                            case 0x9003:
                            case 0x9004:
                                return DateTime.ParseExact(s, "yyyy':'MM':'dd HH':'mm':'ss", CultureInfo.InvariantCulture);
                        }
                        return s;
                    }
                    case 3:
                    {
                        ushort[] numArray = new ushort[item.Len / 2];
                        for (num = 0; num < numArray.Length; num++)
                        {
                            numArray[num] = BitConverter.ToUInt16(item.Value, num * 2);
                        }
                        if (numArray.Length != 1)
                        {
                            return numArray;
                        }
                        PropertyTag id = (PropertyTag) item.Id;
                        if (id != PropertyTag.Orientation)
                        {
                            if (id != PropertyTag.ExifLightSource)
                            {
                                return numArray[0];
                            }
                        }
                        else
                        {
                            switch (numArray[0])
                            {
                                case 2:
                                    return RotateFlipType.RotateNoneFlipX;

                                case 3:
                                    return RotateFlipType.Rotate180FlipNone;

                                case 4:
                                    return RotateFlipType.Rotate180FlipX;

                                case 5:
                                    return RotateFlipType.Rotate270FlipX;

                                case 6:
                                    return RotateFlipType.Rotate270FlipNone;

                                case 7:
                                    return RotateFlipType.Rotate90FlipX;

                                case 8:
                                    return RotateFlipType.Rotate90FlipNone;
                            }
                            return RotateFlipType.RotateNoneFlipNone;
                        }
                        LightSource source = (LightSource) numArray[0];
                        if (!Enum.IsDefined(typeof(LightSource), source))
                        {
                            return LightSource.Reserved;
                        }
                        return source;
                    }
                    case 4:
                    {
                        uint[] numArray2 = new uint[item.Len / 4];
                        for (num = 0; num < numArray2.Length; num++)
                        {
                            numArray2[num] = BitConverter.ToUInt32(item.Value, num * 4);
                        }
                        if (numArray2.Length == 1)
                        {
                            return numArray2[0];
                        }
                        return numArray2;
                    }
                    case 9:
                    {
                        int[] numArray3 = new int[item.Len / 4];
                        for (num = 0; num < numArray3.Length; num++)
                        {
                            numArray3[num] = BitConverter.ToInt32(item.Value, num * 4);
                        }
                        if (numArray3.Length == 1)
                        {
                            return numArray3[0];
                        }
                        return numArray3;
                    }
                }
            }
            return null;
        }

        public static object GetPropertyValue(Image image, PropertyTag tag)
        {
            if (image == null)
            {
                throw new ArgumentNullException();
            }
            try
            {
                return GetPropertyValue(image.GetPropertyItem((int) tag));
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}

