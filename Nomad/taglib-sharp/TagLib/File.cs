namespace TagLib
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public abstract class File : IDisposable
    {
        private static int buffer_size = 0x400;
        private IFileAbstraction file_abstraction;
        private Stream file_stream;
        private static List<FileTypeResolver> file_type_resolvers = new List<FileTypeResolver>();
        private long invariant_end_position;
        private long invariant_start_position;
        private string mime_type;
        private TagLib.TagTypes tags_on_disk;

        protected File(string path)
        {
            this.invariant_start_position = -1L;
            this.invariant_end_position = -1L;
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            this.file_abstraction = new LocalFileAbstraction(path);
        }

        protected File(IFileAbstraction abstraction)
        {
            this.invariant_start_position = -1L;
            this.invariant_end_position = -1L;
            if (abstraction == null)
            {
                throw new ArgumentNullException("abstraction");
            }
            this.file_abstraction = abstraction;
        }

        public static void AddFileTypeResolver(FileTypeResolver resolver)
        {
            if (resolver != null)
            {
                file_type_resolvers.Insert(0, resolver);
            }
        }

        public static TagLib.File Create(string path)
        {
            return Create(path, null, ReadStyle.Average);
        }

        public static TagLib.File Create(IFileAbstraction abstraction)
        {
            return Create(abstraction, null, ReadStyle.Average);
        }

        public static TagLib.File Create(string path, ReadStyle propertiesStyle)
        {
            return Create(path, null, propertiesStyle);
        }

        public static TagLib.File Create(IFileAbstraction abstraction, ReadStyle propertiesStyle)
        {
            return Create(abstraction, null, propertiesStyle);
        }

        public static TagLib.File Create(string path, string mimetype, ReadStyle propertiesStyle)
        {
            return Create(new LocalFileAbstraction(path), mimetype, propertiesStyle);
        }

        public static TagLib.File Create(IFileAbstraction abstraction, string mimetype, ReadStyle propertiesStyle)
        {
            TagLib.File file3;
            if (mimetype == null)
            {
                string str = string.Empty;
                int startIndex = abstraction.Name.LastIndexOf(".") + 1;
                if ((startIndex >= 1) && (startIndex < abstraction.Name.Length))
                {
                    str = abstraction.Name.Substring(startIndex, abstraction.Name.Length - startIndex);
                }
                mimetype = "taglib/" + str.ToLower(CultureInfo.InvariantCulture);
            }
            foreach (FileTypeResolver resolver in file_type_resolvers)
            {
                TagLib.File file = resolver(abstraction, mimetype, propertiesStyle);
                if (file != null)
                {
                    return file;
                }
            }
            if (!FileTypes.AvailableTypes.ContainsKey(mimetype))
            {
                object[] args = new object[] { abstraction.Name, mimetype };
                throw new UnsupportedFormatException(string.Format(CultureInfo.InvariantCulture, "{0} ({1})", args));
            }
            Type type = FileTypes.AvailableTypes[mimetype];
            try
            {
                object[] objArray2 = new object[] { abstraction, propertiesStyle };
                TagLib.File file2 = (TagLib.File) Activator.CreateInstance(type, objArray2);
                file2.MimeType = mimetype;
                file3 = file2;
            }
            catch (TargetInvocationException exception)
            {
                throw exception.InnerException;
            }
            return file3;
        }

        public void Dispose()
        {
            this.Mode = AccessMode.Closed;
        }

        public long Find(ByteVector pattern)
        {
            return this.Find(pattern, 0L);
        }

        public long Find(ByteVector pattern, long startPosition)
        {
            return this.Find(pattern, startPosition, null);
        }

        public long Find(ByteVector pattern, long startPosition, ByteVector before)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }
            this.Mode = AccessMode.Read;
            if (pattern.Count <= buffer_size)
            {
                long num = startPosition;
                int num2 = -1;
                int num3 = -1;
                long position = this.file_stream.Position;
                this.file_stream.Position = startPosition;
                for (ByteVector vector = this.ReadBlock(buffer_size); vector.Count > 0; vector = this.ReadBlock(buffer_size))
                {
                    if ((num2 >= 0) && (buffer_size > num2))
                    {
                        int patternOffset = buffer_size - num2;
                        if (vector.ContainsAt(pattern, 0, patternOffset))
                        {
                            this.file_stream.Position = position;
                            return ((num - buffer_size) + num2);
                        }
                    }
                    if (((before != null) && (num3 >= 0)) && (buffer_size > num3))
                    {
                        int num6 = buffer_size - num3;
                        if (vector.ContainsAt(before, 0, num6))
                        {
                            this.file_stream.Position = position;
                            return -1L;
                        }
                    }
                    long num7 = vector.Find(pattern);
                    if (num7 >= 0L)
                    {
                        this.file_stream.Position = position;
                        return (num + num7);
                    }
                    if ((before != null) && (vector.Find(before) >= 0))
                    {
                        this.file_stream.Position = position;
                        return -1L;
                    }
                    num2 = vector.EndsWithPartialMatch(pattern);
                    if (before != null)
                    {
                        num3 = vector.EndsWithPartialMatch(before);
                    }
                    num += buffer_size;
                }
                this.file_stream.Position = position;
            }
            return -1L;
        }

        public TagLib.Tag GetTag(TagLib.TagTypes type)
        {
            return this.GetTag(type, false);
        }

        public abstract TagLib.Tag GetTag(TagLib.TagTypes type, bool create);
        public void Insert(ByteVector data, long start)
        {
            this.Insert(data, start, 0L);
        }

        public void Insert(ByteVector data, long start, long replace)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            this.Mode = AccessMode.Write;
            if (data.Count == replace)
            {
                this.file_stream.Position = start;
                this.WriteBlock(data);
            }
            else if (data.Count < replace)
            {
                this.file_stream.Position = start;
                this.WriteBlock(data);
                this.RemoveBlock(start + data.Count, replace - data.Count);
            }
            else
            {
                int length = buffer_size;
                while ((data.Count - replace) > length)
                {
                    length += (int) BufferSize;
                }
                long num2 = start + replace;
                long num3 = start;
                this.file_stream.Position = num2;
                byte[] sourceArray = this.ReadBlock(length).Data;
                num2 += length;
                this.file_stream.Position = num3;
                this.WriteBlock(data);
                num3 += data.Count;
                byte[] destinationArray = new byte[sourceArray.Length];
                Array.Copy(sourceArray, 0, destinationArray, 0, sourceArray.Length);
                while (length != 0)
                {
                    this.file_stream.Position = num2;
                    int num4 = this.file_stream.Read(sourceArray, 0, (length >= sourceArray.Length) ? sourceArray.Length : length);
                    num2 += length;
                    this.file_stream.Position = num3;
                    this.file_stream.Write(destinationArray, 0, (length >= destinationArray.Length) ? destinationArray.Length : length);
                    num3 += length;
                    Array.Copy(sourceArray, 0, destinationArray, 0, num4);
                    length = num4;
                }
            }
        }

        public ByteVector ReadBlock(int length)
        {
            if (length < 0)
            {
                throw new ArgumentException("Length must be non-negative", "length");
            }
            if (length == 0)
            {
                return new ByteVector();
            }
            this.Mode = AccessMode.Read;
            byte[] data = new byte[length];
            return new ByteVector(data, this.file_stream.Read(data, 0, length));
        }

        public void RemoveBlock(long start, long length)
        {
            if (length > 0L)
            {
                this.Mode = AccessMode.Write;
                int num = buffer_size;
                long num2 = start + length;
                long num3 = start;
                ByteVector data = 1;
                while (data.Count != 0)
                {
                    this.file_stream.Position = num2;
                    data = this.ReadBlock(num);
                    num2 += data.Count;
                    this.file_stream.Position = num3;
                    this.WriteBlock(data);
                    num3 += data.Count;
                }
                this.Truncate(num3);
            }
        }

        public abstract void RemoveTags(TagLib.TagTypes types);
        public long RFind(ByteVector pattern)
        {
            return this.RFind(pattern, 0L);
        }

        public long RFind(ByteVector pattern, long startPosition)
        {
            return this.RFind(pattern, startPosition, null);
        }

        private long RFind(ByteVector pattern, long startPosition, ByteVector after)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }
            this.Mode = AccessMode.Read;
            if (pattern.Count <= buffer_size)
            {
                long position = this.file_stream.Position;
                if (startPosition == 0)
                {
                    this.Seek((long) (-1 * buffer_size), SeekOrigin.End);
                }
                else
                {
                    this.Seek(startPosition - (1 * buffer_size), SeekOrigin.Begin);
                }
                long num2 = this.file_stream.Position;
                for (ByteVector vector = this.ReadBlock(buffer_size); vector.Count > 0; vector = this.ReadBlock(buffer_size))
                {
                    long num3 = vector.RFind(pattern);
                    if (num3 >= 0L)
                    {
                        this.file_stream.Position = position;
                        return (num2 + num3);
                    }
                    if ((after != null) && (vector.RFind(after) >= 0))
                    {
                        this.file_stream.Position = position;
                        return -1L;
                    }
                    num2 -= buffer_size;
                    this.file_stream.Position = num2;
                }
                this.file_stream.Position = position;
            }
            return -1L;
        }

        public abstract void Save();
        public void Seek(long offset)
        {
            this.Seek(offset, SeekOrigin.Begin);
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            if (this.Mode != AccessMode.Closed)
            {
                this.file_stream.Seek(offset, origin);
            }
        }

        protected void Truncate(long length)
        {
            AccessMode mode = this.Mode;
            this.Mode = AccessMode.Write;
            this.file_stream.SetLength(length);
            this.Mode = mode;
        }

        public void WriteBlock(ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            this.Mode = AccessMode.Write;
            this.file_stream.Write(data.Data, 0, data.Count);
        }

        public static uint BufferSize
        {
            get
            {
                return (uint) buffer_size;
            }
        }

        public long InvariantEndPosition
        {
            get
            {
                return this.invariant_end_position;
            }
            protected set
            {
                this.invariant_end_position = value;
            }
        }

        public long InvariantStartPosition
        {
            get
            {
                return this.invariant_start_position;
            }
            protected set
            {
                this.invariant_start_position = value;
            }
        }

        public long Length
        {
            get
            {
                return ((this.Mode != AccessMode.Closed) ? this.file_stream.Length : 0L);
            }
        }

        public string MimeType
        {
            get
            {
                return this.mime_type;
            }
            internal set
            {
                this.mime_type = value;
            }
        }

        public AccessMode Mode
        {
            get
            {
                return ((this.file_stream != null) ? (!this.file_stream.CanWrite ? AccessMode.Read : AccessMode.Write) : AccessMode.Closed);
            }
            set
            {
                if ((this.Mode != value) && ((this.Mode != AccessMode.Write) || (value != AccessMode.Read)))
                {
                    if (this.file_stream != null)
                    {
                        this.file_abstraction.CloseStream(this.file_stream);
                    }
                    this.file_stream = null;
                    if (value == AccessMode.Read)
                    {
                        this.file_stream = this.file_abstraction.ReadStream;
                    }
                    else if (value == AccessMode.Write)
                    {
                        this.file_stream = this.file_abstraction.WriteStream;
                    }
                    this.Mode = value;
                }
            }
        }

        public string Name
        {
            get
            {
                return this.file_abstraction.Name;
            }
        }

        public abstract TagLib.Properties Properties { get; }

        public abstract TagLib.Tag Tag { get; }

        public TagLib.TagTypes TagTypes
        {
            get
            {
                return ((this.Tag == null) ? TagLib.TagTypes.None : this.Tag.TagTypes);
            }
        }

        public TagLib.TagTypes TagTypesOnDisk
        {
            get
            {
                return this.tags_on_disk;
            }
            protected set
            {
                this.tags_on_disk = value;
            }
        }

        public long Tell
        {
            get
            {
                return ((this.Mode != AccessMode.Closed) ? this.file_stream.Position : 0L);
            }
        }

        public enum AccessMode
        {
            Read,
            Write,
            Closed
        }

        public delegate TagLib.File FileTypeResolver(TagLib.File.IFileAbstraction abstraction, string mimetype, ReadStyle style);

        public interface IFileAbstraction
        {
            void CloseStream(Stream stream);

            string Name { get; }

            Stream ReadStream { get; }

            Stream WriteStream { get; }
        }

        public class LocalFileAbstraction : TagLib.File.IFileAbstraction
        {
            private string name;

            public LocalFileAbstraction(string path)
            {
                if (path == null)
                {
                    throw new ArgumentNullException("path");
                }
                this.name = path;
            }

            public void CloseStream(Stream stream)
            {
                if (stream == null)
                {
                    throw new ArgumentNullException("stream");
                }
                stream.Close();
            }

            public string Name
            {
                get
                {
                    return this.name;
                }
            }

            public Stream ReadStream
            {
                get
                {
                    return System.IO.File.Open(this.Name, FileMode.Open, FileAccess.Read, FileShare.Read);
                }
            }

            public Stream WriteStream
            {
                get
                {
                    return System.IO.File.Open(this.Name, FileMode.Open, FileAccess.ReadWrite);
                }
            }
        }
    }
}

