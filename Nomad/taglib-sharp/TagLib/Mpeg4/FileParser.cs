namespace TagLib.Mpeg4
{
    using System;
    using System.Collections.Generic;
    using TagLib;

    public class FileParser
    {
        private TagLib.File file;
        private BoxHeader first_header;
        private long mdat_end = -1L;
        private long mdat_start = -1L;
        private BoxHeader[] moov_tree;
        private IsoMovieHeaderBox mvhd_box;
        private List<Box> stco_boxes = new List<Box>();
        private List<Box> stsd_boxes = new List<Box>();
        private IsoUserDataBox udta_box;
        private BoxHeader[] udta_tree;

        public FileParser(TagLib.File file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            this.file = file;
            this.first_header = new BoxHeader(file, 0L);
            if (this.first_header.BoxType != "ftyp")
            {
                throw new CorruptFileException("File does not start with 'ftyp' box.");
            }
        }

        private static List<BoxHeader> AddParent(List<BoxHeader> parents, BoxHeader current)
        {
            List<BoxHeader> list = new List<BoxHeader>();
            if (parents != null)
            {
                list.AddRange(parents);
            }
            list.Add(current);
            return list;
        }

        public void ParseBoxHeaders()
        {
            this.ResetFields();
            this.ParseBoxHeaders(this.first_header.TotalBoxSize, this.file.Length, null);
        }

        private void ParseBoxHeaders(long start, long end, List<BoxHeader> parents)
        {
            BoxHeader header;
            for (long i = start; i < end; i += header.TotalBoxSize)
            {
                header = new BoxHeader(this.file, i);
                if ((this.moov_tree == null) && (header.BoxType == BoxType.Moov))
                {
                    List<BoxHeader> list = AddParent(parents, header);
                    this.moov_tree = list.ToArray();
                    this.ParseBoxHeaders(header.HeaderSize + i, header.TotalBoxSize + i, list);
                }
                else if (((header.BoxType == BoxType.Mdia) || (header.BoxType == BoxType.Minf)) || ((header.BoxType == BoxType.Stbl) || (header.BoxType == BoxType.Trak)))
                {
                    this.ParseBoxHeaders(header.HeaderSize + i, header.TotalBoxSize + i, AddParent(parents, header));
                }
                else if ((this.udta_tree == null) && (header.BoxType == BoxType.Udta))
                {
                    this.udta_tree = AddParent(parents, header).ToArray();
                }
                else if (header.BoxType == BoxType.Mdat)
                {
                    this.mdat_start = i;
                    this.mdat_end = i + header.TotalBoxSize;
                }
                if (header.TotalBoxSize == 0)
                {
                    break;
                }
            }
        }

        public void ParseChunkOffsets()
        {
            this.ResetFields();
            this.ParseChunkOffsets(this.first_header.TotalBoxSize, this.file.Length);
        }

        private void ParseChunkOffsets(long start, long end)
        {
            BoxHeader header;
            for (long i = start; i < end; i += header.TotalBoxSize)
            {
                header = new BoxHeader(this.file, i);
                if (header.BoxType == BoxType.Moov)
                {
                    this.ParseChunkOffsets(header.HeaderSize + i, header.TotalBoxSize + i);
                }
                else if (((header.BoxType == BoxType.Moov) || (header.BoxType == BoxType.Mdia)) || (((header.BoxType == BoxType.Minf) || (header.BoxType == BoxType.Stbl)) || (header.BoxType == BoxType.Trak)))
                {
                    this.ParseChunkOffsets(header.HeaderSize + i, header.TotalBoxSize + i);
                }
                else if ((header.BoxType == BoxType.Stco) || (header.BoxType == BoxType.Co64))
                {
                    this.stco_boxes.Add(BoxFactory.CreateBox(this.file, header));
                }
                else if (header.BoxType == BoxType.Mdat)
                {
                    this.mdat_start = i;
                    this.mdat_end = i + header.TotalBoxSize;
                }
                if (header.TotalBoxSize == 0)
                {
                    break;
                }
            }
        }

        public void ParseTag()
        {
            this.ResetFields();
            this.ParseTag(this.first_header.TotalBoxSize, this.file.Length);
        }

        private void ParseTag(long start, long end)
        {
            BoxHeader header;
            for (long i = start; i < end; i += header.TotalBoxSize)
            {
                header = new BoxHeader(this.file, i);
                if (((header.BoxType == BoxType.Moov) || (header.BoxType == BoxType.Mdia)) || (((header.BoxType == BoxType.Minf) || (header.BoxType == BoxType.Stbl)) || (header.BoxType == BoxType.Trak)))
                {
                    this.ParseTag(header.HeaderSize + i, header.TotalBoxSize + i);
                }
                else if ((this.udta_box == null) && (header.BoxType == BoxType.Udta))
                {
                    this.udta_box = BoxFactory.CreateBox(this.file, header) as IsoUserDataBox;
                }
                else if (header.BoxType == BoxType.Mdat)
                {
                    this.mdat_start = i;
                    this.mdat_end = i + header.TotalBoxSize;
                }
                if (header.TotalBoxSize == 0)
                {
                    break;
                }
            }
        }

        public void ParseTagAndProperties()
        {
            this.ResetFields();
            this.ParseTagAndProperties(this.first_header.TotalBoxSize, this.file.Length, null);
        }

        private void ParseTagAndProperties(long start, long end, IsoHandlerBox handler)
        {
            BoxHeader header;
            for (long i = start; i < end; i += header.TotalBoxSize)
            {
                header = new BoxHeader(this.file, i);
                ByteVector boxType = header.BoxType;
                if (((boxType == BoxType.Moov) || (boxType == BoxType.Mdia)) || (((boxType == BoxType.Minf) || (boxType == BoxType.Stbl)) || (boxType == BoxType.Trak)))
                {
                    this.ParseTagAndProperties(header.HeaderSize + i, header.TotalBoxSize + i, handler);
                }
                else if (boxType == BoxType.Stsd)
                {
                    this.stsd_boxes.Add(BoxFactory.CreateBox(this.file, header, handler));
                }
                else if (boxType == BoxType.Hdlr)
                {
                    handler = BoxFactory.CreateBox(this.file, header, handler) as IsoHandlerBox;
                }
                else if ((this.mvhd_box == null) && (boxType == BoxType.Mvhd))
                {
                    this.mvhd_box = BoxFactory.CreateBox(this.file, header, handler) as IsoMovieHeaderBox;
                }
                else if ((this.udta_box == null) && (boxType == BoxType.Udta))
                {
                    this.udta_box = BoxFactory.CreateBox(this.file, header, handler) as IsoUserDataBox;
                }
                else if (boxType == BoxType.Mdat)
                {
                    this.mdat_start = i;
                    this.mdat_end = i + header.TotalBoxSize;
                }
                if (header.TotalBoxSize == 0)
                {
                    break;
                }
            }
        }

        private void ResetFields()
        {
            this.mvhd_box = null;
            this.udta_box = null;
            this.moov_tree = null;
            this.udta_tree = null;
            this.stco_boxes.Clear();
            this.stsd_boxes.Clear();
            this.mdat_start = -1L;
            this.mdat_end = -1L;
        }

        public IsoAudioSampleEntry AudioSampleEntry
        {
            get
            {
                foreach (IsoSampleDescriptionBox box in this.stsd_boxes)
                {
                    IEnumerator<Box> enumerator = box.Children.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            Box current = enumerator.Current;
                            IsoAudioSampleEntry entry = current as IsoAudioSampleEntry;
                            if (entry != null)
                            {
                                return entry;
                            }
                        }
                        continue;
                    }
                    finally
                    {
                        if (enumerator == null)
                        {
                        }
                        enumerator.Dispose();
                    }
                }
                return null;
            }
        }

        public Box[] ChunkOffsetBoxes
        {
            get
            {
                return this.stco_boxes.ToArray();
            }
        }

        public long MdatEndPosition
        {
            get
            {
                return this.mdat_end;
            }
        }

        public long MdatStartPosition
        {
            get
            {
                return this.mdat_start;
            }
        }

        public BoxHeader[] MoovTree
        {
            get
            {
                return this.moov_tree;
            }
        }

        public IsoMovieHeaderBox MovieHeaderBox
        {
            get
            {
                return this.mvhd_box;
            }
        }

        public BoxHeader[] UdtaTree
        {
            get
            {
                return this.udta_tree;
            }
        }

        public IsoUserDataBox UserDataBox
        {
            get
            {
                return this.udta_box;
            }
        }

        public IsoVisualSampleEntry VisualSampleEntry
        {
            get
            {
                foreach (IsoSampleDescriptionBox box in this.stsd_boxes)
                {
                    IEnumerator<Box> enumerator = box.Children.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            Box current = enumerator.Current;
                            IsoVisualSampleEntry entry = current as IsoVisualSampleEntry;
                            if (entry != null)
                            {
                                return entry;
                            }
                        }
                        continue;
                    }
                    finally
                    {
                        if (enumerator == null)
                        {
                        }
                        enumerator.Dispose();
                    }
                }
                return null;
            }
        }
    }
}

