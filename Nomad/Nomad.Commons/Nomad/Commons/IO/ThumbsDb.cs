namespace Nomad.Commons.IO
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    public sealed class ThumbsDb : IDisposable
    {
        private Dictionary<string, int> CatalogMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private CompoundFile Thumbs;

        public ThumbsDb(string fileName)
        {
            this.Thumbs = new CompoundFile(fileName);
            CompoundStreamEntry entry = this.Thumbs.Root["catalog"] as CompoundStreamEntry;
            if (entry == null)
            {
                throw new ArgumentException("Not valid Thumbs.db file.");
            }
            using (Stream stream = entry.OpenRead())
            {
                this.LoadCatalog(stream);
            }
        }

        public void Dispose()
        {
            if (this.Thumbs != null)
            {
                this.Thumbs.Close();
            }
            this.Thumbs = null;
            this.CatalogMap = null;
        }

        private static string GetReversedItemId(int itemId)
        {
            char[] array = itemId.ToString().ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        public Image GetThumbnail(string name)
        {
            int num;
            Image image2;
            if (this.CatalogMap == null)
            {
                throw new ObjectDisposedException("ThumbsDb");
            }
            if (!this.CatalogMap.TryGetValue(name, out num))
            {
                return null;
            }
            CompoundStreamEntry entry = this.Thumbs.Root[GetReversedItemId(num)] as CompoundStreamEntry;
            if (entry == null)
            {
                return null;
            }
            using (Stream stream = entry.OpenRead())
            {
                stream.Seek(12L, SeekOrigin.Begin);
                using (Stream stream2 = new SubStream(stream))
                {
                    using (Image image = Image.FromStream(stream2))
                    {
                        image2 = new Bitmap(image);
                    }
                }
            }
            return image2;
        }

        private void LoadCatalog(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream, Encoding.Unicode))
            {
                reader.ReadInt16();
                reader.ReadInt16();
                int num = reader.ReadInt32();
                this.ThumbnailWidth = reader.ReadInt32();
                this.ThumbnailHeight = reader.ReadInt32();
                for (int i = 0; i < num; i++)
                {
                    char ch;
                    reader.ReadInt32();
                    int num3 = reader.ReadInt32();
                    reader.ReadInt16();
                    reader.ReadInt16();
                    reader.ReadInt16();
                    reader.ReadInt16();
                    StringBuilder builder = new StringBuilder();
                    while ((ch = reader.ReadChar()) != '\0')
                    {
                        builder.Append(ch);
                    }
                    reader.ReadInt16();
                    this.CatalogMap.Add(builder.ToString(), num3);
                }
            }
        }

        public int ThumbnailHeight { get; private set; }

        public Size ThumbnailSize
        {
            get
            {
                return new Size(this.ThumbnailWidth, this.ThumbnailHeight);
            }
        }

        public int ThumbnailWidth { get; private set; }
    }
}

