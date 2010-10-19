namespace Nomad.Commons.IO
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Text;

    public class SummaryInformationSet : PropertySet
    {
        public static readonly Guid FMTID_SummaryInformation = new Guid("F29F85E0-4FF9-1068-AB91-08002B27B3D9");
        public const int PID_APPNAME = 0x12;
        public const int PID_AUTHOR = 4;
        public const int PID_CHARCOUNT = 0x10;
        public const int PID_COMMENTS = 6;
        public const int PID_CREATE_DTM = 12;
        public const int PID_EDITTIME = 10;
        public const int PID_KEYWORDS = 5;
        public const int PID_LASTAUTHOR = 8;
        public const int PID_LASTPRINTED = 11;
        public const int PID_LASTSAVE_DTM = 13;
        public const int PID_PAGECOUNT = 14;
        public const int PID_REVNUMBER = 9;
        public const int PID_SECURITY = 0x13;
        public const int PID_SUBJECT = 3;
        public const int PID_TEMPLATE = 7;
        public const int PID_THUMBNAIL = 0x11;
        public const int PID_TITLE = 2;
        public const int PID_WORDCOUNT = 15;

        internal SummaryInformationSet(BinaryReader reader) : base(reader, FMTID_SummaryInformation)
        {
            object obj2;
            if (base.TryGetValue(1, out obj2))
            {
                try
                {
                    base.DefaultEncoding = Encoding.GetEncoding(Convert.ToInt32(obj2));
                }
                catch
                {
                }
            }
        }

        private T Get<T>(int propertyId, T defaultValue)
        {
            object obj2;
            if (base.TryGetValue(propertyId, out obj2))
            {
                return (T) obj2;
            }
            return defaultValue;
        }

        public string Author
        {
            get
            {
                return this.Get<string>(4, string.Empty);
            }
        }

        public int CharacterCount
        {
            get
            {
                return this.Get<int>(0x10, -1);
            }
        }

        public string Comments
        {
            get
            {
                return this.Get<string>(6, string.Empty);
            }
        }

        public string CreationApplicationName
        {
            get
            {
                return this.Get<string>(0x12, string.Empty);
            }
        }

        public DateTime CreationTime
        {
            get
            {
                return this.Get<DateTime>(12, DateTime.MinValue);
            }
        }

        public string Keywords
        {
            get
            {
                return this.Get<string>(5, string.Empty);
            }
        }

        public DateTime LastPrintedTime
        {
            get
            {
                return this.Get<DateTime>(11, DateTime.MinValue);
            }
        }

        public string LastSavedBy
        {
            get
            {
                return this.Get<string>(8, string.Empty);
            }
        }

        public DateTime LastSavedTime
        {
            get
            {
                return this.Get<DateTime>(13, DateTime.MinValue);
            }
        }

        public int PageCount
        {
            get
            {
                return this.Get<int>(14, -1);
            }
        }

        public string RevisionNumber
        {
            get
            {
                return this.Get<string>(9, string.Empty);
            }
        }

        public int Security
        {
            get
            {
                return this.Get<int>(0x13, -1);
            }
        }

        public string Subject
        {
            get
            {
                return this.Get<string>(3, string.Empty);
            }
        }

        public string Template
        {
            get
            {
                return this.Get<string>(7, string.Empty);
            }
        }

        public Image Thumbnail
        {
            get
            {
                return this.Get<Image>(0x11, null);
            }
        }

        public string Title
        {
            get
            {
                return this.Get<string>(2, string.Empty);
            }
        }

        public TimeSpan TotalEditingTime
        {
            get
            {
                DateTime defaultValue = new DateTime(0x641, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                return (this.Get<DateTime>(10, defaultValue) - defaultValue);
            }
        }

        public int WordCount
        {
            get
            {
                return this.Get<int>(15, -1);
            }
        }
    }
}

