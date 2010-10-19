namespace Nomad.Commons.Drawing
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class IconDictionary : IconCollection, IDictionary<Size, Image>, ICollection<KeyValuePair<Size, Image>>, IEnumerable<KeyValuePair<Size, Image>>, IEnumerable
    {
        private Dictionary<uint, Image> FIcons;

        public void Add(Size key, Image value)
        {
            if (this.FIcons == null)
            {
                this.FIcons = new Dictionary<uint, Image>();
            }
            this.FIcons.Add(IconCollection.FromSize(ref key), value);
        }

        public void Clear()
        {
            this.FIcons.Clear();
        }

        public bool ContainsKey(Size key)
        {
            return ((this.FIcons != null) && this.FIcons.ContainsKey(IconCollection.FromSize(ref key)));
        }

        public IEnumerator<KeyValuePair<Size, Image>> GetEnumerator()
        {
            return new <GetEnumerator>d__0(0) { <>4__this = this };
        }

        public bool Remove(Size key)
        {
            return ((this.FIcons != null) && this.FIcons.Remove(IconCollection.FromSize(ref key)));
        }

        public bool TryGetValue(Size key, out Image value)
        {
            if (this.FIcons == null)
            {
                value = null;
                return false;
            }
            return this.FIcons.TryGetValue(IconCollection.FromSize(ref key), out value);
        }

        public int Count
        {
            get
            {
                return ((this.FIcons == null) ? 0 : this.FIcons.Count);
            }
        }

        public Image this[Size key]
        {
            get
            {
                if (this.FIcons == null)
                {
                    throw new KeyNotFoundException();
                }
                return this.FIcons[IconCollection.FromSize(ref key)];
            }
            set
            {
                if (this.FIcons == null)
                {
                    this.FIcons = new Dictionary<uint, Image>();
                }
                this.FIcons[IconCollection.FromSize(ref key)] = value;
            }
        }

        public ICollection<Size> Keys
        {
            get
            {
                if (this.FIcons == null)
                {
                    return new List<Size>(0);
                }
                ICollection<Size> is2 = new List<Size>(this.FIcons.Count);
                foreach (int num in this.FIcons.Keys)
                {
                    is2.Add(new Size(num, num));
                }
                return is2;
            }
        }

        public ICollection<Image> Values
        {
            get
            {
                return ((this.FIcons == null) ? ((ICollection<Image>) new List<Image>(0)) : ((ICollection<Image>) this.FIcons.Values));
            }
        }

        [CompilerGenerated]
        private sealed class <GetEnumerator>d__0 : IEnumerator<KeyValuePair<Size, Image>>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private KeyValuePair<Size, Image> <>2__current;
            public IconDictionary <>4__this;
            public Dictionary<uint, Image>.Enumerator <>7__wrap2;
            public KeyValuePair<uint, Image> <NextIcon>5__1;

            [DebuggerHidden]
            public <GetEnumerator>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private void <>m__Finally3()
            {
                this.<>1__state = -1;
                this.<>7__wrap2.Dispose();
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>7__wrap2 = this.<>4__this.FIcons.GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrap2.MoveNext())
                            {
                                this.<NextIcon>5__1 = this.<>7__wrap2.Current;
                                this.<>2__current = new KeyValuePair<Size, Image>(IconCollection.ToSize(this.<NextIcon>5__1.Key), this.<NextIcon>5__1.Value);
                                this.<>1__state = 2;
                                return true;
                            Label_0094:
                                this.<>1__state = 1;
                            }
                            this.<>m__Finally3();
                            break;

                        case 2:
                            goto Label_0094;
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
                            this.<>m__Finally3();
                        }
                        break;
                }
            }

            KeyValuePair<Size, Image> IEnumerator<KeyValuePair<Size, Image>>.Current
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

