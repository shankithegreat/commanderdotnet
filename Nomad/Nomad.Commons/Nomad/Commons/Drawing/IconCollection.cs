namespace Nomad.Commons.Drawing
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;

    public abstract class IconCollection : IEnumerable
    {
        protected IconCollection()
        {
        }

        public void Add(KeyValuePair<Size, Image> item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Contains(KeyValuePair<Size, Image> item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyTo(KeyValuePair<Size, Image>[] array, int arrayIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public static IDictionary<Size, Image> Create()
        {
            return new IconList();
        }

        protected static uint FromSize(ref Size key)
        {
            return (uint) ((key.Height << 0x10) | ((ushort) key.Width));
        }

        public bool Remove(KeyValuePair<Size, Image> item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected static Size ToSize(uint key)
        {
            return new Size((ushort) (key & 0xffff), (ushort) (key >> 0x10));
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
    }
}

