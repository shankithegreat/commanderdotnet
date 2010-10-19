namespace TagLib
{
    using System;

    public sealed class ReadOnlyByteVector : ByteVector
    {
        public ReadOnlyByteVector()
        {
        }

        public ReadOnlyByteVector(int size) : this(size, 0)
        {
        }

        public ReadOnlyByteVector(ByteVector vector) : base(vector)
        {
        }

        public ReadOnlyByteVector(params byte[] data) : base(data)
        {
        }

        public ReadOnlyByteVector(int size, byte value) : base(size, value)
        {
        }

        public ReadOnlyByteVector(byte[] data, int length) : base(data, length)
        {
        }

        public static implicit operator ReadOnlyByteVector(byte value)
        {
            return new ReadOnlyByteVector(new byte[] { value });
        }

        public static implicit operator ReadOnlyByteVector(byte[] value)
        {
            return new ReadOnlyByteVector(value);
        }

        public static implicit operator ReadOnlyByteVector(string value)
        {
            return new ReadOnlyByteVector(ByteVector.FromString(value, StringType.UTF8));
        }

        public override bool IsFixedSize
        {
            get
            {
                return true;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return true;
            }
        }
    }
}

