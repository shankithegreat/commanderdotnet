namespace TagLib.Ogg
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using TagLib;

    public class Paginator
    {
        private Codec codec;
        private PageHeader? first_page_header;
        private ByteVectorCollection packets = new ByteVectorCollection();
        private int pages_read;

        public Paginator(Codec codec)
        {
            this.codec = codec;
        }

        public void AddPage(Page page)
        {
            this.pages_read++;
            if (!this.first_page_header.HasValue)
            {
                this.first_page_header = new PageHeader?(page.Header);
            }
            if (page.Packets.Length != 0)
            {
                ByteVector[] packets = page.Packets;
                for (int i = 0; i < packets.Length; i++)
                {
                    if (((((byte) (page.Header.Flags & PageFlags.FirstPacketContinued)) != 0) && (i == 0)) && (this.packets.Count > 0))
                    {
                        this.packets[this.packets.Count - 1].Add(packets[0]);
                    }
                    else
                    {
                        this.packets.Add(packets[i]);
                    }
                }
            }
        }

        private static int GetLacingValueLength(ByteVectorCollection packets, int index)
        {
            int count = packets[index].Count;
            return ((count / 0xff) + ((((index + 1) >= packets.Count) && ((count % 0xff) <= 0)) ? 0 : 1));
        }

        [Obsolete("Use Paginator.Paginate(out int)")]
        public Page[] Paginate()
        {
            int num;
            return this.Paginate(out num);
        }

        public Page[] Paginate(out int change)
        {
            if (this.pages_read == 0)
            {
                change = 0;
                return new Page[0];
            }
            int num = this.pages_read;
            ByteVectorCollection packets = new ByteVectorCollection(this.packets);
            PageHeader header = this.first_page_header.Value;
            List<Page> list = new List<Page>();
            uint offset = 0;
            if (header.PageSequenceNumber == 0)
            {
                ByteVector[] vectorArray1 = new ByteVector[] { packets[0] };
                list.Add(new Page(new ByteVectorCollection(vectorArray1), header));
                offset++;
                packets.RemoveAt(0);
                num--;
            }
            int num3 = 0xfc;
            if (num > 0)
            {
                int num4 = 0;
                for (int i = 0; i < packets.Count; i++)
                {
                    num4 += GetLacingValueLength(packets, i);
                }
                num3 = Math.Min((num4 / num) + 1, num3);
            }
            int num6 = 0;
            ByteVectorCollection vectors2 = new ByteVectorCollection();
            bool flag2 = false;
            while (packets.Count > 0)
            {
                int lacingValueLength = GetLacingValueLength(packets, 0);
                int num8 = num3 - num6;
                bool flag3 = lacingValueLength <= num8;
                if (flag3)
                {
                    vectors2.Add(packets[0]);
                    num6 += lacingValueLength;
                    packets.RemoveAt(0);
                }
                else
                {
                    vectors2.Add(packets[0].Mid(0, num8 * 0xff));
                    packets[0] = packets[0].Mid(num8 * 0xff);
                    num6 += num8;
                }
                if (num6 == num3)
                {
                    list.Add(new Page(vectors2, new PageHeader(header, offset, !flag2 ? PageFlags.None : PageFlags.FirstPacketContinued)));
                    vectors2 = new ByteVectorCollection();
                    num6 = 0;
                    offset++;
                    num--;
                    flag2 = !flag3;
                }
            }
            if (vectors2.Count > 0)
            {
                list.Add(new Page(vectors2, new PageHeader(header.StreamSerialNumber, offset, !flag2 ? PageFlags.None : PageFlags.FirstPacketContinued)));
                offset++;
                num--;
            }
            change = -num;
            return list.ToArray();
        }

        public void SetComment(XiphComment comment)
        {
            this.codec.SetCommentPacket(this.packets, comment);
        }
    }
}

