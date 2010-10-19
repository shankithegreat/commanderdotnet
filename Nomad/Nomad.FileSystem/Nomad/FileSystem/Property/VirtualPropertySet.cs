namespace Nomad.FileSystem.Property
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;

    [TypeConverter(typeof(VirtualPropertySetConverter))]
    public class VirtualPropertySet : IEnumerable<int>, IEnumerable<VirtualProperty>, IEnumerable
    {
        private static VirtualPropertySet FEmpty;
        private BitArray PropertySet;
        private bool ReadOnly;

        public VirtualPropertySet()
        {
            this.PropertySet = new BitArray(VirtualProperty.PropertyList.Count);
        }

        public VirtualPropertySet(VirtualPropertySet propertySet)
        {
            this.PropertySet = new BitArray(propertySet.PropertySet);
        }

        private VirtualPropertySet(BitArray propertySet)
        {
            this.PropertySet = propertySet;
        }

        public VirtualPropertySet(params int[] properties)
        {
            this.PropertySet = new BitArray(VirtualProperty.PropertyList.Count);
            foreach (int num in properties)
            {
                this[num] = true;
            }
        }

        public void And(VirtualPropertySet b)
        {
            if (this.ReadOnly)
            {
                throw new InvalidOperationException();
            }
            NormalizeBitArrays(this.PropertySet, b.PropertySet);
            this.PropertySet = this.PropertySet.And(b.PropertySet);
        }

        public VirtualPropertySet AsReadOnly()
        {
            return new VirtualPropertySet { PropertySet = new BitArray(this.PropertySet), ReadOnly = true };
        }

        public bool Equals(VirtualPropertySet other)
        {
            return ((other != null) && this.Equals(other.PropertySet));
        }

        private bool Equals(BitArray other)
        {
            NormalizeBitArrays(this.PropertySet, other);
            BitArray array = this.PropertySet.Xor(other);
            using (IEnumerator enumerator = array.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if ((bool) enumerator.Current)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool Equals(int propertyId)
        {
            return this.PropertySet[propertyId];
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new <GetEnumerator>d__1(0) { <>4__this = this };
        }

        public static bool Has(VirtualPropertySet a, int propertyId)
        {
            return ((a != null) && a[propertyId]);
        }

        private static void NormalizeBitArrays(BitArray x, BitArray y)
        {
            if (x.Count != y.Count)
            {
                int num = Math.Max(x.Count, y.Count);
                x.Length = num;
                y.Length = num;
            }
        }

        public void Not()
        {
            if (this.ReadOnly)
            {
                throw new InvalidOperationException();
            }
            this.PropertySet = this.PropertySet.Not();
        }

        public static VirtualPropertySet operator &(VirtualPropertySet a, VirtualPropertySet b)
        {
            if ((a == null) || (b == null))
            {
                return new VirtualPropertySet();
            }
            NormalizeBitArrays(a.PropertySet, b.PropertySet);
            return new VirtualPropertySet(a.PropertySet.And(b.PropertySet));
        }

        public static VirtualPropertySet operator |(VirtualPropertySet a, VirtualPropertySet b)
        {
            if ((a != null) && (b != null))
            {
                NormalizeBitArrays(a.PropertySet, b.PropertySet);
                return new VirtualPropertySet(a.PropertySet.Or(b.PropertySet));
            }
            if (a != null)
            {
                return new VirtualPropertySet(new BitArray(a.PropertySet));
            }
            if (b != null)
            {
                return new VirtualPropertySet(new BitArray(b.PropertySet));
            }
            return null;
        }

        public static VirtualPropertySet operator ^(VirtualPropertySet a, VirtualPropertySet b)
        {
            if ((a != null) && (b != null))
            {
                NormalizeBitArrays(a.PropertySet, b.PropertySet);
                return new VirtualPropertySet(a.PropertySet.Xor(b.PropertySet));
            }
            if (a != null)
            {
                return new VirtualPropertySet(new BitArray(a.PropertySet));
            }
            if (b != null)
            {
                return new VirtualPropertySet(new BitArray(b.PropertySet));
            }
            return null;
        }

        public static VirtualPropertySet operator ~(VirtualPropertySet a)
        {
            if (a == null)
            {
                return new VirtualPropertySet(new BitArray(VirtualProperty.PropertyList.Count, true));
            }
            return new VirtualPropertySet(a.PropertySet.Not());
        }

        public void Or(VirtualPropertySet b)
        {
            if (this.ReadOnly)
            {
                throw new InvalidOperationException();
            }
            NormalizeBitArrays(this.PropertySet, b.PropertySet);
            this.PropertySet = this.PropertySet.Or(b.PropertySet);
        }

        IEnumerator<VirtualProperty> IEnumerable<VirtualProperty>.GetEnumerator()
        {
            return new GetEnumerator>d__4(0) { <>4__this = this };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < this.PropertySet.Length; i++)
            {
                if (this.PropertySet[i])
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(" | ");
                    }
                    builder.Append(VirtualProperty.Get(i).PropertyName);
                }
            }
            return builder.ToString();
        }

        public void Xor(VirtualPropertySet b)
        {
            if (this.ReadOnly)
            {
                throw new InvalidOperationException();
            }
            NormalizeBitArrays(this.PropertySet, b.PropertySet);
            this.PropertySet = this.PropertySet.Xor(b.PropertySet);
        }

        public static VirtualPropertySet Empty
        {
            get
            {
                if (FEmpty == null)
                {
                }
                return (FEmpty = new VirtualPropertySet { ReadOnly = true });
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.ReadOnly;
            }
        }

        public bool this[int propertyId]
        {
            get
            {
                return this.PropertySet[propertyId];
            }
            set
            {
                if (this.ReadOnly)
                {
                    throw new InvalidOperationException();
                }
                if (propertyId < this.PropertySet.Count)
                {
                    this.PropertySet[propertyId] = value;
                }
                else
                {
                    if (propertyId >= VirtualProperty.PropertyList.Count)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    this.PropertySet.Length = VirtualProperty.PropertyList.Count;
                    this.PropertySet[propertyId] = value;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetEnumerator>d__1 : IEnumerator<int>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private int <>2__current;
            public VirtualPropertySet <>4__this;
            public int <I>5__2;

            [DebuggerHidden]
            public <GetEnumerator>d__1(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private bool MoveNext()
            {
                switch (this.<>1__state)
                {
                    case 0:
                        this.<>1__state = -1;
                        this.<I>5__2 = 0;
                        while (this.<I>5__2 < this.<>4__this.PropertySet.Length)
                        {
                            if (!this.<>4__this.PropertySet[this.<I>5__2])
                            {
                                goto Label_0069;
                            }
                            this.<>2__current = this.<I>5__2;
                            this.<>1__state = 1;
                            return true;
                        Label_0062:
                            this.<>1__state = -1;
                        Label_0069:
                            this.<I>5__2++;
                        }
                        break;

                    case 1:
                        goto Label_0062;
                }
                return false;
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
            }

            int IEnumerator<int>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class GetEnumerator>d__4 : IEnumerator<VirtualProperty>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private VirtualProperty <>2__current;
            public VirtualPropertySet <>4__this;
            public IEnumerator<int> <>7__wrap6;
            public int <NextProperty>5__5;

            [DebuggerHidden]
            public GetEnumerator>d__4(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private void <>m__Finally7()
            {
                this.<>1__state = -1;
                if (this.<>7__wrap6 != null)
                {
                    this.<>7__wrap6.Dispose();
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>7__wrap6 = this.<>4__this.GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrap6.MoveNext())
                            {
                                this.<NextProperty>5__5 = this.<>7__wrap6.Current;
                                this.<>2__current = VirtualProperty.Get(this.<NextProperty>5__5);
                                this.<>1__state = 2;
                                return true;
                            Label_0071:
                                this.<>1__state = 1;
                            }
                            this.<>m__Finally7();
                            break;

                        case 2:
                            goto Label_0071;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally7();
                        }
                        break;
                }
            }

            VirtualProperty IEnumerator<VirtualProperty>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }
    }
}

