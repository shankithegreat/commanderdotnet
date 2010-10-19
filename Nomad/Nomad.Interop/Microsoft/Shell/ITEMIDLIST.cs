namespace Microsoft.Shell
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct ITEMIDLIST
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=1)]
        public SHITEMID[] mkid;
        private ushort Null;
        public bool IsEmpty
        {
            get
            {
                return ((this.mkid == null) || (this.mkid.Length == 0));
            }
        }
        public int Size
        {
            get
            {
                return this.GetSize();
            }
        }
        public void Append(params SHITEMID[] ItemIdList)
        {
            List<SHITEMID> list = new List<SHITEMID>();
            if (this.mkid != null)
            {
                list.AddRange(this.mkid);
            }
            list.AddRange(ItemIdList);
            this.mkid = list.ToArray();
        }

        public void Append(ITEMIDLIST ItemId)
        {
            if (ItemId.mkid != null)
            {
                this.Append(ItemId.mkid);
            }
        }

        public void Append(IntPtr pidl)
        {
            this.Append(FromPidl(pidl));
        }

        private static bool CompareMem(IntPtr mem1, IntPtr mem2, int size)
        {
            for (int i = 0; i < size; i++)
            {
                if (Marshal.ReadByte(mem1, i) != Marshal.ReadByte(mem2, i))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool Equals(byte[] value1, byte[] value2)
        {
            if ((value1 != null) || (value2 != null))
            {
                if (((value1 == null) || (value2 == null)) || (value1.Length != value2.Length))
                {
                    return false;
                }
                for (int i = 0; i < value1.Length; i++)
                {
                    if (value1[i] != value2[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool Equals(ITEMIDLIST other)
        {
            bool flag = this.mkid.Length == other.mkid.Length;
            if (flag)
            {
                for (int i = 0; (i < this.mkid.Length) && flag; i++)
                {
                    flag = Equals(this.mkid[i].abID, other.mkid[i].abID);
                }
            }
            return flag;
        }

        public static bool Equals(IntPtr pidl1, IntPtr pidl2)
        {
            int size = GetSize(pidl1);
            if (size != GetSize(pidl2))
            {
                return false;
            }
            return CompareMem(pidl1, pidl2, size);
        }

        public static bool Equals(IntPtr pidl1, IntPtr pidl2, int depth)
        {
            if (depth < 1)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (GetDepth(pidl1) < depth)
            {
                return false;
            }
            if (GetDepth(pidl2) < depth)
            {
                return false;
            }
            int size = GetSize(pidl1, depth);
            if (size != GetSize(pidl2, depth))
            {
                return false;
            }
            return CompareMem(pidl1, pidl2, size - Marshal.SizeOf(typeof(ushort)));
        }

        public static int GetDepth(IntPtr pidl)
        {
            int num = 0;
            int ofs = 0;
            for (ushort i = (ushort) Marshal.ReadInt16(pidl); i > 0; i = (ushort) Marshal.ReadInt16(pidl, ofs))
            {
                num++;
                ofs += i;
            }
            return num;
        }

        public static IntPtr GetLastPidl(IntPtr pidl)
        {
            ushort num3;
            int num = 0;
            for (ushort i = (ushort) Marshal.ReadInt16(pidl); i > 0; i = num3)
            {
                num3 = (ushort) Marshal.ReadInt16(pidl, num + i);
                if (num3 == 0)
                {
                    return new IntPtr(pidl.ToInt64() + num);
                }
                num += i;
            }
            return IntPtr.Zero;
        }

        public int GetSize()
        {
            return this.GetSize(this.mkid.Length);
        }

        public int GetSize(int depth)
        {
            if (depth < 1)
            {
                throw new ArgumentOutOfRangeException();
            }
            int num = Marshal.SizeOf(typeof(ushort));
            for (int i = 0; i < depth; i++)
            {
                num += this.mkid[i].cb;
            }
            return num;
        }

        public static int GetSize(IntPtr pidl)
        {
            int ofs = 0;
            for (ushort i = (ushort) Marshal.ReadInt16(pidl); i > 0; i = (ushort) Marshal.ReadInt16(pidl, ofs))
            {
                ofs += i;
            }
            return (ofs + Marshal.SizeOf(typeof(ushort)));
        }

        public static int GetSize(IntPtr pidl, int depth)
        {
            if (depth < 1)
            {
                throw new ArgumentOutOfRangeException();
            }
            int ofs = 0;
            for (ushort i = (ushort) Marshal.ReadInt16(pidl); (depth > 0) && (i > 0); i = (ushort) Marshal.ReadInt16(pidl, ofs))
            {
                ofs += i;
                depth--;
            }
            return ((depth == 0) ? (ofs + Marshal.SizeOf(typeof(ushort))) : -1);
        }

        public static ITEMIDLIST FromByteArray(byte[] pidl)
        {
            List<SHITEMID> list = new List<SHITEMID>();
            int startIndex = 0;
            for (ushort i = BitConverter.ToUInt16(pidl, startIndex); i > 0; i = BitConverter.ToUInt16(pidl, startIndex))
            {
                SHITEMID shitemid;
                shitemid = new SHITEMID {
                    cb = i,
                    abID = new byte[i - Marshal.SizeOf(shitemid.cb)]
                };
                Array.Copy(pidl, startIndex + Marshal.SizeOf(shitemid.cb), shitemid.abID, 0, shitemid.abID.Length);
                list.Add(shitemid);
                startIndex += i;
            }
            return new ITEMIDLIST { mkid = list.ToArray() };
        }

        public static ITEMIDLIST FromPidl(IntPtr pidl)
        {
            List<SHITEMID> list = new List<SHITEMID>();
            long num = pidl.ToInt64();
            int ofs = 0;
            for (ushort i = (ushort) Marshal.ReadInt16(pidl); i > 0; i = (ushort) Marshal.ReadInt16(pidl, ofs))
            {
                SHITEMID shitemid;
                shitemid = new SHITEMID {
                    cb = i,
                    abID = new byte[i - Marshal.SizeOf(shitemid.cb)]
                };
                Marshal.Copy(new IntPtr((num + ofs) + Marshal.SizeOf(shitemid.cb)), shitemid.abID, 0, shitemid.abID.Length);
                list.Add(shitemid);
                ofs += i;
            }
            return new ITEMIDLIST { mkid = list.ToArray() };
        }

        public byte[] ToByteArray()
        {
            return this.ToByteArray((this.mkid != null) ? this.mkid.Length : 0);
        }

        public byte[] ToByteArray(int depth)
        {
            byte[] array = new byte[this.GetSize(depth)];
            int index = 0;
            for (int i = 0; i < depth; i++)
            {
                byte[] bytes = BitConverter.GetBytes((short) this.mkid[i].cb);
                bytes.CopyTo(array, index);
                this.mkid[i].abID.CopyTo(array, (int) (index + bytes.Length));
                index += this.mkid[i].cb;
            }
            array[index] = 0;
            array[index + 1] = 0;
            return array;
        }

        public void ToPidl(IntPtr pidl)
        {
            this.ToPidl(pidl, (this.mkid != null) ? this.mkid.Length : 0);
        }

        public void ToPidl(IntPtr pidl, int depth)
        {
            long num = pidl.ToInt64();
            int ofs = 0;
            for (int i = 0; i < depth; i++)
            {
                Marshal.WriteInt16(pidl, ofs, (short) this.mkid[i].cb);
                Marshal.Copy(this.mkid[i].abID, 0, new IntPtr((num + ofs) + Marshal.SizeOf(this.mkid[i].cb)), this.mkid[i].abID.Length);
                ofs += this.mkid[i].cb;
            }
            Marshal.WriteInt16(pidl, ofs, (short) 0);
        }
    }
}

