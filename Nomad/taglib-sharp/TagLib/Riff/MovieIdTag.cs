namespace TagLib.Riff
{
    using System;
    using TagLib;

    public class MovieIdTag : ListTag
    {
        public MovieIdTag()
        {
        }

        public MovieIdTag(ByteVector data) : base(data)
        {
        }

        public MovieIdTag(TagLib.File file, long position, int length) : base(file, position, length)
        {
        }

        public override ByteVector RenderEnclosed()
        {
            return base.RenderEnclosed("MID ");
        }

        public override string Comment
        {
            get
            {
                foreach (string str in base.GetValuesAsStrings("COMM"))
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
                base.SetValue("COMM", textArray1);
            }
        }

        public override string[] Genres
        {
            get
            {
                return base.GetValuesAsStrings("GENR");
            }
            set
            {
                base.SetValue("GENR", value);
            }
        }

        public override string[] Performers
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

        public override TagLib.TagTypes TagTypes
        {
            get
            {
                return TagLib.TagTypes.MovieId;
            }
        }

        public override string Title
        {
            get
            {
                foreach (string str in base.GetValuesAsStrings("TITL"))
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
                base.SetValue("TITL", textArray1);
            }
        }

        public override uint Track
        {
            get
            {
                return base.GetValueAsUInt("PRT1");
            }
            set
            {
                base.SetValue("PRT1", value);
            }
        }

        public override uint TrackCount
        {
            get
            {
                return base.GetValueAsUInt("PRT2");
            }
            set
            {
                base.SetValue("PRT2", value);
            }
        }
    }
}

