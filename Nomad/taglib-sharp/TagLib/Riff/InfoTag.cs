namespace TagLib.Riff
{
    using System;
    using TagLib;

    public class InfoTag : ListTag
    {
        public InfoTag()
        {
        }

        public InfoTag(ByteVector data) : base(data)
        {
        }

        public InfoTag(TagLib.File file, long position, int length) : base(file, position, length)
        {
        }

        public override ByteVector RenderEnclosed()
        {
            return base.RenderEnclosed("INFO");
        }

        public override string[] AlbumArtists
        {
            get
            {
                return base.GetValuesAsStrings("IART");
            }
            set
            {
                base.SetValue("IART", value);
            }
        }

        public override string Comment
        {
            get
            {
                foreach (string str in base.GetValuesAsStrings("ICMT"))
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        return str;
                    }
                }
                return null;
            }
            set
            {
                string[] textArray1 = new string[] { value };
                base.SetValue("ICMT", textArray1);
            }
        }

        public override string[] Composers
        {
            get
            {
                return base.GetValuesAsStrings("IWRI");
            }
            set
            {
                base.SetValue("IWRI", value);
            }
        }

        public override string Copyright
        {
            get
            {
                foreach (string str in base.GetValuesAsStrings("ICOP"))
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        return str;
                    }
                }
                return null;
            }
            set
            {
                string[] textArray1 = new string[] { value };
                base.SetValue("ICOP", textArray1);
            }
        }

        public override string[] Genres
        {
            get
            {
                return base.GetValuesAsStrings("IGNR");
            }
            set
            {
                base.SetValue("IGNR", value);
            }
        }

        public override string[] Performers
        {
            get
            {
                return base.GetValuesAsStrings("ISTR");
            }
            set
            {
                base.SetValue("ISTR", value);
            }
        }

        public override TagLib.TagTypes TagTypes
        {
            get
            {
                return TagLib.TagTypes.RiffInfo;
            }
        }

        public override string Title
        {
            get
            {
                foreach (string str in base.GetValuesAsStrings("INAM"))
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        return str;
                    }
                }
                return null;
            }
            set
            {
                string[] textArray1 = new string[] { value };
                base.SetValue("INAM", textArray1);
            }
        }

        public override uint Track
        {
            get
            {
                return base.GetValueAsUInt("IPRT");
            }
            set
            {
                base.SetValue("IPRT", value);
            }
        }

        public override uint TrackCount
        {
            get
            {
                return base.GetValueAsUInt("IFRM");
            }
            set
            {
                base.SetValue("IFRM", value);
            }
        }

        public override uint Year
        {
            get
            {
                return base.GetValueAsUInt("ICRD");
            }
            set
            {
                base.SetValue("ICRD", value);
            }
        }
    }
}

