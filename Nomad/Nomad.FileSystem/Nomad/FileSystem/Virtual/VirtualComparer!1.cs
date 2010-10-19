namespace Nomad.FileSystem.Virtual
{
    using Nomad.Commons;
    using Nomad.FileSystem.Property;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable]
    public class VirtualComparer<T> : Comparer<T> where T: IVirtualItem
    {
        public readonly int ComparePropertyId;
        [NonSerialized]
        private IComparer<string> FNameComparer;
        public readonly Nomad.FileSystem.Virtual.NameComparison NameComparison;
        public readonly ListSortDirection SortDirection;

        public VirtualComparer(VirtualComparer<T> comparer, Nomad.FileSystem.Virtual.NameComparison comparison)
        {
            this.ComparePropertyId = comparer.ComparePropertyId;
            this.SortDirection = comparer.SortDirection;
            this.NameComparison = comparison;
        }

        public VirtualComparer(VirtualComparer<T> comparer, ListSortDirection direction)
        {
            this.ComparePropertyId = comparer.ComparePropertyId;
            this.SortDirection = direction;
            this.NameComparison = comparer.NameComparison;
        }

        public VirtualComparer(VirtualComparer<T> comparer, int propertyId)
        {
            this.ComparePropertyId = propertyId;
            this.SortDirection = comparer.SortDirection;
            this.NameComparison = comparer.NameComparison;
        }

        public VirtualComparer(int propertyId, ListSortDirection direction) : this(propertyId, direction, Nomad.FileSystem.Virtual.NameComparison.Natural | Nomad.FileSystem.Virtual.NameComparison.Alphabet)
        {
        }

        public VirtualComparer(int propertyId, ListSortDirection direction, Nomad.FileSystem.Virtual.NameComparison comparison)
        {
            this.ComparePropertyId = propertyId;
            this.SortDirection = direction;
            this.NameComparison = comparison;
        }

        public override int Compare(T x, T y)
        {
            int num = 0;
            bool flag = x is IVirtualFolder;
            bool flag2 = y is IVirtualFolder;
            if (!(!flag || flag2))
            {
                return -1;
            }
            if (!(flag || !flag2))
            {
                return 1;
            }
            if (this.ComparePropertyId == 0)
            {
                num = this.CompareNames(x, y);
                goto Label_0341;
            }
            object a = null;
            object obj3 = null;
            bool flag3 = (x.GetPropertyAvailability(this.ComparePropertyId) == PropertyAvailability.Normal) && ((a = x[this.ComparePropertyId]) != null);
            bool flag4 = (y.GetPropertyAvailability(this.ComparePropertyId) == PropertyAvailability.Normal) && ((obj3 = y[this.ComparePropertyId]) != null);
            if (flag3 && flag4)
            {
                try
                {
                    switch (Type.GetTypeCode(a.GetType()))
                    {
                        case TypeCode.Boolean:
                            num = ((bool) a).CompareTo(Convert.ToBoolean(obj3));
                            goto Label_0322;

                        case TypeCode.Char:
                            num = ((char) a).CompareTo(Convert.ToChar(obj3));
                            goto Label_0322;

                        case TypeCode.SByte:
                            num = ((sbyte) a).CompareTo(Convert.ToSByte(obj3));
                            goto Label_0322;

                        case TypeCode.Byte:
                            num = ((byte) a).CompareTo(Convert.ToByte(obj3));
                            goto Label_0322;

                        case TypeCode.Int16:
                            num = ((short) a).CompareTo(Convert.ToInt16(obj3));
                            goto Label_0322;

                        case TypeCode.UInt16:
                            num = ((ushort) a).CompareTo(Convert.ToUInt16(obj3));
                            goto Label_0322;

                        case TypeCode.Int32:
                            num = ((int) a).CompareTo(Convert.ToInt32(obj3));
                            goto Label_0322;

                        case TypeCode.UInt32:
                            num = ((uint) a).CompareTo(Convert.ToUInt32(obj3));
                            goto Label_0322;

                        case TypeCode.Int64:
                            num = ((long) a).CompareTo(Convert.ToInt64(obj3));
                            goto Label_0322;

                        case TypeCode.UInt64:
                            num = ((ulong) a).CompareTo(Convert.ToUInt64(obj3));
                            goto Label_0322;

                        case TypeCode.Single:
                            num = ((float) a).CompareTo(Convert.ToSingle(obj3));
                            goto Label_0322;

                        case TypeCode.Double:
                            num = ((double) a).CompareTo(Convert.ToDouble(obj3));
                            goto Label_0322;

                        case TypeCode.Decimal:
                            num = ((decimal) a).CompareTo(Convert.ToDecimal(obj3));
                            goto Label_0322;

                        case TypeCode.DateTime:
                            num = ((DateTime) a).CompareTo(Convert.ToDateTime(obj3));
                            goto Label_0322;

                        case TypeCode.String:
                            num = string.Compare((string) a, Convert.ToString(obj3), true);
                            goto Label_0322;
                    }
                    num = CaseInsensitiveComparer.DefaultInvariant.Compare(a, obj3);
                }
                catch (ArgumentException)
                {
                }
                catch (InvalidCastException)
                {
                }
            }
            else if (flag3)
            {
                num = -1;
            }
            else if (flag4)
            {
                num = 1;
            }
        Label_0322:
            if ((num == 0) && (this.ComparePropertyId != 0))
            {
                num = this.CompareNames(x, y);
            }
        Label_0341:
            if (this.SortDirection == ListSortDirection.Descending)
            {
                return -num;
            }
            return num;
        }

        private int CompareNames(T x, T y)
        {
            if (this.FNameComparer == null)
            {
                this.InitializeNameComparer();
            }
            return this.FNameComparer.Compare(x.Name, y.Name);
        }

        public override bool Equals(object obj)
        {
            VirtualItemComparer comparer = obj as VirtualItemComparer;
            return ((((comparer != null) && (comparer.ComparePropertyId == this.ComparePropertyId)) && (comparer.SortDirection == this.SortDirection)) && (comparer.NameComparison == this.NameComparison));
        }

        public override int GetHashCode()
        {
            return ((this.ComparePropertyId.GetHashCode() ^ this.SortDirection.GetHashCode()) ^ this.NameComparison.GetHashCode());
        }

        private void InitializeNameComparer()
        {
            bool flag = (this.NameComparison & Nomad.FileSystem.Virtual.NameComparison.Alphabet) > Nomad.FileSystem.Virtual.NameComparison.Default;
            bool flag2 = (this.NameComparison & Nomad.FileSystem.Virtual.NameComparison.Natural) > Nomad.FileSystem.Virtual.NameComparison.Default;
            if (flag && flag2)
            {
                this.FNameComparer = new NaturalStringComparer(StringComparison.InvariantCultureIgnoreCase);
            }
            else if (flag)
            {
                this.FNameComparer = StringComparer.InvariantCultureIgnoreCase;
            }
            else if (flag2)
            {
                this.FNameComparer = new NaturalStringComparer(StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                this.FNameComparer = StringComparer.OrdinalIgnoreCase;
            }
        }
    }
}

