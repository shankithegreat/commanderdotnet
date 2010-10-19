namespace TagLib.Id3v1
{
    using System;
    using TagLib;

    public class StringHandler
    {
        public virtual string Parse(ByteVector data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            string str = data.ToString(StringType.Latin1).Trim();
            int index = str.IndexOf('\0');
            return ((index < 0) ? str : str.Substring(0, index));
        }

        public virtual ByteVector Render(string text)
        {
            return ByteVector.FromString(text, StringType.Latin1);
        }
    }
}

