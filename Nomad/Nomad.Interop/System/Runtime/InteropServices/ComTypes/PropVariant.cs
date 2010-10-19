namespace System.Runtime.InteropServices.ComTypes
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct PropVariant
    {
        private ushort vt;
        private ushort wReserved1;
        private ushort wReserved2;
        private ushort wReserved3;
        public IntPtr p;
        private int p2;
        public PropVariant(object value)
        {
            this.vt = 0;
            this.wReserved1 = this.wReserved2 = (ushort) (this.wReserved3 = 0);
            this.p = IntPtr.Zero;
            this.p2 = 0;
            this.Value = value;
        }

        private sbyte cVal
        {
            get
            {
                return (sbyte) this.GetDataBytes()[0];
            }
            set
            {
                this.SetDataBytes(new byte[] { (byte) value });
            }
        }
        private byte bVal
        {
            get
            {
                return this.GetDataBytes()[0];
            }
            set
            {
                this.SetDataBytes(new byte[] { value });
            }
        }
        private short iVal
        {
            get
            {
                return BitConverter.ToInt16(this.GetDataBytes(), 0);
            }
            set
            {
                this.SetDataBytes(BitConverter.GetBytes(value));
            }
        }
        private ushort uiVal
        {
            get
            {
                return BitConverter.ToUInt16(this.GetDataBytes(), 0);
            }
            set
            {
                this.SetDataBytes(BitConverter.GetBytes(value));
            }
        }
        private int lVal
        {
            get
            {
                return BitConverter.ToInt32(this.GetDataBytes(), 0);
            }
            set
            {
                this.SetDataBytes(BitConverter.GetBytes(value));
            }
        }
        private uint ulVal
        {
            get
            {
                return BitConverter.ToUInt32(this.GetDataBytes(), 0);
            }
            set
            {
                this.SetDataBytes(BitConverter.GetBytes(value));
            }
        }
        private long hVal
        {
            get
            {
                return BitConverter.ToInt64(this.GetDataBytes(), 0);
            }
            set
            {
                this.SetDataBytes(BitConverter.GetBytes(value));
            }
        }
        private ulong uhVal
        {
            get
            {
                return BitConverter.ToUInt64(this.GetDataBytes(), 0);
            }
            set
            {
                this.SetDataBytes(BitConverter.GetBytes(value));
            }
        }
        private float fltVal
        {
            get
            {
                return BitConverter.ToSingle(this.GetDataBytes(), 0);
            }
            set
            {
                this.SetDataBytes(BitConverter.GetBytes(value));
            }
        }
        private double dblVal
        {
            get
            {
                return BitConverter.ToDouble(this.GetDataBytes(), 0);
            }
            set
            {
                this.SetDataBytes(BitConverter.GetBytes(value));
            }
        }
        private bool boolVal
        {
            get
            {
                return (this.iVal != 0);
            }
            set
            {
                this.iVal = value ? ((short) 1) : ((short) 0);
            }
        }
        private int scode
        {
            get
            {
                return this.lVal;
            }
            set
            {
                this.lVal = value;
            }
        }
        private decimal cyVal
        {
            get
            {
                return decimal.FromOACurrency(this.hVal);
            }
            set
            {
                this.hVal = decimal.ToOACurrency(value);
            }
        }
        private DateTime date
        {
            get
            {
                return DateTime.FromOADate(this.dblVal);
            }
            set
            {
                this.dblVal = value.ToOADate();
            }
        }
        private byte[] GetDataBytes()
        {
            byte[] array = new byte[IntPtr.Size + 4];
            if (IntPtr.Size == 4)
            {
                BitConverter.GetBytes(this.p.ToInt32()).CopyTo(array, 0);
            }
            else if (IntPtr.Size == 8)
            {
                BitConverter.GetBytes(this.p.ToInt64()).CopyTo(array, 0);
            }
            BitConverter.GetBytes(this.p2).CopyTo(array, IntPtr.Size);
            return array;
        }

        private void SetDataBytes(byte[] value)
        {
            byte[] array = new byte[IntPtr.Size + 4];
            value.CopyTo(array, 0);
            if (IntPtr.Size == 4)
            {
                this.p = new IntPtr(BitConverter.ToInt32(array, 0));
            }
            else
            {
                this.p = new IntPtr(BitConverter.ToInt64(array, 0));
            }
            if (value.Length > IntPtr.Size)
            {
                this.p2 = BitConverter.ToInt32(array, IntPtr.Size);
            }
            else
            {
                this.p2 = 0;
            }
        }

        [DllImport("ole32.dll")]
        private static extern int PropVariantClear(ref PropVariant pvar);
        public void Clear()
        {
            switch (this.VarType)
            {
                case VarEnum.VT_EMPTY:
                    return;

                case VarEnum.VT_NULL:
                case VarEnum.VT_I2:
                case VarEnum.VT_I4:
                case VarEnum.VT_R4:
                case VarEnum.VT_R8:
                case VarEnum.VT_CY:
                case VarEnum.VT_DATE:
                case VarEnum.VT_BOOL:
                case VarEnum.VT_I1:
                case VarEnum.VT_UI1:
                case VarEnum.VT_UI2:
                case VarEnum.VT_UI4:
                case VarEnum.VT_I8:
                case VarEnum.VT_UI8:
                case VarEnum.VT_INT:
                case VarEnum.VT_UINT:
                case VarEnum.VT_HRESULT:
                case VarEnum.VT_FILETIME:
                    break;

                case VarEnum.VT_BSTR:
                    Marshal.FreeBSTR(this.p);
                    break;

                default:
                    PropVariantClear(ref this);
                    return;
            }
            this.vt = 0;
            this.wReserved1 = this.wReserved2 = (ushort) (this.wReserved3 = 0);
            this.p = IntPtr.Zero;
            this.p2 = 0;
        }

        public VarEnum VarType
        {
            get
            {
                return (VarEnum) this.vt;
            }
        }
        public object Value
        {
            get
            {
                object objectForNativeVariant;
                switch (this.VarType)
                {
                    case VarEnum.VT_LPSTR:
                        return Marshal.PtrToStringAnsi(this.p);

                    case VarEnum.VT_LPWSTR:
                        return Marshal.PtrToStringUni(this.p);

                    case VarEnum.VT_FILETIME:
                        return DateTime.FromFileTime(this.hVal);

                    case VarEnum.VT_UNKNOWN:
                        return Marshal.GetObjectForIUnknown(this.p);

                    case VarEnum.VT_EMPTY:
                        return null;

                    case VarEnum.VT_NULL:
                        return DBNull.Value;

                    case VarEnum.VT_BSTR:
                        return Marshal.PtrToStringBSTR(this.p);

                    case VarEnum.VT_DISPATCH:
                        return this.p;
                }
                GCHandle handle = GCHandle.Alloc(this, GCHandleType.Pinned);
                try
                {
                    objectForNativeVariant = Marshal.GetObjectForNativeVariant(handle.AddrOfPinnedObject());
                }
                finally
                {
                    handle.Free();
                }
                return objectForNativeVariant;
            }
            set
            {
                this.Clear();
                if (value == null)
                {
                    this.vt = 0;
                }
                else
                {
                    switch (Type.GetTypeCode(value.GetType()))
                    {
                        case TypeCode.DBNull:
                            this.vt = 1;
                            return;

                        case TypeCode.Boolean:
                            this.iVal = Convert.ToInt16(value);
                            this.vt = 11;
                            return;

                        case TypeCode.SByte:
                            this.cVal = (sbyte) value;
                            this.vt = 0x10;
                            return;

                        case TypeCode.Byte:
                            this.bVal = (byte) value;
                            this.vt = 0x11;
                            return;

                        case TypeCode.Int16:
                            this.iVal = (short) value;
                            this.vt = 2;
                            return;

                        case TypeCode.UInt16:
                            this.uiVal = (ushort) value;
                            this.vt = 0x12;
                            return;

                        case TypeCode.Int32:
                            this.lVal = (int) value;
                            this.vt = 3;
                            return;

                        case TypeCode.UInt32:
                            this.ulVal = (uint) value;
                            this.vt = 0x13;
                            return;

                        case TypeCode.Int64:
                            this.hVal = (long) value;
                            this.vt = 20;
                            return;

                        case TypeCode.UInt64:
                            this.uhVal = (ulong) value;
                            this.vt = 0x15;
                            return;

                        case TypeCode.Single:
                            this.fltVal = (float) value;
                            this.vt = 4;
                            return;

                        case TypeCode.Double:
                            this.dblVal = (double) value;
                            this.vt = 5;
                            return;

                        case TypeCode.Decimal:
                            this.cyVal = (decimal) value;
                            this.vt = 6;
                            return;

                        case TypeCode.DateTime:
                            this.date = (DateTime) value;
                            this.vt = 7;
                            return;

                        case TypeCode.String:
                            this.p = Marshal.StringToBSTR((string) value);
                            this.vt = 8;
                            return;
                    }
                    throw new NotSupportedException();
                }
            }
        }
        public bool IsEmpty
        {
            get
            {
                return (this.VarType == VarEnum.VT_EMPTY);
            }
        }
        public bool IsNullOrEmpty
        {
            get
            {
                return ((this.VarType == VarEnum.VT_EMPTY) || (this.VarType == VarEnum.VT_NULL));
            }
        }
    }
}

