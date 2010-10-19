namespace TagLib.Mpeg4
{
    using System;
    using TagLib;

    public static class BoxFactory
    {
        public static Box CreateBox(TagLib.File file, long position)
        {
            return CreateBox(file, position, null);
        }

        public static Box CreateBox(TagLib.File file, BoxHeader header)
        {
            return CreateBox(file, header, null);
        }

        public static Box CreateBox(TagLib.File file, long position, IsoHandlerBox handler)
        {
            return CreateBox(file, position, BoxHeader.Empty, handler, -1);
        }

        public static Box CreateBox(TagLib.File file, BoxHeader header, IsoHandlerBox handler)
        {
            return CreateBox(file, header, BoxHeader.Empty, handler, -1);
        }

        internal static Box CreateBox(TagLib.File file, long position, BoxHeader parent, IsoHandlerBox handler, int index)
        {
            BoxHeader header = new BoxHeader(file, position);
            return CreateBox(file, header, parent, handler, index);
        }

        private static Box CreateBox(TagLib.File file, BoxHeader header, BoxHeader parent, IsoHandlerBox handler, int index)
        {
            if (((parent.BoxType == BoxType.Stsd) && (parent.Box is IsoSampleDescriptionBox)) && (index < (parent.Box as IsoSampleDescriptionBox).EntryCount))
            {
                if ((handler != null) && (handler.HandlerType == BoxType.Soun))
                {
                    return new IsoAudioSampleEntry(header, file, handler);
                }
                if ((handler != null) && (handler.HandlerType == BoxType.Vide))
                {
                    return new IsoVisualSampleEntry(header, file, handler);
                }
                if ((handler != null) && (handler.HandlerType == BoxType.Alis))
                {
                    return new IsoAudioSampleEntry(header, file, handler);
                }
                return new IsoSampleEntry(header, file, handler);
            }
            ByteVector boxType = header.BoxType;
            if (boxType == BoxType.Mvhd)
            {
                return new IsoMovieHeaderBox(header, file, handler);
            }
            if (boxType == BoxType.Stbl)
            {
                return new IsoSampleTableBox(header, file, handler);
            }
            if (boxType == BoxType.Stsd)
            {
                return new IsoSampleDescriptionBox(header, file, handler);
            }
            if (boxType == BoxType.Stco)
            {
                return new IsoChunkOffsetBox(header, file, handler);
            }
            if (boxType == BoxType.Co64)
            {
                return new IsoChunkLargeOffsetBox(header, file, handler);
            }
            if (boxType == BoxType.Hdlr)
            {
                return new IsoHandlerBox(header, file, handler);
            }
            if (boxType == BoxType.Udta)
            {
                return new IsoUserDataBox(header, file, handler);
            }
            if (boxType == BoxType.Meta)
            {
                return new IsoMetaBox(header, file, handler);
            }
            if (boxType == BoxType.Ilst)
            {
                return new AppleItemListBox(header, file, handler);
            }
            if (boxType == BoxType.Data)
            {
                return new AppleDataBox(header, file, handler);
            }
            if (boxType == BoxType.Esds)
            {
                return new AppleElementaryStreamDescriptor(header, file, handler);
            }
            if ((boxType == BoxType.Free) || (boxType == BoxType.Skip))
            {
                return new IsoFreeSpaceBox(header, file, handler);
            }
            if ((boxType == BoxType.Mean) || (boxType == BoxType.Name))
            {
                return new AppleAdditionalInfoBox(header, file, handler);
            }
            if (parent.BoxType == BoxType.Ilst)
            {
                return new AppleAnnotationBox(header, file, handler);
            }
            return new UnknownBox(header, file, handler);
        }
    }
}

