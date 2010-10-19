namespace Nomad.Configuration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class ListViewColumnCollection : CollectionBase, IEnumerable<ListViewColumnInfo>, IEnumerable, IStructuralEquatable
    {
        public int Add(ListViewColumnInfo value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }
            if (this.Contains(value.PropertyId))
            {
                throw new ArgumentException();
            }
            return base.List.Add(value);
        }

        public void AddRange(IEnumerable<ListViewColumnInfo> value)
        {
            foreach (ListViewColumnInfo info in value)
            {
                this.Add(info);
            }
        }

        public bool Contains(int propertyId)
        {
            return (this.IndexOf(propertyId) >= 0);
        }

        public bool Equals(object other, IEqualityComparer comparer)
        {
            ListViewColumnCollection columns = other as ListViewColumnCollection;
            if ((columns == null) || (columns.Count != base.Count))
            {
                return false;
            }
            for (int i = base.Count - 1; i >= 0; i--)
            {
                if (!columns[i].Equals(this[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public IEnumerator<ListViewColumnInfo> GetEnumerator()
        {
            return new <GetEnumerator>d__0(0) { <>4__this = this };
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(int propertyId)
        {
            for (int i = 0; i < base.Count; i++)
            {
                if (((ListViewColumnInfo) base.List[i]).PropertyId == propertyId)
                {
                    return i;
                }
            }
            return -1;
        }

        public void Remove(int propertyId)
        {
            int index = this.IndexOf(propertyId);
            if (index >= 0)
            {
                base.List.RemoveAt(index);
            }
        }

        public ListViewColumnInfo[] ToArray()
        {
            ListViewColumnInfo[] infoArray = new ListViewColumnInfo[base.Count];
            for (int i = 0; i < infoArray.Length; i++)
            {
                infoArray[i] = (ListViewColumnInfo) ((ICloneable) base.List[i]).Clone();
            }
            return infoArray;
        }

        public ListViewColumnInfo this[int index]
        {
            get
            {
                return (ListViewColumnInfo) base.List[index];
            }
        }

        [CompilerGenerated]
        private sealed class <GetEnumerator>d__0 : IEnumerator<ListViewColumnInfo>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private ListViewColumnInfo <>2__current;
            public ListViewColumnCollection <>4__this;
            public IEnumerator <>7__wrap2;
            public IDisposable <>7__wrap3;
            public ListViewColumnInfo <NextColumn>5__1;

            [DebuggerHidden]
            public <GetEnumerator>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private void <>m__Finally4()
            {
                this.<>1__state = -1;
                this.<>7__wrap3 = this.<>7__wrap2 as IDisposable;
                if (this.<>7__wrap3 != null)
                {
                    this.<>7__wrap3.Dispose();
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
                            this.<>7__wrap2 = this.<>4__this.List.GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrap2.MoveNext())
                            {
                                this.<NextColumn>5__1 = (ListViewColumnInfo) this.<>7__wrap2.Current;
                                this.<>2__current = this.<NextColumn>5__1;
                                this.<>1__state = 2;
                                return true;
                            Label_0076:
                                this.<>1__state = 1;
                            }
                            this.<>m__Finally4();
                            break;

                        case 2:
                            goto Label_0076;
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
                            this.<>m__Finally4();
                        }
                        break;
                }
            }

            ListViewColumnInfo IEnumerator<ListViewColumnInfo>.Current
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

